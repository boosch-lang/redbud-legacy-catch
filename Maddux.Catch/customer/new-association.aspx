<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="new-association.aspx.cs" Inherits="Maddux.Catch.customer.new_association" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="row">
        <div class="col-xs-12">
            <div class="form-group">
                <asp:Label Text="Assciation Desc" CssClass="font-weight-bold" for="txtDesc" runat="server" />
                <asp:TextBox runat="server" ID="txtDesc" ClientIDMode="Static" CssClass="form-control" MaxLength="100" />
                <asp:RequiredFieldValidator ErrorMessage="Assciation Desc is required!" CssClass="text-danger" ControlToValidate="txtDesc" runat="server" />
            </div>
        </div>
    </div>
    <div class="row">
         <div class="col-xs-6">
            <label for="ddlClass" class="font-weight-bold">Class</label>
            <asp:DropDownList ID="ddlClass" ClientIDMode="Static" CssClass="form-control" runat="server">
                <asp:ListItem Text="--Select Class --" Value="" />
                <asp:ListItem Text="Customer" Value="Customer" />
                <asp:ListItem Text="Membership" Value="Membership" />
                <asp:ListItem Text="Profile" Value="Profile" />
                <asp:ListItem Text="Media" Value="Media" />
                <asp:ListItem Text="AAA Specialty" Value="AAA Specialty" />
                <asp:ListItem Text="ZZ* Miscellaneous" Value="ZZ* Miscellaneous" />
                <asp:ListItem Text="Newsletters" Value="Newsletters" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ErrorMessage="Class is required!" CssClass="text-danger" ControlToValidate="ddlClass" runat="server" />
        </div>
        <div class="col-xs-6">
            <label for="ddlCalculated" class="font-weight-bold">Is Calculated</label>
            <asp:DropDownList ID="ddlCalculated" ClientIDMode="Static" CssClass="form-control" runat="server">
                <asp:ListItem Text="No" Value="0" />
                <asp:ListItem Text="Yes" Value="1" />
            </asp:DropDownList>
        </div>
       
    </div>
     <div class="row">
        <div class="col-xs-12">
            <div class="form-group">
                <asp:Label Text="Banner Message" CssClass="font-weight-bold" for="BannerMessage" runat="server" />
                <asp:TextBox runat="server" ID="BannerMessage" ClientIDMode="Static" MaxLength="500" CssClass="form-control" TextMode="MultiLine" Rows="4" />
            </div>
            <div class="form-group d-none">
                <asp:Label Text="Tag Line" CssClass="font-weight-bold" for="TagLine" runat="server" />
                <asp:TextBox runat="server" ID="TagLine" ClientIDMode="Static" MaxLength="250" CssClass="form-control" />
            </div>
        </div>
    </div>
    <asp:Button Text="Save" CssClass="btn btn-primary" ID="Save" OnClick="Save_Click" CausesValidation="true" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
