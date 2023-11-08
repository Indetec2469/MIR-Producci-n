SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 25 Marzo 2022
-- Description:	SP para la creacion de la poliza de transferencia
-- =============================================
CREATE OR ALTER PROCEDURE ARspActualizaPolizaTransferencia 
	 @TransferenciaId INT,
	 @FechaPoliza DateTime

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TIPO_MOVIMIENTO_TRANSFERENCIA_PRODUCTO_ID INT = 74
	DECLARE @CM_TIPO_MOVIMIENTO_TRANSFERENCIA INT = 108

	DECLARE @CantidadRegistros INT
	DECLARE @SumaTotal MONEY
	DECLARE @PolizaId INT
	DECLARE @Contador INT
	DECLARE @NumeroRenglon INT
	DECLARE @FolioPoliza NVARCHAR(6)
	DECLARE @Monto MONEY

	DECLARE @CuentaCargo NVARCHAR(50)
	DECLARE @CuentaAbono NVARCHAR(50)
	DECLARE @ObjetoGastoId VARCHAR(8)
	DECLARE @ObjetoGasto VARCHAR(1000)
	DECLARE @AlmacenOrigenId VARCHAR(4)
	DECLARE @AlmacenOrigenNombre VARCHAR(100)
	DECLARE @AlmacenDestinoId VARCHAR(4)
	DECLARE @AlmacenDestinoNombre VARCHAR(100)

	DECLARE @nombreCuenta VARCHAR(500)
	DECLARE @naturaleza VARCHAR(1)
	DECLARE @clasificacion VARCHAR(1)

	DECLARE @CuentaCargoId INT
	DECLARE @CuentaAbonoId INT
		
	DECLARE @tblRegistros TABLE (
				AlmacenOrigenId VARCHAR(4),
				NombreOrigen VARCHAR(100),
				AlmacenDestinoId VARCHAR(4),
				NombreDestino VARCHAR(100),
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
			AO.AlmacenId AS AlmacenId,
			AO.Nombre AS Almacen,
			AD.AlmacenId AS AlmacenId,
			AD.Nombre AS Almacen,
			P.ObjetoGastoId AS ObjetoGastoId,
			OG.Nombre AS ObjetoGasto,		
			TRM.Cantidad * IM.CostoUnitario AS Monto,
			ROW_NUMBER() OVER ( ORDER BY TRM.FechaCreacion DESC) AS NumeroRegistro
		FROM dbo.ARtblTransferencia TR
		INNER JOIN dbo.ARtblTransferenciaMovto TRM ON TRM.TransferenciaId = TR.TransferenciaId
		INNER JOIN dbo.tblProducto P ON TRM.ProductoId = P.ProductoId
		INNER JOIN dbo.tblObjetoGasto OG ON P.ObjetoGastoId = OG.ObjetoGastoId 
		INNER JOIN dbo.ARtblAlmacenProducto APO ON TRM.AlmacenProductoOrigenId = APO.AlmacenProductoId
		INNER JOIN dbo.ARtblAlmacenProducto APD ON TRM.AlmacenProductoDestinoId = APD.AlmacenProductoId
		INNER JOIN dbo.tblAlmacen AO on APO.AlmacenId = AO.AlmacenId
		INNER JOIN dbo.tblAlmacen AD on APD.AlmacenId = AD.AlmacenId
		INNER JOIN dbo.ARtblInventarioMovimientoAgrupador IMA ON IMA.ReferenciaMovtoId = TR.TransferenciaId AND IMA.TipoMovimientoId = @CM_TIPO_MOVIMIENTO_TRANSFERENCIA
		CROSS APPLY (SELECT TOP 1 IM.CostoUnitario 
					 FROM dbo.ARtblInventarioMovimiento IM 
					 WHERE IM.InventarioMovtoAgrupadorId = IMA.InventarioMovtoAgrupadorId AND 
					       IM.ReferenciaMovtoId =  TRM.TransferenciaMovtoId ) AS IM
		WHERE TR.TransferenciaId = @TransferenciaId

		-- Inicializamos variables
		SET @CantidadRegistros = (SELECT MAX(NumeroRegistro) FROM @tblRegistros) 
		SET @Contador = 1
		SET @NumeroRenglon = 1
				  
		--Iteramos por cada detalle
		WHILE(@contador <= @CantidadRegistros)
		BEGIN
	
			-- Obtenemos el id del almacen origen que vamos a afectar
			SET @AlmacenOrigenId = (SELECT AlmacenOrigenId FROM @tblRegistros WHERE NumeroRegistro = @Contador)
		
			-- Obtenemos el nombre  del almacen origen que vamos a afectar
			SET @AlmacenOrigenNombre = (SELECT NombreOrigen FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			-- Obtenemos el id del almacen destino que vamos a afectar
			SET @AlmacenDestinoId = (SELECT AlmacenDestinoId FROM @tblRegistros WHERE NumeroRegistro = @Contador)
		
			-- Obtenemos el nombre  del almacen destino que vamos a afectar
			SET @AlmacenDestinoNombre = (SELECT NombreDestino FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el ID del Objeto del Gasto
			SET @ObjetoGastoId = (SELECT ObjetoGastoId FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el Nombre del Objeto del Gasto
			SET @ObjetoGasto = (SELECT ObjetoGasto FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			--Obtenemos el Monto del registro
			SET @Monto = (SELECT Monto FROM @tblRegistros WHERE NumeroRegistro = @Contador)

			-- Obtenemos las cuentas a cargar / abonar
			SET @CuentaCargo = (SELECT CuentaAlmacenCodigo + '-' + @AlmacenDestinoId + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')
			SET @CuentaAbono = (SELECT CuentaAlmacenCodigo + '-' + @AlmacenOrigenId + '-' + @ObjetoGastoId FROM dbo.CGvwClasificadorObjetoGasto WHERE COGCodigo LIKE SUBSTRING(@ObjetoGastoId, 1, 2)  + '%')

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
			VALUES(@CuentaCargoId, @NumeroRenglon, @Monto, 0, @AlmacenDestinoNombre + ' ' + @ObjetoGasto)

			SET @NumeroRenglon += 1

			--Creamos el Abono
			INSERT INTO @tblPolizaDet_Temp (CatalogoCuentaId, Renglon, Cargo, Abono, Concepto)
			VALUES(@CuentaAbonoId, @NumeroRenglon, 0, @Monto, @AlmacenOrigenNombre + ' ' + @ObjetoGasto)

			SET @NumeroRenglon += 1		
			SET @Contador += 1	
	
		END

		SET @SumaTotal = (SELECT SUM(Cargo) FROM @tblPolizaDet_Temp)
		SET @FolioPoliza = (SELECT * FROM [dbo].[fn_ObtenerFolioPoliza] (YEAR(@FechaPoliza),'P'))

		--Insertamos la Cabecera de la Poliza
		INSERT INTO tblPoliza (Ejercicio, Poliza,  Fecha, Status, Concepto, CantidadRenglones, SumaTotal, NumeroCheque, Beneficiario)
		VALUES(YEAR(@FechaPoliza), @FolioPoliza, @FechaPoliza, 'A', 'Transferencia Producto', @NumeroRenglon -1, @SumaTotal, '', '')

		--Recuperamos el Id que se acaba de generar
		SET @PolizaId = SCOPE_IDENTITY()

		--Insertamos los detalles de la Poliza
		INSERT INTO tblPolizaDet(PolizaId, CatalogoCuentaId, Renglon, Cargo, Abono, Concepto)
		SELECT @PolizaId, CatalogoCuentaId, Renglon, Cargo, Abono, Concepto
		FROM @tblPolizaDet_Temp

		--Creamos la Referencia en tblPolizaRef
		INSERT INTO tblPolizaRef (PolizaId, TipoMovimientoId, Tipo, Referencia)
		SELECT @PolizaId, @TIPO_MOVIMIENTO_TRANSFERENCIA_PRODUCTO_ID, 'A', @TransferenciaId
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END
GO

