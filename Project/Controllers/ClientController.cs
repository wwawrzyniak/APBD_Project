using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdvertApi.DTO.Request;
using AdvertApi.Service;


namespace AdvertApi.Controllers
{

    [ApiController]
    [Route ("api/clients")]
    public class ClientController : ControllerBase
    {

        private readonly IClientDbService _service;

        public ClientController(IClientDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult getAll()
        {
            
            return Ok(_service.returnAll());

        }
        [HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            return _service.RegisterNewUser(request);

        }

        [AllowAnonymous]
        [HttpPost ("refreshToken")]
        public IActionResult RefreshToken ([FromBody] RefreshTokenRequest refreshTokenRequest)
        {

            return _service.RefreshToken (refreshTokenRequest);
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginUserRequest request)
        {
            return _service.LoginUser(request);
        }

    }
}
