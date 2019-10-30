<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="generals.aspx.cs" Inherits="ControlCambios.pages.configurations.generals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <script type="text/javascript">
        function openEquipoModificacionesModal() {
            $('#EquipoModificacionModal').modal('show');
        }
        function openEquipoEstadoModal() {
            $('#EquipoEstadoModal').modal('show');
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
    <div class="container col-md-12 grid-margin">
	    <div class="row align-top">
		    <ul class="breadcrumb">
			    <li class="completed"><a href="/default.aspx">Dashboard</a></li>
                <li class="completed"><a href="javascript:void(0);">Configuraciones</a></li>
			    <li class="active"><a href="javascript:void(0);">Generales</a></li>
		    </ul>
	    </div>
    </div>
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Control de Equipos / Sistemas</h2>
                        <p class="mb-md-0">Configuraciones</p>
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
            <a class="nav-item nav-link active" id="nav_datos_tab" data-toggle="tab" href="#equipos" role="tab" aria-controls="nav-home" aria-selected="true">Creación de Equipo</a>
            <a class="nav-item nav-link" id="nav_tecnicos_tab" data-toggle="tab" href="#sistemas" role="tab" aria-controls="nav-profile" aria-selected="false">Creación de Sistemas</a>
        </div>
    </nav>
    <br />

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="equipos" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Información del Equipo</h4>
                            <p class="card-description">
                                Por favor ingrese todos los siguientes campos
                            </p>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Nombre</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxEquipoNombre" placeholder="Ej. NX00000-A.infatlan.hn" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Tipo Equipo</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxEquipoTipo" placeholder="Ej. Cisco" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">IP</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxEquipoIp" placeholder="Ej. 192.168.1.1" class="form-control" runat="server" TextMode="SingleLine" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Ubicacion</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxEquipoUbicacion" placeholder="Ej. Data center SONISA" class="form-control" runat="server" TextMode="SingleLine" ></asp:TextBox>
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
                            <h4 class="card-title">¿Estas seguro de crear este equipo?</h4>
                            <p class="card-description">
                                Presionar le botón crear para proceder con la acción
                            </p>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <asp:UpdatePanel ID="UpdateUsuarios" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnEquipoGuardar" class="btn btn-primary mr-2" runat="server" Text="Crear" OnClick="BtnEquipoGuardar_Click" />
                                                <asp:Button ID="BtnEquipoCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnEquipoCancelar_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Equipos creados</h4>
                             <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Buscar</label>
                                    <div class="col-sm-9">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxBuscarEquipo" runat="server" placeholder="Ej. 150.150.1.1 / BASA" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscarEquipo_TextChanged"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdateGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVBusquedaEquipos" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                AllowSorting="True"
                                                PageSize="10" OnPageIndexChanging="GVBusquedaEquipos_PageIndexChanging" OnRowCommand="GVBusquedaEquipos_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px" >
                                                        <HeaderTemplate>
                                                            Acción
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnEquipoEstado" runat="server" Text="Estado" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idCatEquipo") %>' CommandName="EquipoEstado" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnEquipoModificar" runat="server" Text="Modificar" class="btn btn-google mr-2" CommandArgument='<%# Eval("idCatEquipo") %>' CommandName="EquipoModificar" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="idCatEquipo" HeaderText="Id" />
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre"  />
                                                    <asp:BoundField DataField="tipoEquipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="ip" HeaderText="IP" />
                                                    <asp:BoundField DataField="ubicacion" HeaderText="Ubicación" />
                                                </Columns>
                                                 
                        
                                                <PagerStyle  HorizontalAlign="Center" />  
                                                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />  
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />  
                                                <SortedAscendingHeaderStyle BackColor="#808080" />  
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />  
                                                <SortedDescendingHeaderStyle BackColor="#383838" />  
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade show" id="sistemas" role="tabpanel" aria-labelledby="nav_tecnicos_tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Información del Sistema</h4>
                            <p class="card-description">
                                Por favor ingrese todos los siguientes campos
                            </p>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Equipo</label>
                                                <div class="col-sm-9">
                                                   <asp:DropDownList ID="DDLSistemaEquipo" runat="server" class="form-control"  AutoPostBack="True">
                                                   </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Sistema</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxSistemaNombre" placeholder="Ej. ocb.bancatlan.hn" class="form-control" runat="server" TextMode="SingleLine" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Descripción</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxSistemaDescripcion" placeholder="Ej. ..." class="form-control" runat="server" TextMode="SingleLine" ></asp:TextBox>
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
                            <h4 class="card-title">¿Estas seguro de crear este sistema?</h4>
                            <p class="card-description">
                                Presionar le botón crear para proceder con la acción
                            </p>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnSistemaGuardar" class="btn btn-primary mr-2" runat="server" Text="Crear" OnClick="BtnSistemaGuardar_Click" />
                                                <asp:Button ID="BtnSistemaCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnSistemaCancelar_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Sistemas creados</h4>

                            <div class="row">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVBusquedaSistemas" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                PageSize="10" OnPageIndexChanging="GVBusquedaSistemas_PageIndexChanging" OnRowCommand="GVBusquedaSistemas_RowCommand" >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px" Visible="false">
                                                        <HeaderTemplate>
                                                            Acción
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnSistemaEstado" runat="server" Text="Estado" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idCatSistemas") %>' CommandName="UsuarioEstado" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px" Visible="false">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnSistemaModificar" runat="server" Text="Modificar" class="btn btn-google mr-2" CommandArgument='<%# Eval("idCatSistemas") %>' CommandName="UsuarioModificar" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="idCatSistemas" HeaderText="Id" />
                                                    <asp:BoundField DataField="idCatEquipo" HeaderText="Equipo" />
                                                    <asp:BoundField DataField="sistema" HeaderText="Sistema" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
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
        </div>
    </div>
    <%--MODAL DE EQUIPO ESTADO--%>
    <div class="modal fade" id="EquipoEstadoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelEstado">Estado del Equipo 
                        <asp:Label ID="LbEquipoEstado" runat="server" Text=""></asp:Label></h4>
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
                            <asp:Button ID="BtnEquipoEstado" runat="server" Text="Modificar" class="btn btn-primary" OnClick="BtnEquipoEstado_Click"  />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%--MODAL DE MODIFICACION--%>
    <div class="modal fade" id="EquipoModificacionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelModificacion">Modificar Equipo <asp:Label ID="LbEquipoModificar" runat="server" Text=""></asp:Label></h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateModificarUsuario" runat="server">
                        <ContentTemplate>
                   
                            
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Dirección Ip</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxEquipoModificarIp" placeholder="Ej. 192.168.0.1" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateModificacionBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnEquipoModificar" runat="server" Text="Modificar" class="btn btn-primary" OnClick="BtnEquipoModificar_Click" />
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
