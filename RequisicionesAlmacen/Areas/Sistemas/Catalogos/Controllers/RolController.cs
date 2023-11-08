using RequisicionesAlmacen.Areas.Sistemas.Catalogos.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Sistemas.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.ROLES)]
    public class RolController : BaseController<RolViewModel, RolViewModel>
    {
        private string API_FICHA = "/sistemas/catalogos/rol/";

        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult Nuevo()
        {
            // Crear los objetos nuevos
            RolViewModel rolViewModel = new RolViewModel();

            // Asignamos los modelos nuevos al ViewModel
            rolViewModel.Rol = new GRtblRol();
            rolViewModel.RolMenuModel = new GRtblRolMenu();
            rolViewModel.ListRolMenu = new List<GRtblRolMenu>();

            // Inicializamos el ID en 0 y EstatusId para indicar que es un Registro Nuevo, tambien alunos.
            rolViewModel.Rol.RolId = 0;
            rolViewModel.Rol.EstatusId = EstatusRegistro.ACTIVO;

            // Agregamos todos los datos necesarios para el funcionamiento de la ficha
            // como son los listados para combos, tablas, arboles.
            GetDatosFicha(ref rolViewModel);

            return View("Rol", rolViewModel);
        }

        public override ActionResult Editar(int id)
        {
            // Crear los objetos nuevos
            RolViewModel rolViewModel = new RolViewModel();

            //Buscamos el modelo por Id
            GRtblRol rol = new RolService().BuscaPorId(id);

            // Asignamos los modelos nuevos al ViewModel
            rolViewModel.Rol = rol != null ? rol : new GRtblRol();

            if (rol == null)
            {
                //Asignamos el error
                SetViewBagError("El Rol no existe o no está Activo. Favor de Revisar.", API_FICHA + "listar");
            }
            else
            {
                rolViewModel.RolMenuModel = new GRtblRolMenu();
                rolViewModel.ListRolMenu = new RolMenuService().BuscaPorRolId(id);

                // Agregamos todos los datos necesarios para el funcionamiento de la ficha
                // como son los listados para combos, tablas, arboles.
                GetDatosFicha(ref rolViewModel);
            }

            return View("Rol", rolViewModel);
        }

        [JsonException]
        public override JsonResult Eliminar(int id)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            
            RolService rolService = new RolService();

            GRtblRol rol = rolService.BuscaPorId(id);
            
            if (rol.EstatusId == EstatusRegistro.BORRADO)
            {
                throw new Exception("El Rol con el código [" + rol.RolId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            // Eliminamos Rol
            rol.EstatusId = EstatusRegistro.BORRADO;
            rol.ModificadoPorId = usuarioId;
            rol.FechaUltimaModificacion = DateTime.Now;

            // Eliminamos Rol Menu
            IEnumerable<GRtblRolMenu> listRolMenu = new RolMenuService().BuscaPorRolId(id);
            foreach (GRtblRolMenu rolMenu in listRolMenu)
            {
                rolMenu.EstatusId = EstatusRegistro.BORRADO;
                rolMenu.ModificadoPorId = usuarioId;
                rolMenu.FechaUltimaModificacion = DateTime.Now;
            }

            //Verificamos si es posible eliminar
            if (!new SistemaService().PermiteEliminarRegistro(rol.RolId, Tablas.GRtblRol))
            {
                throw new Exception("El Rol con el código [" + rol.RolId + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
            }

            rolService.GuardaCambios(rol, listRolMenu);

            return Json("Registro eliminado con Exito!");
        }

        [JsonException]
        public override JsonResult Guardar(RolViewModel rolViewModel)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            
            RolService rolService = new RolService();
            
            if (rolViewModel.Rol != null)
            {
                // Si el ID es nuevo para registrar o actualizar
                if (rolViewModel.Rol.RolId > 0)
                {
                    // Verificar si el usuario ha sido modificado y regresa mensaje de error
                    GRtblRol rol = rolService.BuscaPorId(rolViewModel.Rol.RolId);
                    
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(rolViewModel.Rol.Timestamp, rol.Timestamp))
                    {
                        throw new Exception("Rol con el código [" + rol.RolId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                    }

                    rolViewModel.Rol.ModificadoPorId = usuarioId;
                    rolViewModel.Rol.FechaUltimaModificacion = DateTime.Now;
                }
                else
                {
                    rolViewModel.Rol.CreadoPorId = usuarioId;
                }
            }
            
            if (rolViewModel.ListRolMenu != null)
            {
                RolMenuService rolMenuService = new RolMenuService();
                
                foreach (GRtblRolMenu rolMenu in rolViewModel.ListRolMenu)
                {
                    // Si el ID es nuevo para registrar o actualizar
                    if (rolMenu.RolMenuId > 0)
                    {
                        // Verificar si el usuario ha sido modificado y regresa mensaje de error
                        GRtblRolMenu _rolMenu = rolMenuService.BuscaPorId(rolMenu.RolMenuId);
                        
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(rolMenu.Timestamp, _rolMenu.Timestamp))
                        {
                            throw new Exception("Rol Menu con el código [" + _rolMenu.RolMenuId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        rolMenu.ModificadoPorId = usuarioId;
                        rolMenu.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        rolMenu.CreadoPorId = usuarioId; 
                    }
                }
            }

            rolService.GuardaCambios(rolViewModel.Rol, rolViewModel.ListRolMenu);

            return Json("Registro guardado con Exito!");
        }

        public override ActionResult Listar()
        {
            RolViewModel rolViewModel = new RolViewModel();

            rolViewModel.ListadoRol = new RolService().BuscaListado();

            return View("ListadoRol", rolViewModel);
        }

        protected override void GetDatosFicha(ref RolViewModel rolViewModel)
        {
            rolViewModel.ListMenuPrincipal = new MenuPrincipalService().BuscaTodos();
        }
    }
}