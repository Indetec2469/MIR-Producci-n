SET IDENTITY_INSERT GRtblControlMaestroOrigenArchivo ON
GO

INSERT INTO GRtblControlMaestroOrigenArchivo(
	OrigenArchivoId,
	Descripcion,
	Vigente,
    FechaCreacion,
	CreadoPorId
) VALUES
(3, 'Los imagenes de Logotipo', 1, GETDATE(), 1)
GO

SET IDENTITY_INSERT GRtblControlMaestroOrigenArchivo OFF
GO