<%@ Page Title="" Language="C#" MasterPageFile="~/Redbud.Master" AutoEventWireup="true" CodeBehind="about.aspx.cs" Inherits="Maddux.Pitch.about" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <br />
    <div class="row">
        <div id="bannerDiv" runat="server" class="col-md-4">
            <asp:Image ImageUrl="#" CssClass="img-responsive img-rounded" style="box-shadow:5px!important" ID="BannerImagePath" runat="server" />
        </div>
        <div id="contentDiv" runat="server" class="col-md-8">
            <asp:Literal runat="server" ID="pageHTML"></asp:Literal>
           
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
