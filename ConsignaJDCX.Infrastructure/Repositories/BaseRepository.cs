using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Exceptions;
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
            var result = await _entities.Find(s => s.Activo == true).ToListAsync();
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

        public async Task Update(Guid id, T entity)
        {
            if (entity is BaseEntity be)
            {
                be.FechaModificacion = DateTime.Now;
                if (be.Activo == null)
                    be.Activo = true;
                if (be.Id == null)
                    be.Id = id;
            }
            await _entities.ReplaceOneAsync(su => su.Id == id, entity);
        }

        public async Task Delete(Guid id)
        {
            var entitySearch = await GetById(id);
            if (entitySearch == null)
                throw new ServiceException("No se encontrol Promocion para eliminar");
            if (entitySearch != null && entitySearch is BaseEntity be)
            {
                if (be.Activo == false)
                    throw new ServiceException("La promocion proporcionada ya se encuentra eliminada");
                be.Activo = false;
                await Update(id, be as T);
            }
            else
            {
                await _entities.DeleteOneAsync(su => su.Id == id);
            }

        }
    }
}
