using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Models.Mapeos
{
    public class Tablas
    {
        public static string ARtblAlmacenProducto = "ARtblAlmacenProducto";
        public static string ARtblCompraCancelInfo = "ARtblCompraCancelInfo";
        public static string ARtblControlMaestroConceptoAjusteInventario = "ARtblControlMaestroConceptoAjusteInventario";
        public static string ARtblControlMaestroConfiguracionArea = "ARtblControlMaestroConfiguracionArea";
        public static string ARtblControlMaestroConfiguracionAreaAlmacen = "ARtblControlMaestroConfiguracionAreaAlmacen";
        public static string ARtblControlMaestroConfiguracionAreaProyecto = "ARtblControlMaestroConfiguracionAreaProyecto";
        public static string ARtblControlMaestroConfiguracionMontoCompra = "ARtblControlMaestroConfiguracionMontoCompra";
        public static string ARtblCortesia = "ARtblCortesia";
        public static string ARtblCortesiaDetalle = "ARtblCortesiaDetalle";
        public static string ARtblInventarioAjuste = "ARtblInventarioAjuste";
        public static string ARtblInventarioAjusteDetalle = "ARtblInventarioAjusteDetalle";
        public static string ARtblInventarioFisico = "ARtblInventarioFisico";
        public static string ARtblInventarioFisicoDetalle = "ARtblInventarioFisicoDetalle";
        public static string ARtblInventarioMovimiento = "ARtblInventarioMovimiento";
        public static string ARtblInventarioMovimientoAgrupador = "ARtblInventarioMovimientoAgrupador";
        public static string ARtblInvitacionArticulo = "ARtblInvitacionArticulo";
        public static string ARtblInvitacionArticuloDetalle = "ARtblInvitacionArticuloDetalle";
        public static string ARtblInvitacionCompra = "ARtblInvitacionCompra";
        public static string ARtblInvitacionCompraDetalle = "ARtblInvitacionCompraDetalle";
        public static string ARtblInvitacionCompraDetallePrecioProveedor = "ARtblInvitacionCompraDetallePrecioProveedor";
        public static string ARtblInvitacionCompraProveedor = "ARtblInvitacionCompraProveedor";
        public static string ARtblInvitacionCompraProveedorCotizacion = "ARtblInvitacionCompraProveedorCotizacion";
        public static string ARtblOrdenCompraInvitacionDet = "ARtblOrdenCompraInvitacionDet";
        public static string ARtblOrdenCompraRequisicionDet = "ARtblOrdenCompraRequisicionDet";
        public static string ARtblProveedorProspecto = "ARtblProveedorProspecto";
        public static string ARtblRequisicionMaterial = "ARtblRequisicionMaterial";
        public static string ARtblRequisicionMaterialDetalle = "ARtblRequisicionMaterialDetalle";
        public static string ARtblTransferencia = "ARtblTransferencia";
        public static string ARtblTransferenciaMovto = "ARtblTransferenciaMovto";

        public static string GRtblAlerta = "GRtblAlerta";
        public static string GRtblAlertaAutorizacion = "GRtblAlertaAutorizacion";
        public static string GRtblAlertaConfiguracion = "GRtblAlertaConfiguracion";
        public static string GRtblAlertaDefinicion = "GRtblAlertaDefinicion";
        public static string GRtblAlertaEtapaAccion = "GRtblAlertaEtapaAccion";
        public static string GRtblAlertaNotificacion = "GRtblAlertaNotificacion";
        public static string GRtblArchivo = "GRtblArchivo";
        public static string GRtblArchivoEstructuraCarpeta = "GRtblArchivoEstructuraCarpeta";
        public static string GRtblAutonumerico = "GRtblAutonumerico";
        public static string GRtblControlMaestro = "GRtblControlMaestro";
        public static string GRtblControlMaestroOrigenArchivo = "GRtblControlMaestroOrigenArchivo";
        public static string GRtblMenuPrincipal = "GRtblMenuPrincipal";
        public static string GRtblPermisoFicha = "GRtblPermisoFicha";
        public static string GRtblRol = "GRtblRol";
        public static string GRtblRolMenu = "GRtblRolMenu";
        public static string GRtblTablasSistema = "GRtblTablasSistema";
        public static string GRtblUsarioPermiso = "GRtblUsarioPermiso";
        public static string GRtblUsuario = "GRtblUsuario";

        public static string IGtblBitacoraAPI = "IGtblBitacoraAPI";

        public static string MItblControlMaestroControlPeriodo = "MItblControlMaestroControlPeriodo";
        public static string MItblControlMaestroDimension = "MItblControlMaestroDimension";
        public static string MItblControlMaestroDimensionNivel = "MItblControlMaestroDimensionNivel";
        public static string MItblControlMaestroFrecuenciaMedicion = "MItblControlMaestroFrecuenciaMedicion";
        public static string MItblControlMaestroFrecuenciaMedicionNivel = "MItblControlMaestroFrecuenciaMedicionNivel";
        public static string MItblControlMaestroTipoIndicador = "MItblControlMaestroTipoIndicador";
        public static string MItblControlMaestroTipoIndicadorNivel = "MItblControlMaestroTipoIndicadorNivel";
        public static string MItblControlMaestroUnidadMedida = "MItblControlMaestroUnidadMedida";
        public static string MItblControlMaestroUnidadMedidaDimension = "MItblControlMaestroUnidadMedidaDimension";
        public static string MItblControlMaestroUnidadMedidaFormulaVariable = "MItblControlMaestroUnidadMedidaFormulaVariable";
        public static string MItblMatrizConfiguracionPresupuestal = "MItblMatrizConfiguracionPresupuestal";
        public static string MItblMatrizConfiguracionPresupuestalDetalle = "MItblMatrizConfiguracionPresupuestalDetalle";
        public static string MItblMatrizConfiguracionPresupuestalSeguimientoVariable = "MItblMatrizConfiguracionPresupuestalSeguimientoVariable";
        public static string MItblMatrizIndicadorResultado = "MItblMatrizIndicadorResultado";
        public static string MItblMatrizIndicadorResultadoIndicador = "MItblMatrizIndicadorResultadoIndicador";
        public static string MItblMatrizIndicadorResultadoIndicadorFormulaVariable = "MItblMatrizIndicadorResultadoIndicadorFormulaVariable";
        public static string MItblMatrizIndicadorResultadoIndicadorMeta = "MItblMatrizIndicadorResultadoIndicadorMeta";
        public static string MItblPlanDesarrollo = "MItblPlanDesarrollo";
        public static string MItblPlanDesarrolloEstructura = "MItblPlanDesarrolloEstructura";

        public static string RHtblEmpleado = "RHtblEmpleado";

        public static string VTtblCompraViatico = "VTtblCompraViatico";
        public static string VTtblComprobacionDeGasto = "VTtblComprobacionDeGasto";
        public static string VTtblComprobacionDeGastoDet = "VTtblComprobacionDeGastoDet";
        public static string VTtblCuentaAsignadaRMPago = "VTtblCuentaAsignadaRMPago";
        public static string VTtblGastoPorComprobar = "VTtblGastoPorComprobar";
        public static string VTtblTarifaImpuestoIva = "VTtblTarifaImpuestoIva";
        public static string VTtblViatico = "VTtblViatico";
        public static string VTtblViaticoDet = "VTtblViaticoDet";

        public static string tblCompra = "tblCompra";
        public static string tblCompraDet = "tblCompraDet";
        public static string tblOrdenCompra = "tblOrdenCompra";
        public static string tblOrdenCompraDet = "tblOrdenCompraDet";
        public static string tblPoliza = "tblPoliza";
        public static string tblPolizaDet = "tblPolizaDet";
        public static string tblPolizaRef = "tblPolizaRef";
    }
}