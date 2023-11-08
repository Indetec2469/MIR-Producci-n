using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services
{
    public class UsuarioService : BaseService<GRtblUsuario>
    {
        public override bool Actualiza(GRtblUsuario entidad, SAACGContext context)
        {
            // Agregamos la entidad que vamos a actualizar al Context
            context.GRtblUsuario.Add(entidad);
            context.Entry(entidad).State = EntityState.Modified;

            // Marcar todas las propiedades que no se pueden actualizar como FALSE
            // para que no se actualice su informacion en Base de Datos
            foreach (string propertyName in GRtblUsuario.PropiedadesNoActualizables)
            {
                context.Entry(entidad).Property(propertyName).IsModified = false;
            }

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos true o false si se realizo correctamente la operacion
            return true;
        }

        public override GRtblUsuario BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
                return Context.GRtblUsuario.AsEnumerable().Select(usuario => new GRtblUsuario {
                    UsuarioId = usuario.UsuarioId,
                    NombreUsuario = usuario.NombreUsuario,
                    Contrasenia = usuario.Contrasenia,
                    EmpleadoId = usuario.EmpleadoId,
                    RolId = usuario.RolId,
                    Activo = usuario.Activo,
                    Borrado = usuario.Borrado,
                    FechaCreacion = usuario.FechaCreacion,
                    CreadoPorId = usuario.CreadoPorId,
                    FechaUltimaModificacion = usuario.FechaUltimaModificacion,
                    ModificadoPorId = usuario.ModificadoPorId,
                    Timestamp = usuario.Timestamp
                }).Where(usuario => usuario.UsuarioId == id && !usuario.Borrado).FirstOrDefault();
        }

        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override GRtblUsuario Inserta(GRtblUsuario entidad, SAACGContext context)
        {
            // Agregamos la entidad el Context
            GRtblUsuario usuario = context.GRtblUsuario.Add(entidad);

            // Guardamos cambios
            context.SaveChanges();

            // Retornamos la entidad que se acaba de guardar en la Base de Datos
            return usuario;
        }

        public void GuardaCambios(GRtblUsuario usuario, IEnumerable<GRtblUsarioPermiso> listaUsuarioPermiso)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    // Iniciamos la transacción
                    Context.Database.BeginTransaction();

                    // Creamos el return
                    GRtblUsuario _usuario = usuario;

                    if (_usuario != null)
                    {
                        // Si es un registro nuevo
                        if (_usuario.UsuarioId > 0)
                        {
                            Actualiza(_usuario, Context);
                        }
                        else
                        {
                            _usuario= Inserta(_usuario, Context);
                            if (listaUsuarioPermiso != null)
                            {
                                    listaUsuarioPermiso.ToList().ForEach(up =>
                                {
                                    up.UsuarioId = _usuario.UsuarioId;
                                });
                            }

                        }
                    }

                    // Existe la lista para guardar o actualizar
                    if (listaUsuarioPermiso != null)
                    {
                        GuardaListaUsuarioPermiso(listaUsuarioPermiso, Context);
                    }

                    // Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    // Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
                catch (Exception ex)
                {
                    // Hacemos el Rollback
                    Context.Database.CurrentTransaction.Rollback();

                    throw new Exception(UserExceptionHelper.GetMessage(ex));
                }
            }
        }

        public void GuardaListaUsuarioPermiso(IEnumerable<GRtblUsarioPermiso> listaUsuarioPermiso, SAACGContext context)
        {
            // Service
            UsuarioPermisoService usuarioPermisoService = new UsuarioPermisoService();

            foreach (GRtblUsarioPermiso usuarioPermiso in listaUsuarioPermiso.ToList())
            {
                // Creamos el return
                GRtblUsarioPermiso _usuarioPermiso = usuarioPermiso;

                if (_usuarioPermiso.UsuarioPermisoId > 0)
                {
                    // Actualizamos
                    usuarioPermisoService.Actualiza(_usuarioPermiso, context);
                }
                else
                {
                    // Guardamos
                    _usuarioPermiso = usuarioPermisoService.Inserta(_usuarioPermiso, context);
                }
            }
        }

        public IEnumerable<GRtblUsuario> BuscaActivos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRtblUsuario.Where(usuario => usuario.Activo == true).ToList();
            }
        }

        public IEnumerable<GRvwListadoUsuario> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRvwListadoUsuario.ToList();
            }
        }

        public GRtblUsuario ValidaUsuarioLogin(string nombreUsuario, string contrasenia, string enteId)
        {
            using (var Context = SAACGContextHelper.GetContext(enteId))
            {
                GRtblUsuario usuarioTemp = Context.GRtblUsuario.Where(w => w.NombreUsuario == nombreUsuario && !w.Borrado).FirstOrDefault();

                //Verificamos que exista un usuario con el nombre de Usuario enviado
                if(usuarioTemp == null)
                {
                    throw new Exception("Nombre de Usuario o Contraseña Incorrecta");
                }

                //Verificamos que la contraseña coincida
                if(usuarioTemp.Contrasenia != contrasenia)
                {
                    throw new Exception("Contraseña invalida");
                }

                //Verificamos que el usuario este activo
                if(usuarioTemp.Activo != true)
                {
                    throw new Exception("Usuario no Activo");
                }

                return usuarioTemp;

            }
        }

        public Boolean ValidaExisteUsuario(GRtblUsuario usuario)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.GRtblUsuario.Any(_usuario => _usuario.UsuarioId != usuario.UsuarioId && _usuario.Borrado == false && _usuario.NombreUsuario.ToLower() == usuario.NombreUsuario.ToLower());
            }
        }
    }
}
