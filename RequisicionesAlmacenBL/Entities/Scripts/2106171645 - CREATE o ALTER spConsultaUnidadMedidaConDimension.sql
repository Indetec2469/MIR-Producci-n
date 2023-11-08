SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [dbo].[spConsultaUnidadMedidaConDimension]
AS
/* ****************************************************************
 * spConsultaUnidadMedidaConDimension
 * ****************************************************************
 * Descripción: Buscamos los objetos de Unidad Medida con Dimension.
 * Autor: Rene Carrillo
 * Fecha: 17.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA:
 * PARAMETROS DE SALIDA: UnidadMedidaId, Descripcion, DimensionId
 *****************************************************************
*/ 
SELECT 
	cmum.UnidadMedidaId,
	cmum.Descripcion,
	cmumd.DimensionId
FROM
	MItblControlMaestroUnidadMedida cmum
	INNER JOIN MItblControlMaestroUnidadMedidaDimension cmumd ON cmum.UnidadMedidaId = cmumd.UnidadMedidaId AND cmumd.Borrado = 0
WHERE 
	cmum.Borrado = 0
GO