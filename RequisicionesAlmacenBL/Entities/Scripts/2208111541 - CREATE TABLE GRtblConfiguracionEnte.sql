DROP TABLE IF EXISTS GRtblConfiguracionEnte
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GRtblConfiguracionEnte](
	[ConfiguracionEnteId] [int] IDENTITY(1,1) NOT NULL,
	
	[TipoConfiguracionFechaId] [int] NOT NULL,
	[FechaOperacion] [date] NULL,

	[EstatusId] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT PK_GRtblConfiguracionEnte PRIMARY KEY CLUSTERED 
(
	[ConfiguracionEnteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GRtblConfiguracionEnte]  WITH CHECK ADD  CONSTRAINT [FK_GRtblConfiguracionEnte_TipoConfiguracionFecha] FOREIGN KEY([TipoConfiguracionFechaId])
REFERENCES [dbo].[GRtblControlMaestro] ([ControlId])
GO
ALTER TABLE [dbo].[GRtblConfiguracionEnte] CHECK CONSTRAINT [FK_GRtblConfiguracionEnte_TipoConfiguracionFecha]
GO

ALTER TABLE [dbo].[GRtblConfiguracionEnte]  WITH CHECK ADD  CONSTRAINT [FK_GRtblConfiguracionEnte_Estatus] FOREIGN KEY([EstatusId])
REFERENCES [dbo].[GRtblControlMaestro] ([ControlId])
GO
ALTER TABLE [dbo].[GRtblConfiguracionEnte] CHECK CONSTRAINT [FK_GRtblConfiguracionEnte_Estatus]
GO

ALTER TABLE [dbo].[GRtblConfiguracionEnte]  WITH CHECK ADD  CONSTRAINT [FK_GRtblConfiguracionEnte_CreadoPor] FOREIGN KEY([CreadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[GRtblConfiguracionEnte] CHECK CONSTRAINT [FK_GRtblConfiguracionEnte_CreadoPor]
GO

ALTER TABLE [dbo].[GRtblConfiguracionEnte]  WITH CHECK ADD  CONSTRAINT [FK_GRtblConfiguracionEnte_ModificadoPor] FOREIGN KEY([ModificadoPorId])
REFERENCES [dbo].[GRtblUsuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[GRtblConfiguracionEnte] CHECK CONSTRAINT [FK_GRtblConfiguracionEnte_ModificadoPor]
GO

INSERT INTO GRtblConfiguracionEnte
VALUES
(
    -- ConfiguracionEnteId - int
    137, -- TipoConfiguracionFechaId - int
    NULL, -- FechaOperacion - date
    1, -- EstatusId - int
    GETDATE(), -- FechaCreacion - datetime
    NULL, -- CreadoPorId - int
    NULL, -- FechaUltimaModificacion - datetime
    NULL, -- ModificadoPorId - int
    NULL -- Timestamp - timestamp
)
GO