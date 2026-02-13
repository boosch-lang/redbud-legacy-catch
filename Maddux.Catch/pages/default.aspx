<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Maddux.Catch.pages._default" %>

<%@ MasterType VirtualPath="~/Maddux.Catch.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="row">
        <div class="col-xs-12">
            <asp:GridView runat="server"
                CssClass="table table-hover table-bordered table-hover"
                ID="dgvPages"
                AllowSorting="true"
                SortMode="Automatic"
                GridLines="Horizontal"
                AutoGenerateColumns="False"
                ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                OnRowDataBound="dgvPages_RowDataBound"
                EmptyDataText="There are pages to display.">
                <Columns>
                    <asp:TemplateField HeaderText="Title" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4">
                        <ItemTemplate>
                            <a target="_blank" href="edit.aspx?id=<%# Eval("PageID") %>"><%# Eval("Title") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Slug" HeaderText="Slug" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                    <asp:BoundField DataField="CreatedOn" HeaderText="Created On" DataFormatString="{0:dd MMM, yyyy}" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="Modified On" DataFormatString="{0:dd MMM, yyyy}" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
