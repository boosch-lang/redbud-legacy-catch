<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="orderdetail.aspx.cs" Inherits="Maddux.Catch.order.orderdetail" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/Maddux.Catch.Master" %>
<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="../css/extra.css" rel="stylesheet" />
    <style>
        .font-weight-bold {
            font-weight: bold !important
        }

        .readonly {
            background-color: #eee;
            opacity: 1;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:HiddenField ID="currentReckPrice" Value="" runat="server" />
    <asp:HiddenField ID="expandCustomizeId" Value="" runat="server" />
    <asp:HiddenField runat="server" ID="CurrentTab" ClientIDMode="Static" Value="details" />
    <br />
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div id="ErrorText"></div>
    <div class="row row-margin">        
        <div class="col-xs-4">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete Order" runat="server" ID="btnDeleteOrder"
                OnClick="btnDeleteOrder_Click"
                CssClass="btn btn-danger" />
            <asp:Button Text="Approve" runat="server" ID="btnApprove"
                OnClientClick="return confirm('Are you sure you want to approve this order?')" CssClass="btn btn-primary"
                OnClick="btnApprove_Click" />
            <asp:Button Text="Renew" runat="server" ID="btnRenew"
                OnClientClick="return confirm('Are you sure you want to renew this order?')"
                CssClass="btn btn-primary" OnClick="btnRenew_Click" />
        </div>
        <div class="col-xs-8">
            <div style="margin-bottom: 5px" id="PrintAndEmailDiv" runat="server" class="text-right">
                <div class="btn-group">
                    <asp:Button Text="Print Confirmation" Style="border: solid 1px #808080" CausesValidation="false" ID="btnPrintConfirmation" OnClick="btnPrintConfirmation_Click" CssClass="btn btn-secondary btn-sm" runat="server" />
                    <button id="EmailConfirmBtn" style="border: solid 1px #808080" class="btn btn-secondary btn-sm">Email Confirmation</button>
                </div>
                <asp:Button Text="Print Pick Sheet" Style="border: solid 1px #808080" CausesValidation="false" ID="btnPrintPickSheet" OnClick="btnPrintPickSheet_Click" CssClass="btn btn-secondary btn-sm" runat="server" />
                <asp:Button Text="Email Pick Sheet" Style="border: solid 1px #808080" CausesValidation="false" ID="btnEmailPickSheet" OnClick="btnEmailPickSheet_Click" CssClass="btn btn-secondary btn-sm d-none" runat="server" />
                <button class="btn btn-secondary btn-sm" style="border: solid 1px #808080" id="PrintBOLBtn">Print B.O.L.</button>

            </div>
            <asp:Button Text="Send Confirmation" ID="btnEmailOrderConfirmation" ClientIDMode="Static" CausesValidation="false" OnClick="btnEmailOrderConfirmation_Click" Style="display: none" CssClass="btn btn-primary d-none" runat="server" />
        </div>
    </div>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li class="active"><a href="#details" data-toggle="tab">Details</a></li>
        <li id="tabItems" runat="server"><a href="#items" data-toggle="tab">Items</a></li>
        <li id="tabShipments" runat="server"><a href="#shipments" data-toggle="tab">Shipping</a></li>
    </ul>
    <div class="tab-content" style="padding: 15px">
        <div class="tab-pane fade in active" id="details">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <a class="panel-title" data-toggle="collapse" href="#orderDetails">Order Details</a>
                </div>
                <div id="orderDetails" class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Program </label>
                                    <asp:DropDownList runat="server" AutoPostBack="true" DataTextField="Text" DataValueField="Value"
                                        OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" ID="ddlPrograms"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row row-margin">
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Status </label>
                                    <asp:DropDownList runat="server" DataTextField="Text" DataValueField="Value" ID="ddOrderStatus"
                                        CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">P.O. Number </label>
                                    <asp:TextBox runat="server" ID="txtPONumber" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Sales Rep </label>
                                    <asp:DropDownList runat="server" ID="ddSalesRep" DataTextField="Text" DataValueField="Value"
                                        CssClass="form-control">
                                    </asp:DropDownList>

                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Order Placed By </label>
                                    <asp:TextBox runat="server" ID="txtOrderPlacedBy" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Order Date </label>
                                    <div class='input-group datepicker'>
                                        <asp:TextBox runat="server" ID="txtOrderDate" data-date-format="MMMM DD, YYYY"
                                            CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="fa fa-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Req. Ship Date </label>
                                    <asp:DropDownList ID="ddlShipDate" DataTextFormatString="{0:MMMM dd, yyyy}" runat="server"
                                        CssClass="form-control" />
                                </div>
                            </div>

                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Confirmation Sent </label>
                                    <div class='input-group datepicker'>
                                        <asp:TextBox runat="server" ID="txtConfirmationSent" data-date-format="MMMM DD, YYYY"
                                            CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="fa fa-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Order Approved</label>
                                    <asp:DropDownList ID="ddlOrderApproved" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Received Via</label>
                                    <asp:DropDownList ID="ddlOrderReceivedVia" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <label class="font-weight-bold">Office Notes</label>
                    <asp:TextBox ID="txtOfficeNotes" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <a class="panel-title" data-toggle="collapse" href="#purchaseOrderDetails">Purchase Order Details</a>
                </div>
                <div id="purchaseOrderDetails" class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-sm-6">
                                <label class="required font-weight-bold">Purchase Order</label>
                                <asp:DropDownList ID="ddlPurchaseOrders" DataTextField="Text" DataValueField="Value" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <asp:Panel ID="pnlPO" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-sm-3">
                                    <label class="required font-weight-bold">Total # Full Racks: </label>
                                    <div class="form-control readonly">
                                        <asp:Literal ID="litFullRacks" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="required font-weight-bold">Total # 1/2 Racks: </label>
                                    <div class="form-control readonly">
                                        <asp:Literal ID="litHalfRacks" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="required font-weight-bold">Total # 1/4 Racks: </label>
                                    <div class="form-control readonly">
                                        <asp:Literal ID="litQuarterRacks" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="required font-weight-bold">Allocated Space (feet): </label>
                                    <div class="form-control readonly">
                                        <asp:Literal ID="litAllocatedSapce" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <label class="required font-weight-bold">Date P.O. Sent </label>
                                    <div class='input-group datepicker'>
                                        <asp:TextBox runat="server" ID="txtPOSentDate" data-date-format="MMMM DD, YYYY"
                                            CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="fa fa-calendar"></span>
                                        </span>
                                    </div>

                                    <label for="ddlDeliveryHub" class="required font-weight-bold">Delivery Hub: </label>
                                    <div class="form-control readonly">
                                        <asp:Literal ID="litDeliveryHub" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label for="txtPickupDate" class="required font-weight-bold">Pick Up Date: </label>
                                    <div class="form-control readonly">
                                        <asp:Literal ID="litPickupDate" runat="server"></asp:Literal>
                                    </div>
                                    <label for="ddlShipDate" class="required font-weight-bold">Ship Date: </label>
                                    <div class="form-control readonly">
                                        <asp:Literal ID="litShipDate" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <a class="panel-title" data-toggle="collapse" href="#shippingDetails">Shipping/Billing Details
                    </a>
                </div>
                <div id="shippingDetails" class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-3 pl-0">
                                        <label class="h4 font-weight-bold">Shipping Details</label>
                                    </div>
                                    <div class="col-xs-9 text-right">
                                        <asp:Button CssClass="btn btn-success" CausesValidation="false" Text="Copy Billing Details" ID="btnCopyBilling"
                                            runat="server" OnClick="btnCopyBilling_Click" />
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Company </label>
                                    <asp:TextBox ID="txtShippingCompany" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger"
                                        ID="rfvShippingCompany" runat="server"
                                        ControlToValidate="txtShippingCompany" Display="Dynamic"
                                        ErrorMessage="You must enter a company name."></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Address </label>
                                    <asp:TextBox ID="txtShippingAddress" runat="server" TextMode="MultiLine" Rows="3"
                                        CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger"
                                        ID="rfvShippingAddress" runat="server"
                                        ControlToValidate="txtShippingAddress" Display="Dynamic"
                                        ErrorMessage="You must enter a shipping address."></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-xs-12">

                                    <div class="col-xs-4 pl-0">
                                        <label class="required font-weight-bold">City </label>
                                        <asp:TextBox ID="txtShippingCity" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                            ID="rfvShippingCity" runat="server"
                                            ControlToValidate="txtShippingCity" Display="Dynamic"
                                            ErrorMessage="You must enter the shipping city."></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-xs-4">
                                        <label class="required font-weight-bold">Province </label>
                                        <asp:DropDownList ID="ddShippingProvince" DataTextField="Text" DataValueField="Value"
                                            CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-4 pr-0">
                                        <label class="required font-weight-bold">Country </label>
                                        <asp:DropDownList ID="ddCountry" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-6 pl-0">
                                        <label class="required font-weight-bold">Postal Code </label>
                                        <asp:TextBox ID="txtShippingPostal" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                            ID="rfvShippingPostal" runat="server"
                                            ControlToValidate="txtShippingPostal" Display="Dynamic"
                                            ErrorMessage="You must enter the shipping postal code."></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-xs-6 pr-0">
                                        <label class="required font-weight-bold">Email </label>
                                        <asp:TextBox ID="txtShippingEmail" runat="server" type="email" CssClass="form-control"></asp:TextBox>
                                    </div>


                                </div>

                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Default Shipping Method </label>
                                    <asp:DropDownList runat="server" ID="ddShippingMethod" DataTextField="Text"
                                        DataValueField="Value" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-3 pl-0">
                                        <label class="h4 font-weight-bold">Billing Details</label>
                                    </div>
                                    <div class="col-xs-9 text-right">
                                        <asp:Button CssClass="btn btn-success" Text="Copy Shipping Details" ID="btnCopyShipping"
                                            runat="server" CausesValidation="false" OnClick="btnCopyShipping_Click" />
                                    </div>
                                </div>

                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Company </label>
                                    <asp:TextBox ID="txtBillingCompany" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger"
                                        ID="rfvBillingCompany" runat="server"
                                        ControlToValidate="txtBillingCompany" Display="Dynamic"
                                        ErrorMessage="You must enter a company name."></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Address </label>
                                    <asp:TextBox ID="txtBillingAddress" runat="server" TextMode="MultiLine" Rows="3"
                                        CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger"
                                        ID="rfvBillingAddress" runat="server"
                                        ControlToValidate="txtBillingAddress" Display="Dynamic"
                                        ErrorMessage="You must enter a billing address."></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-xs-12">
                                    <div class="col-xs-4 pl-0">
                                        <label class="required font-weight-bold">City</label>
                                        <asp:TextBox ID="txtBillingCity" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                            ID="rfvBillingCity" runat="server"
                                            ControlToValidate="txtBillingCity" Display="Dynamic"
                                            ErrorMessage="You must enter a billing city."></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-xs-4">
                                        <label class="required font-weight-bold">Province</label>
                                        <asp:DropDownList ID="ddBillingProvince" DataTextField="Text" DataValueField="Value"
                                            CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-4 pr-0">
                                        <label class="required font-weight-bold">Country </label>
                                        <asp:DropDownList ID="ddBillingCountry" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-12 pl-0">
                                        <label class="required font-weight-bold">Postal Code </label>
                                        <asp:TextBox ID="txtBillingPostal" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                            ID="rfvBillingPostal" runat="server"
                                            ControlToValidate="txtBillingPostal" Display="Dynamic"
                                            ErrorMessage="You must enter a billing postal code."></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-xs-6 pl-0">
                                        <label class="required font-weight-bold">Default Terms </label>
                                        <asp:DropDownList runat="server" ID="ddDefaultTerms" DataTextField="Text"
                                            DataValueField="Value" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-6 pr-0">
                                        <label class="required font-weight-bold">Default Payment Type </label>
                                        <asp:DropDownList runat="server" DataTextField="Text" DataValueField="Value"
                                            ID="ddDefaultPaymentType" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <label class="font-weight-bold">Order Notes</label>
                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="items">
            <asp:ScriptManager ID="ScriptManager2" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-xs-12">
                            <a id="lnkAddItems" runat="server" class="btn btn-primary" data-toggle="modal" data-target="#modalView" data-remote="false">
                                <i class="fa fa-plus"></i>Add Items
                            </a>
                            <asp:Button ID="btnRemoveSelectedItems" runat="server" Text="Remove Selected"
                                CssClass="btn btn-danger" OnClick="btnRemoveSelectedItems_Click" />
                            <button class="btn btn-success" id="btnShipSelectedConfirm">Ship Selected</button>
                            <span class="pull-right font-weight-bold">Tray Count: <span id="trayCount" runat="server"></span>
                            </span>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-xs-12">
                            <button id="btnSelectAll" class="btn btn-secondary btn-sm" title="select all"><i class="fa fa-check-square-o"></i>Select All</button>
                            <button id="btnDeSelectAll" class="btn btn-secondary btn-sm" title="select none"><i class="fa fa-square-o"></i>Select None</button>
                        </div>
                    </div>
                    <asp:GridView ID="dgvItems" runat="server" DataKeyNames="OrderItemID"
                        CssClass="table table-hover table-bordered table-hover dataTable"
                        GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                        ShowFooter="True" OnRowDataBound="dgvItems_RowDataBound"
                        EmptyDataText="There are no items to display.">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelected" CssClass="OrderItemChk" runat="server" Visible='<%# ((double)Eval("UnshippedQuantity")) > 0  %>'></asp:CheckBox>
                                    <asp:HiddenField ID="hdnOrderItemId" runat="server" Value='<%#Eval("OrderItemID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>Qty</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" type="number" runat="server" CssClass="form-control text-right tray-qty"
                                        Text='<%# Eval("Qty") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>N/A</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkProductAvailable" Checked='<%# Eval("ProductNotAvailable") %>'
                                        runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Category" HeaderText="Category" />
                            <asp:BoundField DataField="ItemNumber" HeaderText="Item #" />
                            <asp:TemplateField>
                                <HeaderTemplate>Description</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEditProduct" runat="server" Target="_blank" NavigateUrl='<%# "/products/productDetail.aspx?productid=" + Eval("ProductID")%>' Text='<%# Eval("Description")  %>'></asp:HyperLink>
                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Size" HeaderText="Size" />
                            <asp:BoundField DataField="PacksPerUnit" HeaderText="Pcks/Un." />
                            <asp:TemplateField>
                                <HeaderTemplate>Case Price</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control text-right"
                                        Text='<%# Eval("UnitPrice", "{0:C2}") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Discount</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control text-right"
                                        Text='<%# Eval("Discount", "{0:P2}") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DiscountedCasePrice" DataFormatString="{0:C2}" HeaderText="Discounted Case Price" />
                            <asp:BoundField DataField="Total" DataFormatString="{0:C2}" HeaderText="Total" />
                        </Columns>
                    </asp:GridView>
                    <div class="row" id="divTotals" runat="server" visible="false">
                        <div class="row">
                            <div class="col-md-6">
                            </div>
                            <div class="col-md-6" style="border-bottom: black 1px solid; border-top: black 1px solid">
                                <div class="col-md-10 text-right">
                                    <label>Discounted Sub Total:</label>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscountedSubTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6" style="border-bottom: black 1px solid;">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 1:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount1Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtGlobalDiscount1Pct" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblGlobalDiscount1Discount" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6" style="border-bottom: black 1px solid">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 2:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount2Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDiscount2Pct" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscount2" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6" style="border-bottom: black 1px solid">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 3:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount3Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDiscount3Pct" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscount3" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6" style="border-bottom: black 1px solid">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 4:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount4Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDiscount4Pct" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscount4" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6" style="border-bottom: black 1px solid">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 5:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount5Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDiscount5Pct" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscount5" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6">
                                <div class="col-md-8 text-right">
                                    <label>Discounted Sub Total:</label>
                                </div>
                                <div class="col-md-4 text-right">
                                    <asp:Label ID="lblDiscountedSubTotal2" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6">
                                <div class="col-md-5">
                                    <asp:CheckBox runat="server" ID="chkCustomFreightCharge" AutoPostBack="true"
                                        OnCheckedChanged="chkCustomFreightCharge_CheckedChanged" />
                                    <label>Override Freight Charge</label>
                                </div>
                                <div class="col-md-3 text-right">
                                    <label>Freight:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFreightCharge" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6">
                                <div class="col-md-8 text-right">
                                    <asp:Label ID="lblGSTCaption" runat="server" Text="GST:"></asp:Label>
                                </div>
                                <div class="col-md-4 text-right">
                                    <asp:Label ID="lblGST" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6"></div>
                            <div class="col-md-6" style="border-bottom: black 3px double;">
                                <div class="col-md-8 text-right">
                                    <asp:Label ID="lblHSTCaption" runat="server" Text="HST:" Visible="False"></asp:Label>
                                </div>
                                <div class="col-md-4 text-right">
                                    <asp:Label ID="lblHST" runat="server" Visible="False"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-8 text-right">
                                    <label>Total:</label>
                                </div>
                                <div class="col-md-4 text-right">
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="tab-pane fade" id="shipments">
            <asp:GridView ID="dgvShipments" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no shipments to display.">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-md-4"
                        ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-md-4">
                        <HeaderTemplate>Shipment #</HeaderTemplate>
                        <ItemTemplate>
                            <a href="/shipping/shipmentdetail.aspx?id=<%# Eval("ShipmentID") %>&customerId=<%= CustomerID %>"
                                title="Shipment <%# Eval("ShipmentID") %>">
                                <%# Eval("ShipmentID") %></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CreateDate" HeaderText="Date Created" />
                    <asp:BoundField DataField="DateShipped" HeaderText="Date Shipped" />
                    <asp:BoundField DataField="ShipVia" HeaderText="Ship Via" />
                    <asp:BoundField DataField="TrackingNumber" HeaderText="Tracking #" />
                    <asp:BoundField DataField="Total" HeaderText="Total" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div id="ShipSelectedConfirmModal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title">Ship Selected Items
                <span class="pull-right">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </span>
                    </h3>
                </div>
                <div class="modal-body">
                    <br />
                    <br />
                    <h4 class="text-center">Are you sure you want to ship selected items?</h4>
                    <br />
                    <br />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnShipSelected" runat="server" Text="Yes, Ship Selected" CssClass="btn btn-success" OnClick="btnShipSelected_Click" />
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
                    <asp:Button Text="Print B.O.L." CausesValidation="false" ID="btnPrintBOL" OnClientClick="return ValidateShipperInfo()" OnClick="btnPrintBOL_Click" CssClass="btn btn-primary" runat="server" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updatePanelTop" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
            <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <div class="row">
                                <h4 class="modal-title col-xs-11" id="modalViewTitle">Add Items</h4>
                                <div class="text-right col-xs-1">
                                    <a href="#" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></a>
                                </div>
                            </div>
                        </div>

                        <div class="modal-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Catalog:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="ddCatalogList" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddCatalogList_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>

                                        <div class="row">
                                            <div class="col-xs-12">
                                                <asp:GridView ID="grdOrderItems" runat="server"
                                                    CssClass="table table-hover table-bordered table-hover dataTable"
                                                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                                    ShowFooter="true"
                                                    EmptyDataText="There are no items to display.">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>Qty</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtUnits" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("ProductID") %>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Category" HeaderText="Category" />
                                                        <asp:BoundField DataField="PacksPerUnit" HeaderText="Pck / Unit" />
                                                        <asp:BoundField DataField="ItemsPerPack" HeaderText="It. / Pck" />
                                                        <asp:BoundField DataField="ItemNo" HeaderText="Item #" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>Description</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <a href="/products/productDetail.aspx?productid=<%# Eval("ProductID") %>" target="_blank">
                                                                    <%# Eval("Description") %></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Size" HeaderText="Size" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>Case Price</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="UnitPrice" runat="server" CssClass="form-control text-right" Text='<%#Eval("UnitPrice","{0:C2}") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>New</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkNew" runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="saveAndClose" CssClass="btn btn-primary" runat="server" OnClick="saveAndClose_Click" Text="Save and Close" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!--Email Modal -->
    <div class="modal fade" id="emailModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <span id="EmailTitle"></span>
                        <span class="pull-right">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </span>
                    </h4>

                </div>
                <div class="modal-body">
                    <asp:Literal Text="" ID="litEmailMessage" runat="server" />
                    <label class="font-weight-bold">To</label>
                    <asp:TextBox ID="emailTo" type="email" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Cc</label>
                    <asp:TextBox ID="emailCC" type="email" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Subject</label>
                    <asp:TextBox ID="emailSubject" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Body</label>
                    <asp:TextBox ID="emailBody" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="8" runat="server" />
                    <br />
                    <div class="">
                        <asp:CheckBox Text="" CssClass="" ID="chkUpdateConfirmationDate" Checked="true" runat="server" />&nbsp; <span style="font-weight: 700">Update Confirmation Sent date.</span>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button class="btn btn-primary" id="SendConfirmation">Send Confirmation</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">
        $(document).ready(function (e) {
            $(document).ready(function () {
                var tab = $("#CurrentTab").val();
                console.log(tab);
                $('.nav-tabs li a[href="#' + tab + '"]').tab('show');
            })
            $(".reset-btn").hide();
            $(".update-price-btn").hide();
            $("#btnApplyAll").click(function () {
                var qty = $("#QuantityForAll").val()
                $(".popup-quantity-text").val(qty);
            });
        });

        $(".reck-price").text("$ " + $("#cphBody_currentReckPrice").val())
        function resetPanel(e) {
            $.each($(".txtProductQuantity"), function (index, value) {
                $(this).val($(this).data('default'));
            });
            $("#cphBody_currentReckPrice").val(0);
            $(".ScriptDIV").empty();
        };
        $(".formatCurrency").on('keyup', function (evt) {
            if (evt.which != 110) {
                var val = $(this).val().replace(/[^0-9.]/g, '');
                if (isNaN(parseFloat(val))) {
                    $(this).val('');
                } else {
                    $(this).val('$' + val.toLocaleString());
                }
            }
        });

        function displayCustomize(e) {
            $(".update-price-btn").hide();
            $("#cphBody_currentReckPrice").val(0);
            $(".ScriptDIV").empty();
            $(".reset-btn").hide();
            var resetButton = e.parent().find(".reset-btn");
            var pricebutton = e.parent().find(".update-price-btn");
            pricebutton.show();
            resetButton.show();
            var checkbox = e.parent().parent().find(".chkCustomize");
            var checked = checkbox.prop("checked");

            if (checked == false) {
                checkbox.prop("checked", true);
                checked = true;
            }

            $(".chkCustomize").prop("checked", false);
            $(".customSection").hide();
            $(".customize-btn").show();
            $(e).hide();
            if (checked) {
                checkbox.prop("checked", true);
                checkbox.next(".customSection").show();
            }
        }

        $("#btnSelectAll").on("click", function (e) {
            e.preventDefault();
            $(".OrderItemChk input").prop("checked", true);
        })

        //btnDeSelectAll
        $("#btnDeSelectAll").on("click", function (e) {
            e.preventDefault();
            $(".OrderItemChk input").prop("checked", false);
        })
        $("#btnShipSelectedConfirm").on("click", function (e) {
            e.preventDefault();
            var flag = false;
            $(".OrderItemChk input").each(function (i, value) {

                if ($(value).is(":checked")) {
                    flag = true;
                }

            })
            if (flag === false) {
                $("#ErrorText").append(generateError("Please select items that you want ship."))
                return;
            }
            $("#ShipSelectedConfirmModal").modal("show");
        })
        $("#EmailConfirmBtn").on("click", function (e) {
            e.preventDefault();
            $("#EmailTitle").text("Email Confirmation")
            $("#emailModal").modal("show");
        })
        $("#SendConfirmation").on("click", function (e) {
            e.preventDefault();
            $("#emailModal").modal("hide");
            $("#btnEmailOrderConfirmation").click();
        })
        $("#cphBody_btnShipSelected").on("click", function () {
            $("#ShipSelectedConfirmModal").modal("hide");
            return true;
        })
        $("#PrintBOLBtn").on("click", function (e) {
            e.preventDefault();
            $("#ShipperInformationModal").modal("show");
        })

        function generateError(message) {
            return `<div class='alert alert-danger'> 
                        <button type='button' class='close' data-dismiss='alert' aria-label='Close'> 
                        <i class='fa fa-times'></i>
                        </button > 
                        <span >${message}</ span ></div >
            `;
        }
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
        $(document).ready(function (e) {
            updateTrayCount();
        });
        $(".tray-qty").on("change", function (e) {
            e.preventDefault();
            updateTrayCount();
        })
        function updateTrayCount() {
            var trayCount = 0;
            $(".tray-qty").each(function (index, val) {
                trayCount += parseInt($(val).val())
                console.log(trayCount)
            })
            $("#cphBody_trayCount").empty();
            $("#cphBody_trayCount").append(`${trayCount}`);
        }
    </script>


</asp:Content>
