SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConvertirProspectoProveedor]
	@prospectoId INT
AS
-- ==============================================================
-- Author:		Javier Elías
-- Create date: 25/07/2022
-- Modified date: 
-- Description:	Procedimiento para convertir un Prospecto en Proveedor
-- ==============================================================

-- Obtenemos el Id original del Prospecto
DECLARE @prospectoOriginalId INT = @prospectoId - 2000000

-- Insertamos el nuevo Proveedor
INSERT INTO tblProveedor
(
    --ProveedorId - this column value is auto-generated
    TipoProveedorId,
    PaisId,
    EstadoId,
    MunicipioId,
    RazonSocial,
    Status,
    RFC,
    TipoOperacionId,
    TipoComprobanteFiscalId,
    Domicilio,
    Telefono1,
    Email,
    Contacto1,
    Observaciones,
    Origen
)
SELECT TipoProveedorId,
       PaisId,
       EstadoId,
       MunicipioId,
       RazonSocial,
       'A', -- Status
	   RFC,
	   TipoOperacionId,
	   TipoComprobanteFiscalId,
	   Domicilio,
	   Telefono, -- Telefono1
	   CorreoElectronico, -- Email
	   NombreContacto + ' ' + PrimerApellido + ISNULL(' ' + SegundoApellido, ''), -- Contacto1
	   Comentarios, -- Observaciones
	   'R' -- Origen
FROM ARtblProveedorProspecto
WHERE ProveedorProspectoId = @prospectoOriginalId

-- Recuperamos el Id del Proveedor que se generó
DECLARE @proveedorId INT = (SELECT SCOPE_IDENTITY())

-- Actualizamos el Prospecto convertido
UPDATE ARtblProveedorProspecto SET Convertido = 1, ProveedorId = @proveedorId, FechaUltimaModificacion = GETDATE() WHERE ProveedorProspectoId = @prospectoOriginalId

-- Actualizamos los registros con el Id del nuevo Proveedor
UPDATE ARtblInvitacionCompraProveedor SET ProveedorId = @proveedorId WHERE ProveedorId = @prospectoId
UPDATE ArtblInvitacionCompraDetallePrecioProveedor SET ProveedorId = @proveedorId WHERE ProveedorId = @prospectoId