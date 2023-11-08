ALTER TABLE ARtblInventarioMovimiento
ADD CostoUnitario MONEY NULL,
    CostoPromedio MONEY NULL
GO

UPDATE im SET im.CostoUnitario = p.CostoPromedio,
       im.CostoPromedio = p.CostoPromedio
FROM ARtblInventarioMovimiento AS im
     INNER JOIN ARtblAlmacenProducto AS ap ON im.AlmacenProductoId = ap.AlmacenProductoId
     INNER JOIN tblProducto AS p ON ap.ProductoId = p.ProductoId
GO

ALTER TABLE ARtblInventarioMovimiento
ALTER COLUMN CostoUnitario MONEY NOT NULL
GO

ALTER TABLE ARtblInventarioMovimiento
ALTER COLUMN CostoPromedio MONEY NOT NULL
GO

ALTER TABLE ARtblInventarioMovimiento
DROP COLUMN ValorContableAntesMovto
GO

ALTER TABLE ARtblInventarioMovimientoAgrupador
ADD PolizaId INT NULL
GO

ALTER TABLE ARtblInventarioMovimientoAgrupador WITH CHECK ADD  CONSTRAINT FK_ARtblInventarioMovimientoAgrupador_tblPoliza FOREIGN KEY(PolizaId)
REFERENCES tblPoliza (PolizaId)
GO

ALTER TABLE ARtblInventarioMovimientoAgrupador CHECK CONSTRAINT FK_ARtblInventarioMovimientoAgrupador_tblPoliza
GO