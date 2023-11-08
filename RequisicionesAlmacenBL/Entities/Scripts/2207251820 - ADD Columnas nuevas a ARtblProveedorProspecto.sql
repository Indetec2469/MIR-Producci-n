ALTER TABLE ARtblProveedorProspecto
ADD ProveedorId	INT NULL,
    Domicilio VARCHAR(250) NULL,
	PaisId CHAR(2) NULL,
	EstadoId NVARCHAR(2) NULL,
    MunicipioId INT NULL,
    TipoProveedorId CHAR(2) NULL,
    TipoOperacionId CHAR(2) NULL,
    TipoComprobanteFiscalId INT NULL
GO

ALTER TABLE ARtblProveedorProspecto WITH CHECK ADD CONSTRAINT FK_ARtblProveedorProspecto_tblProveedor FOREIGN KEY(ProveedorId)
REFERENCES tblProveedor (ProveedorId)
GO
ALTER TABLE ARtblProveedorProspecto CHECK CONSTRAINT FK_ARtblProveedorProspecto_tblProveedor
GO

ALTER TABLE ARtblProveedorProspecto WITH CHECK ADD CONSTRAINT FK_ARtblProveedorProspecto_tblPais FOREIGN KEY(PaisId)
REFERENCES tblPais (PaisId)
GO
ALTER TABLE ARtblProveedorProspecto CHECK CONSTRAINT FK_ARtblProveedorProspecto_tblPais
GO

ALTER TABLE ARtblProveedorProspecto WITH CHECK ADD CONSTRAINT FK_ARtblProveedorProspecto_tblEstado FOREIGN KEY(EstadoId)
REFERENCES tblEstado (EstadoId)
GO
ALTER TABLE ARtblProveedorProspecto CHECK CONSTRAINT FK_ARtblProveedorProspecto_tblEstado
GO

ALTER TABLE ARtblProveedorProspecto WITH CHECK ADD CONSTRAINT FK_ARtblProveedorProspecto_tblMunicipio FOREIGN KEY(MunicipioId)
REFERENCES tblMunicipio (MunicipioId)
GO
ALTER TABLE ARtblProveedorProspecto CHECK CONSTRAINT FK_ARtblProveedorProspecto_tblMunicipio
GO

ALTER TABLE ARtblProveedorProspecto WITH CHECK ADD CONSTRAINT FK_ARtblProveedorProspecto_tblTipoProveedor FOREIGN KEY(TipoProveedorId)
REFERENCES tblTipoProveedor (TipoProveedorId)
GO
ALTER TABLE ARtblProveedorProspecto CHECK CONSTRAINT FK_ARtblProveedorProspecto_tblTipoProveedor
GO

ALTER TABLE ARtblProveedorProspecto WITH CHECK ADD CONSTRAINT FK_ARtblProveedorProspecto_tblTipoOperacion FOREIGN KEY(TipoOperacionId)
REFERENCES tblTipoOperacion (TipoOperacionId)
GO
ALTER TABLE ARtblProveedorProspecto CHECK CONSTRAINT FK_ARtblProveedorProspecto_tblTipoOperacion
GO

ALTER TABLE ARtblProveedorProspecto WITH CHECK ADD CONSTRAINT FK_ARtblProveedorProspecto_tblTipoComprobanteFiscal FOREIGN KEY(TipoComprobanteFiscalId)
REFERENCES tblTipoComprobanteFiscal (TipoComprobanteFiscalId)
GO
ALTER TABLE ARtblProveedorProspecto CHECK CONSTRAINT FK_ARtblProveedorProspecto_tblTipoComprobanteFiscal
GO

UPDATE pp
  SET
      pp.Convertido = 1,
      pp.ProveedorId = p.ProveedorId,
	  pp.Domicilio = p.Domicilio,
	  pp.PaisId = p.PaisId,
	  pp.EstadoId = p.EstadoId,
	  pp.MunicipioId = p.MunicipioId,
	  pp.TipoProveedorId = p.TipoProveedorId,
	  pp.TipoOperacionId = p.TipoOperacionId,
	  pp.TipoComprobanteFiscalId = p.TipoComprobanteFiscalId,
	  pp.FechaUltimaModificacion = GETDATE()
FROM ARtblProveedorProspecto AS pp
     INNER JOIN tblProveedor AS p ON pp.RFC = p.RFC
WHERE pp.EstatusId = 1
GO

ALTER TABLE ARtblInvitacionCompraProveedor DROP CONSTRAINT IF EXISTS FK_ARtblInvitacionCompraProveedor_tblProveedor
GO

ALTER TABLE ArtblInvitacionCompraDetallePrecioProveedor DROP CONSTRAINT IF EXISTS FK_ArtblInvitacionCompraDetallePrecioProveedor_tblProveedor
GO

/*
ALTER TABLE ARtblProveedorProspecto
ALTER COLUMN PaisId CHAR(2) NOT NULL
GO

ALTER TABLE ARtblProveedorProspecto
ALTER COLUMN EstadoId NVARCHAR(2) NOT NULL
GO

ALTER TABLE ARtblProveedorProspecto
ALTER COLUMN MunicipioId INT NOT NULL
GO

ALTER TABLE ARtblProveedorProspecto
ALTER COLUMN TipoProveedorId CHAR(2) NOT NULL
GO

ALTER TABLE ARtblProveedorProspecto
ALTER COLUMN TipoOperacionId CHAR(2) NOT NULL
GO

ALTER TABLE ARtblProveedorProspecto
ALTER COLUMN TipoComprobanteFiscalId INT NOT NULL
GO
*/