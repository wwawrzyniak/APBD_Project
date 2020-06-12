using AdvertApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdvertApi.Entities
{
    public class Campaign
    {
        public int IdCampaign { get; set; }

        public int IdClient { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

       // [Column(TypeName = "decimal(6,2)")]
        public decimal PricePerSquareMeter { get; set; }

        public int FromIdBuilding { get; set; }

        public int ToIdBuilding { get; set; }
        [JsonIgnore]
        public virtual ICollection<Banner> Banners { get; set; }
        [JsonIgnore]
        public virtual Client Client { get; set; }
        [JsonIgnore]
        public virtual Building Building1 { get; set; }
        [JsonIgnore]
        public virtual Building Building2 { get; set; }
    }

}
