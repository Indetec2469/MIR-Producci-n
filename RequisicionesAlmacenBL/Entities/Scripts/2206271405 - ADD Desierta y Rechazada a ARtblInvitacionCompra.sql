ALTER TABLE ARtblInvitacionCompra ADD FechaDesierta DATETIME
GO

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
(133, 'AREstatusInvitacionCompra', 'Desierta', 1, 1, 0, GETDATE()),
(134, 'AREstatusInvitacionCompra', 'Rechazada', 1, 1, 0, GETDATE()),
(135, 'AREstatusInvitacionCompraDetalle', 'Desierto', 1, 1, 0, GETDATE()),
(136, 'AREstatusInvitacionCompraDetalle', 'Rechazado', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF
GO