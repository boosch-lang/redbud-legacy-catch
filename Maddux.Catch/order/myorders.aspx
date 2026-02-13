<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="myorders.aspx.cs" Inherits="Maddux.Catch.order.myorders" MaintainScrollPositionOnPostback="true" %>

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

        .dataTable, .dataTable td {
            border: none;
        }

        .no-wrap-text {
            white-space: nowrap; /* Prevents text from wrapping */
            overflow: hidden; /* Hides overflowing text */
            text-overflow: ellipsis; /* Adds "..." to indicate truncated text */
        }

        .w-150 {
            max-width: 150px;
        }

        .w-250 {
            max-width: 250px;
        }

        .w-100 {
            max-width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <asp:HiddenField runat="server" ID="hdnUpdatePOSentDate" ClientIDMode="Static" Value="" />
    <div class="col-xs-12" style="padding-left: 0px;">
        <div class="btn-group">
            <button type="button" class="btn btn-default btn-sm checkbox-toggle-on"><i class="fa fa-check-square-o"></i>Select All</button>
            <button type="button" class="btn btn-default btn-sm checkbox-toggle-off"><i class="fa fa-square-o"></i>Select None</button>
        </div>
        <div id="PrintButtons" style="display: none">
            <br />
            <asp:Button Text="Delete Orders" CssClass="btn btn-danger btn-sm" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete these orders?');" Visible="false" ClientIDMode="Static" OnClick="btnDelete_Click" runat="server" />
        </div>
        <div class="pull-right">
            <asp:DropDownList ID="ddlFilterProvince" runat="server" CssClass="btn btn-default btn-sm">
            </asp:DropDownList>
            <asp:ListBox ID="ddlFilterCatalog" SelectionMode="Multiple" runat="server" CssClass="btn btn-default btn-sm multiselect"></asp:ListBox>
            <asp:DropDownList runat="server" ID="ddlFilterShipDate" CssClass="btn btn-default btn-sm">
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddlFilterApproved" CssClass="btn btn-default btn-sm">
                <asp:ListItem Selected="true" Value="">-- All Orders --</asp:ListItem>
                <asp:ListItem Value="1">Approved Orders</asp:ListItem>
                <asp:ListItem Value="0">Not Approved Orders</asp:ListItem>
            </asp:DropDownList>
            <asp:Button Text="Search" CssClass="btn btn-primary btn-sm" ID="btnSearchOrder" OnClick="btnSearchOrder_Click" runat="server" />
        </div>
    </div>
    <div class="row top-buffer">
        <div class="col-sm-12 ">

            <div class="table-responsive">
                <asp:GridView ID="dgvOrders"
                    runat="server"
                    AllowPaging="false"
                    AllowSorting="true"
                    SortMode="Automatic"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal"
                    AutoGenerateColumns="False"
                    ShowHeaderWhenEmpty="true"
                    ShowFooter="true"
                    EnableModelValidation="True"
                    CellSpacing="5"
                    EmptyDataText="There are no orders to display."
                    OnRowDataBound="dgvOrders_RowDataBound"
                    >

                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>

                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1">
                            <HeaderTemplate>Order #</HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Right" CssClass="w-100" />
                            <ItemTemplate>
                                <asp:CheckBox Text="" ID="OrderSelector" class="OrderSelector" name="OrderSelector" runat="server" />
                                &nbsp;&nbsp;
                                <a target="_blank" href="orderdetail.aspx?id=<%# Eval("OrderID") %>"><%# Eval("OrderID") %></a>
                                <asp:HiddenField runat="server" ID="OrderID" Value='<%# Eval("OrderID") %>' />
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="w-100"/>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Approved" HeaderText="Order Approved" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RequestedShipDate" HeaderText="Req. Ship Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2">
                            <HeaderTemplate>Customer</HeaderTemplate>    
                            <ItemTemplate>
                                <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                    <%# Eval("Company") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="no-wrap-text w-250" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2">
                            <HeaderTemplate>Star Rating</HeaderTemplate>
                            <ItemTemplate>
                                <%# GenerateStarRatingGraphic((int?)Eval("StarRating")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="State" HeaderText="Province" HeaderStyle-CssClass="visible-md visible-lg col-1" ItemStyle-CssClass="visible-md visible-lg col-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" HeaderStyle-CssClass="visible-md visible-lg col-1" ItemStyle-CssClass="visible-md visible-lg col-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="ConfirmationSentDate" HeaderText="Conf. Sent Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CatalogName" HeaderText="Catalog" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-2" />
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1">
                            <HeaderTemplate>Rack</HeaderTemplate>
                            <HeaderStyle CssClass="no-wrap-text" />
                            <ItemTemplate>
                                <a target="_blank" href="/racks/rackdetail.aspx?RackID=<%# Eval("RackID") %>"><%# Eval("RackName") %></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="no-wrap-text w-150"/>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Bulk Rack</HeaderTemplate>
                            <HeaderStyle CssClass="no-wrap-text" />
                            <ItemTemplate>
                                <a target="_blank" href="/racks/rackdetail.aspx?RackID=<%# Eval("BulkRackID") %>"><%# Eval("BulkRackName") %></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="no-wrap-text w-150" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrderTotal" HeaderText="Total" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-1">
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:BoundField>

                    </Columns>
                    <EmptyDataRowStyle CssClass="grdNoData" />
                    <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                    <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" PreviousPageText="<" PageButtonCount="5" NextPageText=">" LastPageText=">>" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <div id="ShipperInformationModal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title">Shipper Information
                        <span class="pull-right">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </span>
                    </h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <div id="ShipperInfoMessage"></div>
                        </div>

                        <div class="col-xs-12">
                            <label class="font-weight-bold">Day & Ross Acct No.</label>
                            <asp:TextBox runat="server" ID="txtBolShipperAccountNo" CssClass="form-control" />
                        </div>
                        <div class="col-xs-12">
                            <label class="font-weight-bold">Shippers Name</label>
                            <asp:TextBox runat="server" ID="txtBolShipperName" ClientIDMode="Static" CssClass="form-control" />
                        </div>
                        <div class="col-xs-12">
                            <label class="font-weight-bold">Address</label>
                            <asp:TextBox runat="server" ID="txtBolShipperAddress" ClientIDMode="Static" CssClass="form-control" />
                        </div>

                        <div class="col-xs-4">
                            <label class="font-weight-bold">City</label>
                            <asp:TextBox runat="server" ID="txtBolShipperCity" ClientIDMode="Static" CssClass="form-control" />
                        </div>
                        <div class="col-xs-4">
                            <label class="font-weight-bold">Province</label>
                            <asp:DropDownList ID="ddlProvince" DataTextField="Text" DataValueField="Value" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-4">
                            <label class="font-weight-bold">Country</label>
                            <asp:TextBox runat="server" ID="txtBolShipperCountry" ClientIDMode="Static" Text="Canada" CssClass="form-control" />
                        </div>
                        <div class="col-xs-6">
                            <label class="font-weight-bold">Postal Code</label>
                            <asp:TextBox runat="server" ID="txtBolShipperPostal" ClientIDMode="Static" CssClass="form-control" />

                        </div>
                        <div class="col-xs-6">
                            <label class="font-weight-bold">Phone</label>
                            <asp:TextBox runat="server" ID="txtBolShipperPhone" ClientIDMode="Static" CssClass="form-control" />
                            <asp:RegularExpressionValidator ErrorMessage="Please enter valid number!" CssClass="text-danger" ValidationExpression="^[0-9]*$" ControlToValidate="txtBolShipperPhone" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button Text="Print BOLs" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="return ValidateShipperInfo()" ID="btnPrintBOLs" OnClick="btnPrintBOLs_Click" runat="server" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="updatePODateModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Update P.O. Sent Date
              <span class="pull-right">
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
              </span>
                    </h4>
                </div>
                <div class="modal-body">
                    <p>Would you like to update 'P.O. Sent' date?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" id="YesUpdateConfirmationDate" class="btn btn-primary">Yes</button>
                    <button type="button" id="NoUpdateConfirmationDate" class="btn btn-danger">No</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />

    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap-multiselect.js") %>"></script>
    <script>
        $(document).on("click", "span.OrderSelector", function (e) {
            var flag = false;
            $("span.OrderSelector").each(function (i, v) {
                var checkBox = $(v).find("input[type=checkbox]")
                if (checkBox.is(":checked")) {
                    flag = true;
                    $("#PrintButtons").show(200);
                }
            })
            if (!flag) {
                $("#PrintButtons").hide(200);
            }
        });


        $(document).ready(function () {

            $('.multiselect').multiselect({
                buttonContainer: '<div class="btn-group" />',
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: true,
                enableFullValueFiltering: true,
                enableCaseInsensitiveFiltering: true,
                nonSelectedText: '--All Catalogs--',
            });

            $('.multiselectNoSearch').multiselect({
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                includeSelectAllOption: false
            });

        });
        //btnPurchaseOrder
        $("#PONumberBtn").on("click", function (e) {
            e.preventDefault();
            $("#updatePODateModal").modal("show");

        })
        $("#YesUpdateConfirmationDate").on("click", function (e) {
            e.preventDefault();
            $("#updatePODateModal").modal("hide");
            $("#hdnUpdatePOSentDate").val("True");
            $("#btnPurchaseOrder").click();
        })
        $("#NoUpdateConfirmationDate").on("click", function (e) {
            e.preventDefault();
            $("#updatePODateModal").modal("hide");
            $("#hdnUpdatePOSentDate").val("False");
            $("#btnPurchaseOrder").click();
        })
        $(document).ready(function () {
            $(".btn-group").find(".multiselect").prop("style", "min-width:200px");
        })
        $("#PrintBOLBtn").on("click", function (e) {
            e.preventDefault();
            $("#ShipperInformationModal").modal("show");
        })
        function ValidateShipperInfo() {
            var txtBolShipperName = `${$("#txtBolShipperName").val()}`;
            var txtBolShipperAddress = `${$("#txtBolShipperAddress").val()}`;
            var txtBolShipperCity = `${$("#txtBolShipperCity").val()}`;
            var txtBolShipperPostal = `${$("#txtBolShipperPostal").val()}`;
            var txtBolShipperCountry = `${$("#txtBolShipperCountry").val()}`;

            if (txtBolShipperName.length == 0 || txtBolShipperAddress.length == 0 || txtBolShipperCity.length == 0 || txtBolShipperPostal.length == 0 || txtBolShipperCountry.length == 0) {
                $("#ShipperInfoMessage").append(generateError("Shipper information required!"));
                return false;
            }
            else {
                console.log("success!");
                $("#ShipperInformationModal").modal("toggle");
                return true;
            }
        }
        function generateError(message) {
            return `<div class='alert alert-danger'> 
                        <button type='button' class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='fa fa-times'></i>
                        </button > 
                        <span >${message}</ span ></div >
            `;
        }
        $('.checkbox-toggle-on').on('click', function (e) {
            toggleCheck(true);
            $("#PrintButtons").show(200);
        });

        $('.checkbox-toggle-off').on('click', function (e) {
            toggleCheck(false);
            $("#PrintButtons").hide(200);
        });
        function toggleCheck(checked) {
            $.each($('.OrderSelector').find('input'), function (i, item) {
                $(item).prop('checked', checked);
            });
        }
    </script>
</asp:Content>

