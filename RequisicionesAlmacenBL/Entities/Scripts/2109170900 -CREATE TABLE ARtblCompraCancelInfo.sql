SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ARtblCompraCancelInfo](
	[CompraCancelInfoId] [int] IDENTITY(1,1) NOT NULL,
	[CompraId] [int] NOT NULL,
	[FechaCancelacion] [datetime] NOT NULL,
	[Motivo] [varchar](5000) NOT NULL,
 CONSTRAINT [PK_ARtblCompraCancelInfo] PRIMARY KEY CLUSTERED 
(
	[CompraCancelInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ARtblCompraCancelInfo]  WITH CHECK ADD  CONSTRAINT [FK_ARtblCompraCancelInfo_tblCompra] FOREIGN KEY([CompraId])
REFERENCES [dbo].[tblCompra] ([CompraId])
GO

ALTER TABLE [dbo].[ARtblCompraCancelInfo] CHECK CONSTRAINT [FK_ARtblCompraCancelInfo_tblCompra]
GO


