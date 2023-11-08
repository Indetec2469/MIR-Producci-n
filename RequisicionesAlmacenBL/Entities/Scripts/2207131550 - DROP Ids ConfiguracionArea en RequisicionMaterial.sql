ALTER TABLE ARtblRequisicionMaterial DROP CONSTRAINT IF EXISTS FK_ConfiguracionAreaId
GO

ALTER TABLE ARtblRequisicionMaterialDetalle DROP CONSTRAINT IF EXISTS FK_ConfiguracionAreaAlmacenId
GO

ALTER TABLE ARtblRequisicionMaterialDetalle DROP CONSTRAINT IF EXISTS FK_ConfiguracionAreaProyectoId
GO

ALTER TABLE ARtblRequisicionMaterial DROP COLUMN IF EXISTS ConfiguracionAreaId
GO

ALTER TABLE ARtblRequisicionMaterialDetalle DROP COLUMN IF EXISTS ConfiguracionAreaAlmacenId
GO

ALTER TABLE ARtblRequisicionMaterialDetalle DROP COLUMN IF EXISTS ConfiguracionAreaProyectoId
GO