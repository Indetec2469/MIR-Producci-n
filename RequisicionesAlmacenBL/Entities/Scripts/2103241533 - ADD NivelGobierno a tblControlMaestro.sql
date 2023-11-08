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
(37, 'NivelGobierno', 'Federal', 1, 1, 0, NULL, GETDATE(), 1),
(38, 'NivelGobierno', 'Estatal', 1, 1, 0, NULL, GETDATE(), 1),
(39, 'NivelGobierno', 'Municipal', 1, 1, 0, NULL, GETDATE(), 1)
GO

SET IDENTITY_INSERT tblControlMaestro OFF
GO