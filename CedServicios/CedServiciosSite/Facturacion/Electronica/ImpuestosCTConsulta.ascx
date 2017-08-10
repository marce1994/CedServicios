<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpuestosCTConsulta.ascx.cs" Inherits="CedServicios.Site.Facturacion.Electronica.ImpuestosCTConsulta" %>

		<table style="width:1282px">
			<tr>
				<td style="height: 10px;">
				</td>
			</tr>
			<tr>
				<td class="TextoResaltado" style="text-align: center;">
					IMPUESTOS GLOBALES
				</td>
			</tr>
			<tr>
				<td style="height: 10px;">
				</td>
			</tr>
			<tr>
				<td style="text-align: center; font-weight: normal;">
					<asp:UpdatePanel ID="impuestosUpdatePanel" runat="server" ChildrenAsTriggers="true"
						UpdateMode="Conditional">
						<ContentTemplate>
							<asp:GridView ID="impuestosGridView" runat="server" AutoGenerateColumns="False" BorderColor="gray"
								BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" EnableViewState="true" Font-Bold="false" GridLines="Both"
								CssClass="gridview" ShowFooter="true" ShowHeader="True" ToolTip="El separador de decimales a utilizar es el punto" 
								Width="1260px">
								<Columns>
									<asp:TemplateField HeaderText="C&#243;digo del impuesto">
										<ItemTemplate>
											<asp:Label ID="lblcodigo_impuesto" runat="server" Text='<%# Eval("descripcion") %>'
												Width="250px"></asp:Label>
										</ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
										<ItemStyle HorizontalAlign="Left" Width="350px" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Importe total">
										<ItemTemplate>
											<asp:Label ID="lblimporte_impuesto" runat="server" Width="60px" Text='<%# Eval("importe_impuesto") %>'></asp:Label>
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Right" Width="160px" />
										<HeaderStyle HorizontalAlign="Center" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Jurisdicci�n">
										<ItemTemplate>
											<asp:Label ID="lbljurisdiccion" runat="server" Text='<%# GetJurisdiccion((int)Eval("codigo_jurisdiccion")) %>'
												Width="160px"></asp:Label>
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Left" Width="160px" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Al�cuota %">
										<ItemTemplate>
											<asp:Label ID="lblalicuota" runat="server" Width="100px" Text='<%# GetAlicuota((double)Eval("porcentaje_impuesto")) %>'></asp:Label>
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Right"  Width="100px"/>
										<HeaderStyle Width="50px" />
									</asp:TemplateField>
								</Columns>
								<HeaderStyle Font-Bold="False" />
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
				<td style="text-align: center; height: 10px;">
					<asp:UpdateProgress ID="impuestosUpdateProgress" runat="server" AssociatedUpdatePanelID="impuestosUpdatePanel"
						DisplayAfter="0">
						<ProgressTemplate>
							<asp:Image ID="impuestosImage" runat="server" Height="25px" ImageUrl="~/Imagenes/301.gif">
							</asp:Image>
						</ProgressTemplate>
					</asp:UpdateProgress>
				</td>
			</tr>
		</table>
