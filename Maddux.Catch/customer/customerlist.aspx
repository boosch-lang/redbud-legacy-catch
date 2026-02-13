<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customerlist.aspx.cs" MasterPageFile="~/Maddux.Catch.Master" Inherits="Maddux.Catch.customer.customerlist" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
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
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div style="margin-top: 10px" class="row top-buffer">
        <div class="col-6 col-lg-3">
            <asp:ListBox ID="ddlFilterMembership" SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect multiselectMembership"></asp:ListBox>
        </div>
        <div class="col-6 col-lg-3">
            <asp:ListBox ID="ddlFilterRegion" SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect multiselectRegion"></asp:ListBox>
        </div>
        <div class="col-6 col-lg-3">
            <asp:ListBox ID="ddlFilterStarRating" SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect multiselectRating">
                <asp:ListItem Value="5">5 Star Rating</asp:ListItem>
                <asp:ListItem Value="4">4 Star Rating</asp:ListItem>
                <asp:ListItem Value="3">3 Star Rating</asp:ListItem>
                <asp:ListItem Value="2">2 Star Rating</asp:ListItem>
                <asp:ListItem Value="1">1 Star Rating</asp:ListItem>
                <asp:ListItem Value="0">0 Star Rating</asp:ListItem>
                <asp:ListItem Value="999">No Rating</asp:ListItem>
            </asp:ListBox>
        </div>
        <div class="col-6 col-lg-3">
            <asp:Button Text="Search" Width="300" CssClass="btn btn-primary btn-sm" ID="btnSearch" OnClick="btnSearch_Click" runat="server" />
        </div>
    </div>
    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvCustomers" runat="server"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal"
                    AutoGenerateColumns="False"
                    ShowHeaderWhenEmpty="true"
                    AllowSorting="true"
                    ShowFooter="true"
                    SortMode="Automatic"
                    PageSize="25"
                    DataKeyNames="CustomerId"
                    EmptyDataText="There are no customers to display.">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>Customer</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='customerdetail.aspx?CustomerID=<%# Eval("CustomerID")%>'>
                                    <%# Eval("Company") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="350" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Star Rating</HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("StarRatingGraphic") %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Contact" HeaderText="Contact" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                        <asp:BoundField DataField="City" HeaderText="City" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                        <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                        <asp:TemplateField>
                            <HeaderTemplate>Last order placed</HeaderTemplate>
                            <ItemTemplate>
                                <%# !string.IsNullOrEmpty(Eval("LastOrderDate","{0:MMMM dd, yyyy}")) ? Eval("LastOrderDate","{0:MMMM dd, yyyy}")  : "N/A" %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/scripts/bootstrap-datetimepicker.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap-multiselect.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.multiselectMembership').multiselect({
                buttonContainer: '<div class="btn-group" />',
                buttonWidth: "350px",
                includeSelectAllOption: false,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                nonSelectedText: '-- Any Association --',
            });
            $('.multiselectRating').multiselect({
                buttonContainer: '<div class="btn-group" />',
                buttonWidth: "350px",
                includeSelectAllOption: false,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                nonSelectedText: '-- Any Star Rating --',
            });
            $('.multiselectRegion').multiselect({
                buttonContainer: '<div class="btn-group" />',
                buttonWidth: "350px",
                includeSelectAllOption: false,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                nonSelectedText: '-- Any Region --',
            });
        });
    </script>
</asp:Content>
