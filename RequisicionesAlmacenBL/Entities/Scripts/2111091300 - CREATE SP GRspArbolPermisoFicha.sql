SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* ****************************************************************
 * GRspArbolPermisoFicha
 * ****************************************************************
 * Descripción: Obtener el arbol de Permiso Ficha como los nodos
 * Autor: Rene Carrillo
 * Fecha: 09.11.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA:
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE PROCEDURE [dbo].[GRspArbolPermisoFicha]
AS
	-- Este es las variables para los necesito
	DECLARE @ContadorRegistro INT = -1,
			@NodoMenuId INT,
			@PermisoFichaId_ INT;
	-- Este es la tabla de ListaArbolPermisoFicha para salida con los datos.
	DECLARE @ListaArbolPermisoFicha TABLE(
		PermisoFichaId INT NOT NULL,
		Etiqueta NVARCHAR(200) NOT NULL,
		Descripcion NVARCHAR(500) NULL,
		PermisoFichaPadreId INT NULL
	);
	-- Este es las variables para obtener las tablas de MenuPrincipal y PermisoFicha
	DECLARE @PermisoFichaId INT, @PermisoFichaPadreId INT, @Etiqueta NVARCHAR(200), @Descripcion NVARCHAR(500);

	-- Crear el cursor para el ID del MIRI en cada como arreglo (ciclo)
	DECLARE ListaNodoMenuId CURSOR FOR SELECT pf.NodoMenuId FROM GRtblPermisoFicha pf WHERE pf.EstatusId = 1 GROUP BY pf.NodoMenuId;
	-- Abrir el cursor para empezar el ciclo
	OPEN ListaNodoMenuId;
	-- Para el proximo @NodoMenuId (Ciclo)
	FETCH NEXT FROM ListaNodoMenuId INTO @NodoMenuId;
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		-- Establecer las variables para insertar a la tabla de GrtblMenuPrincipal
		(SELECT  @Etiqueta = mp.Etiqueta, @Descripcion = mp.Descripcion FROM GRtblMenuPrincipal mp WHERE mp.NodoMenuId = @NodoMenuId AND mp.EstatusId = 1);
		-- Insertar la tabla de ListaArbolPermisoFicha
		INSERT INTO @ListaArbolPermisoFicha (PermisoFichaId, Etiqueta, Descripcion)
			VALUES(@ContadorRegistro, @Etiqueta, @Descripcion);
		-- Establecer PermisoFichaPadreId el ID de ContadorRegistro
		SET @PermisoFichaPadreId = @ContadorRegistro
		-- Contador
		SET @ContadorRegistro = @ContadorRegistro - 1;

		-- Crear el cursor para el ID del MIRI en cada como arreglo (ciclo)
		DECLARE ListaPermisoFicha CURSOR FOR SELECT pf.PermisoFichaId FROM GRtblPermisoFicha pf WHERE pf.NodoMenuId = @NodoMenuId AND pf.EstatusId = 1;
		-- Abrir el cursor para empezar el ciclo
		OPEN ListaPermisoFicha;
		-- Para el proximo @PermisoFichaId_
		FETCH NEXT FROM ListaPermisoFicha INTO @PermisoFichaId_;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Establecer las variables
			(SELECT @PermisoFichaId = pf.PermisoFichaId, @Etiqueta = pf.Etiqueta, @Descripcion = pf.Descripcion FROM GRtblPermisoFicha pf WHERE pf.PermisoFichaId = @PermisoFichaId_);
			-- Insertar la tabla de ListaVariableIndicdor
			INSERT INTO @ListaArbolPermisoFicha (PermisoFichaId, Etiqueta, Descripcion, PermisoFichaPadreId)
				VALUES(@PermisoFichaId, @Etiqueta, @Descripcion, @PermisoFichaPadreId);
			-- Para el proximo @@PermisoFichaId_ (Ciclo)						
			FETCH NEXT FROM ListaPermisoFicha INTO @PermisoFichaId_;
		END
		-- Cerrar el cursor
		CLOSE ListaPermisoFicha;
		DEALLOCATE ListaPermisoFicha;
		-- Para el proximo @NodoMenuId (Ciclo)
		FETCH NEXT FROM ListaNodoMenuId INTO @NodoMenuId
	END
	-- Cerrar el cursor
	CLOSE ListaNodoMenuId;
	DEALLOCATE ListaNodoMenuId;
	-- Salida con la tabla de ListaArbolPermisoFicha
	SELECT * FROM @ListaArbolPermisoFicha;
GO