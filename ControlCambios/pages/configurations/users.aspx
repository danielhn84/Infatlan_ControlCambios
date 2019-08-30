<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="ControlCambios.pages.configurations.users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
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
                            <asp:Button ID="BtnGuardarCambios" class="btn btn-primary mr-2" runat="server" Text="Guardar"  />
                            <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar"  />
                        
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="smartwizard">
        <ul>
            <li><a href="#step-1">Paso 1<br />
                <small>Informacion Usuario</small></a></li>
            <li><a href="#step-2">Paso 2<br />
                <small>Grupos</small></a></li>
            <li><a href="#step-3">Paso 3<br />
                <small>Finalizar</small></a></li>
        </ul>
        <div>
            <div id="step-1" class="">

            </div>
            <div id="step-2" class="">
            </div>
            <div id="step-2" class="">
            </div>
        </div>
    </div>

    <br />
    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Usuarios creados</h4>

                            <div class="row">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdateGridView" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false" >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                        <HeaderTemplate>
                                                            Acción
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnUsuarioEstado" runat="server" Text="Estado" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idUsuario") %>' CommandName="UsuarioEstado" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px" >
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
