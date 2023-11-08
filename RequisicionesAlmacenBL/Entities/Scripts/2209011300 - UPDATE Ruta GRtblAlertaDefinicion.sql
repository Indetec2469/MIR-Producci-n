UPDATE GRtblAlertaDefinicion SET RutaAccion = '/' + RutaAccion
GO

UPDATE GRtblControlMaestro SET Valor = 'Requisición Material. Validación' WHERE ControlId = 127
GO

UPDATE GRtblControlMaestro SET Valor = 'Revisión' WHERE ControlId = 131
GO