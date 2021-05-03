using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;

namespace ConsignaJDCX.Infrastructure.Repositories
{
    public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(IMongoContext context) : base(context)
        {

        }
    }
}
