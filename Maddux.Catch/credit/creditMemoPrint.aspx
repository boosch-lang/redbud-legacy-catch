<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="creditMemoPrint.aspx.cs" Inherits="Maddux.Catch.credit.creditMemoPrint" %>

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

        label {
            font-weight: 400 !important;
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
                    <h1 id="lblType" style="font-weight: 600" runat="server">Credit Memo</h1>
                    <div class="row">
                        <label class="col-xs-6">Credit Memo #</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblCrediMemoNumber" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-6">Date:</label>
                        <div class="col-xs-6">
                            <label runat="server" id="lblMemoDate" />
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
                    <asp:GridView ID="dgvCreditItems" runat="server"
                        CssClass="table table-sm"
                        GridLines="None" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                        ShowHeader="true" ShowFooter="false"
                        EmptyDataText="There are no products on this rack.">
                        <Columns>
                            <asp:BoundField DataField="ItemNo" HeaderText="Item Number" HeaderStyle-CssClass="col-xs-2" ItemStyle-CssClass="col-xs-2" FooterStyle-CssClass="col-xs-2" />
                            <asp:BoundField DataField="Quantity" HeaderText="Qty" HeaderStyle-CssClass="col-xs-2" ItemStyle-CssClass="col-xs-2" FooterStyle-CssClass="col-xs-2" />
                            <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-CssClass="col-xs-4" ItemStyle-CssClass="col-xs-4" FooterStyle-CssClass="col-xs-4" />
                            <asp:BoundField DataField="EachPrice" HeaderText="Each Price" DataFormatString="{0:C}" HeaderStyle-CssClass="text-right col-xs-2" ItemStyle-CssClass="text-right col-xs-2" FooterStyle-CssClass="text-right col-xs-2" />
                            <asp:BoundField DataField="Total" HeaderText="Ext. Price" DataFormatString="{0:C}" HeaderStyle-CssClass="text-right col-xs-2" ItemStyle-CssClass="text-right col-xs-2" FooterStyle-CssClass="text-right col-xs-2" />
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
                                <div class="row">
                                    <p class="col-xs-6">Freight</p>
                                    <p class="col-xs-6 text-right" style="padding-right: 22px" id="pShipping" runat="server"></p>
                                </div>
                                <div id="gstDiv" class="row">
                                    <p id="TaxTypeText" runat="server" class="col-xs-6">GST</p>
                                    <p class="col-xs-6 text-right" style="padding-right: 22px" id="GSTText" runat="server"></p>
                                </div>
                                <hr style="margin-bottom: 5px!important; margin-top: 0px!important" />
                                <div class="row">
                                    <p class="col-xs-6">Total Credit:</p>
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

