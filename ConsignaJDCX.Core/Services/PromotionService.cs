using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsignaJDCX.Core.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IMongoContext _context;
        public PromotionService(IMongoContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Promotion>> GetPromotions()
        {
            throw new NotImplementedException();
        }
    }
}
