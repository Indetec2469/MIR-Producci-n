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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.Sistemas.Catalogos.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.USUARIOS)]
    public class UsuarioController : BaseController<UsuarioViewModel, UsuarioViewModel>
    {
        private string API_FICHA = "/sistemas/catalogos/usuario/";

        public ActionResult Index()
        {
            return View();
        }

        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            //Asignamos el modelo al modelView
            usuarioViewModel.Usuario = new GRtblUsuario();
            usuarioViewModel.ListaUsuarioPermiso = new List<GRtblUsarioPermiso>();
            usuarioViewModel.UsuarioPermisoModel = new GRtblUsarioPermiso();
            
            //Inicializamos el ID en 0 para indicar que es un Registro Nuevo
            usuarioViewModel.Usuario.UsuarioId = 0;
            usuarioViewModel.Usuario.Activo = true;
            usuarioViewModel.Usuario.Borrado = false;
            
            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref usuarioViewModel);
            
            //Asignamos el combo de empleado
            usuarioViewModel.ListaEmpleado = new EmpleadoService().BuscaLigadoTodos(usuarioViewModel.Usuario.UsuarioId);
            
            //Retornamos la vista junto con su Objeto Modelo
            return View("Usuario", usuarioViewModel);
        }

        public override ActionResult Editar(int id)
        {
            //Crear un objeto nuevo
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            //Buscamos el objeto por Id
            GRtblUsuario usuario = new UsuarioService().BuscaPorId(id);

            //Asignamos el modelo al modelView
            usuarioViewModel.Usuario = usuario != null ? usuario : new GRtblUsuario();

            if (usuario == null)
            {
                //Asignamos el error
                SetViewBagError("El Usuario no existe o está Borrado. Favor de Revisar.", API_FICHA + "listar");
            }
            else if (!usuario.Activo)
            {
                //Asignamos el error
                SetViewBagError("El Usuario no está Activo. Favor de Revisar.", API_FICHA + "listar");
            }
            else
            {
                usuarioViewModel.ListaUsuarioPermiso = new UsuarioPermisoService().BuscaPorUsuarioId(id);
                usuarioViewModel.UsuarioPermisoModel = new GRtblUsarioPermiso();

                //Agregamos todos los datos necesarios para el funcionamiento de la ficha
                //como son los Listados para combos, tablas, arboles.
                GetDatosFicha(ref usuarioViewModel);

                //Asignamos el combo de empleado
                usuarioViewModel.ListaEmpleado = new EmpleadoService().BuscaLigadoTodos(usuarioViewModel.Usuario.UsuarioId);

                //LLenamos el valor de password
                usuarioViewModel.Usuario.Contrasenia = SACG.GDALib.cSecurity.DecryptINI(usuarioViewModel.Usuario.Contrasenia);
            }
            
            //Retornamos la vista junto con su Objeto Modelo
            return View("Usuario", usuarioViewModel);
        }

        [JsonException]
        public override JsonResult Eliminar(int id)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            UsuarioService usuarioService = new UsuarioService();

            GRtblUsuario usuario = usuarioService.BuscaPorId(id);
            
            if (usuario.Borrado == true)
            {
                throw new Exception("El Usuario con el código [" + usuario.UsuarioId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            //Verificamos si el usuario puede ser eliminado
            bool permiteEliminar = new SistemaService().PermiteEliminarRegistro(usuario.UsuarioId, Tablas.GRtblUsuario);
            
            if (!permiteEliminar)
            {
                throw new Exception("El Usuario con el código [" + usuario.UsuarioId + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
            }
            
            usuario.ModificadoPorId = usuarioId;
            usuario.FechaUltimaModificacion = DateTime.Now;
            usuario.Borrado = true;

            if (usuario.EmpleadoId == null)
            {
                usuario.EmpleadoId = 0;
            }

            //Eliminamos Usuario Permiso
            IEnumerable<GRtblUsarioPermiso> listaUsuarioPermiso = new UsuarioPermisoService().BuscaPorUsuarioId(id);
            
            foreach (GRtblUsarioPermiso usuarioPermiso in listaUsuarioPermiso)
            {
                usuarioPermiso.EstatusId = EstatusRegistro.BORRADO;
                usuarioPermiso.ModificadoPorId = usuarioId;
                usuarioPermiso.FechaUltimaModificacion = DateTime.Now;
            }

            usuarioService.GuardaCambios(usuario, listaUsuarioPermiso);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        [JsonException]
        public override JsonResult Guardar(UsuarioViewModel usuarioViewModel)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;

            if (usuarioViewModel.Usuario != null)
            {
                UsuarioService usuarioService = new UsuarioService();
                
                if (usuarioViewModel.Usuario.UsuarioId > 0)
                {
                    //Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                    GRtblUsuario usuario = usuarioService.BuscaPorId(usuarioViewModel.Usuario.UsuarioId);
                    
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(usuarioViewModel.Usuario.Timestamp, usuario.Timestamp))
                    {
                        throw new Exception("El Usuario con el código [" + usuario.UsuarioId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                    }

                    //Encriptamos el password
                    usuarioViewModel.Usuario.Contrasenia = SACG.GDALib.cSecurity.EncryptINI(usuarioViewModel.Usuario.Contrasenia);

                    //LLenamos los campos de control
                    usuarioViewModel.Usuario.ModificadoPorId = usuarioId;
                    usuarioViewModel.Usuario.FechaUltimaModificacion = DateTime.Now;
                }
                else
                {
                    //Encriptamos el password
                    usuarioViewModel.Usuario.Contrasenia = SACG.GDALib.cSecurity.EncryptINI(usuarioViewModel.Usuario.Contrasenia);
                    
                    //LLenamos los campos de control
                    usuarioViewModel.Usuario.CreadoPorId = usuarioId;
                }
            }

            //Usuario Permiso
            if (usuarioViewModel.ListaUsuarioPermiso != null)
            {
                foreach(GRtblUsarioPermiso usuarioPermiso in usuarioViewModel.ListaUsuarioPermiso)
                {
                    //Si el ID es nuevo para registrar o actualizar
                    if (usuarioPermiso.UsuarioPermisoId > 0)
                    {
                        //Verificar si el usuario ha sido modificado en mismo y regresa mensaje de error
                        GRtblUsarioPermiso _usuarioPermiso = new UsuarioPermisoService().BuscaPorId(usuarioPermiso.UsuarioPermisoId);
                        
                        if (!StructuralComparisons.StructuralEqualityComparer.Equals(usuarioPermiso.Timestamp, _usuarioPermiso.Timestamp))
                        {
                            throw new Exception("El Usuario con el código [" + usuarioPermiso.UsuarioPermisoId + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                        }

                        usuarioPermiso.ModificadoPorId = usuarioId;
                        usuarioPermiso.FechaUltimaModificacion = DateTime.Now;
                    }
                    else
                    {
                        usuarioPermiso.CreadoPorId = usuarioId;
                    }
                }
            }

            new UsuarioService().GuardaCambios(usuarioViewModel.Usuario, usuarioViewModel.ListaUsuarioPermiso);

            return Json("Registro guardado con Exito!");
        }

        public override ActionResult Listar()
        {
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            usuarioViewModel.ListadoUsuario = new UsuarioService().BuscaListado();

            return View("ListadoUsuario", usuarioViewModel);
        }

        protected override void GetDatosFicha(ref UsuarioViewModel usuarioViewModel)
        {
            usuarioViewModel.ListaRol = new RolService().BuscaTodos();
            usuarioViewModel.ListaArbolPermisoFicha = new PermisoFichaService().BuscaArbolPermisoFicha();
        }

        [JsonException]
        public JsonResult ValidaUsuario(UsuarioViewModel usuarioViewModel)
        {
            return Json(new UsuarioService().ValidaExisteUsuario(usuarioViewModel.Usuario) ? false : true);
        }
    }
}