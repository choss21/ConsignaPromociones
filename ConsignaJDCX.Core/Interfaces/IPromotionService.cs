using ConsignaJDCX.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsignaJDCX.Core.Interfaces
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetPromotions();
        Task<Promotion> GetPromotion(Guid id);
        Task<List<Promotion>> GetAvailablePromotions(DateTime date);
        Task<List<Promotion>> GetAvailablePromotionsbySale(string mediopago, string banco, string categoriaproducto);
        Task<Promotion> CreatePromotion(Promotion promotion);
        Task<Promotion> UpdatePromotion(Guid id, Promotion promotion);
        Task<Promotion> UpdateValidatePromotion(Guid id, DateTime? startDate, DateTime? endDate);
        Task DeletePromotion(Guid id);
    }
}
