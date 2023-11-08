SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionMaterialProductos]
	@areaId NVARCHAR(10),
	@unidadAdministrativaId NVARCHAR(10),
	@proyectoId NVARCHAR(10)
AS
-- ===============================================================
-- Author:		Javier Elías
-- Create date: 11/05/2021
-- Modified date: 04/03/2021
-- Description:	Procedimiento para Obtener los registros que se van a agregar
--						al combo de Productos de la Ficha Requisición Material.
-- ===============================================================
-- Creamos tablas temporales para las Dependencias y Proyectos
DECLARE @dependenciasIds TABLE (DependenciaId	VARCHAR (6), Nombre	VARCHAR (250), CuentaDeRegistro	BIT)
DECLARE @proyectosIds TABLE (ProyectoId	VARCHAR (6), ClasificadorFuncionalId	VARCHAR (6), SubProgramaGobiernoId	VARCHAR (6), Nombre	VARCHAR (250), Editable	BIT, ClasificacionGeograficaId	VARCHAR (6), Capitulos	VARCHAR (10))

IF ( @unidadAdministrativaId IS NULL )
BEGIN
		INSERT INTO @dependenciasIds
		EXEC ARspConsultaDependenciasPorAreaId @areaId
END

ELSE
BEGIN
		INSERT INTO @dependenciasIds
		SELECT * FROM tblDependencia WHERE DependenciaId = @unidadAdministrativaId
END

IF ( @proyectoId IS NULL )
BEGIN
		INSERT INTO @proyectosIds
		EXEC ARspConsultaProyectosPorAreaId @areaId
END

ELSE
BEGIN
		INSERT INTO @proyectosIds
		SELECT * FROM tblProyecto WHERE ProyectoId = @proyectoId
END

SELECT CONCAT(ProductoId, AlmacenId, UnidadAdministrativaId, ProyectoId, TipoGastoId) AS ProductoDetalleId, * 
FROM (
	SELECT DISTINCT producto.ProductoId,
		   producto.Descripcion,
		   producto.ProductoId + ' - ' + producto.Descripcion AS Producto,
		   um.UnidadDeMedidaId,
		   um.Descripcion AS UnidadDeMedida,
		   CostoPromedio AS CostoUnitario,
		   almacen.AlmacenId,
		   almacen.Nombre AS Almacen,
		   unidadAdministrativa.DependenciaId AS UnidadAdministrativaId,
		   unidadAdministrativa.Nombre AS UnidadAdministrativa,
		   proyecto.ProyectoId,
		   proyecto.Nombre AS Proyecto,
		   tipoGasto.TipoGastoId,
		   tipoGasto.Nombre AS TipoGasto
	FROM ARtblControlMaestroConfiguracionArea AS area
		 INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen AS areaAlmacen ON area.ConfiguracionAreaId = areaAlmacen.ConfiguracionAreaId AND areaAlmacen.Borrado = 0
		 INNER JOIN ARtblAlmacenProducto AS almacenProducto ON areaAlmacen.AlmacenId = almacenProducto.AlmacenId AND almacenProducto.Borrado = 0
		 INNER JOIN tblAlmacen AS almacen ON almacenProducto.AlmacenId = almacen.AlmacenId
		 INNER JOIN tblProducto AS producto ON almacenProducto.ProductoId = producto.ProductoId AND producto.Status = 'A'
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
		 INNER JOIN tblCuentaPresupuestalEgr AS cuenta ON almacenProducto.CuentaPresupuestalId = cuenta.CuentaPresupuestalEgrId
		 INNER JOIN tblDependencia AS unidadAdministrativa ON cuenta.DependenciaId = unidadAdministrativa.DependenciaId
		 INNER JOIN @dependenciasIds AS dependenciasTemp ON unidadAdministrativa.DependenciaId = dependenciasTemp.DependenciaId
		 INNER JOIN tblProyecto AS proyecto ON cuenta.ProyectoId = proyecto.ProyectoId
		 INNER JOIN @proyectosIds AS proyectosTemp ON proyecto.ProyectoId = proyectosTemp.ProyectoId
		 INNER JOIN tblTipoGasto AS tipoGasto ON cuenta.TipoGastoId = tipoGasto.TipoGastoId
	WHERE area.Borrado = 0
		  AND area.AreaId = @areaId
) AS todo
ORDER BY ProductoId,
         AlmacenId,
		 UnidadAdministrativaId,
		 ProyectoId
GO