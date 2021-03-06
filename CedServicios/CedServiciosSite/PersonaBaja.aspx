﻿<%@ Page Title="" Language="C#" MasterPageFile="~/CedServicios.Master" AutoEventWireup="true" CodeBehind="PersonaBaja.aspx.cs" Inherits="CedServicios.Site.PersonaBaja" Theme="CedServicios" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="domicilio" Src="~/Controles/Domicilio.ascx" %>
<%@ Register TagPrefix="uc1" TagName="contacto" Src="~/Controles/Contacto.ascx" %>
<%@ Register TagPrefix="uc1" TagName="datosImpositivos" Src="~/Controles/DatosImpositivos.ascx" %>
<%@ Register TagPrefix="uc1" TagName="datosIdentificatorios" Src="~/Controles/DatosIdentificatorios.ascx" %>
<%@ Register TagPrefix="uc1" TagName="datosEmailAvisoComprobantePersona" Src="~/Controles/DatosEmailAvisoComprobantePersona.ascx" %>
<%@ Register TagPrefix="uc1" TagName="listaPrecioDefaultPersona" Src="~/Controles/ListaPrecioDefaultPersona.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceDefault" runat="server">
    <div class="container">
    <div class="row">
    <div class="col-lg-12 col-md-12">
    <table style="padding-left:10px;">
        <tr>
            <td align="center" colspan="2" style="padding-top:20px">
                <asp:Label ID="TituloPaginaLabel" runat="server" SkinID="TituloPagina" Text="Baja/Anul.baja de Persona"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right:5px; padding-top: 20px">
                <asp:Label ID="Label3" runat="server" Text="Persona perteneciente al CUIT"></asp:Label>
            </td>
            <td align="left" style="padding-top:20px">
                <asp:TextBox ID="CUITTextBox" runat="server" MaxLength="11" TabIndex="1" ToolTip="Debe ingresar sólo números." Width="90px"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label10" runat="server" Text="Tipo de Persona:"></asp:Label>
                <asp:RadioButton ID="ClienteRadioButton" Text="Cliente" GroupName="TipoPersona" runat="server" Enabled="false" />
                <asp:RadioButton ID="ProveedorRadioButton" Text="Proveedor" GroupName="TipoPersona" runat="server" Enabled="false" />
                <asp:RadioButton ID="AmbosRadioButton" Text="Ambos" GroupName="TipoPersona" runat="server" Enabled="false" />
            </td>
        </tr>
        <tr>
	        <td align="right" style="padding-right:5px; padding-top:5px">
		        <asp:Label ID="Label18" runat="server" Text="Tipo y Nro. de Documento"></asp:Label>
	        </td>
			<td align="left" style="padding-top:5px">
				<asp:DropDownList ID="TipoDocDropDownList" runat="server" TabIndex="2" 
                    Width="100px" DataValueField="Codigo" DataTextField="Descr" 
                    ToolTip="Para personas del exterior seleccione 'CUITPais'" AutoPostBack="true"
                    onselectedindexchanged="TipoDocDropDownList_SelectedIndexChanged" ></asp:DropDownList>
                <asp:TextBox ID="NroDocTextBox" runat="server" MaxLength="11" TabIndex="3" ToolTip="Debe ingresar sólo números." Width="90px" ></asp:TextBox>
                <asp:DropDownList ID="DestinosCuitDropDownList" runat="server" TabIndex="3" Width="306px" DataValueField="Codigo" DataTextField="Descr" Visible="false" ></asp:DropDownList>
			</td>
        </tr>
        <tr>
            <td align="right" style="padding-right:5px; padding-top:5px">
                <asp:Label ID="Label7" runat="server" Text="Id.Persona"></asp:Label>
            </td>
            <td align="left" style="padding-top:5px">
                <asp:TextBox ID="IdPersonaTextBox" runat="server" MaxLength="50" TabIndex="4" Width="300px"></asp:TextBox>
            </td>        
        </tr>
        <tr>
            <td colspan="2" style="padding-top:5px">
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height:1px; background-color:#cccccc">
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-top:5px">
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right:5px; padding-top:2px">
                <asp:Label ID="Label9" runat="server" Text="Razón Social"></asp:Label>
            </td>
            <td align="left" style="padding-top:2px">
                <asp:TextBox ID="RazonSocialTextBox" runat="server" MaxLength="50" TabIndex="5" Width="300px"></asp:TextBox>
            </td>        
        </tr>
        <uc1:domicilio ID="Domicilio" runat="server"/>
        <uc1:contacto ID="Contacto" runat="server" />
        <uc1:datosImpositivos ID="DatosImpositivos" runat="server" />
        <uc1:datosIdentificatorios ID="DatosIdentificatorios" runat="server" />
        <uc1:datosEmailAvisoComprobantePersona ID="DatosEmailAvisoComprobantePersona" runat="server" />
        <tr>
            <td colspan="2" style="padding-top:5px">
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right:5px">
                <asp:Label ID="Label38" runat="server" Text="Envío de <b>aviso</b> automático"></asp:Label><br />
                <asp:Label ID="Label46" runat="server" Text="<b>para visualización</b> del comprobante"></asp:Label><br />
                <asp:Label ID="Label4" runat="server" Text="(desde INTERFACTURAS)"></asp:Label>
            </td>
            <td style="border-style:solid; border-color:Gray; border-width:1px">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right" style="padding-right:5px; padding-top:3px">
                            <asp:Label ID="Label45" runat="server" Text="Email"></asp:Label>
                        </td>
                        <td align="left" style="padding-top:3px">
                            <asp:TextBox ID="EmailAvisoVisualizacionTextBox" runat="server" MaxLength="60" TabIndex="501"
                                ToolTip="A esta dirección se enviará un email de aviso para que el destinatario pueda visualizar el comprobante"
                                Width="315px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-left:5px; padding-right:5px; padding-top:3px">
                            <asp:Label ID="Label42" runat="server" Text="Contraseña"></asp:Label>
                        </td>
                        <td align="left" style="padding-top:3px">
                            <asp:TextBox ID="PasswordAvisoVisualizacionTextBox" runat="server" MaxLength="25" TabIndex="502"
                                ToolTip="Para poder acceder al contenido del comprobante, se solicitará al destinatario el ingreso de esta contraseña"
                                Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="justify" style="padding-left:5px; padding-right:5px; padding-top:5px; font-size:xx-small" colspan="2">
                            Interfacturas enviará, a última hora del día, un aviso (con un link) para que su destinatario pueda visualizar el comprobante generado.<br />Esta funcionalidad puede ser usada, por ejemplo, cuando, por cuestiones de seguridad, no se quiera enviar facturas por email.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <uc1:listaPrecioDefaultPersona ID="ListaPrecioDefaultPersona" runat="server" />
        <tr>
            <td>
            </td>
            <td align="left" style="height: 24px; padding-top:20px">
                <asp:Button ID="AceptarButton" runat="server" class="btn btn-default btn-sm" TabIndex="503" Text="Aceptar" onclick="AceptarButton_Click" />
                <asp:Button ID="SalirButton" runat="server" class="btn btn-default btn-sm" CausesValidation="false" TabIndex="504" Text="Cancelar" onclick="SalirButton_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="padding-top:20px">
                <asp:Label ID="MensajeLabel" runat="server" SkinID="MensajePagina" Text=""></asp:Label>
                <asp:ValidationSummary ID="MensajeValidationSummary" runat="server" SkinID="MensajeValidationSummary"></asp:ValidationSummary>
            </td>
        </tr>
    </table>
    </div>
    </div>
    </div>
</asp:Content>
