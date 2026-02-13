<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bill-of-lading.aspx.cs" Inherits="Maddux.Catch.order.bill_of_lading" %>

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
        .textbox-text-center {
            text-align: center !important;
        }

        input[type=text] {
            text-align: center !important;
        }

        .form-control {
            border-top: none !important;
            border-left: none !important;
            border-right: none !important;
            border-radius: 0px !important;
            border-bottom: solid 1px #000000 !important;
            box-shadow: none !important;
            height: 20px !important;
            background-color: rgba(0,0,0,0) !important;
            padding: 2px !important;
        }

        .orange-border {
            border: solid 2px #ff6a00 !important;
            padding: 5px !important;
        }

        .orange-border-top {
            border-top: solid 2px #ff6a00 !important;
            padding: 5px !important;
        }

        .v-center {
            display: flex;
            align-items: center;
        }

        .p-0 {
            padding: 0px !important;
        }

        body {
            font-size: 11px !important
        }

        .font-9 {
            font-size: 10px !important;
        }

        .form-control-box {
            width: 100% !important;
            border: solid 1px #000000 !important;
            height: 25px
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="container">
            <div class="row p-0">
                <div class="col-xs-3">
                    <img class="img-fluid" src="<%= ResolveUrl("https://catch.redbud.com/img/DayRossLogo.png") %>" />
                </div>
                <div class="col-xs-5">
                    <p class="h4 text-uppercase text-center">
                        STRAIGHT BILL
                        <br />
                        OF  LADING -
                        <br />
                        1.866.DAY.ROSS
                    </p>
                </div>
                <div class="col-xs-4">
                    <p class="h4 text-uppercase text-center">
                        PLACE PRO
                        <br />
                        STICKER HERE
                    </p>
                </div>
            </div>
            <div class="row p-0">
                <div class="col-xs-7">
                    <div class="row">
                        <div class="col-xs-4 p-0">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 33%">Day
                                        <br />
                                        <asp:TextBox ID="txtDay" Style="width: 60px" CssClass="form-control form-control-sm" runat="server" />
                                    </td>
                                    <td style="width: 33%">Month
                                        <br />
                                        <asp:TextBox ID="txtMonth" Style="width: 60px" CssClass="form-control form-control-sm" runat="server" />
                                    </td>
                                    <td style="width: 33%">Year
                                        <br />
                                        <asp:TextBox ID="txtYear" Style="width: 60px" CssClass="form-control form-control-sm" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-xs-4 p-0">
                            <label class="font-weight-bold">Level of Service</label>
                            <div class="mb-1">
                                <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 10px">General</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 10px">Expedited</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 10px">Truckload</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" CssClass="" runat="server" Checked="True" /><span style="margin-left: 10px">LTL</span>
                            </div>

                        </div>
                        <div class="col-xs-4">
                            <label class="font-weight-bold">C.O.D</label>
                            <div class="mb-1">
                                <div class="row">
                                    <div class="col-xs-4 p-0">
                                        <span>Amount</span>
                                    </div>
                                    <div class="col-xs-8 p-0">
                                        <asp:TextBox Style="width: 60px; padding: 0px" Text="$" CssClass="form-control form-control-sm" runat="server" />
                                    </div>
                                </div>
                                <div style="margin-top: 10px!important" class="row">
                                    <div class="col-xs-4 p-0">
                                        <span>Currency</span>
                                    </div>
                                    <div class="col-xs-8 p-0">
                                        <div class="mb-1">
                                            <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 15px">CDN</span>
                                        </div>
                                        <div>
                                            <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 15px">US</span>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 10px" class="row orange-border-top">
                        <div class="col-xs-4 p-0">
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="PrivateResidencePickUp" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Private Residence Pick Up</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="PrivateResidenceDelivery" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Private Residence Delivery</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="PalletsBeingReturned" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Pallets Being Returned</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="HazardousGoods" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Hazardous Goods*</span>
                            </div>
                        </div>
                        <div class="col-xs-3 p-0">
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="CheckBox1" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Tailgate Pick Up</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="CheckBox2" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Heated Service</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="CheckBox3" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Trade Show PU</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="CheckBox4" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">In Bond</span>
                            </div>
                        </div>
                        <div class="col-xs-5 p-0">
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="CheckBox5" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Tailgate Delivery</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="CheckBox6" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Temperature Control (TL Only)</span>
                            </div>
                            <div class="mb-1" style="padding: 0px">
                                <asp:CheckBox Text="" ID="CheckBox7" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Trade Show Delivery</span>
                            </div>
                        </div>
                        <div class="col-xs-12 p-0">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div>
                                        <asp:CheckBox Text="" ID="AppointmentDeliveryDate" ClientIDMode="Static" CssClass="" runat="server" /><span style="margin-left: 10px">Appointment Delivery (Date/Time)</span>
                                    </div>
                                </div>
                                <div class="col-xs-5 p-0">
                                    <asp:TextBox ID="AppointmentDeliveryDateText" CssClass="form-control form-control-sm" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div style="margin-left: -2px" class="col-xs-5 p-0">
                    <div class="orange-border">
                        <label style="margin-left: 15px" class="font-weight-bold">Pick Up Information</label>
                        <div class="row align-items-center">
                            <div class="col-xs-6 ">
                                <div class="v-center">
                                    <label for="PickUpTelephone" style="font-weight: 400; padding-top: 7px">Telephone Number:</label>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <asp:TextBox Style="padding: 0px" ID="PickUpTelephone" ClientIDMode="Static" CssClass="form-control form-control-sm" runat="server" />
                            </div>
                        </div>
                        <div class="row align-items-center">
                            <div class="col-xs-6 ">
                                <div class="v-center">
                                    <label for="PickupContactName" style="font-weight: 400; padding-top: 7px">Contact Name:</label>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <asp:TextBox Style="padding: 0px" ID="PickupContactName" ClientIDMode="Static" CssClass="form-control form-control-sm" runat="server" />
                            </div>
                        </div>
                        <div class="row align-items-center">
                            <div class="col-xs-6 ">
                                <div class="v-center">
                                    <label for="PickupReadyDate" style="font-weight: 400; padding-top: 7px">Ready Date & Time:</label>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <asp:TextBox Style="padding: 0px" ID="PickupReadyDate" ClientIDMode="Static" CssClass="form-control form-control-sm" runat="server" />
                            </div>
                        </div>
                        <div class="row align-items-center">
                            <div class="col-xs-6 ">
                                <div class="v-center">
                                    <label for="PickupClosingTime" style="font-weight: 400; padding-top: 7px">Closing Time:</label>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <asp:TextBox Style="padding: 0px" ID="PickupClosingTime" ClientIDMode="Static" CssClass="form-control form-control-sm" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: -2px" class="orange-border">
                        <label style="margin-left: 15px" class="font-weight-bold">Requested By</label>
                        <div class="row">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <div class="row">
                                    <div style="padding: 0px" class="col-xs-6">
                                        <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 10px">Shipper</span>
                                    </div>
                                    <div style="padding: 0px" class="col-xs-6">
                                        <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 10px">Bill to</span>
                                    </div>
                                </div>
                                <div style="margin-top: 5px" class="row">
                                    <div style="padding: 0px" class="col-xs-6">
                                        <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 10px">3rd Party</span>
                                    </div>
                                    <div style="padding: 0px" class="col-xs-6">
                                        <asp:CheckBox Text="" CssClass="" runat="server" /><span style="margin-left: 10px">Consignee</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-2"></div>
                            <div class="col-xs-12">
                                <br />
                                <div class="row">
                                    <div class="col-xs-3">Email</div>
                                    <div class="col-xs-9">
                                        <asp:TextBox ID="RequestedByEmail" CssClass="form-control form-control-sm" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin-top: -2px" class="orange-border row p-0">
                <div style="border-right: solid 1px #000000; padding-top: 10px; padding-bottom: 10px" class="col-xs-6 text-center">
                    <span class="font-weight-bold">SHIPPER</span>
                    <div class="row">
                        <div style="padding-left: 0px!important" class="col-xs-6 ">
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtShipperAccountNo" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Day & Ross Acct No.</span>
                        </div>
                        <div style="padding-right: 0px!important" class="col-xs-6 ">
                            <asp:TextBox runat="server" ID="txtShipperPhoneNo" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Telephone No.</span>
                        </div>
                        <div style="padding-right: 0px!important; padding-left: 0px" class="col-xs-12 ">
                            <asp:TextBox runat="server" ID="txtShipperName" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Shippers Name</span>
                        </div>
                        <div style="padding-right: 0px!important; padding-left: 0px" class="col-xs-12 ">
                            <asp:TextBox runat="server" ID="txtShipperPickupAddress" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Pick Up Address</span>
                        </div>
                        <div style="padding-left: 0px!important" class="col-xs-4 ">
                            <asp:TextBox runat="server" ID="txtShipperCity" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">City</span>
                        </div>
                        <div class="col-xs-1 ">
                            <asp:TextBox runat="server" ID="txtShipperProvince" Width="30" Text="" ClientIDMode="Static" CssClass="form-control p-0 textbox-text-center" />
                            <span class="font-9">Prov.</span>
                        </div>
                        <div class="col-xs-4 ">
                            <asp:TextBox runat="server" ID="txtShipperPostalCode" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Postal Code</span>
                        </div>
                    </div>
                </div>
                <div style="padding-top: 10px; padding-bottom: 10px" class="col-xs-6 text-center">
                    <span class="font-weight-bold">CONSIGNEE</span>
                    <div class="row">
                        <div style="padding-left: 0px!important" class="col-xs-6 ">
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtConsigneeAccountNo" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Day & Ross Acct No.</span>
                        </div>
                        <div style="padding-right: 0px!important" class="col-xs-6 ">
                            <asp:TextBox runat="server" ID="txtConsigneeTelephoneNo" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Telephone No.</span>
                        </div>
                        <div style="padding-right: 0px!important; padding-left: 0px" class="col-xs-12 ">
                            <asp:TextBox runat="server" ID="txtConsigneeName" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Consignee's Name</span>
                        </div>
                        <div style="padding-right: 0px!important; padding-left: 0px" class="col-xs-12 ">
                            <asp:TextBox runat="server" ID="txtConsigneeDeliveryAddress" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Delivery Address</span>
                        </div>
                        <div style="padding-left: 0px!important" class="col-xs-4 ">
                            <asp:TextBox runat="server" ID="txtConsigneeCity" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">City</span>
                        </div>
                        <div class="col-xs-1 ">
                            <asp:TextBox runat="server" ID="txtConsigneeProvince" Width="30" Text="" ClientIDMode="Static" CssClass="form-control p-0 textbox-text-center" />
                            <span class="font-9">Prov.</span>
                        </div>
                        <div class="col-xs-4 ">
                            <asp:TextBox runat="server" ID="txtConsigneePostalCode" ClientIDMode="Static" CssClass="form-control textbox-text-center" />
                            <span class="font-9">Postal Code</span>
                        </div>
                    </div>
                </div>
            </div>
            <div style="border-bottom: 2px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000" class="row p-0">
                <div style="padding-top: 10px; padding-bottom: 10px; border-right: 1px solid #000000" class="col-xs-6">
                    <div class="text-center"><span class="font-weight-bold">METHOD OF PAYMENT</span></div>

                    <div class="row p-0">
                        <div class="col-xs-6"></div>
                        <div class="col-xs-3">
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkPaymentMethodPrepaid" CssClass="" runat="server" Checked="true" /><span style="margin-left: 10px">Prepaid</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkPaymentMethodCollect" CssClass="" runat="server" /><span style="margin-left: 10px">Collect&nbsp; </span>
                            </div>
                        </div>
                        <div class="col-xs-3">
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkPaymentUSD" CssClass="" runat="server" /><span style="margin-left: 10px">USD</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkPaymentCDN" CssClass="" runat="server" /><span style="margin-left: 10px">CDN</span>
                            </div>
                        </div>
                        <div class="col-xs-5 p-0">
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkPaymentCashInAdvanc" CssClass="" runat="server" /><span style="margin-left: 10px">Cash in Advance</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkThirdParty" CssClass="" runat="server" /><span style="margin-left: 10px">Third Party Bill To</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkVisa" CssClass="" runat="server" /><span style="margin-left: 10px">Visa</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkAmericanExpress" CssClass="" runat="server" /><span style="margin-left: 10px">American Express</span>
                            </div>
                            <div class="mb-1">
                                <asp:CheckBox Text="" ID="chkMastercard" CssClass="" runat="server" /><span style="margin-left: 10px">Mastercard</span>
                            </div>
                        </div>
                        <div style="padding-top: 10px!important" class="col-xs-6 p-0">
                            <div class="row mb-1">
                                <div class="col-xs-5 text-center"><span>Amount</span></div>
                                <div class="col-xs-7 p-0">
                                    <asp:TextBox CssClass="form-control-box" runat="server" />
                                </div>
                            </div>
                            <div class="row mb-1">
                                <div class="col-xs-5 text-center"><span>Account No.</span></div>
                                <div class="col-xs-7 p-0">
                                    <asp:TextBox CssClass="form-control-box" runat="server" />
                                </div>
                            </div>
                            <div class="row mb-1">
                                <div class="col-xs-5 text-center p-0"><span>Card No.</span></div>
                                <div class="col-xs-7 p-0">
                                    <asp:TextBox CssClass="form-control-box" runat="server" />
                                </div>
                            </div>
                            <div class="row mb-1">
                                <div class="col-xs-5 text-center"><span>Expiry Date</span></div>
                                <div class="col-xs-7 p-0">
                                    <asp:TextBox CssClass="form-control-box" runat="server" />
                                </div>
                            </div>
                        </div>

                    </div>

                </div>
                <div style="padding-top: 10px; padding-bottom: 10px;" class="col-xs-6 text-center">
                    <span class="font-weight-bold">OTHER BILL TO PARTICULARS</span>
                    <br />
                    <br />
                    <div style="margin-top: 20px" class="row">
                        <div class="col-xs-2 text-left">
                            <label class="font-weight-normal" for="txtOthersBillTo">Bill To :</label>
                        </div>
                        <div class="col-xs-6">
                            <asp:TextBox runat="server" ID="txtOthersBillTo" Text="REDBUD SUPPLY" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-4"></div>
                    </div>
                    <div style="margin-top: 3px" class="row text-left">
                        <div class="col-xs-3 text-left">
                            <label class="font-weight-normal " for="txtOthersAddress">Address :</label>
                        </div>
                        <div class="col-xs-5">
                            <asp:TextBox runat="server" ID="txtOthersAddress" Text="P.O. BOX 81187" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-4"></div>
                    </div>
                    <div style="margin-top: 3px" class="row text-left">
                        <div class="col-xs-8 ">
                            <asp:TextBox runat="server" ID="txtOthersAddress2" Text="" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-4"></div>
                    </div>
                    <div style="margin-top: 3px" class="row text-left">
                        <div class="col-xs-3 ">
                            <asp:TextBox runat="server" ID="txtOthersCity" Text="ANCASTER" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <label class="font-weight-normal" for="txtOthersCity">City</label>
                        </div>
                        <div class="col-xs-2 ">
                            <asp:TextBox runat="server" ID="txtOthersProvince" Text="ON" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <label class="font-weight-normal" for="txtOthersProvince">Prov.</label>
                        </div>
                        <div class="col-xs-3">
                            <asp:TextBox runat="server" ID="txtOthersPostal" Text="L9G 4X2" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <label class="font-weight-normal" for="txtOthersPostal">Postal</label>
                        </div>
                        <div class="col-xs-4"></div>
                    </div>
                    <div style="margin-top: 3px" class="row text-left">
                        <div class="col-xs-3 text-left">
                            <label class="font-weight-normal " for="txtOthersGST">GST No:</label>
                        </div>
                        <div class="col-xs-5">
                            <asp:TextBox runat="server" ID="txtOthersGST" Text="" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-4"></div>
                    </div>
                </div>
            </div>
            <div style="border-bottom: solid 2px #ff6a00; border-left: solid 1px #000000; border-right: solid 1px #000000;" class="row p-0">
                <div style="border-right: solid 1px #000000; height: 67px" class="col-xs-2">
                    <label>Spot Quote No.</label>
                </div>
                <div class="col-xs-4 text-center">
                    <label>Routing or Special Instructions</label>
                    <asp:Literal ID="litSpecialInstruction" runat="server"></asp:Literal>
                </div>
                <div style="border-left: solid 1px #000000" class="col-xs-6">
                    <span>Maximum liability of $2.00/LB or $4.41/KG computed on the total actual weight unless declared valuations states otherwise. Please see additional terms and conditions on our website at www.dayross.ca or call 1.877.329.7677 and request that a copy be sent to you.</span>
                </div>
            </div>

            <div style="margin-top: -2px; border-top: solid 2px #ff6a00" class="row p-0">
                <div class="col-xs-12 p-0">
                    <table style="width: 100%; margin-bottom: 0px" class="table table-sm">
                        <tr>
                            <td class="col-xs-1 border-1 text-center">
                                <br />
                                <span>No. of Pcs.</span>
                            </td>
                            <td class="col-xs-4 border-1 text-center">
                                <br />
                                <span>Description of Articles and Special Marks</span>
                            </td>
                            <td class="col-xs-1 p-0 border-1 text-center">
                                <div>Dimensions</div>
                                <div style="border-top: solid 1px #000000"></div>
                                <div style="margin-top: 5px">LxWxH</div>
                            </td>
                            <td class="col-xs-2 border-1">
                                <div>Weight</div>
                                <asp:CheckBox Text="" ID="chkWeightLbs" CssClass="" runat="server" Checked="true"/><span style="margin-left: 10px; margin-right: 10px">LBS</span>
                                <asp:CheckBox Text="" ID="chkWeightKgs" CssClass="" runat="server" /><span style="margin-left: 10px">KGS</span>
                            </td>
                            <td class="col-xs-2 border-1">
                                <div>Declared Value</div>
                                <div style="margin-bottom: 5px">
                                    <asp:CheckBox Text="" ID="chkCDN" CssClass="" runat="server" /><span style="margin-left: 10px; margin-right: 10px">CDN</span>
                                    <asp:CheckBox Text="" ID="chkUSD" CssClass="" runat="server" /><span style="margin-left: 10px">US</span>
                                </div>
                            </td>
                            <td class="col-xs-2 border-1 text-center">
                                <br />
                                <span>Charges</span>
                            </td>
                        </tr>
                        <asp:Repeater runat="server" ID="rptOrderRack">
                            <ItemTemplate>
                                <tr>
                                    <td style="padding: 7px!important" class="col-xs-1 border-1 text-center">
                                        <span><%# Eval("NoOFPics") %></span>
                                    </td>
                                    <td class="col-xs-5 border-1 text-center">
                                        <span style="text-transform: uppercase;"><%# Eval("Catalogue") %></span>
                                    </td>
                                    <td class="col-xs-1 border-1 text-center">
                                        <span><%# Eval("Dimensions") %></span>
                                    </td>
                                    <td class="col-xs-2 border-1 text-center">
                                        <span><%# (double)Eval("Weight") > 0 ? Eval("Weight"): "" %></span>
                                    </td>
                                    <td class="col-xs-2 border-1 text-center">&nbsp;
                                    </td>
                                    <td class="col-xs-2 border-1 text-center">&nbsp;
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td style="padding: 7px!important" class="col-xs-1 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-5 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-1 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 7px!important" class="col-xs-1 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-5 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-1 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="col-xs-1 border-1 text-center">&nbsp;
                            </td>
                            <td colspan="2" class="col-xs-5 border-1 text-center">
                                <span>All used household goods and personal effects will be shipped at shipper's own risk of damage.</span>
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                            <td class="col-xs-2 border-1 text-center">&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row p-0">
                <div class="col-xs-3" style="padding-left: 0px; padding-right: 10px">
                    <span>The uniform TERMS OF CARRIAGE apply to this BILL OF LADING (See Term & Condition #1)</span>
                </div>
                <div class="col-xs-4" style="padding-left: 10px; padding-right: 10px">
                    <span>NOTICE OF CLAIM must be submitted in writing within sixty (60) days of delivery. (See Term & Condition #2)</span>
                </div>
                <div class="col-xs-5" style="padding-left: 10px; padding-right: 0px">
                    <span>The carrier's maximum liability is limited by the TERMS & CONDITIONS of the Bill of Lading. (See Term & Condition # 5)</span>
                </div>
            </div>
            <div style="margin-top: 5px!important" class="row p-0">
                <div class="col-xs-1 p-0">
                    <label for="txtShipperRefNo" class="font-weight-bold">Shipper's Ref No.</label>
                </div>
                <div class="col-xs-4 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="txtShipperRefNo" />
                </div>
                <div class="col-xs-1 p-0">
                    <label style="margin-left: 5px!important" for="txtPickUpDate" class="font-weight-bold">Pick-Up Date</label>
                </div>
                <div class="col-xs-3 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="txtPickUpDate" />
                </div>
                <div class="col-xs-1 p-0">
                    <label style="margin-left: 5px!important" for="txtNoOfPics" class="font-weight-bold">No. of Pcs.</label>
                </div>
                <div class="col-xs-2 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="txtNoOfPics" />
                </div>
            </div>
            <div style="margin-top: 5px!important" class="row p-0">
                <div class="col-xs-1 p-0">
                    <label for="txtSignature" class="font-weight-bold">Shipper's Signature</label>
                </div>
                <div class="col-xs-4 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="txtSignature" />
                </div>
                <div class="col-xs-1 p-0">
                    <label style="margin-left: 5px!important" for="txtDriver" class="font-weight-bold">Day & Ross Driver</label>
                </div>
                <div class="col-xs-3 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="txtDriver" />
                </div>
                <div class="col-xs-1 p-0">
                    <label style="margin-left: 5px!important" for="txtPurchaseOrder" class="font-weight-bold">Purchase Order</label>
                </div>
                <div class="col-xs-2 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="txtPurchaseOrder" />
                </div>
            </div>
            <div style="margin-top: 5px!important" class="row p-0">
                <div class="col-xs-1 p-0">
                    <label for="txtSignature" class="font-weight-bold">Print Name</label>
                </div>
                <div class="col-xs-4 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="txtPrintName" />
                </div>
                <div class="col-xs-1 p-0">
                    <label style="margin-left: 5px!important" for="txtDriver" class="font-weight-bold">Power Number</label>
                </div>
                <div class="col-xs-3 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="TextBox2" />
                </div>
                <div class="col-xs-1 p-0">
                    <label style="margin-left: 5px!important" for="txtPurchaseOrder" class="font-weight-bold">Trailer Number</label>
                </div>
                <div class="col-xs-2 p-0">
                    <asp:TextBox runat="server" CssClass="form-control-box-lg" ID="TextBox3" />
                </div>
            </div>
        </div>
    </form>

    <div style="page-break-after: always"></div>
    <script>
        $(".chk").each(function (i, v) {
            $(v).find("input[type=checkbox]").prop("class", "form-control");
        })
    </script>
</body>
</html>
