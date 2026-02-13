<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="Maddux.Catch.freight.edit" %>

<%@ MasterType VirtualPath="~/Maddux.Catch.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="row">
        <div class="col-md-4">
            <label class="font-weight-bold" for="AreaIDText">Area Code</label>
            <asp:TextBox runat="server" MaxLength="3" ID="AreaIDText" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ErrorMessage="Area Code is required!" CssClass="text-danger" ControlToValidate="AreaIDText" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <label class="font-weight-bold" for="PlaceName">Area Name</label>
            <asp:TextBox runat="server" MaxLength="150" ID="PlaceName" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <label class="font-weight-bold" for="Province">Province</label>
            <asp:DropDownList runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddlProvince">
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <label class="font-weight-bold" for="Region">Region</label>
            <asp:DropDownList runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddlRegion">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <label class="font-weight-bold" for="Charge">Charge</label>
            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCharges">
                <asp:ListItem Text="0.00 %" Value="0.00" />
                <asp:ListItem Text="2.50 %" Value="2.50" />
                <asp:ListItem Text="4.00 %" Value="4.00" />
            </asp:DropDownList>
        </div>
    </div>
    <div style="margin-top: 20px" class="row">
        <div class="col-md-8 ">
            <div class="text-center">
                <asp:Button Text="Save" ID="Save" OnClick="Save_Click" CssClass="btn btn-success" runat="server" />&nbsp;
                <asp:Button Text="Delete" ID="Delete" OnClick="Delete_Click" CssClass="btn btn-danger" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
