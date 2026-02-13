<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="associationdetail.aspx.cs" Inherits="Maddux.Catch.customer.associationdetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li class="active"><a href="#associationDetails" data-toggle="tab">Standard Banner Message</a></li>
        <li><a href="#salesDetails" data-toggle="tab">Sales Banner Message</a></li>
        <li><a href="#associationCustomers" data-toggle="tab">Customers</a></li>
    </ul>
    <div class="tab-content" style="padding: 15px">
        <div class="tab-pane fade in active" id="associationDetails">
            <!-- Association Details tab -->
            <asp:Literal Text="" ID="litAssociationDetails" runat="server" />
            <div class="form-group">
                <asp:Label Text="Banner Message" CssClass="font-weight-bold" for="BannerMessage" runat="server" />
                <asp:TextBox ID="BannerMessage" runat="server" CssClass="form-control tinyMCE" Rows="20" TabIndex="5" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <asp:Label Text="Banner Start Date" CssClass="font-weight-bold" for="StartDate" runat="server" />
                    <div class='input-group datepicker'>
                        <asp:TextBox runat="server" ID="bannerStartDate" data-date-format="MMMM DD, YYYY" CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="fa fa-calendar"></span>
                        </span>
                    </div>
                </div>
                <div class="col-md-6">
                    <asp:Label Text="Banner End Date" CssClass="font-weight-bold" for="EndDate" runat="server" />
                    <div class='input-group datepicker'>
                        <asp:TextBox runat="server" ID="bannerEndDate" data-date-format="MMMM DD, YYYY" CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="fa fa-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
            <!--
            <div class="form-group">
                <asp:Label Text="Tag Line" CssClass="font-weight-bold" for="TagLine" runat="server" />
                <asp:TextBox runat="server" ID="TagLine" ClientIDMode="Static" CssClass="form-control" />
            </div>
           -->
            <div id="DivDeleteBtn" style="margin-top: 5px!important" runat="server">
                <asp:Button Text="Save" ID="Save" CssClass="btn btn-primary" OnClick="Save_Click" runat="server" />
                <button class="btn btn-danger" id="btnDeleteAssociation">Delete</button>
            </div>

        </div>
        <div class="tab-pane fade in" id="salesDetails">
            <!-- Association Details tab -->
            <asp:Literal Text="" ID="litSalesDetails" runat="server" />
            <div class="form-group">
                <asp:Label Text="Sales Banner Message" CssClass="font-weight-bold" for="SalesBannerMessage" runat="server" />
                <asp:TextBox ID="SalesBannerMessage" runat="server" CssClass="form-control tinyMCE" Rows="20" TabIndex="5" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <asp:Label Text="Sales Banner Start Date" CssClass="font-weight-bold" for="SalesStartDate" runat="server" />
                    <div class='input-group datepicker'>
                        <asp:TextBox runat="server" ID="SalesStartDate" data-date-format="MMMM DD, YYYY" CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="fa fa-calendar"></span>
                        </span>
                    </div>
                </div>
                <div class="col-md-6">
                    <asp:Label Text="Sales Banner End Date" CssClass="font-weight-bold" for="SalesEndDate" runat="server" />
                    <div class='input-group datepicker'>
                        <asp:TextBox runat="server" ID="SalesEndDate" data-date-format="MMMM DD, YYYY" CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="fa fa-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
            <div style="margin-top: 5px!important" runat="server">
                <asp:Button Text="Save" ID="BtnSalesSave" CssClass="btn btn-primary" OnClick="Save_Click" runat="server" />
            </div>

        </div>
        <div class="tab-pane fade in" id="associationCustomers">
            <!-- List of customers in Association -->
            <div class="row">
                <div class="col-xs-12">
                    <div class="pull-right">
                        <asp:ListBox ID="ddlFilterStarRating" SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect multiselectRating">
                            <asp:ListItem Value="5">5 Star Rating</asp:ListItem>
                            <asp:ListItem Value="4">4 Star Rating</asp:ListItem>
                            <asp:ListItem Value="3">3 Star Rating</asp:ListItem>
                            <asp:ListItem Value="2">2 Star Rating</asp:ListItem>
                            <asp:ListItem Value="1">1 Star Rating</asp:ListItem>
                            <asp:ListItem Value="0">0 Star Rating</asp:ListItem>
                            <asp:ListItem Value="999">No Rating</asp:ListItem>
                        </asp:ListBox>
                        <asp:ListBox ID="ddlFilterRegion" SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect multiselectRegion"></asp:ListBox>
                        <asp:Button Text="Search" CssClass="btn btn-primary btn-sm" ID="btnSearch" OnClick="btnSearch_Click" runat="server" />
                    </div>
                </div>
                <div class="row top-buffer">
                    <div class="col-sm-12 ">
                        <div class="table-responsive">
                            <asp:GridView ID="dgvAssociation" runat="server"
                                CssClass="table table-hover table-bordered table-hover dataTable"
                                GridLines="Horizontal"
                                AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="true"
                                AllowSorting="true"
                                ShowFooter="true"
                                SortMode="Automatic"
                                PageSize="25"
                                OnPageIndexChanging="dgvAssociation_PageIndexChanging"
                                OnRowDataBound="dgvAssociation_RowDataBound"
                                DataKeyNames="CustomerId"
                                EmptyDataText="There are no associations to display.">
                                <%--<PagerStyle CssClass="pagination-ys" />--%>
                                <Columns>
                                    <%-- <asp:BoundField DataField="CustomerID" HeaderText="CustomerID#" HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-4 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4 col-md-2">
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>   --%>

                                    <asp:TemplateField>
                                        <HeaderTemplate>Customer</HeaderTemplate>
                                        <ItemTemplate>
                                            <a target="_blank" href='customerdetail.aspx?CustomerID=<%# Eval("CustomerID")%>'>
                                                <%# Eval("Company") %>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Star Rating</HeaderTemplate>
                                        <ItemTemplate>
                                            <%# GenerateStarRatingGraphic((int?)Eval("StarRating")) %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Contact" HeaderText="Contact" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                                    <asp:BoundField DataField="City" HeaderText="City" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                                    <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Last order placed</HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<%# Eval("LastOrderDate","{0:MMMM dd, yyyy}") %>--%>
                                            <%# !string.IsNullOrEmpty(Eval("LastOrderDate","{0:MMMM dd, yyyy}")) ? Eval("LastOrderDate","{0:MMMM dd, yyyy}")  : "N/A" %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="BlankCol" HeaderText="" HeaderStyle-CssClass= "visible-md visible-lg col-md-4" ItemStyle-CssClass="visible-md visible-lg col-md-4">
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" runat="server" clientidmode="static" id="deleteAssociationModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Delete Association!
              <span class="pull-right">
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
              </span>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div style="padding: 10px" class="col-12 text-center">
                            <h3 class="text-danger">Are you sure you want remove this association?</h3>
                            <h4>If you remove this association all the customers, catalogs and user will be un-associated from this association.</h4>
                            <h4 class="text-danger text-uppercase font-weight-bold">This action can not be un-done!</h4>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button Text="Yes, Delete" ID="Delete" CssClass="btn btn-danger" OnClick="Delete_Click" runat="server" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
    <script src="https://cloud.tinymce.com/stable/jquery.tinymce.min.js"></script>
    <script src="https://cloud.tinymce.com/stable/tinymce.min.js?apiKey=tq4yog56p3newhumnylnl4w3xhgpwknugdwnyxgsekhzaba7"></script>
    <script src="../js/tinyMCE.js"></script>
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap-multiselect.js") %>"></script>
    <script>
        $(document).on("click", "#btnDeleteAssociation", function (e) {
            e.preventDefault();
            $("#deleteAssociationModal").modal("show");
        });

        $(document).ready(function () {

            $('.multiselectRating').multiselect({
                buttonContainer: '<div class="btn-group" />',
                buttonWidth: "200px",
                includeSelectAllOption: false,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                nonSelectedText: '-- Any Star Rating --',
            });

            $('.multiselectRegion').multiselect({
                buttonContainer: '<div class="btn-group" />',
                buttonWidth: '200px',
                includeSelectAllOption: false,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                nonSelectedText: '-- All Regions --',
            });
        });
        

        window.onload = function () {

            var url = document.location.toString();
            if (url.match('#')) {
                $('.nav-tabs a[href="#' + url.split('#')[1] + '"]').tab('show');
            }

            //Change hash for page-reload
            $('.nav-tabs a[href="#' + url.split('#')[1] + '"]').on('shown', function (e) {
                window.location.hash = e.target.hash;
            });
        }
    </script>
</asp:Content>
