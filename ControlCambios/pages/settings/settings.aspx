<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="settings.aspx.cs" Inherits="ControlCambios.pages.settings.settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container col-md-12 grid-margin">
	    <div class="row align-top">
		    <ul class="breadcrumb">
			    <li class="completed"><a href="/default.aspx">Dashboard</a></li>
			    <li class="active"><a href="javascript:void(0);">Settings</a></li>
		    </ul>
	    </div>
    </div>
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>
                            <asp:Label ID="LbNombreUsuario" runat="server" Text=""></asp:Label></h2>
                        <p class="mb-md-0">Configuraciones</p>
                    </div>
                   
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnGuardarCambios" class="btn btn-primary mr-2" runat="server" Text="Guardar" OnClick="BtnGuardarCambios_Click" />
                            <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                        
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Configuraciones de Usuario</h4>
                            <p class="card-description">
                                Ingrese los cambios que desea hacer
                            </p>

                            <h6>Cambio de Password</h6>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Password</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxPassword" placeholder="Password" class="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Confirmar</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxConfirmar" placeholder="Confirmar" class="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <h6>Datos personales</h6>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Telefono</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxTelefono" placeholder="99999999" class="form-control" runat="server" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Correo</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxCorreo" placeholder="informatica@bancatlan.hn" class="form-control" runat="server" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                    </div>
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
</asp:Content>
