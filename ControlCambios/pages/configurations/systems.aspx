<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="systems.aspx.cs" Inherits="ControlCambios.pages.configurations.systems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <asp:Button ID="BtnAgregarSistema" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#SistemasModal" Text="(+) Sistema" />
        </div>
    </div>

    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Sistemas / Servicios afectados</h4>
                    <p class="col-form-label">Ingrese los servicios que se van a cambiar</p>

                    <div class="table-responsive">
                        <asp:GridView ID="GVSistemas" runat="server"
                            CssClass="mydatagrid"
                            PagerStyle-CssClass="pager"
                            HeaderStyle-CssClass="header"
                            RowStyle-CssClass="rows"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                    <HeaderTemplate>
                                        Acción
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="BtnSistemasDelete" runat="server" Text="borrar" class="btn btn-dark mr-2" CommandArgument='<%# Eval("sistema") %>' CommandName="DeleteRow" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="sistema" HeaderText="Nombre" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                                <asp:BoundField DataField="denegacion" HeaderText="Denegación" />
                                <asp:BoundField DataField="inicio" HeaderText="Inicio" />
                                <asp:BoundField DataField="final" HeaderText="Final" />
                            </Columns>
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Datos de Equipo</h4>
                    <p class="col-form-label">Ingrese los equipos en donde se desea instalar los cambios</p>

                    <div class="table-responsive">
                        <asp:GridView ID="GVEquipos" runat="server"
                            CssClass="mydatagrid"
                            PagerStyle-CssClass="pager"
                            HeaderStyle-CssClass="header"
                            RowStyle-CssClass="rows"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                    <HeaderTemplate>
                                        Acción
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="BtnEquipoDelete" runat="server" Text="borrar" class="btn btn-dark mr-2" CommandArgument='<%# Eval("idCatEquipo") %>' CommandName="DeleteRow" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="ip" HeaderText="Dirección IP" />
                                <asp:BoundField DataField="tipoEquipo" HeaderText="Tipo de Equipo" />
                            </Columns>
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
