<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="confirmation.aspx.cs" Inherits="Maddux.Catch.order.confirmation" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/font-awesome.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/rbstyle.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/print-css.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/jquery-2.2.3.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/moment.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/utils.js") %>"></script>
    <style>
        tr td {
            padding: 2px !important;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="container">
            <div class="row">
                <div class="col-xs-3">
                    <img class="set-logo-width" src="<%= ResolveUrl("https://catch.redbud.com/img/Redbud_logo_black.png") %>" />
                    <p>
                        P.O. Box 81187
                        <br />
                        Ancaster, ON L9G 4X2
                    </p>
                    <br />
                    <p>
                        Phone: 1-888-733-2830
                    <br />
                        Fax: 1-888-733-2850
                    <br />
                        Email: sales@redbud.com
                    </p>

                </div>
                <div class="col-xs-4"></div>
                <div class="col-xs-5">
                    <h1 id="lblType" runat="server">Order Confirmation</h1>
                    <div class="row">
                        <label class="col-xs-6">Date:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblDate" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-6">Order Date:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblOrderDate" />
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
                    <div class="row">
                        <label class="col-xs-6">Ship Via:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblShippingMethod" />
                        </div>
                    </div>
                    <div class="row">
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

            <div class="row">
                <div class="col-xs-12" style="border-bottom: 1px solid black">
                    &nbsp;
                </div>
            </div>
            <div class="row" style="padding-top: 5px">
                <div class="col-xs-6">
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
                        &nbsp;
                        <label runat="server" id="lblBillingZip" />
                    </div>
                </div>
                <div class="col-xs-6">
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
                        &nbsp;
                        <label runat="server" id="lblShippingPostal" />
                    </div>
                </div>
            </div>
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
                            <asp:BoundField DataField="ItemNo" HeaderText="Item Number" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-1 col-lg-1" />
                            <asp:BoundField DataField="Quantity" HeaderText="Qty" SortExpression="DefaultQuantity" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-1 col-lg-1" />
                            <asp:BoundField DataField="ProductName" HeaderText="Description" SortExpression="ProductName" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-5" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-5" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-md-4 col-lg-4" />
                            <asp:BoundField DataField="UPCCode" HeaderText="UPC" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-2" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2 col-lg-2" />
                            <asp:BoundField DataField="Discount" HeaderText="Discount" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-1 col-lg-1" />
                            <asp:BoundField DataField="EachPrice" HeaderText="Each Price" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-1 col-lg-1" />
                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" HeaderStyle-CssClass="text-right visible-xs visible-sm visible-md visible-lg col-1" ItemStyle-CssClass="text-right visible-xs visible-sm visible-md visible-lg col-1" FooterStyle-CssClass="text-right visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2 col-lg-2" />
                        </Columns>
                    </asp:GridView>
                    <hr />
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 65%; padding: 10px">
                                <label>Notes:</label>
                                <label id="lblnotes" runat="server"></label>
                            </td>
                            <td style="width: 35%">
                                <div class="row">
                                    <p class="col-xs-6">Sub Total</p>
                                    <p class="col-xs-6 text-right" style="padding-right: 22px" id="pSubtotal" runat="server"></p>
                                </div>
                                <div id="discount1" runat="server" class="row">
                                    <label class="col-xs-6" runat="server" id="lblDiscount1Desc">Discount 1:</label>
                                    <div class="col-xs-6 text-right">
                                        <label runat="server" id="lblDiscount1" />
                                    </div>
                                </div>
                                <div id="discount2" runat="server" class="row">
                                    <label class="col-xs-6" runat="server" id="lblDiscount2Desc">Discount 2:</label>
                                    <div class="col-xs-6 text-right">
                                        <label runat="server" id="lblDiscount2" />
                                    </div>
                                </div>

                                <div id="discount3" runat="server" class="row">
                                    <label class="col-xs-6" runat="server" id="lblDiscount3Desc">Discount 3:</label>
                                    <div class="col-xs-6 text-right">
                                        <label runat="server" id="lblDiscount3" />
                                    </div>
                                </div>
                                <div id="discount4" runat="server" class="row">
                                    <label class="col-xs-6" runat="server" id="lblDiscount4Desc">Discount 4:</label>
                                    <div class="col-xs-6 text-right">
                                        <label runat="server" id="lblDiscount4" />
                                    </div>
                                </div> 
                                <div id="discount5" runat="server" class="row">
                                    <label class="col-xs-6" runat="server" id="lblDiscount5Desc">Discount 5:</label>
                                    <div class="col-xs-6 text-right">
                                        <label runat="server" id="lblDiscount5" />
                                    </div>
                                </div>
                                <div class="row">
                                    <p class="col-xs-6">Freight</p>
                                    <p class="col-xs-6 text-right" style="padding-right: 22px" id="pShipping" runat="server"></p>
                                </div>
                                <div id="gstDiv" class="row">
                                    <p id="TaxTypeText" runat="server" class="col-xs-6">GST</p>
                                    <p class="col-xs-6 text-right" style="padding-right: 22px" id="GSTText" runat="server"></p>
                                </div>
                                <div class="row">
                                    <p class="col-xs-6">Total</p>
                                    <p class="col-xs-6 text-right" style="padding-right: 22px" id="pTotal" runat="server"></p>
                                </div>

                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </div>
        </div>
    </form>
    <div style="page-break-after: always"></div>

</body>
</html>
