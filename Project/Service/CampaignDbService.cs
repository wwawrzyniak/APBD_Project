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

        public IActionResult ListCampaigns(AdvertisingDbContext context)
        {
            try
            {
   
                 var res = context.Campaigns
                      .Include(camp => camp.Banners)
                      .Include(camp => camp.Client)
                      .ToList()
                      .OrderByDescending(camp => camp.StartDate);

                return Ok(res);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        public IActionResult RegisterCampaign(AdvertisingDbContext context, RegisterCampaignRequest registerCampaignRequest)
        {
         using(var transaction = context.Database.BeginTransaction())
          {
                try
                {
                    //calculate the cost of the ad
                    Building fromBuilding = context.Buildings.Where(b => b.IdBuilding == registerCampaignRequest.FromIdBuilding).ToList().First();
                    Building toBuilding = context.Buildings.Where(b => b.IdBuilding == registerCampaignRequest.ToIdBuilding).ToList().First();

                    List<Building> allCampaignBuildings = context.Buildings.Where(b => b.StreetNumber >= fromBuilding.StreetNumber && b.StreetNumber <= toBuilding.StreetNumber).OrderByDescending(x => x.StreetNumber).ToList();

                    //Log lin for now
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


                    decimal banner1Price = highestArea * registerCampaignRequest.PricePerSquareMeter;

                    decimal banner2Price = secondHighestArea * registerCampaignRequest.PricePerSquareMeter;


                    Campaign newCampaign = new Campaign
                    {
                        IdClient = registerCampaignRequest.IdClient,
                        StartDate = registerCampaignRequest.StartDate,
                        EndDate = registerCampaignRequest.EndDate,
                        PricePerSquareMeter = registerCampaignRequest.PricePerSquareMeter,
                        FromIdBuilding = registerCampaignRequest.FromIdBuilding,
                        ToIdBuilding = registerCampaignRequest.ToIdBuilding

                    };

                    context.Campaigns.Add(newCampaign);
                    context.SaveChanges();


                    Banner b1 = new Banner
                    {
                        Name = 1,
                        Price = banner1Price,
                        IdCampaign = newCampaign.IdCampaign,
                        Area = highestArea
                    };

                    Banner b2 = new Banner
                    {
                        Name = 2,
                        Price = banner2Price,
                        IdCampaign = newCampaign.IdCampaign,
                        Area = secondHighestArea
                    };

                    context.Banners.Add(b1);
                    context.Banners.Add(b2);
                    context.SaveChanges();

                    context.Campaigns.ToList();
                    return StatusCode(201, new RegisterCampaignResponse
                    {
                        IdCampaign = newCampaign.IdCampaign,
                        IdClient = newCampaign.IdClient,
                        StartDate = newCampaign.StartDate,
                        EndDate = newCampaign.EndDate,
                        PricePerSquareMeter = newCampaign.PricePerSquareMeter,
                        FromIdBuilding = newCampaign.FromIdBuilding,
                        ToIdBuilding = newCampaign.ToIdBuilding,
                        Banners=newCampaign.Banners
                    });
                }
                catch(Exception x)
                {
                    transaction.Rollback();
                    return BadRequest(x);
                };
           }
            }
        }
    }

