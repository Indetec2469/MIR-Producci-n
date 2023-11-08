SET IDENTITY_INSERT GRtblArchivoEstructuraCarpeta ON
GO

INSERT INTO GRtblArchivoEstructuraCarpeta(
	EstructuraCarpetaId,
	NombreCarpeta,
	DescripcionCorta,
    OrigenArchivoId,
	Vigente,
	FechaCreacion,
	CreadoPorId
) VALUES
(7, 'Logotipos', 'Carpeta raiz de Logotipos', 3, 1, GETDATE(), 1)
GO

SET IDENTITY_INSERT GRtblArchivoEstructuraCarpeta OFF
GO