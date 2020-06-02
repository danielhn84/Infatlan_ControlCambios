<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="control.aspx.cs" Inherits="ControlCambios.pages.configurations.control" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
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
                <li class="active"><a href="javascript:void(0);">Control de Cambios</a></li>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Control de Cambios</h2>
                        <p class="mb-md-0">Configuraciones administrativas</p>
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
            <a class="nav-item nav-link active" id="autorizar_tab" data-toggle="tab" href="#autorizador" role="tab" aria-controls="nav-home" aria-selected="true">Autorizar</a>
            <a class="nav-item nav-link" id="pasos_tab" data-toggle="tab" href="#pasos" role="tab" aria-controls="nav-profile" aria-selected="false">Pasos</a>
            <a class="nav-item nav-link" id="tipoCambio_tab" data-toggle="tab" href="#tipoCambio" role="tab" aria-controls="nav-profile" aria-selected="false">Tipo cambio (STD, NOR, EME)</a>
        </div>
    </nav>
    <br />
    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="autorizador" role="tabpanel" aria-labelledby="autorizar_tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Autorizar cambio</h4>
                            <p class="card-description">
                                Por favor ingrese el cambio a autorizar
                            </p>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">ID Cambio</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxAutorizarCambio" placeholder="Buscar: No. Cambio" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button ID="BtnAutorizarBuscar" class="btn btn-primary mr-2" runat="server" Text="Buscar" />
                                                </div>
                                            </div>

                                            <div class="form-group row" runat="server" id="DivAutorizar" visible="false">
                                                <label class="col-sm-3 col-form-label">Nombre</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxAutorizarNombre" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off" ReadOnly="true"></asp:TextBox>
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
                            <h4 class="card-title">¿Estas seguro de autorizar este cambio?</h4>
                            <p class="card-description">
                                Presionar le botón crear para proceder con la acción
                            </p>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <asp:UpdatePanel ID="UpdateUsuarios" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnAutorizarCambios" class="btn btn-primary mr-2" runat="server" Text="Autorizar" />
                                                <asp:Button ID="BtnAutorizarCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" />
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
        <div class="tab-pane fade" id="pasos" role="tabpanel" aria-labelledby="pasos_tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Pasos de cambio</h4>
                            <p class="card-description">
                                Por favor ingrese el cambio a cambiar de paso
                            </p>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">ID Cambio</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxPasosCambio" placeholder="Buscar: No. Cambio" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button ID="BtnPasosBuscar" class="btn btn-primary mr-2" runat="server" Text="Buscar" />
                                                </div>
                                            </div>

                                            <div class="form-group row" runat="server" id="DivPasosBuscar" visible="false">
                                                <label class="col-sm-3 col-form-label">Nombre</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxPasosNombre" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row" runat="server" id="DivPasosOpciones" visible="false">
                                                <label class="col-sm-3 col-form-label">Paso</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLPasosOpciones" runat="server" class="form-control" AutoPostBack="True" >
                                                        <asp:ListItem Value="0">Promotor</asp:ListItem>
                                                        <asp:ListItem Value="1">QA</asp:ListItem>
                                                        <asp:ListItem Value="2">CAB Manager</asp:ListItem>
                                                        <asp:ListItem Value="3">Implementación</asp:ListItem>
                                                        <asp:ListItem Value="4">Revisión Implementación</asp:ListItem>
                                                        <asp:ListItem Value="5">Confirmación Cierre</asp:ListItem>
                                                        <asp:ListItem Value="6">Cerrado</asp:ListItem>
                                                    </asp:DropDownList>
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
                            <h4 class="card-title">¿Estas seguro de cambiar de paso a este cambio?</h4>
                            <p class="card-description">
                                Presionar el botón cambiar para proceder con la acción
                            </p>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnPasosCambios" class="btn btn-primary mr-2" runat="server" Text="Cambiar" />
                                                <asp:Button ID="BtnPasosCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" />
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
        <div class="tab-pane fade" id="tipoCambio" role="tabpanel" aria-labelledby="tipoCambio_tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Tipo de cambio</h4>
                            <p class="card-description">
                                Por favor ingrese el cambio a cambiar de tipo
                            </p>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">ID Cambio</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxTipoCambio" placeholder="Buscar: No. Cambio" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button ID="BtnTipoBuscar" class="btn btn-primary mr-2" runat="server" Text="Buscar" />
                                                </div>
                                            </div>

                                            <div class="form-group row" runat="server" id="DivTipoBuscar" visible="false">
                                                <label class="col-sm-3 col-form-label">Nombre</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxTipoNombre" class="form-control" runat="server" TextMode="SingleLine" autocomplete="off" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row" runat="server" id="DivTipoOpciones" visible="false">
                                                <label class="col-sm-3 col-form-label">Tipo</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLTipoOpciones" runat="server" class="form-control" AutoPostBack="True" >
                                                        <asp:ListItem Value="1">Estandar</asp:ListItem>
                                                        <asp:ListItem Value="2">Normal</asp:ListItem>
                                                        <asp:ListItem Value="3">Emergencia</asp:ListItem>
                                                    </asp:DropDownList>
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
                            <h4 class="card-title">¿Estas seguro de cambiar de tipo a este cambio?</h4>
                            <p class="card-description">
                                Presionar el botón cambiar para proceder con la acción
                            </p>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnTipoCambios" class="btn btn-primary mr-2" runat="server" Text="Cambiar" />
                                                <asp:Button ID="BtnTipoCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" />
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
