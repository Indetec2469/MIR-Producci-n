SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListadoTransferencias]
AS
-- ===========================================================
-- Author:		Javier Elías
-- Create date: 26/11/2021
-- Modified date: 
-- Description:	View para obtener el Listado de las Transferencias
-- ===========================================================
SELECT transferencia.TransferenciaId,
       transferencia.Codigo,
       dbo.GRfnGetFechaConFormato(transferencia.Fecha, 0) AS Fecha,
	   dbo.fn_GetNombreCompletoEmpleado(usuario.EmpleadoId) AS Usuario,
       almacen.Nombre AS AlmacenOrigen,
	   detalles.Movimientos AS NoMovimientos
FROM ARtblTransferencia AS transferencia
     INNER JOIN tblAlmacen AS almacen ON transferencia.AlmacenOrigenId = almacen.AlmacenId
	 INNER JOIN GRtblUsuario AS usuario ON transferencia.CreadoPorId = usuario.UsuarioId
	 INNER JOIN
	 (
			SELECT TransferenciaId,
				   COUNT(TransferenciaMovtoId) AS movimientos
			FROM ARtblTransferenciaMovto
			GROUP BY TransferenciaId
	 ) AS detalles ON transferencia.TransferenciaId = detalles.TransferenciaId
WHERE transferencia.EstatusId = 1 -- Activo
GO