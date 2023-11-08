DROP PROCEDURE IF EXISTS ARspProcesadorInventarios
GO

DROP TYPE IF EXISTS dbo.ARudtInventarioMovimiento
GO

CREATE TYPE dbo.ARudtInventarioMovimiento AS TABLE(
    AlmacenProductoId INT NOT NULL,
	CantidadMovimiento DECIMAL(28, 10) NOT NULL,
	CostoUnitario MONEY NOT NULL,
	ReferenciaMovtoId INT NOT NULL,

    PRIMARY KEY (ReferenciaMovtoId)
)
GO