DROP TYPE IF EXISTS ARudtInventarioMovimiento
GO

CREATE TYPE ARudtInventarioMovimiento AS TABLE 
(
	AlmacenProductoId INT,
	CantidadMovimiento DECIMAL(28, 10),
	ReferenciaMovtoId INT,

    PRIMARY KEY (ReferenciaMovtoId)
)
GO