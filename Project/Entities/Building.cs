
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace AdvertApi.Entities
{
    public class Building
    {

        public int IdBuilding { get; set; }

        public string Street { get; set; }

        public int StreetNumber { get; set; }

        public string City { get; set; }

        //[Column(TypeName = "decimal(6,2)")]
        public decimal Height { get; set; }
      //  [JsonIgnore]
      //  public virtual ICollection<Campaign> Campaigns { get; set; }
        [JsonIgnore]
        public virtual ICollection<Campaign> FromIdBuildingCampaigns { get; set; }
        [JsonIgnore]
        public virtual ICollection<Campaign> ToIdBuildingCampaigns { get; set; }
    }
}
