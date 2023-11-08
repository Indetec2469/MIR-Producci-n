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
(99, 'AREstatusInvitacionArticulo', 'Activa', 1, 1, 0, GETDATE()),
(100, 'AREstatusInvitacionArticulo', 'Cancelada', 1, 1, 0, GETDATE()),
(101, 'AREstatusInvitacionArticuloDetalle', 'Por Invitar', 1, 1, 0, GETDATE()),
(102, 'AREstatusInvitacionArticuloDetalle', 'Invitado', 1, 1, 0, GETDATE()),
(103, 'AREstatusInvitacionArticuloDetalle', 'Cancelado', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF