DROP TABLE IF EXISTS ARtblAutonumerico
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ARtblAutonumerico](
	[AutonumericoId] [tinyint] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Prefijo] [nvarchar](5) NULL,
	[Siguiente] [bigint] NOT NULL,
	[Ceros] [int] NOT NULL,
	[NodoMenuId] [int] NULL,
	[Ejercicio] [int] NULL,
	[Activo] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[CreadoPorId] [int] NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[ModificadoPorId] [int] NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ARtblAutonumerico] PRIMARY KEY CLUSTERED 
(
	[AutonumericoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ARtblAutonumerico] ADD  CONSTRAINT [DF_ARtblAutonumerico_Siguiente]  DEFAULT ((1)) FOR [Siguiente]
GO

ALTER TABLE [dbo].[ARtblAutonumerico] ADD  CONSTRAINT [DF_ARtblAutonumerico_Ceros]  DEFAULT ((0)) FOR [Ceros]
GO

ALTER TABLE [dbo].[ARtblAutonumerico] ADD  CONSTRAINT [DF_ARtblAutonumerico_Activo]  DEFAULT ((1)) FOR [Activo]
GO

ALTER TABLE [dbo].[ARtblAutonumerico] ADD  CONSTRAINT [DF_ARtblAutonumerico_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO

INSERT INTO ARtblAutonumerico
(
    Nombre,
    Prefijo,
    Siguiente,
    Ceros,
    NodoMenuId,
    Ejercicio,
    Activo,
    FechaCreacion
)
VALUES
('Prospecto a Proveedor', 'PAP', 1, 6, NULL, NULL, 1, GETDATE()),
('Solicitud de Materiales y Consumibles', 'SMC', 1, 6, NULL, 2021, 1, GETDATE()),
('Inventarios Físicos', 'IF', 1, 6, NULL, NULL, 1, GETDATE()),
('Ajuste de Inventario', 'AI', 1, 6, NULL, NULL, 1, GETDATE()),
('Plan Nacional de Desarrollo', 'PND', 1, 6, NULL, NULL, 1, GETDATE()),
('MIR', 'MIR', 1, 6, NULL, 2021, 1, GETDATE()),
('Nivel Fin', 'INFIN', 1, 6, NULL, NULL, 1, GETDATE()),
('Nivel Proposito', 'INPRO', 1, 6, NULL, NULL, 1, GETDATE()),
('Nivel Componente', 'INCOM', 1, 6, NULL, NULL, 1, GETDATE()),
('Nivel Actividad', 'INACT', 1, 6, NULL, NULL, 1, GETDATE())
GO