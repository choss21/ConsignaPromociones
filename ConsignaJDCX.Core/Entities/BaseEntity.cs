using System;

namespace ConsignaJDCX.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid? Id { get; set; }
        public DateTime FechaCreacion { get;  set; } 
        public DateTime? FechaModificacion { get;  set; }
        public bool? Activo { get; set; }
    }
}
