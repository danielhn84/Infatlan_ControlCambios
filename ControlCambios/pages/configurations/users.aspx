<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="ControlCambios.pages.configurations.users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />

    <script type="text/javascript">
        function openModificacionesModal() {
            $('#ModificacionModal').modal('show');
        }
        function openEstadoModal() {
            $('#EstadoModal').modal('show');
        }

        var url = document.location.toString();
        if (url.match('#')) {
            $('.nav-tabs a[href="#' + url.split('#')[1] + '"]').tab('show');
        }

        // Change hash for page-reload
        $('.nav-tabs a').on('shown.bs.tab', function (e) {
            window.location.hash = e.target.hash;
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Control de Usuarios</h2>
                        <p class="mb-md-0">Configuraciones</p>
                    </div>
                    <div class="d-flex">
                        <i class="mdi mdi-home text-muted hover-cursor"></i>
                        <p class="text-muted mb-0 hover-cursor">&nbsp;/&nbsp;Dashboard&nbsp;/&nbsp;</p>
                        <p class="text-primary mb-0 hover-cursor">Configuraciones de Usuario</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-usuarios" role="tab" aria-controls="nav-home" aria-selected="true">Creación de Usuarios</a>
            <a class="nav-item nav-link" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-buscar" role="tab" aria-controls="nav-profile" aria-selected="false">Modificar Usuarios</a>
        </div>
    </nav>
    <br />
    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-usuarios" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div id="smartwizard">
                <ul>
                    <li><a href="#step-1">Paso 1<br />
                        <small>Informacion Usuario</small></a></li>
                    <li><a href="#step-2">Paso 3<br />
                        <small>Finalizar</small></a></li>
                </ul>
                <div>
                    <div id="step-1" class="">
                        <div class="row">
                            <div class="col-12 grid-margin stretch-card">
                                <div class="card">
                                    <div class="card-body">
                                        <h4 class="card-title">Selección de Cargo</h4>
                                        <p class="card-description">
                                            Por favor seleccione un cargo para el usuario
                                        </p>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="row">

                                                    <div class="col-md-6">

                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Usuario</label>
                                                            <div class="col-sm-9">
                                                                <asp:DropDownList ID="DDLCargo" runat="server" class="form-control" OnSelectedIndexChanged="DDLCargo_SelectedIndexChanged" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                                    <asp:ListItem Value="2">Supervisor</asp:ListItem>
                                                                    <asp:ListItem Value="3">Quality Assurance</asp:ListItem>
                                                                    <asp:ListItem Value="4">Implementador</asp:ListItem>
                                                                    <asp:ListItem Value="5">Promotor</asp:ListItem>
                                                                    <asp:ListItem Value="6">CAB Manager</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group row" id="DIVSupervisores" runat="server" visible="false">
                                                            <label class="col-sm-3 col-form-label">Supervisor</label>
                                                            <div class="col-sm-9">
                                                                <asp:DropDownList ID="DDLSupervisor" runat="server" class="form-control"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
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
                                        <h4 class="card-title">Información del nuevo usuario</h4>
                                        <p class="card-description">
                                            Por favor ingrese todos los siguientes campos
                                        </p>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Usuario</label>
                                                            <div class="col-sm-9">
                                                                <asp:TextBox ID="TxUsuario" placeholder="Ej. admin" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Correo</label>
                                                            <div class="col-sm-9">
                                                                <asp:TextBox ID="TxCorreo" placeholder="Ej. admin@banctlan.hn" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Password</label>
                                                            <div class="col-sm-9">
                                                                <asp:TextBox ID="TxPassword" placeholder="xxxxxx" class="form-control" runat="server" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Confirmar</label>
                                                            <div class="col-sm-9">
                                                                <asp:TextBox ID="TxPasswordConfirmacion" placeholder="xxxxxx" class="form-control" runat="server" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Nombres</label>
                                                            <div class="col-sm-9">
                                                                <asp:TextBox ID="TxNombres" placeholder="Ej. Juan Jose" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Apellidos</label>
                                                            <div class="col-sm-9">
                                                                <asp:TextBox ID="TxApellidos" placeholder="Ej. Perez Perez" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="step-2" class="">
                        <div class="row">
                            <div class="col-12 grid-margin stretch-card">
                                <div class="card">
                                    <div class="card-body">
                                        <h4 class="card-title">¿Estas seguro de crear este usuario?</h4>
                                        <p class="card-description">
                                            Presionar le botón crear para proceder con la acción
                                        </p>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <asp:UpdatePanel ID="UpdateUsuarios" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="BtnGuardarCambios" class="btn btn-primary mr-2" runat="server" Text="Crear" OnClick="BtnGuardarCambios_Click" />
                                                            <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="nav-buscar" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Usuarios creados</h4>

                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel ID="UpdateGridView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GVBusqueda" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pgr"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false"
                                                        AllowPaging="true"
                                                        PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging" OnRowCommand="GVBusqueda_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                                <HeaderTemplate>
                                                                    Acción
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnUsuarioEstado" runat="server" Text="Estado" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idUsuario") %>' CommandName="UsuarioEstado" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                                <HeaderTemplate>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnUsuarioModificar" runat="server" Text="Modificar" class="btn btn-google mr-2" CommandArgument='<%# Eval("idUsuario") %>' CommandName="UsuarioModificar" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="idUsuario" HeaderText="Usuario" />
                                                            <asp:BoundField DataField="nombres" HeaderText="Nombres" />
                                                            <asp:BoundField DataField="apellidos" HeaderText="Apellidos" />
                                                            <asp:BoundField DataField="correo" HeaderText="Correo" />
                                                            <asp:BoundField DataField="estado" HeaderText="Estado" />
                                                            <asp:BoundField DataField="perfil" HeaderText="Perfil" />
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
        </div>
    </div>
    <%--MODAL DE ESTADO--%>
    <div class="modal fade" id="EstadoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelEstado">Estado del Usuario <asp:Label ID="LbUsuario" runat="server" Text=""></asp:Label></h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-sm-3 col-form-label">Estado</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="DDLEstado" runat="server" class="form-control">
                                    <asp:ListItem Value="0">Seleccione una opción</asp:ListItem>
                                    <asp:ListItem Value="true">Activo</asp:ListItem>
                                    <asp:ListItem Value="false">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateEstadoBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnEstado" runat="server" Text="Modificar" class="btn btn-primary" OnClick="BtnEstado_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%--MODAL DE MODIFICACION--%>
    <div class="modal fade" id="ModificacionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%);">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 800px; top: 200px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelModificacion">Modificar Usuario</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateModificarUsuario" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Usuario</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModificarUsuario" placeholder="Ej. admin" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Correo</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModificarCorreo" placeholder="Ej. admin@banctlan.hn" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Password</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModificarPassword" placeholder="xxxxxx" class="form-control" runat="server" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Confirmar</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModificarPasswordConfirmar" placeholder="xxxxxx" class="form-control" runat="server" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Nombres</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModificarNombres" placeholder="Ej. Juan Jose" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Apellidos</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModificarApellidos" placeholder="Ej. Perez Perez" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdateUsuarioMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbUsuarioMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateModificacionBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnModificar" runat="server" Text="Modificar" class="btn btn-primary" OnClick="BtnModificar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="/js/jquery.smartWizard.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            // Toolbar extra buttons
            var btnFinish = $('<button></button>').text('Finish')
                .addClass('btn btn-info')
                .on('click', function () { alert('Finish Clicked'); });
            var btnCancel = $('<button></button>').text('Cancel')
                .addClass('btn btn-danger')
                .on('click', function () { $('#smartwizard').smartWizard("reset"); });

            // Smart Wizard
            $('#smartwizard').smartWizard({
                selected: 0,
                theme: 'arrows',
                transitionEffect: 'fade',
                toolbarSettings: {
                    toolbarPosition: 'top'
                }
            });
        });
    </script>

    <script>
        function NextButton() {
            $('#smartwizard').smartWizard("next");
            return true;
        }
    </script>
    <script>
        $(document).ready(function () {
            $(this).scrollTop(0);
        });
    </script>
</asp:Content>
