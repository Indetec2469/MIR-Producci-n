using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Models.Mapeos
{
    public class ControlMaestroMapeo
    {
        public static class EstatusRegistro
        {
            public static int ACTIVO = 1;
            public static int INACTIVO = 2;
            public static int BORRADO = 3;
        }

        public static class TipoCompra
        {
            public static int COMPRA_DIRECTA = 4;
            public static int INVITACION_COMPRA = 5;
            public static int LICITACION = 132;
        }

        public static class TipoMovimiento
        {
            public static int INCREMENTA = 9;
            public static int DISMINUYE = 10;
        }

        public static class TipoRutaArchivo
        {
            public static int RUTA_TEMPORAL_ARCHIVO = 18;
            public static int RUTA_RAIZ_ARCHIVO = 19;
        }

        public static class TipoArchivo
        {
            public static int DOCUMENTOTEXTO = 11;
            public static int HOJACALCULO = 12;
            public static int PDF = 13;
            public static int XML = 14;
            public static int IMAGEN = 15;
            public static int OTRO = 16;
        }

        public static class ListadoCMOA
        {
            public static int FOTOGRAFIA_EMPLEADO = 1;
            public static int EVIDENCIA_AJUSTE_INVENTARIO = 2;
            public static int INVITACION_COMPRA_PROVEEDOR_COTIZACION = 4;
        }

        public static class AREstatusRequisicion
        {
            public static int AUTORIZADA = 64;
            public static int CANCELADA = 65;
            public static int CERRADA = 66;
            public static int EN_ALMACEN = 67;
            public static int EN_PROCESO = 68;
            public static int ENVIADA = 69;
            public static int FINALIZADA = 70;
            public static int GUARDADA = 71;
            public static int ORDEN_COMPRA = 72;
            public static int POR_COMPRAR = 73;
            public static int RECHAZADA = 74;
            public static int REQUISICION_COMPRA = 75;
            public static int REVISION = 76;

            public static Dictionary<int, string> Nombre = new Dictionary<int, string>()
            {
                { AUTORIZADA, "Autorizada" },
                { CANCELADA, "Cancelada" },
                { CERRADA, "Cerrada" },
                { EN_ALMACEN, "En Almacén" },
                { EN_PROCESO, "En Proceso" },
                { ENVIADA, "Enviada" },
                { FINALIZADA, "Finalizada" },
                { GUARDADA, "Guardada" },
                { ORDEN_COMPRA, "Orden Compra" },
                { POR_COMPRAR, "Por Comprar" },
                { RECHAZADA, "Rechazada" },
                { REQUISICION_COMPRA, "Requisición Compra" },
                { REVISION, "Revisión" }
            };
        }

        public static class AREstatusRequisicionDetalle
        {
            public static int ACTIVO = 77;
            public static int CANCELADO = 78;
            public static int CERRADO = 79;
            public static int EN_ALMACEN = 80;
            public static int ENVIADO = 81;
            public static int MODIFICADO = 82;
            public static int POR_COMPRAR = 83;
            public static int POR_INVITAR = 97;
            public static int POR_SURTIR = 84;
            public static int RECHAZADO = 85;
            public static int RELACIONADO_OC = 86;
            public static int RELACIONADO_RC = 87;
            public static int REVISION = 88;
            public static int SURTIDO = 89;
            public static int SURTIDO_PARCIAL = 90;
        }

        public static class AREstatusInvitacionCompra
        {
            public static int CANCELADA = 91;
            public static int GUARDADA = 92;
            public static int CONVERTIDA_PARCIALMENTE = 105;
            public static int FINALIZADA = 106;
            public static int DESIERTA = 133;
            public static int RECHAZADA = 134;

            public static Dictionary<int, string> Nombre = new Dictionary<int, string>()
            {
                { CANCELADA, "Cancelada" },
                { GUARDADA, "Guardada" },
                { CONVERTIDA_PARCIALMENTE, "Convertida Parcialmente" },
                { FINALIZADA, "Finalizada" },
                { DESIERTA, "Desierta" },
                { RECHAZADA, "Rechazada" }
            };
        }

        public static class AREstatusInvitacionCompraDetalle
        {
            public static int ACTIVO = 93;
            public static int CANCELADO = 94;
            public static int CONVERTIDO_OC = 104;
            public static int DESIERTO = 135;
            public static int RECHAZADO = 136;
        }

        public static class AREstatusInvitacionArticulo
        {
            public static int ACTIVA = 99;
            public static int CANCELADA = 100;
        }

        public static class AREstatusInvitacionArticuloDetalle
        {
            public static int POR_INVITAR = 101;
            public static int INVITADO = 102;
            public static int CANCELADO = 103;
        }

        public static class EstatusInventarioFisico
        {
            public static int EN_PROCESO = 32;
            public static int TERMINADO = 33;
            public static int CANCELADO = 34;
        }

        public static class NivelGobierno
        {
            public static int FEDERAL = 37;
            public static int ESTATAL = 38;
            public static int MUNICIPAL = 39;
        }

        public static class Nivel
        {
            public static int FIN = 40;
            public static int PROPOSITO = 41;
            public static int COMPONENTE = 42;
            public static int ACTIVIDAD = 43;
            public static int CALIDAD = 44;
        }

        public static class TipoComponente
        {
            public static int RELACION_ACTIVIDAD = 54;
            public static int RELACION_COMPONENTE = 55;
        }

        public static class TipoPresupuesto
        {
            public static int POR_EJERCER = 56;
            public static int DEVENGADO = 57;
        }

        public static class MIEstatusPeriodo
        {
            public static int ABIERTO = 58;
            public static int CERRADO = 59;
            public static int AUDITADO = 60;

        }

        public static class TipoInventarioMovimiento
        {
            public static int INVENTARIO_FISICO = 35;
            public static int INVENTARIO_AJUSTE = 36;
            public static int REQUISICION_MATERIAL_SURTIMIENTO = 63;
            public static int ORDEN_COMPRA_RECIBO = 95;
            public static int CANCELACION_RECIBO_OC = 96;
            public static int RECIBO_CORTESIA = 107;
        }

        public static class EstatusOrdenCompra
        {
            public static string ACTIVA = "A";
            public static string RECIBO_PARCIAL = "I";
            public static string RECIBIDA = "R";
            public static string CANCELADA = "C";

            public static Dictionary<string, string> Nombre = new Dictionary<string, string>()
            {
                { ACTIVA, "Activa" },
                { RECIBO_PARCIAL, "Recibo Parcial" },
                { RECIBIDA, "Recibida" },
                { CANCELADA, "Cancelada" }
            };
        }

        public static class EstatusOrdenCompraRecibo
        {
            public static string ACTIVO = "A";
            public static string CANCELADO = "C";
            public static string PAGADO = "P";
            public static string ORDEN_PAGO = "O";
            public static string PARCIALMENTE_PAGADO = "F";

            public static Dictionary<string, string> Nombre = new Dictionary<string, string>()
            {
                { ACTIVO, "Activo" },
                { CANCELADO, "Cancelado" },
                { PAGADO, "Pagado" },
                { ORDEN_PAGO, "Orden de Pago" },
                { PARCIALMENTE_PAGADO, "Parcialmente Pagado" }
            };
        }

        public static class AlertaAccion
        {
            public static int INICIAR = 112;
            public static int AUTORIZAR = 113;
            public static int REVISION = 114;
            public static int RECHAZAR = 115;
            public static int CANCELAR = 116;
            public static int OCULTAR = 117;
        }

        public static class GRTipoConfiguracionFecha
        {
            public static int FECHA_OPERACION = 137;
            public static int FECHA_SISTEMA = 138;
        }

        public static class TipoVersion
        {
            public static int MIR = 1;
            public static int COMPRAS = 2;
            public static int TODOS = 3;
        }
    }
}