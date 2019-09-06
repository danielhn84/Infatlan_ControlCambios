<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="changes.aspx.cs" Inherits="ControlCambios.pages.services.changes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />

    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script type="text/javascript">
        function openModal() {
            $('#ConfirmacionModal').modal('show');
        }
    </script>
    <script type="text/javascript">
        function ShowProgress() {
            document.getElementById('<% Response.Write(UpdateProgress1.ClientID); %>').style.display = "inline";
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
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Control de Cambios</h2>
                        <p class="mb-md-0">Creación de cambios</p>
                    </div>
                    <div class="d-flex">
                        <i class="mdi mdi-home text-muted hover-cursor"></i>
                        <p class="text-muted mb-0 hover-cursor">&nbsp;/&nbsp;Dashboard&nbsp;/&nbsp;</p>
                        <p class="text-primary mb-0 hover-cursor">Control de Cambios</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                        <ContentTemplate>

                            <asp:Button ID="BtnAsignarUsuario" runat="server" class="btn btn-light bg-white mr-2 " data-toggle="modal" data-target="#UsuarioModal" Text="Asignar a Usuario" />
                            <asp:Button ID="BtnGuardarCambio" class="btn btn-primary mr-2" runat="server" Text="Guardar" OnClick="BtnGuardarCambio_Click" />
                            <asp:Button ID="BtnCancelarCambio" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelarCambio_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="smartwizard">
        <ul>
            <li><a href="#step-1">Paso 1<br />
                <small>Informacion Cambio</small></a></li>
            <li id="LIPaso2" runat="server" visible="false"><a href="#step-2">Paso 2<br />
                <small>Certificación QA</small></a></li>
            <li id="LIPaso3" runat="server" visible="false"><a href="#step-3">Paso 3<br />
                <small>CAB Manager</small></a></li>
            <li id="LIPaso4" runat="server" visible="false"><a href="#step-4">Paso 4<br />
                <small>Implementación</small></a></li>
            <li id="LIPaso5" runat="server" visible="false"><a href="#step-5">Paso 5<br />
                <small>Monitoreo</small></a></li>
            <li id="LIPaso6" runat="server" visible="false"><a href="#step-6">Paso 6<br />
                <small>Cierre de Cambio</small></a></li>
        </ul>
        <div>
            <div id="step-1" class="">
                <br />
                <nav>
                    <div class="nav nav-pills " id="nav-tab" role="tablist">
                        <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-datos" role="tab" aria-controls="nav-home" aria-selected="true">1. Datos del Cambio</a>
                        <a class="nav-item nav-link" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false">2. Datos Técnicos</a>
                        <a class="nav-item nav-link" id="nav-procesamiento-tab" data-toggle="tab" href="#nav-procesamiento" role="tab" aria-controls="nav-contact" aria-selected="false">3. Procedimientos (Paso a paso)</a>
                        <a class="nav-item nav-link" id="nav-archivos-tab" data-toggle="tab" href="#nav-archivos" role="tab" aria-controls="nav-contact" aria-selected="false">4. Subir archivos</a>
                    </div>
                </nav>
                <div class="tab-content" id="nav-tabContent">
                    <div class="tab-pane fade show active" id="nav-datos" role="tabpanel" aria-labelledby="nav-datos-tab">
                        <br />
                        <div class="row">
                            <div class="col-12 grid-margin stretch-card">
                                <div class="card">
                                    <div class="card-body">
                                        <h4 class="card-title">Información general del cambio</h4>
                                        <p class="card-description">
                                            Ingrese los campos requeridos               
                                        </p>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Nombre</label>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TxNombreMantenimiento" placeholder="ej. Mejoras en sistemas de Caja" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Proveedor</label>
                                                    <div class="col-sm-9">
                                                        <asp:DropDownList ID="DDLProveedor" runat="server" class="form-control"></asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row ">
                                            <div class="col-md-4 grid-margin-md-0 flex-wrap">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label font-weight-bold">Criticidad</label>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Criticidad" id="RBCriticidadAlta" value="1">
                                                                    Alta
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Criticidad" id="RBCriticidadMedia" value="2">
                                                                    Media
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Criticidad" id="RBCriticidadBaja" value="3">
                                                                    Baja
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label font-weight-bold">Impacto</label>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Impacto" id="RBImpactoAlta" value="1">
                                                                    Alta
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Impacto" id="RBImpactoMedia" value="2">
                                                                    Media
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Impacto" id="RBImpactoBaja" value="3">
                                                                    Baja
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label font-weight-bold">Riesgo</label>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Riesgo" id="RBRiesgoAlta" value="1">
                                                                    Alta
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Riesgo" id="RBRiesgoMedia" value="2">
                                                                    Media
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="Riesgo" id="RBRiesgoBaja" value="3">
                                                                    Baja
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputPassword4">Observaciones del cambio (datos generales)</label>
                                            <asp:TextBox ID="TxObservaciones" placeholder="Ingrese una observación del cambio" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 grid-margin stretch-card">
                                <div class="card">
                                    <div class="card-body">
                                        <h4 class="card-title">Mantenimientos</h4>
                                        <div class="form-group">
                                            <label for="exampleInputPassword4">Descripcion del cambio</label>
                                            <asp:TextBox ID="TxMantenimientoDescripcion" placeholder="..." class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Mantenimiento</label>
                                                            <div class="col-sm-9">
                                                                <asp:DropDownList ID="DDLTipoMantenimiento" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLTipoMantenimiento_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group row">
                                                            <label class="col-sm-3 col-form-label">Subtipo</label>
                                                            <div class="col-sm-9">
                                                                <asp:DropDownList ID="DDLSubtipoMantenimiento" runat="server" class="form-control"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="row ">
                                            <div class="col-md-6 grid-margin-md-0 flex-wrap">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label font-weight-bold">Tipo de Cambio</label>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="TipoCambio" id="RBMantenimientoTipoCambio1" value="1">
                                                                    Estándar
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="TipoCambio" id="RBMantenimientoTipoCambio2" value="2">
                                                                    Planificado / Normal
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="TipoCambio" id="RBMantenimientoTipoCambio3" value="3">
                                                                    Emergencia
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label font-weight-bold">Lugar</label>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="LugarMantenimiento" id="RBLugarMantenimiento1" value="1">
                                                                    Centro de Datos Principal
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="LugarMantenimiento" id="RBLugarMantenimiento2" value="2">
                                                                    Centro de Datos Alterno
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="LugarMantenimiento" id="RBLugarMantenimiento3" value="3">
                                                                    Agencias
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="LugarMantenimiento" id="RBLugarMantenimiento4" value="4">
                                                                    Edificio Administrativo
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <label class="form-check-label">
                                                                    <input type="radio" runat="server" class="form-check-input" name="LugarMantenimiento" id="RBLugarMantenimiento5" value="5">
                                                                    Otros
                                                                </label>
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
                        <div class="row">
                            <div class="col-12 grid-margin stretch-card">
                                <div class="card">
                                    <div class="card-body">
                                        <h4 class="card-title">Tiempos y Horarios</h4>
                                        <div class="row">

                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Inicio</label>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TxVentanaDuracionInicio" placeholder="2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Fin</label>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TxVentanaDuracionFin" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-md-6" id="DIVDenegacionInicio" runat="server" visible="false">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Inicio</label>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TxVentanaDenegacionInicio" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6" id="DIVDenegacionFinal" runat="server" visible="false">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Fin</label>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TxVentanaDenegacionFin" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane fade" id="nav-tecnicos" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <asp:Button ID="BtnAgregarSistema" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#SistemasModal" Text="(+) Sistema" OnClick="BtnAgregarSistema_Click" />
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
                                                        AutoGenerateColumns="false" OnRowCommand="GVSistemas_RowCommand">
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
                                        <asp:Button ID="BtnAgregarEquipos" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#EquiposModal" Text="(+) Equipos" />
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
                                                        AutoGenerateColumns="false"
                                                        OnRowCommand="GVEquipos_RowCommand">
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

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <asp:Button ID="BtnAgregarPersonal" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#PersonalModal" Text="(+) Personal" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <div class="card">
                                            <div class="card-body">
                                                <h4 class="card-title">Datos del Personal</h4>
                                                <p class="col-form-label">Ingrese el personal que estara a cargo del cambio</p>

                                                <div class="table-responsive">
                                                    <asp:GridView ID="GVPersonal" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pager"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false" OnRowCommand="GVPersonal_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                                <HeaderTemplate>
                                                                    Acción
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnPersonalDelete" runat="server" Text="borrar" class="btn btn-dark mr-2" CommandArgument='<%# Eval("nombre") %>' CommandName="DeleteRow" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                                            <asp:BoundField DataField="cargo" HeaderText="Cargo" />

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <asp:Button ID="BtnAgregarComunicacion" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#ComunicacionesModal" Text="(+) Comunicacion" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <div class="card">
                                            <div class="card-body">
                                                <h4 class="card-title">Plan de Comunicación</h4>
                                                <p class="col-form-label">Ingrese el plan de comunicaciones deseado</p>

                                                <div class="table-responsive">
                                                    <asp:GridView ID="GVComunicaciones" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pager"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false" OnRowCommand="GVComunicaciones_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                                <HeaderTemplate>
                                                                    Acción
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnPersonalDelete" runat="server" Text="borrar" class="btn btn-dark mr-2" CommandArgument='<%# Eval("cambio") %>' CommandName="DeleteRow" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="cambio" HeaderText="Cambio" />
                                                            <asp:BoundField DataField="incidente" HeaderText="Correo" />

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <div class="tab-pane fade" id="nav-procesamiento" role="tabpanel" aria-labelledby="nav-procesamiento-tab">
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <asp:Button ID="BtnAgregarProcedimiento" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#ProcedimientosModal" Text="(+) Procedimiento" OnClick="BtnAgregarProcedimiento_Click" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <div class="card">
                                            <div class="card-body">
                                                <h4 class="card-title">Procedimientos</h4>
                                                <p class="col-form-label">Ingrese los procedimientos del mantenimiento</p>

                                                <div class="table-responsive">
                                                    <asp:GridView ID="GVProcedimientos" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pager"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false"
                                                        OnRowCommand="GVProcedimientos_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                                <HeaderTemplate>
                                                                    Acción
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnProcedimientoslDelete" runat="server" Text="borrar" class="btn btn-dark mr-2" CommandArgument='<%# Eval("detalle") %>' CommandName="DeleteRow" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                                            <asp:BoundField DataField="detalle" HeaderText="Detalle Actividad" />
                                                            <asp:BoundField DataField="inicio" HeaderText="Inicio" />
                                                            <asp:BoundField DataField="fin" HeaderText="Fin" />
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px" Visible="false">
                                                                <HeaderTemplate>
                                                                    Estado
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBProcedimientos" runat="server" AutoPostBack="true" OnCheckedChanged="CBProcedimientos_CheckedChanged" value='<%# Eval("idProcedimiento") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <asp:Button ID="BtnAgregarRollback" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#RollbackModal" Text="(+) Rollback" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <div class="card">
                                            <div class="card-body">
                                                <h4 class="card-title">RollBack (Paso a paso)</h4>
                                                <p class="col-form-label">Ingrese el plan de rollback en caso de retornar el cambio</p>

                                                <div class="table-responsive">
                                                    <asp:GridView ID="GVRollback" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pager"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false" OnRowCommand="GVRollback_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                                <HeaderTemplate>
                                                                    Acción
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnRollbacklDelete" runat="server" Text="borrar" class="btn btn-dark mr-2" CommandArgument='<%# Eval("detalle") %>' CommandName="DeleteRow" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                                            <asp:BoundField DataField="detalle" HeaderText="Detalle Actividad" />

                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px" Visible="false">
                                                                <HeaderTemplate>
                                                                    Estado
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBRollback" runat="server" AutoPostBack="true" OnCheckedChanged="CBRollback_CheckedChanged" value='<%# Eval("idRollback") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <asp:Button ID="BtnAgregarPruebas" runat="server" class="btn btn-twitter mr-2 " data-toggle="modal" data-target="#PruebasModal" Text="(+) Prueba" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12 grid-margin stretch-card">
                                        <div class="card">
                                            <div class="card-body">
                                                <h4 class="card-title">Pruebas (Paso a paso)</h4>
                                                <p class="col-form-label">Ingrese el plan de pruebas para verificar el cambio</p>

                                                <div class="table-responsive">
                                                    <asp:GridView ID="GVPruebas" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pager"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false" OnRowCommand="GVPruebas_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                                <HeaderTemplate>
                                                                    Acción
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnPruebaslDelete" runat="server" Text="borrar" class="btn btn-dark mr-2" CommandArgument='<%# Eval("responsable") %>' CommandName="DeleteRow" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                                            <asp:BoundField DataField="detalle" HeaderText="Detalle Actividad" />
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px" Visible="false">
                                                                <HeaderTemplate>
                                                                    Estado
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CBPruebas" runat="server" AutoPostBack="true" OnCheckedChanged="CBPruebas_CheckedChanged" value='<%# Eval("idPrueba") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane fade" id="nav-archivos" role="tabpanel" aria-labelledby="nav-archivos-tab">
                        <br />
                        <div class="row">
                            <div class="col-12 grid-margin stretch-card">
                                <div class="card">
                                    <div class="card-body">
                                        <h4 class="card-title">Seleccione los archivos del cambio</h4>
                                        <p class="card-description">
                                            Por favor un maximo de 3 archivos en formato ZIP               
                                        </p>
                                        <div class="row" id="DIVDeposito1" runat="server" visible="true">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Deposito 1</label>
                                                    <div class="col-sm-9">
                                                        <asp:FileUpload ID="FUDeposito1" accept="zip,application/zip,application/x-zip,application/x-zip-compressed" runat="server" class="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="DIVDescargarDeposito1" runat="server" visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Deposito 1</label>
                                                    <div class="col-sm-9">
                                                        <asp:Button ID="BtnDescargarDeposito1" class="btn btn-primary mr-2" runat="server" Text="Descargar" OnClick="BtnDescargarDeposito1_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="DIVDeposito2" runat="server" visible="true">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Deposito 2</label>
                                                    <div class="col-sm-9">
                                                        <asp:FileUpload ID="FUDeposito2" accept="zip,application/zip,application/x-zip,application/x-zip-compressed" runat="server" class="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="DIVDescargarDeposito2" runat="server" visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Deposito 2</label>
                                                    <div class="col-sm-9">
                                                        <asp:Button ID="BtnDescargarDeposito2" class="btn btn-primary mr-2" runat="server" Text="Descargar" OnClick="BtnDescargarDeposito2_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="DIVDeposito3" runat="server" visible="true">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Deposito 3</label>
                                                    <div class="col-sm-9">
                                                        <asp:FileUpload ID="FUDeposito3" accept="zip,application/zip,application/x-zip,application/x-zip-compressed" runat="server" class="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="DIVDescargarDeposito3" runat="server" visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-3 col-form-label">Deposito 3</label>
                                                    <div class="col-sm-9">
                                                        <asp:Button ID="BtnDescargarDeposito3" class="btn btn-primary mr-2" runat="server" Text="Descargar" OnClick="BtnDescargarDeposito3_Click" />
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

            <div id="step-2" class="" style="height: auto;">
                <h3 class="border-bottom border-gray pb-2" style="padding-top: 10px;">Certificación de QA</h3>
                <div class="row">
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Por favor seleccione una opción para el cambio</h4>
                                <p class="card-description">
                                    Tener en cuenta sin esta certificación el cambio no puede avanzar        
                                </p>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Acción</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="DDLCertificacion" runat="server" class="form-control">
                                                    <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                    <asp:ListItem Value="1">Certificado</asp:ListItem>
                                                    <asp:ListItem Value="2">Regresar Cambio a Promotor</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="d-flex justify-content-between align-items-end flex-wrap">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnCertificarCambio" class="btn btn-success mr-2" runat="server" Text="Certificar" OnClick="BtnCertificarCambio_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="step-3" class="" style="height: initial;">
                <h3 class="border-bottom border-gray pb-2" style="padding-top: 10px;">Revisión de CAB Manager</h3>
                <div class="row">
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Por favor seleccione la opción final del cambio</h4>
                                <p class="card-description">
                                    Por favor en caso de que se vaya a cambiar la ventana colocar las nuevas horas en el segmento de tiempos y horarios.           
                                </p>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Acción</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="DDLRevisionQA" runat="server" class="form-control">
                                                    <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                    <asp:ListItem Value="1">Certificado</asp:ListItem>
                                                    <asp:ListItem Value="2">Regresar Cambio a Promotor</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
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
                                <h4 class="card-title">Tiempos y Horarios (Aprobados para el cambio)</h4>
                                <p>Si la hora ingresada en la creación del cambio esta bien por favor no ingrese ninguna fecha en esta area</p>
                                <div class="row">

                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Inicio</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxRevisionQAInicio" placeholder="2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Fin</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxRevisionQAFinal" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="d-flex justify-content-between align-items-end flex-wrap">
                                        <asp:UpdatePanel ID="UpdateRevisionQA" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnRevisionQA" class="btn btn-success mr-2" runat="server" Text="Certificar" OnClick="BtnRevisionQA_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="step-4" class="" style="height: initial;">
                <h3 class="border-bottom border-gray pb-2" style="padding-top: 10px;">Implementación</h3>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-12 grid-margin stretch-card">
                                <div class="card">
                                    <div class="card-body">
                                        <h4 class="card-title">Procedimientos</h4>
                                        <p class="col-form-label">Ingrese los procedimientos del mantenimiento</p>

                                        <div class="table-responsive">
                                            <asp:GridView ID="GVProcedimientosImplementacion" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                OnRowCommand="GVProcedimientos_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                                    <asp:BoundField DataField="detalle" HeaderText="Detalle Actividad" />
                                                    <asp:BoundField DataField="inicio" HeaderText="Inicio" />
                                                    <asp:BoundField DataField="fin" HeaderText="Fin" />
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px" Visible="false">
                                                        <HeaderTemplate>
                                                            Estado
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CBProcedimientos" runat="server" AutoPostBack="true" OnCheckedChanged="CBProcedimientos_CheckedChanged" value='<%# Eval("idProcedimiento") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                                        <h4 class="card-title">RollBack (Paso a paso)</h4>
                                        <p class="col-form-label">Ingrese el plan de rollback en caso de retornar el cambio</p>

                                        <div class="table-responsive">
                                            <asp:GridView ID="GVRollbackImplementacion" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false" OnRowCommand="GVRollback_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                                    <asp:BoundField DataField="detalle" HeaderText="Detalle Actividad" />

                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px" Visible="false">
                                                        <HeaderTemplate>
                                                            Estado
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CBRollback" runat="server" AutoPostBack="true" OnCheckedChanged="CBRollback_CheckedChanged" value='<%# Eval("idRollback") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                                        <h4 class="card-title">Pruebas (Paso a paso)</h4>
                                        <p class="col-form-label">Ingrese el plan de pruebas para verificar el cambio</p>

                                        <div class="table-responsive">
                                            <asp:GridView ID="GVPruebasImplementacion" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false" OnRowCommand="GVPruebas_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="responsable" HeaderText="Responsable" />
                                                    <asp:BoundField DataField="detalle" HeaderText="Detalle Actividad" />
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px" Visible="false">
                                                        <HeaderTemplate>
                                                            Estado
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CBPruebas" runat="server" AutoPostBack="true" OnCheckedChanged="CBPruebas_CheckedChanged" value='<%# Eval("idPrueba") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 grid-margin ">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnImplementacion" class="btn btn-success mr-2" runat="server" Text="Terminar Implementación" OnClick="BtnImplementacion_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="step-5" class="" style="height: initial;">
                <h3 class="border-bottom border-gray pb-2" style="padding-top: 10px;">Cierre de Cambio</h3>
                <div class="row">
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Cerrar el cambio No.<asp:Label ID="LbNumeroCambio" runat="server" Text=""></asp:Label></h4>
                                <p class="card-description">
                                    <asp:Label ID="LbCambioDescripcion" runat="server" Text="Label"></asp:Label>
                                </p>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Resultado</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="DDLResultado" runat="server" class="form-control">
                                                    <asp:ListItem Value="0">Seleccione una Opción</asp:ListItem>
                                                    <asp:ListItem Value="1">Satisfactorio</asp:ListItem>
                                                    <asp:ListItem Value="2">No Satisfactorio</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Observaciones</label>
                                            <asp:TextBox ID="TxCierreObservaciones" placeholder="..." class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Impacto del resultado del cambio</label>
                                            <asp:TextBox ID="TxCierreImpacto" placeholder="..." class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <h4 class="card-title">Tiempos y Horarios</h4>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <h5>
                                                <label class="col-sm-12 col-form-label">Duracion de la Ventana</label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <h5>
                                                <label class="col-sm-12 col-form-label">Suma Total Denegación</label>
                                            </h5>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Inicio</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreVentanaInicio" placeholder="2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Inicio</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreDenegacionInicio" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Fin</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreVentanaFinal" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Fin</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreDenegacionFinal" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
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
                                <h4 class="card-title">Rollback (Opcional)</h4>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Rollback (Solo si aplica)</label>
                                            <asp:TextBox ID="TxCierreRollback" placeholder="..." class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <h5>
                                                <label class="col-sm-12 col-form-label">Duracion de la Ventana</label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <h5>
                                                <label class="col-sm-12 col-form-label">Suma Total Denegación</label>
                                            </h5>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Inicio</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreRollbackInicio" placeholder="2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Inicio</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreRollbackFin" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Fin</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreRollbackDenInicio" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Fin</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="TxCierreRollbackDenFin" placeholder="ej. 2019/01/01 00:00:00" TextMode="DateTimeLocal" class="form-control" runat="server"></asp:TextBox>
                                            </div>
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
                                <h4 class="card-title">Evidencia del cambio</h4>
                                <p class="card-description">
                                    Por favor un maximo de 2 archivos en formato ZIP               
                                </p>
                                <div class="row">

                                    <div class="col-md-6" id="DivEvidenciaSubir1" runat="server" visible="true">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Deposito 1</label>
                                            <div class="col-sm-9">
                                                <asp:FileUpload ID="FUEvidenciaSubir1" accept="zip,application/zip,application/x-zip,application/x-zip-compressed" runat="server" class="form-control" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6" id="DivEvidenciaDescarga1" runat="server" visible="false">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Deposito 1</label>
                                            <div class="col-sm-9">
                                                <asp:Button ID="BtnEvidenciaDescargar1" class="btn btn-primary mr-2" runat="server" Text="Descargar" OnClick="BtnEvidenciaDescargar1_Click"  />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6" id="DivEvidenciaSubir2" runat="server" visible="true">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Deposito 2</label>
                                            <div class="col-sm-9">
                                                <asp:FileUpload ID="FUEvidenciaSubir2" accept="zip,application/zip,application/x-zip,application/x-zip-compressed" runat="server" class="form-control" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6" id="DivEvidenciaDescarga2" runat="server" visible="false">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Deposito 2</label>
                                            <div class="col-sm-9">
                                                <asp:Button ID="BtnEvidenciaDescargar2" class="btn btn-primary mr-2" runat="server" Text="Descargar" OnClick="BtnEvidenciaDescargar2_Click"  />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 grid-margin ">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="BtnResolucion" class="btn btn-success mr-2" runat="server" Text="Terminar Revisión" data-toggle="modal" data-target="#ConfirmacionCierreModal"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div id="step-6" class="" style="height: initial;">
                <h3 class="border-bottom border-gray pb-2" style="padding-top: 10px;">Cerrar cambio</h3>
                <div class="row">
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Por favor seleccione una opción para el cambio</h4>
                                <p class="card-description">
                                    Tener en cuenta sin esta certificación el cambio permanecera abierto       
                                </p>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Acción</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="DDLCerrarCambio" runat="server" class="form-control">
                                                    <asp:ListItem Value="0">Selecione una Opción</asp:ListItem>
                                                    <asp:ListItem Value="1">Certificado</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="d-flex justify-content-between align-items-end flex-wrap">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnCerrarCambio" class="btn btn-success mr-2" runat="server" Text="Finalizar Cambio" OnClick="BtnCerrarCambio_Click" />
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

    <%-- MODAL DE SISTEMAS--%>
    <div class="modal fade" id="SistemasModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelSistemas">Agregar sistema</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Equipo</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="DDLEquipo" class="form-control" runat="server" OnSelectedIndexChanged="DDLEquipo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdateSistemas" runat="server">
                        <ContentTemplate>
                            <div id="DIVSistema" runat="server" visible="false">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Sistema</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="DDLSistema" class="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Descripción</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="TxSistemaDescripcion" class="form-control" runat="server" placeholder="..." TextMode="MultiLine"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Denegación</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="DDLSistemaDenegacion" class="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLSistemaDenegacion_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Seleccione un tipo</asp:ListItem>
                                            <asp:ListItem Value="1">Si</asp:ListItem>
                                            <asp:ListItem Value="2">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group row" id="DIVSistemasTiempoInicial" runat="server" visible="false">
                                    <label class="col-sm-3 col-form-label">Tiempo Inicio</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="TxSistemaTiempoInicio" class="form-control" runat="server" placeholder="2019/01/01 00:00" TextMode="DateTimeLocal"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="form-group row" id="DIVSistemasTiempoFinal" runat="server" visible="false">
                                    <label class="col-sm-3 col-form-label">Tiempo Final</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="TxSistemaTiempoFinal" class="form-control" runat="server" placeholder="2019/01/01 00:00" TextMode="DateTimeLocal"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateSistemasMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensajeSistemas" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateSistemasBotones" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearSistema" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnCrearSistema_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- MODAL DE EQUIPOS--%>
    <div class="modal fade" id="EquiposModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelEquipos">Agregar Equipos</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateEquipos" runat="server">
                        <ContentTemplate>

                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Equipo</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLEquipoSecundario" class="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateEquiposMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbEquiposMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateEquiposBotones" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearEquipos" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnCrearEquipos_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- MODAL DE PERSONAL--%>
    <div class="modal fade" id="PersonalModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelPersonal">Agregar personal</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePersonal" runat="server">
                        <ContentTemplate>

                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Nombre</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="DDLPersonalNombre" class="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Cargo</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxPersonalCargo" class="form-control" runat="server" placeholder="..." TextMode="SingleLine"></asp:TextBox>

                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePersonalMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensajePersonal" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePersonalBotones" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearPersonal" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnCrearPersonal_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE COMUNICACION--%>
    <div class="modal fade" id="ComunicacionesModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelComunicaciones">Agregar comunicaciones</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateComunicaciones" runat="server">
                        <ContentTemplate>

                            <p>
                                Por favor escriba el correo de manera correcta para avisar a las personas interesadas en el cambio.
                            </p>
                            <br />

                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Usuario</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxComunicacionesCambio" class="form-control" runat="server" placeholder="..." TextMode="SingleLine"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Correo</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxComunicacionesIncidente" class="form-control" runat="server" placeholder="..." TextMode="SingleLine"></asp:TextBox>

                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateComunicacionesMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensajeComunicaciones" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateComunicacionesBotones" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearComunicaciones" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnCrearComunicaciones_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE PROCEDIMIENTOS--%>
    <div class="modal fade" id="ProcedimientosModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelProcedimientos">Agregar procedimientos</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateProcedimientos" runat="server">
                        <ContentTemplate>

                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Inicio</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxProcedimientosInicio" class="form-control" runat="server" placeholder="2019-01-01 00:00" TextMode="DateTimeLocal"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Fin</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxProcedimientosFin" class="form-control" runat="server" placeholder="2019-01-01 00:00" TextMode="DateTimeLocal"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Detalle</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxProcedimientosDetalle" class="form-control" runat="server" placeholder="..." TextMode="MultiLine"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Responsable</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="DDLProcedimientosResponsable" class="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateProcedimientosMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensajeProcedimientos" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateProcedimientosBotones" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearProcedimientos" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnCrearProcedimientos_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE ROLLBACK--%>
    <div class="modal fade" id="RollbackModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelRollback">Agregar Rollback</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateRollback" runat="server">
                        <ContentTemplate>

                            <%--<div class="form-group row">
                                <label class="col-sm-3 col-form-label">Inicio</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxRollBackInicio" class="form-control" runat="server" placeholder="2019-01-01 00:00" TextMode="DateTimeLocal"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Fin</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxRollBackFin" class="form-control" runat="server" placeholder="2019-01-01 00:00" TextMode="DateTimeLocal"></asp:TextBox>
                                </div>
                            </div>--%>

                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Detalle</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxRollBackDetalle" class="form-control" runat="server" placeholder="..." TextMode="MultiLine"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Responsable</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="DDLRollBackResponsable" class="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdateRollbackMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensajeRollback" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateRollbackBotones" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearRollback" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnCrearRollback_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE PRUEBAS--%>
    <div class="modal fade" id="PruebasModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelPruebas">Agregar prueba</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePruebas" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Detalle</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxPruebasDetalle" class="form-control" runat="server" placeholder="..." TextMode="MultiLine"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Responsable</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="DDLPruebasResponsable" class="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePruebasMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensajePruebas" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePruebasBotones" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearPruebas" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnCrearPruebas_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE USUARIO ASIGNACION--%>
    <div class="modal fade" id="UsuarioModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelUsuario">Asignar a Usuario</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateUsuario" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Usuarios</label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="DDLUsuarios" class="form-control" runat="server"></asp:DropDownList>
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
                    <asp:UpdatePanel ID="UpdateUsuarioBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCrearUsuario" runat="server" Text="Asignar" class="btn btn-primary" OnClick="BtnCrearUsuario_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE CONFIRMACION--%>
    <div class="modal fade" id="ConfirmacionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelConfirmacion">Paso final de creación de cambios</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <label class="label">¿Estas seguro de ejecutar esta acción?</label>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnProcedeCreacion" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnProcedeCreacion_Click" OnClientClick="ShowProgress();" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnProcedeCreacion" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE CONFIRMACION CIERRE--%>
    <div class="modal fade" id="ConfirmacionCierreModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelConfirmacionCierre">Guardar resolución del cambio</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <label class="label">¿Estas seguro de ejecutar esta acción?</label>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnProcedeCierre" runat="server" Text="Crear" class="btn btn-primary" OnClick="BtnProcedeCreacion_Click" OnClientClick="ShowProgress();" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnProcedeCierre" />
                        </Triggers>
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
                    toolbarPosition: 'none'
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
