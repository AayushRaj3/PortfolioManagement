using NUnit.Framework;
using Moq;
using Castle.Core.Configuration;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Repository;
using DailyMutualFundNAVMicroservice.Provider;
using DailyMutualFundNAVMicroservice.Controllers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MutualFundNAVTest
{
    public class Tests
    {
        List<MutualFundDetails> funds = new List<MutualFundDetails>();
        readonly MutualFundNAVController mutualfundController;
        readonly MutualFundProvider mutualfundProvider;
        private readonly Mock<IMutualFundProvider> mockProvider = new Mock<IMutualFundProvider>();
        private readonly Mock<IMutualFundRepository> mockRepo = new Mock<IMutualFundRepository>();
        public Tests()
        {
            mutualfundController = new MutualFundNAVController(mockProvider.Object);
            mutualfundProvider = new MutualFundProvider(mockRepo.Object);
        }

        [SetUp]
        public void Setup()
        {
            funds = new List<MutualFundDetails>()
            {
                new MutualFundDetails{ MutualFundId=45,MutualFundName="Dummy1",MutualFundValue=145.23},
                new MutualFundDetails{ MutualFundId=65,MutualFundName="Dummy2",MutualFundValue=145.23}
            };
            mockProvider.Setup(x => x.GetMutualFundByNamePro(It.IsAny<string>())).Returns((string s) => funds.FirstOrDefault(
                x => x.MutualFundName.Equals(s)));

            mockRepo.Setup(x => x.GetMutualFundByNameRepo(It.IsAny<string>())).Returns((string s) => funds.FirstOrDefault(
                x => x.MutualFundName.Equals(s)));
        }

        [Test]
        public void GetMutualFundDetailsByNameController_PassCase()
        {
            var fund = mutualfundController.GetMutualFundDetailsByName("Dummy1");
            ObjectResult result = fund as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetMutualFundDetailsByNameController_FailCase()
        {
            var fund = mutualfundController.GetMutualFundDetailsByName("ABC");
            ObjectResult result = fund as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetMutualFundByNameProvider_PassCase()
        {
            var fund = mutualfundProvider.GetMutualFundByNamePro("Dummy1");
            Assert.IsNotNull(fund);
        }
        
        [Test]
        public void GetMutualFundByNameProvider_FailCase()
        {
            var fund = mutualfundProvider.GetMutualFundByNamePro("ABC");
            Assert.IsNull(fund);
        }
    }
}