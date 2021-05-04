using ConsignaJDCX.Core.DTOS;
using System;

namespace ConsignaJDCX.Core.Messages
{
    public class SaveRequest<TEntity> where TEntity :class, new()
    {
        public Guid? EntityId { get; set; }
        public TEntity Entity { get; set; }
    }
}
