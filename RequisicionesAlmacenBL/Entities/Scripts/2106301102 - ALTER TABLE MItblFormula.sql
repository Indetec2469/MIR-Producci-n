DROP TABLE dbo.MItblFormulaDimension
GO
DROP TABLE dbo.MItblFormulaVariable
GO
DROP TABLE dbo.MItblFormula
GO

ALTER TABLE MItblControlMaestroUnidadMedida
ADD Formula varchar(500) not null

EXEC sp_rename 'MItblControlMaestroUnidadMedida.Descripcion', 'Nombre', 'COLUMN';