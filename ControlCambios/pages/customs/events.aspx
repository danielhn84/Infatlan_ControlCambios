<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="events.aspx.cs" Inherits="ControlCambios.pages.customs.events" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 850px;">
    <iframe name="myIframe" id="myIframe" width="" height="" src="/pages/customs/calendar.aspx" style="border: none; width: 100%; height:100%; "></iframe>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
