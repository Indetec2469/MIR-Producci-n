USE [RequisicionesAlmacen]
GO

/****** Object:  StoredProcedure [dbo].[spConsultaUnidadMedidaConDimension]    Script Date: 7/1/2021 2:51:51 PM ******/
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
 * Versión: 1.0.1
 * Modificado: 01.07.2021
 *****************************************************************
 * PARAMETROS DE ENTRADA:
 * PARAMETROS DE SALIDA: Todos de Unidad Medida y DimensionId
 *****************************************************************
*/ 
SELECT 
	cmum.*, cmumd.DimensionId
FROM
	MItblControlMaestroUnidadMedida cmum
	INNER JOIN MItblControlMaestroUnidadMedidaDimension cmumd ON cmum.UnidadMedidaId = cmumd.UnidadMedidaId AND cmumd.Borrado = 0
WHERE 
	cmum.Borrado = 0
GO


