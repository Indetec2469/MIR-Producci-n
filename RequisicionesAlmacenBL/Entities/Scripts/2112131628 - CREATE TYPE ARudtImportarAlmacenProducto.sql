DROP TYPE IF EXISTS dbo.ARudtImportarAlmacenProducto
GO

CREATE TYPE dbo.ARudtImportarAlmacenProducto AS TABLE(
    Cantidad DECIMAL(28, 10) NOT NULL,
	ProductoId VARCHAR (10) NOT NULL,
	CostoUnitario MONEY NULL,
	AlmacenId VARCHAR (4) NOT NULL,
	FuenteFinanciamientoId VARCHAR (6) NOT NULL,
	ProyectoId VARCHAR (6) NOT NULL,
	UnidadAdministrativaId VARCHAR (6) NOT NULL,
	TipoGastoId	VARCHAR (1) NOT NULL
)
GO