using AdvertApi.Entities;
using System;
using System.Collections.Generic;


namespace AdvertApi.DTO.Response
{
    public class RegisterCampaignResponse
    {
        public int IdCampaign { get; set; }

        public int IdClient { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal PricePerSquareMeter { get; set; }

        public int FromIdBuilding { get; set; }

        public int ToIdBuilding { get; set; }
 
        public virtual ICollection<Banner> Banners { get; set; }
    }
}
