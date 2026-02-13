<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="merge.aspx.cs" Inherits="Maddux.Catch.customer.merge" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/font-awesome.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/AdminLTE.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/skins/skin-red.min.css")%>" />

    <script type="text/javascript">
        function ConfirmAction(Message) {
            if (confirm(Message) == true)
                return true;
            else
                return false;
        }
    </script>
</head>
<body>
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <form id="frmMergeCustomer" runat="server">

        <div class="modal-body">
            <div class="form-row">
                <div class="form-group">

                    <asp:Panel ID="panelPrimaryCustomer" runat="server" GroupingText="&nbsp;Primary Customer Details&nbsp;&nbsp;&nbsp;" Font-Bold="True">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-4">
                                        Company:
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:HyperLink ID="lnkCompany" runat="server">Company</asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-4">
                                        Contact:
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:HyperLink ID="lnkContact" runat="server">Link</asp:HyperLink>
                                        <asp:Label ID="lblContact" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-4">
                                        Address:
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-4">
                                        Phone:
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:Label ID="lblCustomerPhone" runat="server" Text="Phone"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group">
                    <asp:Panel ID="panelMerge" runat="server" GroupingText="&nbsp;Customers to be Merged&nbsp;&nbsp;&nbsp;" Font-Bold="True">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-4">
                                        Find:
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" ToolTip="Enter the company name, address, telephone number to find companies to be merged."></asp:TextBox>
                                        <asp:Button ID="btnSearch" CssClass="btn btn-primary" OnClick="btnSearch_Click" runat="server" Text="Search" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:GridView ID="grdMergeItems" runat="server" CssClass="table table-hover table-bordered table-hover dataTable"
                                            GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                            ShowFooter="true" DataKeyNames="CustomerID" OnRowDataBound="grdMergeItems_RowDataBound"
                                            EmptyDataText="There are no records found.">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkCompanySelect" CssClass="checkbox-toggle" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Top" Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg col-sm-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-sm-2">
                                                    <ItemTemplate>
                                                        <a href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>' target="_blank">
                                                            <%# Eval("Company") %>
                                                        </a>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="28%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg col-sm-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-sm-2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContact" runat="server" />
                                                        <asp:HyperLink ID="hypContact" runat="server"><%# Eval("Contact") %></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg col-sm-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-sm-2">
                                                    <ItemTemplate>
                                                        <%# Eval("CityState") %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg col-sm-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-sm-2">
                                                    <ItemTemplate>
                                                        <%# Eval("Phone") %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="20%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 text-center">
                                        <asp:Label ID="lblRecordCount" runat="server" Text="RecordCount" Visible="false"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnMerge" runat="server" Text="Merge" OnClientClick="return ConfirmAction('Are you sure you want to merge the selected customers with the primary customer?');" OnClick="btnMerge_Click" />&nbsp;
            </div>
        </div>
    </form>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-2.2.3.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/app.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/date.js") %>"></script>
</body>
</html>
