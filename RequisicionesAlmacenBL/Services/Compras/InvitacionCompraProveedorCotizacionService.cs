using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services
{
    public class InvitacionCompraProveedorCotizacionService : BaseService<ArtblInvitacionCompraProveedorCotizacion>
    {
        public override ArtblInvitacionCompraProveedorCotizacion Inserta(ArtblInvitacionCompraProveedorCotizacion entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ArtblInvitacionCompraProveedorCotizacion modelo = context.ArtblInvitacionCompraProveedorCotizacion.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos la entidad que se acaba de guardar en la Base de Datos
            return modelo;
        }

        public override bool Actualiza(ArtblInvitacionCompraProveedorCotizacion entidad, SAACGContext context)
        {
            //Agregamos la entidad que vamos a actualizar al Context
            context.ArtblInvitacionCompraProveedorCotizacion.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ArtblInvitacionCompraProveedorCotizacion.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            return context.SaveChanges() > 0;
        
        }

        public void GuardaCambios(Nullable<int> invitacionCompraProveedorId, List<ArtblInvitacionCompraProveedorCotizacion> cotizaciones, SAACGContext context)
        {
            foreach (ArtblInvitacionCompraProveedorCotizacion cotizacionTemp in cotizaciones)
            {
                //Asignamos el Id de la cabecera
                cotizacionTemp.InvitacionCompraProveedorId = invitacionCompraProveedorId.GetValueOrDefault();

                //Guardamos la cotizacion
                ArtblInvitacionCompraProveedorCotizacion cotizacion = GuardaCotizacion(cotizacionTemp, context);

                cotizacion.NombreArchivoTmp = cotizacionTemp.NombreArchivoTmp;

                if (cotizacion.NombreArchivoTmp != null)
                {
                    GuardaArchivo(cotizacion, context);
                }
            }
        }

        public ArtblInvitacionCompraProveedorCotizacion GuardaCotizacion(ArtblInvitacionCompraProveedorCotizacion entidad, SAACGContext context)
        {
            //Agregamos los detalles al Context
            ArtblInvitacionCompraProveedorCotizacion cotizacion = context.ArtblInvitacionCompraProveedorCotizacion.Add(entidad);

            //Si es un registro que se va actualizar
            if (cotizacion.InvitacionCompraProveedorCotizacionId > 0)
            {
                context.Entry(cotizacion).State = EntityState.Modified;

                //Marcar todas las propiedades que no se pueden actualizar como FALSE
                //para que no se actualice su informacion en Base de Datos
                foreach (string propertyName in ArtblInvitacionCompraProveedorCotizacion.PropiedadesNoActualizables)
                {
                    context.Entry(cotizacion).Property(propertyName).IsModified = false;
                }
            }

            //Guardamos cambios
            context.SaveChanges();

            return cotizacion;
        }

        private void GuardaArchivo(ArtblInvitacionCompraProveedorCotizacion cotizacion, SAACGContext context)
        {
            ArchivoService archivoService = new ArchivoService();

            string nombreArchivoTemporal = cotizacion.NombreArchivoTmp;
            string extensionArchivo = nombreArchivoTemporal.Substring(nombreArchivoTemporal.LastIndexOf("."));
            int tipoArchivo = archivoService.ObtenerTipoArchivo(extensionArchivo);
            int id = cotizacion.InvitacionCompraProveedorCotizacionId;

            Guid archivoId = archivoService.GuardaArchivo(cotizacion.CreadoPorId,
                                                            null,
                                                            nombreArchivoTemporal,
                                                            null,
                                                            ListadoCMOA.INVITACION_COMPRA_PROVEEDOR_COTIZACION,
                                                            new List<string>() { id.ToString(), cotizacion.InvitacionCompraProveedorId.ToString() },
                                                            id,
                                                            tipoArchivo,
                                                            context);

            cotizacion.CotizacionId = archivoId;

            GuardaCotizacion(cotizacion, context);
        }

        public override ArtblInvitacionCompraProveedorCotizacion BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ArtblInvitacionCompraProveedorCotizacion.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<ARspConsultaInvitacionCompraProveedorCotizaciones_Result> BuscaInvitacionCompraListadoCotizaciones(int invitacionCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInvitacionCompraProveedorCotizaciones(invitacionCompraId).ToList();
            }
        }
    }
}