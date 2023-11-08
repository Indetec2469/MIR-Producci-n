EXEC sp_rename 'MItblMatrizIndicadorResultadoIndicador.DefinicionIndicadro', 'DefinicionIndicador', 'COLUMN';
GO
ALTER TABLE MItblMatrizIndicadorResultadoIndicador ALTER COLUMN DescripcionVariable2 varchar(200) null
GO
ALTER TABLE MItblMatrizIndicadorResultadoIndicador ALTER COLUMN DescripcionVariable3 varchar(200) null
GO
ALTER TABLE MItblMatrizIndicadorResultadoIndicador ALTER COLUMN DescripcionVariable4 varchar(200) null
GO