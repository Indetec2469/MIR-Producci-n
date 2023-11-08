using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Models.Mapeos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class MatrizConfiguracionPresupuestalDetalleHService : BaseService<MItblMatrizConfiguracionPresupuestalDetalleH>
    {
        public override bool Actualiza(MItblMatrizConfiguracionPresupuestalDetalleH entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.MItblMatrizConfiguracionPresupuestalDetalleH.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in MItblMatrizConfiguracionPresupuestalDetalleH.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override MItblMatrizConfiguracionPresupuestalDetalleH BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.MItblMatrizConfiguracionPresupuestalDetalleH.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override MItblMatrizConfiguracionPresupuestalDetalleH Inserta(MItblMatrizConfiguracionPresupuestalDetalleH entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            MItblMatrizConfiguracionPresupuestalDetalleH matrizConfiguracionPresupuestalDetalle = context.MItblMatrizConfiguracionPresupuestalDetalleH.Add(entidad);
           
            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return matrizConfiguracionPresupuestalDetalle;
        }

        

        public IEnumerable<MItblMatrizConfiguracionPresupuestalDetalleH> BuscaPorConfiguracionPresupuestoId (int configuracionPresupuestoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizConfiguracionPresupuestalDetalleH.Where(mcpd => mcpd.ConfiguracionPresupuestoId == configuracionPresupuestoId && mcpd.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }

        public IEnumerable<MItblMatrizConfiguracionPresupuestalDetalleH> BuscaPorMIRIndicadorId(int mirIndicadorId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizConfiguracionPresupuestalDetalleH.Where(mcpd => mcpd.MIRIndicadorId == mirIndicadorId && mcpd.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).Include(mcp => mcp.MItblMatrizConfiguracionPresupuestal).ToList();
            }
        }

        public Boolean ExistePorMIRI(int mirIndicadorId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizConfiguracionPresupuestalDetalleH.Any(mcpd => mcpd.MIRIndicadorId == mirIndicadorId && mcpd.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO);
            }
        }

        public IEnumerable<MItblMatrizConfiguracionPresupuestalDetalleH> BuscaPorConfiguracionPresupuestoIdYClasificadorId(int configuracionPresupuestoId, int clasificadorId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MItblMatrizConfiguracionPresupuestalDetalleH.Where(mcpd => mcpd.ConfiguracionPresupuestoId == configuracionPresupuestoId && mcpd.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
            }
        }

        public IEnumerable<MIspConsultaMatrizPresupuestoDevengadoH_Result> BuscaMatrizPresupuestoDevengado(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIspConsultaMatrizPresupuestoDevengadoH(mirId).ToList();
            }
        }

        public IEnumerable<MIspConsultaMatrizPresupuestoVigenteH_Result> BuscaMatrizPresupuestoVigente(int mirId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.MIspConsultaMatrizPresupuestoVigenteH(mirId).ToList();
            }
        }

        //public IEnumerable<MItblMatrizConfiguracionPresupuestalDetalleH> BuscaPorMIRYTipoPresupuestoId(int mirId, int tipoPresupuestoId)
        //{
        //    using (var Context = SAACGContextHelper.GetContext())
        //    {
        //        MItblMatrizConfiguracionPresupuestalDetalleH MItblMatrizConfiguracionPresupuestalDetalleH = new MItblMatrizConfiguracionPresupuestalDetalleH()
        //        return Context.Entry(MItblMatrizConfiguracionPresupuestalDetalleH).Collection(m => m.MIRIndicadorId).Where(mcpd => mcpd.mi == configuracionPresupuestoId && mcpd.ClasificadorId == clasificadorId && mcpd.EstatusId == ControlMaestroMapeo.EstatusRegistro.ACTIVO).ToList();
        //    }
        //}

        
    }
}
