using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AdvertApi.DTO.Request;
using AdvertApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Service
{
    public interface IClientDbService
    {
        IActionResult RegisterNewUser(RegisterRequest registerRequest);

        IActionResult RefreshToken (RefreshTokenRequest refreshTokenRequest);

        IActionResult LoginUser (LoginUserRequest loginRequest);

        List<Client> returnAll();

        void deleteAll();

    }
}
