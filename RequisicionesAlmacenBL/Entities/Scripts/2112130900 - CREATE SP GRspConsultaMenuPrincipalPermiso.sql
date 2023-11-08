SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/* ****************************************************************
 * GRspConsultaMenuPrincipalPermiso
 * ****************************************************************
 * Descripción: Consulta para obtener los menus con permisos por el ID de ROL
 * Autor: Rene Carrillo
 * Fecha: 13.12.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: 
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE   PROCEDURE [dbo].[GRspConsultaMenuPrincipalPermiso]
	@RolId INT
AS
	-- Este es las variables para los necesito
	DECLARE @NodoMenuId INT, 
		@Etiqueta NVARCHAR(100),
		@Descripcion NVARCHAR(255),
		@NumeroCicloNivel INT = 1,
		@MIRIndicadorId INT,
		@MIRIndicadorFormulaVariableId INT,
		@MIRIndicadorFormulaVariableId_ INT,
		@MIRIndicadorIdActividad INT, 
		@NumeroNivelActividad INT,
		@Orden INT = 1;
	-- Este es la tabla de ListaMenuPrincipal para salida con los datos.
	DECLARE @ListaMenuPrincipal TABLE(
		NodoMenuId INT NOT NULL, 
		Etiqueta NVARCHAR(100) NOT NULL,
		Descripcion NVARCHAR(255) NULL,
		TipoNodoId INT NOT NULL,
		NodoPadreId INT NULL,
		Url NVARCHAR(255) NULL,
		Icono NVARCHAR(50) NULL,
		Orden TINYINT NOT NULL
	);

	DECLARE @ListaSubMenuPrincipal TABLE(
		NodoMenuId INT NOT NULL, 
		Etiqueta NVARCHAR(100) NOT NULL,
		Descripcion NVARCHAR(255) NULL,
		TipoNodoId INT NOT NULL,
		NodoPadreId INT NULL,
		Url NVARCHAR(255) NULL,
		Icono NVARCHAR(50) NULL,
		Orden TINYINT NOT NULL
	);

	DECLARE @ListaPadreMenuPrincipal TABLE(
		NodoMenuId INT NOT NULL, 
		Etiqueta NVARCHAR(100) NOT NULL,
		Descripcion NVARCHAR(255) NULL,
		TipoNodoId INT NOT NULL,
		NodoPadreId INT NULL,
		Url NVARCHAR(255) NULL,
		Icono NVARCHAR(50) NULL,
		Orden TINYINT NOT NULL
	);

	-- Establecer las variables para insertar a la tabla de ListaMenuPrincipal con el ID de ROL
	INSERT @ListaMenuPrincipal
		SELECT mp.NodoMenuId, mp.Etiqueta, mp.Descripcion, mp.TipoNodoId, mp.NodoPadreId, mp.Url, mp.Icono, mp.Orden 
		FROM GRtblMenuPrincipal mp
			INNER JOIN GRtblRolMenu rm ON mp.NodoMenuId = rm.NodoMenuId 
		WHERE rm.RolId = @RolId AND rm.EstatusId = 1 AND mp.EstatusId = 1;

	-- NodoPadreId (Sub Menu)
	INSERT @ListaSubMenuPrincipal
		SELECT mp.NodoMenuId, mp.Etiqueta, mp.Descripcion, mp.TipoNodoId, mp.NodoPadreId, mp.Url, mp.Icono, mp.Orden
		FROM GRtblMenuPrincipal mp
		WHERE mp.EstatusId = 1 AND mp.NodoMenuId IN (SELECT _mp.NodoPadreId
													FROM @ListaMenuPrincipal _mp
													GROUP BY _mp.NodoPadreId
													);
	
	-- NodoPadreId (Padre Menu)
	INSERT @ListaPadreMenuPrincipal
		SELECT mp.NodoMenuId, mp.Etiqueta, mp.Descripcion, mp.TipoNodoId, mp.NodoPadreId, mp.Url, mp.Icono, mp.Orden
		FROM GRtblMenuPrincipal mp
		WHERE mp.EstatusId = 1 AND mp.NodoMenuId IN (SELECT _mp.NodoPadreId
													FROM @ListaSubMenuPrincipal _mp
													GROUP BY _mp.NodoPadreId
													);

	INSERT @ListaMenuPrincipal
		SELECT mp.NodoMenuId, mp.Etiqueta, mp.Descripcion, mp.TipoNodoId, mp.NodoPadreId, mp.Url, mp.Icono, mp.Orden 
		FROM @ListaSubMenuPrincipal mp;

	INSERT @ListaMenuPrincipal
		SELECT mp.NodoMenuId, mp.Etiqueta, mp.Descripcion, mp.TipoNodoId, mp.NodoPadreId, mp.Url, mp.Icono, mp.Orden 
		FROM @ListaPadreMenuPrincipal mp;

	-- Salida con la tabla de ListaMenuPrincipal
	SELECT * FROM @ListaMenuPrincipal mp ORDER BY mp.Orden ASC
		
	
GO