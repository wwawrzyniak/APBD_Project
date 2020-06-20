using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvertApi.DTO.Request;
using AdvertApi.DTO.Response;
using AdvertApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AdvertApi.Service
{
    public class CampaignDbService : ControllerBase, ICampaignDbService
    {

        private readonly AdvertisingDbContext _context;

        public CampaignDbService(AdvertisingDbContext context)
        {
            _context = context;
        }

        public IActionResult ListCampaigns()
        {
            List<ListCampaignsResponse> finalList = new List<ListCampaignsResponse>();
            try
            {
                var camps = _context.Campaigns.ToList();
                foreach(Campaign camp in camps)
                {
                    finalList.Add(new ListCampaignsResponse
                    {
                        CampaignId = camp.IdCampaign,
                        StartCampaign = camp.StartDate,
                        EndCampaign = camp.EndDate,
                        CampaignCity = _context.Buildings.Where(b => b.IdBuilding == camp.FromIdBuilding).Select(b => b.City).ToList().First(),
                        Banner1Name = _context.Banners.Where(ban => ban.IdCampaign == camp.IdCampaign).Select(b => b.Name).ToList().First(),
                        Banner2Name = _context.Banners.Where(ban => ban.IdCampaign == camp.IdCampaign).Select(b => b.Name).ToList().Skip(1).First(),
                        CustomerFirstName = _context.Clients.Where(c => c.IdClient == camp.IdClient).Select(c => c.FirstName).ToList().First(),
                        CustomerLastName = _context.Clients.Where(c => c.IdClient == camp.IdClient).Select(c => c.LastName).ToList().First(),
                        CustomerPhone = _context.Clients.Where(c => c.IdClient == camp.IdClient).Select(c => c.Phone).ToList().First(),
                    }); ;
                }
                finalList.OrderByDescending(camp => camp.StartCampaign);
                return Ok(finalList);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
        public CalculateMinimalPriceResponse calculateMinimalPrice(RegisterCampaignRequest registerCampaignRequest)
        {
            //calculate the cost of the ad
            Building fromBuilding = _context.Buildings.Where(b => b.IdBuilding == registerCampaignRequest.FromIdBuilding).ToList().First();
            Building toBuilding = _context.Buildings.Where(b => b.IdBuilding == registerCampaignRequest.ToIdBuilding).ToList().First();

            List<Building> allCampaignBuildings = _context.Buildings.Where(b => b.StreetNumber >= fromBuilding.StreetNumber && b.StreetNumber <= toBuilding.StreetNumber).OrderByDescending(x => x.StreetNumber).ToList();

            decimal highestArea = 0;
            decimal secondHighestArea = 0;
            var sortedByHeightBuildings = allCampaignBuildings.OrderByDescending(x => x.Height).ToList();
            Building highestBuilding = sortedByHeightBuildings[0];
            Building secondHighestBuilding = sortedByHeightBuildings[1];
            if((highestBuilding.IdBuilding == fromBuilding.IdBuilding) || highestBuilding.IdBuilding == toBuilding.IdBuilding) // case 1 , right/left most linear or log
            {
                //we go all the way left or all the way right
                highestArea = highestBuilding.Height * 1;
                secondHighestArea = secondHighestBuilding.Height * (sortedByHeightBuildings.Count - 1);
            }
            else
            {

                if(highestBuilding.StreetNumber < secondHighestBuilding.StreetNumber)
                {
                    //everything from highest to left is area1, everything else is area2
                    highestArea = (highestBuilding.StreetNumber - fromBuilding.StreetNumber) * highestBuilding.Height;
                    secondHighestArea = (highestBuilding.StreetNumber - toBuilding.StreetNumber) * secondHighestBuilding.Height;
                }
                else
                {
                    //everything from highest to right is area1, everything else is area2
                    highestArea = (highestBuilding.StreetNumber - toBuilding.StreetNumber) * highestBuilding.Height;
                    secondHighestArea = (highestBuilding.StreetNumber - fromBuilding.StreetNumber) * secondHighestBuilding.Height;
                }
            }
            return new CalculateMinimalPriceResponse
            {
                highestArea = highestArea,
                secondHighestArea = secondHighestArea
            };

        }
        public IActionResult RegisterCampaign(RegisterCampaignRequest registerCampaignRequest)
        {
            try
            {

                var result = calculateMinimalPrice(registerCampaignRequest);
                decimal banner1Price = result.highestArea * registerCampaignRequest.PricePerSquareMeter;

                decimal banner2Price = result.secondHighestArea * registerCampaignRequest.PricePerSquareMeter;


                Campaign newCampaign = new Campaign
                {
                    IdClient = registerCampaignRequest.IdClient,
                    StartDate = registerCampaignRequest.StartDate,
                    EndDate = registerCampaignRequest.EndDate,
                    PricePerSquareMeter = registerCampaignRequest.PricePerSquareMeter,
                    FromIdBuilding = registerCampaignRequest.FromIdBuilding,
                    ToIdBuilding = registerCampaignRequest.ToIdBuilding

                };

                _context.Campaigns.Add(newCampaign);

                _context.SaveChanges();

                Banner b1 = new Banner
                {
                    Name = 1,
                    Price = banner1Price,
                    Campaign = newCampaign,
                    Area = result.highestArea
                };

                Banner b2 = new Banner
                {
                    Name = 2,
                    Price = banner2Price,
                    Campaign = newCampaign,
                    Area = result.secondHighestArea
                };

                _context.Banners.Add(b1);
                _context.Banners.Add(b2);
                _context.SaveChanges();

                return StatusCode(201, new RegisterCampaignResponse
                {
                    IdCampaign = newCampaign.IdCampaign,
                    IdClient = newCampaign.IdClient,
                    StartDate = newCampaign.StartDate,
                    EndDate = newCampaign.EndDate,
                    PricePerSquareMeter = newCampaign.PricePerSquareMeter,
                    FromIdBuilding = newCampaign.FromIdBuilding,
                    ToIdBuilding = newCampaign.ToIdBuilding,
                    Banners = newCampaign.Banners
                });
            }
            catch(Exception x)
            {
                return BadRequest(x);
            }
        }
    }
}

