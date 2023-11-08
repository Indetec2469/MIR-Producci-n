SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ****************************************************************
 * MIspConsultaRepProyecto
 * ****************************************************************
 * Autor: Rene Carrillo
 * Fecha: 03.01.2022
 * Fecha de Modificación: 17.02.2022
 * Versión: 1.0.1
 *****************************************************************
 * PARAMETROS DE ENTRADA: ProyectoIds, Anio
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/ 
--exec [MIspConsultaRepProyecto] '001001, 001002, 002001','2021'
ALTER   PROCEDURE [dbo].[MIspConsultaRepProyecto] 
		@ProyectoIds as VARCHAR(MAX),
		@Anio AS VARCHAR(4)
AS
	-- Tablas
	DECLARE @ListaProyecto TABLE(
		ProyectoId NVARCHAR(6) NULL
	);
	DECLARE @ListaDevengado TABLE(
		ProyectoId NVARCHAR(6) NULL,
		Enero DECIMAL(19,6) NULL,
		Febrero DECIMAL(19,6) NULL,
		Marzo DECIMAL(19,6) NULL,
		Abril DECIMAL(19,6) NULL,
		Mayo DECIMAL(19,6) NULL,
		Junio DECIMAL(19,6) NULL,
		Julio DECIMAL(19,6) NULL,
		Agosto DECIMAL(19,6) NULL,
		Septiembre DECIMAL(19,6) NULL,
		Octubre DECIMAL(19,6) NULL,
		Noviembre DECIMAL(19,6) NULL,
		Diciembre DECIMAL(19,6) NULL,
		Anual DECIMAL(19,6) NULL
	);
	DECLARE @ListaProyectoDevengado TABLE(
		ProyectoId NVARCHAR(6) NULL,
		Nombre NVARCHAR(MAX) NULL,
		PresupuestoVigente DECIMAL(19,6) NULL,
		Devengado DECIMAL(19,6) NULL
	);

	-- Variables
	DECLARE @Contador INT = 1,
	@FechaInicio DATETIME = @Anio + '-01-01 00:00:00.000',
	@FechaFin DATETIME,
	@Mes NVARCHAR(20),
	@ProyectoId_ NVARCHAR(6);

	-- Establecer fin la fecha
	-- SET @FechaFin = DATEADD(DD, -1, DATEADD(MM, 1, @FechaInicio));
	SET @FechaFin = DATEADD(SS, -1, DATEADD(MM, 1, @FechaInicio));

	-- Convertir el arreglo de string a array como tabla y establecer al tabla de proyecto
	INSERT INTO @ListaProyecto(ProyectoId)
		(SELECT * FROM [dbo].[GRfnSplitString](@ProyectoIds, ','));

	-- Insertar la tabla de devengado por los proyectos
	INSERT INTO @ListaDevengado(ProyectoId)
		(SELECT ProyectoId FROM @ListaProyecto);

	-----------------------------------------
	--// Filtro de las claves presupuestarias
	-----------------------------------------
	DECLARE @statement as NVARCHAR(3000)

	--// Elimina la tabla
	IF OBJECT_ID('tempdb..##tblCuentaPresupuestalEgr_1') IS NOT NULL
	BEGIN
		DROP TABLE ##tblCuentaPresupuestalEgr_1
	END

	--// Elimina la tabla
	IF OBJECT_ID('tempdb..##tblCuentaPresupuestalEgr') IS NOT NULL
	BEGIN
		DROP TABLE ##tblCuentaPresupuestalEgr
	END

	--// Elimina la tabla
	IF OBJECT_ID('tempdb..##tmpCatalogoCuentaPolizaDet') IS NOT NULL BEGIN
		DROP TABLE ##tmpCatalogoCuentaPolizaDet 
	END


	--// Construye la sentencia de todas las claves y asi filtrar por proyecto
	SET @statement = N'
	SELECT CuentaPresupuestalEgrId, RamoId, ProyectoId, DependenciaId, ObjetoGastoId, TipoGastoId
	INTO ##tblCuentaPresupuestalEgr_1 
	FROM tblCuentaPresupuestalEgr'

	EXEC dbo.sp_executesql @statement	

	--obtiene solo las claves creadas en el plan de cuentas
	SELECT DISTINCT C.*
	INTO ##tblCuentaPresupuestalEgr
	FROM ##tblCuentaPresupuestalEgr_1 C
	INNER JOIN tblCuentaPresupuestalEgr_CatalogoCuenta X 
			ON C.CuentaPresupuestalEgrId = X.CuentaPresupuestalEgrId

	--Temporal que solo obtiene polizas con cuenta presupuestal 82 para excluir polizas innecesarias
	SELECT CatalogoCuentaId,cuenta,ejercicio,status,cargo,abono,fecha
	INTO ##tmpCatalogoCuentaPolizaDet
	FROM vwPolizaDet
	WHERE  LEFT(Cuenta,2) = '82'

	-- Ciclo Contador (12 meses)
	WHILE @Contador <= 12
	BEGIN

		-- Establecer mes
		SET @mes = 
			CASE
				WHEN @contador = 1 THEN 'Enero'
				WHEN @contador = 2 THEN 'Febrero'
				WHEN @contador = 3 THEN 'Marzo'
				WHEN @contador = 4 THEN 'Abril'
				WHEN @contador = 5 THEN 'Mayo'
				WHEN @contador = 6 THEN 'Junio'
				WHEN @contador = 7 THEN 'Julio'
				WHEN @contador = 8 THEN 'Agosto'
				WHEN @contador = 9 THEN 'Septiembre'
				WHEN @contador = 10 THEN 'Octubre'
				WHEN @contador = 11 THEN 'Noviembre'
				WHEN @contador = 12 THEN 'Diciembre'
			END;

		PRINT @fechaInicio;
		PRINT @fechaFin;
		--// Elimina las tablas que generan las columnas
		IF OBJECT_ID('tempdb..##tmpAprobado') IS NOT NULL
		BEGIN
			DROP TABLE ##tmpAprobado
		END
		IF OBJECT_ID('tempdb..##tmpModificaciones') IS NOT NULL
		BEGIN
			DROP TABLE ##tmpModificaciones
		END
		IF OBJECT_ID('tempdb..##tmpDevengado') IS NOT NULL
		BEGIN
			DROP TABLE ##tmpDevengado
		END

		--// Aprobado
		SELECT CP.ProyectoId, 
					SUM(Abono) - SUM(Cargo) AS Aprobado		
			INTO ##tmpAprobado 
			FROM tblObjetoGasto O
			INNER JOIN ##tblCuentaPresupuestalEgr CP 
					ON O.ObjetoGastoId = CP.ObjetoGastoId
			INNER JOIN tblCuentaPresupuestalEgr_CatalogoCuenta X 
					ON CP.CuentaPresupuestalEgrId = X.CuentaPresupuestalEgrId
			INNER JOIN ##tmpCatalogoCuentaPolizaDet C
					ON X.CatalogoCuentaId = C.CatalogoCuentaId
				WHERE C.Ejercicio = YEAR(@fechaFin)
					AND C.Status not in ('C','E') AND Convert(Char(10), Fecha, 101) <= @FechaFin
					AND LEFT(C.Cuenta,3) = '821'
			GROUP BY CP.ProyectoId
		--// Modificaciones
		SELECT CP.ProyectoId,
					SUM(Abono) - SUM(Cargo) AS Modificaciones
			INTO ##tmpModificaciones
			FROM tblObjetoGasto O
			INNER JOIN ##tblCuentaPresupuestalEgr CP 
					ON O.ObjetoGastoId = CP.ObjetoGastoId
			INNER JOIN tblCuentaPresupuestalEgr_CatalogoCuenta X 
					ON CP.CuentaPresupuestalEgrId = X.CuentaPresupuestalEgrId
			INNER JOIN ##tmpCatalogoCuentaPolizaDet C
					ON X.CatalogoCuentaId = C.CatalogoCuentaId
				WHERE C.Ejercicio = YEAR(@fechaFin)
					AND C.Status not in ('C','E') AND Convert(Char(10), Fecha, 101) <= @FechaFin
					AND LEFT(C.Cuenta,3) = '823'
			GROUP BY CP.ProyectoId

		--// Devengado 
		SELECT CP.ProyectoId,
					SUM(Cargo) AS Devengado
			INTO ##tmpDevengado
			FROM tblObjetoGasto O
			INNER JOIN ##tblCuentaPresupuestalEgr CP 
					ON O.ObjetoGastoId = CP.ObjetoGastoId
			INNER JOIN tblCuentaPresupuestalEgr_CatalogoCuenta X 
					ON CP.CuentaPresupuestalEgrId = X.CuentaPresupuestalEgrId
			INNER JOIN ##tmpCatalogoCuentaPolizaDet C
					ON X.CatalogoCuentaId = C.CatalogoCuentaId
				WHERE C.Ejercicio = YEAR(@fechaFin)
					AND C.Status not in ('C','E') AND Convert(Char(10), Fecha, 101) BETWEEN @FechaInicio AND @FechaFin
					AND LEFT(C.Cuenta,3) = '825'
			GROUP BY CP.ProyectoId


		-- Establecer la tabla de Proyecto Devengado
		--//Resultado
		INSERT INTO @ListaProyectoDevengado (ProyectoId, Nombre, PresupuestoVigente, Devengado)
			SELECT ff.ProyectoId, ff.Nombre,
					ISNULL(SUM(B.Aprobado),0) + ISNULL(SUM(C.Modificaciones),0) as PresupuestoVigente,
					ISNULL(SUM(F.Devengado),0) as Devengado
			FROM tblProyecto FF
			LEFT JOIN ##tmpAprobado B
					ON ff.ProyectoId = B.ProyectoId
			LEFT JOIN ##tmpModificaciones C
					ON ff.ProyectoId= C.ProyectoId 
			LEFT JOIN ##tmpDevengado F
					ON ff.ProyectoId = F.ProyectoId 
			GROUP BY ff.ProyectoId, ff.Nombre
			ORDER BY ff.ProyectoId;

		-- Crear el cursor para el ID del proyecto en cada como arreglo (ciclo)
		DECLARE ListaProyectoId CURSOR FOR SELECT p.ProyectoId FROM @ListaProyecto p;
		-- Abrir el cursor para empezar el ciclo
		OPEN ListaProyectoId;
		-- Para el proximo MIRIndicadorId
		FETCH NEXT FROM ListaProyectoId INTO @ProyectoId_;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @Valor DECIMAL(19,6) = (SELECT pd.Devengado FROM @ListaProyectoDevengado pd WHERE pd.ProyectoId = @ProyectoId_);
			-- Modificar la tabla de Devengado
			IF(@Contador = 1)
			BEGIN
				UPDATE @ListaDevengado SET Enero = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 2)
			BEGIN
				UPDATE @ListaDevengado SET Febrero = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 3)
			BEGIN
				UPDATE @ListaDevengado SET Marzo = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 4)
			BEGIN
				UPDATE @ListaDevengado SET Abril = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 5)
			BEGIN
				UPDATE @ListaDevengado SET Mayo = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 6)
			BEGIN
				UPDATE @ListaDevengado SET Junio = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 7)
			BEGIN
				UPDATE @ListaDevengado SET Julio = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 8)
			BEGIN
				UPDATE @ListaDevengado SET Agosto = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 9)
			BEGIN
				UPDATE @ListaDevengado SET Septiembre = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 10)
			BEGIN
				UPDATE @ListaDevengado SET Octubre = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 11)
			BEGIN
				UPDATE @ListaDevengado SET Noviembre = @Valor WHERE ProyectoId = @ProyectoId_
			END
			IF(@Contador = 12)
			BEGIN
				UPDATE @ListaDevengado SET Diciembre = @Valor, Anual = ISNULL(Enero, 0) + ISNULL(Febrero, 0) + ISNULL(Marzo, 0) + ISNULL(Abril, 0) + ISNULL(Mayo, 0) + ISNULL(Junio, 0) + ISNULL(Julio, 0) + ISNULL(Agosto, 0) + ISNULL(Septiembre, 0) + ISNULL(Octubre, 0) + ISNULL(Noviembre, 0) + ISNULL(@Valor, 0) WHERE ProyectoId = @ProyectoId_
			END
											
			FETCH NEXT FROM ListaProyectoId INTO @proyectoId_
		END
		-- Cerrar el cursor
		CLOSE ListaProyectoId
		DEALLOCATE ListaProyectoId
		-- Borrar la tabla de ListaMIR_ para la proxima nueva tabla
		DELETE FROM @ListaProyectoDevengado;

		SET @fechaInicio = DATEADD(SS, 1, @fechaFin);
		SET @fechaFin = DATEADD(SS, -1, DATEADD(MM, 1, @fechaInicio));
		SET @contador = @contador + 1;
	END;

	-- Salida
	SELECT * FROM @ListaDevengado