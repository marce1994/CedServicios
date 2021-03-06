SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [varchar](50) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Telefono] [varchar](50) NOT NULL,
	[Email] [varchar](128) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Pregunta] [varchar](256) NOT NULL,
	[Respuesta] [varchar](256) NOT NULL,
	[CantidadEnviosMail] [int] NOT NULL,
	[FechaUltimoReenvioMail] [datetime] NOT NULL,
	[EmailSMS] [varchar](50) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[UltActualiz] [timestamp] NOT NULL,
 CONSTRAINT [PK_Table_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Medio](
	[IdMedio] [varchar](15) NOT NULL,
	[DescrMedio] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Table_Medio] PRIMARY KEY CLUSTERED 
(
	[IdMedio] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cuit](
	[Cuit] [varchar](11) NOT NULL,
	[RazonSocial] [varchar](50) NOT NULL,
	[Calle] [varchar](30) NOT NULL,
	[Nro] [varchar](6) NOT NULL,
	[Piso] [varchar](5) NOT NULL,
	[Depto] [varchar](5) NOT NULL,
	[Sector] [varchar](5) NOT NULL,
	[Torre] [varchar](5) NOT NULL,
	[Manzana] [varchar](5) NOT NULL,
	[Localidad] [varchar](25) NOT NULL,
	[IdProvincia] [varchar](2) NOT NULL,
	[DescrProvincia] [varchar](50) NOT NULL,
	[CodPost] [varchar](8) NOT NULL,
	[NombreContacto] [varchar](25) NOT NULL,
	[EmailContacto] [varchar](60) NOT NULL,
	[TelefonoContacto] [varchar](50) NOT NULL,
	[IdCondIVA] [numeric](2, 0) NOT NULL,
	[DescrCondIVA] [varchar](50) NOT NULL,
	[NroIngBrutos] [varchar](13) NOT NULL,
	[IdCondIngBrutos] [numeric](2, 0) NOT NULL,
	[DescrCondIngBrutos] [varchar](50) NOT NULL,
	[GLN] [numeric](13, 0) NOT NULL,
	[FechaInicioActividades] [datetime] NOT NULL,
	[CodigoInterno] [varchar](20) NOT NULL,
	[IdMedio] [varchar](15) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[UltActualiz] [timestamp] NOT NULL,
 CONSTRAINT [PK_Table_Cuit] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UN](
	[Cuit] [varchar](11) NOT NULL,
	[IdUN] [int] NOT NULL,
	[DescrUN] [varchar](50) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[UltActualiz] [timestamp] NOT NULL,
 CONSTRAINT [PK_Table_UN] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC, 
	[IdUN] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoPuntoVta](
	[IdTipoPuntoVta] [varchar](15) NOT NULL,
 CONSTRAINT [PK_TipoPuntoVta] PRIMARY KEY CLUSTERED 
(
	[IdTipoPuntoVta] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetodoGeneracionNumeracionLote](
	[IdMetodoGeneracionNumeracionLote] [varchar](15) NOT NULL,
	[DescrMetodoGeneracionNumeracionLote] [varchar](128) NOT NULL,
 CONSTRAINT [PK_MetodoGeneracionNumeracionLote] PRIMARY KEY CLUSTERED 
(
	[IdMetodoGeneracionNumeracionLote] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PuntoVta](
	[Cuit] [varchar](11) NOT NULL,
	[NroPuntoVta] [numeric](4) NOT NULL,
	[IdUN] [int] NOT NULL,
	[IdTipoPuntoVta] [varchar](15) NOT NULL,
	[UsaSetPropioDeDatosCuit] [bit] NOT NULL,
	[Calle] [varchar](30) NOT NULL,
	[Nro] [varchar](6) NOT NULL,
	[Piso] [varchar](5) NOT NULL,
	[Depto] [varchar](5) NOT NULL,
	[Sector] [varchar](5) NOT NULL,
	[Torre] [varchar](5) NOT NULL,
	[Manzana] [varchar](5) NOT NULL,
	[Localidad] [varchar](25) NOT NULL,
	[IdProvincia] [varchar](2) NOT NULL,
	[DescrProvincia] [varchar](50) NOT NULL,
	[CodPost] [varchar](8) NOT NULL,
	[NombreContacto] [varchar](25) NOT NULL,
	[EmailContacto] [varchar](60) NOT NULL,
	[TelefonoContacto] [varchar](50) NOT NULL,
	[IdCondIVA] [numeric](2, 0) NOT NULL,
	[DescrCondIVA] [varchar](50) NOT NULL,
	[NroIngBrutos] [varchar](13) NOT NULL,
	[IdCondIngBrutos] [numeric](2, 0) NOT NULL,
	[DescrCondIngBrutos] [varchar](50) NOT NULL,
	[GLN] [numeric](13, 0) NOT NULL,
	[FechaInicioActividades] [datetime] NOT NULL,
	[CodigoInterno] [varchar](20) NOT NULL,
	[IdMetodoGeneracionNumeracionLote] [varchar](15) NOT NULL,
	[UltNroLote] [numeric](14, 0) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[UltActualiz] [timestamp] NOT NULL,
 CONSTRAINT [PK_Table_PuntoVta] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC, 
	[NroPuntoVta] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoPermiso](
	[IdTipoPermiso] [varchar](15) NOT NULL,
	[DescrTipoPermiso] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TipoPermiso] PRIMARY KEY CLUSTERED 
(
	[IdTipoPermiso] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permiso](
	[IdUsuario] [varchar](50) NOT NULL,
	[Cuit] [varchar](11) NOT NULL,
	[IdUN] [int] NOT NULL,
	[IdTipoPermiso] [varchar](15) NOT NULL,
	[FechaFinVigencia] [datetime] NOT NULL,
	[IdUsuarioSolicitante] [varchar](50) NOT NULL,
	[AccionTipo] [varchar](15) NOT NULL,
	[AccionNro] [int] NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Table_Permiso] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC,
	[Cuit] ASC,
	[IdUN] ASC,
	[IdTipoPermiso] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Configuracion](
	[IdUsuario] [varchar](50) NOT NULL,
	[Cuit] [varchar](11) NOT NULL,
	[IdUN] [int] NOT NULL,
	[IdTipoPermiso] [varchar](15) NOT NULL,
	[IdItemConfig] [varchar](50) NOT NULL,
	[Valor] [varchar](256) NOT NULL,
 CONSTRAINT [PK_Configuracion] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC,
	[Cuit] ASC,
	[IdUN] ASC,
	[IdTipoPermiso] ASC,
	[IdItemConfig] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Log](
	[IdLog] [int] IDENTITY(1,1) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[IdUsuario] [varchar](50) NOT NULL,
	[Entidad] [varchar](15) NOT NULL,
	[Evento] [varchar](15) NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[Comentario] [varchar](256) NOT NULL,
 CONSTRAINT [PK_Table_Log] PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogDetalle](
	[IdLogDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdLog] [int] NOT NULL,
	[TipoDetalle] [varchar](50) NOT NULL,
	[Detalle] [text] NOT NULL,
 CONSTRAINT [PK_Table_LogDetalle] PRIMARY KEY CLUSTERED 
(
	[IdLogDetalle] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cliente](
	[Cuit] [varchar](11) NOT NULL,
	[IdTipoDoc] [numeric](2, 0) NOT NULL,
	[NroDoc] [numeric](11, 0) NOT NULL,
	[IdCliente] [varchar](50) NOT NULL,
	[DesambiguacionCuitPais] [int] NOT NULL,
	[RazonSocial] [varchar](50) NOT NULL,
	[DescrTipoDoc] [varchar](50) NOT NULL,
	[Calle] [varchar](30) NOT NULL,
	[Nro] [varchar](6) NOT NULL,
	[Piso] [varchar](5) NOT NULL,
	[Depto] [varchar](5) NOT NULL,
	[Sector] [varchar](5) NOT NULL,
	[Torre] [varchar](5) NOT NULL,
	[Manzana] [varchar](5) NOT NULL,
	[Localidad] [varchar](25) NOT NULL,
	[IdProvincia] [varchar](2) NOT NULL,
	[DescrProvincia] [varchar](50) NOT NULL,
	[CodPost] [varchar](8) NOT NULL,
	[NombreContacto] [varchar](25) NOT NULL,
	[EmailContacto] [varchar](60) NOT NULL,
	[TelefonoContacto] [varchar](50) NOT NULL,
	[IdCondIVA] [numeric](2, 0) NOT NULL,
	[DescrCondIVA] [varchar](50) NOT NULL,
	[NroIngBrutos] [varchar](13) NOT NULL,
	[IdCondIngBrutos] [numeric](2, 0) NOT NULL,
	[DescrCondIngBrutos] [varchar](50) NOT NULL,
	[GLN] [numeric](13, 0) NOT NULL,
	[FechaInicioActividades] [datetime] NOT NULL,
	[CodigoInterno] [varchar](20) NOT NULL,
	[EmailAvisoVisualizacion] [varchar](128) NOT NULL,
	[PasswordAvisoVisualizacion] [varchar](50) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[UltActualiz] [timestamp] NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC, 
	[IdTipoDoc] ASC, 
	[NroDoc] ASC, 
	[IdCliente] ASC,  
	[DesambiguacionCuitPais] ASC 
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Articulo](
	[Cuit] [varchar](11) NOT NULL,
	[IdArticulo] [varchar](20) NOT NULL,
	[DescrArticulo] [varchar](100) NOT NULL,
	[GTIN] [varchar](20) NOT NULL,
	[IdUnidad] [varchar](3) NOT NULL,
	[DescrUnidad] [varchar](50) NOT NULL,
	[IndicacionExentoGravado] [varchar](1) NOT NULL,
	[AlicuotaIVA] [numeric](4, 2) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[UltActualiz] [timestamp] NOT NULL,
 CONSTRAINT [PK_Table_Articulo] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC, 
	[IdArticulo] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DestinoComprobante](
	[IdDestinoComprobante] [varchar](15) NOT NULL,
 CONSTRAINT [PK_DestinoComprobante] PRIMARY KEY CLUSTERED 
(
	[IdDestinoComprobante] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Comprobante](
	[Cuit] [varchar](11) NOT NULL,
	[IdTipoComprobante] [numeric](2) NOT NULL, 
	[DescrTipoComprobante] [varchar](30) NOT NULL, 
	[NroPuntoVta] [numeric](4) NOT NULL,
	[NroComprobante] [numeric](8) NOT NULL, 
	[NroLote] [numeric](14) NOT NULL, 
	[IdTipoDoc] [numeric](2, 0) NOT NULL,
	[NroDoc] [numeric](11, 0) NOT NULL,
	[IdCliente] [varchar](50) NOT NULL,
	[DesambiguacionCuitPais] [int] NOT NULL,
	[RazonSocial] [varchar](50) NOT NULL,
	[Detalle] [varchar](256) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[FechaVto] [datetime] NOT NULL,
	[Moneda] [varchar](3) NOT NULL,
	[ImporteMoneda] [numeric](15, 2) NOT NULL,
	[TipoCambio] [numeric](10, 6) NOT NULL,
	[Importe] [numeric](15, 2) NOT NULL,
	[Request] [text] NOT NULL,
	[Response] [text] NOT NULL,
	[IdDestinoComprobante] [varchar](15) NOT NULL,
	[IdWF] [int] NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[UltActualiz] [timestamp] NOT NULL,
 CONSTRAINT [PK_Table_Comprobante] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC, 
	[IdTipoComprobante] ASC, 
	[NroPuntoVta] ASC, 
	[NroComprobante] ASC 
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Entidad](
	[IdEntidad] [varchar](15) NOT NULL,
	[DescrEntidad] [varchar](50) NOT NULL,
	[OrdenReporteActividad] [int] NOT NULL,
 CONSTRAINT [PK_Table_Entidad] PRIMARY KEY CLUSTERED 
(
	[IdEntidad] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO


ALTER TABLE [dbo].[Cuit]  WITH CHECK ADD  CONSTRAINT [FK_Cuit_Medio]
FOREIGN KEY([IdMedio])
REFERENCES [dbo].[Medio] ([IdMedio])
GO
ALTER TABLE [dbo].[Cuit] CHECK CONSTRAINT [FK_Cuit_Medio]
GO

ALTER TABLE [dbo].[UN]  WITH CHECK ADD  CONSTRAINT [FK_UN_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[UN] CHECK CONSTRAINT [FK_UN_Cuit]
GO

ALTER TABLE [dbo].[PuntoVta]  WITH CHECK ADD  CONSTRAINT [FK_PuntoVta_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[PuntoVta] CHECK CONSTRAINT [FK_PuntoVta_Cuit]
GO
ALTER TABLE [dbo].[PuntoVta]  WITH CHECK ADD  CONSTRAINT [FK_PuntoVta_TipoPuntoVta] FOREIGN KEY([IdTipoPuntoVta])
REFERENCES [dbo].[TipoPuntoVta] ([IdTipoPuntoVta])
GO
ALTER TABLE [dbo].[PuntoVta] CHECK CONSTRAINT [FK_PuntoVta_TipoPuntoVta]
GO
ALTER TABLE [dbo].[PuntoVta]  WITH CHECK ADD  CONSTRAINT [FK_PuntoVta_MetodoGeneracionNumeracionLote] FOREIGN KEY([IdMetodoGeneracionNumeracionLote])
REFERENCES [dbo].[MetodoGeneracionNumeracionLote] ([IdMetodoGeneracionNumeracionLote])
GO
ALTER TABLE [dbo].[PuntoVta] CHECK CONSTRAINT [FK_PuntoVta_MetodoGeneracionNumeracionLote]
GO

ALTER TABLE [dbo].[Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_TipoPermiso] FOREIGN KEY([IdTipoPermiso])
REFERENCES [dbo].[TipoPermiso] ([IdTipoPermiso])
GO
ALTER TABLE [dbo].[Permiso] CHECK CONSTRAINT [FK_Permiso_TipoPermiso]
GO

ALTER TABLE [dbo].[LogDetalle]  WITH CHECK ADD  CONSTRAINT [FK_LogDetalle_Log] FOREIGN KEY([IdLog])
REFERENCES [dbo].[Log] ([IdLog])
GO
ALTER TABLE [dbo].[LogDetalle] CHECK CONSTRAINT [FK_LogDetalle_Log]
GO

ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[Cliente] CHECK CONSTRAINT [FK_Cliente_Cuit]
GO

ALTER TABLE [dbo].[Articulo]  WITH CHECK ADD  CONSTRAINT [FK_Articulo_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[Articulo] CHECK CONSTRAINT [FK_Articulo_Cuit]
GO

ALTER TABLE [dbo].[Comprobante]  WITH CHECK ADD  CONSTRAINT [FK_Comprobante_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[Comprobante] CHECK CONSTRAINT [FK_Comprobante_Cuit]
GO
ALTER TABLE [dbo].[Comprobante]  WITH CHECK ADD  CONSTRAINT [FK_Comprobante_DestinoComprobante] FOREIGN KEY([IdDestinoComprobante])
REFERENCES [dbo].[DestinoComprobante] ([IdDestinoComprobante])
GO
ALTER TABLE [dbo].[Comprobante] CHECK CONSTRAINT [FK_Comprobante_DestinoComprobante]
GO

insert Medio values ('Internet', 'Internet')
insert Medio values ('Interfacturas', 'Recomendado por Interfacturas')
insert Medio values ('Conocido', 'Recomendado por un conocido')
insert Medio values ('Mail', 'Mail')
insert Medio values ('Merc.Libre', 'Mercado Libre')
GO

insert TipoPuntoVta values ('BonoFiscal')
insert TipoPuntoVta values ('Exportacion')
insert TipoPuntoVta values ('Comun')
insert TipoPuntoVta values ('RG2904')
GO

insert MetodoGeneracionNumeracionLote values ('Ninguno', 'Ninguno (el n�mero de lote se ingresa manualmente)')
insert MetodoGeneracionNumeracionLote values ('Autonumerador', 'Autonumerador (se calcula a partir del "Ultimo nro. de lote")')
insert MetodoGeneracionNumeracionLote values ('TimeStamp1', 'TimeStamp #1 (A�o-Mes-D�a-Hora-Minutos-Segundos)')
insert MetodoGeneracionNumeracionLote values ('TimeStamp2', 'TimeStamp #2 (d�as transcurridos desde el 01/01/2013-Hora-Minutos-Segundos-Mil�simas de segundos)')
GO

insert TipoPermiso values ('AdminCUIT', 'Administrador del CUIT')
insert TipoPermiso values ('AdminSITE', 'Administrador del site')
insert TipoPermiso values ('AdminUN', 'Administrador de la UN')
insert TipoPermiso values ('UsoCUITxUN', 'Habilitaci�n relaci�n UN-CUIT')
insert TipoPermiso values ('eFact', 'Operador servicio eFact')
insert TipoPermiso values ('eFactArticulos', 'Operador servicio eFactArticulos')
insert TipoPermiso values ('eFactITFonline', 'Operador servicio eFactITFonline')
GO

insert Configuracion values ('', '', 0, '', 'UltimoIdWF', '0')
insert Configuracion values ('', '', 0, '', 'UltimoAccionNro', '0')
insert Configuracion values ('', '', 0, '', 'UltimoMesReporteActividad', '')
insert Configuracion values ('', '', 0, '', 'RegistrarInicioSesion', 'SI')
GO

declare @idWF varchar(256)
declare @accionNro varchar(256)
update Configuracion set @accionNro=Valor=convert(varchar(256), convert(int, Valor)+1) where IdItemConfig='UltimoAccionNro'
update Configuracion set @idWF=Valor=convert(varchar(256), convert(int, Valor)+1) where IdItemConfig='UltimoIdWF'
insert Usuario (IdUsuario, Nombre, Telefono, Email, Password, Pregunta, Respuesta, CantidadEnviosMail, FechaUltimoReenvioMail, EmailSMS, IdWF, Estado) values ('cedeira.migracion', 'Cedeira - Usuario gen�rico para migraci�n', '', 'claudio.cedeira@gmail.com', 'cedeira123', 'Cual es la sigla de mi escuela secundaria', 'encjm', 0, getdate(), 'claudio.cedeira@gmail.com', @IdWF, 'Vigente')
insert Log values (@IdWF, getdate(), 'cedeira.migracion', 'Usuario', 'Alta', 'Vigente', 'Alta x script')
update Configuracion set @idWF=Valor=convert(varchar(256), convert(int, Valor)+1) where IdItemConfig='UltimoIdWF'
insert Permiso values ('cedeira.migracion', '', '', 'AdminSITE', '20621231', '', 'AltaAdminSITEs', @accionNro, @idWF, 'Vigente')
insert Log values (@IdWF, getdate(), 'cedeira.migracion', 'Permiso', 'Alta', 'Vigente', 'Alta x script')
GO

insert DestinoComprobante values ('ITF')
insert DestinoComprobante values ('AFIP')
insert DestinoComprobante values ('Ninguno')
GO

insert Entidad values ('Comprobante', 'Comprobante', 10)
insert Entidad values ('Usuario', 'Usuario', 20)
insert Entidad values ('CUIT', 'CUIT', 30)
insert Entidad values ('UN', 'Unidad de Negocio', 40)
insert Entidad values ('PuntoVta', 'Punto de Venta', 50)
insert Entidad values ('Cliente', 'Cliente', 60)
insert Entidad values ('Articulo', 'Articulo', 70)
insert Entidad values ('Permiso', 'Permiso', 80)
GO

insert Configuracion values ('DEMO', '', 0, '', 'UsuarioDEMO', '')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InicioSesion](
	[Fecha] [datetime] NOT NULL,
	[IdUsuario] [varchar](50) NOT NULL,
	[IP] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Table_InicioSesion] PRIMARY KEY CLUSTERED 
(
	[Fecha] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

