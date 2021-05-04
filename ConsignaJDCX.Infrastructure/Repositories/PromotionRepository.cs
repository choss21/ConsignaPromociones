using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsignaJDCX.Infrastructure.Repositories
{
    public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(IMongoContext context) : base(context)
        {

        }

        public async Task<List<Promotion>> GetAllOverlap(DateTime? fecha1, DateTime? fecha2)
        {
            return await _entities.Find(x => x.Activo == true
            && (
                    (x.FechaInicio <= fecha1 && x.FechaFin >= fecha1) ||
                    (x.FechaInicio <= fecha2 && x.FechaFin >= fecha2)
            )).ToListAsync();
        }
        public async Task<List<Promotion>> GetAvailablePromotions(DateTime date)
        {
            return await _entities.Find(x => x.Activo == true
                            &&
                            x.FechaInicio <= date && x.FechaFin >= date
                    ).ToListAsync();
        }
    }
}
