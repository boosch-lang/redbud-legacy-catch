<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="purchaseorderdetail.aspx.cs" Inherits="Maddux.Catch.purchaseorder.purchaseorderdetail" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
    <style>
        .mx-0 {
            margin-left: 0px !important;
            margin-right: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <asp:HiddenField runat="server" ID="hdnUpdatePOSentDate" ClientIDMode="Static" Value="" />

    <div class="row row-margin">
        <div class="alert alert-success alert-dismissible col-xs-12" id="successAlert" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <strong>Success!</strong> <span id="spSuccessMessage" runat="server"></span>
        </div>

        <div class="col-xs-4">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete Purchase Order" runat="server" ID="btnDeletePurchaseOrder"
                OnClientClick="return ConfirmAction('Are you sure you want to delete this purchase order?')"
                OnClick="btnDeletePurchaseOrder_Click" CssClass="btn btn-danger" />
        </div>
        <div class="col-xs-8">

            <div class="pull-right">
                <span class="font-weight-bold" style="margin-right: 25px;">Total # Full Racks: <span id="fullCount" runat="server"></span>
                </span>
                <span class="font-weight-bold" style="margin-right: 25px;">Total # 1/2 Racks: <span id="halfCount" runat="server"></span>
                </span>
                <span class="font-weight-bold" style="margin-right: 25px;">Total # 1/4 Racks:  <span id="quarterCount" runat="server"></span>
                </span>
                <span class="font-weight-bold" style="margin-right: 25px;">Allocated Space (feet):  <span id="totalFeet" runat="server"></span>
                </span>
            </div>
        </div>
    </div>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li id="tab-item-details" class="active"><a href="#details" class="active" data-toggle="tab" id="tabDetails" runat="server">Details</a></li>
    </ul>
    <div class="tab-content" style="padding: 15px">
        <div class="tab-pane fade active in" id="details">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#purchaseOrderDetails">Purchase Order Details</a>
                    </h4>
                </div>
                <div id="purchaseOrderDetails" class="panel-collapse collapse in p-1" aria-expanded="true">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row mx-0">
                                <div class="col-12">
                                    <label for="txtName" class="required font-weight-bold">Name: </label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger"
                                        ID="rfvName" runat="server"
                                        ControlToValidate="txtName" Display="Dynamic"
                                        ErrorMessage="You must enter a purchase order name.">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row mx-0">
                                <div class="col-12">
                                    <label for="txtPickupDate" class="required font-weight-bold">Pick Up Date: </label>
                                    <div class='input-group datepicker'>
                                        <asp:TextBox ID="txtPickupDate" data-date-format="MMMM DD, YYYY" runat="server" CssClass="form-control" required="required" onkeydown="return false;"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="fa fa-calendar"></span>
                                        </span>
                                    </div>
                                    <asp:RequiredFieldValidator CssClass="text-danger"
                                        ID="rfvPickupDate" runat="server"
                                        ControlToValidate="txtPickupDate" Display="Dynamic"
                                        ErrorMessage="You must enter the pick up date."></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row mx-0">
                                <div class="col-12">
                                    <label for="ddlDeliveryHub" class="required font-weight-bold">Delivery Hub: </label>
                                    <asp:DropDownList ID="ddlDeliveryHub" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlDeliveryHub_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row mx-0">
                                <div class="col-12">
                                    <label for="ddlShipDate" class="required font-weight-bold">Ship Date: </label>
                                    <asp:DropDownList ID="ddlShipDate" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-primary" id="pnlOrders" runat="server" visible="false">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#purchaseOrderOrderDetails">Orders</a>
                    </h4>
                </div>
                <div id="orders" class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-md-12 ">
                                <div class="row">
                                    <div class="col-xs-4">
                                        <a href="/purchaseorder/manageorders.aspx?id=<%= PurchaseOrderID %>&sd=<%= ddlShipDate.SelectedValue %>" class="btn btn-primary"
                                            title="Add Orders" data-toggle="modal" data-target="#modalView" title_text="Add Orders" data-remote="false">
                                            <i class="fa fa-plus"></i>&nbsp;Add Orders
                                        </a>
                                        <asp:Button ID="btnRemoveSelectedOrders" runat="server" Text="Remove Selected"
                                            CssClass="btn btn-danger" OnClick="btnRemoveSelectedOrders_Click" />

                                    </div>
                                    <div class="col-xs-8">
                                        <div style="margin-bottom: 5px" id="PrintAndEmailDiv" runat="server" class="text-right">
                                            <button id="PONumberBtn" style="border: solid 1px #808080" class="btn btn-secondary btn-sm">Print Purchase Order</button>
                                            <asp:Button Text="Print Purchase Order" CssClass="btn btn-default btn-sm d-none" ID="btnPurchaseOrder" ClientIDMode="Static" OnClick="btnPurchaseOrder_Click" runat="server" />
                                            <button class="btn btn-secondary btn-sm" style="border: solid 1px #808080" id="PrintBOLBtn">Print BOLs</button>
                                            <asp:Button Text="Print Pick Sheets" Style="border: solid 1px #808080" CssClass="btn btn-secondary btn-sm" ID="btnPickSheets" OnClick="btnPickSheets_Click" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-12">
                                        <button id="btnSelectAll" class="btn btn-secondary btn-sm" title="select all"><i class="fa fa-check-square-o"></i>Select All</button>
                                        <button id="btnDeSelectAll" class="btn btn-secondary btn-sm" title="select none"><i class="fa fa-square-o"></i>Select None</button>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 ">
                                <div class="table-responsive">
                                    <asp:GridView ID="dgvOrders"
                                        runat="server"
                                        AllowPaging="false"
                                        AllowSorting="true"
                                        ShowFooter="true"
                                        SortMode="Automatic"
                                        CssClass="table table-hover table-bordered table-hover dataTable"
                                        GridLines="Horizontal"
                                        ShowHeaderWhenEmpty="true"
                                        EmptyDataText="There are no orders to display."
                                        AutoGenerateColumns="false"
                                        DataKeyNames="OrderID">
                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1">
                                                <HeaderTemplate>Order #</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox Text="" ID="OrderSelector" class="OrderSelector" name="OrderSelector" runat="server" />
                                                    &nbsp;&nbsp;
                                                    <a target="_blank" href="/order/orderdetail.aspx?id=<%# Eval("OrderID") %>"><%# Eval("OrderID") %></a>
                                                    <asp:HiddenField runat="server" ID="OrderID" Value='<%# Eval("OrderID") %>' />
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-1">
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RequestedShipDate" HeaderText="Req. Ship Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-1">
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2">
                                                <HeaderTemplate>Customer</HeaderTemplate>
                                                <ItemTemplate>
                                                    <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                                        <%# Eval("Customer.Company") %>
                                                    </a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2">
                                                <HeaderTemplate>Star Rating</HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Customer.StarRatingGraphic") %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Customer.State" HeaderText="Province" HeaderStyle-CssClass="visible-md visible-lg col-1" ItemStyle-CssClass="visible-md visible-lg col-1">
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Product</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Repeater ID="rptCatalog" runat="server" DataSource='<%#DataBinder.Eval(Container, "DataItem.OrderRacks") %>'>

                                                        <ItemTemplate>
                                                            <a target="_blank" href='/racks/rackdetail.aspx?RackID=<%# Eval("ProductCatalogRack.RackID")%>'>
                                                                <%# Eval("ProductCatalogRack.RackName") %>
                                                            </a>
                                                            <br />
                                                            <a target="_blank" href='/catalogs/catalogdetail.aspx?CatalogID=<%# Eval("ProductCatalogRack.CatalogID")%>'>(<%# Eval("ProductCatalogRack.CatalogName") %>)
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
                                        <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                                        <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" PreviousPageText="<" PageButtonCount="5" NextPageText=">" LastPageText=">>" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
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
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row">
                        <h4 class="modal-title col-xs-11" id="modalViewTitle">Loading...</h4>
                        <div class="text-right col-xs-1">
                            <a href="#" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></a>
                        </div>
                    </div>
                </div>
                <div id="divModalViewBody" class="modal-body">
                    <img src="img/loading.gif" alt="Loading..." id="imgLoadingModalContent" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphScript" runat="server">
    <script src="../js/extra.js"></script>
    <script type="text/javascript">
        $("#modalView").on("show.bs.modal", function (e) {
            $(this).find(".modal-title").text("Loading...");
            $(this).find(".modal-body").html("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

            var eventSource = $(e.relatedTarget);
            if (eventSource.text().length > 0) {
                $(this).find(".modal-title").text(eventSource.attr("title_text"));
                $(this).find(".modal-body").html('<div class="embed-responsive embed-responsive-4by3" id="iframeContainer"></div>');
                $('<iframe id="modal-popup-content"  style="border:none;" allowfullscreen="true"  />').appendTo('#iframeContainer');
                $('#modal-popup-content').attr("src", eventSource.attr("href"));
            }
        });

        $('#modalView').on('hidden.bs.modal', function (e) {
            $(this).find('iframe').html("").attr("src", "");
        });

        function ConfirmAction(Message) {
            if (confirm(Message) == true)
                return true;
            else
                return false;
        }

        function CloseModal(frameElement) {
            if (frameElement) {
                var dialog = $(frameElement).closest(".modal");
                if (dialog.length > 0) {
                    dialog.modal("hide");
                }
            }
        }
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

        $("#btnSelectAll").on("click", function (e) {
            e.preventDefault();
            $(".OrderSelector input").prop("checked", true);
        })

        //btnDeSelectAll
        $("#btnDeSelectAll").on("click", function (e) {
            e.preventDefault();
            $(".OrderSelector input").prop("checked", false);
        })
    </script>

</asp:Content>
