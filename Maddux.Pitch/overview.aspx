<%@ Page Title="Order | Redbud" Language="C#" MasterPageFile="~/Redbud.Master" AutoEventWireup="true" CodeBehind="overview.aspx.cs" Inherits="Maddux.Pitch.overview" %>


<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        .overviewHeader {
            background-color: #fff;
            width: 100%;
            z-index: 999;
            max-width: 1140px;
        }

        .footer {
            position: fixed;
            bottom: 0px;
            background-color: #f8f9fa;
            width: 100%;
            border-bottom: 1px solid #dee2e6 !important;
            z-index: 999;
            left: 0;
        }

        .overviewFooter {
            position: fixed;
            bottom: 58px;
            width: 100%;
            z-index: 999;
            left: 0;
        }

        @media(max-width:787px) {
            .overviewFooter {
                bottom: 81px;
            }
        }

        #btnSkip {
            color: white;
            background-color: #337ab7;
            border-color: #337ab7;
            border-radius: 0px;
            width: 100%;
        }

        #btnSkip, .btnPlaceOrder, .btnNext {
            color: white;
            background-color: #337ab7;
            border-color: #337ab7;
            border-radius: 0px;
            width: 100%;
        }

            #btnSkip:focus, .btnPlaceOrder:focus, .btnNext:focus {
                color: #fff;
                background-color: #1f71b8;
            }

            #btnSkip:hover, .btnPlaceOrder:hover, .btnNext:hover {
                color: #fff;
                background-color: #1f71b8;
            }

            #btnSkip:active, .btnPlaceOrder:active, .btnNext:active {
                color: #fff;
                background-color: #1f71b8;
            }

        #btnSkip {
            color: white;
            background-color: #5cb85c;
            border-color: #4cae4c;
            border-radius: 0px;
            width: auto;
            margin: 0 auto;
        }

        #validator {
            border-radius: 1em;
        }

        @media(max-width:767px) {
            .center-sm {
                float: none;
                margin: 0 auto;
            }
        }

        #btnPlaceOrder:disabled {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="mainContent">
        <asp:HiddenField ID="hfRackID" ClientIDMode="static" runat="server" />
        <div id="litError" style="padding-top: 10px" runat="server"></div>
        <div class="overviewHeader d-none">
            <div class="col-xs-8 py-2 px-0" style="display: flex; align-items: center; line-height: 1;">
                <a id="backToChoice" class="align-top glyphicon glyphicon-chevron-left p-3 d-none" style="text-decoration: none; color: gray;" href="#"></a>
                <a id="backToList" class="align-top glyphicon glyphicon-chevron-left p-3 d-none" style="text-decoration: none; color: gray;" href="#"></a>
                <div class="d-inline-block">
                    <span class="h4" id="catalogNameTitle" runat="server">Catalogue Name</span>
                    <div style="font-size: 18px;"><span class='label label-danger' id="validator" style="display: inline-block;">Your rack requires more items</span></div>
                    <input type="hidden" id="allowCustomString" value="" runat="server" />
                </div>
            </div>
            <div class="col-xs-4 pull-right mt-2 d-none">
                <div class="rackTotal">
                    <span class="h4 pull-right m-0">Rack Total:</span><br />

                    <span class="h4 pull-right m-0">$<span id="totalPrice">0</span></span>
                </div>
                <div class="orderTotal" style="display: none;">
                    <span class="h4 pull-right m-0">Order Total:</span><br />
                    <span class="h4 pull-right m-0">$<span id="orderTotalPrice">0</span></span>
                </div>
            </div>
        </div>
        <div>
            <input type="hidden" id="hfCustomize" />
            <div id="choiceDiv">
                <h1>
                    <asp:Literal ID="RackChoiceName" runat="server"></asp:Literal>
                </h1>
                <div class="row">
                    <div class="col-xs-4 center-sm">
                        <asp:Image ID="imgRackPhoto" runat="server" CssClass="img-responsive" Style="margin: 0 auto;" />
                    </div>
                    <div class="col-sm-8">
                        <div class="row">
                            <div class="col-xs-12 col-md-9">
                                <p>We have pre-selected our recommended plant choices for this rack. To order our recommended “best mix” for your store, simply click the Best Mix button.</p>
                            </div>
                            <div class="col-xs-12 col-md-3">
                                <p>
                                    <button class="noCustomize btn btn-primary btn-block">Order Best Mix</button>
                                </p>
                            </div>
                        </div>
                        <asp:Panel ID="pnlCustomize" runat="server">
                            <p>- or -</p>
                            <div class="row">
                                <div class="col-xs-12 col-md-9">
                                    <p>Click the Customize button to select your own mix of plants for this rack</p>
                                </div>
                                <div class="col-xs-12 col-md-3">
                                    <p>
                                        <button id="customize" class="btn btn-info btn-block">Customize Order</button>
                                    </p>
                                </div>
                            </div>
                            <h3>Our recommended best mix</h3>
                        </asp:Panel>
                        <asp:Repeater ID="repBestMix" runat="server">
                            <ItemTemplate>
                                <div class="card mt-5">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-xs-3">
                                                <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasProductPhoto") == true %>'>
                                                    <a href="<%#Eval("ProductPhotoPath")%>" data-lightbox="<%# Eval("ProductID") %>-preview" data-title='<%# Eval("ProductName") %>'>
                                                        <img class="thumbnail card-img-top" style="max-height: 120px" src="<%#Eval("ProductPhotoPath")%>" alt="<%#Eval("ProductPhotoPath") %>">
                                                    </a>
                                                    <asp:Repeater ID="repPhotos" runat="server" DataSource='<%#Eval("Photos")%>'>
                                                        <ItemTemplate>
                                                            <a href="<%#Eval("PhotoPath")%>" data-lightbox="<%# Eval("ProductID") %>-preview" data-title='<%# Eval("ProductName") %>'></a>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasProductPhoto") == false %>'>
                                                    <img class="thumbnail card-img-top" style="max-height: 120px" src="/img/program-not-available.jpg" alt="rack-not-available">
                                                </asp:PlaceHolder>
                                            </div>
                                            <div class="col-xs-9">
                                                <span class="h3"><%# Eval("ProductName") %></span>
                                                <h4><span class="product-price"><%# Eval("UnitPriceFormatted") %></span> / tray of <%# Eval("UnitPerCase") %></h4>
                                                Quantity: <%# Eval("DefaultQuantity") %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <br />
                        <p>
                            <button class="noCustomize btn btn-primary btn-block">Order Best Mix</button>
                        </p>
                    </div>
                </div>
            </div>
            <div id="productsDiv" class="d-none">
                <br />
                <br />
                <br />
                <p>Below you will find the plants available for this rack. You can customize the quantity of each plant using the - / + buttons. After you have set your desired amounts click 'Next' to continue.</p>
                <asp:Repeater ID="rptSelectedProducts" runat="server">
                    <HeaderTemplate>
                        <div class="row">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col-xs-12 col-sm-6">
                            <asp:HiddenField ID="hdnProductID" Value='<%#Eval("ProductID") %>' runat="server" />
                            <div class="card rptCards mt-5">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-xs-3">
                                            <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasProductPhoto") == true %>'>
                                                <a href="<%#Eval("ProductPhotoPath")%>" data-lightbox="<%# Eval("ProductID") %>-product" data-title='<%# Eval("ProductName") %>'>
                                                    <img class="thumbnail card-img-top" style="max-height: 120px" src="<%#Eval("ProductPhotoPath")%>" alt="<%#Eval("ProductPhotoPath") %>">
                                                </a>
                                                <asp:Repeater ID="repPhotos" runat="server" DataSource='<%#Eval("Photos")%>'>
                                                    <ItemTemplate>
                                                        <a href="<%#Eval("PhotoPath")%>" data-lightbox="<%# Eval("ProductID") %>-product" data-title='<%# Eval("ProductName") %>'></a>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasProductPhoto") == false %>'>
                                                <a href="/img/program-not-available.jpg" data-lightbox="<%# Eval("ProductName") %>" data-title='<%# Eval("ProductName") %>'>
                                                    <img class="thumbnail card-img-top" style="max-height: 120px" src="/img/program-not-available.jpg" alt="rack-not-available"></a>
                                            </asp:PlaceHolder>
                                        </div>
                                        <div class="col-xs-9">
                                            <span class="h3"><%# Eval("ProductName") %></span>
                                            <h4><span class="product-price"><%# Eval("UnitPriceFormatted") %></span> / tray of <%# Eval("UnitPerCase") %></h4>
                                            <div class="input-group">
                                                <span class="input-group-btn">
                                                    <button type="button" class="btn btn-default btn-number minusButtons customize" data-type="minus" data-field="<%# Eval("ProductID") %>">
                                                        <span class="glyphicon glyphicon-minus"></span>
                                                    </button>
                                                </span>
                                                <input type="hidden" class="productId" value='<%#Eval("ProductID") %>' />
                                                <input type="hidden" class="unitPrice" value='<%#Eval("UnitPrice") %>' />
                                                <asp:TextBox ID="txtProductQuantity" runat="server" type="text" min="0" CssClass="form-control form-control-plaintext txtProductQuantity input-number text-center" onkeypress="return false;" onfocus="return false;" data-name='<%# Eval("ProductID") %>' data-default='<%# Eval("DefaultQuantity") %>' Text='<%# Eval("DefaultQuantity") %>' />
                                                <%--<input runat="server" type="number" name="<%# Eval("ProductID") %>" id="txtProductQuantity" class="txtProductQuantity form-control input-number" value="<%# Eval("DefaultQuantity") %>" min="0" />--%>
                                                <span class="input-group-btn">
                                                    <button type="button" class="btn btn-default btn-number plusButtons customize" data-type="plus" data-field="<%# Eval("ProductID") %>">
                                                        <span class="glyphicon glyphicon-plus"></span>
                                                    </button>
                                                </span>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
                <br />
                <br />
                <br />
            </div>
            <div id="checkoutDiv" class="d-none">
                <br />
                <br />
                <br />
                <p>
                    Please complete the following details to add the order to your cart:
                </p>
                <ul>
                    <li>'Purchase Order #'
                    </li>
                    <li>'Order Placed by Name
                    </li>
                    <li>Rack Quantity
                    </li>
                </ul>
                <p>Once all required fields are entered, click 'Add to Cart' to view a summary of your order. </p>
                <div>
                    <br />
                    <label class="h3">Selected Rack</label>
                    <div class="media">
                        <div class="media-left">
                            <asp:Image ID="imgRackOverview" runat="server" Style="max-height: 120px;" CssClass="media-object" />
                        </div>
                        <div class="media-body">
                            <h4 class="media-body">
                                <asp:Literal ID="litCheckoutRack" runat="server"></asp:Literal>
                            </h4>
                            <asp:Literal ID="litRackProgram" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="form-group mb-0">
                        <label for="PONumber" class="h3">Purchase Order # (Optional)</label>
                        <asp:TextBox runat="server" CssClass="form-control PONumber" ClientIDMode="Static" ID="PONumber" />
                        <asp:RegularExpressionValidator ErrorMessage="Purchase Order # should be letters and numbers only and less than 20 characters" ValidationExpression="^[a-zA-Z0-9].{0,20}$" CssClass="text-danger" ControlToValidate="PONumber" runat="server" />
                    </div>
                    <div class="form-group mb-0">
                        <label for="OrderByField" class="h3">Order Placed By</label>
                        <asp:TextBox runat="server" CssClass="form-control OrderByField" ClientIDMode="Static" ID="OrderByField" />
                        <asp:RegularExpressionValidator ErrorMessage="Order Placed By should be letters only and less than 50 characters" ValidationExpression="^[a-zA-Z].{0,50}$" CssClass="text-danger" ControlToValidate="OrderByField" runat="server" />
                    </div>
                    <label for="rptStores" class="h3">
                        Select Quantity</label>
                    <asp:Repeater ID="repStores" runat="server" OnItemDataBound="repStores_ItemDataBound">
                        <ItemTemplate>
                            <div class="companyContainer">
                                <span style="font-size: 18px;"><%# Eval("Company") %></span>
                                <asp:HiddenField ID="hfCompanyID" Value='<%# Eval("CustomerId") %>' runat="server" />
                                <div class="row" style="margin-left: 0px; margin-right: 0px; border-bottom: 2px solid #ddd">
                                    <div class="col-xs-3" style="border-left: 1px solid #ddd; border-right: 1px solid #ddd; padding: 8px; font-weight: bold; background-color: #337ab7; color: #fff;">
                                        Ship Week
                                    </div>
                                    <div class="col-xs-3" style="border-left: 1px solid #ddd; border-right: 1px solid #ddd; padding: 8px; font-weight: bold; background-color: #337ab7; color: #fff;">
                                        Unit Price
                                    </div>
                                    <div class="col-xs-6" style="border-left: 1px solid #ddd; border-right: 1px solid #ddd; padding: 8px; font-weight: bold; background-color: #337ab7; color: #fff;">
                                        Quantity
                                    </div>
                                </div>
                                <asp:Repeater ID="repShipDates" runat="server">
                                    <ItemTemplate>
                                        <div class="row rptDates" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-3" style="line-height: 34px; border: 1px solid #ddd; padding: 8px;">
                                                <%# ((DateTime)Eval("ShipDate")).ToString("MMM dd") %>
                                                <asp:HiddenField ID="hdnShipDate" Value='<%# Eval("ShipDate") %>' runat="server" />
                                            </div>
                                            <div class="col-xs-3" style="line-height: 34px; border: 1px solid #ddd; padding: 8px;">
                                                $<span class="rackPriceColumn"></span>
                                            </div>
                                            <div class="col-xs-6" style="border: 1px solid #ddd; padding: 8px;">
                                                <div class="input-group">
                                                    <span class="input-group-btn">
                                                        <button type="button" class="btn btn-default btn-number minusButtons" data-type="minus" data-field="<%# Eval("ShipDate") %>" data-extra="true">
                                                            <span class="glyphicon glyphicon-minus"></span>
                                                        </button>
                                                    </span>
                                                    <asp:TextBox ID="txtRackQuantity" type="Text" ClientIDMode="Static" CssClass="form-control form-control-plaintext input-number txtRackQuantity" data-name='<%# Eval("ShipDate") %>' runat="server" MaxLength="10" min="0" Text="0" />
                                                    <span class="input-group-btn">
                                                        <button type="button" class="btn btn-default btn-number plusButtons" data-type="plus" data-field="<%# Eval("ShipDate") %>" data-extra="true">
                                                            <span class="glyphicon glyphicon-plus"></span>
                                                        </button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                    <div class="col-xs-3">
                                    </div>
                                    <div class="col-xs-9" style="border: 1px solid #ddd; padding: 8px; text-align: right;">
                                        <strong>Store Subtotal: $<span class="storeTotal">0</span></strong>
                                    </div>
                                </div>

                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <br />
                <br />
                <br />
            </div>
        </div>
    </div>
    <br />
    <br />
    <br />
    <div class="overviewFooter" style="text-align: center;">
        <button id="btnNext" class="btn btn-lg btnNext d-none" disabled="true">Next</button>
        <asp:Button ID="btnPlaceOrder" ClientIDMode="static" CssClass="btn btn-lg btnPlaceOrder d-none" CausesValidation="true" runat="server" Text="Add to Cart" OnClick="btnPlaceOrder_Click" />
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" src="js/jquery.swipebox.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/jquery-match-height@0.7.2/dist/jquery.matchHeight.min.js"></script>
    <script type="text/javascript">
        lightbox.option({
            'resizeDuration': 200,
        })
        $(document).ready(function () {
            var requestData = {
                RackID: parseInt($('#hfRackID').val()),
                Products: []
            }

            $.each($('.rptCards'), function (index, value) {
                var product = {
                    Id: parseInt($(value).find('.productId').val()),
                    Quantity: parseInt($(value).find('.input-number').val())
                }
                requestData.Products.push(product);
            });

            //console.log(requestData);
            $.ajax({
                async: true,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                type: "POST",
                data: JSON.stringify(requestData),
                url: "/CalculateRackPrice.ashx",

            }).done(function (data, textStatus, jqXHR) {
                var data = JSON.parse(data);
                $("#totalPrice").html(data.price.toFixed(2));
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            });

            var allowCustomization = $("#allowCustomString").val();
            if (allowCustomization == "notAllow") {
                $(".txtProductQuantity").attr('readonly', true);
                $(".minusButtons.customize").attr('disabled', true);
                $(".plusButtons.customize").attr('disabled', true);
                $(".btn-number[data-extra='true']").removeAttr('disabled');
            }

            var totalQuantity = 0;
            $.each($('.rptDates'), function (index, value) {
                var quantity = parseInt($(value).find('.input-number').val());
                totalQuantity += quantity;
            });
            if (totalQuantity <= 0) {
                $("#btnPlaceOrder").attr('disabled', true);
            } else {
                $("#btnPlaceOrder").attr('disabled', false);
            }
        });

        $(document).on("input change", ".txtProductQuantity", function () {
            var _value = $(this).val();
            if (_value === "") {
                $(this).val("0")
            }

            var requestData = {
                RackID: parseInt($('#hfRackID').val()),
                Products: []
            }

            $.each($('.rptCards'), function (index, value) {
                var product = {
                    Id: parseInt($(value).find('.productId').val()),
                    Quantity: parseInt($(value).find('.input-number').val())
                }
                requestData.Products.push(product);
            });

            //console.log(requestData);
            $.ajax({
                async: true,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                type: "POST",
                data: JSON.stringify(requestData),
                url: "/CalculateRackPrice.ashx",

            }).done(function (data, textStatus, jqXHR) {
                //console.log(data);
                //console.log(textStatus);
                //console.log(jqXHR);
                var data = JSON.parse(data);
                //console.log(data);
                var percentageFull = data.percentage;
                var totalTrays = data.count;
                var minItemLimit = data.min;
                var maxItemLimit = data.max;
                $("#totalPrice").html(data.price.toFixed(2));
                $("#validator").html("You have reached the minimum. You may add " + (data.max - data.count) + " trays");

                if (percentageFull >= 100) {
                    $(".plusButtons.customize").attr('disabled', true);
                    $(".btn-number[data-field='btnQuantity']").removeAttr('disabled');
                } else {
                    $(".plusButtons.customize").attr('disabled', false);
                }

                if (totalTrays < minItemLimit) {
                    $("#validator").html("Your rack requires more items");
                    $("#validator").removeClass().addClass("label label-danger");
                    $("#btnNext").attr('disabled', true);
                } else if (totalTrays >= minItemLimit && totalTrays <= maxItemLimit) {
                    $("#validator").removeClass().addClass("label label-success");
                    $("#btnNext").attr('disabled', false);
                } else if (totalTrays > maxItemLimit) {
                    $("#validator").removeClass().addClass("label label-danger");
                    $("#btnNext").attr('disabled', true);
                }

            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            });
        });

        $(document).on("input change", ".txtRackQuantity", function () {
            var _value = $(this).val();
            if (_value === "") {
                $(this).val("0")
            }

            var totalQuantity = 0;
            $.each($('.rptDates'), function (index, value) {
                var quantity = parseInt($(value).find('.input-number').val());
                totalQuantity += quantity;
            });
            if (totalQuantity <= 0) {
                $("#btnPlaceOrder").attr('disabled', true);
            } else {
                $("#btnPlaceOrder").attr('disabled', false);
            }

            $('#orderTotalPrice').html((totalQuantity * parseFloat($('#totalPrice').text())).toFixed(2));

            $(this).parent().parent().parent().find('.totalPriceColumn').html(_value * parseFloat($(this).parent().parent().parent().find('.rackPriceColumn').text()).toFixed(2));

            $.each($('.companyContainer'), function (index, company) {
                var totalQuantity = 0;
                $.each($(company).find('.rptDates'), function (index, value) {
                    var quantity = parseInt($(value).find('.input-number').val());
                    totalQuantity += quantity;
                });
                $(company).find('.storeTotal').html((totalQuantity * parseFloat($('#totalPrice').text())).toFixed(2));
            });
        });

        $('.btn-number').click(function (e) {
            e.preventDefault();
            fieldName = $(this).attr('data-field');
            type = $(this).attr('data-type');
            var input = $("input[data-name='" + fieldName + "']");
            var currentVal = parseInt(input.val());
            if (!isNaN(currentVal)) {
                if (type == 'minus') {
                    if (currentVal > input.attr('min')) {
                        input.val(currentVal - 1).change();
                    }
                    if (parseInt(input.val()) == input.attr('min')) {
                        $(this).attr('disabled', true);
                    }
                } else if (type == 'plus') {
                    input.val(currentVal + 1).change();
                }
            } else {
                input.val(0);
            }
        });
        $('.input-number').focusin(function () {
            $(this).data('oldValue', $(this).val());
        });
        $('.input-number').change(function () {

            minValue = parseInt($(this).attr('min'));
            valueCurrent = parseInt($(this).val());

            name = $(this).attr('data-name');
            if (valueCurrent >= minValue) {
                $(".btn-number[data-type='minus'][data-field='" + name + "']").removeAttr('disabled');
            } else {
                alert('Sorry, the minimum value was reached');
                $(this).val($(this).data('oldValue'));
            }
        });
        $(".input-number").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        $(function () {
            $('.card').matchHeight({
                byRow: false,
            });
        });

        $('.noCustomize').click(function (e) {
            e.preventDefault();
            $("#choiceDiv").addClass("d-none");
            $("#checkoutDiv").removeClass("d-none");
            $("#btnPlaceOrder").removeClass("d-none");
            $("#backToList").addClass("d-none");
            $("#backToChoice").removeClass("d-none");
            $('.overviewHeader').removeClass("d-none");
            $('#hfCustomize').val("false");
            $('.orderTotal').show();
            $('.rackTotal').hide();
            $('#validator').addClass("d-none");

            var totalQuantity = 0;
            $('.rackPriceColumn').html($('#totalPrice').text());
            $.each($('.rptDates'), function (index, value) {
                var quantity = parseInt($(value).find('.input-number').val());
                totalQuantity += quantity;
            });
            $('#orderTotalPrice').html((totalQuantity * parseFloat($('#totalPrice').text())).toFixed(2));


            $.each($('.companyContainer'), function (index, company) {
                var totalQuantity = 0;
                $.each($(company).find('.rptDates'), function (index, value) {
                    var quantity = parseInt($(value).find('.input-number').val());
                    totalQuantity += quantity;
                });
                $(company).find('.storeTotal').html((totalQuantity * parseFloat($('#totalPrice').text())).toFixed(2));
            });
        });

        $('#customize').click(function (e) {
            e.preventDefault();
            $('.input-number').val(0);
            $("#choiceDiv").addClass("d-none");
            $("#productsDiv").removeClass("d-none");
            $("#btnNext").removeClass("d-none");
            $("#backToChoice").removeClass("d-none");
            $('.overviewHeader').removeClass("d-none");
        });

        $('#btnNext').click(function (e) {
            e.preventDefault();
            $(this).addClass("d-none");
            $("#btnPlaceOrder").removeClass("d-none");
            $("#productsDiv").addClass("d-none");
            $("#checkoutDiv").removeClass("d-none");
            $("#backToChoice").addClass("d-none");
            $("#backToList").removeClass("d-none");
            $('#validator').addClass("d-none");

            var totalQuantity = 0;
            $('.rackPriceColumn').html($('#totalPrice').text());
            $.each($('.rptDates'), function (index, value) {
                var inputNumber = $(value).find('.input-number');
                var quantity = parseInt($(value).find('.input-number').val());
                totalQuantity += quantity;
                inputNumber.parent().parent().parent().find('.totalPriceColumn').html(quantity * parseFloat(inputNumber.parent().parent().parent().find('.rackPriceColumn').text()).toFixed(2));
            });
            $('#orderTotalPrice').html((totalQuantity * parseFloat($('#totalPrice').text())).toFixed(2));

            $.each($('.companyContainer'), function (index, company) {
                var totalQuantity = 0;
                $.each($(company).find('.rptDates'), function (index, value) {
                    var quantity = parseInt($(value).find('.input-number').val());
                    totalQuantity += quantity;
                });
                $(company).find('.storeTotal').html(totalQuantity * parseFloat($('#totalPrice').text()).toFixed(2));
            });

            $('.orderTotal').show();
            $('.rackTotal').hide();
        });

        $('#backToChoice').click(function (e) {
            e.preventDefault();
            location.reload(true);
        });

        $('#backToList').click(function (e) {
            e.preventDefault();
            $(this).addClass("d-none");
            $("#backToChoice").removeClass("d-none");
            $("#btnPlaceOrder").addClass("d-none");
            $("#btnNext").removeClass("d-none");
            $("#checkoutDiv").addClass("d-none");
            $("#productsDiv").removeClass("d-none");
            $('.orderTotal').hide();
            $('.rackTotal').show();
            $('#validator').removeClass("d-none");
        });
    </script>
</asp:Content>
