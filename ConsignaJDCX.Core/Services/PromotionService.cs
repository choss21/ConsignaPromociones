using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Exceptions;
using ConsignaJDCX.Core.Interfaces;
using ConsignaJDCX.Core.Rules;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsignaJDCX.Core.Services
{
    public class PromotionService : IPromotionService
    {

        private readonly IPromotionRepository _repository;
        public PromotionService(IPromotionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Promotion>> GetPromotions()
        {
            return await _repository.GetAll();
        }
        public async Task<Promotion> GetPromotion(Guid id)
        {
            return await _repository.GetById(id);
        }
        public async Task<List<Promotion>> GetAvailablePromotions(DateTime date)
        {
            return await _repository.GetAvailablePromotions(date);
        }
        public async Task<Promotion> CreatePromotion(Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));
            fillDataEnumerables(promotion);
            BusinesValidators.ThrowIfNotValid(promotion, BusinesValidators.rulesPromotions);
            //se valida el solapamiento
            var promotionsOverlap = await getPromotionsOverlap(promotion);
            if (promotionsOverlap != null && promotionsOverlap.Any())
            {
                throw new ServiceException("La promocion que se quiere guardar genera un solapamiento con las siguientes promociones " +
                    string.Join(", ", promotionsOverlap.Select(x => x.Id)));
            }
            await _repository.Add(promotion);
            return promotion;
        }
        private async Task<List<Promotion>> getPromotionsOverlap(Promotion promotion, Guid? currentId = null)
        {
            var promotionsOverlap = await _repository.GetAllOverlap(promotion.FechaInicio, promotion.FechaFin);
            var result = new List<Promotion>();
            //No se deben solapar promociones para al menos un medio de pago, banco o categoría
            foreach (var promOverlap in promotionsOverlap)
            {
                if (currentId != null && promOverlap.Id == currentId) continue;
                if (promotion.MediosDePago.Any(x => promOverlap.MediosDePago.Contains(x)) &&
                    promotion.Bancos.Any(x => promOverlap.Bancos.Contains(x)) &&
                    promotion.CategoriasProductos.Any(x => promOverlap.CategoriasProductos.Contains(x)))
                {
                    result.Add(promOverlap);
                }
            }
            return result;
        }
        private void fillDataEnumerables(Promotion promotion)
        {
            #region TODO revisar esta funcionalidad ya que si cuando se pase un array vacio hay que autollenarlo como esta en el ejemplo del PDF
            if (promotion.CategoriasProductos != null && promotion.CategoriasProductos.Count() <= 0)
                promotion.CategoriasProductos = Promotion.CategoriaProductosDisponibles;

            if (promotion.MediosDePago != null && promotion.MediosDePago.Count() <= 0)
                promotion.MediosDePago = promotion.MediosDePago;

            if (promotion.Bancos != null && promotion.Bancos.Count() <= 0)
            {
                if (promotion.MediosDePago.Any(x => Promotion.MedioPagoRequiredBanco(x)))
                    promotion.Bancos = Promotion.BancosDisponibles;
            }
            #endregion
        }
        public async Task<Promotion> UpdateValidatePromotion(Guid id, DateTime? startDate, DateTime? endDate)
        {
            var promotionInBD = await _repository.GetById(id);
            if (promotionInBD == null)
                throw new ServiceException("No se encuentra promocion con id " + id);
            promotionInBD.FechaInicio = startDate;
            promotionInBD.FechaFin = endDate;
            BusinesValidators.ThrowIfNotValid(promotionInBD, BusinesValidators.rulesPromotions);

            //se valida el solapamiento
            var promotionsOverlap = await getPromotionsOverlap(promotionInBD, id);
            if (promotionsOverlap != null && promotionsOverlap.Any())
            {
                throw new ServiceException("La promocion que se quiere guardar genera un solapamiento con las siguientes promociones " +
                    string.Join(", ", promotionsOverlap.Select(x => x.Id)));
            }
            await _repository.Update(id, promotionInBD);
            return promotionInBD;
        }
        public async Task<Promotion> UpdatePromotion(Guid id, Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));

            var promotionInBD = await _repository.GetById(id);
            if (promotionInBD == null)
                throw new ServiceException("No se encuentra promocion con id " + id);
            if (promotion.Id != null) promotionInBD.Id = promotion.Id;
            if (promotion.FechaInicio != null) promotionInBD.FechaInicio = promotion.FechaInicio;
            if (promotion.FechaFin != null) promotionInBD.FechaFin = promotion.FechaFin;
            if (promotion.CategoriasProductos != null) promotionInBD.CategoriasProductos = promotion.CategoriasProductos;
            if (promotion.Bancos != null) promotionInBD.Bancos = promotion.Bancos;
            if (promotion.MediosDePago != null) promotionInBD.MediosDePago = promotion.MediosDePago;
            if (promotion.Activo != null) promotionInBD.Activo = promotion.Activo;

            promotionInBD.MaximaCantidadDeCuotas = promotion.MaximaCantidadDeCuotas;
            promotionInBD.ValorInteresCuotas = promotion.ValorInteresCuotas;
            promotionInBD.PorcentajeDeDescuento = promotion.PorcentajeDeDescuento;

            fillDataEnumerables(promotionInBD);

            BusinesValidators.ThrowIfNotValid(promotionInBD, BusinesValidators.rulesPromotions);
            //se valida el solapamiento
            var promotionsOverlap = await getPromotionsOverlap(promotionInBD, id);
            if (promotionsOverlap != null && promotionsOverlap.Any())
            {
                throw new ServiceException("La promocion que se quiere guardar genera un solapamiento con las siguientes promociones " +
                    string.Join(", ", promotionsOverlap.Select(x => x.Id)));
            }



            await _repository.Update(id, promotionInBD);
            return promotionInBD;
        }
        public async Task DeletePromotion(Guid id)
        {
            await _repository.Delete(id);
        }
    }
}
