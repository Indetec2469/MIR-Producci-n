SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--exec [MIspRep_Poryecto] '01/01/2020','12/31/2020',''
CREATE OR ALTER PROCEDURE [dbo].[MIspRep_Proyecto] 
		@fechaIni as DATETIME,
		@fechaFin as DATETIME,
		@where as VARCHAR(1000)
		
AS
/* ****************************************************************
 * spConsultaMatrizIndicadorResultado
 * ****************************************************************
 * Autor: Rene Carrillo
 * Fecha: 13.07.2021
 * Versión: 1.0.0
 *****************************************************************
*/ 
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
FROM tblCuentaPresupuestalEgr ' + @where

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
			AND C.Status not in ('C','E') AND Convert(Char(10), Fecha, 101) <= @fechaFin
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
			AND C.Status not in ('C','E') AND Convert(Char(10), Fecha, 101) <= @fechaFin
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
			AND C.Status not in ('C','E') AND Convert(Char(10), Fecha, 101) BETWEEN @fechaIni AND @fechaFin
			AND LEFT(C.Cuenta,3) = '825'
	GROUP BY CP.ProyectoId



--//Resultado
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
ORDER BY ff.ProyectoId

GO