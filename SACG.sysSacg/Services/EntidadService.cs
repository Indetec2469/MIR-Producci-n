using SACG.sysSacg.Entities;
using SACG.sysSacg.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACG.sysSacg.Services
{
    public class EntidadService
    {
        public Entidad GetEntidad( string entidadId)
        {
            using (SACGSYSContext context = SAACGSYSContextHelper.GetContext())
            {
                return context.Entidad.Find(entidadId);
            } 
        }

        public IEnumerable<Entidad> ObtenerEntidades()
        {
            using (SACGSYSContext context = SAACGSYSContextHelper.GetContext())
            {
                return context.Entidad.ToList();
            }
        }

        public int ObtenerTipoVersion()
        {
            using (SACGSYSContext context = SAACGSYSContextHelper.GetContext())
            {
                return context.vwTipoVersion.First().TipoVersion;
            }
        }
    }
}
