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

        private readonly AdvertisingDbContext _context;

        public CampaignController(ICampaignDbService service, AdvertisingDbContext context)
        {
            _service = service;
            _context = context;
        }


        [Authorize(Roles = "loggedUser")]
        [HttpGet]
        public IActionResult ListAllCampaigns()
        {
            return Ok(_service.ListCampaigns());
        }

        [Authorize(Roles = "registered, loggedUser")]
        [HttpPost("registerCampaign")]
        public IActionResult RegisterCampaign([FromBody] RegisterCampaignRequest registerCampaignRequest)
        {
            var exists = _context.Clients.Where(c => c.IdClient == registerCampaignRequest.IdClient).ToList();
            if(exists.Count == 0) return StatusCode(401);


            //check if all data has been delivered
            if(string.IsNullOrWhiteSpace(registerCampaignRequest.IdClient.ToString()) || string.IsNullOrWhiteSpace(registerCampaignRequest.PricePerSquareMeter.ToString()) || string.IsNullOrWhiteSpace(registerCampaignRequest.FromIdBuilding.ToString()) || string.IsNullOrWhiteSpace(registerCampaignRequest.ToIdBuilding.ToString()) || string.IsNullOrWhiteSpace(registerCampaignRequest.StartDate.ToString()) || string.IsNullOrWhiteSpace(registerCampaignRequest.EndDate.ToString()))
            {
                return NotFound("Not enough data");
            }

            //check if buildings are on the same street
            var street1 = _context.Buildings.Where(b => b.IdBuilding == registerCampaignRequest.FromIdBuilding).Select(b => b.Street);
            var street2 = _context.Buildings.Where(b => b.IdBuilding == registerCampaignRequest.ToIdBuilding).Select(b => b.Street);

            if(!street1.Equals(street1)) return StatusCode(400);

            return _service.RegisterCampaign(registerCampaignRequest);
        }

    }
}
