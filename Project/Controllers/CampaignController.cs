using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdvertApi.DTO.Request;
using AdvertApi.Entities;
using AdvertApi.Service;

using System.Linq;


namespace AdvertApi.Controllers
{
    [ApiController]
    [Route("api/campaigns")]
    public class CampaignController : ControllerBase
    {

        private readonly ICampaignDbService _service;


        public CampaignController(ICampaignDbService service)
        {
            _service = service;
        }


        [Authorize(Roles = "loggedUser")]
        [HttpGet]
        public IActionResult ListAllCampaigns()
        {
            var result = _service.ListCampaigns();
            if(result == null) return BadRequest("An error occured");
            return Ok(result);
        }

        [Authorize(Roles = "registered, loggedUser")]
        [HttpPost("registerCampaign")]
        public IActionResult RegisterCampaign([FromBody] RegisterCampaignRequest registerCampaignRequest)
        {
            return _service.RegisterCampaign(registerCampaignRequest);
        }

    }
}
