/* OJO QUE SE DESCONCHA LA FK DE LogDetalle a Log
alter table Log add IdLognuevo int identity (1,1) not null
alter table Log drop column IdLog
EXEC sp_rename 'Log.IdLognuevo', 'IdLog', 'COLUMN';
alter table LogDetalle add IdLogDetallenuevo int identity (1,1) not null
alter table LogDetalle drop column IdLogDetalle
*/

EXEC sp_rename 'LogDetalle.IdLogDetallenuevo', 'IdLogDetalle', 'COLUMN';
go
ALTER TABLE [dbo].[Medio] ADD CONSTRAINT [PK_Table_Medio] PRIMARY KEY CLUSTERED 
(
	[IdMedio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cuit] ADD CONSTRAINT [PK_Table_Cuit] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cuit]  WITH CHECK ADD  CONSTRAINT [FK_Cuit_Medio] FOREIGN KEY([IdMedio])
REFERENCES [dbo].[Medio] ([IdMedio])
GO
ALTER TABLE [dbo].[Cuit] CHECK CONSTRAINT [FK_Cuit_Medio]
GO
ALTER TABLE [dbo].[Articulo] ADD CONSTRAINT [PK_Table_Articulo] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[IdArticulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Articulo]  WITH CHECK ADD  CONSTRAINT [FK_Articulo_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[Articulo] CHECK CONSTRAINT [FK_Articulo_Cuit]
GO
ALTER TABLE [dbo].[DestinoComprobante] ADD CONSTRAINT [PK_DestinoComprobante] PRIMARY KEY CLUSTERED 
(
	[IdDestinoComprobante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[NaturalezaComprobante] ADD CONSTRAINT [PK_NaturalezaComprobante] PRIMARY KEY CLUSTERED 
(
	[IdNaturalezaComprobante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comprobante] ADD CONSTRAINT [PK_Table_Comprobante] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[IdTipoComprobante] ASC,
	[NroPuntoVta] ASC,
	[NroComprobante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
ALTER TABLE [dbo].[Comprobante]  WITH CHECK ADD  CONSTRAINT [FK_Comprobante_NaturalezaComprobante] FOREIGN KEY([IdNaturalezaComprobante])
REFERENCES [dbo].[NaturalezaComprobante] ([IdNaturalezaComprobante])
GO
ALTER TABLE [dbo].[Comprobante] CHECK CONSTRAINT [FK_Comprobante_NaturalezaComprobante]
GO
ALTER TABLE [dbo].[Configuracion] ADD CONSTRAINT [PK_Configuracion] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC,
	[Cuit] ASC,
	[IdUN] ASC,
	[IdTipoPermiso] ASC,
	[IdItemConfig] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Persona] ADD CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[IdTipoDoc] ASC,
	[NroDoc] ASC,
	[IdPersona] ASC,
	[DesambiguacionCuitPais] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Cliente_Cuit]
GO
ALTER TABLE [dbo].[DestinatarioFrecuente] ADD CONSTRAINT [PK_DestinatarioFrecuente] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[IdTipoDoc] ASC,
	[NroDoc] ASC,
	[IdPersona] ASC,
	[DesambiguacionCuitPais] ASC,
	[IdDestinatarioFrecuente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DestinatarioFrecuente]  WITH CHECK ADD  CONSTRAINT [FK_DestinatarioFrecuente_Persona] FOREIGN KEY([Cuit], [IdTipoDoc], [NroDoc], [IdPersona], [DesambiguacionCuitPais])
REFERENCES [dbo].[Persona] ([Cuit], [IdTipoDoc], [NroDoc], [IdPersona], [DesambiguacionCuitPais])
GO
ALTER TABLE [dbo].[DestinatarioFrecuente] CHECK CONSTRAINT [FK_DestinatarioFrecuente_Persona]
GO
ALTER TABLE [dbo].[Entidad] ADD CONSTRAINT [PK_Table_Entidad] PRIMARY KEY CLUSTERED 
(
	[IdEntidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ListaPrecio] ADD CONSTRAINT [PK_Table_ListaPrecio] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[IdListaPrecio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ListaPrecio]  WITH CHECK ADD  CONSTRAINT [FK_ListaPrecio_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[ListaPrecio] CHECK CONSTRAINT [FK_ListaPrecio_Cuit]
GO
ALTER TABLE [dbo].[Log] ADD CONSTRAINT [PK_Table_Log] PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LogDetalle] ADD CONSTRAINT [PK_Table_LogDetalle] PRIMARY KEY CLUSTERED 
(
	[IdLogDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LogDetalle]  WITH CHECK ADD  CONSTRAINT [FK_LogDetalle_Log] FOREIGN KEY([IdLog])
REFERENCES [dbo].[Log] ([IdLog])
GO
ALTER TABLE [dbo].[LogDetalle] CHECK CONSTRAINT [FK_LogDetalle_Log]
GO
ALTER TABLE [dbo].[MetodoGeneracionNumeracionLote] ADD CONSTRAINT [PK_MetodoGeneracionNumeracionLote] PRIMARY KEY CLUSTERED 
(
	[IdMetodoGeneracionNumeracionLote] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TipoPermiso] ADD CONSTRAINT [PK_TipoPermiso] PRIMARY KEY CLUSTERED 
(
	[IdTipoPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Permiso] ADD CONSTRAINT [PK_Table_Permiso] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC,
	[Cuit] ASC,
	[IdUN] ASC,
	[IdTipoPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_TipoPermiso] FOREIGN KEY([IdTipoPermiso])
REFERENCES [dbo].[TipoPermiso] ([IdTipoPermiso])
GO
ALTER TABLE [dbo].[Permiso] CHECK CONSTRAINT [FK_Permiso_TipoPermiso]
GO
ALTER TABLE [dbo].[Precio] ADD CONSTRAINT [PK_Table_Precio] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[IdListaPrecio] ASC,
	[IdArticulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Precio]  WITH CHECK ADD  CONSTRAINT [FK_Precio_Articulo] FOREIGN KEY([Cuit], [IdArticulo])
REFERENCES [dbo].[Articulo] ([Cuit], [IdArticulo])
GO
ALTER TABLE [dbo].[Precio] CHECK CONSTRAINT [FK_Precio_Articulo]
GO
ALTER TABLE [dbo].[Precio]  WITH CHECK ADD  CONSTRAINT [FK_Precio_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[Precio] CHECK CONSTRAINT [FK_Precio_Cuit]
GO
ALTER TABLE [dbo].[Precio]  WITH CHECK ADD  CONSTRAINT [FK_Precio_ListaPrecio] FOREIGN KEY([Cuit], [IdListaPrecio])
REFERENCES [dbo].[ListaPrecio] ([Cuit], [IdListaPrecio])
GO
ALTER TABLE [dbo].[Precio] CHECK CONSTRAINT [FK_Precio_ListaPrecio]
GO
ALTER TABLE [dbo].[TipoPuntoVta] ADD CONSTRAINT [PK_TipoPuntoVta] PRIMARY KEY CLUSTERED 
(
	[IdTipoPuntoVta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PuntoVta] ADD CONSTRAINT [PK_Table_PuntoVta] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[NroPuntoVta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PuntoVta]  WITH CHECK ADD  CONSTRAINT [FK_PuntoVta_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[PuntoVta] CHECK CONSTRAINT [FK_PuntoVta_Cuit]
GO
ALTER TABLE [dbo].[PuntoVta]  WITH CHECK ADD  CONSTRAINT [FK_PuntoVta_MetodoGeneracionNumeracionLote] FOREIGN KEY([IdMetodoGeneracionNumeracionLote])
REFERENCES [dbo].[MetodoGeneracionNumeracionLote] ([IdMetodoGeneracionNumeracionLote])
GO
ALTER TABLE [dbo].[PuntoVta] CHECK CONSTRAINT [FK_PuntoVta_MetodoGeneracionNumeracionLote]
GO
ALTER TABLE [dbo].[PuntoVta]  WITH CHECK ADD  CONSTRAINT [FK_PuntoVta_TipoPuntoVta] FOREIGN KEY([IdTipoPuntoVta])
REFERENCES [dbo].[TipoPuntoVta] ([IdTipoPuntoVta])
GO
ALTER TABLE [dbo].[PuntoVta] CHECK CONSTRAINT [FK_PuntoVta_TipoPuntoVta]
GO
ALTER TABLE [dbo].[Ticket] ADD CONSTRAINT [PK_TicketAFIP] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[Service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UN] ADD CONSTRAINT [PK_Table_UN] PRIMARY KEY CLUSTERED 
(
	[Cuit] ASC,
	[IdUN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UN]  WITH CHECK ADD  CONSTRAINT [FK_UN_Cuit] FOREIGN KEY([Cuit])
REFERENCES [dbo].[Cuit] ([Cuit])
GO
ALTER TABLE [dbo].[UN] CHECK CONSTRAINT [FK_UN_Cuit]
GO
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [PK_Table_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO