using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services
{
    public class InvitacionCompraProveedorService : BaseService<ARtblInvitacionCompraProveedor>
    {
        public override ARtblInvitacionCompraProveedor Inserta(ARtblInvitacionCompraProveedor entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ARtblInvitacionCompraProveedor modelo = context.ARtblInvitacionCompraProveedor.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Guardamos las cotizaciones
            if (entidad.Cotizaciones != null)
            {
                new InvitacionCompraProveedorCotizacionService().GuardaCambios(modelo.InvitacionCompraProveedorId, entidad.Cotizaciones, context);
            }

            //Retornamos la entidad que se acaba de guardar en la Base de Datos
            return modelo;
        }

        public override bool Actualiza(ARtblInvitacionCompraProveedor entidad, SAACGContext context)
        {
            //Agregamos la entidad que vamos a actualizar al Context
            context.ARtblInvitacionCompraProveedor.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblInvitacionCompraProveedor.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            context.SaveChanges();

            //Guardamos las cotizaciones
            if (entidad.Cotizaciones != null)
            {
                new InvitacionCompraProveedorCotizacionService().GuardaCambios(entidad.InvitacionCompraProveedorId, entidad.Cotizaciones, context);
            }

            //Guardamos cambios
            return true;   
        }

        public override ARtblInvitacionCompraProveedor BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblInvitacionCompraProveedor.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<tblProveedor> BuscaProveedores(int invitacionCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblProveedor
                    .Join(Context.ARtblInvitacionCompraProveedor, d => d.ProveedorId, e => e.ProveedorId, (d, e) => new { e = e, d = d })
                    .Where(m => m.e.InvitacionCompraId == invitacionCompraId
                             && m.e.EstatusId == EstatusRegistro.ACTIVO)
                    .AsEnumerable().Select(m => new tblProveedor
                    {
                        ProveedorId = m.d.ProveedorId,
                        RFC = m.d.RFC,
                        RazonSocial = m.d.RazonSocial
                    }).ToList();
            }
        }
    }
}