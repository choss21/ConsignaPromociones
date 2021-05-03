using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
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

        public async Task<Promotion> CreatePromotion(Promotion promotion)
        {
            await _repository.Add(promotion);
            return promotion;
        }
    }
}
