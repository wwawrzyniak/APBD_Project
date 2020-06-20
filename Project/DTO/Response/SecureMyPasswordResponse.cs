using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTO.Response
{
    public class SecureMyPasswordResponse
    {
        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
