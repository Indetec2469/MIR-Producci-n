SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ****************************************************************
 * spObtenerFechaDelServidor
 * ****************************************************************
 * Descripción: Obtener la fecha del servidor
 * Autor: Rene Carrillo
 * Fecha: 09.12.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: 
 * PARAMETROS DE SALIDA: Fecha del Servidor
 *****************************************************************
*/

CREATE PROCEDURE [dbo].[spObtenerFechaDelServidor] 
AS
	SELECT GETDATE() AS FechaDelServidor