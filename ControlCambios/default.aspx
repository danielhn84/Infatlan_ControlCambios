﻿<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ControlCambios._default" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/alert.css" rel="stylesheet" />
    
    <%--<script src="/js/jquery-3.1.1.js"></script>
    <script src="/js/bootstrap.js"></script>--%>



    <script type="text/javascript">
        function openModalInfo()
        {
            $(<%= InformacionModal.ClientID%>).modal('show');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    

    <div class="container col-md-12 grid-margin">
        <div class="row align-top">
            <ul class="breadcrumb">
                <li class="active"><a href="/default.aspx">Dashboard</a></li>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Control de Cambios</h2>
                        <p class="mb-md-0">Pagina de Inicio</p>
                    </div>

                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="d-flex justify-content-between align-items-end flex-wrap" runat="server" id="divBtnDashboard" visible="false">

                            <asp:Button ID="BtnReporteSummary" class="btn btn-primary mt-2 mt-xl-0" runat="server" Text="Descargar Reporte" OnClientClick="window.open('dashboard.aspx','_blank');" OnClick="BtnReporteSummary_Click" />

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body dashboard-tabs p-0">
                    <ul class="nav nav-tabs px-4" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="overview-tab" data-toggle="tab" href="#overview" role="tab" aria-controls="overview" aria-selected="true">Cambios</a>
                        </li>


                    </ul>
                    <div class="tab-content py-0 px-0">
                        <div class="tab-pane fade show active" id="overview" role="tabpanel" aria-labelledby="overview-tab">
                            <div class="d-flex flex-wrap justify-content-xl-between">
                                <div class="d-none d-xl-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
                                    <i class="mdi mdi-calendar-heart icon-lg mr-3 text-primary"></i>
                                    <div class="d-flex flex-column justify-content-around">
                                        <small class="mb-1 text-muted">Fechas</small>
                                        <div class="dropdown">
                                            <a class="btn btn-secondary  p-0 bg-transparent border-0 text-dark shadow-none font-weight-medium" href="#" role="button" id="dropdownMenuLinkA" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <h5 class="mb-0 d-inline-block">
                                                    <asp:Literal ID="LitFechaCambios" runat="server"></asp:Literal>
                                                </h5>
                                            </a>


                                        </div>
                                    </div>
                                </div>
                                <div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
                                    <i class="mdi mdi-security-account mr-3 icon-lg text-danger"></i>
                                    <div class="d-flex flex-column justify-content-around">
                                        <small class="mb-1 text-muted">Cambios Creados</small>
                                        <h5 class="mr-2 mb-0">
                                            <asp:Literal ID="LitCambiosCreados" runat="server"></asp:Literal></h5>
                                    </div>
                                </div>
                                <div class="d-flex border-md-right flex-grow-1 align-items-center justify-content-center p-3 item">
                                    <i class="mdi mdi-security-account-outline mr-3 icon-lg text-success"></i>
                                    <div class="d-flex flex-column justify-content-around">
                                        <small class="mb-1 text-muted">Cambios Cerrados</small>
                                        <h5 class="mr-2 mb-0">
                                            <asp:Literal ID="LitCambiosFinalizados" runat="server"></asp:Literal></h5>
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
                    <h4 class="card-title">Ultimos 5 cambios creados</h4>

                    <div class="row">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdateGridView" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="GVBusqueda" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pager"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        GridLines="None"
                                        AutoGenerateColumns="false" OnRowCommand="GVBusqueda_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    Acción
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button ID="BtnEntrar" runat="server" Text="Entrar" class="btn btn-facebook mr-2" CommandArgument='<%# Eval("idcambio") %>' CommandName="EntrarCambio" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="idcambio" HeaderText="No.Cambio" />
                                            <asp:BoundField DataField="mantenimientoNombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="fechaSolicitud" HeaderText="Fecha" />
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
    <%--MODAL INFORMATIVO TELEFONO--%>
    <div class="modal fade" id="InformacionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" runat="server" >
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelInformacion">Información de Usuario</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateMensajeWarning" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label class="label">Por favor actualiza tu numero de telefono en Settings.</label>
                            <label class="label"><b>Dirección:</b> Tu nombre > Settings</label>

                            <label class="label" style="color:indianred; font-size: small;">Este mensaje dejara de aparecer hasta que actualices.</label>
                            <br /><br />
       
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                        </ContentTemplate>
             
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
