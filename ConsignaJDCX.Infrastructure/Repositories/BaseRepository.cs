using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsignaJDCX.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMongoContext _mongoContext;
        protected readonly IMongoCollection<T> _entities;
        public BaseRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
            _entities = mongoContext.GetCollection<T>(typeof(T).Name);

        }

        public async Task<List<T>> GetAll()
        {
            var result = await _entities.Find(s => true).ToListAsync();
            return result;
        }

        public async Task<T> GetById(Guid id)
        {
            return await _entities.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(T entity)
        {
            if (entity is BaseEntity be)
            {
                be.FechaCreacion = DateTime.Now;
                be.Activo = true;
            }
            await _entities.InsertOneAsync(entity);
        }

        public async void Update(T entity)
        {
            if (entity is BaseEntity be)
            {
                be.FechaModificacion = DateTime.Now;
            }
            await _entities.ReplaceOneAsync(su => su.Id == entity.Id, entity);
        }

        public async Task Delete(Guid id)
        {
            await _entities.DeleteOneAsync(su => su.Id == id);
        }
    }
}
