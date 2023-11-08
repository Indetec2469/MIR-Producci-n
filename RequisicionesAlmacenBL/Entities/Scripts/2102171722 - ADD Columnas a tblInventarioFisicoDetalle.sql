ALTER TABLE tblInventarioFisicoDetalle 
	ADD ProyectoId VARCHAR(6) NOT NULL,
			Proyecto VARCHAR(250) NOT NULL,
			FuenteFinanciamientoId VARCHAR(6) NOT NULL, 
			FuenteFinanciamiento VARCHAR(250) NOT NULL,
			UnidadAdministrativaId VARCHAR(6) NOT NULL, 
			UnidadAdministrativa VARCHAR(250) NOT NULL,
			TipoGastoId VARCHAR(6) NOT NULL, 
			TipoGasto VARCHAR(1000) NOT NULL,
			ProductoId VARCHAR(10) NOT NULL,
			Producto VARCHAR(250) NOT NULL,
			UnidadDeMedidaId INT NOT NULL,
			UnidadDeMedida VARCHAR(100) NOT NULL
GO

ALTER TABLE tblInventarioFisicoDetalle 
	ALTER COLUMN Conteo DECIMAL(28, 10) NULL
GO