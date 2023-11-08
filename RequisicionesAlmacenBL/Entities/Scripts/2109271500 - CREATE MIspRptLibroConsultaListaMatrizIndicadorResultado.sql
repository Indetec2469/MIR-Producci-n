SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* ****************************************************************
 * MIspRptLibroConsultaListaMatrizIndicadorResultado
 * ****************************************************************
 * Descripción: Consulta para obtener lista de MIR para el reporte.
 * Autor: Rene Carrillo
 * Fecha: 27.09.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: MIRId
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE OR ALTER   PROCEDURE [dbo].[MIspRptLibroConsultaListaMatrizIndicadorResultado]
	@MIRId INT
AS
	-- Este es las variables para los necesito
	DECLARE @NivelIndicadorId INT, 
		@NombreNivel NVARCHAR(100),
		@NumeroNivel INT,
		-- Para empezar con 1 es FIN (2 = PROPOSITO, 3 = COMPONENTE, 4 = ACTIVIDAD)
		@NumeroCicloNivel INT = 1,
		@MIRIndicadorId INT,
		@MIRIndicadorIdActividad INT, 
		@NumeroNivelActividad INT,
		@Orden INT = 1;
	-- Este es la tabla de ListaVariableIndicdor para salida con los datos.
	DECLARE @ListaMIR TABLE(
		MIRIndicadorId INT NULL,
		Orden INT NULL,
		Nombre NVARCHAR(100) NULL,
		NombreIndicador NVARCHAR(500) NULL,
		ResumenNarrativo NVARCHAR(MAX) NULL,
		Formula NVARCHAR(MAX) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL,
		LineaBase NVARCHAR(MAX) NULL,
		MedioVerificacion NVARCHAR(MAX) NULL,
		Meta DECIMAL(18,2) NULL
	);
	DECLARE @ListaMIRI TABLE (
		MIRIndicadorId INT NULL, 
		NombreIndicador NVARCHAR(500) NULL,
		ResumenNarrativo NVARCHAR(MAX) NULL,
		Formula NVARCHAR(MAX) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL,
		LineaBase NVARCHAR(MAX) NULL,
		MedioVerificacion NVARCHAR(MAX) NULL,
		Meta DECIMAL(18,2) NULL
	);
	DECLARE @ListaMIRI_ TABLE (
		MIRIndicadorId INT NULL, 
		NombreIndicador NVARCHAR(500) NULL,
		ResumenNarrativo NVARCHAR(MAX) NULL,
		Formula NVARCHAR(MAX) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL,
		LineaBase NVARCHAR(MAX) NULL,
		MedioVerificacion NVARCHAR(MAX) NULL,
		Meta DECIMAL(18,2) NULL
	);
	-- Este es las variables para obtener las tablas de MIR, MIRI, FV y SV
	DECLARE @NombreIndicador NVARCHAR(500), @ResumenNarrativo NVARCHAR(MAX), @Formula NVARCHAR(MAX), @FrecuenciaMedicion NVARCHAR(100), @UnidadMedida NVARCHAR(200), @LineaBase NVARCHAR(MAX), @MedioVerificacion NVARCHAR(MAX), @Meta DECIMAL(18,2);

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

		SET @NumeroNivel = 1;

		-- Insertar los datos a la tabla de ListaMIRI
		INSERT INTO @ListaMIRI (MIRIndicadorId, NombreIndicador, ResumenNarrativo, Formula, FrecuenciaMedicion, UnidadMedida, LineaBase, MedioVerificacion, Meta)
			(SELECT miri.MIRIndicadorId, miri.NombreIndicador, miri.ResumenNarrativo, miri.DescripcionFormula, fm.Descripcion, um.Nombre, CONCAT(miri.AnioBase, ' ', miri.ValorBase), miri.MedioVerificacion, (SELECT SUM(mirim.Valor) FROM MItblMatrizIndicadorResultadoIndicadorMeta mirim WHERE mirim.MIRIndicadorId = miri.MIRIndicadorId AND mirim.EstatusId = 1)
			FROM MItblMatrizIndicadorResultadoIndicador miri
				INNER JOIN MItblControlMaestroFrecuenciaMedicion fm ON miri.FrecuenciaMedicionId = fm.FrecuenciaMedicionId
				INNER JOIN MItblControlMaestroUnidadMedida um ON miri.UnidadMedidaId = um.UnidadMedidaId
			WHERE miri.MIRId = @MIRid AND miri.NivelIndicadorId = @NivelIndicadorId AND miri.EstatusId = 1);
		-- Crear el cursor para el ID del MIRI en cada como arreglo (ciclo)
		DECLARE ListaMIRIndicadorId CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI miri;
		-- Establecer variable NumeroNivel para poner el nombre de nivel en lista
		--SET @NumeroNivel = 1;
		-- Abrir el cursor para empezar el ciclo
		OPEN ListaMIRIndicadorId;
		-- Para el proximo MIRIndicadorId (Ciclo)
		FETCH NEXT FROM ListaMIRIndicadorId INTO @MIRIndicadorId;
		WHILE @@FETCH_STATUS = 0
		BEGIN 
			-- Establecer las variables para insertar a la tabla de ListaMIRI
			(SELECT  @NombreIndicador = miri.NombreIndicador, @ResumenNarrativo = miri.ResumenNarrativo, @Formula = miri.Formula, @FrecuenciaMedicion = miri.FrecuenciaMedicion, @UnidadMedida = miri.UnidadMedida, @LineaBase = miri.LineaBase, @MedioVerificacion = miri.MedioVerificacion, @Meta = miri.Meta FROM @ListaMIRI miri WHERE miri.MIRIndicadorId = @MIRIndicadorId);
			-- Insertar la tabla de ListaVariableIndicdor
			INSERT INTO @ListaMIR (MIRIndicadorId, Orden, Nombre, NombreIndicador, ResumenNarrativo, Formula, FrecuenciaMedicion, UnidadMedida, LineaBase, MedioVerificacion, Meta)
				VALUES(@MIRIndicadorId, @Orden, CONCAT(@NombreNivel, ' ' , @NumeroNivel),  @NombreIndicador, @ResumenNarrativo, @Formula, @FrecuenciaMedicion, @UnidadMedida, @LineaBase, @MedioVerificacion, @Meta);
			-- Si es Componente para agregar los actividades
			-- 3 -> Componente
			IF(@NumeroCicloNivel = 3)
				BEGIN
					IF((SELECT COUNT(*) FROM MItblMatrizIndicadorResultadoIndicador miri WHERE miri.NivelIndicadorId = 43 AND miri.MIRIndicadorComponenteId = @MIRIndicadorId AND miri.EstatusId = 1) > 0)
						BEGIN
							SET @Orden = @Orden + 1;
							-- Insertar los datos a la tabla de ListaMIRI_ (Solo Actividad)
							INSERT INTO @ListaMIRI_ (MIRIndicadorId, NombreIndicador, ResumenNarrativo, Formula, FrecuenciaMedicion, UnidadMedida, LineaBase, MedioVerificacion, Meta)
								(SELECT miri.MIRIndicadorId,  miri.NombreIndicador, miri.ResumenNarrativo, miri.DescripcionFormula, fm.Descripcion, um.Nombre, CONCAT(miri.AnioBase, ' ', miri.ValorBase), miri.MedioVerificacion, (SELECT SUM(mirim.Valor) FROM MItblMatrizIndicadorResultadoIndicadorMeta mirim WHERE mirim.MIRIndicadorId = miri.MIRIndicadorId AND mirim.EstatusId = 1)
								FROM MItblMatrizIndicadorResultadoIndicador miri
									INNER JOIN MItblControlMaestroFrecuenciaMedicion fm ON miri.FrecuenciaMedicionId = fm.FrecuenciaMedicionId
									INNER JOIN MItblControlMaestroUnidadMedida um ON miri.UnidadMedidaId = um.UnidadMedidaId
								WHERE miri.MIRId = @MIRid AND miri.NivelIndicadorId = 43 AND miri.MIRIndicadorComponenteId = @MIRIndicadorId AND miri.EstatusId = 1);
							-- Establecer variable NumeroNivelActividad para poner el nombre de nivel en lista
							SET @NumeroNivelActividad = 1;
							-- Crear el cursor para el ID del MIRI en cada como arreglo (ciclo)
							DECLARE ListaMIRIndicadorIdActividad CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI_ miri;
							-- Abrir el cursor para empezar el ciclo
							OPEN ListaMIRIndicadorIdActividad;
							-- Para el proximo MIRIndicadorId
							FETCH NEXT FROM ListaMIRIndicadorIdActividad INTO @MIRIndicadorIdActividad;
							WHILE @@FETCH_STATUS = 0
							BEGIN
								-- Establecer las variables para insertar a la tabla de ListaMIRI
								(SELECT  @NombreIndicador = miri.NombreIndicador, @ResumenNarrativo = miri.ResumenNarrativo, @Formula = miri.Formula, @FrecuenciaMedicion = miri.FrecuenciaMedicion, @UnidadMedida = miri.UnidadMedida, @LineaBase = miri.LineaBase, @MedioVerificacion = miri.MedioVerificacion, @Meta = miri.Meta FROM @ListaMIRI_ miri WHERE miri.MIRIndicadorId = @MIRIndicadorIdActividad);
								-- Insertar la tabla de ListaVariableIndicdor
								INSERT INTO @ListaMIR (MIRIndicadorId, Orden, Nombre, NombreIndicador, ResumenNarrativo, Formula, FrecuenciaMedicion, UnidadMedida, LineaBase, MedioVerificacion, Meta)
									VALUES(@MIRIndicadorIdActividad, @Orden, CONCAT('Actividad', ' ' ,  @NumeroNivel, '.', @NumeroNivelActividad),  @NombreIndicador, @ResumenNarrativo, @Formula, @FrecuenciaMedicion, @UnidadMedida, @LineaBase, @MedioVerificacion, @Meta);
								-- Establecer el contador
								SET @Orden = @Orden + 1;
								SET @NumeroNivelActividad = @NumeroNivelActividad + 1;
											
								FETCH NEXT FROM ListaMIRIndicadorIdActividad INTO @MIRIndicadorIdActividad
							END
							-- Establecer el contador
							SET @Orden = @Orden - 1;
							-- Cerrar el cursor
							CLOSE ListaMIRIndicadorIdActividad
							DEALLOCATE ListaMIRIndicadorIdActividad
							-- Borrar la tabla de ListaMIR_ para la proxima nueva tabla
							DELETE FROM @ListaMIRI_;
						END
				END
			-- Establecer el contador
			SET @Orden = @Orden + 1;
			SET @NumeroNivel = @NumeroNivel + 1;
			-- Para el proximo MIRIndicadorFormulaVariableId
			FETCH NEXT FROM ListaMIRIndicadorId INTO @MIRIndicadorId;
		END
		-- Cerrar el cursor
		CLOSE ListaMIRIndicadorId
		DEALLOCATE ListaMIRIndicadorId
		-- Borramos la tabla de ListaMIRI
		DELETE FROM @ListaMIRI;
		-- Establecer el contador
		SET @NumeroCicloNivel = @NumeroCicloNivel + 1;
	END
	-- Salida con la tabla de ListaMIR
	SELECT * FROM @ListaMIR
GO