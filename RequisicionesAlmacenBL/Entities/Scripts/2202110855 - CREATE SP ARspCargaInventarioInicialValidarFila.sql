SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================
-- Author:		Javier Elías
-- Create date: 10/02/2022
-- Modified date: 
-- Description:	Procesador para validar una fila del Inventario Inicial
--						que se va a cargar.
-- =========================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspCargaInventarioInicialValidarFila]
	@ProductoId VARCHAR(MAX),
	@AlmacenId VARCHAR(MAX),
	@FuenteFinanciamientoId VARCHAR(MAX),
	@ProyectoId VARCHAR(MAX),
	@UnidadAdministrativaId VARCHAR(MAX),
	@TipoGastoId VARCHAR(MAX),
	@Cantidad DECIMAL(28, 10),
	@CostoUnitario DECIMAL(28, 10)
AS
BEGIN
		SET NOCOUNT ON;

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = ''

		DECLARE @51000 VARCHAR(MAX) = @mensaje + '- El Producto [@ProductoId] no existe. <br/>'
		DECLARE @51001 VARCHAR(MAX) = @mensaje + '- El Almacén [@AlmacenId] no existe. <br/>'
		DECLARE @51002 VARCHAR(MAX) = @mensaje + '- La Fuente de Financiamiento [@FuenteFinanciamientoId] no existe. <br/>'
		DECLARE @51003 VARCHAR(MAX) = @mensaje + '- El Proyecto [@ProyectoId] no existe. <br/>'
		DECLARE @51004 VARCHAR(MAX) = @mensaje + '- La Unidad Administrativa [@UnidadAdministrativaId] no existe. <br/>'
		DECLARE @51005 VARCHAR(MAX) = @mensaje + '- El Tipo de Gasto [@TipoGastoId] no existe. <br/>'
		DECLARE @51006 VARCHAR(MAX) = @mensaje + '- No existe cuenta presupuestal con Fuente de FinanciamientoId [@FuenteFinanciamientoId], Proyecto [@ProyectoId], Unidad AdministrativaId [@UnidadAdministrativaId] y Tipo de Gasto [@TipoGastoId]. <br/>'
		DECLARE @51007 VARCHAR(MAX) = @mensaje + '- La cantidad debe ser mayor a 0. <br/>'
		DECLARE @51008 VARCHAR(MAX) = @mensaje + '- El costo unitario no puede ser menor a 0. <br/>'

		--Declaramos las variables para las validaciones
		DECLARE @ContadorProducto INT
		DECLARE @ContadorAlmacen INT
		DECLARE @ContadorFuenteFinanciamiento INT
		DECLARE @ContadorProyecto INT
		DECLARE @ContadorUnidadAdministrativa INT
		DECLARE @ContadorTipoGasto INT
		DECLARE @ContadorCuentaPresupuestal INT

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

		--Validamos que la cantidad sea mayor a 0
		IF ( @Cantidad IS NULL OR @Cantidad <= 0 )
		BEGIN
			SET @mensaje = @mensaje + @51007
		END

		--Validamos el costo unitario no sea menor a 0
		IF ( @CostoUnitario IS NULL OR @CostoUnitario < 0 )
		BEGIN
			SET @mensaje = @mensaje + @51008
		END

		SELECT @mensaje
END
GO