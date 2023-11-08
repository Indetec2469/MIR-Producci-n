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
(105, 'AREstatusInvitacionCompra', 'Convertida Parcialmente', 1, 1, 0, GETDATE()),
(106, 'AREstatusInvitacionCompra', 'Finalizada', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF