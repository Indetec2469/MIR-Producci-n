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
(32, 'EstatusInventarioFisico', 'En Proceso', 1, 1, 0, NULL, GETDATE(), 1),
(33, 'EstatusInventarioFisico', 'Terminado', 1, 1, 0, NULL, GETDATE(), 1),
(34, 'EstatusInventarioFisico', 'Cancelado', 1, 1, 0, NULL, GETDATE(), 1),
(35, 'TipoInventarioMovimiento', 'Inventario Físico', 1, 1, 0, NULL, GETDATE(), 1)
GO

SET IDENTITY_INSERT tblControlMaestro OFF
GO