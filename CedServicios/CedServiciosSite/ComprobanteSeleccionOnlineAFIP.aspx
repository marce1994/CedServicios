﻿<%@ Page Title="" Language="C#" MasterPageFile="~/CedServicios.Master" AutoEventWireup="true" CodeBehind="ComprobanteSeleccionOnlineAFIP.aspx.cs" Inherits="CedServicios.Site.ComprobanteSeleccionOnlineAFIP" Theme="CedServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceDefault" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="padding-left:10px">
        <tr>
            <td align="center" colspan="2" style="padding-top:20px">
                <asp:Label ID="TituloPaginaLabel" runat="server" SkinID="TituloPagina" Text="Consulta de Comprobantes (online Interfacturas)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-top:20px">
                Tipo Comprobante:
            </td>
            <td align="left" style="padding-top:20px; padding-left:5px">
                <asp:DropDownList ID="TipoComprobanteDropDownList" runat="server" AutoPostBack="True" SkinID="ddlch" ToolTip="">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-top:20px">
                Nro. Comprobante:
            </td>
            <td align="left" style="padding-top:20px; padding-left:5px">
                <asp:TextBox ID="NroComprobanteTextBox" runat="server" ToolTip="">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-top:10px">
                Punto de Venta:
            </td>
            <td align="left" style="padding-top:10px; padding-left:5px">
                <asp:DropDownList ID="PtoVtaConsultaDropDownList" runat="server" AutoPostBack="True" SkinID="ddlch" ToolTip="">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-top:10px">
                Cuit:
            </td>
            <td align="left" style="padding-top:10px; padding-left:5px">
                <asp:TextBox ID="CuitConsultaTextBox" ReadOnly="true" runat="server" ToolTip="Cuit del Vendedor.">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-top:20px">
                <asp:Button ID="ConsultarLoteAFIPButton" runat="server"
                    OnClick="ConsultarLoteAFIPButton_Click" Text="Consultar lote en AFIP"
                    ToolTip="Consultar el comprobante en AFIP. Es un servicio On-Line para el cual se requiere un certificado de autenticación."
                    Width="100%" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="padding-top:20px">
                <asp:Label ID="MensajeLabel" runat="server" SkinID="MensajePagina" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
