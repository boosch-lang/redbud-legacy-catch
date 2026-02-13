<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="shipmentdetail.aspx.cs" Inherits="Maddux.Catch.shipping.test" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row row-margin">
        <div class="col-xs-12">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete" runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this order?')"
                OnClick="btnDelete_Click" CssClass="btn btn-danger" />
        </div>
    </div>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li id="detailsTab" class="active"><a href="#details" data-toggle="tab">Details</a></li>
        <li id="itemsTab"><a href="#items" data-toggle="tab">Items</a></li>
    </ul>
    <div class="tab-content" style="padding: 15px">
        <div class="tab-pane fade in active" id="details">
            <div style="margin-bottom: 5px" class="row">
                <div style="margin-right: 15px" class="col-12 text-right">
                    <div class="btn-group">
                        <asp:Button Text="Print Packing Slip" OnClick="btnPrintPackingSlip_Click" ID="btnPrintPackingSlip" Style="border: solid 1px #808080" CssClass="btn btn-secondary btn-sm" runat="server" />
                        <asp:Button Text="Email Packing Slip" ID="btnEmailPackingSlip" ClientIDMode="Static" OnClick="btnEmailPackingSlip_Click" Style="border: solid 1px #808080" CssClass="btn btn-secondary btn-sm d-none" runat="server" />
                        <button id="EmailPackingSlipBtn" style="border: solid 1px #808080" class="btn btn-secondary btn-sm">Email Packing Slip</button>
                    </div>
                    <div class="btn-group">
                        <asp:Button Text="Print Invoice" ID="btnPrintInvoice" OnClick="btnPrintInvoice_Click" Style="border: solid 1px #808080" CssClass="btn btn-secondary btn-sm" runat="server" />
                        <asp:Button Text="Email Invoice" ID="btnEmailInvoice" ClientIDMode="Static" OnClick="btnEmailInvoice_Click" Style="border: solid 1px #808080" CssClass="btn btn-secondary btn-sm d-none" runat="server" />
                        <button id="EmailInvoiceBtn" style="border: solid 1px #808080" class="btn btn-secondary btn-sm">Email Invoice</button>
                    </div>

                </div>
            </div>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title"><a data-toggle="collapse" href="#shipmentDetails">Shipment Details</a>
                    </h3>
                </div>
                <div id="shipmentDetails" style="padding: 15px" class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-md-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-2">
                                        <label class="font-weight-bold">Ship To:</label>
                                    </div>
                                    <div class="col-xs-10">
                                        <asp:HyperLink ID="hlCustomer" runat="server" Target="_blank"></asp:HyperLink>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-2">
                                        <label class="font-weight-bold">Star Rating:</label>
                                    </div>
                                    <div class="col-xs-10">
                                        <asp:Literal ID="litStarRating" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-2">
                                        <label class="font-weight-bold">City</label>
                                    </div>
                                    <div class="col-xs-10">
                                        <asp:Label ID="lblCity" runat="server" Font-Bold="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-2">
                                        <label class="font-weight-bold">Address</label>
                                    </div>
                                    <div class="col-xs-10">
                                        <asp:Label ID="lblAddress" runat="server" Font-Bold="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-2">
                                        <label class="font-weight-bold">Postal Code</label>
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:Label ID="lblPostalCode" runat="server" Font-Bold="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-2">
                                        <label class="font-weight-bold">Country</label>
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:Label ID="lblCountry" runat="server" Font-Bold="false"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Date Created</label>
                                    <div class='input-group datepicker'>
                                        <asp:TextBox ID="txtDateCreated" runat="server" CssClass="form-control" data-date-format="MMMM DD, YYYY"
                                            onkeypress="return false;"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="fa fa-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Date Invoice Sent</label>
                                    <div class='input-group datepicker'>
                                        <asp:TextBox ID="txtInvoice" runat="server" CssClass="form-control" data-date-format="MMMM DD, YYYY"
                                            onkeypress="return false;"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="fa fa-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Date Shipped</label>
                                    <div class='input-group datepicker'>
                                        <asp:TextBox ID="txtShipDate" runat="server" CssClass="form-control" data-date-format="MMMM DD, YYYY"
                                            onkeypress="return false;"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="fa fa-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Ship Via</label>
                                    <asp:DropDownList ID="ddShipVia" DataTextField="Text" DataValueField="Value" CssClass="form-control"
                                        runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Tracking</label>
                                    <asp:TextBox ID="txtTracking" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>


            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Shipment Notes
                    </h3>
                </div>
                <div class="panel-body">
                    <div class="row row-margin">
                        <div class="col-xs-12">
                            <asp:TextBox ID="txtShipmentNotes" TextMode="MultiLine" Rows="5" CssClass="form-control"
                                runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Office Notes
                    </h3>
                </div>
                <div class="panel-body">
                    <div class="row row-margin">
                        <div class="col-xs-12">
                            <asp:TextBox ID="txtOfficeNotes" TextMode="MultiLine" Rows="5" CssClass="form-control"
                                runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="items">
            <div class="row" style="padding-bottom: 15px;">
                <div class="col-xs-12">
                    <asp:Button ID="btnRemoveSelectedItems" runat="server" Text="Remove Selected" CssClass="btn btn-danger"
                        OnClick="btnRemoveSelectedItems_Click" />
                </div>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:GridView ID="grdShipmentItems" runat="server"
                                CssClass="table table-hover table-bordered dataTable "
                                GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                ShowFooter="true" OnRowDataBound="grdShipmentItems_RowDataBound"
                                DataKeyNames="ShipmentItemId, OrderItemId"
                                EmptyDataText="There are no items to display.">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelected" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Qty Shipped</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQtyShipped" runat="server" Text='<%# Eval("QuantityShipped") %>'
                                                CssClass="form-control text-right" type="number" min="0" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="QuantityOrdered" HeaderText="Qty Ordered" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>N/A</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" Enabled="false" Checked='<%# Eval("ProductNotAvailable") %>'></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Category" HeaderText="Category" />
                                    <asp:BoundField DataField="ItemNo" HeaderText="Item #" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>Description</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkEditProduct" runat="server" NavigateUrl='<%# "/products/productDetail.aspx?productid=" + Eval("ProductID")%>' Text='<%# Eval("Description")  %>'></asp:HyperLink>
                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Size" HeaderText="Size" />
                                    <asp:BoundField DataField="PacksPerUnit" HeaderText="Pck / Unit" />
                                    <asp:BoundField DataField="ItemsPerPack" HeaderText="It. / Pck" />
                                    <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:P2}" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="row">
                        <div class="row">
                            <div class="col-md-7">
                            </div>
                            <div class="col-md-5" style="border-bottom: black 1px solid; border-top: black 1px solid">
                                <div class="col-md-10 text-right">
                                    <label>Discounted Sub Total:</label>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscountedSubTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-5" style="border-bottom: black 1px solid;">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 1:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount1Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtGlobalDiscount1Pct" runat="server"
                                        CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblGlobalDiscount1Discount" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-5" style="border-bottom: black 1px solid">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 2:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount2Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDiscount2Pct" MaxLength="8" runat="server"
                                        CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscount2" runat="server"></asp:Label>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-5" style="border-bottom: black 1px solid">
                                <div class="col-md-3 text-right">
                                    <label>Global Discount 3:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscount3Desc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDiscount3Pct" runat="server"
                                        CssClass="form-control text-right"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscount3" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-5" style="border-bottom: black 1px solid">
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
                            <div class="col-md-7"></div>
                            <div class="col-md-5" style="border-bottom: black 1px solid">
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
                            <div class="col-md-7"></div>
                            <div class="col-md-5">
                                <div class="col-md-10 text-right">
                                    <label>Discounted Sub Total:</label>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblDiscountedSubTotal2" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-5">
                                <div class="col-md-5">
                                    <asp:CheckBox runat="server" ID="chkCustomFreightCharge" AutoPostBack="true"
                                        OnCheckedChanged="chkCustomFreightCharge_CheckedChanged" />
                                    <label>Override Freight Charge</label>
                                </div>
                                <div class="col-md-5 text-right">
                                    <label>Freight:</label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtFreightCharge" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-5">
                                <div class="col-md-10 text-right">
                                    <asp:Label ID="lblGSTCaption" runat="server" Text="GST:"></asp:Label>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblGST" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-5" style="border-bottom: black 3px double;">
                                <div class="col-md-7">
                                </div>
                                <div class="col-md-3 text-right">
                                    <asp:Label ID="lblHSTCaption" runat="server" Text="HST:" Visible="False"></asp:Label>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblHST" runat="server" Visible="False"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-7">
                            </div>
                            <div class="col-md-5">
                                <div class="col-md-10 text-right">
                                    <label>Total:</label>
                                </div>
                                <div class="col-md-2 text-right">
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Email Invoice Modal -->
    <div class="modal fade" id="emailModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <span>Email Invoice</span>
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
                        <asp:CheckBox Text="" CssClass="" ID="chkUpdateInvoiceSentDate" ClientIDMode="Static" Checked="true" runat="server" />&nbsp; <span style="font-weight: 700">Update Invoice Sent date.</span>
                    </div>
                    <div class="row" id="InvoiceSentDateDiv">
                        <div class="col-md-6">
                            <label class="font-weight-bold">Select Invoice Date</label>
                            <div class='input-group datepicker'>
                                <asp:TextBox runat="server" ID="invoiceDate" data-date-format="MMMM DD, YYYY" CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                                <span class="input-group-addon">
                                    <span class="fa fa-calendar"></span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button class="btn btn-primary" id="SendInvoice">Send Invoice</button>
                </div>
            </div>
        </div>
    </div>
    <!--Email Packing Slip Modal -->
    <div class="modal fade" id="emailPackingSlipModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <span>Email Packing Slip</span>
                        <span class="pull-right">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </span>
                    </h4>

                </div>
                <div class="modal-body">
                    <label class="font-weight-bold">To</label>
                    <asp:TextBox ID="emailPackingSlipTo" type="email" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Cc</label>
                    <asp:TextBox ID="emailPackingSlipCc" type="email" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Subject</label>
                    <asp:TextBox ID="emailPackingSlipSubject" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Body</label>
                    <asp:TextBox ID="emailPackingSlipBody" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="8" runat="server" />
                    <br />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button class="btn btn-primary" id="SendPackingSlip">Send Packing Slip</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script>
        $("#EmailInvoiceBtn").on("click", function (e) {
            e.preventDefault();
            $("#emailModal").modal("show");
        })
        $("#SendInvoice").on("click", function (e) {
            e.preventDefault();
            $("#emailModal").modal("hide");
            $("#btnEmailInvoice").click();
        })

        $("#EmailPackingSlipBtn").on("click", function (e) {
            e.preventDefault();
            $("#emailPackingSlipModal").modal("show");
        })
        $("#SendPackingSlip").on("click", function (e) {
            e.preventDefault();
            $("#emailPackingSlipModal").modal("hide");
            $("#btnEmailPackingSlip").click();
        })
        $("#chkUpdateInvoiceSentDate").on("change", function (e) {
            if ($(this).is(":checked")) {
                $("#InvoiceSentDateDiv").show(200);
            }
            else {
                $("#InvoiceSentDateDiv").hide(200);
            }
        })
    </script>
</asp:Content>
