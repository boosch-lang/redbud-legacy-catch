<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="additem.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Maddux.Catch.order.additem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="../css/swipebox.css" />
    <style>
        .checkbox input[type=checkbox] {
            margin-left: 0px !important;
        }

        .modal-body {
            padding: 0 !important
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:HiddenField ID="currentReckPrice" Value="" runat="server" />
    <asp:Literal runat="server" ID="lbError"></asp:Literal>
    <div class="row">
        <div class="col-md-2">
            <label>Catalog:</label>
        </div>
        <div class="col-md-10">
            <asp:DropDownList ID="ddCatalogList" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddCatalogList_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12">
            <div class="row" id="SelectedCatalog">
                <asp:Repeater ID="rptRacks" runat="server" OnItemDataBound="rptRacks_ItemDataBound">
                    <ItemTemplate>
                        <asp:HiddenField ID="rackID" Value='<%# Eval("RackID") %>' runat="server" />
                        <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                            <div class="thumbnail text-left shadow-depth-1" style="background-color: #F4EBE4 !important">
                                <div class="row">
                                    <div class="col-lg-5 col-md-4 col-sm-12 col-xs-12">
                                        <div class="top-buffer-small" style="height: 280px;">
                                            <a href="forms/rackdetail.aspx?rackid=<%# Eval("RackID") %>" title="<%#Eval("RackDescription") %>" data-toggle="modal" data-target="#modalView" data-remote="false" title_text=" <%# Eval("ProgramName") + " - " + Eval("RackDescription") %>">
                                                <img src="<%#Eval("RackImage")%>" style="max-width: 100%; max-height: 100%" class="img-responsive center-block" data-toggle="modal" data-target=".modal-profile-lg" />
                                            </a>
                                        </div>
                                    </div>
                                    <div class="col-lg-7 col-md-8 col-sm-12 col-xs-12 top-buffer-extrasmall">

                                        <div class="row">
                                            <div class="col-xs-12 bottom-buffer">
                                                <input type="hidden" runat="server" id="hfRackName" />
                                                <a href="/racks/rackdetail.aspx?rackid=<%# Eval("RackID") %>" title="<%#Eval("RackDescription") %>" data-toggle="modal" data-target="#modalView" data-remote="false" title_text="<%# Eval("ProgramName") + " - " + Eval("RackDescription") %>">
                                                    <h4><%# Eval("RackDescription") %></h4>
                                                </a>
                                            </div>
                                        </div>

                                        <div class="row bottom-buffer-small">
                                            <div class="col-xs-5">
                                                Price:
                                            </div>
                                            <div class="col-xs-4">
                                                <asp:Label runat="server" ID="lblRackPrice" Style="font-weight: 700" class="input-form-label text-right" Text="$0.00"></asp:Label>
                                                <%--<label style="font-weight: bold;" class="input-form-label text-right <%# "S"+Eval("RackID") %>">$0.00</label>--%>
                                            </div>
                                        </div>

                                        <div class="row bottom-buffer-small">
                                            <div class="col-xs-5">
                                                Ship Week(s) Available:
                                            </div>
                                            <div class="col-xs-7">
                                                <asp:Panel ID="pnlNotAvailable" runat="server" Style="min-height: 150px;">
                                                    <label class="plain-label">Not currently available</label>
                                                </asp:Panel>
                                                <asp:Label runat="server" ID="lblShipDates" />
                                            </div>
                                        </div>
                                    </div>
                                    <div style="display: none" class="col-xs-12 text-right pb-5 clear-all-div clear-<%#Eval("RackID") %>">
                                        <button class="btn btn-warning btn-sm clear-all-btn">Clear All</button>
                                    </div>
                                    <div class="col-xs-12 rack-products-div">
                                        <asp:Panel ID="pnlCustomize" runat="server">
                                            <asp:HiddenField ID="KeepThisExpanded" Value="" runat="server" />
                                            <input type="checkbox" class="chkCustomize btn btn-default hidden" value="Customize" />
                                            <div class="collapse customSection <%#Eval("RackID") %>">
                                                <div class="row" style="padding-bottom: 20px;">
                                                    <div class="col-xs-4 font-weight-bold">Product</div>
                                                    <div class="col-xs-2 font-weight-bold text-center">Qty</div>
                                                    <div class="col-xs-3 font-weight-bold text-center">Each Price</div>
                                                    <div class="col-xs-2 font-weight-bold text-center">Order</div>
                                                </div>

                                                <asp:Repeater ID="rptProducts" runat="server">
                                                    <ItemTemplate>
                                                        <div style="<%#Eval("RackContainerMargin") %>; background-color: white">
                                                            <div class="row">
                                                                <asp:HiddenField ID="RackID" Value='<%#Eval("RackID") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnProductID" Value='<%#Eval("ProductID") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnUnitPrice" Value='<%#Eval("UnitPrice") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnIsFirstTime" Value='<%#Eval("IsFirstInRack") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnIsBulkRack" Value='<%#Eval("IsBulkRack") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnUPCCode" Value='<%#Eval("ProductUPCCode") %>' runat="server" />
                                                                <div class="col-xs-4">
                                                                    <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasProductPhoto") == true %>'>
                                                                        <button style="padding: 0px; white-space: normal;" class="gallery btn btn-link" id="<%#Eval("ProductID") %>" title="<%# Eval("ProductName") %>"><%# Eval("ProductName") %></button>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasProductPhoto") == false %>'>
                                                                        <div style="padding-left: "><%# Eval("ProductName") %></div>

                                                                    </asp:PlaceHolder>
                                                                </div>
                                                                <div class="col-xs-2 text-center">
                                                                    <%# Eval("UnitPerCase") %>
                                                                </div>
                                                                <div class="text-center col-xs-3">
                                                                    <span class="product-price"><%# Eval("UnitPriceFormatted") %></span>

                                                                </div>

                                                                <div class="col-xs-3 text-center">
                                                                    <asp:TextBox runat="server" Visible='<%# Eval("IsFirstInRack") %>' CssClass="form-control txtProductQuantity" type="number" Width="60" min="0" data-default='<%# Eval("DefaultQuantity") %>' ID="txtProductQuantity" Text='<%# Eval("DefaultQuantity") %>' />
                                                                    <div id=""></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </asp:Panel>
                                        <p class="text-center">
                                            <asp:Label runat="server" CssClass="text-dark" ID="lblReckSizeInfo" />
                                        </p>
                                        <div id="DivRackQtyWarning" runat="server" class="text-center "></div>
                                    </div>
                                    <div class="col-xs-12  text-center">
                                        <asp:Button Text="Customize" ID="CustomizeBtn" OnClientClick="return false;" runat="server" />
                                        <asp:Button ID="btnReset" runat="server" CssClass='btn btn-default reset-btn' Text="Reset" OnClick="BtnReset_Click" OnClientClick="resetPanel($(this));" />
                                        <asp:Button ID="btnChooseStores" runat="server" OnClientClick='<%# Eval("RackID", "return displayStoreList({0});return false;") %>' CssClass="btn btn-primary next-btn" Text="Next" />
                                        <span class="<%#"R"+Eval("RackID") %>">
                                            <asp:Button ID="btnUpdatePrice" runat="server" CssClass='btn btn-warning update-price-btn' Text="Update Price" OnClick="btnUpdatePrice_Click" /></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalStores" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #dd4b39; color: white; font-weight: 700!important">
                    <h3 class="modal-title">Order
                        <span class="pull-right">
                            <button type="button" class="modal-close btn btn-link" data-dismiss="modal" aria-label="Close"><i class="fa fa-times" style="font-size: 20px; color: white"></i></button>
                        </span>
                    </h3>
                </div>
                <div id="divStoreBody" style="padding-top: 0" class="modal-body">
                    <input type="hidden" id="hdnRackID" runat="server" class="hdnRackID" />
                    <div class="col-xs-6">
                        <label for="ddlOrderReceivedVia" class="h4">Order Received Via:</label>
                        <asp:DropDownList ID="ddlOrderReceivedVia" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-xs-6">
                        <label for="ddlOrderApproved" class="h4">Order Approved:</label>
                        <asp:DropDownList ID="ddlOrderApproved" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-xs-6">
                        <label for="ddOrderStatus" class="h4">Status</label>
                        <asp:DropDownList ID="ddOrderStatus" ClientIDMode="Static" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Quote" Value="-1" Selected="false"></asp:ListItem>
                            <asp:ListItem Text="Draft" Value="0" Selected="false"></asp:ListItem>
                            <asp:ListItem Text="Order" Value="1" Selected="true"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-xs-6">
                        <label for="txtPO" class="h4">PO #</label>
                        <asp:TextBox runat="server" ID="txtPO" ClientIDMode="static" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div style="margin-top: 10px" class="col-xs-6 dateSelector">
                        <label style="margin-top: 10px" class="h4">Choose Ship Date(s):</label>
                        <div class="checkbox checkboxlist" style="">
                            <asp:Repeater OnItemDataBound="rptShipDates_ItemDataBound" runat="server" ID="rptShipDates">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="lstShipDateSelector" runat="server" CssClass="form-control no-border order-date-selector" RepeatDirection="Vertical" RepeatLayout="Flow" Height="100%"></asp:CheckBoxList>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div style="margin-top: 10px" class="col-xs-6">
                        <div class="row">
                            <div class="col-xs-6" style="padding-right: 0px">
                                <label for="txtPO" class="h5">Global Discount 1:</label>
                                <asp:TextBox runat="server" ID="txtDiscount1Desc" ClientIDMode="static" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-6">
                                <label class="h5">&nbsp;</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtDiscount1Pct" ClientIDMode="static" CssClass="form-control text-right" TextMode="Number" Text="0.00"></asp:TextBox>
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6" style="padding-right: 0px">
                                <label for="txtPO" class="h5">Global Discount 2:</label>
                                <asp:TextBox runat="server" ID="txtDiscount2Desc" ClientIDMode="static" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-6">
                                <label class="h5">&nbsp;</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtDiscount2Pct" ClientIDMode="static" CssClass="form-control text-right" TextMode="Number" Text="0.00"></asp:TextBox>
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6" style="padding-right: 0px">
                                <label for="txtPO" class="h5">Global Discount 3:</label>
                                <asp:TextBox runat="server" ID="txtDiscount3Desc" ClientIDMode="static" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-6">
                                <label class="h5">&nbsp;</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtDiscount3Pct" ClientIDMode="static" CssClass="form-control text-right" TextMode="Number" Text="0.00"></asp:TextBox>
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6" style="padding-right: 0px">
                                <label for="txtPO" class="h5">Global Discount 4:</label>
                                <asp:TextBox runat="server" ID="txtDiscount4Desc" ClientIDMode="static" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-6">
                                <label class="h5">&nbsp;</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtDiscount4Pct" ClientIDMode="static" CssClass="form-control text-right" TextMode="Number" Text="0.00"></asp:TextBox>
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6" style="padding-right: 0px">
                                <label for="txtPO" class="h5">Global Discount 5:</label>
                                <asp:TextBox runat="server" ID="txtDiscount5Desc" ClientIDMode="static" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-6">
                                <label class="h5">&nbsp;</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtDiscount5Pct" ClientIDMode="static" CssClass="form-control text-right" TextMode="Number" Text="0.00"></asp:TextBox>
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 15px" class="col-xs-12">
                        <div class="pl-0 col-xs-6 col-md-9">
                            <label class="h4">Stores </label>
                        </div>
                        <div id="SelectAllDiv" runat="server" class="pr-0 col-xs-6 col-md-3">
                            <div class="input-group mb-3">
                                <input class="form-control" min="0" id="QuantityForAll" type="number" value="1" />
                                <span style="padding: 0px 12px!important; background-color: #dd4b39!important" class="input-group-addon">
                                    <button id="btnApplyAll" style="padding: 0px!important; color: white!important" class="btn btn-link">Set All</button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlSubCustomers" runat="server" CssClass="pl-0 pr-0 col-xs-12  bottom-buffer-small">
                        <div class="col-xs-12">
                            <button id="btnSelectAll" style="display: none!important" class="btn btn-default">Select All</button>
                            <button id="btnSelectNone" style="display: none!important" class="btn btn-default">None</button>
                            <%--<asp:Button ID="btnSelectAll" runat="server" class="btn btn-default" Text="Select All"></asp:Button>
                            <asp:Button ID="btnSelectNone" runat="server" class="btn btn-default" Text="None"></asp:Button>--%>
                        </div>

                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-xs-12">
                                    <table class="table table-striped" style="width: 100%">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th style="width: 75%!important">Store</th>
                                                <th style="width: 20%!important">Quantity</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptStores" runat="server" OnItemDataBound="rptStores_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hdnCompanyId" Value='<%# Eval("CustomerId") %>' runat="server" />
                                                            <asp:CheckBox runat="server" Style="display: none!important" ID="chkStoreSelected" Checked="false" CssClass="chkSelected" />
                                                        </td>
                                                        <td style="width: 75%!important">
                                                            <label><%# Eval("Company") %></label>
                                                        </td>
                                                        <td style="width: 20%!important">
                                                            <asp:TextBox ID="txtRackQuantity" type="Number" CssClass="form-control text-right popup-quantity-text" runat="server" MaxLength="10" min="0" onkeydown="return numberKeysOnly(event);" onblur="validateNumberInput(this);">1</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <label class="text-danger" id="errorMsg">
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div>&nbsp;</div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnPlaceOrder" CssClass="btn btn-primary AddToCartButton" runat="server" Text="Save Order" OnClick="btnPlaceOrder_Click" OnClientClick="return validatePlaceOrder();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" src="../js/jquery.swipebox.js"></script>
    <script>
        $(document).ready(function (e) {
            $(".reset-btn").hide();
            $(".update-price-btn").hide();

            $(".AddToCartButton").prop("disabled", true);
        });
        $("#btnApplyAll").click(function (e) {
            e.preventDefault();
            var qty = $("#QuantityForAll").val()
            $(".popup-quantity-text").val(qty);
        });
        $(".reck-price").text("$ " + $("#cphBody_currentReckPrice").val())
        function CheckAddToCart() {
            //var $PONumber = $("#PONumber").val();
            //var $OrderBy = $("#OrderByField").val();
            var OrderReceivedVia = $("#ddlOrderReceivedVia");
            var OrderApproved = $("#ddlOrderApproved");
            if ($(".order-date-selector input:checked").length > 0 && OrderReceivedVia.val() != '0' && OrderApproved.val() != '-1') {
                $(".AddToCartButton").prop("disabled", false);
            }
            else {
                $(".AddToCartButton").prop("disabled", true);
            }
        }
        $(document).on('click', ".order-date-selector", function (e) {
            CheckAddToCart()
        });
        $(".txtProductQuantity").on("change", function (e) {
            console.log("Changed!")
            $(this).parent().parent().parent().parent().parent().parent().parent().siblings("div.text-center").find(".next-btn").prop("disabled", "disabled");
        })
        $("#ddlOrderReceivedVia").on('change', function () {
            CheckAddToCart()
        });
        $("#ddlOrderApproved").on('change', function () {
            CheckAddToCart()
        });
        $(document).on("click", ".clear-all-btn", function (e) {
            e.preventDefault();
            var txtQtyBtns = $(this).parent().siblings(".rack-products-div").find(".txtProductQuantity");
            txtQtyBtns.each(function (index, txtBox) {
                $(txtBox).val(0);
            });
        })
        function resetPanel(e) {
            $.each($(".txtProductQuantity"), function (index, value) {
                $(this).val($(this).data('default'));
            });
            $("#cphBody_currentReckPrice").val(0);
            $(".ScriptDIV").empty();
        };
        $(document).on('click', '.customize-btn', function () {
            displayCustomize($(this));
            $(".next-btn").show();
            $(this).siblings("input.next-btn").hide();
            $(this).siblings("a.detail-btn").hide();
            $(".clear-all-div").hide();
            $(this).parent().siblings(".clear-all-div").show();
        })
        $(document).on("click", ".clear-all-btn", function (e) {
            e.preventDefault();

            var txtQtyBtns = $(this).parent().siblings(".rack-products-div").find(".txtProductQuantity");

            txtQtyBtns.each(function (index, txtBox) {
                $(txtBox).val(0);
            });
        })
        function displayCustomize(e) {
            //$(".txtProductQuantity").val(0);
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
        function ValidateOrder(rackID) {
            var hasSubCustomers = false;
            var customersSelected = 0;
            var customersDesc = '';
            var datesSelected = 0;
            var datesDesc = '';

            var txtQty = $('[tag="qty' + rackID + '"]');
            if (txtQty != null) {
                if (txtQty.val() == '' || txtQty.val() == '0') {
                    alert('Please enter the quantity of racks you wish to order.');
                    return false;
                }
                else {
                    var customers = $('[tag="cs' + rackID + '"]');

                    if (customers != null) {
                        if ($('#' + customers.attr('id') + ' input:checkbox').length > 0) {
                            hasSubCustomers = true;
                            $('#' + customers.attr('id') + ' input:checked').each(function () {
                                customersDesc += $(this).next().html() + '\n';
                                customersSelected++;
                            });
                        }
                        else {
                            customersSelected = 1;
                        }

                        if (customersSelected > 0) {
                            var shipDates = $('[tag="sd' + rackID + '"]');
                            if (shipDates != null) {
                                $('#' + shipDates.attr('id') + ' input:checked').each(function () {
                                    datesDesc += $(this).next().html() + '\n';
                                    datesSelected++;
                                });

                                if (datesSelected > 0) {
                                    var hRackName = $('[tag="h' + rackID + '"]');
                                    var rackName = hRackName.val();

                                    if (hRackName != null) {
                                        var msg = 'You have selected ' + txtQty.val() + ' of ' + rackName + ' to be shipped ';

                                        if (hasSubCustomers) {
                                            if (customersSelected > 1) {
                                                msg += 'to the ' + customersSelected + ' stores you selected ';
                                            }
                                            else {
                                                msg += 'to the store you selected ';
                                            }
                                        }

                                        msg += 'during the following weeks:\n\n' + datesDesc + '\nAre you sure you wish to proceed?';

                                        return confirm(msg);
                                    }
                                    else {
                                        return false;
                                    }
                                }
                                else {
                                    alert('Please select at least one shipping week.');
                                    return false;
                                }
                            }
                            else {
                                return false;
                            }
                        }
                        else {
                            alert('Please select at least one store.');
                            return false;
                        }
                    }
                    else {
                        return false;
                    }
                }
            }
            else {
                return false;
            }

            return false;
        }
        $(document).on("input", ".txtProductQuantity", function () {
            console.log($(this).val());
            var _value = $(this).val();
            if (_value === "") {
                $(this).val("0")
            }
        })
        $("#modalStores").on("hidden.bs.modal", function (e) {
            var ShipDateCheckBoxes = $(".order-date-selector").find("input[type=checkbox]")
            $("#ddlOrderReceivedVia").val(0);
            $("#ddlOrderApproved").val(-1);
            $.each(ShipDateCheckBoxes, function (i, v) {
                var chk = $(v)
                chk.prop("checked", false);
            })
            var qtyTextBoxes = $(".popup-quantity-text")

            $.each(qtyTextBoxes, function (i, v) {
                var qty = $(v)
                qty.val("1");
            })
            $(".AddToCartButton").prop("disabled", true);
        });
        function displayStoreList(rackID) {
            $("#modalStores").modal("show");
            $(".hdnRackID").val(rackID);
            $(".dateSelector span").hide();
            $('[tag="sd' + rackID + '"]').show().find("input:first").prop("checked");
            return false;
        }
        function SelectCustomers(select) {
            var customers = $(".chkSelected");
            if (customers != null) {
                $('.chkSelected input').each(function () {
                    $(this).prop('checked', select);
                });
            }

            return false;
        }
        $("#btnSelectAll").click(function (e) {
            e.preventDefault()
            SelectCustomers(true);
            $("#modalStores").modal('show');
        })
        $("#btnSelectNone").click(function (e) {
            e.preventDefault()
            SelectCustomers(false);
            $("#modalStores").modal('show');
        })
        $(document).on('click', ".order-date-selector input:checkbox", function (e) {
            console.log($(".order-date-selector input:checked").length);
            if ($(".order-date-selector input:checked").length > 0 && $(".chkSelected input:checked").length > 0) {
                $(".AddToCartButton").prop("disabled", false);
            }
            else {
                $(".AddToCartButton").prop("disabled", true);
            }
        });
        $(document).on('click', '.chkSelected', function () {
            console.log($(".chkSelected input:checked").length);
            if ($(".order-date-selector input:checked").length > 0 && $(".chkSelected input:checked").length > 0) {
                $(".AddToCartButton").prop("disabled", false);
            }
            else {
                $(".AddToCartButton").prop("disabled", true);
            }
        })
        $(document).on("click", ".gallery", function (e) {
            e.preventDefault();
            var productID = this.id;
            var data = new FormData();
            data.append('productID', productID);
            $.ajax({
                url: 'request/GetProductPhotos.ashx',
                data: data,
                type: 'POST',
                processData: false,
                contentType: false,
                success: function (data) {
                    console.log(data);
                    //var photosTest = [];
                    //photosTest.push({ href: '~/Photos/MiniTropicalQuarterRack.jpg', title: '1' });
                    //photosTest.push({ href: '~/Photos/redbud_quarter_rack-clean.jpg', title: '2' });
                    //photosTest.push({ href: '~/Photos/TropicalFoliageHalfRack.jpg', title: '3' });
                    //$.swipebox(photosTest);
                    $.swipebox(data.photos);
                }
            })
        })
    </script>
    <div runat="server" id="ScriptDIV" class="ScriptDIV"></div>
    <div runat="server" id="HideScripts" class="HideScripts"></div>
</asp:Content>
