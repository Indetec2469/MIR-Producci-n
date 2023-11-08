using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Data.Entity.Core.Objects;

namespace RequisicionesAlmacenBL.Services
{
    public class AutonumericoService : BaseService<GRtblAutonumerico>
    {
        public override bool Actualiza(GRtblAutonumerico entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblAutonumerico BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblAutonumerico Inserta(GRtblAutonumerico entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public string GetSiguienteAutonumerico(string nombreAutonumerico, SAACGContext context)
        {
            return GetSiguienteAutonumerico(nombreAutonumerico, null, context);
        }

        public string GetSiguienteAutonumerico(string nombreAutonumerico, Nullable<int> ejercicio, SAACGContext context)
        {
                var valorSalida = new ObjectParameter("valorSalida", "");

                context.GRspAutonumericoSigIncr(nombreAutonumerico, ejercicio, valorSalida);

                return valorSalida.Value.ToString();
        }
    }
}