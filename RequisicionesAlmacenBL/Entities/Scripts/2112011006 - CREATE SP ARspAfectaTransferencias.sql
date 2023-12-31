SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================
-- Author:		Javier El�as
-- Create date: 27/11/2021
-- Modified date: 
-- Description:	Procesador para afecta el inventario
--						por una Transferencia
-- ============================================
CREATE OR ALTER PROCEDURE [dbo].[ARspAfectaTransferencias]
	@TransferenciaId INT,
	@UsuarioId INT
AS
BEGIN
		SET NOCOUNT ON;

		DECLARE @tbl TABLE (
			AlmacenProductoId INT,
			Producto VARCHAR(250),
			CuentaPresupuestalId INT,
			CantidadMovimiento DECIMAL(28, 10),
			CostoUnitario MONEY,
			MotivoMovto VARCHAR(MAX),
			DetalleId INT,
			Orden INT
		)

		--Buscamos los datos del producto a afectar
		INSERT INTO @tbl
		SELECT AlmacenProductoId,
			   Producto,
			   CuentaPresupuestalId,
			   CantidadMovimiento,
			   CostoPromedio,
			   MotivoMovto,
			   TransferenciaMovtoId,
			   ROW_NUMBER() OVER(ORDER BY NumeroLinea, CantidadMovimiento) AS Orden
		FROM
		(
			SELECT detalle.AlmacenProductoOrigenId AS AlmacenProductoId,
				   CONVERT(VARCHAR(10), detalle.ProductoId)+' - '+detalle.Descripcion AS Producto,
				   ap.CuentaPresupuestalId,
				   -1 * detalle.Cantidad AS CantidadMovimiento,
				   producto.CostoPromedio,
				   'Transferencia: '+transferencia.Codigo AS MotivoMovto,
				   detalle.TransferenciaMovtoId,
				   detalle.NumeroLinea
			FROM ARtblTransferencia AS transferencia
				 INNER JOIN ARtblTransferenciaMovto AS detalle ON transferencia.TransferenciaId = detalle.TransferenciaId
				 INNER JOIN ARtblAlmacenProducto AS ap ON detalle.AlmacenProductoOrigenId = ap.AlmacenProductoId
				 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
			WHERE transferencia.TransferenciaId = @TransferenciaId
			UNION ALL
			SELECT detalle.AlmacenProductoDestinoId AS AlmacenProductoId,
				   CONVERT(VARCHAR(10), detalle.ProductoId)+' - '+detalle.Descripcion AS Producto,
				   ap.CuentaPresupuestalId,
				   detalle.Cantidad AS CantidadMovimiento,
				   producto.CostoPromedio,
				   'Transferencia: '+transferencia.Codigo AS MotivoMovto,
				   detalle.TransferenciaMovtoId,
				   detalle.NumeroLinea
			FROM ARtblTransferencia AS transferencia
				 INNER JOIN ARtblTransferenciaMovto AS detalle ON transferencia.TransferenciaId = detalle.TransferenciaId
				 INNER JOIN ARtblAlmacenProducto AS ap ON detalle.AlmacenProductoDestinoId = ap.AlmacenProductoId
				 INNER JOIN tblProducto AS producto ON detalle.ProductoId = producto.ProductoId
			WHERE transferencia.TransferenciaId = @TransferenciaId
		) AS movtos
		ORDER BY NumeroLinea,
				 CantidadMovimiento

		DECLARE @contador INT = 1
		DECLARE @registros INT = ( SELECT COUNT(AlmacenProductoId) FROM @tbl )

		DECLARE @AlmacenProductoId INT
		DECLARE @Producto VARCHAR(250)
		DECLARE @CuentaPresupuestalId INT
		DECLARE @CantidadMovimiento DECIMAL(28, 10)
		DECLARE @CostoUnitario MONEY
		DECLARE @TipoMovimientoId INT = 108 -- Transferencia
		DECLARE @MotivoMovto VARCHAR(MAX)
		DECLARE @ReferenciaDetalleId INT

		--THROWS
		DECLARE @mensaje VARCHAR(MAX) = 'No es posible afectar el inventario. '

		DECLARE @51000 VARCHAR(MAX) = @mensaje + 'El conteo del art�culo [@Producto] con cuenta presupuestal [@CuentaPresupuestalId] no puede estar vac�o.'
		DECLARE @51001 VARCHAR(MAX) = @mensaje + 'No existen movimientos por registrar.'

		BEGIN TRY
		--BEGIN TRANSACTION
				--Construimos los movimientos para el procesador
				DECLARE @Movimientos ARudtInventarioMovimiento
				
				WHILE ( @contador <= @registros )
				BEGIN
						SELECT @AlmacenProductoId = AlmacenProductoId,
							   @Producto = Producto,
							   @CuentaPresupuestalId = CuentaPresupuestalId,
							   @CantidadMovimiento = CantidadMovimiento,
							   @CostoUnitario = CostoUnitario,
							   @MotivoMovto = MotivoMovto,
							   @ReferenciaDetalleId = DetalleId
						FROM @tbl
						WHERE Orden = @contador

						--Validamos que se haya llenado la columna de conteo
						IF ( @CantidadMovimiento IS NULL )
						BEGIN 
							SET @mensaje = (REPLACE(REPLACE(@51000, '@Producto', @Producto), '@CuentaPresupuestalId', @CuentaPresupuestalId));
							THROW 51000, @mensaje, 1;
						END

						--Verificamos que la cantidad de movimiento sea diferente de 0
						IF ( @CantidadMovimiento != 0 )
						BEGIN
							--Si pas� las validaciones agregamos el movimiento
							INSERT INTO @Movimientos
							SELECT @AlmacenProductoId,
										 @CantidadMovimiento,
										 @CostoUnitario,
										 @ReferenciaDetalleId
						END

						SET  @contador = @contador + 1
				END

				--Validamos que se existan movimientos por registrar
				DECLARE @contadorMovimientos INT = ( SELECT COUNT(AlmacenProductoId) FROM @Movimientos )

				IF ( @contadorMovimientos = 0 )
						THROW 51001, @mensaje, 1;

				--Si pas� las validaciones mandamos llamar al Procesador de Inventarios
				EXEC ARspProcesadorInventarios
								@TipoMovimientoId,
								@MotivoMovto,
								@UsuarioId,
								1, -- Insertar agrupador
								@TransferenciaId,
								NULL, -- Poliza Id
								@Movimientos
		--COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			--ROLLBACK TRANSACTION;
			THROW;
		END CATCH
END
GO