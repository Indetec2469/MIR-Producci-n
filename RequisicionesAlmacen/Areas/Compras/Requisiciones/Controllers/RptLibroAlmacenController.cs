using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FastReport.Web;
using RequisicionesAlmacen.Helpers;
using RequisicionesAlmacenBL.Services.SAACG;

namespace RequisicionesAlmacen.Areas.Compras.Requisiciones.Controllers
{
    public class RptLibroAlmacenController : Controller
    {
        // GET: Compras/RptLibroAlmacen
        public ActionResult RptLibroAlmacenPorArticulo(int id)
        {
            ReportHelper reportHelper = new ReportHelper();

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@pTituloReporte", "Inventario Físico por Artículo");
            parametros.Add("@pNombreReporte", "rptLibroAlmacen");
            parametros.Add("@pAlmacen", new AlmacenService().BuscaPorId(id).Nombre);
            parametros.Add("@pAlmacenId", id);
            
            WebReport webReport = reportHelper.GetWebReport("Almacen/InventarioFisico/ARrptInventarioFisico_por_articulo.frx", parametros);
            ViewBag.WebReport = webReport;
            return View("rptLibroAlmacen");
        }

        public ActionResult RptLibroAlmacenPorClave(int id)
        {
            ReportHelper reportHelper = new ReportHelper();

            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@pTituloReporte", "Inventario Físico por Clave");
            parametros.Add("@pNombreReporte", "rptLibroAlmacen");
            parametros.Add("@pAlmacen", new AlmacenService().BuscaPorId(id).Nombre);
            parametros.Add("@pAlmacenId", id);

            WebReport webReport = reportHelper.GetWebReport("Almacen/InventarioFisico/ARrptInventarioFisico_por_clave.frx", parametros);
            ViewBag.WebReport = webReport;
            return View("rptLibroAlmacen");
        }
    }
}