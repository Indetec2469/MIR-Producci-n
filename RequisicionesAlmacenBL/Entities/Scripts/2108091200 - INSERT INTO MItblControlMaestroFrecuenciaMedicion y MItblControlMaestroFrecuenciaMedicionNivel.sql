DELETE FROM MItblControlMaestroFrecuenciaMedicionNivel
GO

DELETE FROM MItblControlMaestroFrecuenciaMedicion
GO

SET IDENTITY_INSERT MItblControlMaestroFrecuenciaMedicion ON
INSERT INTO MItblControlMaestroFrecuenciaMedicion(FrecuenciaMedicionId, Descripcion, Borrado, FechaCreacion, CreadoPorId)
VALUES (1, 'Menusal', 0, GETDATE(), 2),
(2, 'Trimestral', 0, GETDATE(), 2),
(3, 'Semestral', 0, GETDATE(), 2),
(4, 'Anual', 0, GETDATE(), 2),
(5, 'Bianual', 0, GETDATE(), 2),
(6, 'Trianual', 0, GETDATE(), 2),
(7, 'Sexenal', 0, GETDATE(), 2)
SET IDENTITY_INSERT MItblControlMaestroFrecuenciaMedicion OFF
GO

SET IDENTITY_INSERT MItblControlMaestroFrecuenciaMedicionNivel ON
INSERT INTO MItblControlMaestroFrecuenciaMedicionNivel(FrecuenciaMedicionNivelId, FrecuenciaMedicionId, NivelId, Borrado, FechaCreacion, CreadoPorId)
VALUES (1, 4, 40, 0, GETDATE(), 2),
(2, 5, 40, 0, GETDATE(), 2),
(3, 6, 40, 0, GETDATE(), 2),
(4, 7, 40, 0, GETDATE(), 2),
(5, 3, 41, 0, GETDATE(), 2),
(6, 4, 41, 0, GETDATE(), 2),
(7, 5, 41, 0, GETDATE(), 2),
(8, 6, 41, 0, GETDATE(), 2),
(9, 2, 42, 0, GETDATE(), 2),
(10, 3, 42, 0, GETDATE(), 2),
(11, 4, 42, 0, GETDATE(), 2),
(12, 1, 43, 0, GETDATE(), 2),
(13, 2, 43, 0, GETDATE(), 2),
(14, 3, 43, 0, GETDATE(), 2)
SET IDENTITY_INSERT MItblControlMaestroFrecuenciaMedicionNivel OFF
GO