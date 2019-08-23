<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="logs.aspx.cs" Inherits="ControlCambios.pages.logs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Logs</h2>
                        <p class="mb-md-0">Informes del Sistema</p>
                    </div>
                    <div class="d-flex">
                        <i class="mdi mdi-home text-muted hover-cursor"></i>
                        <p class="text-muted mb-0 hover-cursor">&nbsp;/&nbsp;Dashboard&nbsp;/&nbsp;</p>
                        <p class="text-primary mb-0 hover-cursor">Informes del Sistema</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnLimpiarLogs" class="btn btn-primary mr-2" runat="server" Text="Limpiar" OnClick="BtnLimpiarLogs_Click" />
                            <asp:Button ID="BtnSalir" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnSalir_Click" />
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
                    <h4 class="card-title">Logs de sistema</h4>
                    <div class="row">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdateGridView" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="GVLog" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pager"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="idlog" HeaderText="No." />
                                            <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                                            <asp:BoundField DataField="lugar" HeaderText="Lugar" />
                                            <asp:BoundField DataField="mensaje" HeaderText="Mensaje" />
                                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
