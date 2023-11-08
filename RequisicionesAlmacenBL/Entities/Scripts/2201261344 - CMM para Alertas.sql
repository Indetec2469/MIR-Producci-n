SET IDENTITY_INSERT GRtblControlMaestro ON
INSERT INTO GRtblControlMaestro
(
    ControlId,
    Control,
    Valor,
    Sistema,
    Activo,
    ControlSencillo,
    FechaCreacion
)
VALUES
(112, 'AlertaAccion', 'Iniciar Alerta', 1, 1, 0, GETDATE()),
(113, 'AlertaAccion', 'Autorizar Alerta', 1, 1, 0, GETDATE()),
(114, 'AlertaAccion', 'Revisión Alerta', 1, 1, 0, GETDATE()),
(115, 'AlertaAccion', 'Rechazar Alerta', 1, 1, 0, GETDATE()),
(116, 'AlertaAccion', 'Cancelar Alerta', 1, 1, 0, GETDATE()),
(117, 'AlertaAccion', 'Ocultar Alertas', 1, 1, 0, GETDATE()),

(118, 'EstatusAlerta', 'En Proceso', 1, 1, 0, GETDATE()),
(119, 'EstatusAlerta', 'Rechazada', 1, 1, 0, GETDATE()),
(120, 'EstatusAlerta', 'En Revisión', 1, 1, 0, GETDATE()),
(121, 'EstatusAlerta', 'Finalizada', 1, 1, 0, GETDATE()),

(122, 'TipoNotificacionAlerta', 'Autorización', 1, 1, 0, GETDATE()),
(123, 'TipoNotificacionAlerta', 'Notificación', 1, 1, 0, GETDATE()),

(124, 'AlertaConfiguracionFigura', 'Jefe inmediato', 1, 1, 0, GETDATE()),
(125, 'AlertaConfiguracionFigura', 'Solicitante', 1, 1, 0, GETDATE())

SET IDENTITY_INSERT GRtblControlMaestro OFF