UPDATE movimiento SET movimiento.FechaCreacion = DATEADD(DAY, MONTH(movimiento.FechaCreacion) - DAY(movimiento.FechaCreacion), DATEADD(MONTH, (DAY(movimiento.FechaCreacion) - MONTH(movimiento.FechaCreacion)), movimiento.FechaCreacion))
FROM ARtblInventarioMovimientoAgrupador AS movimiento
WHERE movimiento.FechaCreacion > GETDATE()
GO

UPDATE movimiento SET movimiento.FechaCreacion = DATEADD(DAY, MONTH(movimiento.FechaCreacion) - DAY(movimiento.FechaCreacion), DATEADD(MONTH, (DAY(movimiento.FechaCreacion) - MONTH(movimiento.FechaCreacion)), movimiento.FechaCreacion))
FROM ARtblInventarioMovimiento AS movimiento
WHERE movimiento.FechaCreacion > GETDATE()
GO