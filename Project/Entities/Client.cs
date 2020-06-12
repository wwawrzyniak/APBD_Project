using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdvertApi.Entities
{
    public class Client
    {
        public int IdClient { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string RefreshToken { get; set; }
        [JsonIgnore]
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}
