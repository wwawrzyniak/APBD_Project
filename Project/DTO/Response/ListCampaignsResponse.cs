using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTO.Response
{
    public class ListCampaignsResponse
    {
        public int CampaignId { get; set; }
        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerPhone { get; set; }

        public DateTime StartCampaign {get; set;}

        public DateTime EndCampaign { get; set; }

        public string CampaignCity { get; set; }

        public int Banner1Name { get; set; }

        public int Banner2Name { get; set; }


    }
}
