class ControlMaestroMapeo {
    static EstatusRegistro = {
        ACTIVO: 1,
        INACTIVO: 2,
        BORRADO: 3
    }

    static TipoCompra = {
        COMPRA_DIRECTA: 4,
        INVITACION_COMPRA: 5,
        LICITACION: 132
    }

    static AREstatusRequisicion = {
        AUTORIZADA: 64,
        CANCELADA: 65,
        CERRADA: 66,
        EN_ALMACEN: 67,
        EN_PROCESO: 68,
        ENVIADA: 69,
        FINALIZADA: 70,
        GUARDADA: 71,
        ORDEN_COMPRA: 72,
        POR_COMPRAR: 73,
        RECHAZADA: 74,
        REQUISICION_COMPRA: 75,
        REVISION: 76
    }

    static AREstatusRequisicionDetalle = {
        ACTIVO: 77,
        CANCELADO: 78,
        CERRADO: 79,
        EN_ALMACEN: 80,
        ENVIADO: 81,
        MODIFICADO: 82,
        POR_COMPRAR: 83,
        POR_SURTIR: 84,
        RECHAZADO: 85,
        RELACIONADO_OC: 86,
        RELACIONADO_RC: 87,
        REVISION: 88,
        SURTIDO: 89,
        SURTIDO_PARCIAL: 90
    }

    static Nivel = {
        FIN: 40,
        PROPOSITO: 41,
        COMPONENTE: 42,
        ACTIVIDAD: 43,
        CALIDAD: 44
    }

    static FrecuenciaMedicion = {
        MENSUAL: 45,
        TRIMESTRAL: 46,
        SEMESTRAL: 47,
        ANUAL: 48,
        BIANUAL: 49,
        TRIANUAL: 50,
        SEXENAL: 51
    }

    static Sentido = {
        ASCENDENTE: 52,
        DESCENDENTE: 53
    }

    static TipoComponente = {
        RELACION_ACTIVIDAD: 54,
        RELACION_COMPONENTE: 55
    }

    static TipoPresupuesto = {
        POR_EJERCER: 56,
        DEVENGADO: 57
    }

    static MIEstatusPeriodo = {
        ABIERTO: 58,
        CERRADO: 59,
        AUDITADO: 60
    }

    static EstatusOrdenCompra = {
        ACTIVA: "A",
        RECIBO_PARCIAL: "I",
        RECIBIDA: "R",
        CANCELADA: "C"
    }

    static TipoNotificacionAlerta = {
        AUTORIZACION: 122,
        NOTIFICACION: 123
    }

    static GRTipoConfiguracionFecha = {
        FECHA_OPERACION: 137,
        FECHA_SISTEMA: 138,
    }
}