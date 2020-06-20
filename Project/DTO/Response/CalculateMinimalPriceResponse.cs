using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTO.Response
{
    public class CalculateMinimalPriceResponse
    {
        public decimal highestArea { get; set; }

        public decimal secondHighestArea { get; set; }
    }
}
