using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AdvertApi.DTO.Request;
using AdvertApi.DTO.Response;
using AdvertApi.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace AdvertApi.Service
{
    public class ClientDbService : ControllerBase, IClientDbService
    {

        private readonly AdvertisingDbContext _context;

        private IConfiguration configuration { get; set; }
        public ClientDbService(IConfiguration configuration, AdvertisingDbContext context)
        {
            this.configuration = configuration;
            _context = context;
        }
        public List<Client> returnAll() { return _context.Clients.ToList(); }

        public void deleteAll() {
            foreach(Client c in _context.Clients)
                _context.Clients.Remove(c);

            _context.SaveChanges();
        }

        public string generateRandomSalt() {
            var salt = "";
            byte[] randomBytes = new byte[128 / 8];
            using(var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                salt = Convert.ToBase64String(randomBytes);
            }
            return salt;
        }

        public string hashPassword(string pass, string salt)
        {

            byte[] randomBytes = new byte[128 / 8];

            var valueBytes = KeyDerivation.Pbkdf2(password: pass,
               salt: Encoding.UTF8.GetBytes(salt),
               prf: KeyDerivationPrf.HMACSHA512,
               iterationCount: 10000,
               numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public SecureMyPasswordResponse secureMyPassword(string password)
        {
            try
            {
                var saltReturn = generateRandomSalt();
                var newPass = hashPassword(password, saltReturn);

                return new SecureMyPasswordResponse { Password = newPass, Salt = saltReturn };
            }
            catch(Exception ex)
            {
                return new SecureMyPasswordResponse { };
            }
        }

        public TokenCreationResponse createTokens(Client client, string clientsRole)
        {
            var claims = new[]
                   {
                    new Claim(ClaimTypes.NameIdentifier, client.IdClient.ToString()),
                    new Claim(ClaimTypes.Name, client.Login),
                    new Claim(ClaimTypes.Role, clientsRole),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Nisia",
                audience: "Users",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            Guid refreshToken = Guid.NewGuid();

            client.RefreshToken = refreshToken.ToString();

            return new TokenCreationResponse { Token = token, RefreshToken = refreshToken };
        }

        public IActionResult RegisterNewUser(RegisterRequest registerRequest)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(registerRequest.FirstName) || string.IsNullOrWhiteSpace(registerRequest.LastName) || string.IsNullOrWhiteSpace(registerRequest.Email) || string.IsNullOrWhiteSpace(registerRequest.Phone) || string.IsNullOrWhiteSpace(registerRequest.Login) || string.IsNullOrWhiteSpace(registerRequest.Password))
                {
                    return BadRequest("Not enough data");
                }

                var isLoginUnique = _context.Clients.Where(c => c.Login == registerRequest.Login).ToList();

                if(isLoginUnique.Count != 0) return BadRequest("Login already taken");
                var securedPassword = secureMyPassword(registerRequest.Password);
                Client client = new Client
                {
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    Phone = registerRequest.Phone,
                    Login = registerRequest.Login,
                    Password = securedPassword.Password,
                    Salt = securedPassword.Salt
                };

                _context.Clients.Add(client);
                _context.SaveChanges();

                TokenCreationResponse tokenCreationResponse = createTokens(client, "registered");
                var token = tokenCreationResponse.Token;
                var refreshToken = tokenCreationResponse.RefreshToken.ToString();

                var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(token);
                _context.Update(client);
                _context.SaveChanges();

                return StatusCode(201, new RegistrationResponse
                {
                    AccessToken = tokenToReturn,
                    RefreshToken = refreshToken
                }
                );

            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        
        }

        public IActionResult RefreshToken(string tokenToRefresh)
        {

            var client = _context.Clients
                     .Where(c => c.RefreshToken == tokenToRefresh)
                     .ToList();


            if(client.Count == 0 || client.Count > 1)
            {
                return NotFound("Client with this refresh token doesnt exist or more than one client has this refresh token");
            }

            Client client2 = client.First();

            TokenCreationResponse tokenCreationResponse = createTokens(client2, "loggedUser");

            var token = tokenCreationResponse.Token;

            var refreshToken = tokenCreationResponse.RefreshToken;

            _context.Update(client2);
            _context.SaveChanges();

            return Ok
            (
                new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken
                }
            );
        }

        public IActionResult LoginUser(LoginUserRequest loginRequest)
        {
            try
            {
                //verify if user with this login exists
                Client thisUser = _context.Clients.Where(c => c.Login == loginRequest.Login).ToList().First();

                if(thisUser == null) return StatusCode(401);

                //decode the string password given by user to check against the secure password stored in database

                var newPass = hashPassword(loginRequest.Password, thisUser.Salt);

                //password is now as hashed in database - now we can verify login
                Client loggedUser = _context.Clients.Where(c =>
                c.Login == loginRequest.Login && c.Password == newPass
                ).ToList().First();

                if(loggedUser == null)
                {
                    return StatusCode(401);
                }
                TokenCreationResponse tokenCreationResponse = createTokens(loggedUser, "loggedUser");
                var token = tokenCreationResponse.Token;
                var refreshToken = tokenCreationResponse.RefreshToken;

                _context.Update(loggedUser);
                _context.SaveChanges();

                return Ok
                (
                    new RegistrationResponse
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken.ToString()
                    }
                );

            }
            catch(Exception ex)
            {
                return StatusCode(401);
            }
        }

   
    }
}

    
