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
    public interface ICampaignDbService
    {
        IActionResult ListCampaigns(AdvertisingDbContext context);


        IActionResult RegisterCampaign(AdvertisingDbContext context, RegisterCampaignRequest registerCampaignRequest);
    }
}
