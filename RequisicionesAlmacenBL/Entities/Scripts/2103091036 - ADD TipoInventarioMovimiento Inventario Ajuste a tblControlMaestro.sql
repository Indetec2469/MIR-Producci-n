SET IDENTITY_INSERT tblControlMaestro ON
GO

INSERT INTO tblControlMaestro(
	ControlId,
	Control,
	Valor,
    Sistema,
	Activo,
	ControlSencillo,
	ModuloId,
	FechaCreacion,
    CreadoPorId	
) VALUES
(36, 'TipoInventarioMovimiento', 'Inventario Ajuste', 1, 1, 0, NULL, GETDATE(), 1)
GO

SET IDENTITY_INSERT tblControlMaestro OFF
GO