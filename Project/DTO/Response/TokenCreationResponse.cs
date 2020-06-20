using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTO.Response
{
    public class TokenCreationResponse
    {
        public JwtSecurityToken Token { get; set; }

        public Guid RefreshToken { get; set; }
    }
}
