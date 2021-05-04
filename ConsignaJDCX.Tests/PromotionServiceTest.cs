using AutoFixture;
using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
using ConsignaJDCX.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConsignaJDCX.Tests
{
    public class PromotionServiceTest
    {
        public Mock<IPromotionRepository> mockPromotionRepository = new Mock<IPromotionRepository>();
        public Fixture fixture = new Fixture();
        [Fact]
        public async void GetAllPromotions()
        {
            var resultRepo = fixture.CreateMany<Promotion>(10).ToList();
            mockPromotionRepository.Setup(x => x.GetAll()).Returns(Task.FromResult(resultRepo));
            var service = new PromotionService(mockPromotionRepository.Object);
            var resultado = await service.GetPromotions();
            Assert.True(resultado.Count() == 10);
        }
        [Fact]
        public async void GetPromotionById()
        {
            var promotionInBD = fixture.Create<Promotion>();
            mockPromotionRepository.Setup(x => x.GetById(promotionInBD.Id.Value)).Returns(Task.FromResult(promotionInBD));
            var service = new PromotionService(mockPromotionRepository.Object);
            var resultado = await service.GetPromotion(promotionInBD.Id.Value);
            Assert.NotNull(resultado);
            Assert.True(resultado.Id == promotionInBD.Id);
        }
        [Fact]
        public async void GetPromotionById_NotFound_Promotion()
        {

            mockPromotionRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult<Promotion>(null));
            var service = new PromotionService(mockPromotionRepository.Object);
            var resultado = await service.GetPromotion(Guid.NewGuid());
            Assert.Null(resultado);
        }
        [Fact]
        public async void SavePromotion_ThrowsArgumentNullException_DueToNullPromotion()
        {
            //mockPromotionRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult<Promotion>(null));
            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => service.CreatePromotion(null));
            Assert.True(exception.Result.Message == "Value cannot be null. (Parameter 'promotion')");
        }

    }
}
