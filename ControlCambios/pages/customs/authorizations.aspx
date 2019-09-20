<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="authorizations.aspx.cs" Inherits="ControlCambios.pages.customs.authorizations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />

    <script type="text/javascript">
    function openModal() {
        $('#AutorizarModal').modal('show');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Autorización de Cambio</h2>
                        <p class="mb-md-0">Buscar cambios</p>
                    </div>
                    <div class="d-flex">
                        <i class="mdi mdi-home text-muted hover-cursor"></i>
                        <p class="text-muted mb-0 hover-cursor">&nbsp;/&nbsp;Dashboard&nbsp;/&nbsp;</p>
                        <p class="text-primary mb-0 hover-cursor">Autorización de Cambios</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnCrear" class="btn btn-primary mr-2" runat="server" Text="Crear nuevo" OnClick="BtnCrear_Click"  />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="row" >
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Busqueda de cambios</h4>
                    <p class="card-description">
                        Los cambios solo apareceran si te fuerón asignados para autorización.
                    </p>
                    <asp:Panel DefaultButton="BtnBuscarCambio" runat="server">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Numero</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="TxBuscarNumero" placeholder="Ej. 1000" class="form-control" runat="server" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Nombre</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="TxBuscarNombre" placeholder="Ej. Mantenimiento" class="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="d-flex justify-content-between align-items-end flex-wrap">
                            <asp:UpdatePanel ID="UpdatePrincipalConfirmacion" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="BtnBuscarCambio" class="btn btn-facebook mr-2" runat="server" Text="Buscar Cambios" OnClick="BtnBuscarCambio_Click"  />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row" id="DivBusqueda" runat="server" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Resultados de busqueda</h4>

                            <div class="row">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdateGridView" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false" OnRowCommand="GVBusqueda_RowCommand" >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                        <HeaderTemplate>
                                                            Acción
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnEntrar" runat="server" Text="Entrar" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idcambio") %>' CommandName="EntrarCambio" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                        <HeaderTemplate>
                                                            Acción
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnAutorizar" runat="server" Text="Autorizar" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idcambio") %>' CommandName="AutorizarCambio" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                            
                                                    <asp:BoundField DataField="idcambio" HeaderText="No.Cambio" />
                                                    <asp:BoundField DataField="mantenimientoNombre" HeaderText="Nombre" />
                                                    <asp:BoundField DataField="fechaSolicitud" HeaderText="Fecha" />
                                                    <asp:BoundField DataField="idUsuarioResponsable" HeaderText="Asignado" Visible="false"/>
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="AutorizarModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    
                        <asp:UpdatePanel ID="UpdateLabelCambio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <h4 class="modal-title" id="ModalLabelUsuario">Autorización de Cambio 
                                    <asp:Label ID="LbNumeroCambio" runat="server" Text=""></asp:Label>
                                </h4>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateAutorizar" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                ¿Estas seguro de autorizar este cambio?
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateAutorizarMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbAutorizarMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateUsuarioBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnAutorizarCambio" runat="server" Text="Autorizar" class="btn btn-primary" OnClick="BtnAutorizarCambio_Click"  />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
