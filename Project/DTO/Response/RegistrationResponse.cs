using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTO.Response
{
    public class RegistrationResponse
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
