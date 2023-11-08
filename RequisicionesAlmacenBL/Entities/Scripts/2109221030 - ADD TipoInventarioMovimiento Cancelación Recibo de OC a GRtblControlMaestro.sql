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
(96, 'TipoInventarioMovimiento', 'Cancelación Recibo de OC', 1, 1, 0, GETDATE())
SET IDENTITY_INSERT GRtblControlMaestro OFF