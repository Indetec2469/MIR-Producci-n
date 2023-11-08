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
(137, 'GRTipoConfiguracionFecha', 'Fecha de Operación', 1, 1, 0, GETDATE()),
(138, 'GRTipoConfiguracionFecha', 'Fecha Sistema', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF
GO