using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class InvitacionCompraDetallePrecioProveedorService : BaseService<ArtblInvitacionCompraDetallePrecioProveedor>
    {
        public override ArtblInvitacionCompraDetallePrecioProveedor Inserta(ArtblInvitacionCompraDetallePrecioProveedor entidad, SAACGContext context)
        {
            //Agregamos la entidad el Context
            ArtblInvitacionCompraDetallePrecioProveedor modelo = context.ArtblInvitacionCompraDetallePrecioProveedor.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos la entidad que se acaba de guardar en la Base de Datos
            return modelo;
        }

        public override bool Actualiza(ArtblInvitacionCompraDetallePrecioProveedor entidad, SAACGContext context)
        {
            //Agregamos la entidad que vamos a actualizar al Context
            context.ArtblInvitacionCompraDetallePrecioProveedor.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ArtblInvitacionCompraDetallePrecioProveedor.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            return context.SaveChanges() > 0;        
        }

        public override ArtblInvitacionCompraDetallePrecioProveedor BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ArtblInvitacionCompraDetallePrecioProveedor.Find(id);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public List<ARspConsultaInvitacionCompraDetallePreciosProveedores_Result> BuscaInvitacionCompraListadoPreciosProveedores(int invitacionCompraId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaInvitacionCompraDetallePreciosProveedores(invitacionCompraId).ToList();
            }
        }
    }
}