SET IDENTITY_INSERT tblListadoCMOA ON
GO

INSERT INTO tblListadoCMOA (
	ControlOrigenArchivoId,
	Descripcion,
	Vigente,
	FechaCreacion,
	CreadoPorId
)
VALUES
(2, 'Evidencia de Ajuste de Inventario', 1, GETDATE(), 1)
GO

SET IDENTITY_INSERT tblListadoCMOA OFF
GO


SET IDENTITY_INSERT tblArchivoEstructuraCarpeta ON
GO

INSERT INTO tblArchivoEstructuraCarpeta (
	EstructuraId,
	NombreCarpeta,
	DescripcionCorta,
	EstructuraReferenciaId,
	OrigenArchivoId,
	NombreCalculado,
	TipoCalculo,
	Vigente,
	FechaCreacion,
	CreadoPorId
)
VALUES
(4, 'Ajustes de Inventarios', 'Carpeta raiz de Ajustes de Inventarios', NULL, NULL, 0, NULL, 1, GETDATE(), 1),
(5, NULL, 'Carpeta del Ajuste de Inventario', 4, NULL, 1, 17, 1, GETDATE(), 1),
(6, 'Evidencias', 'Evidencias del Ajuste de Inventario', 5, 2, 0, NULL, 1, GETDATE(), 1)
GO

SET IDENTITY_INSERT tblArchivoEstructuraCarpeta OFF
GO