using ConsignaJDCX.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsignaJDCX.Core.Interfaces
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<List<Promotion>> GetAllOverlap(DateTime? fechaInicio, DateTime? fechaFin);
        Task<List<Promotion>> GetAvailablePromotions(DateTime date);
    }
}
