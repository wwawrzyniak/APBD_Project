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


namespace AdvertApi.Service
{
    public class ClientDbService : ControllerBase, IClientDbService
    {

        private readonly AdvertisingDbContext context;

        private IConfiguration configuration { get; set; }
        public ClientDbService(IConfiguration configuration, AdvertisingDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        public List<Client> returnAll() { return context.Clients.ToList(); }
        public IActionResult RegisterNewUser(RegisterRequest registerRequest)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(registerRequest.FirstName) || string.IsNullOrWhiteSpace(registerRequest.LastName) || string.IsNullOrWhiteSpace(registerRequest.Email) || string.IsNullOrWhiteSpace(registerRequest.Phone) || string.IsNullOrWhiteSpace(registerRequest.Login) || string.IsNullOrWhiteSpace(registerRequest.Password))
                {
                    return NotFound("Not enough data");
                }

                var isLoginUnique = context.Clients.Where(c => c.Login == registerRequest.Login).ToList();

                if(isLoginUnique.Count != 0) return BadRequest("Login already taken");

                Client client = new Client
                {
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    Phone = registerRequest.Phone,
                    Login = registerRequest.Login,
                    Password = secureMyPassword(registerRequest.Password)
                };

                context.Clients.Add(client);
                context.SaveChanges();


                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, client.IdClient.ToString()),
                    new Claim(ClaimTypes.Name, registerRequest.Login),
                    new Claim(ClaimTypes.Role, "registered"),
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

                context.Update(client);
                context.SaveChanges();

                return StatusCode(201, new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken
                });

            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        public string secureMyPassword(string password)
        {
            try
            {
                var salt = "b3w5ucH/iQuAmwYUBnmXeQ==";
                var valueBytes = KeyDerivation.Pbkdf2(password: password,
                              salt: Encoding.UTF8.GetBytes(salt),
                              prf: KeyDerivationPrf.HMACSHA512,
                              iterationCount: 10000,
                              numBytesRequested: 256 / 8);

                var newPass = Convert.ToBase64String(valueBytes);

                return newPass;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public IActionResult RefreshToken(string tokenToRefresh)
        {

            var client = context.Clients
                     .Where(c => c.RefreshToken == tokenToRefresh)
                     .ToList();


            if(client.Count == 0 || client.Count > 1)
            {
                return NotFound("Client with this refresh token doesnt exist or more than one client has this refresh token");
            }

            Client client2 = client.First();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, client2.IdClient.ToString()),
                new Claim(ClaimTypes.Name, client2.Login),
                new Claim(ClaimTypes.Role, "registered"),
                new Claim(ClaimTypes.Role,"loggedUser")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Nisia",
                audience: "Users",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials

            );

            Guid refreshToken = Guid.NewGuid();

            client2.RefreshToken = refreshToken.ToString();

            context.Update(client2);
            context.SaveChanges();

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
                var exists = context.Clients.Where(c => c.Login == loginRequest.Login).ToList();

                if(exists.Count == 0) return StatusCode(401);

                //decode the string password given by user to check against the secure password stored in database
                var pass = loginRequest.Password;

                var salt = "b3w5ucH/iQuAmwYUBnmXeQ==";
                byte[] randomBytes = new byte[128 / 8];

                var valueBytes = KeyDerivation.Pbkdf2(password: pass,
                   salt: Encoding.UTF8.GetBytes(salt),
                   prf: KeyDerivationPrf.HMACSHA512,
                   iterationCount: 10000,
                   numBytesRequested: 256 / 8);

                //password is now as hashed in database - now we can verify login
                var newPass = Convert.ToBase64String(valueBytes);

                var client = context.Clients.Where(c => c.Password == newPass.ToString()).ToList();

                //verify if user with this password exists
                if(client.Count == 0) return StatusCode(401);

                var result = context.Clients.Where(c =>
                c.Login == loginRequest.Login && c.Password == newPass
                );

                if(string.IsNullOrEmpty(result.ToString()))
                {
                    return StatusCode(401);
                }

                var clientId = context.Clients.Where(c => c.Login == loginRequest.Login).Select(c => c.IdClient);
                var claims = new[] //TODO: those values must be from db
                {
                    new Claim(ClaimTypes.NameIdentifier, clientId.ToString()),
                    new Claim(ClaimTypes.Name, loginRequest.Login),
                    new Claim(ClaimTypes.Role, "registered"),
                    new Claim(ClaimTypes.Role,"loggedUser"),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: "Nisia",
                    audience: "Users",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: credentials

                );

                Guid refreshToken = Guid.NewGuid();

                var client2 = context.Clients.Where(c => c.Login == loginRequest.Login).ToList().First();

                client2.RefreshToken = refreshToken.ToString();

                context.Update(client2);
                context.SaveChanges();

                return Ok
                (
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        refreshToken
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

    
