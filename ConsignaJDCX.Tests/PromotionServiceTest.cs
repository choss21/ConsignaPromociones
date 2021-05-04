using AutoFixture;
using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Exceptions;
using ConsignaJDCX.Core.Interfaces;
using ConsignaJDCX.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ConsignaJDCX.Tests
{
    public class PromotionServiceTest
    {
        public Mock<IPromotionRepository> mockPromotionRepository = new Mock<IPromotionRepository>();
        public Fixture fixture = new Fixture();
        private readonly ITestOutputHelper _output;

        public PromotionServiceTest(ITestOutputHelper output)
        {
            _output = output;
        }
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
            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreatePromotion(null));
            Assert.True(exception.Message == "Value cannot be null. (Parameter 'promotion')");
        }
        /// <summary>
        /// Fecha fin no puede ser mayor que fecha inicio
        /// </summary>
        [Fact]
        public async void SavePromotion_ThrowsServiceException_Due_InvalidPromotion_case1()
        {
            var invalidPromotion = fixture
               .Build<Promotion>()
               .With(x => x.PorcentajeDeDescuento, 30)
               .Without(x => x.MaximaCantidadDeCuotas)
               .Without(x => x.ValorInteresCuotas)
               .With(x => x.MediosDePago, new[] { "EFECTIVO" })
               .With(x => x.CategoriasProductos, Enumerable.Empty<string>())
               .With(x => x.Bancos, Enumerable.Empty<string>())
               .With(x => x.FechaInicio, DateTime.Now.Date)
               .With(x => x.FechaFin, DateTime.Now.Date.AddDays(-5))
               .Create();

            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = await Assert.ThrowsAsync<ServiceException>(() => service.CreatePromotion(invalidPromotion));
            _output.WriteLine(exception.Message);
        }
        /// <summary>
        /// Cantidad de cuotas y porcentaje de descuento son nullables pero al menos una debe tener valor
        /// </summary>
        [Fact]
        public async void SavePromotion_ThrowsServiceException_Due_InvalidPromotion_case2()
        {
            var invalidPromotion = fixture
               .Build<Promotion>()
               .With(x => x.PorcentajeDeDescuento, 30)
               .With(x => x.MaximaCantidadDeCuotas, 10)
               .With(x => x.ValorInteresCuotas, 20)
               .With(x => x.MediosDePago, new[] { "EFECTIVO" })
               .With(x => x.CategoriasProductos, Enumerable.Empty<string>())
               .With(x => x.Bancos, Enumerable.Empty<string>())
               .With(x => x.FechaInicio, DateTime.Now.Date)
               .With(x => x.FechaFin, DateTime.Now.Date.AddDays(10))
               .Create();

            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = await Assert.ThrowsAsync<ServiceException>(() => service.CreatePromotion(invalidPromotion));
            _output.WriteLine(exception.Message);
        }
        /// <summary>
        /// Porcentaje interés cuota solo puede tener valor si cantidad de cuotas tiene valor
        /// </summary>
        [Fact]
        public async void SavePromotion_ThrowsServiceException_Due_InvalidPromotion_case3()
        {
            var invalidPromotion = fixture
               .Build<Promotion>()
               .Without(x => x.PorcentajeDeDescuento)
               .With(x => x.MaximaCantidadDeCuotas, 10)
               .Without(x => x.ValorInteresCuotas)
               .With(x => x.MediosDePago, new[] { "EFECTIVO" })
               .With(x => x.CategoriasProductos, Enumerable.Empty<string>())
               .With(x => x.Bancos, Enumerable.Empty<string>())
               .With(x => x.FechaInicio, DateTime.Now.Date)
               .With(x => x.FechaFin, DateTime.Now.Date.AddDays(10))
               .Create();

            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = await Assert.ThrowsAsync<ServiceException>(() => service.CreatePromotion(invalidPromotion));
            _output.WriteLine(exception.Message);
        }
        /// <summary>
        /// Porcentaje descuento en caso de tener valor, debe estar comprendido entre 5 y 80
        /// </summary>
        [Fact]
        public async void SavePromotion_ThrowsServiceException_Due_InvalidPromotion_case4()
        {
            var invalidPromotion = fixture
               .Build<Promotion>()
               .With(x => x.PorcentajeDeDescuento, 3)
               .Without(x => x.MaximaCantidadDeCuotas)
               .Without(x => x.ValorInteresCuotas)
               .With(x => x.MediosDePago, new[] { "EFECTIVO" })
               .With(x => x.CategoriasProductos, Enumerable.Empty<string>())
               .With(x => x.Bancos, Enumerable.Empty<string>())
               .With(x => x.FechaInicio, DateTime.Now.Date)
               .With(x => x.FechaFin, DateTime.Now.Date.AddDays(10))
               .Create();
            var invalidPromotion2 = fixture
              .Build<Promotion>()
              .With(x => x.PorcentajeDeDescuento, 85)
              .Without(x => x.MaximaCantidadDeCuotas)
              .Without(x => x.ValorInteresCuotas)
              .With(x => x.MediosDePago, new[] { "EFECTIVO" })
              .With(x => x.CategoriasProductos, Enumerable.Empty<string>())
              .With(x => x.Bancos, Enumerable.Empty<string>())
              .With(x => x.FechaInicio, DateTime.Now.Date)
              .With(x => x.FechaFin, DateTime.Now.Date.AddDays(10))
              .Create();

            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = await Assert.ThrowsAsync<ServiceException>(() => service.CreatePromotion(invalidPromotion));
            _output.WriteLine($"test1 {invalidPromotion.PorcentajeDeDescuento}  - {exception.Message}");
            exception = await Assert.ThrowsAsync<ServiceException>(() => service.CreatePromotion(invalidPromotion2));
            _output.WriteLine($"test2 {invalidPromotion2.PorcentajeDeDescuento}  - {exception.Message}");
        }
        /// <summary>
        /// se proporciona un valor diferente a los permitidos en los Enumerables de Bancos,Medios de pago, Categorias de producto
        /// </summary>
        [Fact]
        public async void SavePromotion_ThrowsServiceException_Due_InvalidPromotion_case5()
        {
            var invalidPromotion = fixture
               .Build<Promotion>()
               .With(x => x.PorcentajeDeDescuento, 10)
               .Without(x => x.MaximaCantidadDeCuotas)
               .Without(x => x.ValorInteresCuotas)
               .With(x => x.MediosDePago, new[] { "EFECTIVO", "VALES DE DESPENSA" })
               .With(x => x.CategoriasProductos, new[] { "CATEGORIA 1" })
               .With(x => x.Bancos, new[] { "Banco 1" })
               .With(x => x.FechaInicio, DateTime.Now.Date)
               .With(x => x.FechaFin, DateTime.Now.Date.AddDays(10))
               .Create();

            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = await Assert.ThrowsAsync<ServiceException>(() => service.CreatePromotion(invalidPromotion));
            _output.WriteLine(exception.Message);
        }

        /// <summary>
        /// Las promociones no se deben solapar ni duplicar
        /// </summary>
        [Fact]
        public async void SavePromotion_ThrowsServiceException_Due_InvalidPromotion_case6()
        {
            var invalidPromotion = fixture
               .Build<Promotion>()
               .Without(x => x.PorcentajeDeDescuento)
               .With(x => x.MaximaCantidadDeCuotas, 12)
               .With(x => x.ValorInteresCuotas, 10)
               .With(x => x.MediosDePago, new[] { "EFECTIVO", "TARJETA_CREDITO" })
               .With(x => x.CategoriasProductos, new[] { "Hogar" })
               .With(x => x.Bancos, new[] { "Galicia" })
               .With(x => x.FechaInicio, DateTime.Now.Date)
               .With(x => x.FechaFin, DateTime.Now.Date.AddDays(10))
               .Create();
            //_repository.GetAllOverlap(promotion.FechaInicio, promotion.FechaFin);

            var resultRepo = fixture.CreateMany<Promotion>(10)
                .ToList();
            resultRepo.ForEach(x =>
            {
                x.FechaInicio = invalidPromotion.FechaInicio.Value.AddDays(1);
                x.FechaFin = invalidPromotion.FechaFin.Value.AddDays(-1);
                x.MediosDePago = invalidPromotion.MediosDePago;
                x.CategoriasProductos = invalidPromotion.CategoriasProductos;
                x.Bancos = invalidPromotion.Bancos;
            });
            mockPromotionRepository.Setup(x => x.GetAllOverlap(invalidPromotion.FechaInicio, invalidPromotion.FechaFin))
                .Returns(Task.FromResult(resultRepo));
            var service = new PromotionService(mockPromotionRepository.Object);
            var exception = await Assert.ThrowsAsync<ServiceException>(() => service.CreatePromotion(invalidPromotion));
            _output.WriteLine(exception.Message);
        }

        /// <summary>
        /// se guardar de forma correcta una promocion
        /// </summary>
        [Fact]
        public async void SavePromotion_success()
        {
            var promotionSave = fixture
               .Build<Promotion>()
               .Without(x => x.PorcentajeDeDescuento)
               .With(x => x.MaximaCantidadDeCuotas, 12)
               .With(x => x.ValorInteresCuotas, 10)
               .With(x => x.MediosDePago, new[] { "EFECTIVO", "TARJETA_CREDITO" })
               .With(x => x.CategoriasProductos, new[] { "Hogar" })
               .With(x => x.Bancos, new[] { "Galicia" })
               .With(x => x.FechaInicio, DateTime.Now.Date)
               .With(x => x.FechaFin, DateTime.Now.Date.AddDays(10))
               .Create();

            var resultRepo = new List<Promotion>();
            mockPromotionRepository.Setup(x => x.GetAllOverlap(promotionSave.FechaInicio, promotionSave.FechaFin))
                .Returns(Task.FromResult(resultRepo));
            mockPromotionRepository.Setup(x => x.Add(promotionSave))
                .Returns(Task.FromResult(promotionSave));

            var service = new PromotionService(mockPromotionRepository.Object);
            var result = await service.CreatePromotion(promotionSave);
            Assert.NotNull(result);
            //_output.WriteLine(exception.Message);
        }
    }
}
