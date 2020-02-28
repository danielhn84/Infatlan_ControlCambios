<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="ControlCambios.pages.services.search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <script type="text/javascript">
        function openModal() {
            $('#CerrarModal').modal('show');
        }
    </script>
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script>
        function GoBottom() {
            $('html,body').animate({ scrollTop: document.body.scrollHeight }, "fast");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #ffffff; opacity: 0.7; margin: 0;">
                <span style="display: inline-block; height: 100%; vertical-align: middle;"></span>
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="/images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="display: inline-block; vertical-align: middle;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="container col-md-12 grid-margin">
	    <div class="row align-top">
		    <ul class="breadcrumb">
			    <li class="completed"><a href="/default.aspx">Dashboard</a></li>
			    <li><a href="javascript:void(0);">Control de Cambios</a></li>
			    <li class="active"><a href="javascript:void(0);">Buscar</a></li>
		    </ul>
	    </div>
    </div>
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Control de Cambios</h2>
                        <p class="mb-md-0">Buscar cambios</p>
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
    <div class="row" id="DivBusquedaCampos" runat="server" visible="false">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Busqueda de cambios</h4>
                    <p class="card-description">
                        Busqueda por nombre o numero de cambio
                    </p>
                    <asp:Panel DefaultButton="BtnBuscarCambio" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
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
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Tipo Cambio</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="DDLTipoCambio" runat="server" class="form-control">
                                                    <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                    <asp:ListItem Value="1">Estándar</asp:ListItem>
                                                    <asp:ListItem Value="2">Planificado / Normal</asp:ListItem>
                                                    <asp:ListItem Value="3">Emergencia</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6" runat="server" id="DivFecha" visible="false">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Fecha</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxFechaCambio" placeholder="" class="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Estado</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="DDLCambioEstado" runat="server" class="form-control">
                                                    <asp:ListItem Value="100">Selecione una Opción</asp:ListItem>
                                                    <asp:ListItem Value="0">Correciones Promotor</asp:ListItem>
                                                    <asp:ListItem Value="1">Revisión QA</asp:ListItem>
                                                    <asp:ListItem Value="2">CAB Manager</asp:ListItem>
                                                    <asp:ListItem Value="3">Implementación</asp:ListItem>
                                                    <asp:ListItem Value="4">Revisión Implementador</asp:ListItem>
                                                    <asp:ListItem Value="5">Cambio terminado / No cerrado</asp:ListItem>
                                                    <asp:ListItem Value="6">Cambio cerrado</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>

                    <div class="row">
                        <div class="d-flex justify-content-between align-items-end flex-wrap">
                            <asp:UpdatePanel ID="UpdatePrincipalConfirmacion" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="BtnBuscarCambio" class="btn btn-facebook mr-2" runat="server" Text="Buscar Cambios" OnClick="BtnBuscarCambio_Click" />
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
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false" OnRowCommand="GVBusqueda_RowCommand"
                                                AllowPaging="true"
                                                GridLines="None"
                                                PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                        <HeaderTemplate>
                                                            
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnEntrar" runat="server" Text="Entrar" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idcambio") %>' CommandName="EntrarCambio" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px" Visible="false">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnCerrarCambio" runat="server" Text="Finalizar" class="btn btn-google mr-2" CommandArgument='<%# Eval("idcambio") %>' CommandName="CerrarCambio" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnDocumento" runat="server" Text="Download" class="btn btn-inverse-success mr-2 " CommandArgument='<%# Eval("idcambio") %>' CommandName="DescargarDocumento" >
                                                                <i class="mdi mdi-download text-success"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="idcambio" HeaderText="No." />
                                                    <asp:BoundField DataField="mantenimientoNombre" HeaderText="Nombre" />
                                                    <asp:BoundField DataField="fechaSolicitud" HeaderText="Fecha Solicitud" />
                                                    <asp:BoundField DataField="fechaImplementacion" HeaderText="Implementación" />
                                                    <asp:BoundField DataField="idUsuarioResponsable" HeaderText="Asignado" Visible="false" />
                                                    <asp:BoundField DataField="estado" HeaderText="Estado" />
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

            <div id="DIVDivisor" runat="server"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="CerrarModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdateLabelCambio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelUsuario">Cerrar cambio - Paso Final
                                    <asp:Label ID="LbNumeroCambio" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateCerrar" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">

                                <div class="col-sm-12">
                                    ¿Estas seguro que deseas dar por terminado el cambio?
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateCerrarMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbCerrarMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateUsuarioBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnCerrarCambio" runat="server" Text="Cerrar cambio" class="btn btn-primary" OnClick="BtnCerrarCambio_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
