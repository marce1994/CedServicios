﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using CedServicios.Site.Facturacion.Electronica;

namespace CedServicios.Site
{
    public partial class ComprobanteConsulta : System.Web.UI.Page
    {
        #region Variables
        string gvUniqueID = String.Empty;
        System.Collections.Generic.List<FeaEntidades.InterFacturas.informacion_comprobanteReferencias> referencias;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Funciones.SessionTimeOut(Session))
                {
                    Response.Redirect("~/SessionTimeout.aspx");
                }
                else
                {
                    Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];

                    //VENDEDOR
                    Condicion_IVA_VendedorDropDownList.DataValueField = "Codigo";
                    Condicion_IVA_VendedorDropDownList.DataTextField = "Descr";
                    Condicion_IVA_VendedorDropDownList.DataSource = FeaEntidades.CondicionesIVA.CondicionIVA.Lista();
                    Condicion_IVA_VendedorDropDownList.DataBind();
                    Condicion_Ingresos_Brutos_VendedorDropDownList.DataValueField = "Codigo";
                    Condicion_Ingresos_Brutos_VendedorDropDownList.DataTextField = "Descr";
                    Condicion_Ingresos_Brutos_VendedorDropDownList.DataSource = FeaEntidades.CondicionesIB.CondicionIB.Lista();
                    Condicion_Ingresos_Brutos_VendedorDropDownList.DataBind();
                    Provincia_VendedorDropDownList.DataValueField = "Codigo";
                    Provincia_VendedorDropDownList.DataTextField = "Descr";
                    Provincia_VendedorDropDownList.DataSource = FeaEntidades.CodigosProvincia.CodigoProvincia.Lista();
                    Provincia_VendedorDropDownList.DataBind();

                    //COMPRADOR
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.Lista();
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataBind();
                    Condicion_IVA_CompradorDropDownList.DataValueField = "Codigo";
                    Condicion_IVA_CompradorDropDownList.DataTextField = "Descr";
                    Condicion_IVA_CompradorDropDownList.DataSource = FeaEntidades.CondicionesIVA.CondicionIVA.Lista();
                    Condicion_IVA_CompradorDropDownList.DataBind();
                    Provincia_CompradorDropDownList.DataValueField = "Codigo";
                    Provincia_CompradorDropDownList.DataTextField = "Descr";
                    Provincia_CompradorDropDownList.DataSource = FeaEntidades.CodigosProvincia.CodigoProvincia.Lista();
                    Provincia_CompradorDropDownList.DataBind();

                    //COMPROBANTE
                    Tipo_De_ComprobanteDropDownList.DataValueField = "Codigo";
                    Tipo_De_ComprobanteDropDownList.DataTextField = "Descr";
                    Tipo_De_ComprobanteDropDownList.DataSource = FeaEntidades.TiposDeComprobantes.TipoComprobante.ListaCompletaAFIPCompras();
                    Tipo_De_ComprobanteDropDownList.DataBind();

                    CodigoOperacionDropDownList.DataValueField = "Codigo";
                    CodigoOperacionDropDownList.DataTextField = "Descr";
                    CodigoOperacionDropDownList.DataSource = FeaEntidades.CodigosOperacion.CodigoOperacion.Lista();
                    CodigoOperacionDropDownList.DataBind();
                    IVAcomputableDropDownList.DataValueField = "Codigo";
                    IVAcomputableDropDownList.DataTextField = "Descr";
                    IVAcomputableDropDownList.DataSource = FeaEntidades.Dicotomicos.Dicotomico.Lista();
                    IVAcomputableDropDownList.DataBind();
                    MonedaComprobanteDropDownList.DataValueField = "Codigo";
                    MonedaComprobanteDropDownList.DataTextField = "Descr";
                    MonedaComprobanteDropDownList.DataSource = FeaEntidades.CodigosMoneda.CodigoMoneda.ListaNoExportacion();
                    MonedaComprobanteDropDownList.DataBind();


                    TipoExpDropDownList.DataValueField = "Codigo";
                    TipoExpDropDownList.DataTextField = "Descr";
                    TipoExpDropDownList.DataSource = FeaEntidades.TiposExportacion.TipoExportacion.ListaSinInformar();
                    TipoExpDropDownList.DataBind();

                    IdiomaDropDownList.DataValueField = "Codigo";
                    IdiomaDropDownList.DataTextField = "Descr";
                    IdiomaDropDownList.DataSource = FeaEntidades.Idiomas.Idioma.ListaSinInformar();
                    IdiomaDropDownList.DataBind();

                    PaisDestinoExpDropDownList.DataValueField = "Codigo";
                    PaisDestinoExpDropDownList.DataTextField = "Descr";
                    PaisDestinoExpDropDownList.DataSource = FeaEntidades.DestinosPais.DestinoPais.ListaSinInformar();
                    PaisDestinoExpDropDownList.DataBind();

                    IncotermsDropDownList.DataValueField = "Codigo";
                    IncotermsDropDownList.DataTextField = "Descr";
                    IncotermsDropDownList.DataSource = FeaEntidades.Incoterms.Incoterm.ListaSinInformar();
                    IncotermsDropDownList.DataBind();

                    CodigoConceptoDropDownList.DataValueField = "Codigo";
                    CodigoConceptoDropDownList.DataTextField = "Descr";
                    CodigoConceptoDropDownList.DataSource = FeaEntidades.CodigosConcepto.CodigosConcepto.Lista();
                    CodigoConceptoDropDownList.DataBind();

                    System.Collections.Generic.List<Entidades.PuntoVta> listaPuntoVta = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta;
                    PuntoVtaDropDownList.DataValueField = "Nro";
                    PuntoVtaDropDownList.DataTextField = "Descr";
                    System.Collections.Generic.List<Entidades.PuntoVta> puntoVtalist = new System.Collections.Generic.List<Entidades.PuntoVta>();
                    Entidades.PuntoVta puntoVta = new Entidades.PuntoVta();
                    puntoVta.Nro = 0;
                    puntoVtalist.Add(puntoVta);
                    if (listaPuntoVta != null)
                    {
                        puntoVtalist.AddRange(listaPuntoVta);
                    }
                    PuntoVtaDropDownList.DataSource = puntoVtalist;
                    PuntoVtaDropDownList.DataBind();

                    PeriodicidadEmisionDropDownList.DataValueField = "Id";
                    PeriodicidadEmisionDropDownList.DataTextField = "Descr";
                    PeriodicidadEmisionDropDownList.DataSource = RN.PeriodicidadEmision.Lista(true);
                    PeriodicidadEmisionDropDownList.DataBind();

                    IdDestinoComprobanteDropDownList.DataValueField = "Id";
                    IdDestinoComprobanteDropDownList.DataTextField = "Descr";
                    IdDestinoComprobanteDropDownList.DataSource = sesion.Cuit.DestinosComprobante();
                    IdDestinoComprobanteDropDownList.DataBind();

                    //DataBind();
                    BindearDropDownLists();

                    DeshabilitarControles();

