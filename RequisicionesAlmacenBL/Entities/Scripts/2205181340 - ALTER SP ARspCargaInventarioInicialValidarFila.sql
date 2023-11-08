SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================
-- Author:		Javier Elías
-- Create date: 10/02/2022
-- Modified date: 18/05/2022
-- Description:	Procesador para validar una fila del Inventario Inicial
--						que se va a cargar.
-- =========================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspCargaInventarioInicialValidarFila]
	@ProductoId VARCHAR(4000),
	@AlmacenId VARCHAR(4000),
	@FuenteFinanciamientoId VARCHAR(4000),
	@ProyectoId VARCHAR(4000),
	@UnidadAdministrativaId VARCHAR(4000),
	@TipoGastoId VARCHAR(4000),
	@Cantidad VARCHAR(4000),
	@CostoUnitario VARCHAR(4000)
AS
BEGIN
		SET NOCOUNT ON;

		--THROWS
		DECLARE @mensaje VARCHAR(4000) = ''

		DECLARE @51000 VARCHAR(4000) = @mensaje + '- El Producto [@ProductoId] no existe. <br/>'
		DECLARE @51001 VARCHAR(4000) = @mensaje + '- El Almacén [@AlmacenId] no existe. <br/>'
		DECLARE @51002 VARCHAR(4000) = @mensaje + '- La Fuente de Financiamiento [@FuenteFinanciamientoId] no existe. <br/>'
		DECLARE @51003 VARCHAR(4000) = @mensaje + '- El Proyecto [@ProyectoId] no existe. <br/>'
		DECLARE @51004 VARCHAR(4000) = @mensaje + '- La Unidad Administrativa [@UnidadAdministrativaId] no existe. <br/>'
		DECLARE @51005 VARCHAR(4000) = @mensaje + '- El Tipo de Gasto [@TipoGastoId] no existe. <br/>'
		DECLARE @51006 VARCHAR(4000) = @mensaje + '- No existe cuenta presupuestal con Fuente de FinanciamientoId [@FuenteFinanciamientoId], Proyecto [@ProyectoId], Unidad AdministrativaId [@UnidadAdministrativaId] y Tipo de Gasto [@TipoGastoId]. <br/>'
		DECLARE @51007 VARCHAR(4000) = @mensaje + '- La información proporcionada en Cantidad no es correcta. <br/>'
		DECLARE @51008 VARCHAR(4000) = @mensaje + '- La Cantidad debe ser mayor a 0. <br/>'
		DECLARE @51009 VARCHAR(4000) = @mensaje + '- La información proporcionada en Costo Unitario no es correcta. <br/>'
		DECLARE @51010 VARCHAR(4000) = @mensaje + '- El Costo Unitario debe ser mayor a 0. <br/>'

		--Declaramos las variables para las validaciones
		DECLARE @ContadorProducto INT
		DECLARE @ContadorAlmacen INT
		DECLARE @ContadorFuenteFinanciamiento INT
		DECLARE @ContadorProyecto INT
		DECLARE @ContadorUnidadAdministrativa INT
		DECLARE @ContadorTipoGasto INT
		DECLARE @ContadorCuentaPresupuestal INT
		DECLARE @CantidadD DECIMAL(28, 10)
		DECLARE @CostoUnitarioD DECIMAL(28, 10)

		--Validamos que el ProductoId exista
		SELECT @ContadorProducto = COUNT(ProductoId) FROM tblProducto WHERE ProductoId = @ProductoId AND Status = 'A'

		IF ( @ContadorProducto = 0 )
		BEGIN
			SET @mensaje = @mensaje + (REPLACE(@51000, '@ProductoId', @ProductoId))
		END

		--Validamos que el AlmacenId exista
		SELECT @ContadorAlmacen = COUNT(AlmacenId) FROM tblAlmacen WHERE AlmacenId = @AlmacenId

		IF ( @ContadorAlmacen = 0 )
		BEGIN
			SET @mensaje = @mensaje + (REPLACE(@51001, '@AlmacenId', @AlmacenId));
		END

		--Validamos que el FuenteFinanciamientoId exista
		SELECT @ContadorFuenteFinanciamiento = COUNT(RamoId) FROM tblRamo WHERE RamoId = @FuenteFinanciamientoId

		IF ( @ContadorFuenteFinanciamiento = 0 )
		BEGIN
			SET @mensaje = @mensaje + (REPLACE(@51002, '@FuenteFinanciamientoId', @FuenteFinanciamientoId))
		END

		--Validamos que el ProyectoId exista
		SELECT @ContadorProyecto = COUNT(ProyectoId) FROM tblProyecto WHERE ProyectoId = @ProyectoId

		IF ( @ContadorProyecto = 0 )
		BEGIN
			SET @mensaje = @mensaje + (REPLACE(@51003, '@ProyectoId', @ProyectoId))
		END

		--Validamos que el UnidadAdministrativaId exista
		SELECT @ContadorUnidadAdministrativa = COUNT(DependenciaId) FROM tblDependencia WHERE DependenciaId = @UnidadAdministrativaId

		IF ( @ContadorUnidadAdministrativa = 0 )
		BEGIN
			SET @mensaje = @mensaje + (REPLACE(@51004, '@UnidadAdministrativaId', @UnidadAdministrativaId))
		END

		--Validamos que el TipoGastoId exista
		SELECT @ContadorTipoGasto = COUNT(TipoGastoId) FROM tblTipoGasto WHERE TipoGastoId = @TipoGastoId

		IF ( @ContadorTipoGasto = 0 )
		BEGIN
			SET @mensaje = @mensaje + (REPLACE(@51005, '@TipoGastoId', @TipoGastoId))
		END

		--Validamos que exista la Cuenta Presupuestal
		SELECT @ContadorCuentaPresupuestal = COUNT(CuentaPresupuestalEgrId)
		FROM tblProducto AS producto
			 INNER JOIN tblCuentaPresupuestalEgr AS cp ON producto.ObjetoGastoId = cp.ObjetoGastoId
														  AND cp.RamoId = @FuenteFinanciamientoId
														  AND cp.ProyectoId = @ProyectoId
														  AND cp.DependenciaId = @UnidadAdministrativaId
														  AND cp.TipoGastoId = @TipoGastoId
		WHERE producto.ProductoId = @ProductoId
			  AND producto.Status = 'A'

		IF ( @ContadorCuentaPresupuestal = 0 )
		BEGIN
			SET @mensaje = @mensaje + (REPLACE(REPLACE(REPLACE(REPLACE(@51006, '@FuenteFinanciamientoId', @FuenteFinanciamientoId), '@ProyectoId', @ProyectoId), '@UnidadAdministrativaId', @UnidadAdministrativaId), '@TipoGastoId', @TipoGastoId))
		END

		--Validamos que la cantidad sea mayor a 0 y tenga un formato válido
		BEGIN TRY  
			 SET @CantidadD = CONVERT(DECIMAL(28, 10), @Cantidad)
		END TRY  
		BEGIN CATCH
		END CATCH

		IF ( @CantidadD IS NULL OR @CantidadD <= 0 )
		BEGIN
			SET @mensaje = @mensaje + (CASE WHEN @CantidadD IS NULL THEN @51007 ELSE @51008 END )
		END

		--Validamos el costo unitario sea mayor a 0 y tenga un formato válido
		BEGIN TRY  
			 SET @CostoUnitarioD = CONVERT(DECIMAL(28, 10), @CostoUnitario)
		END TRY  
		BEGIN CATCH
		END CATCH

		IF ( @CostoUnitarioD IS NULL OR @CostoUnitarioD <= 0 )
		BEGIN
			SET @mensaje = @mensaje + (CASE WHEN @CostoUnitarioD IS NULL THEN @51009 ELSE @51010 END )
		END

		SELECT @mensaje
END
GO