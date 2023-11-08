using System.Data.Entity.Core.Objects;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.Sistema
{
    public class SistemaService
    {
        public bool PermiteEliminarRegistro(int registroId, string nombreTabla)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                ObjectParameter permiteEliminar = new ObjectParameter("PermiteEliminar", typeof(bool));
                var value = Context.GRspPermiteEliminarRegistro(registroId, nombreTabla, permiteEliminar);
                return (bool) permiteEliminar.Value;
            }
        }

        public bool RevisarPeriodoAbierto(int ejercicio, int mes, string tipoPoliza)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblAdministracionPeriodo.Where(m => m.Ejercicio == ejercicio && 
                                                                   m.Mes == mes && 
                                                                   m.TipoPoliza.Equals(tipoPoliza) &&
                                                                   m.Status == 1 //Abierto
                                                             ).FirstOrDefault() != null;
            }
        }

        public string GetFechaConFormato(DateTime date)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return GetFechaConFormato(date, false);
            }
        }

        public string GetFechaConFormato(DateTime date, bool mostrarHoras)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRspGetFechaConFormato(date, mostrarHoras).FirstOrDefault();
            }
        }
    }
}