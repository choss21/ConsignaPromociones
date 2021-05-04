using ConsignaJDCX.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsignaJDCX.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAll();
        Task<T> GetById(Guid id);
        Task Add(T entity);
        Task Update(Guid id, T entity);
        Task Delete(Guid id);
    }
}
