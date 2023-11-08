ALTER TABLE tblProducto ADD StockMinimo FLOAT NULL, StockMaximo FLOAT NULL
GO

UPDATE tblProducto SET StockMinimo = 1, StockMaximo = 1
GO

ALTER TABLE tblProducto ALTER COLUMN StockMinimo FLOAT NOT NULL
GO

ALTER TABLE tblProducto ALTER COLUMN StockMaximo FLOAT NOT NULL
GO