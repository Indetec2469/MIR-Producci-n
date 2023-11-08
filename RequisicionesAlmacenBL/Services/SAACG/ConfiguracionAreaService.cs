using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RequisicionesAlmacenBL.Services.SAACG
{
    public class ConfiguracionAreaService : BaseService<ARtblControlMaestroConfiguracionArea>
    {
        public override ARtblControlMaestroConfiguracionArea BuscaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionArea.Where(m => 
                    m.ConfiguracionAreaId == id 
                    && m.Borrado == false
                ).FirstOrDefault();
            }
        }

        public List<ARtblControlMaestroConfiguracionArea> BuscaTodos()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionArea.ToList();
            }
        }

        public List<ARvwListadoConfiguracionArea> BuscaListado()
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARvwListadoConfiguracionArea.ToList();
            }
        }

        public override ARtblControlMaestroConfiguracionArea Inserta(ARtblControlMaestroConfiguracionArea entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }

        public override bool Actualiza(ARtblControlMaestroConfiguracionArea entidad, SAACGContext context)
        {
            throw new NotImplementedException();
        }
        
        public bool Guarda(List<ARtblControlMaestroConfiguracionArea> entidades)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                try
                {
                    //Iniciamos la Transacción
                    Context.Database.BeginTransaction();

                    foreach (ARtblControlMaestroConfiguracionArea entidad in entidades)
                    {
                        //Respaldamos los almacenes y proyectos
                        ICollection<ARtblControlMaestroConfiguracionAreaProyecto> proyectos = entidad.ARtblControlMaestroConfiguracionAreaProyecto;
                        ICollection<ARtblControlMaestroConfiguracionAreaAlmacen> almacenes = entidad.ARtblControlMaestroConfiguracionAreaAlmacen;

                        entidad.ARtblControlMaestroConfiguracionAreaAlmacen = null;
                        entidad.ARtblControlMaestroConfiguracionAreaProyecto = null;

                        //Agregamos la entidad el Context
                        ARtblControlMaestroConfiguracionArea configuracionArea = Context.ARtblControlMaestroConfiguracionArea.Add(entidad);

                        //Verificar si es una entidad a actualizar
                        if (entidad.ConfiguracionAreaId > 0)
                        {
                            Context.Entry(entidad).State = EntityState.Modified;

                            //Marcar todas las propiedades que no se pueden actualizar como FALSE
                            //para que no se actualice su informacion en Base de Datos
                            foreach (string propertyName in configuracionArea.PropiedadesNoActualizables)
                            {
                                Context.Entry(entidad).Property(propertyName).IsModified = false;
                            }
                        }

                        //Guardamos cambios
                        Context.SaveChanges();

                        //Guardamos los Detalles
                        foreach (ARtblControlMaestroConfiguracionAreaProyecto detalle in proyectos)
                        {
                            if (detalle.ConfiguracionAreaProyectoId > 0 || !detalle.Borrado)
                            {
                                if (detalle.ConfiguracionAreaProyectoId == 0)
                                {
                                    detalle.ConfiguracionAreaId = configuracionArea.ConfiguracionAreaId;
                                    detalle.CreadoPorId = configuracionArea.ModificadoPorId != null ? configuracionArea.ModificadoPorId.GetValueOrDefault() : configuracionArea.CreadoPorId;
                                }
                                else
                                {
                                    detalle.ModificadoPorId = configuracionArea.ModificadoPorId != null ? configuracionArea.ModificadoPorId.GetValueOrDefault() : configuracionArea.CreadoPorId;
                                    detalle.FechaUltimaModificacion = configuracionArea.FechaUltimaModificacion != null ? configuracionArea.FechaUltimaModificacion.GetValueOrDefault() : configuracionArea.FechaCreacion;
                                }

                                //Agregamos los detalles al Context
                                Context.ARtblControlMaestroConfiguracionAreaProyecto.Add(detalle);

                                if (detalle.ConfiguracionAreaProyectoId > 0)
                                {
                                    Context.Entry(detalle).State = EntityState.Modified;

                                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                                    //para que no se actualice su informacion en Base de Datos
                                    foreach (string propertyName in detalle.PropiedadesNoActualizables)
                                    {
                                        Context.Entry(detalle).Property(propertyName).IsModified = false;
                                    }
                                }

                                //Guardamos cambios
                                Context.SaveChanges();
                            }
                        }

                        //Guardamos los Almacenes
                        foreach (ARtblControlMaestroConfiguracionAreaAlmacen detalle in almacenes)
                        {
                            if (detalle.ConfiguracionAreaAlmacenId > 0 || !detalle.Borrado)
                            {
                                if (detalle.ConfiguracionAreaAlmacenId == 0)
                                {
                                    detalle.ConfiguracionAreaId = configuracionArea.ConfiguracionAreaId;
                                    detalle.CreadoPorId = configuracionArea.ModificadoPorId != null ? configuracionArea.ModificadoPorId.GetValueOrDefault() : configuracionArea.CreadoPorId;
                                }
                                else
                                {
                                    detalle.ModificadoPorId = configuracionArea.ModificadoPorId != null ? configuracionArea.ModificadoPorId.GetValueOrDefault() : configuracionArea.CreadoPorId;
                                    detalle.FechaUltimaModificacion = configuracionArea.FechaUltimaModificacion != null ? configuracionArea.FechaUltimaModificacion.GetValueOrDefault() : configuracionArea.FechaCreacion;
                                }

                                //Agregamos los almacenes al Context
                                Context.ARtblControlMaestroConfiguracionAreaAlmacen.Add(detalle);

                                if (detalle.ConfiguracionAreaAlmacenId > 0)
                                {
                                    Context.Entry(detalle).State = EntityState.Modified;

                                    //Marcar todas las propiedades que no se pueden actualizar como FALSE
                                    //para que no se actualice su informacion en Base de Datos
                                    foreach (string propertyName in detalle.PropiedadesNoActualizables)
                                    {
                                        Context.Entry(detalle).Property(propertyName).IsModified = false;
                                    }
                                }

                                //Guardamos cambios
                                Context.SaveChanges();
                            }
                        }
                    }                    

                    //Hacemos el Commit
                    Context.Database.CurrentTransaction.Commit();

                    //Retornamos true si se realizo correctamente la operación
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
        
        public override bool Elimina(int id, int eliminadoPorId, SAACGContext context)
        {
            throw new NotImplementedException();
        }        

        public List<ARtblControlMaestroConfiguracionAreaProyecto> BuscaDetallesPorConfiguracionAreaId(int configuracionAreaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionAreaProyecto.Where(m => 
                    m.ConfiguracionAreaId == configuracionAreaId 
                    && m.Borrado == false
                ).ToList();
            }
        }

        public List<ARspConsultaConfiguracionAreaProyectos_Result> BuscaDependenciasProyectosPorConfiguracionAreaId(int configuracionAreaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaConfiguracionAreaProyectos(configuracionAreaId).ToList();
            }
        }

        public List<ARtblControlMaestroConfiguracionAreaAlmacen> BuscaConfiguracionAlmacenesPorConfiguracionAreaId(int configuracionAreaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionAreaAlmacen.Where(m =>
                    m.ConfiguracionAreaId == configuracionAreaId
                    && m.Borrado == false
                ).ToList();
            }
        }

        public List<ARspConsultaConfiguracionAreaAlmacenes_Result> BuscaAlmacenesPorConfiguracionAreaId(int configuracionAreaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARspConsultaConfiguracionAreaAlmacenes(configuracionAreaId).ToList();
            }
        }

        public bool ExisteArea(int id, string areaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionArea.Where(m => m.ConfiguracionAreaId != id
                                                                            && m.AreaId.Equals(areaId)
                                                                            && m.Borrado == false).FirstOrDefault() != null;
            }
        }

        public tblProyecto_Dependencia BuscaProyectoDependenciaPorId(int id)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.tblProyecto_Dependencia.Where(m => m.idPD == id).FirstOrDefault();
            }
        }

        public ARtblControlMaestroConfiguracionArea BuscaPorAreaId(string areaId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionArea.Where(m =>
                    m.AreaId.Equals(areaId)
                    && m.Borrado == false
                ).FirstOrDefault();
            }
        }

        public ARtblControlMaestroConfiguracionAreaProyecto BuscaConfiguracionProyectoPorAreaId(int configuracionAreaId, string dependenciaId, string proyectoId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionAreaProyecto
                    .Join(Context.tblProyecto_Dependencia, d => d.ProyectoDependenciaId, e => e.idPD, (d, e) => new { e = e, d = d })
                    .Where(m => m.d.ConfiguracionAreaId == configuracionAreaId
                             && m.e.DependenciaId.Equals(dependenciaId)
                             && m.e.ProyectoId.Equals(proyectoId)
                             && m.d.Borrado == false
                          )
                    .AsEnumerable().Select(m => new ARtblControlMaestroConfiguracionAreaProyecto
                    {
                        ConfiguracionAreaProyectoId = m.d.ConfiguracionAreaProyectoId
                    }).FirstOrDefault();
            }
        }

        public ARtblControlMaestroConfiguracionAreaAlmacen BuscaConfiguracionAlmacenPorAreaId(int configuracionAreaId, string almacenId)
        {
            using (var Context = SAACGContextHelper.GetContext())
            {
                return Context.ARtblControlMaestroConfiguracionAreaAlmacen
                    .Where(m => m.ConfiguracionAreaId == configuracionAreaId
                             && m.AlmacenId.Equals(almacenId)
                             && m.Borrado == false
                          ).FirstOrDefault();
            }
        }
    }
}
