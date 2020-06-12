using Microsoft.EntityFrameworkCore;
using AdvertApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.EntityFrameworkContext
{

    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            var mock2 = new List<Building>();
            mock2.Add(new Building { IdBuilding = 1, Street = "pretty", StreetNumber = 1, City = "Warsaw", Height = 16 });
            mock2.Add(new Building { IdBuilding = 2, Street = "pretty", StreetNumber = 2, City = "Warsaw", Height = 10 });
            mock2.Add(new Building { IdBuilding = 3, Street = "pretty", StreetNumber = 3, City = "Warsaw", Height = 7 });
            mock2.Add(new Building { IdBuilding = 4, Street = "pretty", StreetNumber = 4, City = "Warsaw", Height = 12 });
            mock2.Add(new Building { IdBuilding = 5, Street = "nice", StreetNumber = 10, City = "Warsaw", Height = 20 });
            mock2.Add(new Building { IdBuilding = 6, Street = "nice", StreetNumber = 12, City = "Warsaw", Height = 30 });
            mock2.Add(new Building { IdBuilding = 7, Street = "nice", StreetNumber = 14, City = "Warsaw", Height = 2 });

            modelBuilder.Entity<Building>().HasData(mock2);


        }
    }
}