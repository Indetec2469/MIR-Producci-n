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
(109, 'TipoInventarioMovimiento', 'Carga Inventario Inicial', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF