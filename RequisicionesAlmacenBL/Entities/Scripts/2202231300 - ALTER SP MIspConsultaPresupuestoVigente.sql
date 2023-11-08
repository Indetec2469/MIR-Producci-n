SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* ****************************************************************
 * MIspConsultaPresupuestoVigente
 * ****************************************************************
 * Descripción: Consulta para obtener los calculos.
 * Autor: Rene Carrillo
 * Fecha: 16.02.2022
 * Fecha de Modificación: 23.02.2022 
 * Versión: 1.0.2
 *****************************************************************
 * PARAMETROS DE ENTRADA: MIRId, Ejercicio
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

--exec MIspConsultaPresupuestoVigente 32,'2022'
CREATE OR ALTER PROCEDURE [dbo].[MIspConsultaPresupuestoVigente] 
	@MIRId AS INT,
	@Ejercicio AS NVARCHAR(4)
AS
	-- Variables
	DECLARE @ContadorRegistrosNuevos INT = -1,
		@EsProyecto BIT,
		@ContadorIndicador INT,
		@ContadorIndicador_ INT,
		@ConfiguracionPresupuestoId INT,
		@ProyectoIdsComma NVARCHAR(MAX),
		@MIRIndicadorId INT,
		@MIRIndicadorId_ INT,
		@ProyectoId NVARCHAR(6),
		@EsComponenteProyecto BIT,
		@Porcentaje DECIMAL(18,2),
		@TotalPorcentaje DECIMAL(19,2),
		@Componente NVARCHAR(30),
		@ContadorComponente INT = 1,
		@Actividad NVARCHAR(30),
		@ContadorActividad INT,
		@InicioMes NVARCHAR(20),
		@ContadorMes INT,
		@EsEditado BIT,
		@CadaMes DECIMAL(18,2),
		@UltimoMes DECIMAL(18,2),
		@FilaMonto DECIMAL(18,2),
		@UltimaFilaMonto DECIMAL(18,2),
		@TotalFilaMonto DECIMAL(18,2),
		@TotalAnual DECIMAL(18,2),
		@InicioMesMonto DECIMAL(18,2),
		@EsUltimaActividad BIT,
		@EsCicloMes BIT,
		@Anual DECIMAL(18,2),
		@Enero DECIMAL(16,2),
		@Febrero DECIMAL(16,2),
		@Marzo DECIMAL(16,2),
		@Abril DECIMAL(16,2),
		@Mayo DECIMAL(16,2),
		@Junio DECIMAL(16,2),
		@Julio DECIMAL(16,2),
		@Agosto DECIMAL(16,2),
		@Septiembre DECIMAL(16,2),
		@Octubre DECIMAL(16,2),
		@Noviembre DECIMAL(16,2),
		@Diciembre DECIMAL(16,2);

	-- Crear la tabla de proyecto
	DECLARE @ListaProyecto TABLE(
		ProyectoId NVARCHAR(6) NULL
	);

	-- Crear la tabla de RepProyecto
	DECLARE @ListaRepProyecto TABLE(
		ProyectoId NVARCHAR(6),
		Enero DECIMAL(16,2),
		Febrero DECIMAL(16,2),
		Marzo DECIMAL(16,2),
		Abril DECIMAL(16,2),
		Mayo DECIMAL(16,2),
		Junio DECIMAL(16,2),
		Julio DECIMAL(16,2),
		Agosto DECIMAL(16,2),
		Septiembre DECIMAL(16,2),
		Octubre DECIMAL(16,2),
		Noviembre DECIMAL(16,2),
		Diciembre DECIMAL(16,2),
		Anual DECIMAL(16,2)
	);

	-- Crear la tabla de MIRI
	DECLARE @ListaMIRI TABLE(
		MIRIndicadorId INT NOT NULL,
		NivelIndicadorId INT NOT NULL,
		NombreIndicador NVARCHAR(500) NOT NULL,
		TipoComponenteId INT NULL,
		MIRIndicadorComponenteId INT NULL,
		ProyectoId NVARCHAR(6),
		PorcentajeProyecto DECIMAL(18,2),
		MontoProyecto DECIMAL(16,2),
		PorcentajeActividad DECIMAL(18,2),
		MontoActividad DECIMAL(16,2),
		EstatusId INT NOT NULL
	);

	-- Crear la tabla de MIRI solo las actividades
	DECLARE @ListaMIRIActividad TABLE(
		MIRIndicadorId INT NOT NULL,
		NivelIndicadorId INT NOT NULL,
		NombreIndicador NVARCHAR(500) NOT NULL,
		ProyectoId NVARCHAR(6),
		PorcentajeProyecto DECIMAL(18,2),
		MontoProyecto DECIMAL(16,2),
		PorcentajeActividad DECIMAL(18,2),
		MontoActividad DECIMAL(16,2)
	);

	-- Crear la tabla de ListaPonderacion
	DECLARE @ListaPonderacion TABLE(
		MIRIndicadorComponenteId INT NOT NULL,
		Enero DECIMAL(18,2),
		Febrero DECIMAL(18,2) NULL,
		Marzo DECIMAL(18,2),
		Abril DECIMAL(18,2),
		Mayo DECIMAL(18,2),
		Junio DECIMAL(18,2),
		Julio DECIMAL(18,2),
		Agosto DECIMAL(18,2),
		Septiembre DECIMAL(18,2),
		Octubre DECIMAL(18,2),
		Noviembre DECIMAL(18,2),
		Diciembre DECIMAL(18,2)
	);

	-- Crear la tabla de ListaDetalle para la salida
	DECLARE @ListaDetalle TABLE(
		ConfiguracionPresupuestoDetalleId INT NOT NULL,
		ConfiguracionPresupuestoId INT NOT NULL,
		ClasificadorId INT NULL,
		MIRIndicadorId INT NOT NULL,
		MIRIndicadorComponenteId INT NULL,
		Enero DECIMAL(18, 2) NULL,
		Febrero DECIMAL(18, 2) NULL,
		Marzo DECIMAL(18, 2) NULL,
		Abril DECIMAL(18, 2) NULL,
		Mayo DECIMAL(18, 2) NULL,
		Junio DECIMAL(18, 2) NULL,
		Julio DECIMAL(18, 2) NULL,
		Agosto DECIMAL(18, 2) NULL,
		Septiembre DECIMAL(18, 2) NULL,
		Octubre DECIMAL(18, 2) NULL,
		Noviembre DECIMAL(18, 2) NULL,
		Diciembre DECIMAL(18, 2) NULL,
		Anual DECIMAL(18, 2) NULL,
		Porcentaje DECIMAL(18, 2) NULL,
		EstatusId INT NOT NULL,
		Componente NVARCHAR(30) NULL,
		Actividad NVARCHAR(30),
		EsProyecto BIT NULL,
		MontoIndicador DECIMAL(18,2) NULL,
		CabeceraMes DECIMAL(18,2) NULL,
		CabeceraAnual DECIMAL(18,2) NULL,
		MontoProyecto DECIMAL(18,2) NULL,
		InicioMes NVARCHAR(20) NULL,
		EsEditado BIT NULL,
		TotalPorcentaje DECIMAL(18,2) NULL
	);

	-- Crear la tabla de ListaMIRIGrupoActividad
	DECLARE @ListaMIRIGrupoActividad TABLE(
		MIRIndicadorComponenteId INT NULL,
		Enero DECIMAL(18, 2) NULL,
		Febrero DECIMAL(18, 2) NULL,
		Marzo DECIMAL(18, 2) NULL,
		Abril DECIMAL(18, 2) NULL,
		Mayo DECIMAL(18, 2) NULL,
		Junio DECIMAL(18, 2) NULL,
		Julio DECIMAL(18, 2) NULL,
		Agosto DECIMAL(18, 2) NULL,
		Septiembre DECIMAL(18, 2) NULL,
		Octubre DECIMAL(18, 2) NULL,
		Noviembre DECIMAL(18, 2) NULL,
		Diciembre DECIMAL(18, 2) NULL
	);

	-- Convertir el arreglo de string a array como tabla y establecer al tabla de proyecto para enviar el SP MIspConsultaRepProyecto
	INSERT INTO @ListaProyecto(ProyectoId)
		(SELECT miri.ProyectoId FROM MItblMatrizIndicadorResultadoIndicador miri WHERE MIRId = @MIRId AND miri.NivelIndicadorId IN (42,43) AND miri.TipoComponenteId IN (54, 55) AND ProyectoId != '' AND miri.EstatusId = 1 GROUP BY miri.ProyectoId);
	
	SET @ProyectoIdsComma = (SELECT STRING_AGG(ProyectoId, ',') FROM @ListaProyecto);
	
	-- Establecer al tabla de RepProyecto en el SP MIspConsultaRepProyecto para obtener los datos
	INSERT INTO @ListaRepProyecto(ProyectoId, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre, Anual)
		EXEC MIspConsultaRepProyecto  @ProyectoIdsComma, @Ejercicio
	


	-- Establecer al tabla de MIRI
	INSERT INTO @ListaMIRI(MIRIndicadorId, NivelIndicadorId, NombreIndicador, TipoComponenteId, MIRIndicadorComponenteId, ProyectoId, PorcentajeProyecto, MontoProyecto, PorcentajeActividad, MontoActividad, EstatusId)
		(SELECT miri.MIRIndicadorId, miri.NivelIndicadorId, miri.NombreIndicador, miri.TipoComponenteId, miri.MIRIndicadorComponenteId, miri.ProyectoId, miri.PorcentajeProyecto, miri.MontoProyecto, miri.PorcentajeActividad, miri.MontoActividad, miri.EstatusId FROM MItblMatrizIndicadorResultadoIndicador miri WHERE MIRId = @MIRId AND miri.NivelIndicadorId IN (42, 43) AND miri.EstatusId = 1)

	-- Obtenemos el ID de Configuracion Presupuesto
	SET @ConfiguracionPresupuestoId = ISNULL((SELECT cp.ConfiguracionPresupuestoId FROM MItblMatrizConfiguracionPresupuestal cp WHERE cp.MIRId = @MIRId AND cp.EstatusId = 1), -1);

	-- Crear el cursor para el ID del componentes en cada como arreglo (ciclo) (solo los componentes)
	DECLARE ListaMIRIComponente CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI miri WHERE miri.NivelIndicadorId = 42;
	-- Abrir el cursor para empezar el ciclo
	OPEN ListaMIRIComponente;
	-- Para el proximo MIRIndicadorId
	FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Cambiamos los valores por DEFAULT
		SET @ContadorActividad = 1; SET @ProyectoId = ''; SET @Porcentaje = 0; SET @EsProyecto = 0;

		-- Verificar si el componente tiene el ID de Proyecto para establecer si no en actividad
		--SELECT @ProyectoId = miri.ProyectoId, @Porcentaje = CASE WHEN miri.PorcentajeProyecto IS NOT NULL THEN miri.PorcentajeProyecto ELSE 0 END, @EsProyecto = CASE WHEN miri.TipoComponenteId = 55 THEN 1 ELSE 0 END FROM @ListaMIRI miri WHERE miri.NivelIndicadorId = 42 AND miri.MIRIndicadorId = @MIRIndicadorId;
		SELECT @ProyectoId = miri.ProyectoId, @EsProyecto = CASE WHEN miri.TipoComponenteId = 55 THEN 1 ELSE 0 END, @Componente = CONCAT('C', @ContadorComponente, ' - ', miri.NombreIndicador) FROM @ListaMIRI miri WHERE miri.NivelIndicadorId = 42 AND miri.MIRIndicadorId = @MIRIndicadorId;
		-- Establecer al tabla de ListaMIRIActividad solo las actividades
		INSERT INTO @ListaMIRIActividad(MIRIndicadorId, NivelIndicadorId, NombreIndicador, ProyectoId, PorcentajeProyecto, MontoProyecto, PorcentajeActividad, MontoActividad)
			(SELECT miri.MIRIndicadorId, miri.NivelIndicadorId, miri.NombreIndicador, miri.ProyectoId, miri.PorcentajeProyecto, miri.MontoProyecto, miri.PorcentajeActividad, miri.MontoActividad FROM @ListaMIRI miri WHERE miri.MIRIndicadorComponenteId = @MIRIndicadorId AND miri.EstatusId = 1);

		-- Crear el cursor para el ID del Actividades en cada como arreglo (ciclo) (solo las actividades)
		DECLARE ListaMIRIActividad CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRIActividad miri;
		-- Abrir el cursor para empezar el ciclo
		OPEN ListaMIRIActividad;
		-- Para el proximo MIRIndicadorId
		FETCH NEXT FROM ListaMIRIActividad INTO @MIRIndicadorId_;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @MontoProyecto DECIMAL(18,2) = 0,
				 @MontoIndicador DECIMAL(18,2);

			SET @Actividad = CONCAT('Actividad ', @ContadorActividad);

			-- Relacion Componente
			IF(@EsProyecto = 'true')
				BEGIN
					SET @MontoProyecto = (SELECT p.Anual FROM @ListaRepProyecto p WHERE p.ProyectoId = @ProyectoId);
					SET @Porcentaje = (SELECT miri.PorcentajeActividad FROM @ListaMIRIActividad miri WHERE miri.MIRIndicadorId = @MIRIndicadorId_);
					SET @MontoIndicador = CAST(@MontoProyecto * (@Porcentaje / 100) AS DECIMAL(18,2));
					SET @TotalPorcentaje = (SELECT SUM(miri.PorcentajeActividad) FROM @ListaMIRIActividad miri);
				END
			ELSE -- Relacion Actividad
				BEGIN
					SELECT @ProyectoId = miri.ProyectoId, @Porcentaje = miri.PorcentajeProyecto FROM @ListaMIRIActividad miri WHERE miri.MIRIndicadorId = @MIRIndicadorId_;
					SET @MontoProyecto = (SELECT p.Anual FROM @ListaRepProyecto p WHERE p.ProyectoId = @ProyectoId);
					SET @MontoIndicador = CAST(@MontoProyecto * (@Porcentaje / 100) AS DECIMAL(18,2));
					SET @TotalPorcentaje = (SELECT SUM(miri.PorcentajeProyecto) FROM @ListaMIRIActividad miri);
				END
			
			IF EXISTS(SELECT * FROM MItblMatrizConfiguracionPresupuestalDetalle pd WHERE pd.MIRIndicadorId = @MIRIndicadorId_ AND pd.EstatusId = 1)
				BEGIN
					
					INSERT INTO @ListaDetalle(ConfiguracionPresupuestoDetalleId, ConfiguracionPresupuestoId, ClasificadorId, MIRIndicadorId, MIRIndicadorComponenteId, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre, Anual, Porcentaje, EstatusId, Componente, Actividad, EsProyecto, MontoIndicador, MontoProyecto, TotalPorcentaje)
						(SELECT pd.ConfiguracionPresupuestoDetalleId, pd.ConfiguracionPresupuestoId, pd.ClasificadorId, @MIRIndicadorId_, @MIRIndicadorId,  pd.Enero, pd.Febrero, pd.Marzo, pd.Abril, pd.Mayo, pd.Junio, pd.Julio, pd.Agosto, pd.Septiembre, pd.Octubre, pd.Noviembre, pd.Diciembre, pd.Anual, @Porcentaje, pd.EstatusId, @Componente, @Actividad, @EsProyecto, @MontoIndicador, @MontoProyecto, @TotalPorcentaje FROM MItblMatrizConfiguracionPresupuestalDetalle pd WHERE pd.MIRIndicadorId = @MIRIndicadorId_ AND pd.EstatusId = 1)

				END
			ELSE
				BEGIN
					INSERT INTO @ListaDetalle(ConfiguracionPresupuestoDetalleId, ConfiguracionPresupuestoId, ClasificadorId, MIRIndicadorId, MIRIndicadorComponenteId, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre, Anual, Porcentaje, EstatusId, Componente, Actividad, EsProyecto, MontoIndicador, MontoProyecto, TotalPorcentaje)
						VALUES(@ContadorRegistrosNuevos, @ConfiguracionPresupuestoId, 56, @MIRIndicadorId_, @MIRIndicadorId, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, @Porcentaje, 1, @Componente, @Actividad, @EsProyecto, @MontoIndicador, @MontoProyecto, @TotalPorcentaje)

					-- Descontador el registro
					SET @ContadorRegistrosNuevos = @ContadorRegistrosNuevos - 1;
				END

			-- Contador
			SET @ContadorActividad = @ContadorActividad + 1;

			FETCH NEXT FROM ListaMIRIActividad INTO @MIRIndicadorId_;
		END
		-- Cerrar el cursor
		CLOSE ListaMIRIActividad;
		DEALLOCATE ListaMIRIActividad;
		
		-- Actualizamos por la cabecera anual
		UPDATE @ListaDetalle SET CabeceraAnual = (SELECT SUM(ISNULL(d.MontoIndicador, 0)) FROM @ListaDetalle d WHERE d.MIRIndicadorComponenteId = @MIRIndicadorId) WHERE MIRIndicadorComponenteId = @MIRIndicadorId;
		
		-- Asignamos el dato al tabla de ListaMIRIGrupoActividad para hacer un ciclo para saber cual es iniciar de mes
		INSERT INTO @ListaMIRIGrupoActividad(MIRIndicadorComponenteId, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
			(SELECT @MIRIndicadorId, SUM(d.Enero), SUM(d.Febrero), SUM(d.Marzo), SUM(d.Abril), SUM(d.Mayo), SUM(d.Junio), SUM(d.Julio), SUM(d.Agosto), SUM(d.Septiembre), SUM(d.Octubre), SUM(d.Noviembre), SUM(d.Diciembre) FROM @ListaDetalle d WHERE d.MIRIndicadorComponenteId = @MIRIndicadorId)

		-- Relacion Componente
		IF(@EsProyecto = 'true')
			BEGIN
				-- Asignamos el Anual con el ID de Proyecto y con el porcentaje
				SET @Anual = (SELECT TOP 1 d.CabeceraAnual FROM @ListaDetalle d WHERE d.MIRIndicadorComponenteId = @MIRIndicadorId);

				-- Hacer los calculos (ponderacion) de los indicadores originales.
				SET @CadaMes = CAST(@Anual / 12 AS DECIMAL(18,2));
				SET	@UltimoMes = 0;

				IF((@CadaMes * 12) = @Anual)
					SET @UltimoMes = @CadaMes;
				ELSE
					BEGIN
						IF((@CadaMes * 12) > @Anual)
							SET @UltimoMes = @CadaMes - ((@CadaMes * 12) - @Anual);
						ELSE
							SET @UltimoMes = @CadaMes + (@Anual - (@CadaMes * 12));
					END

				INSERT INTO @ListaPonderacion(MIRIndicadorComponenteId, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
					VALUES(@MIRIndicadorId, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @UltimoMes);

			END
		ELSE -- Relacion Actividad
			BEGIN
				SET @Anual = (SELECT TOP 1 d.CabeceraAnual FROM @ListaDetalle d WHERE d.MIRIndicadorComponenteId = @MIRIndicadorId);

				-- Hacer los calculos (ponderacion) de los indicadores originales.
				SET @CadaMes = CAST(@Anual / 12 AS DECIMAL(18,2));
				SET @UltimoMes = 0;

				IF((@CadaMes * 12) = @Anual)
					SET @UltimoMes = @CadaMes;
				ELSE
					BEGIN
						IF((@CadaMes * 12) > @Anual)
							SET @UltimoMes = @CadaMes - ((@CadaMes * 12) - @Anual);
						ELSE
							SET @UltimoMes = @CadaMes + (@Anual - (@CadaMes * 12));
					END

				INSERT INTO @ListaPonderacion(MIRIndicadorComponenteId, Enero, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre)
					VALUES(@MIRIndicadorId, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @CadaMes, @UltimoMes);

			END		

		-- Contador
		SET @ContadorComponente = @ContadorComponente + 1;
		
		FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
		-- Borrar la tabla de ListaMIRIActividad para la proxima nueva tabla
		DELETE FROM @ListaMIRIActividad;
	END
	-- Cerrar el cursor
	CLOSE ListaMIRIComponente
	DEALLOCATE ListaMIRIComponente

	-- Este es como funcion para saber cual es inicio de mes
	-- Crear el cursor para el ID del componentes en cada como arreglo (ciclo) (solo los componentes)
	DECLARE ListaMIRIComponente CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI miri WHERE miri.NivelIndicadorId = 42;
	-- Abrir el cursor para empezar el ciclo
	OPEN ListaMIRIComponente;
	-- Para el proximo MIRIndicadorId
	FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
	DECLARE @EsBucleParada BIT = 0, @ContadorBucleMes INT = 1;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		WHILE @ContadorBucleMes <= 12
		BEGIN
			SELECT @Enero = ga.Enero, @Febrero = ga.Febrero, @Marzo = ga.Marzo, @Abril = ga.Abril, @Mayo = ga.Mayo, @Junio = ga.Junio, @Julio = ga.Julio, @Agosto = ga.Agosto, @Septiembre = ga.Septiembre, @Octubre = ga.Octubre, @Noviembre = ga.Noviembre, @Diciembre = ga.Diciembre FROM @ListaMIRIGrupoActividad ga WHERE ga.MIRIndicadorComponenteId = @MIRIndicadorId;

			IF(@ContadorBucleMes = 1 AND @Enero IS NULL)
				BEGIN
					SET @InicioMes = 'Enero'; SET @EsBucleParada = 1; SET @ContadorMes = 1; BREAK;
				END
			IF(@ContadorBucleMes = 2 AND @Febrero IS NULL)
				BEGIN
					SET @InicioMes = 'Febrero'; SET @EsBucleParada = 1; SET @ContadorMes = 2; BREAK;
				END
			IF(@ContadorBucleMes = 3 AND @Marzo IS NULL)
				BEGIN
					SET @InicioMes = 'Marzo'; SET @EsBucleParada = 1; SET @ContadorMes = 3; BREAK;
				END
			IF(@ContadorBucleMes = 4 AND @Abril IS NULL)
				BEGIN
					SET @InicioMes = 'Abril'; SET @EsBucleParada = 1; SET @ContadorMes = 4; BREAK;
				END
			IF(@ContadorBucleMes = 5 AND @Mayo IS NULL)
				BEGIN
					SET @InicioMes = 'Mayo'; SET @EsBucleParada = 1; SET @ContadorMes = 5; BREAK;
				END
			IF(@ContadorBucleMes = 6 AND @Junio IS NULL)
				BEGIN
					SET @InicioMes = 'Junio'; SET @EsBucleParada = 1; SET @ContadorMes = 6; BREAK;
				END
			IF(@ContadorBucleMes = 7 AND @Julio IS NULL)
				BEGIN
					SET @InicioMes = 'Julio'; SET @EsBucleParada = 1; SET @ContadorMes = 7; BREAK;
				END
			IF(@ContadorBucleMes = 8 AND @Agosto IS NULL)
				BEGIN
					SET @InicioMes = 'Agoso'; SET @EsBucleParada = 1; SET @ContadorMes = 8; BREAK;
				END
			IF(@ContadorBucleMes = 9 AND @Septiembre IS NULL)
				BEGIN
					SET @InicioMes = 'Septiembre'; SET @EsBucleParada = 1; SET @ContadorMes = 9; BREAK;
				END
			IF(@ContadorBucleMes = 10 AND @Octubre IS NULL)
				BEGIN
					SET @InicioMes = 'Octubre'; SET @EsBucleParada = 1; SET @ContadorMes = 10; BREAK;
				END
			IF(@ContadorBucleMes = 11 AND @Noviembre IS NULL)
				BEGIN
					SET @InicioMes = 'Noviembre'; SET @EsBucleParada = 1; SET @ContadorMes = 11; BREAK;
				END
			IF(@ContadorBucleMes = 12 AND @Diciembre IS NULL)
				BEGIN
					SET @InicioMes = 'Diciembre'; SET @EsBucleParada = 1; SET @ContadorMes = 12; BREAK;
				END

			-- Cambiamos los valores
			SET @ContadorBucleMes = @ContadorBucleMes + 1;
			SET @ContadorMes = 0;
		END

		-- Si existe hay mes en ciclo de mes para parada el ciclo de grupo actividad
		IF(@EsBucleParada = 'true' OR @ContadorBucleMes = 12)
		BEGIN
			BREAK;
		END
			
		FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
		-- Borrar la tabla de ListaMIRIActividad para la proxima nueva tabla
	END
	-- Cerrar el cursor
	CLOSE ListaMIRIComponente
	DEALLOCATE ListaMIRIComponente

	-- Actualizamos el campo InicioMes
	IF(@InicioMes != '')
		UPDATE @ListaDetalle SET InicioMes = @InicioMes;

	-- Este es como un funcion para saber si los indicadores es editado o no para modificar el monto
	-- Crear el cursor para el ID del componentes en cada como arreglo (ciclo) (solo los componentes)
	DECLARE ListaMIRIComponente CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI miri WHERE miri.NivelIndicadorId = 42;
	-- Abrir el cursor para empezar el ciclo
	OPEN ListaMIRIComponente;
	-- Para el proximo MIRIndicadorId
	FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Verificar si los indicadores ha modificado que no estar de acuerdo de ponderacion (editado)
		SET @EsEditado = 0;
		DECLARE @Enero_ DECIMAL(18,2), @Febrero_ DECIMAL(18,2), @Marzo_ DECIMAL(18,2), @Abril_ DECIMAL(18,2), @Mayo_ DECIMAL(18,2), @Junio_ DECIMAL(18,2), @Julio_ DECIMAL(18,2), @Agosto_ DECIMAL(18,2), @Septiembre_ DECIMAL(18,2), @Octubre_ DECIMAL(18,2), @Noviembre_ DECIMAL(18,2), @Diciembre_ DECIMAL(18,2);
		-- Establecen Enero a Diciembre
		SELECT @Enero = ga.Enero, @Febrero = ga.Febrero, @Marzo = ga.Marzo, @Abril = ga.Abril, @Mayo = ga.Mayo, @Junio = ga.Junio, @Julio = ga.Julio, @Agosto = ga.Agosto, @Septiembre = ga.Septiembre, @Octubre = ga.Octubre, @Noviembre = ga.Noviembre, @Diciembre = ga.Diciembre FROM @ListaMIRIGrupoActividad ga WHERE ga.MIRIndicadorComponenteId = @MIRIndicadorId;
		SELECT @Enero_ = p.Enero, @Febrero_ = p.Febrero, @Marzo_ = p.Marzo, @Abril_ = p.Abril, @Mayo_ = p.Mayo, @Junio_ = p.Junio, @Julio_ = p.Julio, @Agosto_ = p.Agosto, @Septiembre_ = p.Septiembre, @Octubre_ = p.Octubre, @Noviembre_ = p.Noviembre, @Diciembre_ = p.Diciembre FROM @ListaPonderacion p WHERE p.MIRIndicadorComponenteId = @MIRIndicadorId;
		-- Cambiamos el valor
		SET @EsCicloMes = 1;

		WHILE @EsCicloMes = 'true'
		BEGIN
			-- Enero
			IF (@ContadorMes <= 1)
				BREAK;
			ELSE
				BEGIN
					IF (@Enero != @Enero_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Febrero
			IF (@ContadorMes <= 2)
				BREAK;
			ELSE
				BEGIN
					IF (@Febrero != @Febrero_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END
				END
			-- Marzo
			IF (@ContadorMes <= 3)
				BREAK;
			ELSE
				BEGIN
					IF (@Marzo != @Marzo_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Abril
			IF (@ContadorMes <= 4)
				BREAK;
			ELSE
				BEGIN
					IF (@Abril != @Abril_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Mayo
			IF (@ContadorMes <= 5)
				BREAK;
			ELSE
				BEGIN
					IF (@Mayo != @Mayo_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Junio
			IF (@ContadorMes <= 6)
				BREAK;
			ELSE
				BEGIN
					IF (@Junio != @Junio_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Julio
			IF (@ContadorMes <= 7)
				BREAK;
			ELSE
				BEGIN
					IF (@Julio != @Julio_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Agosto
			IF (@ContadorMes <= 8)
				BREAK;
			ELSE
				BEGIN
					IF (@Agosto != @Agosto_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Septiembre
			IF (@ContadorMes <= 9)
				BREAK;
			ELSE
				BEGIN
					IF (@Septiembre != @Septiembre_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Octubre
			IF (@ContadorMes <= 10)
				BREAK;
			ELSE
				BEGIN
					IF (@Octubre != @Octubre_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Noviembre
			IF (@ContadorMes <= 11)
				BREAK;
			ELSE
				BEGIN
					IF (@Noviembre != @Noviembre_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Diciembre
			IF (@ContadorMes <= 12)
				BREAK;
			ELSE
				BEGIN
					IF (@Diciembre != @Diciembre_)
						BEGIN
							SET @EsEditado = 1; BREAK;
						END	
				END
			-- Cambiamos el valor
			SET @EsCicloMes = 0;
		END

		-- Actualizamos el campo EsEditado
		UPDATE @ListaDetalle SET EsEditado = @EsEditado WHERE MIRIndicadorComponenteId = @MIRIndicadorId;

		FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
	END
	-- Cerrar el cursor
	CLOSE ListaMIRIComponente
	DEALLOCATE ListaMIRIComponente

	-- Este es como un funcion para hacer el calculo del mes.
	-- Crear el cursor para el ID del componentes en cada como arreglo (ciclo) (solo los componentes)
	DECLARE ListaMIRIComponente CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI miri WHERE miri.NivelIndicadorId = 42;
	-- Abrir el cursor para empezar el ciclo
	OPEN ListaMIRIComponente;
	-- Para el proximo MIRIndicadorId
	FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @AnualOriginal DECIMAL(18,2) = 0, @TotalMeses DECIMAL(18,2) = 0;
		-- Cambiamos los valores por default
		SET @InicioMesMonto = 0;
		-- Verificar si los indicadores ha modificado que no estar de acuerdo de ponderacion (editado)
		SELECT TOP 1 @EsEditado = d.EsEditado, @EsProyecto = d.EsProyecto, @MontoProyecto = d.MontoProyecto FROM @ListaDetalle d WHERE d.MIRIndicadorComponenteId = @MIRIndicadorId;
		
		SELECT @Enero = SUM(ISNULL(d.Enero, 0)), @Febrero = SUM(ISNULL(d.Febrero, 0)), @Marzo = SUM(ISNULL(d.Marzo, 0)), @Abril = SUM(ISNULL(d.Abril, 0)), @Mayo = SUM(ISNULL(d.Mayo, 0)), @Junio = SUM(ISNULL(d.Junio, 0)), @Julio = SUM(ISNULL(d.Julio, 0)), @Agosto = SUM(ISNULL(d.Agosto, 0)), @Septiembre = SUM(ISNULL(d.Septiembre, 0)), @Octubre = SUM(ISNULL(d.Octubre, 0)), @Noviembre = SUM(ISNULL(d.Noviembre, 0)), @Diciembre = SUM(ISNULL(d.Diciembre, 0)), @Anual = SUM(ISNULL(d.MontoIndicador, 0)) FROM @ListaDetalle d WHERE d.MIRIndicadorComponenteId = @MIRIndicadorId;
		SET @AnualOriginal = @Anual;
		SET @Anual = @Anual - (@Enero + @Febrero + @Marzo + @Abril + @Mayo + @Junio + @Julio + @Agosto + @Septiembre + @Octubre + @Noviembre + @Diciembre);
		SET @CadaMes = CAST(@Anual / (13 - @ContadorMes) AS DECIMAL(18,2));
		SET @TotalMeses = CAST(@CadaMes * (13 - @ContadorMes) AS DECIMAL(18,2));
		SET @UltimoMes = 0;
		IF(@TotalMeses = @Anual)
			SET @UltimoMes = @CadaMes;
		ELSE
			BEGIN
				IF(@TotalMeses > @Anual)
					SET @UltimoMes = @CadaMes - (@TotalMeses - @Anual);
				ELSE
					SET @UltimoMes = @CadaMes + (@Anual - @TotalMeses);
			END

		-- Actualizamos el campo CabeceraMes
		IF(@ContadorMes = 12)
			BEGIN
				UPDATE @ListaDetalle SET CabeceraMes = @UltimoMes WHERE MIRIndicadorComponenteId = @MIRIndicadorId;
				SET @InicioMesMonto = @UltimoMes;
			END
						
		ELSE
			BEGIN
				UPDATE @ListaDetalle SET CabeceraMes = @CadaMes WHERE MIRIndicadorComponenteId = @MIRIndicadorId;
				SET @InicioMesMonto = @CadaMes;
			END

		-- Asignamos cuantos hay indicadores como actividades y otros variables por default
		SET @ContadorIndicador = (SELECT COUNT(*) FROM @ListaMIRI miri WHERE miri.MIRIndicadorComponenteId = @MIRIndicadorId);
		SET @ContadorIndicador_ = 1;
		SET @TotalFilaMonto = 0;
		SET @EsUltimaActividad = 0;	
						
		-- Este es como un funcion mientras para hacer los calculos de las actividades por el monto y despues otro un funcion para hacer la ultima actividad con el monto que no se queda unos centavos.
		-- Crear el cursor para el ID del Actividades en cada como arreglo (ciclo) (solo las actividades)
		DECLARE ListaMIRIActividad CURSOR FOR SELECT miri.MIRIndicadorId FROM @ListaMIRI miri WHERE miri.MIRIndicadorComponenteId = @MIRIndicadorId;
		-- Abrir el cursor para empezar el ciclo
		OPEN ListaMIRIActividad;
		-- Para el proximo MIRIndicadorId
		FETCH NEXT FROM ListaMIRIActividad INTO @MIRIndicadorId_;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SELECT @Porcentaje = d.Porcentaje FROM @ListaDetalle d WHERE d.MIRIndicadorId = @MIRIndicadorId_;
			IF(@EsEditado = 'true')
				BEGIN
					SELECT @Enero = ISNULL(d.Enero, 0), @Febrero = ISNULL(d.Febrero, 0), @Marzo = ISNULL(d.Marzo, 0), @Abril = ISNULL(d.Abril, 0), @Mayo = ISNULL(d.Mayo, 0), @Junio = ISNULL(d.Junio, 0), @Julio = ISNULL(d.Julio, 0), @Agosto = ISNULL(d.Agosto, 0), @Septiembre = ISNULL(d.Septiembre, 0), @Octubre = ISNULL(d.Octubre, 0), @Noviembre = ISNULL(d.Noviembre, 0), @Diciembre = ISNULL(d.Diciembre, 0), @Anual = ISNULL(d.MontoIndicador, 0) FROM @ListaDetalle d WHERE d.MIRIndicadorId = @MIRIndicadorId_;
					SET @TotalAnual = @Enero + @Febrero + @Marzo + @Abril + @Mayo + @Junio + @Julio + @Agosto + @Septiembre + @Octubre + @Noviembre + @Diciembre;
					SET @CadaMes = CAST(@InicioMesMonto / @ContadorIndicador AS DECIMAL(18,2));
					SET @TotalMeses = CAST(@CadaMes * @ContadorIndicador AS DECIMAL(18,2));
					SET @UltimoMes = 0;
					IF(@TotalMeses = @InicioMesMonto)
						SET @UltimoMes = @CadaMes;
					ELSE
						BEGIN
							IF(@TotalMeses > @InicioMesMonto)
								SET @UltimoMes = @CadaMes - (@TotalMeses - @InicioMesMonto);
							ELSE
								SET @UltimoMes = @CadaMes + (@InicioMesMonto - @TotalMeses);
						END
				END
			ELSE
				BEGIN
					SELECT @Enero = ISNULL(d.Enero, 0), @Febrero = ISNULL(d.Febrero, 0), @Marzo = ISNULL(d.Marzo, 0), @Abril = ISNULL(d.Abril, 0), @Mayo = ISNULL(d.Mayo, 0), @Junio = ISNULL(d.Junio, 0), @Julio = ISNULL(d.Julio, 0), @Agosto = ISNULL(d.Agosto, 0), @Septiembre = ISNULL(d.Septiembre, 0), @Octubre = ISNULL(d.Octubre, 0), @Noviembre = ISNULL(d.Noviembre, 0), @Diciembre = ISNULL(d.Diciembre, 0), @Anual = ISNULL(d.MontoIndicador, 0) FROM @ListaDetalle d WHERE d.MIRIndicadorId = @MIRIndicadorId_;
					SET @TotalAnual = @Enero + @Febrero + @Marzo + @Abril + @Mayo + @Junio + @Julio + @Agosto + @Septiembre + @Octubre + @Noviembre + @Diciembre;
					SET @Anual = @Anual - @TotalAnual;
					SET @CadaMes = CAST(@Anual / (13 - @ContadorMes) AS DECIMAL(18,2));
					SET @TotalMeses = CAST(@CadaMes * (13 - @ContadorMes) AS DECIMAL(18,2));
					SET @UltimoMes = 0;
					IF(@TotalMeses = @Anual)
						SET @UltimoMes = @CadaMes;
					ELSE
						BEGIN
							IF(@TotalMeses > @Anual)
								SET @UltimoMes = @CadaMes - (@TotalMeses - @Anual);
							ELSE
								SET @UltimoMes = @CadaMes + (@Anual - @TotalMeses);
						END
				END

			SET @FilaMonto = CASE WHEN @ContadorMes = 12 THEN @UltimoMes ELSE @CadaMes END;

			SET @TotalFilaMonto = @TotalFilaMonto + @FilaMonto;

			IF(@ContadorIndicador = @ContadorIndicador_)
				BEGIN
					-- Cambiamos el valor porque esta es ultima actividad
					SET @EsUltimaActividad = 1;

					-- Verificar si el monto es ultima actividad para que no queda los centavos.
					IF(@TotalFilaMonto = @InicioMesMonto)
						BEGIN
							SET @UltimaFilaMonto = @FilaMonto;
						END
									
					ELSE
						BEGIN
							IF(@TotalFilaMonto > @InicioMesMonto)
								SET @UltimaFilaMonto = @FilaMonto - (@TotalFilaMonto - @InicioMesMonto);
							ELSE
								SET @UltimaFilaMonto = @FilaMonto + (@InicioMesMonto - @TotalFilaMonto);
						END
				END

			DECLARE @Monto DECIMAL(18,2) = CASE WHEN @EsUltimaActividad = 'true' THEN @UltimaFilaMonto ELSE @FilaMonto END;
			SET @TotalAnual = @TotalAnual + @Monto;

			-- Actualizamos el CabeceraMes con el que inicio de mes
			-- Enero
			IF(@ContadorMes = 1)
				UPDATE @ListaDetalle SET Enero = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Febrero
			IF(@ContadorMes = 2)
				UPDATE @ListaDetalle SET Febrero = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Marzo
			IF(@ContadorMes = 3)
				UPDATE @ListaDetalle SET Marzo = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Abril
			IF(@ContadorMes = 4)
				UPDATE @ListaDetalle SET Abril = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Mayo
			IF(@ContadorMes = 5)
				UPDATE @ListaDetalle SET Mayo = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Junio
			IF(@ContadorMes = 6)
				UPDATE @ListaDetalle SET Junio = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Julio
			IF(@ContadorMes = 7)
				UPDATE @ListaDetalle SET Julio = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Agosto
			IF(@ContadorMes = 8)
				UPDATE @ListaDetalle SET Agosto = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Septiembre
			IF(@ContadorMes = 9)
				UPDATE @ListaDetalle SET Septiembre = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Octubre
			IF(@ContadorMes = 10)
				UPDATE @ListaDetalle SET Octubre = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Noviembre
			IF(@ContadorMes = 11)
				UPDATE @ListaDetalle SET Noviembre = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;
			-- Diciembre
			IF(@ContadorMes = 12)
				UPDATE @ListaDetalle SET Diciembre = @Monto, Anual = @TotalAnual WHERE MIRIndicadorId = @MIRIndicadorId_;

			-- Contador
			SET @ContadorIndicador_ = @ContadorIndicador_ + 1;

			FETCH NEXT FROM ListaMIRIActividad INTO @MIRIndicadorId_;
		END
		-- Cerrar el cursor
		CLOSE ListaMIRIActividad;
		DEALLOCATE ListaMIRIActividad;

		FETCH NEXT FROM ListaMIRIComponente INTO @MIRIndicadorId;
	END
	-- Cerrar el cursor
	CLOSE ListaMIRIComponente
	DEALLOCATE ListaMIRIComponente
	
	-- Salida
	SELECT * FROM @ListaDetalle;
GO