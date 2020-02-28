<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ControlCambios.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <title>Inflatlan | Login</title>
    <link rel="stylesheet" href="/vendors/mdi/css/materialdesignicons.min.css">
    <link rel="stylesheet" href="/vendors/base/vendor.bundle.base.css">
    <link rel="stylesheet" href="/css/style.css">
    <link rel="shortcut icon" href="/images/logo_mini.png" />

    <script type="text/javascript">
        function openInformacionModal() {
            $('#InformacionModal').modal('show');
        }
    </script>

</head>
<body onload="openInformacionModal()">
    <div class="container-scroller">
        <div class="container-fluid page-body-wrapper full-page-wrapper">
            <div class="content-wrapper d-flex align-items-stretch auth auth-img-bg">
                <div class="row flex-grow">
                    <div class="col-lg-6 d-flex align-items-center justify-content-center">
                        <div class="auth-form-transparent text-left p-3">
                            <div class="brand-logo">
                                <img src="/images/logo.png" alt="logo">
                            </div>
                            <h4>Bienvenidos | Control de Cambios</h4>
                            <h6 class="font-weight-light">Ingresa tus credenciales para ingresar al aplicativo</h6>
                            <form id="form1" runat="server">
                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                <asp:UpdatePanel ID="UpdateLogin" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label for="exampleInputEmail">Usuario</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend bg-transparent">
                                                    <span class="input-group-text bg-transparent border-right-0">
                                                        <i class="mdi mdi-account-outline text-primary"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TxUsername" class="form-control form-control-lg border-left-0" placeholder="Username" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputPassword">Password</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend bg-transparent">
                                                    <span class="input-group-text bg-transparent border-right-0">
                                                        <i class="mdi mdi-lock-outline text-primary"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TxPassword" TextMode="Password" class="form-control form-control-lg border-left-0" placeholder="Password" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="my-2 d-flex justify-content-between align-items-center">
                                            <!--<div class="form-check">
                                        <label class="form-check-label text-muted">
                                            <input type="checkbox" class="form-check-input" id="CBSession" runat="server">
                                            Mantener la Session
                   
                                        </label>
                                    </div>
                                    <a href="#" class="auth-link text-black">Olvidaste tu password?</a>-->
                                        </div>
                                        <div class="my-3">
                                            <asp:Button ID="BtnLogin" class="btn btn-block btn-primary btn-lg font-weight-medium auth-form-btn" runat="server" Text="Entrar" OnClick="BtnLogin_Click" />
                                        </div>

                                        <div class="my-2 d-flex justify-content-center align-center" style="color: indianred;">
                                            <asp:Label ID="LbMensaje" runat="server" Text=""></asp:Label>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </form>
                        </div>
                    </div>
                    <div class="col-lg-6 login-half-bg d-flex flex-row">
                        <p class="text-black font-weight-medium text-center flex-grow align-self-end">Copyright Infatlan &copy; 2019  Derechos Reservados.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="InformacionModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelSupervisro">Información Control de Cambios v1.2.1</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <h4 class="col-sm-12 col-form-label">Nueva versión</h4>
                        <label class="col-sm-12 col-form-label">
                            1. Corrección error de duplicidad (Sesiones)
                            <br />
                            2. Cambios generales 1.2.1
                            <ul style="margin-left: 20px">
                                <li>Observaciones en cierre</li>
                                <li>Subir archivos de todo tipo en paso 1</li>
                                <li>Nuevos campos requeridos en paso 1 (Comunicacion, Sistemas, Equipos, Procedimientos)</li>
                                <li>Cambio de empresa a clasificacion en paso 1</li>
                            </ul>
                            
                            3. Modificación de flujos (Ingreso de datos)
                            <ul style="margin-left: 20px">
                                <li>Envio de correos plan comunicación por cada paso</li>
                            </ul>
                        </label>
                        <label class="col-sm-12 col-form-label" style="color: indianred">
                            Este mensaje desaparecera la primera semana de marzo.
                        </label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <script src="/vendors/base/vendor.bundle.base.js"></script>
    <script src="/js/off-canvas.js"></script>
    <script src="/js/hoverable-collapse.js"></script>
    <script src="/js/template.js"></script>
</body>
</html>
