﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Xml;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;

namespace CedServicios.Site
{
    public partial class ExploradorComprobante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Funciones.SessionTimeOut(Session))
                {
                    Response.Redirect("~/SessionTimeout.aspx");
                }
                else
                {
                    Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];
                    if (sesion.UsuarioDemo == true)
                    {
                        FechaDesdeTextBox.Text = "20130101";
                        FechaHastaTextBox.Text = DateTime.Today.ToString("yyyyMMdd");
                    }
                    else
                    {
                        FechaDesdeTextBox.Text = DateTime.Today.ToString("yyyyMMdd");
                        FechaHastaTextBox.Text = DateTime.Today.ToString("yyyyMMdd");
                    }
                    ViewState["Clientes"] = RN.Cliente.ListaPorCuit(false, true, sesion);
                    ClienteDropDownList.DataSource = (List<Entidades.Cliente>)ViewState["Clientes"];
                    DataBind();
                    if (ClienteDropDownList.Items.Count > 0)
                    {
                        ClienteDropDownList.SelectedValue = "0";
                    }
                }
            }
        }
        protected void ComprobantesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MensajeLabel.Text = "";
            int item = Convert.ToInt32(e.CommandArgument);
            List<Entidades.Comprobante> lista = (List<Entidades.Comprobante>)ViewState["Comprobantes"];
            Entidades.Comprobante comprobante = lista[item];
            string script;

            switch (e.CommandName)
            {
                case "ConsultaLocal":
                    #region ConsultaLocal
                    FeaEntidades.InterFacturas.lote_comprobantes lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                    byte[] bytes;
                    System.IO.MemoryStream ms;
                    try
                    {
                        comprobante.Response = comprobante.Response.Replace("iso-8859-1", "utf-16");
                        bytes = new byte[comprobante.Response.Length * sizeof(char)];
                        System.Buffer.BlockCopy(comprobante.Response.ToCharArray(), 0, bytes, 0, bytes.Length);
                        ms = new System.IO.MemoryStream(bytes);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);
                    }
                    catch 
                    {
                        bytes = new byte[comprobante.Request.Length * sizeof(char)];
                        System.Buffer.BlockCopy(comprobante.Request.ToCharArray(), 0, bytes, 0, bytes.Length);
                        ms = new System.IO.MemoryStream(bytes);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);
                    }
                    Cache["ComprobanteAConsultar"] = lote;
                    script = "window.open('/ComprobanteConsulta.aspx', '');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);
                    #endregion
                    break;
                case "ConsultarOnLine":
                    #region ConsultarOnLine
                    try
                    {
                        string NroCertif = ((Entidades.Sesion)Session["Sesion"]).Cuit.NroSerieCertifITF;
                        if (NroCertif.Equals(string.Empty))
                        {
                            MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                            return;
                        }
                        RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Consulta de Lote CUIT: " + comprobante.Cuit + "  Nro.Lote: " + comprobante.NroLote + "  Nro. Punto de Vta.: " + comprobante.NroPuntoVta);
                        RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "NroSerieCertifITF: " + NroCertif);
                        if (NroCertif.Equals(string.Empty))
                        {
                            MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                            return;
                        }
                        string certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(NroCertif, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                        org.dyndns.cedweb.consulta.ConsultaIBK clcdyndns = new org.dyndns.cedweb.consulta.ConsultaIBK();
                        string ConsultaIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKUtilizarServidorExterno"];
                        RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKUtilizarServidorExterno: " + ConsultaIBKUtilizarServidorExterno);
                        if (ConsultaIBKUtilizarServidorExterno == "SI")
                        {
                            clcdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"];
                            RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"]);
                        }
                        org.dyndns.cedweb.consulta.ConsultarResult clcrdyndns = new org.dyndns.cedweb.consulta.ConsultarResult();
                        clcrdyndns = clcdyndns.Consultar(Convert.ToInt64(comprobante.Cuit), comprobante.NroLote, comprobante.NroPuntoVta, certificado);
                        //Cache["ComprobanteAConsultar"] = clcrdyndns;
                        FeaEntidades.InterFacturas.lote_comprobantes lc = new FeaEntidades.InterFacturas.lote_comprobantes();
                        lc = Funciones.Ws2Fea(clcrdyndns);
                        Cache["ComprobanteAConsultar"] = lc;
                        //Controlar que sea el mismo comprobante (local vs on-line)
                        if (comprobante.Nro != lc.comprobante[0].cabecera.informacion_comprobante.numero_comprobante)
                        {
                            MensajeLabel.Text = "(Campo: Nro. de Comprobante). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. No se puede actualizar el estado.";
                            return;
                        }
                        if (comprobante.TipoComprobante.Id != lc.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante)
                        {
                            MensajeLabel.Text = "(Campo: Tipo de Comprobante). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. No se puede actualizar el estado.";
                            return;
                        }
                        script = "window.open('/ComprobanteConsulta.aspx', '');";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);
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
                            MensajeLabel.Text = soapEx.Actor + " : " + errorMessage.Replace("\r", "").Replace("\n", "");
                        }
                        catch (Exception)
                        {
                            throw soapEx;
                        }
                    }
                    #endregion
                    break;
            }
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
        public void ExecuteCommand(string NombreArchivosbXML, string NombreArchivosbPDF)
        {
            int exitcode;
            //ProcessStartInfo ProcessInfo;
            //Process process;
            //RN.Sesion.GrabarLogTexto(Server.MapPath("~/Detallar.txt"), "java.exe");
            ////ProcessInfo = new ProcessStartInfo(@"C:\SVNCedServicios\CedServicios\CedServiciosSite\TempRender\GenerarPDF.bat");
            //ProcessInfo = new ProcessStartInfo(@"C:\inetpub\wwwroot\CedeiraServicios\TempRender\GenerarPDF.bat");
            ////ProcessInfo = new ProcessStartInfo("java.exe");
            ////ProcessInfo.Arguments = @"-cp " + Server.MapPath("~/TempRender/cfe-factura-render-2.57-ejecutable.jar") + " ar.com.ib.cfe.render.GenerarPDF " + Server.MapPath("~/TempRender/" + NombreArchivosbXML) + " " + Server.MapPath("~/TempRender/" + NombreArchivosbPDF) + " ORIGINAL";
            //RN.Sesion.GrabarLogTexto(Server.MapPath("~/Detallar.txt"), "Argumentos: " + "-cp " + Server.MapPath("~/TempRender/cfe-factura-render-2.57-ejecutable.jar") + " ar.com.ib.cfe.render.GenerarPDF " + Server.MapPath("~/TempRender/" + NombreArchivosbXML) + " " + Server.MapPath("~/TempRender/" + NombreArchivosbPDF) + " ORIGINAL");
            //ProcessInfo.WorkingDirectory = @"C:\inetpub\wwwroot\CedeiraServicios\TempRender\";
            //ProcessInfo.CreateNoWindow = false;
            //ProcessInfo.UseShellExecute = false;
            //// redirecting standard output and error
            //ProcessInfo.RedirectStandardError = true;
            //ProcessInfo.RedirectStandardOutput = true;
            //process = Process.Start(ProcessInfo);
            //process.WaitForExit();

            string command = string.Format("GenerarPDF.bat {0} {1} {2}", Server.MapPath("~/TempRender/" + "cfe-factura-render-2.57-ejecutable.jar"), Server.MapPath("~/TempRender/" + NombreArchivosbXML), Server.MapPath("~/TempRender/" + NombreArchivosbPDF));
            //string command = "GenerarPDF.bat";
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            //procStartInfo.WorkingDirectory = @"C:\inetpub\wwwroot\CedeiraServicios\TempRender\";
            procStartInfo.WorkingDirectory = Server.MapPath("~/TempRender/");
            procStartInfo.RedirectStandardError = true;
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            Process process = new Process();
            process.StartInfo = procStartInfo;
            process.Start();
            process.WaitForExit();

            //Reading output and error
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitcode = process.ExitCode;
            MensajeLabel.Text = "";
            //Exit code '0' denotes success and '1' denotes failure
            if (exitcode != 0)
            {
                MensajeLabel.Text += "Exit Code:" + exitcode + "  ";
            }
            if (error != "")
            {
                MensajeLabel.Text += "Error:" + error + "  ";
            }
            if (output != "")
            {
                MensajeLabel.Text += output + " ";
            }
            RN.Sesion.GrabarLogTexto(Server.MapPath("~/Detallar.txt"), "Process Info: " + MensajeLabel.Text);
            process.Close();
        }
        public string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        protected void ComprobantesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[12].Text != "Vigente")
                {
                    e.Row.ForeColor = Color.Red;
                }
            }
        }
        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            if (Funciones.SessionTimeOut(Session))
            {
                Response.Redirect("~/SessionTimeout.aspx");
            }
            else
            {
                Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];
                List<Entidades.Comprobante> lista = new List<Entidades.Comprobante>();
                MensajeLabel.Text = String.Empty;
                Entidades.Cliente cliente;
                if (ClienteDropDownList.SelectedIndex >= 0)
                {
                    cliente = ((List<Entidades.Cliente>)ViewState["Clientes"])[ClienteDropDownList.SelectedIndex];
                }
                else
                {
                    cliente = new Entidades.Cliente();
                }
                lista = RN.Comprobante.ListaFiltrada(SoloVigentesCheckBox.Checked, FechaDesdeTextBox.Text, FechaHastaTextBox.Text, cliente, sesion);
                if (lista.Count == 0)
                {
                    ComprobantesGridView.DataSource = null;
                    ComprobantesGridView.DataBind();
                    MensajeLabel.Text = "No se han encontrado Comprobantes que satisfagan la busqueda";
                }
                else
                {
                    ComprobantesGridView.DataSource = lista;
                    ViewState["Comprobantes"] = lista;
                    ComprobantesGridView.DataBind();
                }
            }
        }
        protected void SalirButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(((Entidades.Sesion)Session["Sesion"]).Usuario.PaginaDefault((Entidades.Sesion)Session["Sesion"]));
        }
        protected void AccionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int item = row.RowIndex;
            List<Entidades.Comprobante> lista = (List<Entidades.Comprobante>)ViewState["Comprobantes"];
            Entidades.Comprobante comprobante = lista[item];
            string comando = ddl.SelectedValue;
            //MensajeLabel.Text = comando + " " + comprobante.NroFORMATEADO;
            ddl.SelectedIndex = -1;
            MensajeLabel.Text = "";
            FeaEntidades.InterFacturas.lote_comprobantes lote;
            Entidades.Sesion sesion = (Entidades.Sesion)Session["Sesion"];
            string certificado;
            string DetalleIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["DetalleIBKUtilizarServidorExterno"];
            org.dyndns.cedweb.detalle.DetalleIBK clcdyndns = new org.dyndns.cedweb.detalle.DetalleIBK();
            org.dyndns.cedweb.detalle.cecd cecd = new org.dyndns.cedweb.detalle.cecd();
            List<FeaEntidades.InterFacturas.Listado.emisor_comprobante_listado> listaR = new List<FeaEntidades.InterFacturas.Listado.emisor_comprobante_listado>();
            System.Xml.Serialization.XmlSerializer x;
            byte[] bytes;
            System.IO.MemoryStream ms;
            string resp;
            string script;

            switch (comando)
            {
                case "ActualizarOnLine":
                    #region ActualizarOnLine
                    try
                    {
                        if (comprobante.Estado == "Vigente")
                        {
                            MensajeLabel.Text = "El comprobante ya está vigente. No es posible actualizar su estado.";
                            return;
                        }
                        if (comprobante.IdDestinoComprobante != "AFIP")
                        {
                            if (sesion.Cuit.NroSerieCertifITF.Equals(string.Empty))
                            {
                                MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                                return;
                            }

                            RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Consulta de Lote CUIT: " + comprobante.Cuit + "  Nro.Lote: " + comprobante.NroLote + "  Nro. Punto de Vta.: " + comprobante.NroPuntoVta);
                            RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "NroSerieCertifITF: " + sesion.Cuit.NroSerieCertifITF);

                            certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(sesion.Cuit.NroSerieCertifITF, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                            org.dyndns.cedweb.consulta.ConsultaIBK clcdyndnsConsultaIBK = new org.dyndns.cedweb.consulta.ConsultaIBK();
                            string ConsultaIBKUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKUtilizarServidorExterno"];
                            RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKUtilizarServidorExterno: " + ConsultaIBKUtilizarServidorExterno);
                            if (ConsultaIBKUtilizarServidorExterno == "SI")
                            {
                                clcdyndnsConsultaIBK.Url = System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"];
                                RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), "Parametro ConsultaIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["ConsultaIBKurl"]);
                            }
                            org.dyndns.cedweb.consulta.ConsultarResult clcrdyndns = new org.dyndns.cedweb.consulta.ConsultarResult();
                            clcrdyndns = clcdyndnsConsultaIBK.Consultar(Convert.ToInt64(comprobante.Cuit), comprobante.NroLote, comprobante.NroPuntoVta, certificado);
                            FeaEntidades.InterFacturas.lote_comprobantes lc = new FeaEntidades.InterFacturas.lote_comprobantes();
                            lc = Funciones.Ws2Fea(clcrdyndns);
                            Cache["ComprobanteAConsultar"] = lc;
                            string XML = "";
                            RN.Comprobante.SerializarLc(out XML, lc);
                            comprobante.Response = XML;
                            if (lc.cabecera_lote.resultado == "A")
                            {
                                //Controlar que sea el mismo comprobante (local vs on-line)
                                if (comprobante.Nro != lc.comprobante[0].cabecera.informacion_comprobante.numero_comprobante)
                                {
                                    MensajeLabel.Text = "(Campo: Nro. de Comprobante). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. No se puede actualizar el estado.";
                                    return;
                                }
                                if (comprobante.TipoComprobante.Id != lc.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante)
                                {
                                    MensajeLabel.Text = "(Campo: Tipo de Comprobante). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. No se puede actualizar el estado.";
                                    return;
                                }
                                if (comprobante.Importe != lc.comprobante[0].resumen.importe_total_factura)
                                {
                                    MensajeLabel.Text += "(Campo: Importe). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                    comprobante.Importe = lc.comprobante[0].resumen.importe_total_factura;
                                }
                                if (comprobante.Moneda != lc.comprobante[0].resumen.codigo_moneda)
                                {
                                    MensajeLabel.Text += "(Campo: Moneda). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                    comprobante.Moneda = lc.comprobante[0].resumen.codigo_moneda;
                                }
                                if (lc.comprobante[0].resumen.importes_moneda_origen != null)
                                {
                                    if (comprobante.ImporteMoneda != lc.comprobante[0].resumen.importes_moneda_origen.importe_total_factura)
                                    {
                                        MensajeLabel.Text += "(Campo: Importe Moneda). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                        comprobante.ImporteMoneda = lc.comprobante[0].resumen.importes_moneda_origen.importe_total_factura;
                                    }
                                }
                                else
                                {
                                    if (comprobante.ImporteMoneda != 0)
                                    {
                                        MensajeLabel.Text += "(Campo: Importe Moneda). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                        comprobante.ImporteMoneda = 0;
                                    }
                                }
                                if (comprobante.TipoCambio != lc.comprobante[0].resumen.tipo_de_cambio)
                                {
                                    MensajeLabel.Text += "(Campo: Tipo de cambio). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                    comprobante.TipoCambio = lc.comprobante[0].resumen.tipo_de_cambio;
                                }
                                //if (comprobante.Fecha != lc.comprobante[0].cabecera.informacion_comprobante.fecha_emision)
                                //{
                                //    MensajeLabel.Text += "(Campo: Tipo de cambio). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                //    comprobante.Fecha = lc.comprobante[0].resumen.fecha_emision;
                                //}
                                //if (comprobante.Fecha != lc.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento)
                                //{
                                //    MensajeLabel.Text += "(Campo: Tipo de cambio). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                //    comprobante.FechaVto = lc.comprobante[0].resumen.fecha_vencimiento;
                                //}
                                if (comprobante.Documento.Tipo.Id != lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio.ToString())
                                {
                                    MensajeLabel.Text += "(Campo: Cod.Doc). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                    comprobante.Documento.Tipo.Id = lc.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio.ToString();
                                }
                                if (comprobante.Documento.Nro != lc.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio)
                                {
                                    MensajeLabel.Text += "(Campo: Nro.Doc). Hay diferencias entre en comprobante local y el registrado en Interfacturas / AFIP. Igualmente se pudo actualizar la información y el estado.";
                                    comprobante.Documento.Nro = lc.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio;
                                }

                                comprobante.WF.Estado = "Vigente";
                                RN.Comprobante.Actualizar(comprobante, (Entidades.Sesion)Session["Sesion"]);
                                script = "window.open('/ComprobanteConsulta.aspx', '');";
                                BuscarButton_Click(sender, new EventArgs());
                                RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), script);
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);
                            }
                            else if (lc.cabecera_lote.resultado == "R")
                            {
                                comprobante.WF.Estado = "Rechazado";
                                RN.Comprobante.Actualizar(comprobante, (Entidades.Sesion)Session["Sesion"]);
                                script = "<SCRIPT LANGUAGE='javascript'>alert('Respuesta de ITF / AFIP.\\n" + "Resultado: " + lc.cabecera_lote.resultado + "  Motivo: " + lc.cabecera_lote.motivo + "');</script>";
                                RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), script);
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Message", script, true);
                            }
                            else
                            {
                                MensajeLabel.Text = "No se puede realizar la actualización, cuando el comprobante se encuentra en el siguiente estado en Interfacturas ( Estado: " + clcrdyndns.comprobante[0].cabecera.informacion_comprobante.resultado + ").";
                                return;
                            }
                        }
                        else
                        {
                            string respuesta = "";
                            //Deserializar
                            FeaEntidades.InterFacturas.lote_comprobantes lcFea = new FeaEntidades.InterFacturas.lote_comprobantes();
                            string xml = comprobante.Request;
                            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(FeaEntidades.InterFacturas.lote_comprobantes));
                            using (TextReader reader = new StringReader(xml))
                            {
                                lcFea = (FeaEntidades.InterFacturas.lote_comprobantes)serializer.Deserialize(reader);
                            }
                            string caeNro;
                            string caeFecVto;
                            string caeFecPro;
                            respuesta = RN.ComprobanteAFIP.ConsultarAFIP(out caeNro, out caeFecVto, out caeFecPro, lcFea, (Entidades.Sesion)Session["Sesion"]);
                            Cache["ComprobanteAConsultar"] = lcFea;
                            if (respuesta.Length >= 12 && respuesta.Substring(0, 12) == "Resultado: A")
                            {
                                comprobante.WF.Estado = "Vigente";
                                if (caeNro != "")
                                {
                                    lcFea.cabecera_lote.resultado = "A";
                                    lcFea.comprobante[0].cabecera.informacion_comprobante.resultado = "A";
                                    lcFea.comprobante[0].cabecera.informacion_comprobante.cae = caeNro;
                                    lcFea.comprobante[0].cabecera.informacion_comprobante.caeSpecified = true;
                                    lcFea.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento_cae = caeFecVto;
                                    lcFea.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento_caeSpecified = true;
                                    lcFea.comprobante[0].cabecera.informacion_comprobante.fecha_obtencion_cae = caeFecPro;
                                    lcFea.comprobante[0].cabecera.informacion_comprobante.fecha_obtencion_caeSpecified = true;
                                }
                                Cache["ComprobanteAConsultar"] = lcFea;
                                string XML = "";
                                RN.Comprobante.SerializarLc(out XML, lcFea);
                                comprobante.Response = XML;

                                RN.Comprobante.Actualizar(comprobante, (Entidades.Sesion)Session["Sesion"]);
                                script = "window.open('/ComprobanteConsulta.aspx', '');";
                                BuscarButton_Click(sender, new EventArgs());
                                RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), script);
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);
                            }
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
                            MensajeLabel.Text = soapEx.Actor + "\\n" + errorMessage.Replace("\r", "").Replace("\n", "");
                        }
                        catch (Exception)
                        {
                            throw soapEx;
                        }
                    }
                    #endregion
                    break;
                case "ExportarRG2485":
                    #region ExportarRG2485
                    lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                    x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                    if (comprobante.Estado != "Vigente")
                    {
                        MensajeLabel.Text = "El comprobante no está vigente.";
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

                        //Crear nombre de archivo default sin extensión
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(lote.cabecera_lote.cuit_vendedor);
                        sb.Append("-");
                        sb.Append(lote.cabecera_lote.punto_de_venta.ToString("0000"));
                        sb.Append("-");
                        sb.Append(lote.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante.ToString("00"));
                        sb.Append("-");
                        sb.Append(lote.comprobante[0].cabecera.informacion_comprobante.numero_comprobante.ToString("00000000"));
                    
                        //Crear nombre de archivo ZIP
                        System.Text.StringBuilder sbZIP = new System.Text.StringBuilder();
                        sbZIP.Append(sb.ToString() + ".zip");

                        //Crear archivo CABECERA EMISOR
                        System.Text.StringBuilder sbCabeceraE = new System.Text.StringBuilder();
                        sbCabeceraE.Append(sb.ToString() + "-CABECERA_EMISOR.txt");
                        System.IO.MemoryStream m = new System.IO.MemoryStream();
                        System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath(@"~/Temp/" + sbCabeceraE.ToString()), System.IO.FileMode.Create);
                        m.WriteTo(fs);
                        fs.Close();
                        //Guardar info en archivo CABECERA EMISOR
                        System.Text.StringBuilder sbDataCabeceraE = new System.Text.StringBuilder();
                        string Campo2 = String.Format("{0,11}", sesion.Cuit.Nro);
                        string Campo3 = String.Format("{0,-30}", Truncate(sesion.Cuit.RazonSocial, 30));
                        string Campo4 = String.Format("{0,-30}", sesion.Cuit.DatosImpositivos.NroIngBrutos);
                        string Campo5 = sesion.Cuit.DatosImpositivos.IdCondIVA.ToString("00");
                        string Campo6 = String.Format("{0,-30}", ""); 
                        try
                        {
                            string RespAux6 = FeaEntidades.CondicionesIVA.CondicionIVA.Lista().Find(delegate(FeaEntidades.CondicionesIVA.CondicionIVA ci)
                            {
                                return (ci.Codigo == sesion.Cuit.DatosImpositivos.IdCondIVA);
                            }).Descr;
                            Campo6 = String.Format("{0,-27}", Truncate(RespAux6, 27));
                        }
                        catch 
                        {
                        }
                        string Campo7 = String.Format("{0,-8}", sesion.Cuit.DatosImpositivos.FechaInicioActividades.ToString("yyyyMMdd"));
                        string Campo8 = String.Format("{0,-30}", sesion.Cuit.Domicilio.Calle);
                        string Campo9 = String.Format("{0,-6}", sesion.Cuit.Domicilio.Nro);
                        string Campo10 = String.Format("{0,-5}", sesion.Cuit.Domicilio.Piso);
                        string Campo11 = String.Format("{0,-5}", sesion.Cuit.Domicilio.Depto);
                        string Campo12 = String.Format("{0,-5}", sesion.Cuit.Domicilio.Sector);
                        string Campo13 = String.Format("{0,-5}", sesion.Cuit.Domicilio.Torre);
                        string Campo14 = String.Format("{0,-5}", sesion.Cuit.Domicilio.Manzana);
                        string Campo15 = Convert.ToInt32(sesion.Cuit.Domicilio.Provincia.Id).ToString("00");
                        string Campo16 = String.Format("{0,-8}", sesion.Cuit.Domicilio.CodPost);
                        string Campo17 = String.Format("{0,-25}", Truncate(sesion.Cuit.Domicilio.Localidad, 25));
                        sbDataCabeceraE.AppendLine("1" + Campo2 + Campo3 + Campo4 + Campo5 + Campo6 + Campo7 + Campo8 + Campo9 + Campo10 + Campo11 + Campo12 + Campo13 + Campo14 + Campo15 + Campo16 + Campo17);
                        using (StreamWriter outfile = new StreamWriter(Server.MapPath(@"~/Temp/" + sbCabeceraE.ToString())))
                        {
                            outfile.Write(sbDataCabeceraE.ToString());
                        }

                        //Crear archivo CABECERA COMPROBANTE 
                        System.Text.StringBuilder sbCabeceraC = new System.Text.StringBuilder();
                        sbCabeceraC.Append(sb.ToString() + "-CABECERA_COMPROBANTE.txt");
                        m = new System.IO.MemoryStream();
                        fs = new System.IO.FileStream(Server.MapPath(@"~/Temp/" + sbCabeceraC.ToString()), System.IO.FileMode.Create);
                        m.WriteTo(fs);
                        fs.Close();
                        //Guardar info en archivo CABECERA COMPROBANTE
                        System.Text.StringBuilder sbDataCabeceraC = new System.Text.StringBuilder();
                        Campo2 = "ORIGINAL";
                        Campo3 = String.Format("{0,-8}", lote.comprobante[0].cabecera.informacion_comprobante.fecha_emision);
                        Campo4 = lote.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante.ToString("00");
                        if (Campo4 == "01" || Campo4 == "02" || Campo4 == "03" || Campo4 == "04" || Campo4 == "05" || Campo4 == "39" || Campo4 == "60" || Campo4 == "63")
                        {
                            Campo5 = "A";
                        }
                        else if (Campo4 == "06" || Campo4 == "07" || Campo4 == "08" || Campo4 == "09" || Campo4 == "10" || Campo4 == "40" || Campo4 == "61" || Campo4 == "64")
                        {
                            Campo5 = "B";
                        }
                        else
                        {
                            Campo5 = " ";
                        }
                        Campo6 = lote.comprobante[0].cabecera.informacion_comprobante.punto_de_venta.ToString("0000");
                        Campo7 = lote.comprobante[0].cabecera.informacion_comprobante.numero_comprobante.ToString("00000000");
                        Campo8 = lote.comprobante[0].cabecera.informacion_comprobante.numero_comprobante.ToString("00000000");
                        Campo9 = lote.comprobante[0].cabecera.informacion_comprador.codigo_doc_identificatorio.ToString("00");
                        Campo10 = lote.comprobante[0].cabecera.informacion_comprador.nro_doc_identificatorio.ToString("00000000000");
                        Campo11 = String.Format("{0,-30}", lote.comprobante[0].cabecera.informacion_comprador.denominacion);
                        Campo12 = lote.comprobante[0].cabecera.informacion_comprador.condicion_IVA.ToString("00");
                        Campo13 = String.Format("{0,-30}", Truncate(lote.comprobante[0].cabecera.informacion_comprador.domicilio_calle, 30));
                        Campo14 = String.Format("{0,-6}", lote.comprobante[0].cabecera.informacion_comprador.domicilio_numero);
                        Campo15 = String.Format("{0,-5}", lote.comprobante[0].cabecera.informacion_comprador.domicilio_piso);
                        Campo16 = String.Format("{0,-5}", lote.comprobante[0].cabecera.informacion_comprador.domicilio_depto);
                        Campo17 = String.Format("{0,-5}", lote.comprobante[0].cabecera.informacion_comprador.domicilio_sector);
                        string Campo18 = String.Format("{0,-5}", lote.comprobante[0].cabecera.informacion_comprador.domicilio_torre);
                        string Campo19 = String.Format("{0,-5}", lote.comprobante[0].cabecera.informacion_comprador.domicilio_manzana);
                        string Campo20 = String.Format("{0,2}", lote.comprobante[0].cabecera.informacion_comprador.provincia);
                        string Campo21 = String.Format("{0,-8}", lote.comprobante[0].cabecera.informacion_comprador.cp);
                        string Campo22 = String.Format("{0,-25}", Truncate(lote.comprobante[0].cabecera.informacion_comprador.localidad, 25));
                        string Campo23 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_factura.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_factura.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo24 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_concepto_no_gravado.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_concepto_no_gravado.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo25 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_neto_gravado.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_neto_gravado.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo26 = String.Format("{0,16}", lote.comprobante[0].resumen.impuesto_liq.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.impuesto_liq.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo27 = String.Format("{0,16}", lote.comprobante[0].resumen.impuesto_liq_rni.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.impuesto_liq_rni.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo28 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_operaciones_exentas.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_operaciones_exentas.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo29 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_impuestos_nacionales.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_impuestos_nacionales.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo30 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_ingresos_brutos.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_ingresos_brutos.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo31 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_impuestos_municipales.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_impuestos_municipales.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo32 = String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_impuestos_internos.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].resumen.importe_total_impuestos_internos.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                        string Campo33 = String.Format("{0,-3}", lote.comprobante[0].resumen.codigo_moneda);
                        string Campo34 = String.Format("{0,11}", lote.comprobante[0].resumen.tipo_de_cambio.ToString(new string(Convert.ToChar("0"), 8) + ".00")).Substring(0, 8) + String.Format("{0,11}", lote.comprobante[0].resumen.tipo_de_cambio.ToString(new string(Convert.ToChar("0"), 8) + ".00")).Substring(9, 2);
                        int CantAlicuotas = 0;
                        if (lote.comprobante[0].resumen.cant_alicuotas_iva == 0)
                        {
                            if (lote.comprobante[0].resumen.impuestos != null)
                            {
                                for (int z = 0; z < lote.comprobante[0].resumen.impuestos.Length; z++)
                                {
                                    if (lote.comprobante[0].resumen.impuestos[z].codigo_impuesto == 1)
                                    {
                                        CantAlicuotas += 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            CantAlicuotas = lote.comprobante[0].resumen.cant_alicuotas_iva;
                        }
                        string Campo35 = String.Format("{0,1}", CantAlicuotas);
                        string Campo36 = String.Format("{0,1}", lote.comprobante[0].cabecera.informacion_comprobante.codigo_operacion);
                        string Campo37 = String.Format("{0,-14}", lote.comprobante[0].cabecera.informacion_comprobante.cae);
                        string Campo38 = String.Format("{0,-8}", lote.comprobante[0].cabecera.informacion_comprobante.fecha_vencimiento_cae);
                        string Campo39 = String.Format("{0,8}", "        ");

                        sbDataCabeceraC.AppendLine("1" + Campo2 + Campo3 + Campo4 + Campo5 + Campo6 + Campo7 + Campo8 + Campo9 + Campo10 + Campo11 + Campo12 + Campo13 + Campo14 + Campo15 + Campo16 + Campo17 + Campo18 + Campo19 + Campo20 + Campo21 + Campo22 + Campo23 + Campo24 + Campo25 + Campo26 + Campo27 + Campo28 + Campo29 + Campo30 + Campo31 + Campo32 + Campo33 + Campo34 + Campo35 + Campo36 + Campo37 + Campo38 + Campo39); 
                        using (StreamWriter outfile = new StreamWriter(Server.MapPath(@"~/Temp/" + sbCabeceraC.ToString())))
                        {
                            outfile.Write(sbDataCabeceraC.ToString());
                        }

                        //Crear archivo DETALLE
                        System.Text.StringBuilder sbDetalle = new System.Text.StringBuilder();
                        sbDetalle.Append(sb.ToString() + "-DETALLE.txt");
                        m = new System.IO.MemoryStream();
                        fs = new System.IO.FileStream(Server.MapPath(@"~/Temp/" + sbDetalle.ToString()), System.IO.FileMode.Create);
                        m.WriteTo(fs);
                        fs.Close();
                        //Guardar info en archivo DETALLE
                        System.Text.StringBuilder sbDataDetalle = new System.Text.StringBuilder();
                        for (int i = 0; i < lote.comprobante[0].detalle.linea.Length; i++)
                        {
                            string descr = lote.comprobante[0].detalle.linea[i].descripcion;
                            if (descr.Length > 0 && descr.Substring(0, 1) == "%")
                            {
                                descr = RN.Funciones.HexToString(descr);
                            }
                            Campo2 = String.Format("{0,-100}", Truncate(descr, 100));
                            //cantidad de 12 (7 + 5)
                            Campo3 = String.Format("{0,13}", lote.comprobante[0].detalle.linea[i].cantidad.ToString(new string(Convert.ToChar("0"), 7) + ".00000")).Substring(0, 7) + String.Format("{0,13}", lote.comprobante[0].detalle.linea[i].cantidad.ToString(new string(Convert.ToChar("0"), 7) + ".00000")).Substring(8, 5);
                            //ojo format
                            Campo4 = Convert.ToInt32(lote.comprobante[0].detalle.linea[i].unidad).ToString("00");
                            Campo5 = String.Format("{0,17}", lote.comprobante[0].detalle.linea[i].precio_unitario.ToString(new string(Convert.ToChar("0"), 13) +".000")).Substring(0, 13) + String.Format("{0,17}", lote.comprobante[0].detalle.linea[i].precio_unitario.ToString(new string(Convert.ToChar("0"), 13) + ".000")).Substring(14, 3);
                            Campo6 = String.Format("{0,16}", lote.comprobante[0].detalle.linea[i].importe_total_descuentos.ToString(new string(Convert.ToChar("0"), 13) +".00")).Substring(0, 13) + String.Format("{0,16}", lote.comprobante[0].detalle.linea[i].importe_total_descuentos.ToString(new string(Convert.ToChar("0"), 13) + ".00")).Substring(14, 2);
                            //importe ajuste
                            Campo7 = String.Format("{0,16}", new string(Convert.ToChar("0"), 16));
                            Campo8 = String.Format("{0,17}", lote.comprobante[0].detalle.linea[i].importe_total_articulo.ToString(new string(Convert.ToChar("0"), 13) +".000")).Substring(0, 13) + String.Format("{0,17}", lote.comprobante[0].detalle.linea[i].importe_total_articulo.ToString(new string(Convert.ToChar("0"), 13) + ".000")).Substring(14, 3);
                            Campo9 = String.Format("{0,5}", lote.comprobante[0].detalle.linea[i].alicuota_iva.ToString("00.00")).Substring(0, 2) + String.Format("{0,5}", lote.comprobante[0].detalle.linea[i].alicuota_iva.ToString("00.00")).Substring(3, 2);
                            Campo10 = String.Format("{0,17}", lote.comprobante[0].detalle.linea[i].importe_iva.ToString(new string(Convert.ToChar("0"), 14) + ".00")).Substring(0, 14) + String.Format("{0,17}", lote.comprobante[0].detalle.linea[i].importe_iva.ToString(new string(Convert.ToChar("0"), 14) + ".00")).Substring(15, 2);
                            Campo11 = String.Format("{0,1}", lote.comprobante[0].detalle.linea[i].indicacion_exento_gravado);
                            sbDataDetalle.AppendLine("3" + Campo2 + Campo3 + Campo4 + Campo5 + Campo6 + Campo7 + Campo8 + Campo9 + Campo10 + Campo11);
                        }
                        using (StreamWriter outfile = new StreamWriter(Server.MapPath(@"~/Temp/" + sbDetalle.ToString())))
                        {
                            outfile.Write(sbDataDetalle.ToString());
                        }

                        //Descargar ZIP ( Cabecera Emisor, Cabecera Comprobante y Detalle )
                        string filename = sbZIP.ToString();
                        String dlDir = @"~/Temp/";
                        String path = Server.MapPath(dlDir + filename);
                        System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
                        System.IO.FileInfo toCabeceraE = new System.IO.FileInfo(Server.MapPath(dlDir + sbCabeceraE.ToString()));
                        System.IO.FileInfo toCabeceraC = new System.IO.FileInfo(Server.MapPath(dlDir + sbCabeceraC.ToString()));
                        System.IO.FileInfo toDetalle = new System.IO.FileInfo(Server.MapPath(dlDir + sbDetalle.ToString()));

                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddFile(Server.MapPath(dlDir + sbCabeceraE.ToString()), "");
                            zip.AddFile(Server.MapPath(dlDir + sbCabeceraC.ToString()), "");
                            zip.AddFile(Server.MapPath(dlDir + sbDetalle.ToString()), "");
                            zip.Save(Server.MapPath(dlDir + filename));
                        }
                        if (toDownload.Exists)
                        {
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                            Response.AddHeader("Content-Length", toDownload.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.WriteFile(dlDir + filename);
                            Response.End();

                            toDownload.Delete();
                            toCabeceraE.Delete();
                            toCabeceraC.Delete();
                            toDetalle.Delete();
                        }
                        else
                        {
                            WebForms.Excepciones.Redireccionar(new EX.Validaciones.ArchivoInexistente(filename), "~/NotificacionDeExcepcion.aspx");
                        }
                        //Server.Transfer("~/DescargaTemporarios.aspx?archivo=" + sb.ToString(), false);
                    }
                    catch (Exception ex)
                    {
                        script = "Problemas para generar la interfaz.\\n" + ex.Message + "\\n" + ex.StackTrace;
                        RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), script);
                        MensajeLabel.Text = script;
                    }
                    #endregion
                    break;
                case "XMLOnLine":
                    #region XMLOnLine
                    if (comprobante.Estado != "Vigente")
                    {
                        MensajeLabel.Text = "El comprobante no está vigente.";
                        return;
                    }
                    MensajeLabel.Text = String.Empty;
                    Entidades.Cliente cliente = ((List<Entidades.Cliente>)ViewState["Clientes"])[ClienteDropDownList.SelectedIndex];
                    //resp = RN.Comprobante.ComprobanteDetalleIBK(((Entidades.Sesion)Session["Sesion"]).Cuit.Nro, comprobante.NroPuntoVta.ToString(), comprobante.TipoComprobante.Id.ToString(), comprobante.Nro, 0, ((Entidades.Sesion)Session["Sesion"]).Cuit.NroSerieCertifITF);

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
                        MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                        return;
                    }
                    GrabarLogTexto("~/Detallar.txt", "Consulta de Lote CUIT: " + sesion.Cuit.Nro + "  Fecha Desde: " + FechaDesdeTextBox.Text + "  Fecha Hasta: " + FechaHastaTextBox.Text);
                    GrabarLogTexto("~/Detallar.txt", "NroSerieCertifITF: " + sesion.Cuit.NroSerieCertifITF);
                    if (sesion.Cuit.NroSerieCertifITF.Equals(string.Empty))
                    {
                        MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                        return;
                    }

                    certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(sesion.Cuit.NroSerieCertifITF, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                    GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKUtilizarServidorExterno: " + DetalleIBKUtilizarServidorExterno);
                    if (DetalleIBKUtilizarServidorExterno == "SI")
                    {
                        clcdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"];
                        GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"]);
                    }
                    resp = clcdyndns.DetallarIBK(cecd, certificado);

                    try
                    {
                        string comprobanteXML = resp;
                        System.Text.StringBuilder sbXMLData = new System.Text.StringBuilder();
                        sbXMLData.AppendLine(comprobanteXML);

                        //Crear nombre de archivo default sin extensión
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(comprobante.Cuit);
                        sb.Append("-");
                        sb.Append(comprobante.NroPuntoVta.ToString("0000"));
                        sb.Append("-");
                        sb.Append(comprobante.TipoComprobante.Id.ToString("00"));
                        sb.Append("-");
                        sb.Append(comprobante.Nro.ToString("00000000"));

                        //Crear nombre de archivo XML
                        System.Text.StringBuilder sbXML = new System.Text.StringBuilder();
                        sbXML.Append(sb.ToString() + ".xml");

                        //Crear archivo comprobante XML
                        System.IO.MemoryStream m = new System.IO.MemoryStream();
                        System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath(@"~/Temp/" + sbXML.ToString()), System.IO.FileMode.Create);
                        m.WriteTo(fs);
                        fs.Close();

                        //Grabar información comprobante XML
                        using (StreamWriter outfile = new StreamWriter(Server.MapPath(@"~/Temp/" + sbXML.ToString())))
                        {
                            outfile.Write(sbXMLData.ToString());
                        }

                        string filename = sbXML.ToString();
                        String dlDir = @"~/Temp/";
                        System.IO.FileInfo toDownload = new System.IO.FileInfo(Server.MapPath(dlDir + filename));
                        if (toDownload.Exists)
                        {
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                            Response.AddHeader("Content-Length", toDownload.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.WriteFile(dlDir + filename);
                            Response.End();

                            toDownload.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        script = "Problemas para generar el XML.\\n" + ex.Message;
                        script += ex.StackTrace;
                        if (ex.InnerException != null)
                        {
                            script = ex.InnerException.Message;
                        }
                        RN.Sesion.GrabarLogTexto(Server.MapPath("~/Detallar.txt"), script);
                        MensajeLabel.Text += script;
                    }
                    #endregion
                    break;
                case "PDF":
                    if (comprobante.IdDestinoComprobante == "ITF")
                    {
                        #region PDF (InterFacturas)
                        //OBTENCIÓN DE PDF DE INTERFACTURAS
                        if (comprobante.Estado != "Vigente")
                        {
                            MensajeLabel.Text = "El comprobante no está vigente.";
                            return;
                        }
                        MensajeLabel.Text = String.Empty;
                        //Entidades.Cliente cliente = ((List<Entidades.Cliente>)ViewState["Clientes"])[ClienteDropDownList.SelectedIndex];
                        //resp = RN.Comprobante.ComprobanteDetalleIBK(((Entidades.Sesion)Session["Sesion"]).Cuit.Nro, comprobante.NroPuntoVta.ToString(), comprobante.TipoComprobante.Id.ToString(), comprobante.Nro, 0, ((Entidades.Sesion)Session["Sesion"]).Cuit.NroSerieCertifITF);

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
                            MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                            return;
                        }
                        GrabarLogTexto("~/Detallar.txt", "Consulta de Lote CUIT: " + sesion.Cuit.Nro + "  Fecha Desde: " + FechaDesdeTextBox.Text + "  Fecha Hasta: " + FechaHastaTextBox.Text);
                        GrabarLogTexto("~/Detallar.txt", "NroSerieCertifITF: " + sesion.Cuit.NroSerieCertifITF);
                        if (sesion.Cuit.NroSerieCertifITF.Equals(string.Empty))
                        {
                            MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                            return;
                        }

                        certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(sesion.Cuit.NroSerieCertifITF, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                        GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKUtilizarServidorExterno: " + DetalleIBKUtilizarServidorExterno);
                        if (DetalleIBKUtilizarServidorExterno == "SI")
                        {
                            clcdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"];
                            GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"]);
                        }
                        resp = clcdyndns.DetallarIBK(cecd, certificado);

                        try
                        {
                            string comprobanteXML = resp;

                            GrabarLogTexto("~/Detallar.txt", "Inicia ExecuteCommand");
                            org.dyndns.cedweb.generoPDF.GeneroPDF pdfdyndns = new org.dyndns.cedweb.generoPDF.GeneroPDF();

                            string GenerarPDFUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["GenerarPDFUtilizarServidorExterno"];
                            GrabarLogTexto("~/Detallar.txt", "Parametro GenerarPDFUtilizarServidorExterno: " + GenerarPDFUtilizarServidorExterno);
                            if (GenerarPDFUtilizarServidorExterno == "SI")
                            {
                                pdfdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["GenerarPDFurl"];
                                GrabarLogTexto("~/Detallar.txt", "Parametro GenerarPDFurl: " + System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"]);
                            }
                            string RespPDF = pdfdyndns.GenerarPDF(comprobante.Cuit, comprobante.NroPuntoVta, comprobante.TipoComprobante.Id, comprobante.Nro, comprobanteXML);
                            GrabarLogTexto("~/Detallar.txt", "Finaliza ExecuteCommand");

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
                            //string fileName = Server.MapPath("~/TempRender/a.pdf");
                            string filename = sb.ToString();
                            String dlDir = @"~/TempRender/";
                            new System.Net.WebClient().DownloadFile(url, Server.MapPath(dlDir + filename));

                            System.IO.FileInfo toDownload = new System.IO.FileInfo(Server.MapPath(dlDir + filename));
                            if (toDownload.Exists)
                            {
                                Response.Clear();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                                Response.AddHeader("Content-Length", toDownload.Length.ToString());
                                Response.ContentType = "application/octet-stream";
                                Response.WriteFile(dlDir + filename);
                                Response.End();

                                toDownload.Delete();
                            }

                            //script = "window.open('" + RespPDF + "', '');";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);

                            //Response.Write("<script>window.open('" + RespPDF + "','_blank');</script>");
                            //Response.Flush();
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
                            MensajeLabel.Text = script;
                        }
                        #endregion
                    }
                    else
                    {
                        #region PDF (local)
                        //GENERACIÓN DE PDF A PARTIR DE DATOS LOCALES
                        lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                        x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                        if (comprobante.Estado != "Vigente")
                        {
                            MensajeLabel.Text = "El comprobante no está vigente.";
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

                            string comprobanteXML = "";
                            RN.Comprobante.SerializarC(out comprobanteXML, lote.comprobante[0]);
                            System.Text.StringBuilder sbXMLData = new System.Text.StringBuilder();
                            sbXMLData.AppendLine(comprobanteXML);

                            //Crear nombre de archivo default sin extensión
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(lote.cabecera_lote.cuit_vendedor);
                            sb.Append("-");
                            sb.Append(lote.cabecera_lote.punto_de_venta.ToString("0000"));
                            sb.Append("-");
                            sb.Append(lote.comprobante[0].cabecera.informacion_comprobante.tipo_de_comprobante.ToString("00"));
                            sb.Append("-");
                            sb.Append(lote.comprobante[0].cabecera.informacion_comprobante.numero_comprobante.ToString("00000000"));

                            //Crear nombre de archivo ZIP
                            System.Text.StringBuilder sbXML = new System.Text.StringBuilder();
                            sbXML.Append(sb.ToString() + ".xml");
                            System.Text.StringBuilder sbPDF = new System.Text.StringBuilder();
                            sbPDF.Append(sb.ToString() + ".pdf");

                            //Crear archivo comprobante XML
                            System.IO.MemoryStream m = new System.IO.MemoryStream();
                            System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath(@"~/Temp/" + sbXML.ToString()), System.IO.FileMode.Create);
                            m.WriteTo(fs);
                            fs.Close();

                            //Grabar información comprobante XML
                            using (StreamWriter outfile = new StreamWriter(Server.MapPath(@"~/Temp/" + sbXML.ToString())))
                            {
                                outfile.Write(sbXMLData.ToString());
                            }

                            ExecuteCommand(sbXML.ToString(), sbPDF.ToString());
                        }
                        catch (Exception ex)
                        {
                            script = "Problemas para generar el PDF.\\n" + ex.Message;
                            RN.Sesion.GrabarLogTexto(Server.MapPath("~/Consultar.txt"), script);
                            MensajeLabel.Text = script;
                        }
                        #endregion
                    }
                    break;
                case "PDF-Viewer":
                    #region PDF-Viewer
                    if (comprobante.Estado != "Vigente")
                    {
                        MensajeLabel.Text = "El comprobante no está vigente.";
                        return;
                    }
                    MensajeLabel.Text = String.Empty;
                    //Entidades.Cliente cliente = ((List<Entidades.Cliente>)ViewState["Clientes"])[ClienteDropDownList.SelectedIndex];
                    //resp = RN.Comprobante.ComprobanteDetalleIBK(((Entidades.Sesion)Session["Sesion"]).Cuit.Nro, comprobante.NroPuntoVta.ToString(), comprobante.TipoComprobante.Id.ToString(), comprobante.Nro, 0, ((Entidades.Sesion)Session["Sesion"]).Cuit.NroSerieCertifITF);

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
                        MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                        return;
                    }
                    GrabarLogTexto("~/Detallar.txt", "Consulta de Lote CUIT: " + sesion.Cuit.Nro + "  Fecha Desde: " + FechaDesdeTextBox.Text + "  Fecha Hasta: " + FechaHastaTextBox.Text);
                    GrabarLogTexto("~/Detallar.txt", "NroSerieCertifITF: " + sesion.Cuit.NroSerieCertifITF);
                    if (sesion.Cuit.NroSerieCertifITF.Equals(string.Empty))
                    {
                        MensajeLabel.Text = "Aún no disponemos de su certificado digital";
                        return;
                    }

                    certificado = CaptchaDotNet2.Security.Cryptography.Encryptor.Encrypt(sesion.Cuit.NroSerieCertifITF, "srgerg$%^bg", Convert.FromBase64String("srfjuoxp")).ToString();
                    GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKUtilizarServidorExterno: " + DetalleIBKUtilizarServidorExterno);
                    if (DetalleIBKUtilizarServidorExterno == "SI")
                    {
                        clcdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"];
                        GrabarLogTexto("~/Detallar.txt", "Parametro DetalleIBKurl: " + System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"]);
                    }
                    resp = clcdyndns.DetallarIBK(cecd, certificado);

                    try
                    {
                        string comprobanteXML = resp;

                        GrabarLogTexto("~/Detallar.txt", "Inicia ExecuteCommand");
                        org.dyndns.cedweb.generoPDF.GeneroPDF pdfdyndns = new org.dyndns.cedweb.generoPDF.GeneroPDF();

                        string GenerarPDFUtilizarServidorExterno = System.Configuration.ConfigurationManager.AppSettings["GenerarPDFUtilizarServidorExterno"];
                        GrabarLogTexto("~/Detallar.txt", "Parametro GenerarPDFUtilizarServidorExterno: " + GenerarPDFUtilizarServidorExterno);
                        if (GenerarPDFUtilizarServidorExterno == "SI")
                        {
                            pdfdyndns.Url = System.Configuration.ConfigurationManager.AppSettings["GenerarPDFurl"];
                            GrabarLogTexto("~/Detallar.txt", "Parametro GenerarPDFurl: " + System.Configuration.ConfigurationManager.AppSettings["DetalleIBKurl"]);
                        }
                        string RespPDF = pdfdyndns.GenerarPDF(comprobante.Cuit, comprobante.NroPuntoVta, comprobante.TipoComprobante.Id, comprobante.Nro, comprobanteXML);
                        GrabarLogTexto("~/Detallar.txt", "Finaliza ExecuteCommand");

                        script = "window.open('" + RespPDF + "', '');";
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
                        MensajeLabel.Text = script;
                    }
                    #endregion  
                    break;
                case "XML-ClonarAlta":
                    #region XML-ClonarAlta
                    lote = new FeaEntidades.InterFacturas.lote_comprobantes();
                    x = new System.Xml.Serialization.XmlSerializer(lote.GetType());
                    try
                    {
                        comprobante.Response = comprobante.Response.Replace("iso-8859-1", "utf-16");
                        bytes = new byte[comprobante.Response.Length * sizeof(char)];
                        System.Buffer.BlockCopy(comprobante.Response.ToCharArray(), 0, bytes, 0, bytes.Length);
                        ms = new System.IO.MemoryStream(bytes);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);
                    }
                    catch
                    {
                        bytes = new byte[comprobante.Request.Length * sizeof(char)];
                        System.Buffer.BlockCopy(comprobante.Request.ToCharArray(), 0, bytes, 0, bytes.Length);
                        ms = new System.IO.MemoryStream(bytes);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        lote = (FeaEntidades.InterFacturas.lote_comprobantes)x.Deserialize(ms);
                    }
                    Cache["ComprobanteAClonar"] = lote;
                    script = "window.open('/Facturacion/Electronica/Lote.aspx', '');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "popup", script, true);
                    #endregion
                    break;
            }
        }
    }
}