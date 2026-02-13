<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageorders.aspx.cs" Inherits="Maddux.Catch.purchaseorder.manageorders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/font-awesome.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/AdminLTE.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/skins/skin-red.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap-multiselect.css") %>" type="text/css" />

    <style>
        .multiselect-all {
            display: none;
        }

        .dropdown-menu {
            background-color: #ddd;
        }

        button.multiselect {
            background-color: #f4f4f4;
            border: 1px solid #ced4da;
        }

        .multiselect-container.dropdown-menu {
            border: 1px solid #a7a8a9;
            border-radius: 0;
        }

        .dropdown-menu > li > a {
            color: #000;
        }

        .multiselect.dropdown-toggle.btn.btn-default {
            padding: 5px 10px;
            font-size: 12px;
            line-height: 1.5;
            border-radius: 3px;
        }

        .multiselect-container > li {
            padding: 0;
            font-size: 12px;
            line-height: 1.5;
        }

        .dropdown-menu > li > a:hover {
            background-color: blue;
            color: #fff;
        }
    </style>
</head>
<body>
    <form id="frmPurchaseOrders" runat="server">
        <asp:Literal runat="server" ID="litMessage"></asp:Literal>
        <div class="modal-body">
            <div class="row">
                <div class="col-xs-12">
                    <div class="btn-group">
                        <button type="button" class="btn btn-default btn-sm checkbox-toggle-on"><i class="fa fa-check-square-o"></i>Select All</button>
                        <button type="button" class="btn btn-default btn-sm checkbox-toggle-off"><i class="fa fa-square-o"></i>Select None</button>
                    </div>
                    <div class="pull-right">
                        <asp:ListBox ID="ddlFilterProvince"  SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect"></asp:ListBox>
                        <asp:ListBox ID="ddlFilterCatalog" SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect"></asp:ListBox>
                        <asp:DropDownList runat="server" ID="ddlFilterShipDate" CssClass="btn btn-default btn-sm"></asp:DropDownList>
                        <asp:Button Text="Search" CssClass="btn btn-primary btn-sm" ID="btnSearchOrder" OnClick="btnSearchOrder_Click" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-xs-12">
                    <asp:GridView ID="dgvOrders"
                        runat="server"
                        CssClass="table table-hover table-bordered table-hover dataTable"
                        GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                        ShowFooter="true"
                        EmptyDataText="There are no orders to display.">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>Order #</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox Text="" ID="OrderSelector" class="OrderSelector" name="OrderSelector" runat="server" />&nbsp;&nbsp;
                                    <a target="_blank" href="/order/orderdetail.aspx?id=<%# Eval("OrderID") %>"><%# Eval("OrderID") %></a>
                                    <asp:HiddenField runat="server" ID="OrderID" Value='<%# Eval("OrderID") %>' />
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" CssClass="text-nowrap" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Customer</HeaderTemplate>
                                <ItemTemplate>
                                    <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                        <%# Eval("Customer.Company") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer.State" HeaderText="Province"></asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>Catalog</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Repeater ID="rptCatalog" runat="server" DataSource='<%#DataBinder.Eval(Container, "DataItem.OrderRacks") %>'>
                                        <ItemTemplate>
                                            <a target="_blank" href='/catalogs/catalogdetail.aspx?CatalogID=<%# Eval("ProductCatalogRack.CatalogID")%>'>
                                                <%# Eval("ProductCatalogRack.CatalogName") %><br />
                                            </a>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate># 1/2 Racks</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Repeater ID="rptHalfCount" runat="server" DataSource='<%#DataBinder.Eval(Container, "DataItem.OrderRacks") %>'>
                                        <ItemTemplate>
                                            <%# GetCount(Eval("Quantity"), Eval("ProductCatalogRack.RackSize"), "1/2") %>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate># 1/4 Racks</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Repeater ID="retQuarterCount" runat="server" DataSource='<%#DataBinder.Eval(Container, "DataItem.OrderRacks") %>'>
                                        <ItemTemplate>
                                            <%# GetCount(Eval("Quantity"), Eval("ProductCatalogRack.RackSize"), "1/4") %>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="grdNoData" />
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="saveAndClose" CssClass="btn btn-primary" runat="server" OnClick="saveAndClose_Click" Text="Add Selected Orders" />
            <button type="button" class="btn btn-default" onclick="Close()">Close</button>
        </div>
    </form>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-2.2.3.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/app.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/date.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap-multiselect.js") %>"></script>
    <script>
        $(document).ready(function () {

            $('#ddlFilterProvince').multiselect({
                buttonContainer: '<div class="btn-group" />',
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: true,
                enableFullValueFiltering: true,
                enableCaseInsensitiveFiltering: true,
                nonSelectedText: '--All Provinces --',
            });
            $('#ddlFilterCatalog').multiselect({
                buttonContainer: '<div class="btn-group" />',
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: true,
                enableFullValueFiltering: true,
                enableCaseInsensitiveFiltering: true,
                nonSelectedText: '--All Catalogs--',
            });
        });

        $('.checkbox-toggle-on').on('click', function (e) {
            toggleCheck(true);
        });

        $('.checkbox-toggle-off').on('click', function (e) {
            toggleCheck(false);
        });
        $(document).ready(function () {
            $(".btn-group").find(".multiselect").prop("style", "min-width:210px");
        })
        function toggleCheck(checked) {
            $.each($('.OrderSelector').find('input'), function (i, item) {
                $(item).prop('checked', checked);
            });
        }

        function Close() {
            window.parent.CloseModal(window.frameElement);
        }

    </script>
</body>
</html>
