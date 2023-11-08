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
(11, 'TipoArchivo', 'Documento de texto', 1, 1, 0, NULL, GETDATE(), 1),
(12, 'TipoArchivo', 'Hoja de cálculo', 1, 1, 0, NULL, GETDATE(), 1),
(13, 'TipoArchivo', 'PDF', 1, 1, 0, NULL, GETDATE(), 1),
(14, 'TipoArchivo', 'XML', 1, 1, 0, NULL, GETDATE(), 1),
(15, 'TipoArchivo', 'Imagen', 1, 1, 0, NULL, GETDATE(), 1),
(16, 'TipoArchivo', 'Otro', 1, 1, 0, NULL, GETDATE(), 1),
(17, 'TipoCalculoNombreArchivo', 'Sencillo', 1, 1, 0, NULL, GETDATE(), 1),
(18, 'RutaTemporalArchivo', 'C:/PIXVS Archivos/REQUISICIONES/tmp', 1, 1, 1, NULL, GETDATE(), 1),
(19, 'RutaRaizArchivo', 'C:/PIXVS Archivos/REQUISICIONES/raiz', 1, 1, 1, NULL, GETDATE(), 1),
(20, 'CantidadCerosAutonumericoArchivo', '12', 1, 1, 1, NULL, GETDATE(), 1),
(21, 'IdAutonumericoArchivo', '200', 1, 1, 1, NULL, GETDATE(), 1)
GO

SET IDENTITY_INSERT tblControlMaestro OFF
GO