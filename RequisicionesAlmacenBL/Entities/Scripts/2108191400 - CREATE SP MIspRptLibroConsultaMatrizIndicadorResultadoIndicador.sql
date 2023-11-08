SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* ****************************************************************
 * MIspRptLibroConsultaMatrizIndicadorResultadoIndicador
 * ****************************************************************
 * Descripción: Consulta para obtener el reporte de Matriz Indicador Resultado.
 * Autor: Rene Carrillo
 * Fecha: 19.08.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: MIRId
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE OR ALTER PROCEDURE [dbo].[MIspRptLibroConsultaMatrizIndicadorResultadoIndicador]
	@MIRId INT
AS
	-- Este es las variables para los necesito
	DECLARE @NivelIndicadorId INT, 
		@NombreNivel NVARCHAR(100),
		@NumeroNivel INT = 1,
		-- Para empezar con 1 es FIN (2 = PROPOSITO, 3 = COMPONENTE, 4 = ACTIVIDAD)
		@NumeroCicloNivel INT = 1,
		@MIRIndicadorId INT,
		@MIRIndicadorIdActividad INT, 
		@NumeroNivelActividad INT;
	-- Este es la tabla de ListaMIRI para salida con los datos.
	DECLARE @ListaMIRI TABLE(
		Nombre NVARCHAR(100) NULL,
		ResumenNarrativo NVARCHAR(MAX) NULL,
		NombreIndicador NVARCHAR(500) NULL,
		Formula NVARCHAR(MAX) NULL,
		TipoIndicador NVARCHAR(MAX) NULL,
		Dimension NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		ValorBase DECIMAL(18,2) NULL,
		Sentido NVARCHAR(100) NULL,
		Meta DECIMAL(18,2) NULL,
		MedioVerificacion NVARCHAR(MAX) NULL,
		FuenteInformacion NVARCHAR(MAX) NULL
	);
	DECLARE @ListaMIRI_ TABLE (
		MIRIndicadorId INT NULL, 
		ResumenNarrativo NVARCHAR(MAX) NULL,
		NombreIndicador NVARCHAR(500) NULL,
		Formula NVARCHAR(MAX) NULL,
		TipoIndicador NVARCHAR(MAX) NULL,
		Dimension NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		ValorBase DECIMAL(18,2) NULL,
		Sentido NVARCHAR(100) NULL,
		Meta DECIMAL(18,2) NULL,
		MedioVerificacion NVARCHAR(MAX) NULL,
		FuenteInformacion NVARCHAR(MAX) NULL
	);
	DECLARE @ListaMIRI__ TABLE (
		MIRIndicadorId INT NULL, 
		ResumenNarrativo NVARCHAR(MAX) NULL,
		NombreIndicador NVARCHAR(500) NULL,
		Formula NVARCHAR(MAX) NULL,
		TipoIndicador NVARCHAR(MAX) NULL,
		Dimension NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		ValorBase DECIMAL(18,2) NULL,
		Sentido NVARCHAR(100) NULL,
		Meta DECIMAL(18,2) NULL,
		MedioVerificacion NVARCHAR(MAX) NULL,
		FuenteInformacion NVARCHAR(MAX) NULL
	);
	-- Este es las variables para obtener la tabla de MIRI
	DECLARE @ResumenNarrativo NVARCHAR(MAX), @NombreIndicador NVARCHAR(500), @Formula NVARCHAR(MAX), @TipoIndicador NVARCHAR(MAX), @Dimension NVARCHAR(100), @UnidadMedida NVARCHAR(200), @FrecuenciaMedicion NVARCHAR(100), @ValorBase DECIMAL(18,2), @Sentido NVARCHAR(100), @Meta DECIMAL(18,2), @MedioVerificacion NVARCHAR(MAX), @FuenteInformacion NVARCHAR(MAX);

	WHILE @NumeroCicloNivel <= 3
	BEGIN
		IF(@NumeroCicloNivel = 1)
			BEGIN
				SET @NivelIndicadorId = 40; -- FIN
				SET	@NombreNivel = 'FIN';
			END

		IF(@NumeroCicloNivel = 2)
			BEGIN
				SET @NivelIndicadorId = 41; -- PROPOSITO
				SET	@NombreNivel = 'PROPÓSITO';
			END

		IF(@NumeroCicloNivel = 3)
			BEGIN
				SET @NivelIndicadorId = 42; -- COMPONENTE
				SET	@NombreNivel = 'COMPONENTE';
			END

		--IF(@NumeroCicloNivel = 4)
		--	BEGIN
		--		SET @NivelIndicadorId = 43; -- ACTIVIDAD
		--		SET	@NombreNivel = 'ACTIVIDAD';
		--	END

		-- Insertar los datos a la tabla de ListaMIRI_
		INSERT INTO @ListaMIRI_ (MIRIndicadorId, ResumenNarrativo, NombreIndicador, Formula, TipoIndicador, Dimension, UnidadMedida, FrecuenciaMedicion, ValorBase, Sentido, Meta, MedioVerificacion, FuenteInformacion)
			(SELECT miri.MIRIndicadorId, miri.ResumenNarrativo, miri.NombreIndicador, miri.DescripcionFormula, ti.Descripcion, d.Descripcion, um.Nombre, fm.Descripcion, miri.ValorBase, cmsentido.Valor, (SELECT SUM(mirim.Valor) FROM MItblMatrizIndicadorResultadoIndicadorMeta mirim WHERE mirim.MIRIndicadorId = miri.MIRIndicadorId AND mirim.EstatusId = 1), miri.MedioVerificacion, miri.FuenteInformacion
			FROM MItblMatrizIndicadorResultadoIndicador miri
				INNER JOIN MItblControlMaestroTipoIndicador ti ON miri.TipoIndicadorId = ti.TipoIndicadorId
				INNER JOIN MItblControlMaestroDimension d ON miri.DimensionId = d.DimensionId
				INNER JOIN MItblControlMaestroUnidadMedida um ON miri.UnidadMedidaId = um.UnidadMedidaId
				INNER JOIN MItblControlMaestroFrecuenciaMedicion fm ON miri.FrecuenciaMedicionId = fm.FrecuenciaMedicionId
				INNER JOIN GRtblControlMaestro cmsentido ON miri.SentidoId = cmsentido.ControlId
			WHERE miri.MIRId = @MIRid AND miri.NivelIndicadorId = @NivelIndicadorId AND miri.EstatusId = 1);
		-- Crear el cursor para el ID del MIRI en cada como arreglo (ciclo)
		DECLARE ListaMIRIndicadorId CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI_ miri;
		-- Establecer variable NumeroNivel para poner el nombre de nivel en lista
		SET @NumeroNivel = 1;
		-- Abrir el cursor para empezar el ciclo
		OPEN ListaMIRIndicadorId;
		-- Para el proximo MIRIndicadorId (Ciclo)
		FETCH NEXT FROM ListaMIRIndicadorId INTO @MIRIndicadorId;
		WHILE @@FETCH_STATUS = 0
		BEGIN 
			-- Establecer las variables para insertar a la tabla de ListaMIRI
			(SELECT @ResumenNarrativo = miri.ResumenNarrativo, @NombreIndicador = miri.NombreIndicador, @Formula = miri.Formula, @TipoIndicador = miri.TipoIndicador, @Dimension = miri.Dimension, @UnidadMedida = miri.UnidadMedida, @FrecuenciaMedicion = miri.FrecuenciaMedicion, @ValorBase = miri.ValorBase, @Sentido = miri.Sentido, @Meta = miri.Meta, @MedioVerificacion = miri.MedioVerificacion, @FuenteInformacion = miri.FuenteInformacion FROM @ListaMIRI_ miri WHERE miri.MIRIndicadorId = @MIRIndicadorId);
			-- Insertar la tabla de ListaMIRI
			INSERT INTO @ListaMIRI (Nombre, ResumenNarrativo, NombreIndicador, Formula, TipoIndicador, Dimension, UnidadMedida, FrecuenciaMedicion, ValorBase, Sentido, Meta, MedioVerificacion, FuenteInformacion)
			VALUES(CONCAT(@NombreNivel, ' ' , @NumeroNivel), @ResumenNarrativo, @NombreIndicador, @Formula, @TipoIndicador, @Dimension, @UnidadMedida, @FrecuenciaMedicion, @ValorBase, @Sentido, @Meta, @MedioVerificacion, @FuenteInformacion);
			-- Si es Componente para agregar los actividades
			-- 3 -> Componente
			IF(@NumeroCicloNivel = 3)
				BEGIN
					IF((SELECT COUNT(*) FROM MItblMatrizIndicadorResultadoIndicador miri WHERE miri.NivelIndicadorId = 43 AND miri.MIRIndicadorComponenteId = @MIRIndicadorId AND miri.EstatusId = 1) > 0)
						BEGIN
							-- Insertar los datos a la tabla de ListaMIRI__
							INSERT INTO @ListaMIRI__ (MIRIndicadorId, ResumenNarrativo, NombreIndicador, Formula, TipoIndicador, Dimension, UnidadMedida, FrecuenciaMedicion, ValorBase, Sentido, Meta, MedioVerificacion, FuenteInformacion)
								(SELECT miri.MIRIndicadorId, miri.ResumenNarrativo, miri.NombreIndicador, miri.DescripcionFormula, ti.Descripcion, d.Descripcion, um.Nombre, fm.Descripcion, miri.ValorBase, cmsentido.Valor, 200, miri.MedioVerificacion, miri.FuenteInformacion
								FROM MItblMatrizIndicadorResultadoIndicador miri
									INNER JOIN MItblControlMaestroTipoIndicador ti ON miri.TipoIndicadorId = ti.TipoIndicadorId
									INNER JOIN MItblControlMaestroDimension d ON miri.DimensionId = d.DimensionId
									INNER JOIN MItblControlMaestroUnidadMedida um ON miri.UnidadMedidaId = um.UnidadMedidaId
									INNER JOIN MItblControlMaestroFrecuenciaMedicion fm ON miri.FrecuenciaMedicionId = fm.FrecuenciaMedicionId
									INNER JOIN GRtblControlMaestro cmsentido ON miri.SentidoId = cmsentido.ControlId
								WHERE miri.MIRId = @MIRid AND miri.NivelIndicadorId = 43 AND miri.MIRIndicadorComponenteId = @MIRIndicadorId AND miri.EstatusId = 1);
							-- Establecer variable NumeroNivelActividad para poner el nombre de nivel en lista
							SET @NumeroNivelActividad = 1;
							-- Crear el cursor para el ID del MIRI en cada como arreglo (ciclo)
							DECLARE ListaMIRIndicadorIdActividad CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI__ miri;
							-- Abrir el cursor para empezar el ciclo
							OPEN ListaMIRIndicadorIdActividad;
							-- Para el proximo MIRIndicadorId
							FETCH NEXT FROM ListaMIRIndicadorIdActividad INTO @MIRIndicadorIdActividad;
							WHILE @@FETCH_STATUS = 0
							BEGIN
								-- Establecer las variables para insertar a la tabla de ListaMIRI
								(SELECT @ResumenNarrativo = miri.ResumenNarrativo, @NombreIndicador = miri.NombreIndicador, @Formula = miri.Formula, @TipoIndicador = miri.TipoIndicador, @Dimension = miri.Dimension, @UnidadMedida = miri.UnidadMedida, @FrecuenciaMedicion = miri.FrecuenciaMedicion, @ValorBase = miri.ValorBase, @Sentido = miri.Sentido, @Meta = miri.Meta, @MedioVerificacion = miri.MedioVerificacion, @FuenteInformacion = miri.FuenteInformacion FROM @ListaMIRI__ miri WHERE miri.MIRIndicadorId = @MIRIndicadorIdActividad);
								-- Insertar la tabla de ListaMIRI
								INSERT INTO @ListaMIRI (Nombre, ResumenNarrativo, NombreIndicador, Formula, TipoIndicador, Dimension, UnidadMedida, FrecuenciaMedicion, ValorBase, Sentido, Meta, MedioVerificacion, FuenteInformacion)
								VALUES(CONCAT('ACTIVIDAD', ' ', @NumeroNivel, '.', @NumeroNivelActividad), @ResumenNarrativo, @NombreIndicador, @Formula, @TipoIndicador, @Dimension, @UnidadMedida, @FrecuenciaMedicion, @ValorBase, @Sentido, @Meta, @MedioVerificacion, @FuenteInformacion);
								-- Establecer el contador
								SET @NumeroNivelActividad = @NumeroNivelActividad + 1;
								-- Para el proximo MIRIndicadorId (Ciclo)
								FETCH NEXT FROM ListaMIRIndicadorIdActividad INTO @MIRIndicadorIdActividad
							END
							-- Cerrar el cursor
							CLOSE ListaMIRIndicadorIdActividad
							DEALLOCATE ListaMIRIndicadorIdActividad
							-- Borrar la tabla de ListaMIR__ para la proxima nueva tabla
							DELETE FROM @ListaMIRI__;
						END
				END
			-- Establecer el contador
			SET @NumeroNivel = @NumeroNivel + 1;
			-- Para el proximo MIRIndicadorId
			FETCH NEXT FROM ListaMIRIndicadorId INTO @MIRIndicadorId
		END
		-- Cerrar el cursor
		CLOSE ListaMIRIndicadorId
		DEALLOCATE ListaMIRIndicadorId
		-- Borramos la tabla de ListaMIRI_
		DELETE FROM @ListaMIRI_;
		-- Establecer el contador
		SET @NumeroCicloNivel = @NumeroCicloNivel + 1;
	END
	-- Salida con la tabla de ListaMIRI
	SELECT * FROM @ListaMIRI
GO