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
(110, 'TipoCostoArticulo', 'Promedio', 1, 1, 0, GETDATE()),
(111, 'TipoCostoArticulo', 'Último', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF