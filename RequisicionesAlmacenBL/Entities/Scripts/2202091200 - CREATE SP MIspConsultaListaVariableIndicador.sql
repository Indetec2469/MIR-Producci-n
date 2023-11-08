SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ****************************************************************
 * MIspConsultaVariableIndicadorConMIRI
 * ****************************************************************
 * Descripción: Consulta para obtener lista de el reporte de Varables de los Indicadores.
 * Autor: Rene Carrillo
 * Fecha: 09.02.2022
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: MIRId
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE OR ALTER PROCEDURE [dbo].[MIspConsultaListaVariableIndicador]
	@MIRId INT
AS
	-- Este es las variables para los necesito
	DECLARE @NivelIndicadorId INT, 
		@NombreNivel NVARCHAR(100),
		@NumeroNivel INT,
		-- Para empezar con 1 es FIN (2 = PROPOSITO, 3 = COMPONENTE, 4 = ACTIVIDAD)
		@NumeroCicloNivel INT = 1,
		@MIRIndicadorId INT,
		@MIRIndicadorFormulaVariableId INT,
		@MIRIndicadorFormulaVariableId_ INT,
		@MIRIndicadorIdActividad INT, 
		@NumeroNivelActividad INT,
		@Orden INT = 1;
	-- Este es la tabla de ListaVariableIndicdor para salida con los datos.
	DECLARE @ListaVariableIndicdor TABLE(
		MIRIndicadorId INT NULL,
		MIRIndicadorComponenteId INT NULL,
		Orden INT NULL,
		Nombre NVARCHAR(100) NULL,
		ResumenNarrativo NVARCHAR(MAX) NULL,
		NombreIndicador NVARCHAR(500) NULL,
		Formula NVARCHAR(MAX) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL,
		MIRIndicadorFormulaVariableId INT NULL,
		DescripcionVariable NVARCHAR(500) NULL,
		Enero MONEY NULL,
		Febrero MONEY NULL,
		Marzo MONEY NULL,
		Abril MONEY NULL,
		Mayo MONEY NULL,
		Junio MONEY NULL,
		Julio MONEY NULL,
		Agosto MONEY NULL,
		Septiembre MONEY NULL,
		Octubre MONEY NULL,
		Noviembre MONEY NULL,
		Diciembre MONEY NULL
	);
	DECLARE @ListaMIRI TABLE (
		MIRIndicadorId INT NULL, 
		MIRIndicadorComponenteId INT NULL,
		ResumenNarrativo NVARCHAR(MAX) NULL,
		NombreIndicador NVARCHAR(500) NULL,
		Formula NVARCHAR(MAX) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL
	);
	DECLARE @ListaMIRI_ TABLE (
		MIRIndicadorId INT NULL, 
		ResumenNarrativo NVARCHAR(MAX) NULL,
		NombreIndicador NVARCHAR(500) NULL,
		Formula NVARCHAR(MAX) NULL,
		FrecuenciaMedicion NVARCHAR(100) NULL,
		UnidadMedida NVARCHAR(200) NULL
	);
	DECLARE @ListaFV TABLE (
		MIRIndicadorFormulaVariableId INT NULL, 
		DescripcionVariable NVARCHAR(MAX) NULL
	);
	DECLARE @ListaFV_ TABLE (
		MIRIndicadorFormulaVariableId INT NULL, 
		DescripcionVariable NVARCHAR(MAX) NULL
	);
	-- Este es las variables para obtener las tablas de MIR, MIRI, FV y SV
	DECLARE @MIRIndicadorComponenteId INT, @ResumenNarrativo NVARCHAR(MAX), @NombreIndicador NVARCHAR(500), @Formula NVARCHAR(MAX), @FrecuenciaMedicion NVARCHAR(100), @UnidadMedida NVARCHAR(200), @DescripcionVariable NVARCHAR(500), @Enero MONEY, @Febrero MONEY, @Marzo MONEY, @Abril MONEY, @Mayo MONEY, @Junio MONEY, @Julio MONEY, @Agosto MONEY, @Septiembre MONEY, @Octubre MONEY, @Noviembre MONEY, @Diciembre MONEY;

	WHILE @NumeroCicloNivel <= 4
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

		IF(@NumeroCicloNivel = 4)
			BEGIN
				SET @NivelIndicadorId = 43; -- ACTIVIDAD
				SET	@NombreNivel = 'ACTIVIDAD';
			END
		SET @NumeroNivel = 1;
		-- Insertar los datos a la tabla de ListaMIRI
		INSERT INTO @ListaMIRI (MIRIndicadorId, MIRIndicadorComponenteId, ResumenNarrativo, NombreIndicador, Formula, FrecuenciaMedicion, UnidadMedida)
			(SELECT miri.MIRIndicadorId, miri.MIRIndicadorComponenteId, miri.ResumenNarrativo, miri.NombreIndicador, miri.DescripcionFormula, fm.Descripcion, um.Nombre
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
			-- Insertar la tabla de ListaMIRI
			INSERT INTO @ListaFV (MIRIndicadorFormulaVariableId, DescripcionVariable)
				(SELECT fv.MIRIndicadorFormulaVariableId, fv.DescripcionVariable
				FROM MItblMatrizIndicadorResultadoIndicadorFormulaVariable fv
				WHERE fv.MIRIndicadorId = @MIRIndicadorId AND fv.EstatusId = 1);

			-- Crear el cursor para el ID del MIRI en cada como arreglo (ciclo)
			DECLARE ListaMIRIndicadorFormulaVariableId CURSOR FOR SELECT fv.MIRIndicadorFormulaVariableId FROM @ListaFV fv;
			-- Abrir el cursor para empezar el ciclo
			OPEN ListaMIRIndicadorFormulaVariableId;
			-- Para el proximo MIRIndicadorId (Ciclo)
			FETCH NEXT FROM ListaMIRIndicadorFormulaVariableId INTO @MIRIndicadorFormulaVariableId;
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @Enero = null; SET @Febrero = null; SET @Marzo = null; SET @Abril = null; SET @Mayo = null; SET @Junio = null; SET @Julio = null; SET @Agosto = null; SET @Septiembre = null; SET @Octubre = null; SET @Noviembre = null; SET @Diciembre = null;
				-- Establecer las variables para insertar a la tabla de ListaMIRI
				(SELECT @MIRIndicadorComponenteId = miri.MIRIndicadorComponenteId, @ResumenNarrativo = miri.ResumenNarrativo, @NombreIndicador = miri.NombreIndicador, @Formula = miri.Formula, @FrecuenciaMedicion = miri.FrecuenciaMedicion, @UnidadMedida = miri.UnidadMedida FROM @ListaMIRI miri WHERE miri.MIRIndicadorId = @MIRIndicadorId);
				-- Establecer las variables para insertar a la tabla de ListaVariableIndicador
				(SELECT @DescripcionVariable = fv.DescripcionVariable FROM @ListaFV fv WHERE fv.MIRIndicadorFormulaVariableId = @MIRIndicadorFormulaVariableId);
				-- Establecer las variables para insertar a la tabla de ListaVariableIndicador
				(SELECT @Enero = sv.Enero, @Febrero = sv.Febrero, @Marzo = sv.Marzo, @Abril = sv.Abril, @Mayo = sv.Mayo, @Junio = sv.Junio, @Julio = sv.Julio, @Agosto = sv.Agosto, @Septiembre = sv.Septiembre, @Octubre = sv.Octubre, @Noviembre = sv.Noviembre, @Diciembre = sv.Diciembre FROM MItblMatrizConfiguracionPresupuestalSeguimientoVariable sv WHERE sv.MIRIndicadorFormulaVariableId = @MIRIndicadorFormulaVariableId);
				-- Insertar la tabla de ListaVariableIndicdor
				INSERT INTO @ListaVariableIndicdor (MIRIndicadorId, MIRIndicadorComponenteId, Orden, Nombre, ResumenNarrativo, NombreIndicador, Formula, FrecuenciaMedicion, UnidadMedida, MIRIndicadorFormulaVariableId, DescripcionVariable, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
					VALUES(@MIRIndicadorId, @MIRIndicadorComponenteId, @Orden, CONCAT(@NombreNivel, ' ' , @NumeroNivel), @ResumenNarrativo, @NombreIndicador, @Formula, @FrecuenciaMedicion, @UnidadMedida, @MIRIndicadorFormulaVariableId, @DescripcionVariable, @Enero, @Febrero, @Marzo, @Abril, @Mayo, @Junio, @Julio, @Agosto, @Septiembre, @Octubre, @Noviembre, @Diciembre);
				-- Para el proximo MIRIndicadorFormulaVariableId
				FETCH NEXT FROM ListaMIRIndicadorFormulaVariableId INTO @MIRIndicadorFormulaVariableId;
			END
			-- Cerrar el cursor
			CLOSE ListaMIRIndicadorFormulaVariableId;
			DEALLOCATE ListaMIRIndicadorFormulaVariableId;
			-- Establecer el contador
			SET @Orden = @Orden + 1;
			SET @NumeroNivel = @NumeroNivel + 1;
			-- Borrar la tabla de ListaFV para la proxima nueva tabla
			DELETE FROM @ListaFV;
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
	-- Salida con la tabla de ListaMIRI
	-- SELECT * FROM @ListaVariableIndicdor vi WHERE vi.MIRIndicadorId = @MIRIndicadorId_ 
	SELECT * FROM @ListaVariableIndicdor vi
	