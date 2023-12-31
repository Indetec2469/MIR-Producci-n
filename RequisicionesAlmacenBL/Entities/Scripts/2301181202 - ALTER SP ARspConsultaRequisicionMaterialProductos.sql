SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[ARspConsultaRequisicionMaterialProductos]
	@areaId NVARCHAR(10),
	@unidadAdministrativaId NVARCHAR(10),
	@proyectoId NVARCHAR(10)
AS
-- ===============================================================
-- Author:		Javier Elías
-- Create date: 11/05/2021
-- Modified date: 18/01/2023
-- Description:	Procedimiento para Obtener los registros que se van a agregar
--						al combo de Productos de la Ficha Requisición Material.
-- ===============================================================
-- Creamos tablas temporales para las Dependencias y Proyectos
DECLARE @dependenciasIds TABLE (DependenciaId	VARCHAR (6))
DECLARE @proyectosIds TABLE (ProyectoId	VARCHAR (6))

IF ( @unidadAdministrativaId IS NULL )
BEGIN
		INSERT INTO @dependenciasIds
		EXEC ARspConsultaDependenciasPorAreaId @areaId
END

ELSE
BEGIN
		INSERT INTO @dependenciasIds
		SELECT DependenciaId FROM tblDependencia WHERE DependenciaId = @unidadAdministrativaId
END

IF ( @proyectoId IS NULL )
BEGIN
		INSERT INTO @proyectosIds
		EXEC ARspConsultaProyectosPorAreaId @areaId
END

ELSE
BEGIN
		INSERT INTO @proyectosIds
		SELECT ProyectoId FROM tblProyecto WHERE ProyectoId = @proyectoId
END

SELECT CONCAT(ProductoId, UnidadAdministrativaId, ProyectoId, TipoGastoId) AS ProductoDetalleId,
	   ProductoId, Descripcion, Producto, UnidadDeMedidaId, UnidadDeMedida, CostoUnitario, UnidadAdministrativaId, UnidadAdministrativa, 
	   ProyectoId, Proyecto, TipoGastoId, TipoGasto		
FROM (
	SELECT producto.ProductoId,
		   producto.Descripcion,
		   producto.ProductoId + ' - ' + producto.Descripcion AS Producto,
		   um.UnidadDeMedidaId,
		   um.Descripcion AS UnidadDeMedida,
		   CostoUltimo AS CostoUnitario,
		   unidadAdministrativa.DependenciaId AS UnidadAdministrativaId,
		   unidadAdministrativa.Nombre AS UnidadAdministrativa,
		   proyecto.ProyectoId,
		   proyecto.Nombre AS Proyecto,
		   tipoGasto.TipoGastoId,
		   tipoGasto.Nombre AS TipoGasto
	FROM tblProducto as producto
	INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	INNER JOIN tblCuentaPresupuestalEgr cuenta on producto.ObjetoGastoId = cuenta.ObjetoGastoId
	INNER JOIN tblDependencia AS unidadAdministrativa ON cuenta.DependenciaId = unidadAdministrativa.DependenciaId
	INNER JOIN tblProyecto AS proyecto ON cuenta.ProyectoId = proyecto.ProyectoId
	INNER JOIN tblTipoGasto AS tipoGasto ON cuenta.TipoGastoId = tipoGasto.TipoGastoId
	WHERE producto.Status = 'A' AND
	      unidadAdministrativa.DependenciaId IN (SELECT DependenciaId FROM @dependenciasIds) AND
		  proyecto.ProyectoId IN (SELECT ProyectoId FROM @proyectosIds)
    GROUP BY  producto.ProductoId, producto.Descripcion, um.UnidadDeMedidaId, um.Descripcion, CostoUltimo,
		   unidadAdministrativa.DependenciaId, unidadAdministrativa.Nombre, proyecto.ProyectoId,
		   proyecto.Nombre, tipoGasto.TipoGastoId, tipoGasto.Nombre 	
) AS todo
ORDER BY ProductoId,
		 UnidadAdministrativaId,
		 ProyectoId,
		 TipoGastoId
