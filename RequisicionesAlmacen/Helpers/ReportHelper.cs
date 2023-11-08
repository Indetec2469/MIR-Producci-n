using FastReport;
using FastReport.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SACG.sysSacg.Services;
using SACG.sysSacg.Entities;
using RequisicionesAlmacenBL.Helpers;

namespace RequisicionesAlmacen.Helpers
{
    public class ReportHelper
    {
        protected string rutaReportes = "~/App_Data/Reportes/";

        public WebReport GetWebReport(string rutaReporte, Dictionary<string, object> parametros)
        {
            return GetWebReport(rutaReporte, parametros, true);
        }

        public WebReport GetWebReport(string rutaReporte, Dictionary<string, object> parametros, bool usaEncabezado)
        {
            WebReport webReport = new WebReport(true, true);
 
            //Obtenemos la ruta del reporte a mostrar
            webReport.Report.Load(System.Web.HttpContext.Current.Server.MapPath(rutaReportes + rutaReporte));

            //Cambiamos la conexion para indicarle a que base de datos debe conectarse para obtener la informacion
            webReport.Report.Dictionary.Connections[0].ConnectionString = SAACGContextHelper.GetCadenaConexionReportes(SessionHelper.GetUsuario().EnteId);
       
            //Agregamos los parametros para el encabezado del reporte
            Entidad entidad = GetDatosEntidad();
            
            if (entidad != null)
            {
                webReport.Report.SetParameterValue("@pNombreEnte", entidad.Nombre);
                webReport.Report.SetParameterValue("@pEstado", entidad.Estado);
                webReport.Report.SetParameterValue("@pFechaCorte", SessionHelper.GetUsuario().Ejercicio);
                webReport.Report.SetParameterValue("@pUsuario", SessionHelper.GetUsuario().NombreUsuario);
            }
            
            //Agregamos los demas parametros que se necesiten para el reporte
            SetParametros(ref webReport, parametros);
            
            //Preparamos el reporte
            webReport.Report.Prepare(true);

            //Retornamos el reporte
            return webReport;
        }

        protected void SetParametros(ref WebReport webReport, Dictionary<string, object> parametros)
        {
            if (parametros != null)
            {
                foreach (KeyValuePair<string, object> parametro in parametros)
                {
                    webReport.Report.SetParameterValue(parametro.Key, parametro.Value);
                }
            }
        }

        public Entidad GetDatosEntidad()
        {
            return new EntidadService().GetEntidad(SessionHelper.GetUsuario().EnteId);
        }
    }
}