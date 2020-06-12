
using System.Text.Json.Serialization;


namespace AdvertApi.Entities
{
    public class Banner
    {
        public int IdAdvertisement { get; set; }

        public int Name { get; set; }

       // [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public int IdCampaign { get; set; }

      //  [Column(TypeName = "decimal(6,2)")]
        public decimal Area { get; set; }

        [JsonIgnore]
        public virtual Campaign Campaign { get; set; }
    }
}
