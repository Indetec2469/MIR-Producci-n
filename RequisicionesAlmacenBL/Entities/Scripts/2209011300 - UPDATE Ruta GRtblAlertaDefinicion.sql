UPDATE GRtblAlertaDefinicion SET RutaAccion = '/' + RutaAccion
GO

UPDATE GRtblControlMaestro SET Valor = 'Requisici�n Material. Validaci�n' WHERE ControlId = 127
GO

UPDATE GRtblControlMaestro SET Valor = 'Revisi�n' WHERE ControlId = 131
GO