
update tblProducto set StockMinimo = 0, StockMaximo = 0 where StockMinimo is null or StockMaximo is null
GO

alter table tblProducto alter column StockMinimo float not null
alter table tblProducto alter column StockMaximo float not null
GO