                    Entidades.ComprobanteATratar comprobanteATratar = (Entidades.ComprobanteATratar)Session["ComprobanteATratar"];
                    TratamientoTextBox.Text = comprobanteATratar.Tratamiento.ToString();
                    if (comprobanteATratar.Tratamiento == Entidades.Enum.TratamientoComprobante.ConsultaITF)
                    {
                        IdNaturalezaComprobanteTextBox.Text = "Venta";
                        org.dyndns.cedweb.consulta.ConsultarResult clcrdyndns = (org.dyndns.cedweb.consulta.ConsultarResult)comprobanteATratar.ConsultaITF;
                        CompletarUI(clcrdyndns, new EventArgs());
                    }
                    else
                    {
                        IdNaturalezaComprobanteTextBox.Text = comprobanteATratar.Comprobante.NaturalezaComprobante.Id;
                        CompletarUI(comprobanteATratar.Comprobante);
                        ViewState["ComprobanteATratar"] = comprobanteATratar;
                    }
                    string descrTratamiento = String.Empty;
                    switch (TratamientoTextBox.Text)
                    {
                        case "Consulta":
                            descrTratamiento = "Consulta";
                            break;
                        case "ConsultaITF":
                            descrTratamiento = "Consulta (ITF)";
                            break;
                        case "Baja_AnulBaja":
                            Baja_AnulBaPanel.Visible = true;
                            Baja_AnulBajaButton.Visible = true;
                            Baja_FisicaButton.Visible = false;
                            RN.Comprobante.Leer(comprobanteATratar.Comprobante, sesion);
                            if (comprobanteATratar.Comprobante.Estado == "DeBaja")
                            {
                                descrTratamiento = "Anulación de baja";
                                Baja_AnulBajaButton.Text = "Anular la baja";
                            }
                            else
                            {
                                descrTratamiento = "Baja";
                                Baja_AnulBajaButton.Text = "Registrar la baja";
                            }
                            break;
                        case "Baja_Fisica":
                            Baja_AnulBaPanel.Visible = true;
                            Baja_AnulBajaButton.Visible = false;
                            Baja_FisicaButton.Visible = true;
                            descrTratamiento = "Baja Física";
                            break;
                        case "Envio":
                            AFIPpanel.Visible = true;
                            InterfacturasOnLinePanel.Visible = true;
                            descrTratamiento = "Envio (AFIP/ITF)";
                            break;
                    }
                    DatosEmailAvisoComprobanteContratoConsultaPanel.Visible = false;
                    if (IdNaturalezaComprobanteTextBox.Text.IndexOf("Venta") != -1)
                    {
                        #region Personalización campos vendedor y comprador para VENTAS
                        //VendedorUpdatePanel.Visible = false;
                        pBody.Enabled = false;
                        switch (IdNaturalezaComprobanteTextBox.Text)
                        {
                            case "Venta":
                                TituloPaginaLabel.Text = descrTratamiento + " de Comprobante";
                                DatosComprobanteLabel.Text = "COMPROBANTE DE VENTA (electrónica)";
                                DatosEmisionPanel.Visible = false;
                                break;
                            case "VentaTradic":
                                TituloPaginaLabel.Text = descrTratamiento + " de Comprobante";
                                DatosComprobanteLabel.Text = "COMPROBANTE DE VENTA (tradicional)";
                                LoteUpdatePanel.Visible = false;
                                Id_LoteTextbox.Text = "1";
                                if (TratamientoTextBox.Text.IndexOf("Consulta") != -1) AccionesPanel.Visible = false;
                                DatosEmisionPanel.Visible = false;
                                break;
                            case "VentaContrato":
                                TituloPaginaLabel.Text = descrTratamiento + " de Contrato";
                                DatosComprobanteLabel.Text = "COMPROBANTE DE VENTA (electrónica)";
                                LoteUpdatePanel.Visible = false;
                                Id_LoteTextbox.Text = "1";
                                if (TratamientoTextBox.Text.IndexOf("Consulta") != -1) AccionesPanel.Visible = false;
                                CAEPanel.Visible = false;
                                FechaEmisionLabel.Text = "Fecha de alta:";
                                DatosEmailAvisoComprobanteContratoConsultaPanel.Visible = true;
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Personalización campos vendedor y comprador para COMPRAS
                        CollapsiblePanelExtenderVendedor.Collapsed = false;
                        TituloPaginaLabel.Text = descrTratamiento + " de Comprobante";
                        DatosComprobanteLabel.Text = "COMPROBANTE DE COMPRA";
                        PuntoVtaDropDownList.Visible = false;
                        PuntoVtaTextBox.Visible = true;
                        LoteUpdatePanel.Visible = false;
                        Id_LoteTextbox.Text = "1";
                        compradorUpdatePanel.Visible = false;
                        ReferenciasPanel.Visible = false;
                        ExportacionPanel.Visible = false;
                        DatosComerciales.Visible = false;
                        if (TratamientoTextBox.Text.IndexOf("Consulta") != -1) AccionesPanel.Visible = false;
                        DatosEmisionPanel.Visible = false;
                        FechaProximaEmisionDatePickerWebUserControl.Text = new DateTime(9999, 12, 31).ToString("yyyyMMdd");
                        #endregion
                    }
                    LoteUpdatePanel.Visible = IdDestinoComprobanteDropDownList.SelectedValue == "ITF" || comprobanteATratar.Comprobante.Estado == "PteEnvio";
                }
            }
        }
        private void DeshabilitarControles()
        {
            Tipo_De_ComprobanteDropDownList.Enabled = false;
            IVAcomputableDropDownList.Enabled = false;
            CodigoOperacionDropDownList.Enabled = false;
            CodigoConceptoDropDownList.Enabled = false;
            Condicion_De_PagoTextBox.ReadOnly = true;
            Numero_ComprobanteTextBox.ReadOnly = true;
            FechaEmisionDatePickerWebUserControl.ReadOnly = true; ImageCalendarFechaEmision.Enabled = false;
            FechaVencimientoDatePickerWebUserControl.ReadOnly = true; ImageCalendarFechaVencimiento.Enabled = false;
            FechaServDesdeDatePickerWebUserControl.ReadOnly = true; ImageCalendarFechaServDesde.Enabled = false;
            FechaServHastaDatePickerWebUserControl.ReadOnly = true; ImageCalendarFechaServHasta.Enabled = false;

            Id_LoteTextbox.ReadOnly = true;
            LabelTipoNumeracionLote.Visible = false;
            TipoNumeracionLote.Visible = false;

            Razon_Social_VendedorTextBox.ReadOnly = true;
            Domicilio_Calle_VendedorTextBox.ReadOnly = true;
            Domicilio_Numero_VendedorTextBox.ReadOnly = true;
            Domicilio_Piso_VendedorTextBox.ReadOnly = true;
            Domicilio_Depto_VendedorTextBox.ReadOnly = true;
            Domicilio_Sector_VendedorTextBox.ReadOnly = true;
            Domicilio_Torre_VendedorTextBox.ReadOnly = true;
            Domicilio_Manzana_VendedorTextBox.ReadOnly = true;
            Localidad_VendedorTextBox.ReadOnly = true;
            Provincia_VendedorDropDownList.Enabled = false;
            Cp_VendedorTextBox.ReadOnly = true;
            Contacto_VendedorTextBox.ReadOnly = true;
            Telefono_VendedorTextBox.ReadOnly = true;
            Cuit_VendedorTextBox.ReadOnly = true;
            InicioDeActividadesVendedorDatePickerWebUserControl.ReadOnly = true; ImageCalendarInicioDeActividadesVendedor.Enabled = false;
            Condicion_Ingresos_Brutos_VendedorDropDownList.Enabled = false;
            NroIBVendedorTextBox.ReadOnly = true;
            Condicion_IVA_VendedorDropDownList.Enabled = false;
            GLN_VendedorTextBox.ReadOnly = true;
            Codigo_Interno_VendedorTextBox.ReadOnly = true;
            Email_VendedorTextBox.ReadOnly = true;

            Denominacion_CompradorTextBox.ReadOnly = true;
            Domicilio_Calle_CompradorTextBox.ReadOnly = true;
            Domicilio_Numero_CompradorTextBox.ReadOnly = true;
            Domicilio_Piso_CompradorTextBox.ReadOnly = true;
            Domicilio_Depto_CompradorTextBox.ReadOnly = true;
            Domicilio_Sector_CompradorTextBox.ReadOnly = true;
            Domicilio_Torre_CompradorTextBox.ReadOnly = true;
            Domicilio_Manzana_CompradorTextBox.ReadOnly = true;
            Localidad_CompradorTextBox.ReadOnly = true;
            Provincia_CompradorDropDownList.Enabled = false;
            Cp_CompradorTextBox.ReadOnly = true;
            EmailAvisoVisualizacionTextBox.ReadOnly = true;
            PasswordAvisoVisualizacionTextBox.ReadOnly = true;
            Codigo_Doc_Identificatorio_CompradorDropDownList.Enabled = false;
            Nro_Doc_Identificatorio_CompradorTextBox.ReadOnly = true;
            InicioDeActividadesCompradorDatePickerWebUserControl.ReadOnly = true; ImageCalendarInicioDeActividadesComprador.Enabled = false;
            Condicion_IVA_CompradorDropDownList.Enabled = false;
            GLN_CompradorTextBox.ReadOnly = true;
            Codigo_Interno_CompradorTextBox.ReadOnly = true;
            Contacto_CompradorTextBox.ReadOnly = true;
            Email_CompradorTextBox.ReadOnly = true;
            Telefono_CompradorTextBox.ReadOnly = true;

            //referenciasGridView.Enabled = false;

            TipoExpDropDownList.Enabled = false;
            PaisDestinoExpDropDownList.Enabled = false;
            IdiomaDropDownList.Enabled = false;
            IncotermsDropDownList.Enabled = false;

            DatosComerciales.ReadOnly = true;

            ComentariosTextBox.ReadOnly = true;

            CAETextBox.ReadOnly = true;
            FechaCAEVencimientoDatePickerWebUserControl.ReadOnly = true; ImageCalendarFechaCAEVencimiento.Enabled = false;
            FechaCAEObtencionDatePickerWebUserControl.ReadOnly = true; ImageCalendarFechaCAEObtencion.Enabled = false;
            ResultadoTextBox.ReadOnly = true;
            MotivoTextBox.ReadOnly = true;

            PeriodicidadEmisionDropDownList.Enabled = false;
            IdDestinoComprobanteDropDownList.Enabled = false;
            FechaProximaEmisionDatePickerWebUserControl.ReadOnly = true; ImageCalendarFechaProximaEmision.Enabled = false;
            CantidadComprobantesAEmitirTextBox.ReadOnly = true;
            CantidadComprobantesEmitidosTextBox.ReadOnly = true;
            CantidadDiasFechaVtoTextBox.ReadOnly = true;

            Importe_Total_Neto_Gravado_ResumenTextBox.ReadOnly = true;
            Importe_Total_Concepto_No_Gravado_ResumenTextBox.ReadOnly = true;
            Importe_Operaciones_Exentas_ResumenTextBox.ReadOnly = true;
            Impuesto_Liq_ResumenTextBox.ReadOnly = true;
            Impuesto_Liq_Rni_ResumenTextBox.ReadOnly = true;
            Importe_Total_Impuestos_Municipales_ResumenTextBox.ReadOnly = true;
            Importe_Total_Impuestos_Nacionales_ResumenTextBox.ReadOnly = true;
            Importe_Total_Ingresos_Brutos_ResumenTextBox.ReadOnly = true;
            Importe_Total_Impuestos_Internos_ResumenTextBox.ReadOnly = true;
            Importe_Total_Factura_ResumenTextBox.ReadOnly = true;
            Tipo_de_cambioTextBox.ReadOnly = true;

            Observaciones_ResumenTextBox.ReadOnly = true;
        }
        private void BindearDropDownLists()
        {
            InfoReferencias.BindearDropDownLists();
            PermisosExpo.BindearDropDownLists();
            DetalleLinea.BindearDropDownLists();
        }
        private void CompletarUI(Entidades.Comprobante Comprobante)
        {
            FeaEntidades.InterFacturas.lote_comprobantes lc = new FeaEntidades.InterFacturas.lote_comprobantes();
            #region Obtención del lote desde el comprobante
            System.Xml.Serialization.XmlSerializer x;
            byte[] bytes;
            System.IO.MemoryStream ms;
            x = new System.Xml.Serialization.XmlSerializer(lc.GetType());
            try
            {
                Comprobante.Response = Comprobante.Response.Replace("iso-8859-1", "utf-16");
                bytes = new byte[Comprobante.Response.Length * sizeof(char)];
                System.Buffer.BlockCopy(Comprobante.Response.ToCharArray(), 0, bytes, 0, bytes.Length);
                ms = new System.IO.MemoryStream(bytes);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                lc = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);
            }
            catch
            {
                bytes = new byte[Comprobante.Request.Length * sizeof(char)];
                System.Buffer.BlockCopy(Comprobante.Request.ToCharArray(), 0, bytes, 0, bytes.Length);
                ms = new System.IO.MemoryStream(bytes);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                lc = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);
            }
            #endregion
            //Cabecera
            CompletarCabecera(lc);
            //Comprobante
            CompletarComprobante(lc);
            //Exportacion
            CompletarExportacion(lc);
            //Referencias
            //CompletarReferencias(lc);
            InfoReferencias.PuntoDeVenta = lc.comprobante[0].cabecera.informacion_comprobante.punto_de_venta.ToString();
            InfoReferencias.CompletarReferencias(lc);
            PermisosExpo.CompletarPermisos(lc);
            //Comprador
            CompletarComprador(lc);
            //Vendedor
            CompletarVendedor(lc);
            //Detalle
            DetalleLinea.CompletarDetalles(lc);
            //Descuentos globales
            DescuentosGlobales.Completar(lc);
            //impuestos globales
            ImpuestosGlobales.Completar(lc);
            ComentariosTextBox.Text = lc.comprobante[0].detalle.comentarios;
            //Resumen
            CompletarResumen(lc);
            Observaciones_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.observaciones);
            if (!lc.comprobante[0].resumen.codigo_moneda.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
            {
                Tipo_de_cambioLabel.Visible = true;
                Tipo_de_cambioTextBox.Visible = true;
            }
            else
            {
                Tipo_de_cambioLabel.Visible = false;
                Tipo_de_cambioTextBox.Visible = false;
                Tipo_de_cambioTextBox.Text = null;
            }
            //CAE
            CompletarCAE(lc);
            //Datos de emisión
            PeriodicidadEmisionDropDownList.SelectedValue = Comprobante.PeriodicidadEmision;
            IdDestinoComprobanteDropDownList.SelectedValue = Comprobante.IdDestinoComprobante;
            FechaProximaEmisionDatePickerWebUserControl.Text = Comprobante.FechaProximaEmision.ToString("yyyyMMdd");
            CantidadComprobantesAEmitirTextBox.Text = Comprobante.CantidadComprobantesAEmitir.ToString();
            CantidadComprobantesEmitidosTextBox.Text = Comprobante.CantidadComprobantesEmitidos.ToString();
            CantidadDiasFechaVtoTextBox.Text = Comprobante.CantidadDiasFechaVto.ToString();
            DatosEmailAvisoComprobanteContratoConsulta1.Datos = Comprobante.DatosEmailAvisoComprobanteContrato;
            //Esquema contable
            EsquemaContable.Completar(Comprobante);

            BindearDropDownLists();
        }
        private void CompletarCAE(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            CAETextBox.Text = lc.comprobante[0].cabecera.informacion_comprobante.cae;
            FechaCAEObtencionDatePickerWebUserControl.Text = lc.comprobante[0].cabecera.informacion_comprobante.fecha_obtencion_cae;
            FechaCAEVencimientoDatePickerWebUserControl.Text = lc.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento_cae;
            ResultadoTextBox.Text = lc.comprobante[0].cabecera.informacion_comprobante.resultado;
            MotivoTextBox.Text = lc.comprobante[0].cabecera.informacion_comprobante.motivo;
        }
        private void CompletarComprobante(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            Numero_ComprobanteTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.numero_comprobante);
            FechaEmisionDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_emision);
            FechaVencimientoDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento);
            FechaServDesdeDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_serv_desde);
            FechaServHastaDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_serv_hasta);
            Condicion_De_PagoTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.condicion_de_pago);
            IVAcomputableDropDownList.SelectedIndex = IVAcomputableDropDownList.Items.IndexOf(IVAcomputableDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.iva_computable)));
            CodigoOperacionDropDownList.SelectedIndex = CodigoOperacionDropDownList.Items.IndexOf(CodigoOperacionDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.codigo_operacion)));
            CodigoConceptoDropDownList.SelectedIndex = CodigoConceptoDropDownList.Items.IndexOf(CodigoConceptoDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.codigo_concepto)));
        }
        private void CompletarCabecera(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            try
            {
                Id_LoteTextbox.Text = Convert.ToString(lc.cabecera_lote.id_lote);
                Presta_ServCheckBox.Checked = Convert.ToBoolean(lc.cabecera_lote.presta_serv);
                if (IdNaturalezaComprobanteTextBox.Text != "Compra")
                {
                    PuntoVtaDropDownList.SelectedValue = Convert.ToString(lc.cabecera_lote.punto_de_venta);
                    AjustarCamposXPtaVentaChanged(PuntoVtaDropDownList.SelectedValue);
                }
                else
                {
                    PuntoVtaDropDownList.Visible = false;
                    PuntoVtaTextBox.Visible = true;
                    PuntoVtaTextBox.Text = Convert.ToString(lc.cabecera_lote.punto_de_venta);
                }
                int auxPV = Convert.ToInt32(lc.cabecera_lote.punto_de_venta);
                ViewState["PuntoVenta"] = auxPV;
                DetalleLinea.PuntoDeVenta = Convert.ToString(auxPV);
                //InfoReferencias.PuntoDeVenta = Convert.ToString(auxPV);
                Tipo_De_ComprobanteDropDownList.SelectedIndex = Tipo_De_ComprobanteDropDownList.Items.IndexOf(Tipo_De_ComprobanteDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante)));
                AjustarCamposXVersion(lc);
                Tipo_De_ComprobanteDropDownList.SelectedIndex = Tipo_De_ComprobanteDropDownList.Items.IndexOf(Tipo_De_ComprobanteDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante)));
            }
            catch (NullReferenceException)//detalle_factura.xml
            {
                PuntoVtaDropDownList.SelectedValue = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.punto_de_venta);
                int auxPV = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
                ViewState["PuntoVenta"] = auxPV;
                DetalleLinea.PuntoDeVenta = Convert.ToString(auxPV);
                //InfoReferencias.PuntoDeVenta = Convert.ToString(auxPV);
                AjustarCamposXPtaVentaChanged(PuntoVtaDropDownList.SelectedValue);
                AjustarCamposXVersion(lc);
                Tipo_De_ComprobanteDropDownList.SelectedIndex = Tipo_De_ComprobanteDropDownList.Items.IndexOf(Tipo_De_ComprobanteDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante)));
            }
        }
        private void CompletarExportacion(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            if (lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion != null)
            {
                PaisDestinoExpDropDownList.SelectedIndex = PaisDestinoExpDropDownList.Items.IndexOf(PaisDestinoExpDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion.destino_comprobante)));
                IncotermsDropDownList.SelectedIndex = IncotermsDropDownList.Items.IndexOf(IncotermsDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion.incoterms)));
                TipoExpDropDownList.SelectedIndex = TipoExpDropDownList.Items.IndexOf(TipoExpDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion.tipo_exportacion)));
            }
            else
            {
                PaisDestinoExpDropDownList.SelectedIndex = -1;
                IncotermsDropDownList.SelectedIndex = -1;
                TipoExpDropDownList.SelectedIndex = -1;
            }
            if (lc.comprobante[0].extensiones != null)
            {
                if (lc.comprobante[0].extensiones.extensiones_camara_facturas != null)
                {
                    IdiomaDropDownList.SelectedIndex = IdiomaDropDownList.Items.IndexOf(IdiomaDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].extensiones.extensiones_camara_facturas.id_idioma)));
                }
                else
                {
                    IdiomaDropDownList.SelectedIndex = -1;
                }
                if (lc.comprobante[0].extensiones.extensiones_datos_comerciales != null && lc.comprobante[0].extensiones.extensiones_datos_comerciales != "")
                {
                    //Compatibilidad con archivos xml viejos. Verificar si la descripcion está en Hexa.
                    if (lc.comprobante[0].extensiones.extensiones_datos_comerciales.Substring(0, 1) == "%")
                    {
                        DatosComerciales.Texto = RN.Funciones.HexToString(lc.comprobante[0].extensiones.extensiones_datos_comerciales).Replace("<br>", System.Environment.NewLine);
                    }
                    else
                    {
                        DatosComerciales.Texto = lc.comprobante[0].extensiones.extensiones_datos_comerciales.Replace("<br>", System.Environment.NewLine);
                    }
                }
                else
                {
                    DatosComerciales.Texto = string.Empty;
                }
            }
            else
            {
                IdiomaDropDownList.SelectedIndex = -1;
                DatosComerciales.Texto = string.Empty;
            }
        }

        private void CompletarVendedor(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            if (lc.comprobante[0].cabecera.informacion_vendedor.razon_social != null)
            {
                Razon_Social_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.razon_social);
            }
            Localidad_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.localidad);
            if (lc.comprobante[0].cabecera.informacion_vendedor.GLN != 0)
            {
                GLN_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.GLN);
            }
            Email_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.email);
            Cuit_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.cuit);
            Provincia_VendedorDropDownList.SelectedIndex = Provincia_VendedorDropDownList.Items.IndexOf(Provincia_VendedorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.provincia)));
            Condicion_IVA_VendedorDropDownList.SelectedIndex = Condicion_IVA_VendedorDropDownList.Items.IndexOf(Condicion_IVA_VendedorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.condicion_IVA)));
            Condicion_Ingresos_Brutos_VendedorDropDownList.SelectedIndex = Condicion_Ingresos_Brutos_VendedorDropDownList.Items.IndexOf(Condicion_Ingresos_Brutos_VendedorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.condicion_ingresos_brutos)));
            Cp_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.cp);
            Contacto_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.contacto);
            Telefono_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.telefono);
            Codigo_Interno_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.codigo_interno);
            NroIBVendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.nro_ingresos_brutos);
            InicioDeActividadesVendedorDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.inicio_de_actividades);
            Domicilio_Calle_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_calle);
            Domicilio_Numero_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_numero);
            Domicilio_Piso_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_piso);
            Domicilio_Depto_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_depto);
            Domicilio_Sector_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_sector);
            Domicilio_Torre_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_torre);
            Domicilio_Manzana_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_manzana);
        }
        private void CompletarComprador(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            if (lc.comprobante[0].cabecera.informacion_comprador.GLN != 0)
            {
                GLN_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.GLN);
            }
            Codigo_Interno_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.codigo_interno);
            if (!lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio.Equals(70))
            {
                Nro_Doc_Identificatorio_CompradorTextBox.Visible = true;
                Nro_Doc_Identificatorio_CompradorDropDownList.Visible = false;
                Nro_Doc_Identificatorio_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio);
            }
            else
            {
                Nro_Doc_Identificatorio_CompradorTextBox.Visible = false;
                Nro_Doc_Identificatorio_CompradorDropDownList.Visible = true;
                Nro_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = Nro_Doc_Identificatorio_CompradorDropDownList.Items.IndexOf(Nro_Doc_Identificatorio_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio)));
            }
            Codigo_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = Codigo_Doc_Identificatorio_CompradorDropDownList.Items.IndexOf(Codigo_Doc_Identificatorio_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio)));
            Denominacion_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.denominacion);
            Domicilio_Calle_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_calle);
            Domicilio_Numero_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_numero);
            Domicilio_Piso_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_piso);
            Domicilio_Depto_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_depto);
            Domicilio_Sector_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_sector);
            Domicilio_Torre_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_torre);
            Domicilio_Manzana_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_manzana);
            Localidad_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.localidad);
            Cp_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.cp);
            Contacto_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.contacto);
            Email_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.email);
            Telefono_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.telefono);
            InicioDeActividadesCompradorDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.inicio_de_actividades);
            Provincia_CompradorDropDownList.SelectedIndex = Provincia_CompradorDropDownList.Items.IndexOf(Provincia_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.provincia)));
            Condicion_IVA_CompradorDropDownList.SelectedIndex = Condicion_IVA_CompradorDropDownList.Items.IndexOf(Condicion_IVA_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.condicion_IVA)));
            if (lc.comprobante[0].extensiones != null)
            {
                if (lc.comprobante[0].extensiones.extensiones_destinatarios != null)
                {
                    EmailAvisoVisualizacionTextBox.Text = lc.comprobante[0].extensiones.extensiones_destinatarios.email;
                }
            }
        }
        private void CompletarResumen(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            MonedaComprobanteDropDownList.SelectedIndex = MonedaComprobanteDropDownList.Items.IndexOf(MonedaComprobanteDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].resumen.codigo_moneda)));
            Tipo_de_cambioTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.tipo_de_cambio);
            if (lc.comprobante[0].resumen.codigo_moneda.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
            {
                Importe_Total_Neto_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_neto_gravado);
                Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_concepto_no_gravado);
                Importe_Operaciones_Exentas_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_operaciones_exentas);
                Impuesto_Liq_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.impuesto_liq);
                Impuesto_Liq_Rni_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.impuesto_liq_rni);
                Importe_Total_Factura_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_factura);
                if (lc.comprobante[0].resumen.importe_total_impuestos_nacionalesSpecified.Equals(true))
                {
                    Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_impuestos_nacionales);
                }
                else
                {
                    Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text = string.Empty;
                }
                if (lc.comprobante[0].resumen.importe_total_impuestos_municipalesSpecified.Equals(true))
                {
                    Importe_Total_Impuestos_Municipales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_impuestos_municipales);
                }
                else
                {
                    Importe_Total_Impuestos_Municipales_ResumenTextBox.Text = string.Empty;
                }
                if (lc.comprobante[0].resumen.importe_total_impuestos_internosSpecified.Equals(true))
                {
                    Importe_Total_Impuestos_Internos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_impuestos_internos);
                }
                else
                {
                    Importe_Total_Impuestos_Internos_ResumenTextBox.Text = string.Empty;
                }
                if (lc.comprobante[0].resumen.importe_total_ingresos_brutosSpecified.Equals(true))
                {
                    Importe_Total_Ingresos_Brutos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_ingresos_brutos);
                }
                else
                {
                    Importe_Total_Ingresos_Brutos_ResumenTextBox.Text = string.Empty;
                }
            }
            else
            {
                Importe_Total_Neto_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_neto_gravado);
                Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_concepto_no_gravado);
                Importe_Operaciones_Exentas_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_operaciones_exentas);
                Impuesto_Liq_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.impuesto_liq);
                Impuesto_Liq_Rni_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.impuesto_liq_rni);
                Importe_Total_Factura_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_factura);
                if (lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_nacionalesSpecified.Equals(true))
                {
                    Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_nacionales);
                }
                else
                {
                    Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text = string.Empty;
                }
                if (lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_municipalesSpecified.Equals(true))
                {
                    Importe_Total_Impuestos_Municipales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_municipales);
                }
                else
                {
                    Importe_Total_Impuestos_Municipales_ResumenTextBox.Text = string.Empty;
                }
                if (lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_internosSpecified.Equals(true))
                {
                    Importe_Total_Impuestos_Internos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_internos);
                }
                else
                {
                    Importe_Total_Impuestos_Internos_ResumenTextBox.Text = string.Empty;
                }
                if (lc.comprobante[0].resumen.importes_moneda_origen.importe_total_ingresos_brutosSpecified.Equals(true))
                {
                    Importe_Total_Ingresos_Brutos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_ingresos_brutos);
                }
                else
                {
                    Importe_Total_Ingresos_Brutos_ResumenTextBox.Text = string.Empty;
                }
            }
        }
        private void CompletarUI(org.dyndns.cedweb.consulta.ConsultarResult lc, EventArgs e)
        {
            //Cabecera
            Tipo_De_ComprobanteDropDownList.SelectedIndex = Tipo_De_ComprobanteDropDownList.Items.IndexOf(Tipo_De_ComprobanteDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante)));
            Id_LoteTextbox.Text = Convert.ToString(lc.cabecera_lote.id_lote);
            Presta_ServCheckBox.Checked = Convert.ToBoolean(lc.cabecera_lote.presta_serv);
            PuntoVtaDropDownList.SelectedValue = Convert.ToString(lc.cabecera_lote.punto_de_venta);
            int auxPV = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
            ViewState["PuntoVenta"] = auxPV;
            DetalleLinea.PuntoDeVenta = Convert.ToString(auxPV);
            AjustarCamposXPtaVentaChanged(PuntoVtaDropDownList.SelectedValue);
            AjustarCamposXVersion(lc);
            //Comprobante
            Numero_ComprobanteTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.numero_comprobante);
            FechaEmisionDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_emision);
            FechaVencimientoDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento);
            FechaServDesdeDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_serv_desde);
            FechaServHastaDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.fecha_serv_hasta);
            Condicion_De_PagoTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.condicion_de_pago);
            IVAcomputableDropDownList.SelectedIndex = IVAcomputableDropDownList.Items.IndexOf(IVAcomputableDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.iva_computable)));
            CodigoOperacionDropDownList.SelectedIndex = CodigoOperacionDropDownList.Items.IndexOf(CodigoOperacionDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.codigo_operacion)));

            //Exportacion
            if (lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion != null)
            {
                PaisDestinoExpDropDownList.SelectedIndex = PaisDestinoExpDropDownList.Items.IndexOf(PaisDestinoExpDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion.destino_comprobante)));
                IncotermsDropDownList.SelectedIndex = IncotermsDropDownList.Items.IndexOf(IncotermsDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion.incoterms)));
                TipoExpDropDownList.SelectedIndex = TipoExpDropDownList.Items.IndexOf(TipoExpDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprobante.informacion_exportacion.tipo_exportacion)));
            }
            else
            {
                PaisDestinoExpDropDownList.SelectedIndex = -1;
                IncotermsDropDownList.SelectedIndex = -1;
                TipoExpDropDownList.SelectedIndex = -1;
            }
            if (lc.comprobante[0].extensiones != null)
            {
                if (lc.comprobante[0].extensiones.extensiones_camara_facturas != null)
                {
                    IdiomaDropDownList.SelectedIndex = IdiomaDropDownList.Items.IndexOf(IdiomaDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].extensiones.extensiones_camara_facturas.id_idioma)));
                }
                else
                {
                    IdiomaDropDownList.SelectedIndex = -1;
                }
                if (lc.comprobante[0].extensiones.extensiones_datos_comerciales != null && lc.comprobante[0].extensiones.extensiones_datos_comerciales != "")
                {
                    //Compatibilidad con archivos xml viejos. Verificar si la descripcion está en Hexa.
                    if (lc.comprobante[0].extensiones.extensiones_datos_comerciales.Substring(0, 1) == "%")
                    {
                        DatosComerciales.Texto = RN.Funciones.HexToString(lc.comprobante[0].extensiones.extensiones_datos_comerciales).Replace("<br>", System.Environment.NewLine);
                    }
                    else
                    {
                        DatosComerciales.Texto = lc.comprobante[0].extensiones.extensiones_datos_comerciales.Replace("<br>", System.Environment.NewLine);
                    }
                }
            }
            else
            {
                IdiomaDropDownList.SelectedIndex = -1;
            }

            //Referencias
            InfoReferencias.CompletarWS(lc);

            //Comprador
            if (lc.comprobante[0].cabecera.informacion_comprador.GLN != 0)
            {
                GLN_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.GLN);
            }
            Codigo_Interno_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.codigo_interno);
            //Codigo_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = Codigo_Doc_Identificatorio_CompradorDropDownList.Items.IndexOf(Codigo_Doc_Identificatorio_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio)));
            //Nro_Doc_Identificatorio_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio);
            Denominacion_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.denominacion);
            Domicilio_Calle_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_calle);
            Domicilio_Numero_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_numero);
            Domicilio_Piso_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_piso);
            Domicilio_Depto_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_depto);
            Domicilio_Sector_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_sector);
            Domicilio_Torre_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_torre);
            Domicilio_Manzana_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.domicilio_manzana);
            Localidad_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.localidad);
            Cp_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.cp);
            Contacto_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.contacto);
            Email_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.email);
            Telefono_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.telefono);
            InicioDeActividadesCompradorDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.inicio_de_actividades);
            Provincia_CompradorDropDownList.SelectedIndex = Provincia_CompradorDropDownList.Items.IndexOf(Provincia_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.provincia)));

            string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
            {
                return pv.Nro == auxPV;
            }).IdTipoPuntoVta;
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
            if (!idtipo.Equals("Exportacion") || (lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio != null && lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio != 70) || PaisDestinoExpDropDownList.SelectedItem.Text.ToUpper().Contains("ARGENTINA"))
            {
                Nro_Doc_Identificatorio_CompradorTextBox.Visible = true;
                Nro_Doc_Identificatorio_CompradorDropDownList.Visible = false;
                Nro_Doc_Identificatorio_CompradorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio);
                Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.ListaNoExportacion();
            }
            else
            {
                Nro_Doc_Identificatorio_CompradorTextBox.Visible = false;
                Nro_Doc_Identificatorio_CompradorDropDownList.Visible = true;
                Nro_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.DestinosCuit.DestinoCuit.ListaSinInformar();
                Nro_Doc_Identificatorio_CompradorDropDownList.DataBind();
                Nro_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = Nro_Doc_Identificatorio_CompradorDropDownList.Items.IndexOf(Nro_Doc_Identificatorio_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio)));
                Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.ListaExportacion();
            }
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataBind();
            Codigo_Doc_Identificatorio_CompradorDropDownList.SelectedValue = Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio);
            
            Condicion_IVA_CompradorDropDownList.SelectedIndex = Condicion_IVA_CompradorDropDownList.Items.IndexOf(Condicion_IVA_CompradorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_comprador.condicion_IVA)));
            //Vendedor
            if (lc.comprobante[0].cabecera.informacion_vendedor.razon_social != null)
            {
                Razon_Social_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.razon_social);
            }
            Localidad_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.localidad);
            if (lc.comprobante[0].cabecera.informacion_vendedor.GLN != 0)
            {
                GLN_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.GLN);
            }
            Email_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.email);
            Cuit_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.cuit);
            Provincia_VendedorDropDownList.SelectedIndex = Provincia_VendedorDropDownList.Items.IndexOf(Provincia_VendedorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.provincia)));
            Condicion_IVA_VendedorDropDownList.SelectedIndex = Condicion_IVA_VendedorDropDownList.Items.IndexOf(Condicion_IVA_VendedorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.condicion_IVA)));
            Condicion_Ingresos_Brutos_VendedorDropDownList.SelectedIndex = Condicion_Ingresos_Brutos_VendedorDropDownList.Items.IndexOf(Condicion_Ingresos_Brutos_VendedorDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.condicion_ingresos_brutos)));
            Cp_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.cp);
            Contacto_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.contacto);
            Telefono_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.telefono);
            Codigo_Interno_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.codigo_interno);
            NroIBVendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.nro_ingresos_brutos);
            InicioDeActividadesVendedorDatePickerWebUserControl.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.inicio_de_actividades);
            Domicilio_Calle_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_calle);
            Domicilio_Numero_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_numero);
            Domicilio_Piso_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_piso);
            Domicilio_Depto_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_depto);
            Domicilio_Sector_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_sector);
            Domicilio_Torre_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_torre);
            Domicilio_Manzana_VendedorTextBox.Text = Convert.ToString(lc.comprobante[0].cabecera.informacion_vendedor.domicilio_manzana);

            //Detalle
            DetalleLinea.CompletarDetallesWS(lc);

            //Descuentos globales
            DescuentosGlobales.CompletarDetallesWS(lc);

            //impuestos globales
            ImpuestosGlobales.CompletarWS(lc);

            ComentariosTextBox.Text = lc.comprobante[0].detalle.comentarios;
            //Resumen
            MonedaComprobanteDropDownList.SelectedIndex = MonedaComprobanteDropDownList.Items.IndexOf(MonedaComprobanteDropDownList.Items.FindByValue(Convert.ToString(lc.comprobante[0].resumen.codigo_moneda)));
            Tipo_de_cambioTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.tipo_de_cambio);
            if (lc.comprobante[0].resumen.codigo_moneda.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
            {
                Importe_Total_Neto_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_neto_gravado);
                Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_concepto_no_gravado);
                Importe_Operaciones_Exentas_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_operaciones_exentas);
                Impuesto_Liq_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.impuesto_liq);
                Impuesto_Liq_Rni_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.impuesto_liq_rni);
                Importe_Total_Factura_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_factura);
                Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_impuestos_nacionales);
                Importe_Total_Impuestos_Municipales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_impuestos_municipales);
                Importe_Total_Impuestos_Internos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_impuestos_internos);
                Importe_Total_Ingresos_Brutos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importe_total_ingresos_brutos);
            }
            else
            {
                Importe_Total_Neto_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_neto_gravado);
                Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_concepto_no_gravado);
                Importe_Operaciones_Exentas_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_operaciones_exentas);
                Impuesto_Liq_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.impuesto_liq);
                Impuesto_Liq_Rni_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.impuesto_liq_rni);
                Importe_Total_Factura_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_factura);
                Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_nacionales);
                Importe_Total_Impuestos_Municipales_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_municipales);
                Importe_Total_Impuestos_Internos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_impuestos_internos);
                Importe_Total_Ingresos_Brutos_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.importes_moneda_origen.importe_total_ingresos_brutos);
            }
            Observaciones_ResumenTextBox.Text = Convert.ToString(lc.comprobante[0].resumen.observaciones);
            if (!lc.comprobante[0].resumen.codigo_moneda.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
            {
                Tipo_de_cambioLabel.Visible = true;
                Tipo_de_cambioTextBox.Visible = true;
            }
            else
            {
                Tipo_de_cambioLabel.Visible = false;
                Tipo_de_cambioTextBox.Visible = false;
                Tipo_de_cambioTextBox.Text = null;
            }
            //CAE
            CAETextBox.Text = lc.comprobante[0].cabecera.informacion_comprobante.cae;
            FechaCAEObtencionDatePickerWebUserControl.Text = lc.comprobante[0].cabecera.informacion_comprobante.fecha_obtencion_cae;
            FechaCAEVencimientoDatePickerWebUserControl.Text = lc.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento_cae;
            ResultadoTextBox.Text = lc.comprobante[0].cabecera.informacion_comprobante.resultado;
            MotivoTextBox.Text = lc.comprobante[0].cabecera.informacion_comprobante.motivo;
            MotivoTextBox.Text += lc.cabecera_lote.motivo;
            BindearDropDownLists();
        }
        protected void MonedaComprobanteDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void ResetearVendedor()
        {
            Razon_Social_VendedorTextBox.Text = string.Empty;
            Domicilio_Calle_VendedorTextBox.Text = string.Empty;
            Domicilio_Numero_VendedorTextBox.Text = string.Empty;
            Domicilio_Piso_VendedorTextBox.Text = string.Empty;
            Domicilio_Depto_VendedorTextBox.Text = string.Empty;
            Domicilio_Sector_VendedorTextBox.Text = string.Empty;
            Domicilio_Torre_VendedorTextBox.Text = string.Empty;
            Domicilio_Manzana_VendedorTextBox.Text = string.Empty;
            Localidad_VendedorTextBox.Text = "";
            FechaEmisionDatePickerWebUserControl.Text = "";
            Numero_ComprobanteTextBox.Text = "";
            Provincia_VendedorDropDownList.SelectedValue = Convert.ToString(0);
            Cp_VendedorTextBox.Text = string.Empty;
            Contacto_VendedorTextBox.Text = string.Empty;
            Email_VendedorTextBox.Text = string.Empty;
            Telefono_VendedorTextBox.Text = string.Empty;
            Cuit_VendedorTextBox.Text = string.Empty;
            Condicion_IVA_VendedorDropDownList.SelectedValue = Convert.ToString(0);
            NroIBVendedorTextBox.Text = string.Empty;
            Condicion_Ingresos_Brutos_VendedorDropDownList.SelectedValue = Convert.ToString(0);
            GLN_VendedorTextBox.Text = string.Empty;
            Codigo_Interno_VendedorTextBox.Text = string.Empty;
            InicioDeActividadesVendedorDatePickerWebUserControl.Text = string.Empty;

            System.Collections.Generic.List<Entidades.PuntoVta> listaPuntoVta = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta;
            PuntoVtaDropDownList.DataValueField = "Nro";
            PuntoVtaDropDownList.DataTextField = "DescrCombo";
            System.Collections.Generic.List<Entidades.PuntoVta> puntoVtalist = new System.Collections.Generic.List<Entidades.PuntoVta>();
            Entidades.PuntoVta puntoVta = new Entidades.PuntoVta();
            puntoVta.Nro = 0;
            puntoVtalist.Add(puntoVta);
            if (listaPuntoVta != null)
            {
                puntoVtalist.AddRange(listaPuntoVta);
            }
            PuntoVtaDropDownList.DataSource = puntoVtalist;
            PuntoVtaDropDownList.DataBind();
            PuntoVtaDropDownList_SelectedIndexChanged(PuntoVtaDropDownList, new EventArgs());
        }
        private void ResetearComprador()
        {
            Denominacion_CompradorTextBox.Text = string.Empty;
            Domicilio_Calle_CompradorTextBox.Text = string.Empty;
            Domicilio_Numero_CompradorTextBox.Text = string.Empty;
            Domicilio_Piso_CompradorTextBox.Text = string.Empty;
            Domicilio_Depto_CompradorTextBox.Text = string.Empty;
            Domicilio_Sector_CompradorTextBox.Text = string.Empty;
            Domicilio_Torre_CompradorTextBox.Text = string.Empty;
            Domicilio_Manzana_CompradorTextBox.Text = string.Empty;
            Localidad_CompradorTextBox.Text = string.Empty;
            Provincia_CompradorDropDownList.SelectedValue = Convert.ToString(0);
            Cp_CompradorTextBox.Text = string.Empty;
            Contacto_CompradorTextBox.Text = string.Empty;
            Email_CompradorTextBox.Text = string.Empty;
            Telefono_CompradorTextBox.Text = string.Empty;
            Condicion_IVA_CompradorDropDownList.SelectedValue = Convert.ToString(0);
            Nro_Doc_Identificatorio_CompradorTextBox.Text = "";
            GLN_CompradorTextBox.Text = string.Empty;
            Codigo_Interno_CompradorTextBox.Text = string.Empty;
            InicioDeActividadesCompradorDatePickerWebUserControl.Text = string.Empty;
            EmailAvisoVisualizacionTextBox.Text = string.Empty;
            PasswordAvisoVisualizacionTextBox.Text = string.Empty;
        }
        private void ResetearGrillas()
        {
            DetalleLinea.ResetearGrillas();

            //referencias = new System.Collections.Generic.List<FeaEntidades.InterFacturas.informacion_comprobanteReferencias>();
            //FeaEntidades.InterFacturas.informacion_comprobanteReferencias referencia = new FeaEntidades.InterFacturas.informacion_comprobanteReferencias();
            //referencias.Add(referencia);
            //referenciasGridView.DataSource = referencias;
            //ViewState["referencias"] = referencias;
            //referenciasGridView.DataBind();

            BindearDropDownLists();
            InfoReferencias.ResetearGrillas();
            PermisosExpo.ResetearGrillas();
            DetalleLinea.ResetearGrillas();
            ImpuestosGlobales.ResetearGrillas();
            DatosComerciales.Texto = "";
            DatosComerciales.ReadOnly = true;
        }
        private void ResetearOtros()
        {
            //Información comprobante
            FechaVencimientoDatePickerWebUserControl.Text = "";
            IVAcomputableDropDownList.SelectedValue = "";
            CodigoOperacionDropDownList.SelectedValue = "";
            CodigoConceptoDropDownList.SelectedValue = "1";
            FechaServDesdeDatePickerWebUserControl.Text = "";
            FechaServHastaDatePickerWebUserControl.Text = "";
            Condicion_De_PagoTextBox.Text = "";

            //Comentarios
            ComentariosTextBox.Text = "";

            //Observaciones
            Observaciones_ResumenTextBox.Text = "";

            //Datos del Lote
            Id_LoteTextbox.Text = "";

            //Información CAE
            CAETextBox.Text = "";
            FechaCAEVencimientoDatePickerWebUserControl.Text = "";
            FechaCAEObtencionDatePickerWebUserControl.Text = "";
            MotivoTextBox.Text = "";
            ResultadoTextBox.Text = "";

            //Resumen
            Importe_Total_Neto_Gravado_ResumenTextBox.Text = "";
            Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text = "";
            Importe_Operaciones_Exentas_ResumenTextBox.Text = "";
            Impuesto_Liq_ResumenTextBox.Text = "";
            Impuesto_Liq_Rni_ResumenTextBox.Text = "";
            Importe_Total_Impuestos_Municipales_ResumenTextBox.Text = "";
            Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text = "";
            Importe_Total_Ingresos_Brutos_ResumenTextBox.Text = "";
            Importe_Total_Impuestos_Internos_ResumenTextBox.Text = "";
            Importe_Total_Factura_ResumenTextBox.Text = "";
            Tipo_de_cambioTextBox.Text = "";
        }
        private void AjustarCamposXPtaVentaChanged(string PuntoDeVenta)
        {
            if (!PuntoDeVenta.Equals(string.Empty))
            {
                int auxPV;
                try
                {
                    auxPV = Convert.ToInt32(PuntoDeVenta);
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    TipoPtoVentaLabel.Text = idtipo;
                    Tipo_De_ComprobanteDropDownList.DataValueField = "Codigo";
                    Tipo_De_ComprobanteDropDownList.DataTextField = "Descr";
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
                    Nro_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
                    Nro_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
                    System.Collections.Generic.List<Entidades.Persona> listacompradores = new System.Collections.Generic.List<Entidades.Persona>();
                    switch (idtipo)
                    {
                        case "Comun":
                            AjustarCamposXPtaVentaComun();
                            ExportacionPanel.Visible = false;
                            break;
                        case "RG2904":
                            AjustarCamposXPtaVentaRG2904(Tipo_De_ComprobanteDropDownList.SelectedValue);
                            ExportacionPanel.Visible = false;
                            break;
                        case "BFiscal":
                            AjustarCamposXPtaVentaBonoFiscal();
                            ExportacionPanel.Visible = false;
                            break;
                        case "Exportacion":
                            AjustarCamposXPtaVentaExport();
                            ExportacionPanel.Visible = true;
                            break;
                        default:
                            throw new Exception("Tipo de punto de venta no contemplado en la lógica de la aplicación (" + idtipo + ")");

                    }
                    Tipo_De_ComprobanteDropDownList.DataBind();
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataBind();
                    TipoPtoVentaLabel.Text = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                }
                catch
                {
                    AjustarCamposXPtaVentaIndefinido();
                }
            }
            else
            {
                AjustarCamposXPtaVentaIndefinido();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Debe definir el punto de venta"), false);
            }
        }
        private void AjustarCamposXPtaVentaExport()
        {
            Presta_ServCheckBox.Checked = false;
            Presta_ServCheckBox.Enabled = false;
            Presta_ServCheckBox.Visible = true;
            Presta_ServLabel.Visible = true;
            Version1RadioButton.Visible = false;
            CodigoConceptoLabel.Visible = false;
            CodigoConceptoDropDownList.Visible = false;
            FechaServDesdeDatePickerWebUserControl.Text = string.Empty;
            FechaServDesdeDatePickerWebUserControl.Visible = false;
            ImageCalendarFechaServDesde.Visible = false;
            FechaInicioServLabel.Visible = false;
            FechaHstServLabel.Visible = false;
            FechaServHastaDatePickerWebUserControl.Text = string.Empty;
            FechaServHastaDatePickerWebUserControl.Visible = false;
            ImageCalendarFechaServHasta.Visible = false;
            Tipo_De_ComprobanteDropDownList.DataSource = FeaEntidades.TiposDeComprobantes.TipoComprobante.ListaParaExportaciones();

            Nro_Doc_Identificatorio_CompradorTextBox.Visible = false;
            Nro_Doc_Identificatorio_CompradorDropDownList.Visible = true;
            Nro_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
            Nro_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
            Nro_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.DestinosCuit.DestinoCuit.ListaSinInformar();
            Nro_Doc_Identificatorio_CompradorDropDownList.DataBind();
            Nro_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = Nro_Doc_Identificatorio_CompradorDropDownList.Items.IndexOf(Nro_Doc_Identificatorio_CompradorDropDownList.Items.FindByValue(Nro_Doc_Identificatorio_CompradorTextBox.Text));
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.ListaExportacion();

            CodigoOperacionDropDownList.Visible = true;
            CodigoOperacionLabel.Visible = true;
        }

        private void AjustarCamposXPtaVentaBonoFiscal()
        {
            Presta_ServCheckBox.Checked = false;
            Presta_ServCheckBox.Enabled = false;
            Presta_ServCheckBox.Visible = true;
            Presta_ServLabel.Visible = true;
            Version1RadioButton.Visible = false;
            CodigoConceptoLabel.Visible = false;
            CodigoConceptoDropDownList.Visible = false;
            FechaServDesdeDatePickerWebUserControl.Text = string.Empty;
            FechaServDesdeDatePickerWebUserControl.Visible = false;
            ImageCalendarFechaServDesde.Visible = false;
            FechaInicioServLabel.Visible = false;
            FechaHstServLabel.Visible = false;
            FechaServHastaDatePickerWebUserControl.Text = string.Empty;
            FechaServHastaDatePickerWebUserControl.Visible = false;
            ImageCalendarFechaServHasta.Visible = false;
            Tipo_De_ComprobanteDropDownList.DataSource = FeaEntidades.TiposDeComprobantes.TipoComprobante.ListaParaBienesDeCapital();
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.Lista();
            Nro_Doc_Identificatorio_CompradorDropDownList.Visible = false;
            Nro_Doc_Identificatorio_CompradorTextBox.Visible = true;

            TipoExpDropDownList.SelectedIndex = -1;
            PaisDestinoExpDropDownList.SelectedIndex = -1;
            IdiomaDropDownList.SelectedIndex = -1;
            IncotermsDropDownList.SelectedIndex = -1;
            CodigoOperacionDropDownList.Visible = true;
            CodigoOperacionLabel.Visible = true;
        }
        private void AjustarCamposXPtaVentaComun()
        {
            //Presta_ServCheckBox.Enabled = true;
            HacerVisiblesV0V1();
            AjustarCamposXPtaVtaComunYRG2904();
        }
        private void AjustarCamposXPtaVentaRG2904(string tipoComprobante)
        {
            Presta_ServCheckBox.Enabled = true;
            AjustarCamposXPtaVtaComunYRG2904();
            AjustarCodigoOperacionEn2904(tipoComprobante);
        }
        private void AjustarCodigoOperacionEn2904(string valor)
        {
            if (valor.Equals("2") || valor.Equals("3"))
            {
                CodigoOperacionDropDownList.Visible = false;
                CodigoOperacionLabel.Visible = false;
            }
            else
            {
                CodigoOperacionDropDownList.Visible = true;
                CodigoOperacionLabel.Visible = true;
            }
        }
        private void AjustarCamposXPtaVtaComunYRG2904()
        {
            AjustarPrestaServxVersiones();

            FechaInicioServLabel.Visible = true;
            FechaHstServLabel.Visible = true;
            Tipo_De_ComprobanteDropDownList.DataSource = FeaEntidades.TiposDeComprobantes.TipoComprobante.ListaCompletaAFIP();
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.Lista();
            Nro_Doc_Identificatorio_CompradorDropDownList.Visible = false;
            Nro_Doc_Identificatorio_CompradorTextBox.Visible = true;
            TipoExpDropDownList.SelectedIndex = -1;
            PaisDestinoExpDropDownList.SelectedIndex = -1;
            IdiomaDropDownList.SelectedIndex = -1;
            IncotermsDropDownList.SelectedIndex = -1;
            CodigoOperacionDropDownList.Visible = true;
            CodigoOperacionLabel.Visible = true;
        }
        private void HacerVisiblesV0V1()
        {
            Version1RadioButton.Checked = true;
        }
        private void AjustarCamposXPtaVentaIndefinido()
        {
            TipoPtoVentaLabel.Text = "No definido";
            Presta_ServCheckBox.Visible = true;
            Presta_ServLabel.Visible = true;
            Version1RadioButton.Visible = false;
            CodigoConceptoLabel.Visible = false;
            CodigoConceptoDropDownList.Visible = false;
            FechaInicioServLabel.Visible = true;
            FechaHstServLabel.Visible = true;
            Tipo_De_ComprobanteDropDownList.DataSource = FeaEntidades.TiposDeComprobantes.TipoComprobante.ListaCompleta();
            Tipo_De_ComprobanteDropDownList.DataBind();
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.Lista();
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataBind();
            Nro_Doc_Identificatorio_CompradorDropDownList.Visible = false;
            Nro_Doc_Identificatorio_CompradorTextBox.Visible = true;
            CodigoOperacionDropDownList.Visible = true;
            CodigoOperacionLabel.Visible = true;
        }
        private FeaEntidades.InterFacturas.lote_comprobantes GenerarLote()
        {
            FeaEntidades.InterFacturas.lote_comprobantes lote = new FeaEntidades.InterFacturas.lote_comprobantes();
            FeaEntidades.InterFacturas.comprobante comp = new FeaEntidades.InterFacturas.comprobante();
            FeaEntidades.InterFacturas.cabecera_lote cab = new FeaEntidades.InterFacturas.cabecera_lote();
            cab.cantidad_reg = 1;
            cab.cuit_canal = Convert.ToInt64(Entidades.Const.CuitInterfacturas);
            cab.cuit_vendedor = Convert.ToInt64(Cuit_VendedorTextBox.Text);
            cab.id_lote = Convert.ToInt64(Id_LoteTextbox.Text);

            GenerarPrestaServicio(cab);

            cab.punto_de_venta = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
            lote.cabecera_lote = cab;

            FeaEntidades.InterFacturas.cabecera compcab = new FeaEntidades.InterFacturas.cabecera();

            FeaEntidades.InterFacturas.informacion_comprador infcompra = new FeaEntidades.InterFacturas.informacion_comprador();

            if (!MonedaComprobanteDropDownList.SelectedValue.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
            {
                Tipo_de_cambioLabel.Visible = true;
                Tipo_de_cambioTextBox.Visible = true;
            }
            else
            {
                Tipo_de_cambioLabel.Visible = false;
                Tipo_de_cambioTextBox.Visible = false;
                Tipo_de_cambioTextBox.Text = null;
            }

            GenerarInfoComprador(compcab, infcompra);
            FeaEntidades.InterFacturas.informacion_comprobante infcomprob = GenerarInfoComprobante();
            GenerarReferencias(infcomprob);
            GenerarInfoExportacion(comp, infcomprob);
            GenerarInfoExtensionesComerciales(comp);
            GenerarInfoExtensionesCamaraFacturas(comp);
            GenerarInfoExtensionesDestinatarios(comp);
            compcab.informacion_comprobante = infcomprob;
            GenerarInfoVendedor(compcab);
            comp.cabecera = compcab;

            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            string idtipo;
            try
            {
                idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
            }
            catch (NullReferenceException)
            {
                idtipo = "Comun";
            }
            FeaEntidades.InterFacturas.detalle det = DetalleLinea.GenerarDetalles(MonedaComprobanteDropDownList.SelectedValue, Tipo_de_cambioTextBox.Text, idtipo, Tipo_De_ComprobanteDropDownList.SelectedValue);

            det.comentarios = ComentariosTextBox.Text;
            comp.detalle = det;

            FeaEntidades.InterFacturas.resumen r = new FeaEntidades.InterFacturas.resumen();
            if (Tipo_de_cambioTextBox.Text != string.Empty)
            {
                r.tipo_de_cambio = Convert.ToDouble(Tipo_de_cambioTextBox.Text);
            }
            else
            {
                r.tipo_de_cambio = 1;
            }
            r.codigo_moneda = MonedaComprobanteDropDownList.SelectedValue;

            if (MonedaComprobanteDropDownList.SelectedValue.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
            //Moneda local
            {
                GenerarImportesMonedaLocal(r);
            }
            else
            //Moneda extranjera
            {
                GenerarImportesMonedaExtranjera(r);
            }

            r.observaciones = Observaciones_ResumenTextBox.Text;
            comp.resumen = r;
            System.Collections.Generic.List<FeaEntidades.InterFacturas.resumenImpuestos> listadeimpuestos = ImpuestosGlobales.Lista;
            auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion"))
                {
                    if (listadeimpuestos[0].importe_impuesto != 0 || listadeimpuestos.Count > 1)
                    {
                        ImpuestosGlobales.Focus();
                        throw new Exception("Los impuestos globales no se deben informar para exportación");
                    }
                }
                else
                {
                    ImpuestosGlobales.GenerarImpuestos(comp, MonedaComprobanteDropDownList.SelectedValue, Tipo_de_cambioTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                ImpuestosGlobales.GenerarImpuestos(comp, MonedaComprobanteDropDownList.SelectedValue, Tipo_de_cambioTextBox.Text);
            }
            //DescuentosGlobales.GenerarResumen(comp, MonedaComprobanteDropDownList.SelectedValue, Tipo_de_cambioTextBox.Text);
            lote.comprobante[0] = comp;
            return lote;
        }
        private void GenerarPrestaServicio(FeaEntidades.InterFacturas.cabecera_lote cab)
        {
            cab.presta_servSpecified = true;
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion"))
                {
                    cab.presta_servSpecified = false;
                }
                else if (idtipo.Equals("Comun"))
                {
                    GenerarV0oV1(cab);
                }
                else
                {
                    cab.presta_serv = Convert.ToInt32(Presta_ServCheckBox.Checked);
                }
            }
            catch (System.NullReferenceException)
            {
                cab.presta_serv = Convert.ToInt32(Presta_ServCheckBox.Checked);
            }
        }
        private void GenerarV0oV1(FeaEntidades.InterFacturas.cabecera_lote cab)
        {
            cab.presta_servSpecified = false;
        }
        private void GenerarInfoExtensionesDestinatarios(FeaEntidades.InterFacturas.comprobante comp)
        {
            if (!EmailAvisoVisualizacionTextBox.Text.Equals(string.Empty))
            {
                comp.extensionesSpecified = true;
                if (comp.extensiones == null)
                {
                    comp.extensiones = new FeaEntidades.InterFacturas.extensiones();
                }
                comp.extensiones.extensiones_destinatarios = new FeaEntidades.InterFacturas.extensionesExtensiones_destinatarios();
                comp.extensiones.extensiones_destinatarios.email = EmailAvisoVisualizacionTextBox.Text;
            }
        }
        private void GenerarInfoExtensionesComerciales(FeaEntidades.InterFacturas.comprobante comp)
        {
            if (!DatosComerciales.Texto.Equals(string.Empty))
            {
                comp.extensionesSpecified = true;
                if (comp.extensiones == null)
                {
                    comp.extensiones = new FeaEntidades.InterFacturas.extensiones();
                }
                string textoSinSaltoDeLinea = DatosComerciales.Texto.Replace(System.Environment.NewLine, "<br>");
                comp.extensiones.extensiones_datos_comerciales = RN.Funciones.ConvertToHex(textoSinSaltoDeLinea);
            }
        }
        private void GenerarInfoExtensionesCamaraFacturas(FeaEntidades.InterFacturas.comprobante comp)
        {
            if (!PasswordAvisoVisualizacionTextBox.Text.Equals(string.Empty))
            {
                comp.extensionesSpecified = true;
                if (comp.extensiones == null)
                {
                    comp.extensiones = new FeaEntidades.InterFacturas.extensiones();
                }
                if (comp.extensiones.extensiones_camara_facturas == null)
                {
                    comp.extensiones.extensiones_camara_facturas = new FeaEntidades.InterFacturas.extensionesExtensiones_camara_facturas();
                }
                comp.extensiones.extensiones_camara_facturas.clave_de_vinculacion = RN.Funciones.CreateMD5Hash(PasswordAvisoVisualizacionTextBox.Text);
                comp.extensiones.extensiones_camara_facturasSpecified = true;
            }
        }
        private FeaEntidades.InterFacturas.informacion_comprobante GenerarInfoComprobante()
        {
            FeaEntidades.InterFacturas.informacion_comprobante infcomprob = new FeaEntidades.InterFacturas.informacion_comprobante();
            infcomprob.tipo_de_comprobante = Convert.ToInt32(Tipo_De_ComprobanteDropDownList.SelectedValue);
            infcomprob.numero_comprobante = Convert.ToInt64(Numero_ComprobanteTextBox.Text);
            infcomprob.punto_de_venta = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
            infcomprob.fecha_emision = FechaEmisionDatePickerWebUserControl.Text;
            GenerarInfoFechaVto(infcomprob);
            infcomprob.fecha_serv_desde = FechaServDesdeDatePickerWebUserControl.Text;
            infcomprob.fecha_serv_hasta = FechaServHastaDatePickerWebUserControl.Text;

            GenerarIVAComputable(infcomprob);

            if (!Condicion_De_PagoTextBox.Text.Equals(string.Empty))
            {
                infcomprob.condicion_de_pago = Condicion_De_PagoTextBox.Text;
                infcomprob.condicion_de_pagoSpecified = true;
            }
            else
            {
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        throw new Exception("La condición de pago es obligatoria para exportación");
                    }
                    else
                    {
                        infcomprob.condicion_de_pago = string.Empty;
                        infcomprob.condicion_de_pagoSpecified = false;
                    }
                }
                catch (System.NullReferenceException)
                {
                    infcomprob.condicion_de_pago = Condicion_De_PagoTextBox.Text;
                    infcomprob.condicion_de_pagoSpecified = false;
                }
            }

            GenerarCodigoOperacion(infcomprob);
            GenerarCodigoConcepto(infcomprob);

            if (!CAETextBox.Text.Equals(string.Empty))
            {
                infcomprob.cae = CAETextBox.Text;
                infcomprob.caeSpecified = true;
            }
            else
            {
                infcomprob.cae = null;
                infcomprob.caeSpecified = false;
            }
            if (!FechaCAEObtencionDatePickerWebUserControl.Text.ToString().Equals(string.Empty))
            {
                infcomprob.fecha_obtencion_cae = FechaCAEObtencionDatePickerWebUserControl.Text;
                infcomprob.fecha_obtencion_caeSpecified = true;
            }
            else
            {
                infcomprob.fecha_obtencion_cae = null;
                infcomprob.fecha_obtencion_caeSpecified = false;
            }

            if (!FechaCAEVencimientoDatePickerWebUserControl.Text.Equals(string.Empty))
            {
                infcomprob.fecha_vencimiento_cae = FechaCAEVencimientoDatePickerWebUserControl.Text;
                infcomprob.fecha_vencimiento_caeSpecified = true;
            }
            else
            {
                infcomprob.fecha_vencimiento_cae = null;
                infcomprob.fecha_vencimiento_caeSpecified = true;
            }
            if (!ResultadoTextBox.Text.Equals(string.Empty))
            {
                infcomprob.resultado = ResultadoTextBox.Text;
            }
            if (!MotivoTextBox.Text.Equals(string.Empty))
            {
                infcomprob.motivo = MotivoTextBox.Text;
            }
            return infcomprob;
        }
        private void GenerarInfoFechaVto(FeaEntidades.InterFacturas.informacion_comprobante infcomprob)
        {
            if (!FechaVencimientoDatePickerWebUserControl.Text.Equals(string.Empty))
            {
                infcomprob.fecha_vencimiento = FechaVencimientoDatePickerWebUserControl.Text;
            }
            else
            {
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (!idtipo.Equals("Exportacion"))
                    {
                        throw new Exception("La fecha de vencimiento es obligatoria");
                    }
                }
                catch (System.NullReferenceException)
                {
                    throw new Exception("La fecha de vencimiento es obligatoria");
                }
            }
        }
        private void GenerarIVAComputable(FeaEntidades.InterFacturas.informacion_comprobante infcomprob)
        {
            //No se tiene que informar para exportación
            if (!IVAcomputableDropDownList.SelectedValue.Equals(string.Empty))
            {
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        IVAcomputableDropDownList.Focus();
                        throw new Exception("El IVA computable no se debe informar para exportación");
                    }
                    else
                    {
                        infcomprob.iva_computable = IVAcomputableDropDownList.SelectedValue;
                    }
                }
                catch (System.NullReferenceException)
                {
                    infcomprob.iva_computable = IVAcomputableDropDownList.SelectedValue;
                }
            }
        }
        private void GenerarCodigoOperacion(FeaEntidades.InterFacturas.informacion_comprobante infcomprob)
        {
            //No se tiene que informar para exportación
            //El nodo no se debe informar para RG2904 (solo NC A y ND A)
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (!CodigoOperacionDropDownList.SelectedValue.Equals(string.Empty))
                {
                    if (idtipo.Equals("Exportacion"))
                    {
                        CodigoOperacionDropDownList.Focus();
                        throw new Exception("El código de operación no se debe informar para exportación");
                    }
                    else if (idtipo.Equals("RG2904") && (Tipo_De_ComprobanteDropDownList.SelectedValue.Equals("2") || Tipo_De_ComprobanteDropDownList.SelectedValue.Equals("3")))
                    {
                        infcomprob.codigo_operacion = string.Empty;
                        infcomprob.codigo_operacionSpecified = false;
                    }
                    else
                    {
                        infcomprob.codigo_operacion = CodigoOperacionDropDownList.SelectedValue;
                        infcomprob.codigo_operacionSpecified = true;
                    }
                }
                else
                {
                    if (!idtipo.Equals("Exportacion"))
                    {
                        if (idtipo.Equals("RG2904") && (Tipo_De_ComprobanteDropDownList.SelectedValue.Equals("2") || Tipo_De_ComprobanteDropDownList.SelectedValue.Equals("3")))
                        {
                            infcomprob.codigo_operacion = string.Empty;
                            infcomprob.codigo_operacionSpecified = false;
                        }
                        else
                        {
                            infcomprob.codigo_operacion = CodigoOperacionDropDownList.SelectedValue;
                            infcomprob.codigo_operacionSpecified = true;
                        }

                    }
                }
            }
            catch (System.NullReferenceException)
            {
                infcomprob.codigo_operacion = CodigoOperacionDropDownList.SelectedValue;
                infcomprob.codigo_operacionSpecified = true;
            }
        }
        private void GenerarCodigoConcepto(FeaEntidades.InterFacturas.informacion_comprobante infcomprob)
        {
            //Se tiene que informar para versión 1 de punto común
            if (CodigoConceptoDropDownList.Visible)
            {
                infcomprob.codigo_concepto = Convert.ToInt32(CodigoConceptoDropDownList.SelectedValue);
                infcomprob.codigo_conceptoSpecified = true;
            }
            else
            {
                infcomprob.codigo_conceptoSpecified = false;
            }
        }
        private void GenerarReferencias(FeaEntidades.InterFacturas.informacion_comprobante infcomprob)
        {
            if (this.InfoReferencias.HayReferencias)
            {
                //infcomprob.referencias = new FeaEntidades.InterFacturas.informacion_comprobanteReferencias[5];
                for (int i = 0; i < this.InfoReferencias.ListaReferencias.Count; i++)
                {
                    if (infcomprob.tipo_de_comprobante.Equals(19))
                    {
                        throw new Exception("Las referencias no se deben informar para facturas de exportación(19). Sólo para notas de débito y/o crédito (20 y 21).");
                    }
                    infcomprob.referencias[i] = new FeaEntidades.InterFacturas.informacion_comprobanteReferencias();
                    infcomprob.referencias[i].descripcioncodigo_de_referencia = this.InfoReferencias.ListaReferencias[i].descripcioncodigo_de_referencia;
                    infcomprob.referencias[i].dato_de_referencia = this.InfoReferencias.ListaReferencias[i].dato_de_referencia;
                    infcomprob.referencias[i].codigo_de_referencia = this.InfoReferencias.ListaReferencias[i].codigo_de_referencia;
                }
            }
        }
        private void GenerarInfoExportacion(FeaEntidades.InterFacturas.comprobante comp, FeaEntidades.InterFacturas.informacion_comprobante infcomprob)
        {
            FeaEntidades.InterFacturas.informacion_exportacion ie = new FeaEntidades.InterFacturas.informacion_exportacion();
            bool exportacion = false;
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                string tipoComp = Tipo_De_ComprobanteDropDownList.SelectedValue;
                string tipoExp = TipoExpDropDownList.SelectedValue;
                if (idtipo.Equals("Exportacion"))
                {
                    if (tipoComp.Equals("19"))
                    {
                        if (PaisDestinoExpDropDownList.SelectedValue.Equals("0"))
                        {
                            throw new Exception("El país destino de exportación es obligatorio");
                        }
                        if (IncotermsDropDownList.SelectedValue.Equals(string.Empty))
                        {
                            throw new Exception("Incoterms es obligatorio");
                        }
                        if (tipoExp.Equals("0"))
                        {
                            throw new Exception("El tipo de exportación es obligatorio");
                        }
                        if (IdiomaDropDownList.SelectedValue.Equals("0"))
                        {
                            throw new Exception("El idioma es obligatorio");
                        }
                    }
                    else //NC y ND
                    {
                        if (PaisDestinoExpDropDownList.SelectedValue.Equals("0"))
                        {
                            throw new Exception("El país destino de exportación es obligatorio");
                        }
                        if (tipoExp.Equals("0"))
                        {
                            throw new Exception("El tipo de exportación es obligatorio");
                        }
                        if (IdiomaDropDownList.SelectedValue.Equals("0"))
                        {
                            throw new Exception("El idioma es obligatorio");
                        }
                    }
                }
            }
            catch (System.NullReferenceException)
            {
            }
            if (!PaisDestinoExpDropDownList.SelectedValue.Equals("0"))
            {
                ie.destino_comprobante = Convert.ToInt32(PaisDestinoExpDropDownList.SelectedValue);
                exportacion = true;
            }
            if (!IncotermsDropDownList.SelectedValue.Equals(string.Empty))
            {
                ie.incoterms = IncotermsDropDownList.SelectedValue;
                exportacion = true;
            }
            if (!TipoExpDropDownList.SelectedValue.Equals("0"))
            {
                ie.tipo_exportacion = Convert.ToInt32(TipoExpDropDownList.SelectedValue);
                exportacion = true;
            }
            if (!IdiomaDropDownList.SelectedValue.Equals("0"))
            {
                comp.extensionesSpecified = true;
                comp.extensiones = new FeaEntidades.InterFacturas.extensiones();
                comp.extensiones.extensiones_camara_facturasSpecified = true;
                comp.extensiones.extensiones_camara_facturas = new FeaEntidades.InterFacturas.extensionesExtensiones_camara_facturas();
                comp.extensiones.extensiones_camara_facturas.id_idioma = IdiomaDropDownList.SelectedValue;
                exportacion = true;
            }

            GenerarInfoPermisosExportacion(ie, infcomprob);

            if (exportacion)
            {
                infcomprob.informacion_exportacion = ie;
            }
        }
        private void GenerarInfoPermisosExportacion(FeaEntidades.InterFacturas.informacion_exportacion ie, FeaEntidades.InterFacturas.informacion_comprobante infcomprob)
        {
            if (infcomprob.tipo_de_comprobante.Equals(19) && ie.tipo_exportacion.Equals(1))
            {
                if (this.PermisosExpo.HayPermisos)
                {
                    ie.permiso_existente = "S";
                    ie.permisos = new FeaEntidades.InterFacturas.permisos[5];
                    for (int i = 0; i < this.PermisosExpo.PermisosExportacion.Count; i++)
                    {
                        ie.permisos[i] = new FeaEntidades.InterFacturas.permisos();
                        ie.permisos[i].descripcion_destino_mercaderia = this.PermisosExpo.PermisosExportacion[i].descripcion_destino_mercaderia;
                        ie.permisos[i].destino_mercaderia = this.PermisosExpo.PermisosExportacion[i].destino_mercaderia;
                        ie.permisos[i].id_permiso = this.PermisosExpo.PermisosExportacion[i].id_permiso;
                    }
                }
                else
                {
                    ie.permiso_existente = "N";
                }
            }
            else
            {
                if (this.PermisosExpo.HayPermisos)
                {
                    throw new Exception("No se deben informar permisos de exportación para este tipo de comprobante");
                }
            }
        }
        private void GenerarInfoVendedor(FeaEntidades.InterFacturas.cabecera compcab)
        {
            FeaEntidades.InterFacturas.informacion_vendedor infovend = new FeaEntidades.InterFacturas.informacion_vendedor();
            if (!GLN_VendedorTextBox.Text.Equals(string.Empty))
            {
                infovend.GLN = Convert.ToInt64(GLN_VendedorTextBox.Text);
                infovend.GLNSpecified = true;
            }
            infovend.codigo_interno = Codigo_Interno_VendedorTextBox.Text;
            infovend.razon_social = Razon_Social_VendedorTextBox.Text;
            infovend.cuit = Convert.ToInt64(Cuit_VendedorTextBox.Text);
            int auxCondIVAVend = Convert.ToInt32(Condicion_IVA_VendedorDropDownList.SelectedValue);
            if (!auxCondIVAVend.Equals(0))
            {
                infovend.condicion_IVASpecified = true;
                infovend.condicion_IVA = auxCondIVAVend;
            }

            try
            {
                infovend.condicion_ingresos_brutos = Convert.ToInt32(Condicion_Ingresos_Brutos_VendedorDropDownList.SelectedValue);
                infovend.nro_ingresos_brutos = NroIBVendedorTextBox.Text;
            }
            catch
            {

            }
            finally
            {
                if (infovend.condicion_ingresos_brutos != 0)
                {
                    infovend.condicion_ingresos_brutosSpecified = true;
                }
                else
                {
                    infovend.nro_ingresos_brutos = null;
                }
            }
            infovend.inicio_de_actividades = InicioDeActividadesVendedorDatePickerWebUserControl.Text;
            infovend.contacto = Contacto_VendedorTextBox.Text;
            infovend.domicilio_calle = Domicilio_Calle_VendedorTextBox.Text;
            infovend.domicilio_numero = Domicilio_Numero_VendedorTextBox.Text;
            infovend.domicilio_piso = Domicilio_Piso_VendedorTextBox.Text;
            infovend.domicilio_depto = Domicilio_Depto_VendedorTextBox.Text;
            infovend.domicilio_sector = Domicilio_Sector_VendedorTextBox.Text;
            infovend.domicilio_torre = Domicilio_Torre_VendedorTextBox.Text;
            infovend.domicilio_manzana = Domicilio_Manzana_VendedorTextBox.Text;
            infovend.localidad = Localidad_VendedorTextBox.Text;
            string auxCodProvVend = Convert.ToString(Provincia_VendedorDropDownList.SelectedValue);
            if (!auxCodProvVend.Equals("0"))
            {
                infovend.provincia = auxCodProvVend;
            }
            infovend.cp = Cp_VendedorTextBox.Text;
            infovend.email = Email_VendedorTextBox.Text;
            infovend.telefono = Telefono_VendedorTextBox.Text;
            compcab.informacion_vendedor = infovend;
        }
        private void GenerarInfoComprador(FeaEntidades.InterFacturas.cabecera compcab, FeaEntidades.InterFacturas.informacion_comprador infcompra)
        {
            if (!GLN_CompradorTextBox.Text.Equals(string.Empty))
            {
                infcompra.GLN = Convert.ToInt64(GLN_CompradorTextBox.Text);
                infcompra.GLNSpecified = true;
            }
            infcompra.codigo_interno = Codigo_Interno_CompradorTextBox.Text;
            try
            {
                infcompra.codigo_doc_identificatorio = Convert.ToInt32(Codigo_Doc_Identificatorio_CompradorDropDownList.SelectedValue);
            }
            catch (FormatException)
            {
                throw new Exception("Tipo de documento del comprador no informado");
            }

            if (Nro_Doc_Identificatorio_CompradorTextBox.Visible)
            {
                try
                {
                    infcompra.nro_doc_identificatorio = Convert.ToInt64(Nro_Doc_Identificatorio_CompradorTextBox.Text);
                }
                catch (FormatException)
                {
                    throw new Exception("Nro documento del comprador no informado");
                }
            }
            else
            {
                try
                {
                    infcompra.nro_doc_identificatorio = Convert.ToInt64(Nro_Doc_Identificatorio_CompradorDropDownList.SelectedValue);
                }
                catch (FormatException)
                {
                    throw new Exception("Nro documento del comprador para exportación no informado");
                }
            }
            infcompra.denominacion = Denominacion_CompradorTextBox.Text;
            int auxCondIVACompra = Convert.ToInt32(Condicion_IVA_CompradorDropDownList.SelectedValue);
            if (!auxCondIVACompra.Equals(0))
            {
                infcompra.condicion_IVASpecified = true;
                infcompra.condicion_IVA = auxCondIVACompra;
            }
            //infcompra.condicion_ingresos_brutosSpecified = true;
            //infcompra.condicion_ingresos_brutos = Convert.ToInt32(Condicion_Ingresos_Brutos_CompradorDropDownList.SelectedValue);
            //infcompra.nro_ingresos_brutos
            infcompra.inicio_de_actividades = InicioDeActividadesCompradorDatePickerWebUserControl.Text;
            infcompra.contacto = Contacto_CompradorTextBox.Text;

            //obligatorio para exportación
            if (Domicilio_Calle_CompradorTextBox.Text.Equals(string.Empty))
            {
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        Domicilio_Calle_CompradorTextBox.Focus();
                        throw new Exception("La calle del domicilio del comprador es obligatoria para exportación");
                    }
                    else
                    {
                        infcompra.domicilio_calle = string.Empty;
                    }
                }
                catch (System.NullReferenceException)
                {
                    infcompra.domicilio_calle = string.Empty;
                }
            }
            else
            {
                infcompra.domicilio_calle = Domicilio_Calle_CompradorTextBox.Text;
            }

            //obligatorio para exportación
            if (Domicilio_Numero_CompradorTextBox.Text.Equals(string.Empty))
            {
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        Domicilio_Numero_CompradorTextBox.Focus();
                        throw new Exception("El número de la calle del domicilio del comprador es obligatorio para exportación");
                    }
                    else
                    {
                        infcompra.domicilio_numero = string.Empty;
                    }
                }
                catch (System.NullReferenceException)
                {
                    infcompra.domicilio_numero = string.Empty;
                }
            }
            else
            {
                infcompra.domicilio_numero = Domicilio_Numero_CompradorTextBox.Text;
            }
            infcompra.domicilio_piso = Domicilio_Piso_CompradorTextBox.Text;
            infcompra.domicilio_depto = Domicilio_Depto_CompradorTextBox.Text;
            infcompra.domicilio_sector = Domicilio_Sector_CompradorTextBox.Text;
            infcompra.domicilio_torre = Domicilio_Torre_CompradorTextBox.Text;
            infcompra.domicilio_manzana = Domicilio_Manzana_CompradorTextBox.Text;
            infcompra.localidad = Localidad_CompradorTextBox.Text;
            string auxCodProvCompra = Convert.ToString(Provincia_CompradorDropDownList.SelectedValue);
            //No se tiene que informar para exportación
            if (!auxCodProvCompra.Equals("0"))
            {
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        if (!PaisDestinoExpDropDownList.SelectedItem.Text.ToUpper().Contains("ARGENTINA"))
                        {
                            Provincia_CompradorDropDownList.Focus();
                            throw new Exception("La provincia del domicilio del comprador no se debe informar para exportación");
                        }
                        else
                        {
                            infcompra.provincia = auxCodProvCompra;
                        }
                    }
                    else
                    {
                        infcompra.provincia = auxCodProvCompra;
                    }
                }
                catch (System.NullReferenceException)
                {
                    infcompra.provincia = auxCodProvCompra;
                }
            }
            infcompra.cp = Cp_CompradorTextBox.Text;
            infcompra.email = Email_CompradorTextBox.Text;
            infcompra.telefono = Telefono_CompradorTextBox.Text;

            compcab.informacion_comprador = infcompra;
        }
        private void GenerarImportesMonedaExtranjera(FeaEntidades.InterFacturas.resumen r)
        {
            double tipodecambio = Convert.ToDouble(Tipo_de_cambioTextBox.Text);

            FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo = new FeaEntidades.InterFacturas.resumenImportes_moneda_origen();

            GenerarImporteTotalNetoGravadoExtranjera(r, tipodecambio, rimo);
            GenerarImporteTotalConceptoNoGravadoExtranjera(r, tipodecambio, rimo);
            GenerarImporteOperacionesExentasExtranjera(r, tipodecambio, rimo);
            GenerarImpuestoLiqExtranjera(r, tipodecambio, rimo);
            GenerarImpuestoLiqRNIExtranjera(r, tipodecambio, rimo);

            //para exportación no se debe informar
            try
            {
                double importe_total_impuestos_nacionales = Convert.ToDouble(Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_impuestos_nacionalesSpecified = false;
                        rimo.importe_total_impuestos_nacionalesSpecified = false;
                        throw new Exception("El importe total de impuestos nacionales en moneda extranjera no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalImpuestosNacionalesMonedaExtranjera(r, tipodecambio, rimo);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalImpuestosNacionalesMonedaExtranjera(r, tipodecambio, rimo);
                }
            }
            catch (FormatException)
            {
            }
            //para exportación no se debe informar
            try
            {
                double importe_total_ingresos_brutos = Convert.ToDouble(Importe_Total_Ingresos_Brutos_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_ingresos_brutosSpecified = false;
                        rimo.importe_total_ingresos_brutosSpecified = false;
                        throw new Exception("El importe total de ingresos brutos en moneda extranjera no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalIngresosBrutosMonedaExtranjera(r, tipodecambio, rimo);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalIngresosBrutosMonedaExtranjera(r, tipodecambio, rimo);
                }
            }
            catch (FormatException)
            {
            }
            //para exportación no se debe informar
            try
            {
                double importe_total_impuestos_municipales = Convert.ToDouble(Importe_Total_Impuestos_Municipales_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_impuestos_municipalesSpecified = false;
                        rimo.importe_total_impuestos_municipalesSpecified = false;
                        throw new Exception("El importe total de impuestos municipales en moneda extranjera no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalImpuestosMunicipalesMonedaExtranjera(r, tipodecambio, rimo);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalImpuestosMunicipalesMonedaExtranjera(r, tipodecambio, rimo);
                }
            }
            catch (FormatException)
            {
            }
            //para exportación no se debe informar
            try
            {
                //double importe_total_impuestos_internos = Convert.ToDouble(Importe_Total_Impuestos_Internos_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_impuestos_internosSpecified = false;
                        rimo.importe_total_impuestos_internosSpecified = false;
                        throw new Exception("El importe total de impuestos internos en moneda extranjera no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalImpuestosInternosMonedaExtranjera(r, tipodecambio, rimo);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalImpuestosInternosMonedaExtranjera(r, tipodecambio, rimo);
                }
            }
            catch (FormatException)
            {
            }
            r.importe_total_factura = 0;
            rimo.importe_total_factura = Convert.ToDouble(Importe_Total_Factura_ResumenTextBox.Text);
            r.importes_moneda_origen = rimo;
        }
        private void GenerarImpuestoLiqRNIExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            //para exportación se debe informar en 0
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Impuesto_Liq_Rni_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El Impuesto liquidado a RNI o percepción a no categorizados debe informarse en 0 para exportación.");
                }
                else
                {
                    r.impuesto_liq_rni = 0;
                    rimo.impuesto_liq_rni = Convert.ToDouble(Impuesto_Liq_Rni_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.impuesto_liq_rni = Math.Round(Convert.ToDouble(Impuesto_Liq_Rni_ResumenTextBox.Text) * tipodecambio, 2);
                rimo.impuesto_liq_rni = Convert.ToDouble(Impuesto_Liq_Rni_ResumenTextBox.Text);
            }
        }
        private void GenerarImpuestoLiqExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            //para exportación se debe informar en 0
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Impuesto_Liq_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El IVA Responsable inscripto debe informarse en 0 para exportación.");
                }
                else
                {
                    r.impuesto_liq = 0;
                    rimo.impuesto_liq = Convert.ToDouble(Impuesto_Liq_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.impuesto_liq = Math.Round(Convert.ToDouble(Impuesto_Liq_ResumenTextBox.Text) * tipodecambio, 2);
                rimo.impuesto_liq = Convert.ToDouble(Impuesto_Liq_ResumenTextBox.Text);
            }
        }
        private void GenerarImporteOperacionesExentasExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            //para exportación se debe informar en 0
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Importe_Operaciones_Exentas_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El importe de operaciones exentas debe informarse en 0 para exportación.");
                }
                else
                {
                    r.importe_operaciones_exentas = 0;
                    rimo.importe_operaciones_exentas = Convert.ToDouble(Importe_Operaciones_Exentas_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.importe_operaciones_exentas = Math.Round(Convert.ToDouble(Importe_Operaciones_Exentas_ResumenTextBox.Text) * tipodecambio, 2);
                rimo.importe_operaciones_exentas = Convert.ToDouble(Importe_Operaciones_Exentas_ResumenTextBox.Text);
            }
        }
        private void GenerarImporteTotalConceptoNoGravadoExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            //para exportación se debe informar en 0
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El importe total de conceptos que no integren el precio neto gravado debe informarse en 0 para exportación.");
                }
                else
                {
                    r.importe_total_concepto_no_gravado = 0;
                    rimo.importe_total_concepto_no_gravado = Convert.ToDouble(Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.importe_total_concepto_no_gravado = Math.Round(Convert.ToDouble(Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text) * tipodecambio, 2);
                rimo.importe_total_concepto_no_gravado = Convert.ToDouble(Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text);
            }
        }
        private void GenerarImporteTotalNetoGravadoExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            //para exportación se debe informar en 0
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Importe_Total_Neto_Gravado_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El importe total neto gravado debe informarse en 0 para exportación.");
                }
                else
                {
                    r.importe_total_neto_gravado = 0;
                    rimo.importe_total_neto_gravado = Convert.ToDouble(Importe_Total_Neto_Gravado_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.importe_total_neto_gravado = Math.Round(Convert.ToDouble(Importe_Total_Neto_Gravado_ResumenTextBox.Text) * tipodecambio, 2);
                rimo.importe_total_neto_gravado = Convert.ToDouble(Importe_Total_Neto_Gravado_ResumenTextBox.Text);
            }
        }
        private void GenerarImporteTotalImpuestosInternosMonedaExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            r.importe_total_impuestos_internos = 0;
            rimo.importe_total_impuestos_internos = Convert.ToDouble(Importe_Total_Impuestos_Internos_ResumenTextBox.Text);
            if (rimo.importe_total_impuestos_internos != 0)
            {
                r.importe_total_impuestos_internosSpecified = true;
                rimo.importe_total_impuestos_internosSpecified = true;
            }
        }
        private void GenerarImporteTotalImpuestosMunicipalesMonedaExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            r.importe_total_impuestos_municipales = 0;
            rimo.importe_total_impuestos_municipales = Convert.ToDouble(Importe_Total_Impuestos_Municipales_ResumenTextBox.Text);
            if (rimo.importe_total_impuestos_municipales != 0)
            {
                r.importe_total_impuestos_municipalesSpecified = true;
                rimo.importe_total_impuestos_municipalesSpecified = true;
            }
        }
        private void GenerarImporteTotalIngresosBrutosMonedaExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            r.importe_total_ingresos_brutos = 0;
            rimo.importe_total_ingresos_brutos = Convert.ToDouble(Importe_Total_Ingresos_Brutos_ResumenTextBox.Text);
            if (rimo.importe_total_ingresos_brutos != 0)
            {
                r.importe_total_ingresos_brutosSpecified = true;
                rimo.importe_total_ingresos_brutosSpecified = true;
            }
        }
        private void GenerarImporteTotalImpuestosNacionalesMonedaExtranjera(FeaEntidades.InterFacturas.resumen r, double tipodecambio, FeaEntidades.InterFacturas.resumenImportes_moneda_origen rimo)
        {
            r.importe_total_impuestos_nacionales = 0;
            rimo.importe_total_impuestos_nacionales = Convert.ToDouble(Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text);
            if (rimo.importe_total_impuestos_nacionales != 0)
            {
                r.importe_total_impuestos_nacionalesSpecified = true;
                rimo.importe_total_impuestos_nacionalesSpecified = true;
            }
        }
        private void GenerarImportesMonedaLocal(FeaEntidades.InterFacturas.resumen r)
        {
            GenerarImporteTotalNetoGravado(r);
            GenerarImporteTotalConceptoNoGravado(r);
            GenerarImporteOperacionesExentas(r);
            GenerarImpuestoLiq(r);
            GenerarImpuestoLiqRNI(r);

            //para exportación no se debe informar
            try
            {
                double importe_total_impuestos_nacionales = Convert.ToDouble(Importe_Total_Impuestos_Nacionales_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_impuestos_nacionalesSpecified = false;
                        throw new Exception("El importe total de impuestos nacionales no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalImpuestosNacionales(r, importe_total_impuestos_nacionales);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalImpuestosNacionales(r, importe_total_impuestos_nacionales);
                }
            }
            catch (FormatException)
            {
            }
            //para exportación no se debe informar
            try
            {
                double importe_total_ingresos_brutos = Convert.ToDouble(Importe_Total_Ingresos_Brutos_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_ingresos_brutosSpecified = false;
                        throw new Exception("El importe total de ingresos brutos no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalIngresosBrutos(r);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalIngresosBrutos(r);
                }
            }
            catch (FormatException)
            {
            }
            //para exportación no se debe informar
            try
            {
                double importe_total_impuestos_municipales = Convert.ToDouble(Importe_Total_Impuestos_Municipales_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_impuestos_municipalesSpecified = false;
                        throw new Exception("El importe total de impuestos municipales no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalImpuestosMunicipales(r);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalImpuestosMunicipales(r);
                }
            }
            catch (FormatException)
            {
            }
            //para exportación no se debe informar
            try
            {
                double importe_total_impuestos_internos = Convert.ToDouble(Importe_Total_Impuestos_Internos_ResumenTextBox.Text);
                int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
                try
                {
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    if (idtipo.Equals("Exportacion"))
                    {
                        r.importe_total_impuestos_internosSpecified = false;
                        throw new Exception("El importe total de impuestos internos no se debe informar para exportación");
                    }
                    else
                    {
                        GenerarImporteTotalImpuestosInternos(r);
                    }
                }
                catch (System.NullReferenceException)
                {
                    GenerarImporteTotalImpuestosInternos(r);
                }
            }
            catch (FormatException)
            {
            }
            r.importe_total_factura = Convert.ToDouble(Importe_Total_Factura_ResumenTextBox.Text);
        }
        private void GenerarImpuestoLiqRNI(FeaEntidades.InterFacturas.resumen r)
        {
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Impuesto_Liq_Rni_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El Impuesto liquidado a RNI o percepción a no categorizados debe informarse en 0 para exportación.");
                }
                else
                {
                    r.impuesto_liq_rni = Convert.ToDouble(Impuesto_Liq_Rni_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.impuesto_liq_rni = Convert.ToDouble(Impuesto_Liq_Rni_ResumenTextBox.Text);
            }
        }
        private void GenerarImpuestoLiq(FeaEntidades.InterFacturas.resumen r)
        {
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Impuesto_Liq_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El IVA Responsable inscripto debe informarse en 0 para exportación.");
                }
                else
                {
                    r.impuesto_liq = Convert.ToDouble(Impuesto_Liq_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.impuesto_liq = Convert.ToDouble(Impuesto_Liq_ResumenTextBox.Text);
            }
        }
        private void GenerarImporteOperacionesExentas(FeaEntidades.InterFacturas.resumen r)
        {
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Importe_Operaciones_Exentas_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El importe de operaciones exentas debe informarse en 0 para exportación.");
                }
                else
                {
                    r.importe_operaciones_exentas = Convert.ToDouble(Importe_Operaciones_Exentas_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.importe_operaciones_exentas = Convert.ToDouble(Importe_Operaciones_Exentas_ResumenTextBox.Text);
            }
        }
        private void GenerarImporteTotalConceptoNoGravado(FeaEntidades.InterFacturas.resumen r)
        {
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El importe total de conceptos que no integren el precio neto gravado debe informarse en 0 para exportación.");
                }
                else
                {
                    r.importe_total_concepto_no_gravado = Convert.ToDouble(Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.importe_total_concepto_no_gravado = Convert.ToDouble(Importe_Total_Concepto_No_Gravado_ResumenTextBox.Text);
            }

        }
        private void GenerarImporteTotalNetoGravado(FeaEntidades.InterFacturas.resumen r)
        {
            int auxPV = Convert.ToInt32(((DropDownList)PuntoVtaDropDownList).SelectedValue);
            try
            {
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("Exportacion") && !Importe_Total_Neto_Gravado_ResumenTextBox.Text.Equals("0"))
                {
                    throw new Exception("El importe total neto gravado debe informarse en 0 para exportación.");
                }
                else
                {
                    r.importe_total_neto_gravado = Convert.ToDouble(Importe_Total_Neto_Gravado_ResumenTextBox.Text);
                }
            }
            catch (System.NullReferenceException)
            {
                r.importe_total_neto_gravado = Convert.ToDouble(Importe_Total_Neto_Gravado_ResumenTextBox.Text);
            }
        }
        private void GenerarImporteTotalImpuestosInternos(FeaEntidades.InterFacturas.resumen r)
        {
            r.importe_total_impuestos_internos = Convert.ToDouble(Importe_Total_Impuestos_Internos_ResumenTextBox.Text);
            if (r.importe_total_impuestos_internos != 0)
            {
                r.importe_total_impuestos_internosSpecified = true;
            }
        }
        private void GenerarImporteTotalImpuestosMunicipales(FeaEntidades.InterFacturas.resumen r)
        {
            r.importe_total_impuestos_municipales = Convert.ToDouble(Importe_Total_Impuestos_Municipales_ResumenTextBox.Text);
            if (r.importe_total_impuestos_municipales != 0)
            {
                r.importe_total_impuestos_municipalesSpecified = true;
            }
        }
        private void GenerarImporteTotalIngresosBrutos(FeaEntidades.InterFacturas.resumen r)
        {
            r.importe_total_ingresos_brutos = Convert.ToDouble(Importe_Total_Ingresos_Brutos_ResumenTextBox.Text);
            if (r.importe_total_ingresos_brutos != 0)
            {
                r.importe_total_ingresos_brutosSpecified = true;
            }
        }
        private static void GenerarImporteTotalImpuestosNacionales(FeaEntidades.InterFacturas.resumen r, double importe_total_impuestos_nacionales)
        {
            r.importe_total_impuestos_nacionales = importe_total_impuestos_nacionales;
            r.importe_total_impuestos_nacionalesSpecified = true;
        }
        private void GrabarLogTexto(string archivo, string mensaje)
        {
            try
            {
                using (FileStream fs = File.Open(Server.MapPath(archivo), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "  " + mensaje);
                    }
                }
            }
            catch
            {
            }
        }
        protected void AccionBaja_AnulBajaButton_Click(object sender, EventArgs e)
        {
            if (Funciones.SessionTimeOut(Session))
            {
                Response.Redirect("~/SessionTimeout.aspx");
            }
            else
            {
                Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];
                if (sesion.Usuario.Id == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Su sesión ha caducado por inactividad. Por favor vuelva a loguearse."), false);
                }
                else
                {
                    string mensaje = String.Empty;
                    try
                    {
                        Entidades.Comprobante comprobante = new Entidades.Comprobante();
                        comprobante.NaturalezaComprobante.Id = IdNaturalezaComprobanteTextBox.Text;
                        comprobante.Cuit = sesion.Cuit.Nro;
                        comprobante.TipoComprobante.Id = Convert.ToInt32(Tipo_De_ComprobanteDropDownList.SelectedValue);
                        if (comprobante.NaturalezaComprobante.Id != "Compra")
                        {
                            comprobante.NroPuntoVta = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
                        }
                        else
                        {
                            comprobante.NroPuntoVta = Convert.ToInt32(PuntoVtaTextBox.Text);
                            comprobante.Documento.Tipo.Id = "80";
                            comprobante.Documento.Nro = Cuit_VendedorTextBox.Text;
                        }
                        comprobante.Nro = Math.Abs(Convert.ToInt64(Numero_ComprobanteTextBox.Text));
                        RN.Comprobante.Leer(comprobante, sesion);
                        if (comprobante.Estado != "DeBaja")
                        {
                            RN.Comprobante.DarDeBaja(comprobante, sesion);
                            mensaje = "Baja de " + ((IdNaturalezaComprobanteTextBox.Text == "VentaContrato") ? "Contrato" : "Comprobante") + " registrada satisfactoriamente";
                        }
                        else
                        {
                            RN.Comprobante.AnularBaja(comprobante, sesion);
                            mensaje = "Anulación de baja de " + ((IdNaturalezaComprobanteTextBox.Text == "VentaContrato") ? "Contrato" : "Comprobante") + " registrada satisfactoriamente";
                        }
                        AccionesPanel.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        mensaje = "Problemas en la baja/anul.baja del " + ((IdNaturalezaComprobanteTextBox.Text == "VentaContrato") ? "Contrato" : "Comprobante") + ".  " + ex.Message;
                    }
                    finally
                    {
                        ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript(mensaje));
                    }
                }
            }
        }
        protected void AccionBaja_FisicaButton_Click(object sender, EventArgs e)
        {
            if (Funciones.SessionTimeOut(Session))
            {
                Response.Redirect("~/SessionTimeout.aspx");
            }
            else
            {
                Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];
                if (sesion.Usuario.Id == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Su sesión ha caducado por inactividad. Por favor vuelva a loguearse."), false);
                }
                else
                {
                    string mensaje = String.Empty;
                    try
                    {
                        Entidades.Comprobante comprobante = new Entidades.Comprobante();
                        comprobante.Cuit = sesion.Cuit.Nro;
                        comprobante.NaturalezaComprobante.Id = IdNaturalezaComprobanteTextBox.Text;
                        comprobante.TipoComprobante.Id = Convert.ToInt32(Tipo_De_ComprobanteDropDownList.SelectedValue);
                        if (comprobante.NaturalezaComprobante.Id != "Compra")
                        {
                            comprobante.NroPuntoVta = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
                        }
                        else
                        {
                            comprobante.NroPuntoVta = Convert.ToInt32(PuntoVtaTextBox.Text);
                            comprobante.Documento.Tipo.Id = "80"; 
                            comprobante.Documento.Nro = Cuit_VendedorTextBox.Text;
                        }
                        comprobante.Nro = Math.Abs(Convert.ToInt64(Numero_ComprobanteTextBox.Text));
                        RN.Comprobante.Leer(comprobante, sesion);
                        if (comprobante.Estado == "DeBaja")
                        {
                            RN.Comprobante.DarDeBajaFisica(comprobante, sesion);
                            mensaje = "Baja Fisica de " + ((IdNaturalezaComprobanteTextBox.Text == "VentaContrato") ? "Contrato" : "Comprobante") + " registrada satisfactoriamente";
                        }
                        else
                        {
                            mensaje = "No es posible realizar la Baja Fisica, ya que el " + ((IdNaturalezaComprobanteTextBox.Text == "VentaContrato") ? "Contrato" : "Comprobante") + " se encuentra en estado: " + comprobante.Estado;
                        }
                        AccionesPanel.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        mensaje = "Problemas en la baja/anul.baja del " + ((IdNaturalezaComprobanteTextBox.Text == "VentaContrato") ? "Contrato" : "Comprobante") + ".  " + ex.Message;
                    }
                    finally
                    {
                        ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript(mensaje));
                    }
                }
            }
        }
        
        private void ActualizarTipoDeCambio()
        {
            if (!MonedaComprobanteDropDownList.SelectedValue.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
            {
                Tipo_de_cambioLabel.Visible = true;
                Tipo_de_cambioTextBox.Visible = true;
            }
            else
            {
                Tipo_de_cambioLabel.Visible = false;
                Tipo_de_cambioTextBox.Visible = false;
                Tipo_de_cambioTextBox.Text = null;
            }
        }
        protected void tipoCambioUpdatePanel_Load(object sender, EventArgs e)
        {
            ActualizarTipoDeCambio();
        }
        protected void Version1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            AjustarPrestaServxVersiones();
        }
        private void AjustarPrestaServxVersiones()
        {
            Presta_ServCheckBox.Visible = false;
            Presta_ServLabel.Visible = false;
            CodigoConceptoLabel.Visible = true;
            CodigoConceptoDropDownList.Visible = true;
        }
        private void AjustarCamposXVersion(org.dyndns.cedweb.consulta.ConsultarResult lc)
        {
        }
        private void AjustarCamposXVersion(FeaEntidades.InterFacturas.lote_comprobantes lc)
        {
            Version1RadioButton.Checked = true;
            AjustarPrestaServxVersiones();
        }
        protected void PaisDestinoExpDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Collections.Generic.List<Entidades.Persona> listacompradores;
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
            if (PaisDestinoExpDropDownList.SelectedItem.Text.ToUpper().Contains("ARGENTINA"))
            {
                listacompradores = RN.Persona.ListaSinExportacion(((Entidades.Sesion)Session["Sesion"]).Usuario, ((Entidades.Sesion)Session["Sesion"]), true);
                Nro_Doc_Identificatorio_CompradorTextBox.Visible = true;
                Nro_Doc_Identificatorio_CompradorDropDownList.Visible = false;
                Nro_Doc_Identificatorio_CompradorTextBox.Text = string.Empty;
                Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.ListaNoExportacion();
            }
            else if (PaisDestinoExpDropDownList.SelectedItem.Text.Equals(string.Empty))
            {
                try
                {
                    int auxPV = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;
                    Nro_Doc_Identificatorio_CompradorTextBox.Visible = false;
                    Nro_Doc_Identificatorio_CompradorDropDownList.Visible = true;
                    Nro_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
                    Nro_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
                    Nro_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.DestinosCuit.DestinoCuit.ListaSinInformar();
                    Nro_Doc_Identificatorio_CompradorDropDownList.DataBind();
                    Nro_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = -1;
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.ListaExportacion();
                }
                catch
                {
                    Nro_Doc_Identificatorio_CompradorTextBox.Visible = true;
                    Nro_Doc_Identificatorio_CompradorDropDownList.Visible = false;
                    Nro_Doc_Identificatorio_CompradorTextBox.Text = string.Empty;
                    Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.Lista();
                }
            }
            else
            {
                Nro_Doc_Identificatorio_CompradorTextBox.Visible = false;
                Nro_Doc_Identificatorio_CompradorDropDownList.Visible = true;
                Nro_Doc_Identificatorio_CompradorDropDownList.DataValueField = "Codigo";
                Nro_Doc_Identificatorio_CompradorDropDownList.DataTextField = "Descr";
                Nro_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.DestinosCuit.DestinoCuit.ListaSinInformar();
                Nro_Doc_Identificatorio_CompradorDropDownList.DataBind();
                Nro_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = -1;
                Codigo_Doc_Identificatorio_CompradorDropDownList.DataSource = FeaEntidades.Documentos.Documento.ListaExportacion();
            }
            Codigo_Doc_Identificatorio_CompradorDropDownList.DataBind();
            Codigo_Doc_Identificatorio_CompradorDropDownList.SelectedIndex = -1;
            ResetearComprador();
        }
        public DetalleConsulta Articulos
        {
            get
            {
                return this.DetalleLinea;
            }
        }
        protected void SalirButton_Click(object sender, EventArgs e)
        {

        }
        protected void PuntoVtaDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AjustarCamposXPtaVentaChanged(((DropDownList)sender).SelectedValue);
                int auxPV = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                if (ViewState["PuntoVenta"] != null)
                {
                    int auxViewState = Convert.ToInt32(ViewState["PuntoVenta"]);
                    try
                    {
                        string idtipoAnterior = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                        {
                            return pv.Nro == auxViewState;
                        }).IdTipoPuntoVta;
                        string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                        {
                            return pv.Nro == auxPV;
                        }).IdTipoPuntoVta;
                        if (!idtipo.Equals(idtipoAnterior))
                        {
                            ResetearGrillas();
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        ResetearGrillas();
                    }
                }
                ViewState["PuntoVenta"] = auxPV;
                DetalleLinea.PuntoDeVenta = Convert.ToString(auxPV);
            }
            catch
            {
                ResetearGrillas();
            }
            //Al cambiar el punto de venta se inicializa el Nro.Lote
            Id_LoteTextbox.Text = "";
        }
        protected void Tipo_De_ComprobanteDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int auxPV = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
                string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                {
                    return pv.Nro == auxPV;
                }).IdTipoPuntoVta;
                if (idtipo.Equals("RG2904"))
                {
                    AjustarCodigoOperacionEn2904(((DropDownList)sender).SelectedValue);
                }
            }
            catch
            {
            }
        }
        protected void AccionSubirAAFIPButton_Click(object sender, EventArgs e)
        {
            if (Funciones.SessionTimeOut(Session))
            {
                Response.Redirect("~/SessionTimeout.aspx");
            }
            else
            {
                ActualizarEstadoPanel.Visible = false;
                DescargarPDFPanel.Visible = false;
                if (((Entidades.Sesion)Session["Sesion"]).Usuario.Id == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Su sesión ha caducado por inactividad. Por favor vuelva a loguearse."), false);
                }
                else
                {
                    try
                    {
                        try
                        {
                            int auxPV;
                            auxPV = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
                            string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                            {
                                return pv.Nro == auxPV;
                            }).IdTipoPuntoVta;
                            if (idtipo != "Comun")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Esta opción solo está habilitada para puntos de venta Comun RG.2485."), false);
                                return;
                            }
                            string respuesta = "";
                            FeaEntidades.InterFacturas.lote_comprobantes lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                            byte[] bytes = new byte[((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante.Request.Length * sizeof(char)];
                            System.Buffer.BlockCopy(((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante.Request.ToCharArray(), 0, bytes, 0, bytes.Length);
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                            ms.Seek(0, System.IO.SeekOrigin.Begin);
                            lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);

                            //Grabar en base de datos
                            lote.cabecera_lote.DestinoComprobante = "AFIP";
                            lote.comprobante[0].cabecera.informacion_comprobante.Observacion = "";
                            RN.Comprobante.Registrar(lote, null, IdNaturalezaComprobanteTextBox.Text, "AFIP", "PteEnvio", PeriodicidadEmisionDropDownList.SelectedValue, DateTime.ParseExact(FechaProximaEmisionDatePickerWebUserControl.Text, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), Convert.ToInt32(CantidadComprobantesAEmitirTextBox.Text), Convert.ToInt32(CantidadComprobantesEmitidosTextBox.Text), Convert.ToInt32(CantidadDiasFechaVtoTextBox.Text), string.Empty, false, string.Empty, string.Empty, string.Empty, ((Entidades.Sesion)Session["Sesion"]));

                            string caeNro = "";
                            string caeFecVto = "";
                            respuesta = RN.ComprobanteAFIP.EnviarAFIP(out caeNro, out caeFecVto, lote, (Entidades.Sesion)Session["Sesion"]);

                            RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), respuesta);
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript(respuesta), false);

                            if (respuesta.Length >= 12 && respuesta.Substring(0, 12) == "Resultado: A")
                            {
                                AjustarLoteParaITF(lote);

                                //Actualizar estado on-line.
                                if (caeNro != "")
                                {
                                    lote.cabecera_lote.resultado = "A";
                                    lote.comprobante[0].cabecera.informacion_comprobante.resultado = "A";
                                    lote.comprobante[0].cabecera.informacion_comprobante.cae = caeNro;
                                    lote.comprobante[0].cabecera.informacion_comprobante.caeSpecified = true;
                                    lote.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento_cae = caeFecVto;
                                    lote.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento_caeSpecified = true;
                                    lote.comprobante[0].cabecera.informacion_comprobante.fecha_obtencion_cae = DateTime.Now.ToString("yyyyMMdd");
                                    lote.comprobante[0].cabecera.informacion_comprobante.fecha_obtencion_caeSpecified = true;
                                }
                                string XML = "";
                                RN.Comprobante.SerializarLc(out XML, lote);
                                Entidades.Comprobante comprobante = new Entidades.Comprobante();
                                comprobante.Cuit = lote.comprobante[0].cabecera.informacion_vendedor.cuit.ToString();
                                comprobante.TipoComprobante.Id = lote.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante;
                                comprobante.NroPuntoVta = lote.comprobante[0].cabecera.informacion_comprobante.punto_de_venta;
                                comprobante.Nro = lote.comprobante[0].cabecera.informacion_comprobante.numero_comprobante;
                                comprobante.NaturalezaComprobante.Id = "Venta";
                                RN.Comprobante.Leer(comprobante, ((Entidades.Sesion)Session["Sesion"]));
                                comprobante.Response = XML;
                                comprobante.WF.Estado = "Vigente";
                                RN.Comprobante.Actualizar(comprobante, (Entidades.Sesion)Session["Sesion"]);

                                RN.Comprobante.Leer(comprobante, ((Entidades.Sesion)Session["Sesion"]));
                                DescargarPDFPanel.Visible = true;
                            }
                        }
                        catch (System.Web.Services.Protocols.SoapException soapEx)
                        {
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(soapEx.Detail.OuterXml);
                                XmlNamespaceManager nsManager = new
                                    XmlNamespaceManager(doc.NameTable);
                                nsManager.AddNamespace("errorNS",
                                    "http://www.cedeira.com.ar/webservices");
                                XmlNode Node =
                                    doc.DocumentElement.SelectSingleNode("errorNS:Error", nsManager);
                                string errorNumber =
                                    Node.SelectSingleNode("errorNS:ErrorNumber",
                                    nsManager).InnerText;
                                string errorMessage =
                                    Node.SelectSingleNode("errorNS:ErrorMessage",
                                    nsManager).InnerText;
                                string errorSource =
                                    Node.SelectSingleNode("errorNS:ErrorSource",
                                    nsManager).InnerText;
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript(soapEx.Actor.Trim() + ": " + errorMessage), false);
                            }
                            catch (Exception)
                            {
                                throw soapEx;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), ex.Message);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Problemas al enviar el comprobante a AFIP.  " + ex.Message), false);
                    }
                }
            }
        }
        protected void AccionValidarEnInterfacturasButton_Click(object sender, EventArgs e)
        {
            if (Funciones.SessionTimeOut(Session))
            {
                Response.Redirect("~/SessionTimeout.aspx");
            }
            else
            {
                ActualizarEstadoPanel.Visible = false;
                DescargarPDFPanel.Visible = false;
                //ActualizarEstadoButton.DataBind();
                //DescargarPDFButton.DataBind();
                if (((Entidades.Sesion)Session["Sesion"]).Usuario.Id == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Su sesión ha caducado por inactividad. Por favor vuelva a loguearse."), false);
                }
                else
                {
                    try
                    {
                        string NroCertif = ((Entidades.Sesion)Session["Sesion"]).Cuit.NroSerieCertifITF;
                        if (NroCertif.Equals(string.Empty))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Aún no disponemos de su certificado digital."), false);
                            return;
                        }
                        try
                        {
                            string certificado = "";
                            string respuesta = "";

                            certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(NroCertif, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                            org.dyndns.cedweb.valido.ValidoIBK edyndns = new org.dyndns.cedweb.valido.ValidoIBK();
                            string ValidarIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["ValidarIBKUtilizarServidorExterno"];
                            if (ValidarIBKUtilizarServidorExterno == "SI")
                            {
                                edyndns.Url = System.Configuration.ConfigurationManager.AppSettings["ValidarIBKurl"];
                            }
                            FeaEntidades.InterFacturas.lote_comprobantes lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                            byte[] bytes = new byte[((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante.Request.Length * sizeof(char)];
                            System.Buffer.BlockCopy(((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante.Request.ToCharArray(), 0, bytes, 0, bytes.Length);
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                            ms.Seek(0, System.IO.SeekOrigin.Begin);
                            lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);

                            AjustarLoteParaITF(lote);

                            string xmlTexto = "";
                            RN.Comprobante.SerializarLc(out xmlTexto, lote);

                            respuesta = edyndns.ValidarIBK(xmlTexto, certificado);

                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript(respuesta), false);
                        }
                        catch (System.Web.Services.Protocols.SoapException soapEx)
                        {
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(soapEx.Detail.OuterXml);
                                XmlNamespaceManager nsManager = new
                                    XmlNamespaceManager(doc.NameTable);
                                nsManager.AddNamespace("errorNS",
                                    "http://www.cedeira.com.ar/webservices");
                                XmlNode Node =
                                    doc.DocumentElement.SelectSingleNode("errorNS:Error", nsManager);
                                string errorNumber =
                                    Node.SelectSingleNode("errorNS:ErrorNumber",
                                    nsManager).InnerText;
                                string errorMessage =
                                    Node.SelectSingleNode("errorNS:ErrorMessage",
                                    nsManager).InnerText;
                                string errorSource =
                                    Node.SelectSingleNode("errorNS:ErrorSource",
                                    nsManager).InnerText;
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript(soapEx.Actor.Trim() + ": " + errorMessage), false);
                            }
                            catch (Exception)
                            {
                                throw soapEx;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Problemas al enviar el comprobante a Interfacturas.  " + ex.Message), false);
                    }
                }
            }
        }
        protected void AccionSubirAInterfacturasButton_Click(object sender, EventArgs e)
        {
            if (Funciones.SessionTimeOut(Session))
            {
                Response.Redirect("~/SessionTimeout.aspx");
            }
            else
            {
                Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];
                ActualizarEstadoPanel.Visible = false;
                DescargarPDFPanel.Visible = false;
                if (sesion.Usuario.Id == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Su sesión ha caducado por inactividad. Por favor vuelva a loguearse."), false);
                }
                else
                {
                    try
                    {
                        string NroCertif = "";
                        NroCertif = sesion.Cuit.NroSerieCertifITF;
                        if (NroCertif.Equals(string.Empty))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Aún no disponemos de su certificado digital."), false);
                            return;
                        }
                        try
                        {
                            string certificado = "";
                            string respuesta = "";
                            certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(NroCertif, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                            org.dyndns.cedweb.envio.EnvioIBK edyndns = new org.dyndns.cedweb.envio.EnvioIBK();
                            string EnvioIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["EnvioIBKUtilizarServidorExterno"];
                            if (EnvioIBKUtilizarServidorExterno == "SI")
                            {
                                edyndns.Url = System.Configuration.ConfigurationManager.AppSettings["EnvioIBKurl"];
                            }
                            org.dyndns.cedweb.envio.lc lcIBK = new org.dyndns.cedweb.envio.lc();

                            FeaEntidades.InterFacturas.lote_comprobantes lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                            byte[] bytes = new byte[((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante.Request.Length * sizeof(char)];
                            System.Buffer.BlockCopy(((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante.Request.ToCharArray(), 0, bytes, 0, bytes.Length);
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                            ms.Seek(0, System.IO.SeekOrigin.Begin);
                            lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);

                            //Grabar en base de datos
                            lote.cabecera_lote.DestinoComprobante = "ITF";
                            lote.comprobante[0].cabecera.informacion_comprobante.Observacion = "";

                            AjustarLoteParaITF(lote);

                            lcIBK = Conversor.Entidad2IBK(lote);

                            respuesta = edyndns.EnviarIBK(lcIBK, certificado);
                            respuesta = respuesta.Replace("'", "-");

                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript(respuesta), false);

                            if (respuesta == "Comprobante enviado satisfactoriamente a Interfacturas.")
                            {
                                Entidades.Comprobante comprobante = ((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante;
                                RN.Comprobante.Leer(comprobante, sesion);
                                comprobante.WF.Estado = "PteConf";
                                RN.Comprobante.Actualizar(comprobante, sesion);
                                RN.Comprobante.Leer(comprobante, sesion);
                                //Consultar y Actualizar estado on-line.                              
                                org.dyndns.cedweb.consulta.ConsultaIBK clcdyndnsConsultaIBK = new org.dyndns.cedweb.consulta.ConsultaIBK();
                                string ConsultaIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKUtilizarServidorExterno"];
                                RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKUtilizarServidorExterno: " + ConsultaIBKUtilizarServidorExterno);
                                if (ConsultaIBKUtilizarServidorExterno == "SI")
                                {
                                    clcdyndnsConsultaIBK.Url = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"];
                                    RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"]);
                                }
                                System.Threading.Thread.Sleep(2000);
                                org.dyndns.cedweb.consulta.ConsultarResult clcrdyndns = new org.dyndns.cedweb.consulta.ConsultarResult();
                                clcrdyndns = clcdyndnsConsultaIBK.Consultar(Convert.ToInt64(lote.comprobante[0].cabecera.informacion_vendedor.cuit), lote.cabecera_lote.id_lote, lote.comprobante[0].cabecera.informacion_comprobante.punto_de_venta, certificado);
                                FeaEntidades.InterFacturas.lote_comprobantes lc = new FeaEntidades.InterFacturas.lote_comprobantes();
                                lc = Funciones.Ws2Fea(clcrdyndns);
                                string XML = "";
                                RN.Comprobante.SerializarLc(out XML, lc);
                                comprobante.Response = XML;
                                if (lc.cabecera_lote.resultado == "A")
                                {
                                    comprobante.WF.Estado = "Vigente";
                                    RN.Comprobante.Actualizar(comprobante, sesion);
                                    RN.Comprobante.Leer(comprobante, sesion);
                                    DescargarPDFPanel.Visible = true;
                                }
                                else if (lc.cabecera_lote.resultado == "R")
                                {
                                    comprobante.WF.Estado = "Rechazado";
                                    RN.Comprobante.Actualizar(comprobante, sesion);
                                    RN.Comprobante.Leer(comprobante, sesion);
                                    ActualizarEstadoPanel.Visible = false;
                                    DescargarPDFPanel.Visible = true;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Problemas al enviar el comprobante a Interfacturas. " + Funciones.TextoScript(respuesta)), false);
                            }
                        }
                        catch (System.Web.Services.Protocols.SoapException soapEx)
                        {
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(soapEx.Detail.OuterXml);
                                XmlNamespaceManager nsManager = new
                                    XmlNamespaceManager(doc.NameTable);
                                nsManager.AddNamespace("errorNS",
                                    "http://www.cedeira.com.ar/webservices");
                                XmlNode Node =
                                    doc.DocumentElement.SelectSingleNode("errorNS:Error", nsManager);
                                string errorNumber =
                                    Node.SelectSingleNode("errorNS:ErrorNumber",
                                    nsManager).InnerText;
                                string errorMessage =
                                    Node.SelectSingleNode("errorNS:ErrorMessage",
                                    nsManager).InnerText;
                                string errorSource =
                                    Node.SelectSingleNode("errorNS:ErrorSource",
                                    nsManager).InnerText;
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript(soapEx.Actor.Trim() + ": " + errorMessage), false);
                            }
                            catch (Exception)
                            {
                                throw soapEx;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Problemas al enviar el comprobante a Interfacturas.  " + ex.Message), false);
                    }
                }
            }
        }
        protected void AccionActualizarEstadoButton_Click(object sender, EventArgs e)
        {
            string NroCertif = "";
            NroCertif = ((Entidades.Sesion)Session["Sesion"]).Cuit.NroSerieCertifITF;
            if (NroCertif.Equals(string.Empty))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Aún no disponemos de su certificado digital."), false);
                return;
            }
            try
            {
                Entidades.Comprobante comprobante = ((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante;
                if (comprobante != null)
                {
                    string certificado = "";
                    certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(NroCertif, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();

                    //Consultar y Actualizar estado on-line.                              
                    org.dyndns.cedweb.consulta.ConsultaIBK clcdyndnsConsultaIBK = new org.dyndns.cedweb.consulta.ConsultaIBK();
                    string ConsultaIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKUtilizarServidorExterno"];
                    RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKUtilizarServidorExterno: " + ConsultaIBKUtilizarServidorExterno);
                    if (ConsultaIBKUtilizarServidorExterno == "SI")
                    {
                        clcdyndnsConsultaIBK.Url = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"];
                        RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"]);
                    }
                    System.Threading.Thread.Sleep(2000);
                    org.dyndns.cedweb.consulta.ConsultarResult clcrdyndns = new org.dyndns.cedweb.consulta.ConsultarResult();
                    clcrdyndns = clcdyndnsConsultaIBK.Consultar(Convert.ToInt64(comprobante.Cuit), comprobante.NroLote, comprobante.NroPuntoVta, certificado);
                    FeaEntidades.InterFacturas.lote_comprobantes lc = new FeaEntidades.InterFacturas.lote_comprobantes();
                    lc = Funciones.Ws2Fea(clcrdyndns);
                    string XML = "";
                    RN.Comprobante.SerializarLc(out XML, lc);
                    //c.Leer(comprobante, ((Entidades.Sesion)Session["Sesion"]));
                    comprobante.Response = XML;
                    if (lc.cabecera_lote.resultado == "A")
                    {
                        comprobante.WF.Estado = "Vigente";
                        RN.Comprobante.Actualizar(comprobante, (Entidades.Sesion)Session["Sesion"]);
                        RN.Comprobante.Leer(comprobante, ((Entidades.Sesion)Session["Sesion"]));
                        ActualizarEstadoPanel.Visible = false;
                        DescargarPDFPanel.Visible = true;
                    }
                    else if (lc.cabecera_lote.resultado == "R")
                    {
                        comprobante.WF.Estado = "Rechazado";
                        RN.Comprobante.Actualizar(comprobante, (Entidades.Sesion)Session["Sesion"]);
                        RN.Comprobante.Leer(comprobante, ((Entidades.Sesion)Session["Sesion"]));
                        ActualizarEstadoPanel.Visible = false;
                        DescargarPDFPanel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Problemas al enviar el comprobante a Interfacturas.  " + ex.Message), false);
            }
        }
        protected void AccionDescargarPDFButton_Click(object sender, EventArgs e)
        {
            if (Funciones.SessionTimeOut(Session))
            {
                Response.Redirect("~/SessionTimeout.aspx");
            }
            else
            {
                if (((Entidades.Sesion)Session["Sesion"]).Usuario.Id == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Su sesión ha caducado por inactividad. Por favor vuelva a loguearse."), false);
                }
                else
                {
                    try
                    {
                        FeaEntidades.InterFacturas.lote_comprobantes lote;
                        string certificado;
                        Entidades.Comprobante comprobante = ((Entidades.ComprobanteATratar)ViewState["ComprobanteATratar"]).Comprobante;
                        Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];
                        string DetalleIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["DetalleIBKUtilizarServidorExterno"];
                        org.dyndns.cedweb.detalle.DetalleIBK clcdyndns = new org.dyndns.cedweb.detalle.DetalleIBK();
                        org.dyndns.cedweb.detalle.cecd cecd = new org.dyndns.cedweb.detalle.cecd();
                        System.Xml.Serialization.XmlSerializer x;
                        byte[] bytes;
                        System.IO.MemoryStream ms;
                        string resp;
                        string script;

                        if (comprobante.IdDestinoComprobante == "ITF")
                        {
                            #region PDF (InterFacturas)
                            //OBTENCIÓN DE PDF DE INTERFACTURAS
                            if (comprobante.Estado != "Vigente")
                            {
                                ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript("El comprobante no está vigente.")); 
                                return;
                            }

                            //On-Line
                            cecd.cuit_canal = Convert.ToInt64("30690783521");
                            cecd.cuit_vendedor = Convert.ToInt64(comprobante.Cuit);
                            cecd.punto_de_venta = Convert.ToInt32(comprobante.NroPuntoVta);
                            cecd.tipo_de_comprobante = Convert.ToInt32(comprobante.TipoComprobante.Id);
                            cecd.numero_comprobante = comprobante.Nro;
                            cecd.id_Lote = 0;
                            cecd.id_LoteSpecified = false;
                            cecd.estado = "PR";

                            if (sesion.Cuit.NroSerieCertifITF.Equals(string.Empty))
                            {
                                ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript("Aún no disponemos de su certificado digital"));
                                return;
                            }
                            Funciones.GrabarLogTexto("~/Detallar.txt", "Consulta de Lote CUIT: " + sesion.Cuit.Nro + "  Punto de Venta: " + cecd.punto_de_venta.ToString() + "  Tipo de Comprobante: " + cecd.tipo_de_comprobante.ToString() + "  Nro de Comprobante: " + cecd.numero_comprobante.ToString());
                            Funciones.GrabarLogTexto("~/Detallar.txt", "NroSerieCertifITF: " + sesion.Cuit.NroSerieCertifITF);
                            if (sesion.Cuit.NroSerieCertifITF.Equals(string.Empty))
                            {
                                ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript("Aún no disponemos de su certificado digital"));
                                return;
                            }

                            certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(sesion.Cuit.NroSerieCertifITF, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                            Funciones.GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKUtilizarServidorExterno: " + DetalleIBKUtilizarServidorExterno);
                            if (DetalleIBKUtilizarServidorExterno == "SI")
                            {
                                clcdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"];
                                Funciones.GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"]);
                            }
                            resp = clcdyndns.DetallarIBK(cecd, certificado);
                            resp = resp.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                            resp = resp.Replace(" xmlns:xsi=\"http://lote.schemas.cfe.ib.com.ar/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", " xmlns=\"http://lote.schemas.cfe.ib.com.ar/\"");
                            //Fin On-Line

                            try
                            {
                                string comprobanteXML = resp;

                                Funciones.GrabarLogTexto("~/Detallar.txt", "Inicia ExecuteCommand");
                                org.dyndns.cedweb.generoPDF.GeneroPDF pdfdyndns = new org.dyndns.cedweb.generoPDF.GeneroPDF();

                                string GenerarPDFUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["GenerarPDFUtilizarServidorExterno"];
                                Funciones.GrabarLogTexto("~/Detallar.txt", "Parametro GenerarPDFUtilizarServidorExterno: " + GenerarPDFUtilizarServidorExterno);
                                if (GenerarPDFUtilizarServidorExterno == "SI")
                                {
                                    pdfdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["GenerarPDFurl"];
                                    Funciones.GrabarLogTexto("~/Detallar.txt", "Parametro GenerarPDFurl: " + System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"]);
                                }
                                string RespPDF = pdfdyndns.GenerarPDF(comprobante.Cuit, comprobante.NroPuntoVta, comprobante.TipoComprobante.Id, comprobante.Nro, comprobante.IdDestinoComprobante, comprobanteXML);
                                Funciones.GrabarLogTexto("~/Detallar.txt", "Finaliza ExecuteCommand");

                                //Crear nombre de archivo default sin extensión
                                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                sb.Append(comprobante.Cuit);
                                sb.Append("-");
                                sb.Append(comprobante.NroPuntoVta.ToString("0000"));
                                sb.Append("-");
                                sb.Append(comprobante.TipoComprobante.Id.ToString("00"));
                                sb.Append("-");
                                sb.Append(comprobante.Nro.ToString("00000000"));
                                sb.Append(".pdf");

                                string url = RespPDF;
                                string filename = sb.ToString();
                                String dlDir = @"~/TempRender/";
                                new System.Net.WebClient().DownloadFile(url, Server.MapPath(dlDir + filename));

                                script = "window.open('/DescargaTemporarios.aspx?archivo=" + sb.ToString() + "&path=" + @"~/TempRender/" + "', '');";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);
                            }
                            catch (Exception ex)
                            {
                                script = "Problemas para generar el PDF.\\n" + ex.Message;
                                script += ex.StackTrace;
                                if (ex.InnerException != null)
                                {
                                    script = ex.InnerException.Message;
                                }
                                RN.Sesion.GrabarLogTexto(Server.MapPath("~/Detallar.txt"), script);
                                ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript(script));
                            }
                            #endregion
                        }
                        else
                        {
                            #region PDF (AFIP)
                            //GENERACIÓN DE PDF A PARTIR DE DATOS LOCALES
                            lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                            x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                            if (comprobante.Estado != "Vigente")
                            {
                                ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript("El comprobante no está vigente."));
                                return;
                            }
                            try
                            {
                                comprobante.Response = comprobante.Response.Replace("iso-8859-1", "utf-16");
                                bytes = new byte[comprobante.Response.Length * sizeof(char)];
                                System.Buffer.BlockCopy(comprobante.Response.ToCharArray(), 0, bytes, 0, bytes.Length);
                                ms = new System.IO.MemoryStream(bytes);
                                ms.Seek(0, System.IO.SeekOrigin.Begin);
                                lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);

                                RN.Comprobante.AjustarLoteParaImprimirPDF(lote);

                                Session["lote"] = lote;
                                script = "window.open('/Facturacion/Electronica/Reportes/FacturaWebForm.aspx', '');";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);

                            }
                            catch (Exception ex)
                            {
                                script = "Problemas para generar el PDF.\\n" + ex.Message;
                                RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), script);
                                ClientScript.RegisterStartupScript(GetType(), "Message", Funciones.TextoScript(script));
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Message", Funciones.TextoScript("Problemas al descargar el archivo PDF.  " + ex.Message), false);
                    }
                }
            }
        }
        private void AjustarLoteParaITF(FeaEntidades.InterFacturas.lote_comprobantes lote)
        {
            string TipoCbte = lote.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante.ToString();
            switch (TipoCbte)
            {
                case "6":
                case "7":
                case "8":
                case "9":
                case "10":
                case "40":
                case "61":
                case "64":
                    FeaEntidades.InterFacturas.detalle det = new FeaEntidades.InterFacturas.detalle();
                    det = lote.comprobante[0].detalle;
                    FeaEntidades.InterFacturas.linea[] listadelineas;
                    listadelineas = det.linea;
                    Double TipoDeCambio = lote.comprobante[0].resumen.tipo_de_cambio;

                    int auxPV;
                    auxPV = Convert.ToInt32(PuntoVtaDropDownList.SelectedValue);
                    string idtipo = ((Entidades.Sesion)Session["Sesion"]).UN.PuntosVta.Find(delegate(Entidades.PuntoVta pv)
                    {
                        return pv.Nro == auxPV;
                    }).IdTipoPuntoVta;

                    for (int i = 0; i < listadelineas.Length; i++)
                    {
                        if (det.linea[i] == null)
                        {
                            break;
                        }

                        double precio_unitario = det.linea[i].precio_unitario;
                        double alicuota_iva = det.linea[i].alicuota_iva;
                        double importe_iva = det.linea[i].importe_iva;
                        double importe_total_articulo = det.linea[i].importe_total_articulo;

                        //Moneda Local
                        if (lote.comprobante[0].resumen.codigo_moneda.Equals(FeaEntidades.CodigosMoneda.CodigoMoneda.Local))
                        {
                            det.linea[i].precio_unitarioSpecified = listadelineas[i].precio_unitarioSpecified;
                            if (!listadelineas[i].alicuota_iva.Equals(new FeaEntidades.IVA.SinInformar().Codigo))
                            {
                                det.linea[i].precio_unitario = Math.Round(precio_unitario * (1 + alicuota_iva / 100), 3);
                            }
                            else
                            {
                                det.linea[i].precio_unitario = Math.Round(precio_unitario, 3);
                            }
                            if (idtipo.Equals("RG2904"))
                            {
                                det.linea[i].importe_total_articulo = Math.Round(importe_total_articulo, 2);
                            }
                            else
                            {
                                det.linea[i].importe_total_articulo = Math.Round(importe_total_articulo + importe_iva, 2);
                            }
                            //Borrar alicuota e importe
                            det.linea[i].alicuota_ivaSpecified = false;
                            det.linea[i].alicuota_iva = 0;
                            det.linea[i].indicacion_exento_gravado = null;
                            det.linea[i].importe_ivaSpecified = false;
                            det.linea[i].importe_iva = 0;
                        }
                        else
                        {
                            //Moneda Extranjera

                            det.linea[i].precio_unitarioSpecified = listadelineas[i].precio_unitarioSpecified;
                            if (!listadelineas[i].alicuota_iva.Equals(new FeaEntidades.IVA.SinInformar().Codigo))
                            {
                                det.linea[i].precio_unitario = Math.Round(precio_unitario * Convert.ToDouble(TipoDeCambio) * (1 + alicuota_iva / 100), 3);
                            }
                            else
                            {
                                det.linea[i].precio_unitario = Math.Round(precio_unitario * Convert.ToDouble(TipoDeCambio), 3);
                            }
                            det.linea[i].importe_total_articulo = Math.Round(((importe_total_articulo) + importe_iva) * Convert.ToDouble(TipoDeCambio), 2);
                            //Borrar alicuota e importe
                            det.linea[i].alicuota_ivaSpecified = false;
                            det.linea[i].alicuota_iva = 0;
                            det.linea[i].indicacion_exento_gravado = null;
                            det.linea[i].importe_ivaSpecified = false;
                            det.linea[i].importe_iva = 0;

                            //importes_moneda_origen
                            FeaEntidades.InterFacturas.lineaImportes_moneda_origen limo = new FeaEntidades.InterFacturas.lineaImportes_moneda_origen();
                            limo.importe_total_articuloSpecified = true;
                            limo.precio_unitarioSpecified = listadelineas[i].precio_unitarioSpecified;
                            if (!listadelineas[i].alicuota_iva.Equals(new FeaEntidades.IVA.SinInformar().Codigo))
                            {
                                limo.precio_unitario = Math.Round(precio_unitario * (1 + alicuota_iva / 100), 3);
                            }
                            else
                            {
                                limo.precio_unitario = Math.Round(precio_unitario, 3);
                            }
                            if (idtipo.Equals("RG2904"))
                            {
                                limo.importe_total_articulo = Math.Round(importe_total_articulo, 2);
                            }
                            else
                            {
                                limo.importe_total_articulo = Math.Round(importe_total_articulo + importe_iva, 2);
                            }
                            limo.importe_ivaSpecified = false;
                            limo.importe_iva = 0;
                            det.linea[i].importes_moneda_origen = limo;
                        }
                    }
                    break;
            }
        }
        protected void ModalPopupExtender1_Load(object sender, EventArgs e)
        {

        }
        protected void ModalPopupExtender3_Load(object sender, EventArgs e)
        {

        }
    }
}