SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 25 Marzo 2022
-- Description:	SP para la creacion de la poliza de ajuste de inventario
-- =============================================
CREATE OR ALTER PROCEDURE ARspActualizaPolizaAjusteInventario 
	 @AjusteInventarioId INT,
	 @FechaPoliza DateTime

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @TIPO_MOVIMIENTO_INCREMENTA_ID INT = 9
	DECLARE @TIPO_MOVIMIENTO_DISMINUYE_ID INT = 10

	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_DONACION_BAJA_ID INT = 1
	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_FALTANTE_ID INT = 2
	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_ROBO_ID INT = 3
	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_MERMA_ID INT = 4
	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_OBSOLETO_ID INT = 5

	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_SOBRANTE_INVENTARIO_ID INT = 6
	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_SOBRANTE_OBRA_ID INT = 7
	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_DONACION_ALTA_ID INT = 8
	DECLARE @CONCEPTO_AJUSTE_INVENTARIO_DACION_PAGO_ID INT = 9

	DECLARE @TIPO_MOVIMIENTO_AJUSTE_INVENTARIO_ID INT = 73

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
				TipoMovimientoId INT,
				TipoMovimiento VARCHAR(MAX),
				ConceptoAjusteId INT,
				ConceptoAjuste VARCHAR(150),
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
			CM.ControlId AS TipoMovimientoId ,
			CM.Valor AS TipoMovimiento ,
			CAI.ConceptoAjusteInventarioId AS ConceptoAjusteId,		
			CAI.ConceptoAjuste AS ConceptoAjuste,		
			IAD.Cantidad * IAD.CostoUnitario AS Monto,
			ROW_NUMBER() OVER ( ORDER BY IAD.FechaCreacion DESC) AS NumeroRegistro
		FROM ARtblInventarioAjuste IA
		INNER JOIN ARtblInventarioAjusteDetalle IAD ON IA.InventarioAjusteId = IAD.InventarioAjusteId  
		INNER JOIN ARtblControlMaestroConceptoAjusteInventario CAI ON IAD.ConceptoAjusteId = CAI.ConceptoAjusteInventarioId
		INNER JOIN GRtblControlMaestro CM ON IAD.TipoMovimientoId = CM.ControlId
		INNER JOIN tblProducto P ON IAD.ProductoId = P.ProductoId
		INNER JOIN tblAlmacen A on IAD.AlmacenId = A.AlmacenId 
		INNER JOIN dbo.tblObjetoGasto OG ON P.ObjetoGastoId = OG.ObjetoGastoId 
		--INNER JOIN tblCatalogoCuenta CC ON A.CuentaAlmacenId = CC.CatalogoCuentaId
		WHERE IA.InventarioAjusteId = @AjusteInventarioId

		-- Inicializamos variables
		SET @CantidadRegistros = (SELECT MAX(NumeroRegistro) FROM @tblRegistros) 
		SET @Contador = 1
		SET @NumeroRenglon = 1
				  
		--Iteramos por cada detalle
		WHILE(@contador <= @CantidadRegistros)
		BEGIN
	
			-- Obtenemos el tipo de movimiento para saber que cuentas vamos a afectar
			SET @TipoMovimientoId = (SELECT TipoMovimientoId FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			-- Obtenemos el concepto de ajuste para saber que cuentas vamos a afectar
			SET @ConceptoAjusteId = (SELECT ConceptoAjusteId FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			-- Obtenemos el id del almacen que vamos a afectar
			SET @AlmacenId = (SELECT AlmacenId FROM @tblRegistros WHERE NumeroRegistro = @Contador)
		
			-- Obtenemos el nombre  del almacen que vamos a afectar
			SET @AlmacenNombre = (SELECT Nombre FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el codigo de la cuenta de Almacen
			--SET @CuentaAlmacen = (SELECT CuentaAlmacen FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el ID del Objeto del Gasto
			SET @ObjetoGastoId = (SELECT ObjetoGastoId FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el Nombre del Objeto del Gasto
			SET @ObjetoGasto = (SELECT ObjetoGasto FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el Monto del registro
			SET @Monto = (SELECT Monto FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			-- Si el movimiento es de tipo INCREMENTAR
			IF ( @TipoMovimientoId = @TIPO_MOVIMIENTO_INCREMENTA_ID)
			BEGIN
				-- Sobrante de inventario y sobrante de obra
				IF (@ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_SOBRANTE_INVENTARIO_ID OR @ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_SOBRANTE_OBRA_ID)
				BEGIN
					SET @CuentaCargo = (SELECT CuentaAlmacenCodigo + '-' + @AlmacenId + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')
					SET @CuentaAbono = '4325-' + @AlmacenId + '-' + @ObjetoGastoId 
				END	
				--Donacion y Dacion Pago
				ELSE IF (@ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_DONACION_ALTA_ID OR @ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_DACION_PAGO_ID)
				BEGIN
					SET @CuentaCargo = (SELECT CuentaAlmacenCodigo + '-' + @AlmacenId + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')
					SET @CuentaAbono = '4169-' + @ObjetoGastoId 				    
				END	
			END

			-- Si el movimiento es de tipo DISMINUIR
			ELSE IF (@TipoMovimientoId = @TIPO_MOVIMIENTO_DISMINUYE_ID)
			BEGIN

				-- Donación
				IF (@ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_DONACION_BAJA_ID)
				BEGIN
					SET @CuentaCargo = (SELECT CuentaGastoCodigo + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')
					SET @CuentaAbono = (SELECT CuentaAlmacenCodigo + '-' + @AlmacenId + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')
				END

				-- Faltante, Robo, Merma u Obsoleto
				ELSE IF (@ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_FALTANTE_ID OR @ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_ROBO_ID OR @ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_MERMA_ID OR @ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_OBSOLETO_ID)
				BEGIN
					SET @CuentaCargo = '5535-' + @AlmacenId + '-' + @ObjetoGastoId
					SET @CuentaAbono = (SELECT CuentaAlmacenCodigo + '-' + @AlmacenId + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')
				END
			END

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
			VALUES(@CuentaCargoId, @NumeroRenglon, @Monto, 0, CASE WHEN  @ConceptoAjusteId = @CONCEPTO_AJUSTE_INVENTARIO_DONACION_BAJA_ID THEN @ObjetoGasto ELSE @AlmacenNombre + ' ' + @ObjetoGasto END)

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
		VALUES(YEAR(@FechaPoliza), @FolioPoliza, @FechaPoliza, 'A', 'Ajuste de Inventario', @NumeroRenglon -1, @SumaTotal, '', '')

		--Recuperamos el Id que se acaba de generar
		SET @PolizaId = SCOPE_IDENTITY()

		--Insertamos los detalles de la Poliza
		INSERT INTO tblPolizaDet(PolizaId, CatalogoCuentaId, Renglon, Cargo, Abono, Concepto)
		SELECT @PolizaId, CatalogoCuentaId, Renglon, Cargo, Abono, Concepto
		FROM @tblPolizaDet_Temp

		--Creamos la Referencia en tblPolizaRef
		INSERT INTO tblPolizaRef (PolizaId, TipoMovimientoId, Tipo, Referencia)
		SELECT @PolizaId, @TIPO_MOVIMIENTO_AJUSTE_INVENTARIO_ID, 'A', @AjusteInventarioId
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END
GO

