<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="status.aspx.cs" Inherits="ControlCambios.pages.services.status" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Control de Cambios</h2>
                        <p class="mb-md-0">Status del cambio</p>
                    </div>
                    <div class="d-flex">
                        <i class="mdi mdi-home text-muted hover-cursor"></i>
                        <p class="text-muted mb-0 hover-cursor">&nbsp;/&nbsp;Dashboard&nbsp;/&nbsp;</p>
                        <p class="text-primary mb-0 hover-cursor">Status del Cambios</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCrear" class="btn btn-primary mr-2" runat="server" Text="Crear nuevo" OnClick="BtnCrear_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Detalle del cambio No.
                        <asp:Label ID="LbCambio" runat="server" Text="Label"></asp:Label></h4>
                    <p class="card-description">
                        El cambio se ha creado con exito y esta en proceso de aprobación, estas son las personas notificadas para la autorización              
                    </p>
                    <div class="row">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdateGridView" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="GVNotificaciones" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pager"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        AutoGenerateColumns="false" OnRowCommand="GVNotificaciones_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    Notificación
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button ID="BtnEnviarCorreo" runat="server" Text="Enviar Notificación" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("usuario") %>' CommandName="EnviarRow" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                            <asp:BoundField DataField="estado" HeaderText="Estado" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <br />
                </div>
            </div>
        </div>
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
