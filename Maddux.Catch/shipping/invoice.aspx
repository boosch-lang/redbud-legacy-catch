<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="invoice.aspx.cs" Inherits="Maddux.Catch.shipping.invoice" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/rbstyle.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/print-css.css") %>" />
    <style>
        tr td {
            padding: 2px !important;
        }

        label {
            margin-bottom: 0;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="container">
            <div class="row">
                <div class="col-xs-3">
                    <img class="set-logo-width" src="https://catch.redbud.com/img/Redbud_logo_black.png" />
                </div>
                <div class="col-xs-4">
                    P.O. Box 81187
                    <br />
                    Ancaster, ON L9G 4X2
                    <br />
                    Phone: 1-888-733-2830
                    <br />
                    Fax: 1-888-733-2850
                    <br />
                    Email: sales@redbud.com
                </div>
                <div class="col-xs-5">
                    <div class="text-align: right;">
                        <div id="lblType" runat="server" class="h1" style="margin: 0;">
                            Invoice
                        </div>
                        <div class="row">
                            <label class="col-xs-6">Invoice Number:</label>
                            <div class="col-xs-6">
                                <label runat="server" id="lblInvoiceNumber" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <hr style="border-top: 1px solid #000; margin: 0;" />
            <br />
            <div class="row">
                <div class="col-xs-4">
                    <p style="font-weight: bold">Bill To:</p>
                    <div>
                        <label runat="server" id="lblBillingName" />
                    </div>
                    <div>
                        <label runat="server" id="lblBillingAddress" />
                    </div>
                    <div>
                        <label runat="server" id="lblBillingCity" />
                        ,
                        <label runat="server" id="lblBillingState" />
                        &nbsp;<label runat="server" id="lblBillingZip" />
                    </div>
                </div>
                <div class="col-xs-4">
                    <p style="font-weight: bold">Ship To:</p>
                    <div>
                        <label runat="server" id="lblShippingName" />
                    </div>
                    <div>
                        <label runat="server" id="lblShippingAddress" />
                    </div>
                    <div>
                        <label runat="server" id="lblShippingCity" />
                        ,
                        <label runat="server" id="lblShippingState" />
                        &nbsp;<label runat="server" id="lblShippingPostal" />
                    </div>
                </div>
                <div class="col-xs-4">
                    <div class="row">
                        <label class="col-xs-6">Invoice Date:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblInvoiceDate" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-6">Order Number:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblID" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-6">Purchase Order #:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblPONumber" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-6">Terms:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblTerms" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-6">Vendor Number:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblVendorNumber" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-6">Sales Rep:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblSalesperson" />
                        </div>
                    </div>
                    <div class="row d-none">
                        <label class="col-xs-6">Ship Via:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblShippingMethod" />
                        </div>
                    </div>
                    <div class="row d-none">
                        <label class="col-xs-6">Requested Ship Date:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblRequestedShipDate" />
                        </div>
                    </div>
                    <div id="shipDateArea" runat="server">
                        <div class="row d-none">
                            <label class="col-xs-6">Ship Date:</label>
                            <div class="col-xs-6">
                                <label runat="server" id="lblShipDate" />
                            </div>
                        </div>
                        <div class="row d-none">
                            <label class="col-xs-6">Order Confirmation Date:</label>
                            <div class="col-xs-6">
                                <label runat="server" id="lblConfirmationSent" />
                            </div>
                        </div>
                        <div class="row d-none">
                            <label class="col-xs-6">PO Sent Date:</label>
                            <div class="col-xs-6">
                                <label runat="server" id="lblPOSent" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <div class="row">
                <div class="col-xs-12">
                    <div style="background-color: lightgray; padding: 2px">
                        <label>Catalog:</label>
                        <label runat="server" id="lblRackName" />
                    </div>
                </div>
                <div class="col-xs-12">
                    <asp:GridView ID="dgvProducts" runat="server"
                        CssClass="table table-sm"
                        GridLines="None" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                        ShowHeader="true" ShowFooter="false"
                        EmptyDataText="There are no products on this rack.">
                        <Columns>
                            <asp:BoundField
                                DataField="ItemNo"
                                HeaderText="Item Number"
                                ItemStyle-CssClass="col-xs-2" />
                            <asp:BoundField
                                DataField="Quantity"
                                HeaderText="Qty"
                                SortExpression="DefaultQuantity"
                                ItemStyle-CssClass="col-xs-1" />
                            <asp:BoundField DataField="ProductName"
                                HeaderText="Description"
                                ItemStyle-CssClass="col-xs-4" />
                            <asp:BoundField
                                DataField="Discount"
                                HeaderText="Discount"
                                HeaderStyle-CssClass="text-right"
                                ItemStyle-CssClass="text-right col-xs-1" />
                            <asp:BoundField
                                DataField="DiscountedEachPrice"
                                HeaderText="Each Price"
                                DataFormatString="{0:C}"
                                HeaderStyle-CssClass="text-right"
                                ItemStyle-CssClass="text-right col-xs-2" />
                            <asp:BoundField
                                DataField="Total"
                                HeaderText="Total"
                                DataFormatString="{0:C}"
                                HeaderStyle-CssClass="text-right"
                                ItemStyle-CssClass="text-right col-xs-2" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div class="col-xs-8">
                    <label>Notes:</label>
                    <label id="lblnotes" runat="server"></label>
                </div>
                <div class="col-xs-4">
                    <div class="row">
                        <p class="col-xs-6">Sub Total</p>
                        <p class="col-xs-6 text-right" id="pSubtotal" runat="server"></p>
                    </div>
                    <div id="discountTotal" runat="server" class="row">
                        <p class="col-xs-6">Discount</p>
                        <p class="col-xs-6 text-right" id="pDiscountTotal" runat="server"></p>
                    </div>
                    <div id="discount1" runat="server" class="row">
                        <p class="col-xs-6" runat="server" id="lblDiscount1Desc">Discount 1:</p>
                        <div class="col-xs-6 text-right">
                            <p runat="server" id="lblDiscount1"></p>
                        </div>
                    </div>
                    <div id="discount2" runat="server" class="row">
                        <p class="col-xs-6" runat="server" id="lblDiscount2Desc">Discount 2:</p>
                        <div class="col-xs-6 text-right">
                            <p runat="server" id="lblDiscount2"></p>
                        </div>
                    </div>
                    <div id="discount3" runat="server" class="row">
                        <p class="col-xs-6" runat="server" id="lblDiscount3Desc">Discount 3:</p>
                        <div class="col-xs-6 text-right">
                            <p runat="server" id="lblDiscount3"></p>
                        </div>
                    </div>
                    <div id="discount4" runat="server" class="row">
                        <p class="col-xs-6" runat="server" id="lblDiscount4Desc">Discount 4:</p>
                        <div class="col-xs-6 text-right">
                            <p runat="server" id="lblDiscount4"></p>
                        </div>
                    </div>
                    <div id="discount5" runat="server" class="row">
                        <p class="col-xs-6" runat="server" id="lblDiscount5Desc">Discount 5:</p>
                        <div class="col-xs-6 text-right">
                            <p runat="server" id="lblDiscount5"></p>
                        </div>
                    </div>
                    <div class="row">
                        <p class="col-xs-6">Freight</p>
                        <p class="col-xs-6 text-right" id="pShipping" runat="server"></p>
                    </div>
                    <div id="gstDiv" class="row">
                        <p id="TaxTypeText" runat="server" class="col-xs-6">GST</p>
                        <p class="col-xs-6 text-right" id="GSTText" runat="server"></p>
                    </div>
                    <div class="row">
                        <p class="col-xs-6">Total</p>
                        <p class="col-xs-6 text-right" id="pTotal" runat="server"></p>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <div style="page-break-after: always"></div>
</body>
</html>
