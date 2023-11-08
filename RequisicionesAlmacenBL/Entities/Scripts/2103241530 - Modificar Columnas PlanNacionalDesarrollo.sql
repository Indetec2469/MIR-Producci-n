EXECUTE sp_rename 'tblPlanNacionalDesarrollo.PlanNacionalDesarolloId', 'PlanNacionalDesarrolloId', 'COLUMN';

ALTER TABLE tblPlanNacionalDesarrolloEstructura ALTER COLUMN EstructuraPadreId INT NULL
GO