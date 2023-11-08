SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 25 Julio 2022
-- Description:	SP para la creacion de la poliza de surtimieno de requisicion
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[ARspActualizaPolizaSurtimiento] 
	 @InventarioMovimientoAgrupadorId INT,
	 @FechaPoliza DateTime

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--DECLARE @TIPO_MOVIMIENTO_INVENTARIO_SURTIMIENTO_ID INT = 63
	DECLARE @TIPO_MOVIMIENTO_SURTIMIENTO_ID INT = 75

	DECLARE @CodigoRequisicion VARCHAR(20) 
	DECLARE @CantidadRegistros INT
	DECLARE @SumaTotal MONEY
	DECLARE @PolizaId INT
	DECLARE @Contador INT
	DECLARE @NumeroRenglon INT
	DECLARE @FolioPoliza NVARCHAR(6)
	DECLARE @TipoMovimientoId INT
	DECLARE @ConceptoAjusteId INT
	DECLARE @Monto MONEY

	DECLARE @CuentaAlmacen NVARCHAR(50)
	DECLARE @CuentaCargo NVARCHAR(50)
	DECLARE @CuentaAbono NVARCHAR(50)
	DECLARE @ObjetoGastoId VARCHAR(8)
	DECLARE @ObjetoGasto VARCHAR(1000)
	DECLARE @AlmacenId VARCHAR(4)
	DECLARE @AlmacenNombre VARCHAR(100)

	DECLARE @nombreCuenta VARCHAR(500)
	DECLARE @naturaleza VARCHAR(1)
	DECLARE @clasificacion VARCHAR(1)

	DECLARE @CuentaCargoId INT
	DECLARE @CuentaAbonoId INT
		
	DECLARE @tblRegistros TABLE (
				AlmacenId VARCHAR(4),
				Nombre VARCHAR(100),
				ObjetoGastoId VARCHAR(8),
				ObjetoGasto VARCHAR(1000),
				Monto MONEY,
				NumeroRegistro INT		
	)
	
	DECLARE @tblPolizaDet_Temp TABLE(
		CatalogoCuentaId INT,
		Cuenta NVARCHAR(50), 
		Renglon INT,
		Cargo MONEY,
		Abono MONEY,
		Concepto NVARCHAR(600)	
	)


	BEGIN TRY
		-- Obtenemos la informacion que incluira la poliza
		INSERT INTO @tblRegistros
		SELECT 
			A.AlmacenId AS AlmacenId,
			A.Nombre AS Almacen,
			P.ObjetoGastoId AS ObjetoGastoId,
			OG.Nombre AS ObjetoGasto,
			ABS(IM.CantidadMovimiento) * IM.CostoUnitario AS Monto,
			ROW_NUMBER() OVER ( ORDER BY IM.FechaCreacion DESC) AS NumeroRegistro
		FROM ARtblInventarioMovimientoAgrupador IMA
		INNER JOIN ARtblInventarioMovimiento IM ON IMA.InventarioMovtoAgrupadorId = IM.InventarioMovtoAgrupadorId  
		INNER JOIN ARtblAlmacenProducto AP ON IM.AlmacenProductoId = AP.AlmacenProductoId
		INNER JOIN tblProducto P ON AP.ProductoId = P.ProductoId
		INNER JOIN tblAlmacen A on AP.AlmacenId = A.AlmacenId
		INNER JOIN tblObjetoGasto OG ON P.ObjetoGastoId = OG.ObjetoGastoId
		WHERE IMA.InventarioMovtoAgrupadorId = @InventarioMovimientoAgrupadorId

		-- Inicializamos variables
		SET @CantidadRegistros = (SELECT MAX(NumeroRegistro) FROM @tblRegistros) 
		SET @Contador = 1
		SET @NumeroRenglon = 1
		SET @CodigoRequisicion = (SELECT RM.CodigoRequisicion FROM ARtblRequisicionMaterial RM INNER JOIN ARtblInventarioMovimientoAgrupador IMA ON RM.RequisicionMaterialId = IMA.ReferenciaMovtoId WHERE IMA.InventarioMovtoAgrupadorId = @InventarioMovimientoAgrupadorId)
				  
		--Iteramos por cada detalle
		WHILE(@contador <= @CantidadRegistros)
		BEGIN

			-- Obtenemos el id del almacen que vamos a afectar
			SET @AlmacenId = (SELECT AlmacenId FROM @tblRegistros WHERE NumeroRegistro = @Contador)
		
			-- Obtenemos el nombre  del almacen que vamos a afectar
			SET @AlmacenNombre = (SELECT Nombre FROM @tblRegistros WHERE NumeroRegistro = @Contador)
			
			--Obtenemos el ID del Objeto del Gasto
			SET @ObjetoGastoId = (SELECT ObjetoGastoId FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el Nombre del Objeto del Gasto
			SET @ObjetoGasto = (SELECT ObjetoGasto FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el Monto del registro
			SET @Monto = (SELECT Monto FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos la cuenta de Cargo
			SET @CuentaCargo = (SELECT CuentaGastoCodigo + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')
					
			--Obtenemos la cuenta de Abono
			SET @CuentaAbono = (SELECT CuentaAlmacenCodigo + '-' + @AlmacenId + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')

			--Buscamos los ID's de las Cuentas a Afectar
			SET @CuentaCargoId = (SELECT CatalogoCuentaId FROM tblCatalogoCuenta WHERE Cuenta = @CuentaCargo) 
			SET @CuentaAbonoId = (SELECT CatalogoCuentaId FROM tblCatalogoCuenta WHERE Cuenta = @CuentaAbono) 

			-- Si las cuentas no existen, crearlas
			IF(@CuentaCargoId IS NULL)
			BEGIN

				SELECT @nombreCuenta =  Nombre, @naturaleza = Naturaleza, @clasificacion = Clasificacion
				FROM dbo.tblCatalogoCuenta WHERE Cuenta = SUBSTRING(@CuentaCargo, 1, CHARINDEX('-', @CuentaCargo) - 1)

				EXEC dbo.spCrearCuentaConPadres @codigoCuenta = @CuentaCargo,  -- nvarchar(50)
									@nombre = @nombreCuenta,        -- nvarchar(500)
									@naturaleza = @naturaleza,    -- nvarchar(1)
									@clasificacion = @clasificacion, -- nvarchar(1)
									@tipo = 'R'          -- nvarchar(1)

				SET @CuentaCargoId = (SELECT CatalogoCuentaId FROM tblCatalogoCuenta WHERE Cuenta = @CuentaCargo)
			END

			-- Si las cuentas no existen, crearlas
			IF(@CuentaAbonoId IS NULL)
			BEGIN

				SELECT @nombreCuenta =  Nombre, @naturaleza = Naturaleza, @clasificacion = Clasificacion
				FROM dbo.tblCatalogoCuenta WHERE Cuenta = SUBSTRING(@CuentaAbono, 1, CHARINDEX('-', @CuentaAbono) - 1)

				EXEC dbo.spCrearCuentaConPadres @codigoCuenta = @CuentaAbono,  -- nvarchar(50)
									@nombre = @nombreCuenta,        -- nvarchar(500)
									@naturaleza = @naturaleza,    -- nvarchar(1)
									@clasificacion = @clasificacion, -- nvarchar(1)
									@tipo = 'R'          -- nvarchar(1)

				SET @CuentaAbonoId = (SELECT CatalogoCuentaId FROM tblCatalogoCuenta WHERE Cuenta = @CuentaAbono)
			END

			--Creamos el Cargo
			INSERT INTO @tblPolizaDet_Temp (CatalogoCuentaId, Renglon, Cargo, Abono, Concepto)
			VALUES(@CuentaCargoId, @NumeroRenglon, @Monto, 0, @ObjetoGasto)

			SET @NumeroRenglon += 1

			--Creamos el Abono
			INSERT INTO @tblPolizaDet_Temp (CatalogoCuentaId, Renglon, Cargo, Abono, Concepto)
			VALUES(@CuentaAbonoId, @NumeroRenglon, 0, @Monto, @AlmacenNombre + ' ' + @ObjetoGasto)

			SET @NumeroRenglon += 1		
			SET @Contador += 1	
	
		END

		SET @SumaTotal = (SELECT SUM(Cargo) FROM @tblPolizaDet_Temp)
		SET @FolioPoliza = (SELECT * FROM [dbo].[fn_ObtenerFolioPoliza] (YEAR(@FechaPoliza),'P'))

		--Insertamos la Cabecera de la Poliza
		INSERT INTO tblPoliza (Ejercicio, Poliza,  Fecha, Status, Concepto, CantidadRenglones, SumaTotal, NumeroCheque, Beneficiario)
		VALUES(YEAR(@FechaPoliza), @FolioPoliza, @FechaPoliza, 'A', 'Surtimiento Solicitud: ' + @CodigoRequisicion, @NumeroRenglon -1, @SumaTotal, '', '')

		--Recuperamos el Id que se acaba de generar
		SET @PolizaId = SCOPE_IDENTITY()

		--Insertamos los detalles de la Poliza
		INSERT INTO tblPolizaDet(PolizaId, CatalogoCuentaId, Renglon, Cargo, Abono, Concepto)
		SELECT @PolizaId, CatalogoCuentaId, Renglon, Cargo, Abono, Concepto
		FROM @tblPolizaDet_Temp

		--Creamos la Referencia en tblPolizaRef
		INSERT INTO tblPolizaRef (PolizaId, TipoMovimientoId, Tipo, Referencia)
		SELECT @PolizaId, @TIPO_MOVIMIENTO_SURTIMIENTO_ID, 'A', @InventarioMovimientoAgrupadorId
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS ErrorMessage;
		THROW;
	END CATCH
END
