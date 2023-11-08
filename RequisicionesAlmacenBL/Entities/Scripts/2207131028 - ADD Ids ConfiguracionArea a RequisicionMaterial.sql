ALTER TABLE ARtblRequisicionMaterial ADD ConfiguracionAreaId INT
GO

UPDATE requisicion SET requisicion.ConfiguracionAreaId = ca.ConfiguracionAreaId
FROM ARtblRequisicionMaterial AS requisicion
     INNER JOIN ARtblControlMaestroConfiguracionArea AS ca ON requisicion.AreaId = ca.AreaId
GO

ALTER TABLE ARtblRequisicionMaterial ALTER COLUMN ConfiguracionAreaId INT NOT NULL
GO

ALTER TABLE ARtblRequisicionMaterial  WITH CHECK ADD  CONSTRAINT FK_ConfiguracionAreaId FOREIGN KEY(ConfiguracionAreaId)
REFERENCES ARtblControlMaestroConfiguracionArea (ConfiguracionAreaId)
GO

ALTER TABLE ARtblRequisicionMaterial CHECK CONSTRAINT FK_ConfiguracionAreaId
GO

ALTER TABLE ARtblRequisicionMaterialDetalle ADD ConfiguracionAreaAlmacenId INT
GO

ALTER TABLE ARtblRequisicionMaterialDetalle ADD ConfiguracionAreaProyectoId INT
GO

UPDATE detalle SET detalle.ConfiguracionAreaAlmacenId = caAlmacen.ConfiguracionAreaAlmacenId
FROM ARtblRequisicionMaterial AS requisicion
     INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId
     INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen AS caAlmacen ON detalle.AlmacenId = caAlmacen.AlmacenId AND requisicion.ConfiguracionAreaId = caAlmacen.ConfiguracionAreaId AND caAlmacen.Borrado = 0
GO

UPDATE detalle SET detalle.ConfiguracionAreaAlmacenId = caAlmacen.ConfiguracionAreaAlmacenId
FROM ARtblRequisicionMaterial AS requisicion
     INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId AND detalle.ConfiguracionAreaAlmacenId IS NULL
     INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen AS caAlmacen ON detalle.AlmacenId = caAlmacen.AlmacenId AND requisicion.ConfiguracionAreaId = caAlmacen.ConfiguracionAreaId AND caAlmacen.Borrado = 1
GO

UPDATE detalle SET detalle.ConfiguracionAreaProyectoId = caProyecto.ConfiguracionAreaProyectoId
FROM ARtblRequisicionMaterial AS requisicion
     INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId
     INNER JOIN tblProyecto_Dependencia AS pd ON detalle.UnidadAdministrativaId = DependenciaId AND detalle.ProyectoId = pd.ProyectoId
	 INNER JOIN ARtblControlMaestroConfiguracionAreaProyecto AS caProyecto ON pd.idPD = caProyecto.ProyectoDependenciaId AND requisicion.ConfiguracionAreaId = caProyecto.ConfiguracionAreaId AND caProyecto.Borrado = 0
GO

UPDATE detalle SET detalle.ConfiguracionAreaProyectoId = caProyecto.ConfiguracionAreaProyectoId
FROM ARtblRequisicionMaterial AS requisicion
     INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId
     INNER JOIN tblProyecto_Dependencia AS pd ON detalle.UnidadAdministrativaId = DependenciaId AND detalle.ProyectoId = pd.ProyectoId AND detalle.ConfiguracionAreaProyectoId IS NULL 
	 INNER JOIN ARtblControlMaestroConfiguracionAreaProyecto AS caProyecto ON pd.idPD = caProyecto.ProyectoDependenciaId AND requisicion.ConfiguracionAreaId = caProyecto.ConfiguracionAreaId AND caProyecto.Borrado = 1
GO

ALTER TABLE ARtblRequisicionMaterialDetalle ALTER COLUMN ConfiguracionAreaAlmacenId INT NOT NULL
GO

ALTER TABLE ARtblRequisicionMaterialDetalle ALTER COLUMN ConfiguracionAreaProyectoId INT NOT NULL
GO

ALTER TABLE ARtblRequisicionMaterialDetalle  WITH CHECK ADD  CONSTRAINT FK_ConfiguracionAreaAlmacenId FOREIGN KEY(ConfiguracionAreaAlmacenId)
REFERENCES ARtblControlMaestroConfiguracionAreaAlmacen (ConfiguracionAreaAlmacenId)
GO

ALTER TABLE ARtblRequisicionMaterialDetalle CHECK CONSTRAINT FK_ConfiguracionAreaAlmacenId
GO

ALTER TABLE ARtblRequisicionMaterialDetalle  WITH CHECK ADD  CONSTRAINT FK_ConfiguracionAreaProyectoId FOREIGN KEY(ConfiguracionAreaProyectoId)
REFERENCES ARtblControlMaestroConfiguracionAreaProyecto (ConfiguracionAreaProyectoId)
GO

ALTER TABLE ARtblRequisicionMaterialDetalle CHECK CONSTRAINT FK_ConfiguracionAreaProyectoId
GO