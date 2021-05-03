using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsignaJDCX.Core.DTOS
{
    public abstract class BaseEntityDTO
    {
        public Guid? Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Activo { get; set; }
    }
}
