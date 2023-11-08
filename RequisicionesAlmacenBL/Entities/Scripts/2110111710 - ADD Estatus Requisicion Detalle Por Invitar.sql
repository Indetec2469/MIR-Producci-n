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
(97, 'AREstatusRequisicionDetalle', 'Por Invitar', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF