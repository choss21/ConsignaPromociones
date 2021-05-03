using System;
using System.Collections.Generic;

namespace ConsignaJDCX.Core.DTOS
{
    public class PromotionDTO : BaseEntityDTO
    {
        /// <summary>
        /// Medio de pago
        /// </summary>
        public IEnumerable<string> MediosDePago { get; set; }
        /// <summary>
        /// Banco
        /// </summary>
        public IEnumerable<string> Bancos { get; set; }
        /// <summary>
        /// Categoria producto
        /// </summary>
        public IEnumerable<string> CategoriasProductos { get; set; }
        /// <summary>
        /// Cantidad de cuotas
        /// </summary>
        public int? MaximaCantidadDeCuotas { get; set; }
        /// <summary>
        /// Porcentaje interés cuota
        /// </summary>
        public decimal? ValorInteresCuotas { get; set; }
        /// <summary>
        /// Porcentaje de descuento
        /// </summary>
        public decimal? PorcentajeDeDescuento { get; set; }
        /// <summary>
        /// Fecha inicio
        /// </summary>
        public DateTime? FechaInicio { get; set; }
        /// <summary>
        /// Fecha Fin
        /// </summary>
        public DateTime? FechaFin { get; set; }
    }
}
