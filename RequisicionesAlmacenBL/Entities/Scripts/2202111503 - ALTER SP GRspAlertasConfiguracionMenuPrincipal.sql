SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GRspAlertasConfiguracionMenuPrincipal]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 03/02/2022
-- Modified date: 11/02/2022
-- Description:	Procedimiento para obtener los nodos del Menú Principal
--						que se cargan en la Ficha de Configuración de Alertas.
-- ============================================================
	DECLARE @tbl TABLE ( 
		NodoMenuId INT, 
		NodoPadreId INT, 
		Etiqueta VARCHAR(4000), 
		Nivel INT, 
		Mostrar BIT, 
		AlertaEtapaAccionId INT, 
		PermiteNotificacion BIT, 
		PermiteAutorizacion BIT, 
		Orden INT, 
		OrdenArbol VARCHAR(4000)
	)

	DECLARE @tblTemp TABLE (
		NodoMenuId INT, 
		NodoPadreId INT, 
		Mostrar BIT, 
		IdTemp INT, 
		Ordenamiento VARCHAR(4000)
	)

	INSERT INTO @tbl
	SELECT *
	FROM
	(
		SELECT NodoMenuId,
			   NodoPadreId,
			   Etiqueta,
			   CASE TipoNodoId WHEN 6 THEN 1 -- Módulo
			   WHEN 98 THEN 2 -- SubMódulo
			   WHEN 7 THEN 3 -- Ficha
			   END AS Nivel,
			   0 AS Mostrar,
			   NULL AS AlertaEtapaAccionId,
			   0 AS PermiteNotificacion,
			   0 AS PermiteAutorizacion,
			   Orden,
			   NULL AS OrdenArbol
		FROM GRtblMenuPrincipal
		WHERE EstatusId = 1
    
		UNION ALL
    
		SELECT *,
			   ROW_NUMBER() OVER(ORDER BY Etiqueta) AS Orden,
			   NULL AS OrdenArbol
		FROM
		(
			SELECT DISTINCT ControlId AS NodoMenuId,
						 definicion.NodoMenuId AS NodoPadreId,
						 REPLACE(Valor, Etiqueta+'. ', '') AS Etiqueta,
						 4 AS Nivel,
						 0 AS Mostrar,
						 NULL AS AlertaEtapaAccionId,
						 0 AS PermiteNotificacion,
						 0 AS PermiteAutorizacion
			FROM GRtblMenuPrincipal AS menu
				 INNER JOIN GRtblAlertaDefinicion AS definicion ON menu.NodoMenuId = definicion.NodoMenuId AND Borrado = 0
				 INNER JOIN GRtblAlertaEtapaAccion AS etapaAccion ON definicion.AlertaEtapaAccionId = etapaAccion.AlertaEtapaAccionId
				 INNER JOIN GRtblControlMaestro AS etapa ON etapaAccion.EtapaId = etapa.ControlId
			WHERE EstatusId = 1
		) AS todo
    
		UNION ALL
    
		SELECT ControlId AS NodoMenuId,
			   EtapaId AS NodoPadreId,
			   Valor AS Etiqueta,
			   5 AS Nivel,
			   1 AS Mostrar,
			   etapaAccion.AlertaEtapaAccionId,
			   PermiteNotificacion,
			   PermiteAutorizacion,
			   ROW_NUMBER() OVER(ORDER BY Valor) AS Orden,
			   NULL AS OrdenArbol
		FROM GRtblMenuPrincipal AS menu
			 INNER JOIN GRtblAlertaDefinicion AS definicion ON menu.NodoMenuId = definicion.NodoMenuId AND Borrado = 0
			 INNER JOIN GRtblAlertaEtapaAccion AS etapaAccion ON definicion.AlertaEtapaAccionId = etapaAccion.AlertaEtapaAccionId
			 INNER JOIN GRtblControlMaestro AS accion ON etapaAccion.AccionId = accion.ControlId
		WHERE EstatusId = 1
	) AS todo
	ORDER BY Nivel,
			 NodoPadreId,
			 Orden;

	-- Árbol del Menú Principal
	WITH cte
	AS 
	(
		SELECT tbl.*,
			CAST(RIGHT('00000' + Ltrim(Rtrim(ROW_NUMBER() OVER(ORDER BY tbl.Orden))),5) AS NVARCHAR(255)) AS Ordenamiento
		FROM @tbl AS tbl
		WHERE NodoPadreId IS NULL

		UNION ALL
	
		SELECT tbl.*,
			CAST(cte.Ordenamiento + '.' + RIGHT('00000' + Ltrim(Rtrim(ROW_NUMBER() OVER(ORDER BY tbl.Orden))),5) AS NVARCHAR(255)) AS Ordenamiento
		FROM @tbl AS tbl
			INNER JOIN cte ON tbl.NodoPadreId = cte.NodoMenuId
	)

	INSERT INTO @tblTemp
	SELECT NodoMenuId,
		   NodoPadreId,
		   Mostrar,
		   ROW_NUMBER() OVER(ORDER BY Ordenamiento DESC) AS IdTemp,
		   Ordenamiento
	FROM cte

	DECLARE @contador INT = 1
	DECLARE @noRegistros INT = ( SELECT COUNT(IdTemp) FROM @tblTemp )

	DECLARE @nodoMenuId INT
	DECLARE @padreId INT
	DECLARE @mostrar BIT
	DECLARE @ordenArbol VARCHAR(4000)

	WHILE ( @contador <= @noRegistros )
	BEGIN
			SELECT @nodoMenuId = NodoMenuId, @padreId = NodoPadreId, @mostrar = Mostrar, @ordenArbol = Ordenamiento FROM @tblTemp WHERE IdTemp = @contador

			UPDATE @tbl SET OrdenArbol = @ordenArbol WHERE NodoMenuId = @nodoMenuId

			IF ( @mostrar = 1 )
			BEGIN
					UPDATE @tbl SET Mostrar = 1 WHERE NodoMenuId = @padreId
					UPDATE @tblTemp SET Mostrar = 1 WHERE NodoMenuId = @padreId
			END

			SET @contador = @contador + 1
	END

	SELECT NodoMenuId,
		   NodoPadreId,
		   Etiqueta,
		   Nivel,
		   AlertaEtapaAccionId,
		   PermiteNotificacion,
		   PermiteAutorizacion,
		   Orden
	FROM @tbl
	WHERE Mostrar = 1
	ORDER BY OrdenArbol
GO