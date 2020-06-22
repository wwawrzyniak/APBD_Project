using AdvertApi.Controllers;
using AdvertApi.DTO.Response;
using AdvertApi.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestProject
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestSuccessListAllCampaigns()
        {
            var db = new Mock<ICampaignDbService>();
            db.Setup(d => d.ListCampaigns()).Returns(new List<ListCampaignsResponse> { new ListCampaignsResponse {
             CampaignId =50 ,
             CustomerFirstName ="Weronika",

             CustomerLastName ="Wawrzyniak",

             CustomerPhone ="790614208",

             StartCampaign = DateTime.Now,

             EndCampaign = DateTime.Today,

             CampaignCity ="Warsaw",

             Banner1Name =1,

             Banner2Name =2

              } });
            CampaignController controller = new CampaignController(db.Object);

            var result = (OkObjectResult)controller.ListAllCampaigns();

            List<ListCampaignsResponse> listResult = result.Value as List<ListCampaignsResponse>;

            Assert.NotNull(listResult);
            Assert.AreEqual(listResult.Count, 1);
            Assert.AreEqual(listResult[0].EndCampaign, DateTime.Today);
        }
    }
}