using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTO.Request
{
    public class RegisterCampaignRequest
    {

        public int IdClient { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal PricePerSquareMeter { get; set; }

        public int FromIdBuilding { get; set; }

        public int ToIdBuilding { get; set; }
    }
}
