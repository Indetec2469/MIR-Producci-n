using Newtonsoft.Json;
using RequisicionesAlmacen.Areas.RH.Ingreso.Models;
using RequisicionesAlmacen.Areas.RH.Ingreso.Models.ViewModel;
using RequisicionesAlmacen.Controllers;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Entities;
using RequisicionesAlmacenBL.Models.Mapeos;
using RequisicionesAlmacenBL.Services;
using RequisicionesAlmacenBL.Services.SAACG;
using RequisicionesAlmacenBL.Services.Sistema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using static RequisicionesAlmacenBL.Models.Mapeos.ControlMaestroMapeo;

namespace RequisicionesAlmacen.Areas.RH.Ingreso.Controllers
{
    [Authenticated(nodoMenuId = MenuPrincipalMapeo.ID.EMPLEADOS)]
    public class EmpleadosController : BaseController<RHtblEmpleado, EmpleadoViewModel>
    {
        private string API_FICHA = "/rh/ingreso/empleados/";

        public override ActionResult Nuevo()
        {
            //Crear un objeto nuevo
            RHtblEmpleado empleado = new RHtblEmpleado();
            EmpleadoViewModel empleadoViewModel = new EmpleadoViewModel();

            //Inicializamos el ID en 0 para indicar que es un Registro Nuevo
            empleado.EmpleadoId = 0;

            //Asignamos el modelo al modelView
            empleadoViewModel.Empleado = empleado;

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref empleadoViewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("Empleado", empleadoViewModel);
        }

        public override ActionResult Editar(int id)
        {
            EmpleadoViewModel empleadoViewModel = new EmpleadoViewModel();

            //Buscamos el Objeto por el Id que se envio como parametro
            RHtblEmpleado empleado = new EmpleadoService().BuscaPorId(id);

            if (empleado == null || !empleado.Vigente || empleado.EstatusId != EstatusRegistro.ACTIVO)
            {
                //Asignamos el error
                SetViewBagError("El Empleado no existe o no está Activo. Favor de revisar.", API_FICHA + "listar");
            }

            //Asignamos el modelo al viewModel
            empleadoViewModel.Empleado = empleado != null ? empleado :  new RHtblEmpleado();
            
            //Asignamos el la imagen al viewModel
            empleadoViewModel.ImageSrc = empleadoViewModel.Empleado.Fotografia != null ? new ArchivoService().GetImageSrc(empleadoViewModel.Empleado.Fotografia) : null;

            //Agregamos todos los datos necesarios para el funcionamiento de la ficha
            //como son los Listados para combos, tablas, arboles.
            GetDatosFicha(ref empleadoViewModel);

            //Retornamos la vista junto con su Objeto Modelo
            return View("Empleado", empleadoViewModel);
        }

        [JsonException]
        public override JsonResult Guardar(RHtblEmpleado empleado)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        [HttpPost]
        public JsonResult GuardaCambios(HttpPostedFileBase file, bool cambioImagen)
        {
            int usuarioId = SessionHelper.GetUsuario().UsuarioId;
            DateTime fecha = DateTime.Now;            

            EmpleadoService service = new EmpleadoService();

            RHtblEmpleado empleado = JsonConvert.DeserializeObject<RHtblEmpleado>(Request.Form.Get("empleado"));

            bool eliminaArchivo = false;
            Nullable<Guid> archivoAnteriorId = null;

            if (service.ExisteRFC(empleado.EmpleadoId, empleado.RFC))
            {
                throw new Exception("Ya existe un registro con el mismo RFC: " + empleado.RFC + ". Favor de verificar.");
            }

            if (service.ExisteNumeroEmpleado(empleado.EmpleadoId, empleado.NumeroEmpleado))
            {
                throw new Exception("Ya existe un registro con el mismo Numero de Empleado: " + empleado.NumeroEmpleado + ". Favor de verificar.");
            }

            //Construimos los objetos de la imagen
            string fileName = null;
            Stream fileInputStream = null;

            if (file != null)
            {
                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fileName = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fileName = file.FileName;
                }

                fileInputStream = file.InputStream;
            }

            //Estatus del empleado
            empleado.EstatusId = EstatusRegistro.ACTIVO;

            //Si es un nuevo registro ponemos Vigente y llenamos el campo de CreadoPor
            if (empleado.EmpleadoId == 0)
            {
                empleado.Vigente = true;
                empleado.CreadoPorId = usuarioId;

                cambioImagen = true;
            }

            //De lo contrario llenamos el campo de ModificadoPor y Fecha de Ultima Modificacion
            else
            {
                RHtblEmpleado temp = service.BuscaPorId(empleado.EmpleadoId);

                if (!StructuralComparisons.StructuralEqualityComparer.Equals(empleado.Timestamp, temp.Timestamp))
                {
                    throw new Exception("El Empleado con el número [" + empleado.NumeroEmpleado + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
                }

                if (cambioImagen && fileInputStream == null && temp.Fotografia != null)
                {
                    empleado.Fotografia = null;

                    eliminaArchivo = true;
                    archivoAnteriorId = temp.Fotografia;
                }

                empleado.ModificadoPorId = usuarioId;
                empleado.FechaUltimaModificacion = fecha;
            }
            
            //Si pasó las validaciones guardamos los cambios
            service.GuardaCambios(empleado, fileName, fileInputStream, cambioImagen, eliminaArchivo, archivoAnteriorId);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro guardado con Exito!");
        }

        public override JsonResult Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        [JsonException]
        public JsonResult Eliminar(RHtblEmpleado empleado)
        {
            EmpleadoService service = new EmpleadoService();

            RHtblEmpleado temp = service.BuscaPorId(empleado.EmpleadoId);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(empleado.Timestamp, temp.Timestamp))
            {
                throw new Exception("El Empleado con el número [" + empleado.NumeroEmpleado + "] ha sido modificado por otro usuario. Favor de recargar la vista antes de guardar.");
            }

            empleado.EstatusId = EstatusRegistro.BORRADO;
            empleado.ModificadoPorId = SessionHelper.GetUsuario().UsuarioId;
            empleado.FechaUltimaModificacion = DateTime.Now;

            //Verificamos si es posible eliminar
            if (!new SistemaService().PermiteEliminarRegistro(empleado.EmpleadoId, Tablas.RHtblEmpleado))
            {
                throw new Exception("El empleado con el número [" + empleado.NumeroEmpleado + "] no puede ser eliminado ya que está siendo utilizado para otros procesos.");
            }

            service.Elimina(empleado);

            //Retornamos un mensaje de Exito si todo salio correctamente
            return Json("Registro eliminado con Exito!");
        }

        public override ActionResult Listar()
        {
            EmpleadoViewModel empleadoViewModel = new EmpleadoViewModel();

            empleadoViewModel.ListEmpleado = new EmpleadoService().BuscaActivos();

            return View("ListadoEmpleados", empleadoViewModel);
        }

        protected override void GetDatosFicha(ref EmpleadoViewModel empleadoViewModel)
        {
            IList<PuestoListSelect> listadoPuestos = new List<PuestoListSelect>();
            listadoPuestos.Add(new PuestoListSelect { Value = "Desarollador Jr", ID = 1 });
            listadoPuestos.Add(new PuestoListSelect { Value = "Desarollador Sr", ID = 2 });
            empleadoViewModel.ListPuesto = listadoPuestos;

            empleadoViewModel.ListAreaAdscripcion = new DependenciaService().BuscaTodos();
        }
    }
}