using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacenBL.Services
{
    public class ProveedorProspectoService : BaseService<ARtblProveedorProspecto>
    {
        public override ARtblProveedorProspecto Inserta(ARtblProveedorProspecto entidad, SAACGContext context)
        {
            //Asignamos el autonumerico
            entidad.CodigoProspecto = new AutonumericoService().GetSiguienteAutonumerico("Prospecto a Proveedor", context);

            //Agregamos la entidad el Context
            ARtblProveedorProspecto proveedorProspecto = context.ARtblProveedorProspecto.Add(entidad);

            //Guardamos cambios
            context.SaveChanges();

            //Retornamos la entidad que se acaba de guardar en la Base de Datos
            return proveedorProspecto;
        }

        public override bool Actualiza(ARtblProveedorProspecto entidad, SAACGContext context)
        {
            //Agregamos la entidad que vamos a actualizar al Context
            context.ARtblProveedorProspecto.Add(entidad);

            //Marcamos el modelo como modificado
            context.Entry(entidad).State = EntityState.Modified;

            //Marcar todas las propiedades que no se pueden actualizar como FALSE
            //para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in ARtblProveedorProspecto.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            //Guardamos cambios
            return context.SaveChanges() > 0;      
        }
        
        public bool Guarda(List<ARtblProveedorProspecto> entidades)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    foreach (ARtblProveedorProspecto entidad in entidades)
                    {
                        if (entidad.ProveedorProspectoId == 0)
                        {
                            Inserta(entidad, Context);
                        }
                        else
                        {
                            Actualiza(entidad, Context);
                        }
                    }

                    //Guardamos cambios
                    Context.SaveChanges();

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    //Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public override ARtblProveedorProspecto BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblProveedorProspecto.Find(id);
            }
        }

        public ARtblProveedorProspecto BuscaActivoPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                //Retornamos la entidad con el ID que se envio como parametro
                return Context.ARtblProveedorProspecto.FirstOrDefault(m => m.ProveedorProspectoId == id && m.EstatusId == EstatusRegistro.ACTIVO);
            }
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ARtblProveedorProspecto> BuscaNoConvertidos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblProveedorProspecto.Where(m => m.Convertido == false && m.EstatusId == EstatusRegistro.ACTIVO).ToList();
            }
        }

        public bool ExisteRFCRazonSocial(int proveedorProspectoId, string rfc, string razonSocial)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblProveedorProspecto.FirstOrDefault(m => 
                    m.ProveedorProspectoId != proveedorProspectoId 
                    && m.EstatusId == EstatusRegistro.ACTIVO
                    && m.RFC.ToLower() == rfc.ToLower() 
                    && m.RazonSocial.ToLower() == razonSocial.ToLower()) != null;
            }
        }
    }
}