﻿<%@ Page Title="" Language="C#" MasterPageFile="~/CedServicios.Master" AutoEventWireup="true" CodeBehind="SolicPermisoAdminUN.aspx.cs" Inherits="CedServicios.Site.SolicPermisoAdminUN" Theme="CedServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceDefault" runat="server">
    <div class="container">
    <div class="row">
    <div class="col-lg-12 col-md-12">
    <table align="center">
        <tr>
            <td align="center" colspan="3" style="padding-top: 20px">
                <asp:Label ID="Label1" runat="server" SkinID="TituloPagina" Text="Solicitud permiso de administrador de Unidad de Negocio"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right: 5px; padding-top: 20px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server"
                    ControlToValidate="CUITTextBox" ErrorMessage="CUIT" SetFocusOnError="True" ValidationExpression="[0-9]{11}">
                    <asp:Label ID="Label45" runat="server" SkinID="IndicadorValidacion"></asp:Label>
                </asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="CUITTextBox"
                    ErrorMessage="CUIT" SetFocusOnError="True">
                    <asp:Label ID="Label46" runat="server" SkinID="IndicadorValidacion"></asp:Label>
                </asp:RequiredFieldValidator>
                <asp:Label ID="Label19" runat="server" Text="CUIT"></asp:Label>
            </td>
            <td align="left" style="padding-top: 20px">
                <asp:TextBox ID="CUITTextBox" runat="server" MaxLength="11" TabIndex="1" ToolTip="Debe ingresar sólo números."
                    Width="90px"></asp:TextBox>
                <asp:Button ID="LeerListaUNsButton" runat="server" OnClick="LeerListaUNsButton_Click" TabIndex="2"
                    Text="Leer unidad(es) de negocio del CUIT" />
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right:5px; padding-top:5px; height:25px;">
                <asp:Label ID="Label5" runat="server" Text="Unidad de Negocio"></asp:Label>
            </td>
            <td align="left" style="padding-top:5px; height:25px;">
                <asp:DropDownList ID="IdUNDropDownList" runat="server" TabIndex="3" Width="183px" DataValueField="Id" DataTextField="Descr" >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left" style="height: 24px; padding-top: 20px">
                <asp:Button ID="SolicitarButton" runat="server" OnClick="SolicitarButton_Click" TabIndex="3" Text="Solicitar" />
                <asp:Button ID="SalirButton" runat="server" CausesValidation="false" TabIndex="4" Text="Cancelar" onclick="SalirButton_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="padding-bottom: 30px; padding-top: 20px">
                <asp:Label ID="MensajeLabel" runat="server" SkinID="MensajePagina" Text=""></asp:Label>
                <asp:ValidationSummary ID="MensajeValidationSummary" runat="server" SkinID="MensajeValidationSummary">
                </asp:ValidationSummary>
            </td>
        </tr>
    </table>
    </div>
    </div>
    </div>
</asp:Content>
