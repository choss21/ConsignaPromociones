using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsignaJDCX.Core.Entities
{
    public class Promotion : BaseEntity
    {
        public IEnumerable<string> MediosDePago { get; set; }
        public IEnumerable<string> Bancos { get; set; }
        public IEnumerable<string> CategoriasProductos { get; set; }
        public int? MaximaCantidadDeCuotas { get; set; }
        public decimal? ValorInteresCuotas { get; set; }
        public decimal? PorcentajeDeDescuento { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public static IEnumerable<string> MediosDePagoDisponibles
        {
            get
            {
                yield return "TARJETA_CREDITO";
                yield return "TARJETA_DEBITO";
                yield return "EFECTIVO";
                yield return "GIFT_CARD";
            }
        }
        public static IEnumerable<string> BancosDisponibles
        {
            get
            {
                yield return "Galicia";
                yield return "Santander Rio";
                yield return "Ciudad";
                yield return "Nacion";
                yield return "ICBC";
                yield return "BBVA";
                yield return "Macro";
            }
        }
        public static IEnumerable<string> CategoriaProductosDisponibles
        {
            get
            {
                yield return "Hogar";
                yield return "Jardin";
                yield return "ElectroCocina";
                yield return "GrandesElectro";
                yield return "Colchones";
                yield return "Celulares";
                yield return "Tecnologia";
                yield return "Audio";
            }
        }

    }

   
}
