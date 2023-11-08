using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.Sistema
{
    class ArchivoEstructuraCarpetaService : BaseService<GRtblArchivoEstructuraCarpeta>
    {
        public override bool Actualiza(GRtblArchivoEstructuraCarpeta entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblArchivoEstructuraCarpeta BuscaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblArchivoEstructuraCarpeta Inserta(GRtblArchivoEstructuraCarpeta entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public string GetCarpetaPorOrigenArchivoId(int OrigenArchivoId)
        {
            using (var content = SAACGContextHelper.GetContext())
            {
                return content.GRtblArchivoEstructuraCarpeta.Where(aec => aec.OrigenArchivoId == 3 && aec.Vigente == true).FirstOrDefault().NombreCarpeta;
            }
        }
    }
}
