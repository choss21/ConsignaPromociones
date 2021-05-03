using ConsignaJDCX.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsignaJDCX.Core.Interfaces
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetPromotions();
        Task<Promotion> CreatePromotion(Promotion promotion);
    }
}
