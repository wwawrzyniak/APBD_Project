using AdvertApi.Controllers;
using AdvertApi.DTO.Request;
using AdvertApi.DTO.Response;
using AdvertApi.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;


namespace TestProject
{
    [TestFixture]
    class RegisterNewUserUnitTest
    {
        [Test]
        public void TestSuccessRegisterNewUser()
        {
            var db = new Mock<IClientDbService>();
            var registerRequest = new RegisterRequest
            {
                FirstName = "Weronika",
                LastName = "Wawrzyniak",
                Email = "ww@wp.pl",
                Phone = "790614208",
                Login = "wwawrzyniak",
                Password = "werkajestsuper"
            };

            db.Setup(d => d.RegisterNewUser(registerRequest)).Returns(new RegistrationResponse { AccessToken = "wdqwd", RefreshToken = "ssss" });

            var controller = new ClientController(db.Object);
            var result = controller.Register(registerRequest);
            var resultValue = (RegistrationResponse)((ObjectResult)result).Value;
            Assert.AreEqual("wdqwd", resultValue.AccessToken);
            Assert.AreNotEqual("kkk", resultValue.RefreshToken);
        }
    }
}
