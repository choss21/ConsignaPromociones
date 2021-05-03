using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Exceptions;
using System.Linq;

namespace ConsignaJDCX.Core.Rules
{
    public class BusinesValidators
    {

        public static readonly ValidationItem<Promotion>[] rulesPromotions =
        {
            new ValidationItem<Promotion>((x)=>x.FechaInicio != null,"La fecha de inicio es requerida"),
            new ValidationItem<Promotion>((x)=>x.FechaFin != null,"La fecha fin de promocion es requerida"),
            new ValidationItem<Promotion>((x)=> x.FechaInicio != null && x.FechaFin != null && x.FechaFin >= x.FechaInicio,"La fecha Fin tiene que ser mayor o igual a la fecha de inicio"),
            //La promoción puede tener porcentaje de descuento o cuotas. NO ambas
            new ValidationItem<Promotion>((x)=>{
                if(x.PorcentajeDeDescuento== null)
                {//se espera que se este usando cuotas
                    if(x.MaximaCantidadDeCuotas != null && x.ValorInteresCuotas != null)
                        return true;//es valido
                    else
                        return false;//es invalido
                }
                else
                {
                    //solo se debe usar Porcentaje de descuento
                    if(x.MaximaCantidadDeCuotas == null && x.ValorInteresCuotas == null)
                        return true;//es valido
                    else
                        return false;//es invalido
                }
            },"La promoción puede tener porcentaje de descuento o cuotas. NO ambas o ninguna"),
            new ValidationItem<Promotion>((x)=>{
                if(x.MediosDePago != null && x.MediosDePago.Count() > 0)
                {
                    foreach(var mp in x.MediosDePago)
                    {
                        if (!Promotion.MediosDePagoDisponibles.Contains(mp))
                        {
                            return false;
                        }
                    }
                }
                return true;
            },"Solo se permiten los siguientes Medios de Pago: " + string.Join(", " , Promotion.MediosDePagoDisponibles)),
            new ValidationItem<Promotion>((x)=>{
                if(x.Bancos != null && x.Bancos.Count() > 0)
                {
                    foreach(var mp in x.Bancos)
                    {
                        if (!Promotion.BancosDisponibles.Contains(mp))
                        {
                            return false;
                        }
                    }
                }
                return true;
            },"Solo se permiten los siguientes Bancos: " + string.Join(", " , Promotion.BancosDisponibles)),
            new ValidationItem<Promotion>((x)=>{
                if(x.CategoriasProductos != null && x.CategoriasProductos.Count() > 0)
                {
                    foreach(var mp in x.CategoriasProductos)
                    {
                        if (!Promotion.CategoriaProductosDisponibles.Contains(mp))
                        {
                            return false;
                        }
                    }
                }
                return true;
            },"Solo se permiten las siguientes Categorías productos: " + string.Join(", " , Promotion.CategoriaProductosDisponibles)),
            new ValidationItem<Promotion>((x)=>{
                if(x.PorcentajeDeDescuento!= null)
                {//se espera que se este usando cuotas
                    if(x.PorcentajeDeDescuento < 5 || x.PorcentajeDeDescuento > 80)
                        return false;
                }
                return true;
            },"El Porcentaje descuento en caso de tener valor, debe estar comprendido entre 5 y 80"),
        };


        public static void ThrowIfNotValid<T>(T data, params ValidationItem<T>[] validations)
        {
            var errors = validations.Where(x => !x.Predicate(data)).Select(x => x.MessageError);
            if (errors.Count() > 0)
            {
                throw new ServiceException(string.Join(", ", errors));
            }
        }

    }
}

