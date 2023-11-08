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
(1, 'Fotografias de los empleados', 1, GETDATE(), 1)
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
(1, 'Empleados', 'Carpeta raiz de empleados', NULL, NULL, 0, NULL, 1, GETDATE(), 1),
(2, NULL, 'Carpeta del empleado', 1, NULL, 1, 17, 1, GETDATE(), 1),
(3, 'Fotografías', 'Fotografias del empleado', 2, 1, 0, NULL, 1, GETDATE(), 1)
GO

SET IDENTITY_INSERT tblArchivoEstructuraCarpeta OFF
GO