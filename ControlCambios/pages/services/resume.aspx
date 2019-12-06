<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="resume.aspx.cs" Inherits="ControlCambios.pages.services.resume" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Control de Cambios | Resumen de cambio</title>
    <style>
        html,body,form,#div1 {
            height: 100%; 
            width: 100%;
            overflow:hidden;
            left: 0;
            top: 0;
            margin: 0;
            position: fixed;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="div1" >
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" Height="100%" Width="100%">
              <ServerReport ReportPath="http://10.128.0.52/ReportServer" ReportServerUrl="" />
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
