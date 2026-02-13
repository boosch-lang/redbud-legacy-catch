<%@ Page Title="Dashboard | Redbud" Language="C#" MasterPageFile="~/Authorized.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Maddux.Pitch._default" ValidateRequest="false" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="css/bootstrap-datetimepicker.min.css" type="text/css" />
    <style>
        input:focus {
            box-shadow: none !important;
            border-color: black !important;
            border-right: none;
            border-left: none;
        }

        hr {
            opacity: 100;
        }

        .rack-discount-overlay {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            border-radius: 10px 10px 0 0;
            background-color: #ED174F;
            color: #FFFFFF;
            height: 32px;
            font-size: 13.4px;
            text-align: center;
            line-height: 32px;
            font-weight: 900;
        }

        .rack-discount-container {
            position: relative;
        }

        .modal-body {
            background-color: #F4EBE4 !important;
        }

        .card-content-scroll-wrapper {
            overflow-x: auto;
        }

        @media (max-width: 1000px) {
            .card-content-scroll-shadow-wrapper {
                position: relative;
                padding: 0;
            }

                .card-content-scroll-shadow-wrapper:before {
                    content: "";
                    position: absolute;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    pointer-events: none;
                    box-shadow: inset -20px 0px 10px -20px black, inset 20px 0px 10px -20px black;
                    z-index: 3;
                }
        }

        .card-content-scroll-section {
            min-width: 850px;
        }

        .modal-footer {
            background-color: #F4EBE4 !important;
        }

        .modal-close {
            position: absolute;
            top: 25px;
            right: 30px;
        }

        .modal-lg {
            max-width: 95vw;
        }

        /* Hide number input arrows in Chrome, Safari, Edge, Opera */
        input::-webkit-outer-spin-button,
        input::-webkit-inner-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }

        .btn-add-to-cart {
            z-index: 1050;
            position: fixed;
            bottom: 50px;
            right: 30px;
            padding: 5px 20px;
            border-radius: 20px;
        }

        .txtCustomRackQuantity, txtCustomProductQuantity {
            border-left: none !important;
            border-right: none !important;
        }

        .bg-input {
            background-color: #F4EBE4 !important;
            border-color: black !important;
        }

        .minusButtons {
            border-radius: 10px 0 0 10px !important;
            background-color: #F4EBE4 !important;
            border-color: black !important;
        }

        .plusButtons {
            border-radius: 0 10px 10px 0 !important;
            background-color: #F4EBE4 !important;
            border-color: black !important;
        }

        .welcome-banner.navbar-default {
            font-size: 16px !important;
            background-color: #ed174f !important;
            border-bottom-width: 0 !important;
            margin-bottom: 0 !important;
            border-bottom-color: #ed174f !important;
            border-bottom-style: solid !important;
            color: #fff;
            padding: 10px;
            text-align: center;
        }

        .welcome-callout {
            background-image: url('../img/shop_hero.png');
            background-size: cover;
            background-position: center;
            min-height: 465px;
            color: white;
        }

        .welcome-mask {
            background-color: #094C86CC;
            opacity: 80%;
            min-height: 465px;
        }

        input[type="number"] {
            -moz-appearance: textfield;
        }

        .checkbox input[type="checkbox"],
        .checkbox-inline input[type="checkbox"],
        .radio input[type="radio"],
        .radio-inline input[type="radio"] {
            margin-left: 0;
        }

        .chkShipDate input {
            position: absolute;
            opacity: 0;
        }

            .chkShipDate input + label {
                position: relative;
                cursor: pointer;
                padding: 0;
            }

                .chkShipDate input + label:before {
                    content: '';
                    margin-right: 10px;
                    display: inline-block;
                    vertical-align: text-top;
                    width: 20px;
                    height: 20px;
                    background: #f8f8f8;
                    border: 1px solid rgba(0, 0, 0, 0.125);
                    border-radius: 0.25rem;
                }

            .chkShipDate input:hover + label:before {
                background: #337ab7;
            }

            .chkShipDate input:focus + label:before {
                box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.12);
            }

            .chkShipDate input:checked + label:before {
                background: #337ab7;
            }

            .chkShipDate input:disabled + label {
                color: #b8b8b8;
                cursor: auto;
            }

                .chkShipDate input:disabled + label:before {
                    box-shadow: none;
                    background: #ddd;
                }

            .chkShipDate input:checked + label:after {
                content: '✓';
                position: absolute;
                left: 4px;
                top: 3px;
                color: white;
                width: 2px;
                height: 2px;
                font-weight: bold;
            }


        .input-custom-number {
            min-width: 40px;
        }


        .updated {
            color: #ed174f;
            font-style: italic;
            font-weight: bold;
        }

        .unavailable,
        .hide-True {
            display: none;
        }

        .btn-blue, .btnNext, .btnPlaceOrder {
            color: #fff;
            background-color: #094C86;
            border-color: #094C86;
            border-radius: 10px;
        }

            .btn-blue:focus,
            .btnNext:focus,
            .btnPlaceOrder:focus
            .btn-blue:hover,
            .btnNext:hover,
            .btnPlaceOrder:hover
            .btn-blue:active,
            .btnNext:active,
            .btnPlaceOrder:active {
                color: #fff;
                background-color: #094C86;
            }

            .btn-blue:disabled,
            .btnNext:disabled,
            .btnPlaceOrder:disabled {
                color: #000;
                background-color: #D1E5F6;
                border-color: #D1E5F6;
            }

        #validator {
            border-radius: 1em;
        }

        .validator {
            font-size: 24px;
            margin-bottom: 5px;
        }

        .full-width-element {
            width: 99.5vw;
            position: relative;
            left: 50%;
            right: 50%;
            margin-left: -50vw;
            background-color: red; /* Optional: for visibility */
        }


        /* Rack Image Carousel*/
        .carousel-control-prev-icon,
        .carousel-control-next-icon {
            background-color: black; /* Background color for the icon */
            background-size: 100%, 100%;
            filter: invert(1); /* Invert color for a different effect */
        }

            .carousel-control-prev-icon:hover,
            .carousel-control-next-icon:hover {
                background-color: darkgray; /* Color on hover */
            }

        table.no-border,
        table.no-border th,
        table.no-border td {
            border: none;
        }

        .custom-checkbox input[type="checkbox"] {
            display: none;
        }

        .custom-checkbox label::before {
            content: '';
            display: inline-block;
            width: 35px;
            height: 35px;
            border: 1px solid #000;
            margin-right: 30px;
            vertical-align: middle;
            background-color: #fff;
            cursor: pointer;
        }

        .custom-checkbox input[type="checkbox"]:checked + label::before {
            background-color: #000;
            border-color: #000;
            content: '\2713'; /* Checkmark symbol */
            color: #fff;
            font-size: 20px;
            text-align: center;
            line-height: 30px;
        }

        .custom-checkbox label {
            font-size: 25px;
            font-family: 'Museo-Sans-300', Arial, sans-serif !important;
            font-weight: 300;
            cursor: pointer;
        }

            .custom-checkbox label span {
                vertical-align: middle;
            }



        @media (max-width: 1600px) {
            .full-width-element {
                width: 99.4vw;
            }
        }

        @media (max-width: 1337px) {
            .full-width-element {
                width: 99.3vw;
            }
        }

        @media (max-width: 1142px) {
            .full-width-element {
                width: 99.2vw;
            }
        }

        @media (max-width: 1000px) {
            .full-width-element {
                width: 99.1vw;
            }
        }

        @media (max-width: 889px) {
            .full-width-element {
                width: 99vw;
            }
        }

        @media (max-width: 800px) {
            .full-width-element {
                width: 98.9vw;
            }
        }

        @media (max-width: 727px) {
            .full-width-element {
                width: 98.8vw;
            }
        }

        @media (max-width: 667px) {
            .full-width-element {
                width: 98.7vw;
            }
        }

        @media (max-width: 613px) {
            .full-width-element {
                width: 98.6vw;
            }
        }

        @media (max-width: 572px) {
            .full-width-element {
                width: 98.5vw;
            }
        }

        @media (max-width: 534px) {
            .full-width-element {
                width: 98.4vw;
            }
        }

        @media (max-width: 500px) {
            .full-width-element {
                width: 98.3vw;
            }
        }

        @media (max-width: 473px) {
            .full-width-element {
                width: 98.2vw;
            }
        }

        @media (max-width: 444px) {
            .full-width-element {
                width: 98.1vw;
            }
        }


        .landing-page-input-height {
            height: 30px;
        }

        select {
            font-size: 20px !important;
        }

        option {
            font-size: 20px;
        }

        .text-multiline {
            white-space: pre-line;
        }

        .bulk-rack-row {
            background-color: #87a492;
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">

    <!-- Full-width element -->


    <asp:Literal runat="server" ID="Literal1"></asp:Literal>
    <asp:Literal runat="server" ID="Literal2"></asp:Literal>

    <div class="d-flex justify-content-center align-items-center">
        <div id="litError" runat="server"></div>

    </div>
    <div class="panelRacks">
        <div class="row pt-5 pb-2">
            <div class="col-12 d-flex justify-content-end">
                <div class="col-auto">
                    <div class="dropDown-black bg-dark" style="border-radius: 15px;">
                        <asp:DropDownList ID="ddlShipWeekFilter" runat="server" CssClass="form-control form-control-blk ps-4 pe-5" Style="border-radius: 15px;">
                            <asp:ListItem Value="all">Filter By Ship Week</asp:ListItem>
                            <asp:ListItem Value="all">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <asp:ListView ID="lvRacks"
                runat="server"
                Visible="true"
                OnItemDataBound="lvRacks_ItemDataBound"
                DataKeyNames="RackID"
                ClientIDMode="Predictable">
                <ItemTemplate>
                    <div class="col-xxl-3 col-xl-4 col-lg-6 col-md-6 col-sm-12 col-xs-12 px-3 mb-4 py-4 hide-<%# Eval("HasNoShipDates") %> catalog-<%# Eval("CatalogID") %> rackdisplay">
                        <div class="card h-100 p-3 border-dark">
                            <div class="product-image-container">
                                <div class="rack-discount-container w-100">
                                    <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasMultipleImages") %>'>
                                        <div id="rackImages<%# Eval("RackID") %>" class="carousel slide w-100" data-bs-interval="false">

                                            <!-- The slideshow/carousel -->
                                            <div class="carousel-inner w-100">
                                                <div class="carousel-item active">
                                                    <div class="row justify-content-center">
                                                        <div class="col-auto">
                                                            <img src="<%# Eval("RackImage") %>" class="rounded img-fluid" style="max-height: 300px;" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:Repeater ID="rptRackImage" runat="server" DataSource='<%# Eval("RackImages") %>'>
                                                    <ItemTemplate>
                                                        <div class="carousel-item">
                                                            <div class="row justify-content-center">
                                                                <div class="col-auto">
                                                                    <img src="<%# Eval("PhotoPath") %>" class="rounded img-fluid" style="max-height: 300px;" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </div>

                                            <!-- Left and right controls/icons -->
                                            <button class="carousel-control-prev" type="button" data-bs-target="#rackImages<%# Eval("RackID") %>" data-bs-slide="prev">
                                                <span class="carousel-control-prev-icon"></span>
                                            </button>
                                            <button class="carousel-control-next" type="button" data-bs-target="#rackImages<%# Eval("RackID") %>" data-bs-slide="next">
                                                <span class="carousel-control-next-icon"></span>
                                            </button>

                                        </div>
                                    </asp:PlaceHolder>

                                    <asp:PlaceHolder runat="server" Visible='<%# !(bool)Eval("HasMultipleImages") %>'>
                                        <div class="row justify-content-center">
                                            <div class="col-auto">
                                                <img src="<%# Eval("RackImage") %>" class="rounded img-fluid" style="max-height: 300px;" />
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>


                                    <asp:PlaceHolder runat="server" Visible='<%# (Double)Eval("Discount") > 0 %>'>
                                        <div class="rack-discount-overlay ms-400"><%# ((Double)Eval("Discount")) %>% Volume Discount</div>
                                    </asp:PlaceHolder>
                                </div>
                            </div>
                            <div class="card-body pb-0 px-0">
                                <div class="row justify-content-center py-3 ">
                                    <div class="col-auto px-0">
                                        <h4 class="ms-700 fs-18 text-center"><%# Eval("RackDescription") %></h4>
                                    </div>
                                </div>
                                <div class="row pt-2 pb-3" style="margin-left: 0; margin-right: 0;">
                                    <div class="col text-start px-0 ms-250 fs-15">
                                        <input type="hidden" class="hiddenPrice" value='<%# Eval("Price") %>' />
                                        <div id="priceDefault"><span class="price"><%# ((Double)Eval("Price")).ToString("C2") %></span></div>
                                        <div id="priceCustom" class="d-none">
                                            <span class="price"></span>
                                            <br />
                                            <span class="updated">*Updated Price</span>
                                        </div>
                                    </div>
                                    <div class="col-auto text-end px-0 ms-250 fs-15">
                                        <input type="hidden" class="hiddenUnits" value='<%# ((Double)Eval("UnitCount")).ToString() %>' />
                                        <div id="unitsDefault"><span class="units"><%# ((Double)Eval("UnitCount")).ToString() %></span>&nbsp;UNITS</div>
                                        <div id="unitsCustom" class="d-none"><span class="units"></span>&nbsp;UNITS</div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row justify-content-center py-3">
                                    <div class="col-auto px-0">
                                        <h4 class="ms-500 fs-15 text-uppercase">Order By Ship Week</h4>
                                    </div>
                                </div>
                                <asp:Repeater ID="repShipDates" runat="server" DataSource='<%# Eval("ShipDates") %>'>
                                    <ItemTemplate>
                                        <div class="row my-2 <%# Eval("styleClass") %> ship-date-item">
                                            <div class="col-6 col-xxl-5 pr-0">
                                                <div class="checkbox pb-3 mt-2">
                                                    <asp:CheckBox ID="chkBxShipDate" CssClass="chkShipDate" runat="server" Style="display: none" Text='<%# ((DateTime)Eval("ShipDate")).ToString("MMM dd yyyy") %>' />
                                                    <asp:Label CssClass="ms-250 fs-15" Text='<%# ((DateTime)Eval("ShipDate")).ToString("MMM dd yyyy") %>' runat="server" />
                                                    <asp:HiddenField ID="hfShipDate" runat="server" Value='<%# Eval("ShipDate") %>' ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hfRackID" runat="server" Value='<%# Eval("RackID") %>' />
                                                    <asp:HiddenField ID="hfCatalogID" runat="server" Value='<%# Eval("CatalogID") %>' />
                                                    <asp:HiddenField ID="hfProgramID" runat="server" Value='<%# Eval("ProgramID") %>' />
                                                </div>
                                            </div>
                                            <div class="col-6 col-xxl-7 pl-0">
                                                <div class="input-group landing-page-input-height">
                                                    <span class="p-0 border-0  ">
                                                        <button type="button" class="btn btn-form p-0 btn-number minusButtons border-end-0 bg-input landing-page-input-height fs-15" style="width: 40px;" data-type="minus" data-field="<%# Eval("RackID") %>">
                                                            -
                                                        </button>
                                                    </span>
                                                    <asp:TextBox ID="txtRackQuantity" runat="server" type="number" min="0" CssClass="form-control bg-input text-center border-start-0 border-end-0 landing-page-input-height"
                                                        data-name='<%# Eval("RackID") %>' Text='0' />
                                                    <span class="p-0 border-0 ">
                                                        <button type="button" class="btn btn-form p-0 btn-number plusButtons border-start-0  bg-input landing-page-input-height fs-15" style="width: 40px;" data-type="plus" data-field="<%# Eval("RackID") %>">
                                                            +
                                                        </button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--
                                <!-- Not currently required by the client -->
                                <div id="shipDatesControls" class="row justify-content-center">
                                    <div class="col-auto px-0 d-flex align-items-center showControl">
                                        <h4 class="ms-300 fs-20 mb-0">Show All Ship Weeks</h4>
                                        <i class="fa fa-plus-circle fa-2x showAllShipWeeks px-3"></i>
                                    </div>
                                    <div class="col-auto px-0 d-flex align-items-center d-none hideControl">
                                        <h4 class="ms-300 fs-20 mb-0">Hide Ship Weeks</h4>
                                        <i class="fa fa-minus-circle fa-2x hideShipWeeks px-3"></i>
                                    </div>
                                </div>
                                --%>

                                <div class="col-12">
                                    <div class="row justify-content-center">
                                        <div class="col-xs-12 col-sm-6 col-md-8 col-lg-8  text-center">
                                            <button id="btnReset" type="button" class="btn d-none ms-300 fs-15 my-2 w-100 btn-dark" onclick="resetModal(<%# Eval("RackID") %>);">Reset Order</button>
                                            <asp:Button CssClass="ms-300 fs-15 btn-dark" Text="Customize" ID="CustomizeBtn" OnClientClick="return false;" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row justify-content-center">
                                        <div class="col-xs-12 col-sm-6 col-md-8 col-lg-8  text-center">
                                            <a id="lnkDetailsImg_<%# Eval("RackID") %>" href="forms/rackdetail.aspx?rackid=<%# Eval("RackID") %>" title="<%#Eval("RackDescription") %>" data-bs-toggle="modal" data-bs-target="#modalView" data-remote="false" title_text=" <%# Eval("ProgramName") + " - " + Eval("RackDescription") %>" class="btn ms-300 fs-15 my-2 w-100 btn-dark">View Details</a>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlCustomize" runat="server">
                                    <input type="checkbox" class="chkCustomize btn btn-default d-none" value="Customize" />
                                    <div class="modal fade customizeModal" id="modalCustomize_<%#Eval("RackID") %>" tabindex="-1" role="dialog" aria-labelledby="modalCustomize" aria-hidden="true">
                                        <div class="modal-dialog modal-lg">
                                            <div class="modal-content">
                                                <div class="modal-header position-relative d-flex justify-content-between align-items-center">
                                                    <h4 class="modal-title mx-auto text-center fs-30 ms-400"><%# Eval("RackDescription") %></h4>
                                                    <button type="button" class="btn-close modal-close" onclick="closeModal(<%# Eval("RackID") %>)" aria-label="Close"></button>

                                                </div>
                                                <div class="modal-body p-3 p-sm-5">
                                                    <div class="row gx-2 gx-sm-4">
                                                        <div class="col-auto">
                                                            <div class="rack-discount-container">
                                                                <img src="<%# Eval("RackImage") %>" class="card-img-top rounded-25 img-fluid" style="max-height: 200px; max-width: 200px" data-toggle="modal" data-target=".modal-profile-lg" />

                                                                <asp:PlaceHolder runat="server" Visible='<%# (Double)Eval("Discount") > 0 %>'>
                                                                    <div class="rack-discount-overlay ms-400"><%# ((Double)Eval("Discount")) %>% Volume Discount</div>
                                                                </asp:PlaceHolder>
                                                            </div>

                                                        </div>
                                                        <div class="col">
                                                            <div class="row py-3">
                                                                <div class="col-12">
                                                                    <input type="hidden" class="minItems" value='<%#Eval("MinimumItems") %>' />
                                                                    <input type="hidden" class="itemType" value='<%#Eval("ItemType") %>' />
                                                                    <div class="validator"><span class='bg-danger rounded px-3 py-2 text-white ms-300 fs-20' id="validator" style="display: inline-block;">Your order requires <%# Eval("MinimumItems").ToString() %> more <%# Eval("ItemType")  %> </span></div>
                                                                </div>
                                                            </div>
                                                            <div class="row d-none d-sm-flex py-3">
                                                                <div class="col-12">
                                                                    <p class="text-multiline"><%# Eval("ProductCatalogDescription") %></p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-12 col-lg-auto">
                                                            <div class="row justify-content-center justify-content-lg-end">
                                                                <div class="col">
                                                                    <button id="btnModalClose" type="button" class="btn-blue px-5 mx-2 fs-20 ms-300 my-2 w-100 py-2" onclick="resetAndClose(<%# Eval("RackID") %>);">Reset to Best Mix</button>
                                                                </div>
                                                            </div>
                                                            <div class="row justify-content-center justify-content-lg-end">
                                                                <div class="col">
                                                                    <button id="btnModalUpdate" type="button" class="btn-blue px-5 mx-2 fs-20 ms-300 my-2 w-100 py-2" onclick="applyCustomization(<%# Eval("RackID") %>);">Update Your Custom Order</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row d-sm-none pt-3">
                                                        <div class="col-12">
                                                            <p class="text-multiline"><%# Eval("ProductCatalogDescription") %></p>
                                                        </div>
                                                    </div>
                                                    <asp:HiddenField runat="server" ClientIDMode="Static" ID="customizeUnits" Value="0" />
                                                    <asp:HiddenField runat="server" ClientIDMode="Static" ID="customizePrice" Value="0" />
                                                    <asp:HiddenField runat="server" ClientIDMode="Static" ID="isCustomized" Value="0" />

                                                    <div class="col">
                                                        <div class="card-content-scroll-shadow-wrapper mt-4">
                                                            <div class="card-content-scroll-wrapper">
                                                                <div class="card-content-scroll-section overflow-hidden">
                                                                    <div class="row mb-2">
                                                                        <div class="col-12">
                                                                            <div class="bg-dark text-white ps-3 py-3 rounded-25">
                                                                                <div class="row">
                                                                                    <div class="col ms-400 fs-22">
                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Standard %>'>Quantity
                                                                                        </asp:PlaceHolder>
                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Bulk %>'>Smart Stack
                                                                                        </asp:PlaceHolder>
                                                                                    </div>
                                                                                    <div class="col ms-400 fs-22">
                                                                                        Item Image
                                                                                    </div>
                                                                                    <div class="col ms-400 fs-22">
                                                                                        Description
                                                                                    </div>
                                                                                    <div class="col  ms-400 fs-22">
                                                                                        Item #
                                                                                    </div>
                                                                                    <div class="col ms-400 fs-22">
                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Standard %>'>Case
                                                                                        </asp:PlaceHolder>
                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Bulk %>'>Qty
                                                                                        </asp:PlaceHolder>
                                                                                    </div>
                                                                                    <asp:PlaceHolder Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Bulk %>' runat="server">
                                                                                        <div class="col ms-400 fs-22">
                                                                                            Base Price
                                                                                        </div>
                                                                                    </asp:PlaceHolder>
                                                                                    <div class="col ms-400 fs-22">
                                                                                        Discounted Price
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-xs-12">
                                                                            <asp:Repeater
                                                                                runat="server"
                                                                                ID="rptProductRacks">
                                                                                <ItemTemplate>
                                                                                    <div class="card mt-3 w-100 rounded-25">
                                                                                        <div class="card-body py-0">
                                                                                            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Bulk %>'>
                                                                                                <div class="row align-items-center bulk-rack-row py-3 mb-5">
                                                                                                    <div class="col-2">
                                                                                                        <div class="row">
                                                                                                            <div class="col-12">
                                                                                                                <div class="input-group rptProductRack">
                                                                                                                    <span class="p-0 border-0">
                                                                                                                        <button type="button" class="btn btn-form p-0 btn-number minusButtons border-end-0 bg-input" style="height: 40px; width: 40px;" data-type="minus" data-field="<%# string.Format("{0}_{1}", Eval("ParentRackId"), Eval("RackId")) %>">
                                                                                                                            -
                                                                                                                        </button>
                                                                                                                    </span>
                                                                                                                    <asp:HiddenField ID="hdnProductRackID" Value='<%#Eval("RackID") %>' runat="server" />
                                                                                                                    <input type="hidden" class="productRackId" value='<%#Eval("RackID") %>' />
                                                                                                                    <asp:TextBox ID="txtCustomRackQuantity"
                                                                                                                        runat="server"
                                                                                                                        type="text"
                                                                                                                        min="0"
                                                                                                                        CssClass="form-control txtCustomRackQuantity input-custom-number bg-input text-center"
                                                                                                                        data-name='<%# string.Format("{0}_{1}", Eval("ParentRackId"), Eval("RackId")) %>'
                                                                                                                        data-default='<%# Eval("DefaultQuantity") %>'
                                                                                                                        data-rack='<%# Eval("RackId") %>'
                                                                                                                        data-parentRack='<%# Eval("ParentRackId") %>'
                                                                                                                        Text='0'
                                                                                                                        Style="height: 40px;" />
                                                                                                                    <span class="p-0 border-0">
                                                                                                                        <button type="button" class="btn btn-form p-0 btn-custom-number plusButtons customize border-start-0" style="height: 40px; width: 40px;" data-type="plus" data-field="<%# string.Format("{0}_{1}", Eval("ParentRackId"), Eval("RackId")) %>">
                                                                                                                            +
                                                                                                                        </button>
                                                                                                                    </span>
                                                                                                                </div>

                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="col">
                                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") > 0 %>'>
                                                                                                            <a href="<%#Eval("RackPhoto")%>" data-lightbox="<%# Eval("RackID") %>-rack" data-title='<%# Eval("RackName") %>'>
                                                                                                                <img class="thumbnail img-fluid rounded" style="max-height: 100px" src="<%#Eval("RackPhoto")%>" alt="<%#Eval("RackPhoto") %>">
                                                                                                            </a>
                                                                                                            <asp:Repeater ID="repPhotos" runat="server" DataSource='<%#Eval("Photos")%>'>
                                                                                                                <ItemTemplate>
                                                                                                                    <a href="<%#Eval("PhotoPath")%>" data-lightbox="<%# Eval("RackID") %>-rack" data-title='<%# Eval("RackName") %>'></a>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:Repeater>
                                                                                                        </asp:PlaceHolder>
                                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") == 0 %>'>
                                                                                                            <a href="/img/program-not-available.jpg" data-lightbox="<%# Eval("RackName") %>" data-title='<%# Eval("RackName") %>'>
                                                                                                                <img class="thumbnail img-fluid rounded" style="max-height: 100px" src="/img/program-not-available.jpg" alt="rack-not-available">
                                                                                                            </a>
                                                                                                        </asp:PlaceHolder>
                                                                                                    </div>
                                                                                                    <div class="col ms-300 fs-20"><%# Eval("RackName") %></div>
                                                                                                    <div class="col ms-300 fs-20">&nbsp;</div>
                                                                                                    <div class="col ms-300 fs-20">&nbsp;</div>
                                                                                                    <div class="col ms-300 fs-20"><%# String.Format("{0:c}", Eval("RackBasePrice"))  %></div>
                                                                                                    <div class="col ms-300 fs-20"><%# String.Format("{0:c}", Eval("RackPrice"))  %></div>
                                                                                                </div>
                                                                                            </asp:PlaceHolder>

                                                                                            <div class="row align-items-center">
                                                                                                <%--Smart Stack Column--%>
                                                                                                <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Bulk %>'>
                                                                                                    <div class="col-2">
                                                                                                        <div class="row">
                                                                                                            <div class="col-12">
                                                                                                                &nbsp;
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </asp:PlaceHolder>

                                                                                                <%--Other columns--%>
                                                                                                <div class="col">
                                                                                                    <asp:Repeater ID="rptProducts" runat="server" DataSource='<%# Eval("Products") %>'>
                                                                                                        <ItemTemplate>
                                                                                                            <div class="row align-items-center">
                                                                                                                <%--Standard Rack Quantity Column--%>
                                                                                                                <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("ParentRackType") == (int)Redbud.BL.RackType.Standard %>'>

                                                                                                                    <div class="col-2">
                                                                                                                        <div class="input-group rptCard">
                                                                                                                            <span class="p-0 border-0">
                                                                                                                                <button type="button" class="btn btn-form p-0 btn-custom-number minusButtons customize border-end-0" style="height: 40px; width: 40px;" data-type="minus" data-field="<%# string.Format("{0}_{1}", Eval("RackID"), Eval("ProductID")) %>">
                                                                                                                                    -
                                                                                                                                </button>
                                                                                                                            </span>
                                                                                                                            <asp:HiddenField ID="hdnProductID" Value='<%#Eval("ProductID") %>' runat="server" />
                                                                                                                            <input type="hidden" class="productId" value='<%#Eval("ProductID") %>' />
                                                                                                                            <input type="hidden" class="unitPrice" value='<%#Eval("UnitPrice") %>' />

                                                                                                                            <asp:TextBox ID="txtCustomProductQuantity"
                                                                                                                                runat="server"
                                                                                                                                type="text"
                                                                                                                                min="0"
                                                                                                                                CssClass="form-control txtCustomProductQuantity input-custom-number  bg-input text-center"
                                                                                                                                data-name='<%# string.Format("{0}_{1}", Eval("RackID"), Eval("ProductID")) %>'
                                                                                                                                data-default='<%# Eval("DefaultQuantity") %>'
                                                                                                                                data-rack='<%# Eval("RackID") %>'
                                                                                                                                Text='0' />
                                                                                                                            <span class="p-0 border-0">

                                                                                                                                <button type="button" class="btn btn-form p-0 btn-custom-number plusButtons customize border-start-0" style="height: 40px; width: 40px;" data-type="plus" data-field="<%# string.Format("{0}_{1}", Eval("RackID"), Eval("ProductID")) %>">
                                                                                                                                    +
                                                                                                                                </button>
                                                                                                                            </span>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </asp:PlaceHolder>
                                                                                                                <%--Other Columns--%>
                                                                                                                <div class="col">
                                                                                                                    <div class="row align-items-center py-1">
                                                                                                                        <div class="col">
                                                                                                                            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") > 0 %>'>
                                                                                                                                <a href="<%#Eval("ItemPhoto")%>" data-lightbox="<%# Eval("ProductID") %>-product" data-title='<%# Eval("ProductName") %>'>
                                                                                                                                    <img class="thumbnail img-fluid rounded" style="max-height: 70px" src="<%#Eval("ItemPhoto")%>" alt="<%#Eval("ItemPhoto") %>">
                                                                                                                                </a>
                                                                                                                                <asp:Repeater ID="repPhotos" runat="server" DataSource='<%#Eval("Photos")%>'>
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <a href="<%#Eval("PhotoPath")%>" data-lightbox="<%# Eval("ProductID") %>-product" data-title='<%# Eval("ProductName") %>'></a>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:Repeater>
                                                                                                                            </asp:PlaceHolder>
                                                                                                                            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") == 0 %>'>
                                                                                                                                <a href="/img/program-not-available.jpg" data-lightbox="<%# Eval("ProductName") %>" data-title='<%# Eval("ProductName") %>'>
                                                                                                                                    <img class="thumbnail img-fluid rounded" style="max-height: 70px" src="/img/program-not-available.jpg" alt="rack-not-available">
                                                                                                                                </a>
                                                                                                                            </asp:PlaceHolder>
                                                                                                                        </div>
                                                                                                                        <div class="col ms-300 fs-20"><%# Eval("ProductName") %></div>
                                                                                                                        <div class="col ms-300 fs-20"><%# Eval("ItemNumber") %></div>
                                                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("ParentRackType") == (int)Redbud.BL.RackType.Standard %>'>
                                                                                                                            <div class="col ms-300 fs-20"><%# (int)Eval("PackagesPerUnit") %></div>
                                                                                                                        </asp:PlaceHolder>
                                                                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("ParentRackType") == (int)Redbud.BL.RackType.Bulk %>'>
                                                                                                                            <div class="col ms-300 fs-20"><%# CalculateRackQty((int)Eval("PackagesPerUnit"), (double)Eval("DefaultQuantity")) %></div>
                                                                                                                        </asp:PlaceHolder>
                                                                                                                        <asp:PlaceHolder Visible='<%# (int)Eval("ParentRackType") == (int)Redbud.BL.RackType.Bulk %>' runat="server">
                                                                                                                            <div class="col ms-300 fs-20"><%# String.Format("{0:c}", Eval("EachBasePrice")) %></div>
                                                                                                                        </asp:PlaceHolder>
                                                                                                                        <div class="col ms-300 fs-20"><%# String.Format("{0:c}", Eval("EachPrice"))  %></div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:Repeater>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <div class="panelInfo" style="display: none;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-6 p-0">
                    <button type="button" id="backToList" class="align-top glyphicon glyphicon-chevron-left p-3 btn btn-link" style="display: none; text-decoration: none; color: gray; padding-top: 9px !important;" onclick="$('#btnNext').show();$('.btnPlaceOrder').hide();$('#selectionSubtotal').show();$('#backToList').hide();$('.panelInfo').hide();$('.panelRacks').show();$('.btnPlaceOrder').attr('disabled', $('.chkSelected input:checked').length == 0)"></button>
                </div>
            </div>
            <br />
            <input type="hidden" id="hdnRackID" runat="server" class="hdnRackID" />
            <asp:Label ID="litErrorModal" CssClass="text-danger" ClientIDMode="Static" runat="server" />

            <br />
            <asp:Panel ID="pnlSubCustomers" runat="server" CssClass="row bottom-buffer-small bg-white p-5 pt-3 border border-dark">
                <div class="col-12">
                    <div class="row">
                        <div class="col-12">
                            <label class="ms-400 fs-25 mb-5">Select Desired Stores</label>
                            <asp:Repeater ID="rptStores" runat="server">
                                <ItemTemplate>
                                    <div class="row my-3">
                                        <div class="col">
                                            <asp:HiddenField ID="hdnCompanyId" Value='<%# Eval("CustomerId") %>' runat="server" />
                                            <div class="chkSelected custom-checkbox">
                                                <asp:CheckBox runat="server" ID="chkStoreSelected" Checked='<%# ((Container.Parent as Repeater).DataSource as IList).Count==1 %>' Text='<%# Eval("Company") %>' />
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>

    <div class="overviewFooter d-flex justify-content-end">
        <button id="btnNext" type="button" class="btn btn-lg btnNext btn-add-to-cart ms-400 fs-20" disabled="true" onclick="nextClick();">
            Create and 
        <br />
            View Order
        </button>
        <asp:Button ID="btnPlaceOrder" CssClass="btn btn-lg btnPlaceOrder ms-400 fs-20 mt-3" Enabled="false" Style="display: none;" CausesValidation="true" runat="server" Text="Proceed to Cart" OnClick="btnPlaceOrder_Click" />
    </div>

    <!-- Member detail modal -->
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header position-relative d-flex justify-content-between align-items-center">
                    <h4 class="modal-title mx-auto text-center fs-30 ms-400" id="modalViewTitle">Loading...</h4>

                    <button type="button" class="btn-close modal-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div id="divModalViewBody" class="modal-body px-5 ">
                    <img src="img/loading.gif" alt="Loading..." id="imgLoadingModalContent" />
                </div>
                <div class="modal-footer  d-flex justify-content-center py-5" style="padding-bottom: 80px !important; border-top: none">
                    <button id="btnModalClose" type="button" class="btn btn-dark px-5 mx-2 fs-20 ms-300" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" src="js/jquery.swipebox.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/jquery-match-height@0.7.2/dist/jquery.matchHeight.min.js"></script>
    <script>

        $(document).ready(function () {
            // Function to initialize show/hide buttons for a ListView
            function initializeListView(listView) {
                const defaultCards = 2;
                const $listView = $(listView);
                const $shipDatesContainer = $listView.find('.ship-date-item');
                /* // Expansion of ship dates is not currently required by the client
                const $showControl = $listView.find(".showControl");
                const $hideControl = $listView.find(".hideControl");
                const $showAllButton = $listView.find('.showAllShipWeeks');
                const $hideButton = $listView.find('.hideShipWeeks');
                */

                // Check if there are more than 2 ship dates
                if ($shipDatesContainer.length > defaultCards) {
                    // Hide all ship dates except the first 2
                    //  $shipDatesContainer.slice(defaultCards).addClass('d-none');

                    /* // Expansion of ship dates is not currently required by the client

                    // Show the "Show All" control
                    $showControl.removeClass('d-none');

                    // Add click event for "Show All" button
                    $showAllButton.on('click', function () {
                        $shipDatesContainer.removeClass('d-none');
                        $showControl.addClass('d-none');
                        $hideControl.removeClass('d-none');
                    });

                    // Add click event for "Hide" button
                    $hideButton.on('click', function () {
                        $shipDatesContainer.slice(defaultCards).addClass('d-none');
                        $showControl.removeClass('d-none');
                        $hideControl.addClass('d-none');
                    });
                    */
                } else {
                    // If there are 2 or fewer ship dates, hide both buttons
                    //$listView.find('#shipDatesControls').addClass('d-none'); // Expansion of ship dates is not currently required by the client
                }
            }

            // Initialize each ListView on the page
            $('.rackdisplay').each(function () {
                initializeListView(this);
            });

            $('#<%= ddlShipWeekFilter.ClientID %>').change(function () {
                // Get the selected value
                var selectedValue = $(this).val();
                if (selectedValue == 'all') {
                    $('.rackdisplay').each(function () {
                        $(this).removeClass("d-none");
                    });
                } else {
                    var selectedDate = Date.parse(selectedValue);

                    $('.rackdisplay').each(function () {
                        var shouldShow = false;

                        // Check each hfShipDate input within the current .rackdisplay
                        $(this).find('input[id$="hfShipDate"]').each(function () {
                            // Get the ship date value
                            var shipDate = Date.parse($(this).val());

                            // Set the flag to true if the ship date matches
                            if (shipDate === selectedDate) {
                                shouldShow = true;
                                return false; // Exit the .each loop
                            }
                        });

                        // Hide the .rackdisplay element if no matching ship date was found
                        if (!shouldShow) {
                            $(this).addClass('d-none');
                        } else {
                            $(this).removeClass('d-none');
                        }
                    });


                }
            });

        });
    </script>
    <script>
        $(document).ready(function () {
            if ($('.chkShipDate input:checked').length > 0) {
                $('.btnNext').prop('disabled', false);
            }
            else {
                $('.btnNext').prop('disabled', true);
            }
        });

        $('.chkShipDate input').change(function () {
            var txtProductQuantity = $(this).closest('.row').find('.txtProductQuantity');
            if (txtProductQuantity.val() == "0")
                txtProductQuantity.val(1).change();

            if ($('.chkShipDate input:checked').length > 0) {
                $('.btnNext').prop('disabled', false);
            }
            else {
                $('.btnNext').prop('disabled', true);
            }
        });

        $('.chkSelected input').change(function () {
            $('.btnPlaceOrder').attr('disabled', $('.chkSelected input:checked').length == 0);
        });

        $('.btn-number').click(function (e) {
            e.preventDefault();
            fieldName = $(this).attr('data-field');
            type = $(this).attr('data-type');
            var input = $(this).closest('.row').find("input[data-name='" + fieldName + "']");
            var currentVal = parseInt(input.val());
            if (!isNaN(currentVal)) {
                if (type == 'minus') {
                    if (currentVal > input.attr('min')) {
                        input.val(currentVal - 1).change();
                    }
                } else if (type == 'plus') {
                    input.val(currentVal + 1).change();
                }
            } else {
                input.val(1);
            }

            $(this).closest('.row').find('input[type=checkbox]').prop('checked', (input.val() > 0));
            if ($('.chkShipDate input:checked').length > 0) {
                $('.btnNext').prop('disabled', false);
            }
            else {
                $('.btnNext').prop('disabled', true);
            }
        });

        // Fill modal with content from link href
        $("#modalView").on("show.bs.modal", function (e) {

            $(this).find(".modal-title").text("Loading...");
            $(this).find(".modal-body").html("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

            var eventSource = $(e.relatedTarget);
            if (eventSource.text().length > 0) {
                $(this).find(".modal-title").text(eventSource.attr("title_text"));
                $(this).find(".modal-body").load(eventSource.attr("href"));
            }
        });

        $('#modalView').on('hidden.bs.modal', function (e) {
            $(this).find(".modal-title").text("Loading...");
            $(this).find(".modal-body").empty();
            $(this).find(".modal-body").append("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

        });

        function nextClick() {

            var numOfStores = '<%=((int)ViewState["NumberOfStores"]) %>';

            if (numOfStores > 1) {
                $('#btnNext').hide();
                $('.btnPlaceOrder').show();
                $('#selectionSubtotal').hide();
                $('#backToList').show();
                $('.panelInfo').show();
                $('.panelRacks').hide();
            } else {
                $('.btnPlaceOrder').click();
            }
        }


        //Customization 
        $('.customizeModal').on('hidden.bs.modal', function (e) {
            $(".customize-btn").show();
        });
        $(document).on("input change", " .txtCustomProductQuantity", function () {
            var _value = $(this).val();
            if (_value === "") {
                $(this).val("0")
            }

            rackId = $(this).attr('data-rack');

            var requestData = {
                RackID: parseInt(rackId),
                Products: []
            }

            $.each($('#modalCustomize_' + rackId).find('.rptCard'), function (index, value) {
                var product = {
                    Id: parseInt($(value).find('.productId').val()),
                    Quantity: parseInt($(value).find('.input-custom-number').val())
                }
                requestData.Products.push(product);
            });

            $.ajax({
                async: true,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                type: "POST",
                data: JSON.stringify(requestData),
                url: "/CalculateRackPrice.ashx",

            }).done(function (data, textStatus, jqXHR) {
                onCustomize(data, rackId);

            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            });
        });

        $(document).on("input change", " .txtCustomRackQuantity", function () {
            var _value = $(this).val();
            if (_value === "") {
                $(this).val("0")
            }

            parentRackId = $(this).attr('data-parentRack');
            rackId = $(this).attr('data-rack');

            var requestData = {
                RackID: parseInt(parentRackId),
                Products: []
            }

            $.each($('#modalCustomize_' + parentRackId).find('.rptProductRack'), function (index, value) {
                var rack = {
                    Id: parseInt($(value).find('.productRackId').val()),
                    Quantity: parseInt($(value).find('.input-custom-number').val())
                }
                requestData.Products.push(rack);
            });

            $.ajax({
                async: true,
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                type: "POST",
                data: JSON.stringify(requestData),
                url: "/CalculateRackPrice.ashx",

            }).done(function (data, textStatus, jqXHR) {
                onCustomize(data, parentRackId);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            });

        });

        function onCustomize(data, rackId) {
            var data = JSON.parse(data);
            var percentageFull = data.percentage;
            var totalTrays = data.count;
            var minItemLimit = data.min;
            var maxItemLimit = data.max;
            var rackType = data.rackType;
            var itemType = rackType == 'standard' ? 'tray' : 'Smart Stack';
            var modal = $('#modalCustomize_' + rackId);

            modal.find('#customizePrice').val(data.price.toFixed(2));
            modal.find('#customizeUnits').val(data.units);

            if (percentageFull >= 100) {
                modal.find(".plusButtons.customize").attr('disabled', true);
            } else {
                modal.find(".plusButtons.customize").attr('disabled', false);
            }

            if (totalTrays < minItemLimit) {
                var required = (minItemLimit - totalTrays);
                if (required > 1) {
                    itemType = itemType + 's';
                }

                modal.find("#validator").html("Your order requires " + (minItemLimit - totalTrays) + " more " + itemType);
                modal.find("#validator").removeClass().addClass("bg-danger rounded px-3 py-2 text-white ms-300 fs-25");
                modal.find("#btnModalUpdate").attr('disabled', true);
                modal.find("#isCustomized").val('0');
            } else if (totalTrays >= minItemLimit && totalTrays < maxItemLimit) {
                var available = data.max - data.count;
                if (available > 1) {
                    itemType = itemType + 's';
                }
                modal.find("#validator").html("You have reached the minimum.  You may add " + available + " " + itemType);
                modal.find("#validator").removeClass().addClass("bg-success rounded px-3 py-2 text-white ms-300 fs-25");
                modal.find("#btnModalUpdate").attr('disabled', false);
                modal.find("#isCustomized").val('1');
            } else if (totalTrays == maxItemLimit) {
                modal.find("#validator").html("You have reached the full order");
                modal.find("#validator").removeClass().addClass("bg-success rounded px-3 py-2 text-white ms-300 fs-25");
                modal.find("#btnModalUpdate").attr('disabled', false);
                modal.find("#isCustomized").val('1');
            }

        }

        $('.btn-custom-number').click(function (e) {
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
                } else if (type == 'plus') {
                    input.val(currentVal + 1).change();
                }
            } else {
                input.val(0);
            }
        });
        $('.input-custom-number').focusin(function () {
            $(this).data('oldValue', $(this).val());
        });
        $('input-custom-number').change(function () {

            minValue = parseInt($(this).attr('min'));
            valueCurrent = parseInt($(this).val());

            name = $(this).attr('data-name');

            if (valueCurrent >= minValue) {
                $(".btn-custom-number[data-type='minus'][data-field='" + name + "']").removeAttr('disabled');
            } else {
                alert('Sorry, the minimum value was reached');
                $(this).val($(this).data('oldValue'));
            }
        });
        $(".input-custom-number").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        $(document).on('click', '.customize-btn', function () {
            displayCustomize($(this));
        });

        function resetAndClose(rackId) {

            resetModal(rackId);
            closeModal(rackId);
        }

        function closeModal(rackId) {
            var modal = $('#modalCustomize_' + rackId);
            var isCustomized = parseInt(modal.find("#isCustomized").val());

            if (isCustomized === 0) {
                resetModal(rackId);
            }

            modal.modal("hide");

        }

        function resetModal(rackId) {

            var modal = $('#modalCustomize_' + rackId);
            var custDiv = modal.parent().parent().parent().find('#priceCustom');
            var defaultDiv = modal.parent().parent().parent().find('#priceDefault');
            var custUnitsDiv = modal.parent().parent().parent().find('#unitsCustom');
            var defaultUnitsDiv = modal.parent().parent().parent().find('#unitsDefault');


            modal.parent().parent().find("#btnReset").addClass('d-none');
            var minItems = modal.find(".minItems").val();
            var itemType = modal.find(".itemType").val();

            modal.find("#customizePrice").val(0);
            modal.find("#customizeUnits").val(0);
            modal.find("#validator").html('Your order requires ' + minItems + ' more ' + itemType + '.');
            modal.find("#validator").removeClass().addClass("bg-danger rounded px-3 py-2 text-white ms-300 fs-25");
            modal.find("#btnModalUpdate").attr('disabled', true);
            modal.find("#isCustomized").val('0');
            modal.find(".plusButtons.customize").attr('disabled', false);

            defaultDiv.removeClass("d-none");
            custDiv.addClass("d-none");

            defaultUnitsDiv.removeClass("d-none");
            custUnitsDiv.addClass("d-none");


            var modalLnk1 = $("#lnkDetailsImg_" + rackId);
            var modalLnk2 = $("#lnkDetailsDescription_" + rackId);

            var lnk = "/forms/rackdetail.aspx?rackid=" + rackId;
            modalLnk1.attr("href", lnk);
            modalLnk2.attr("href", lnk);


            $.each($("#modalCustomize_" + rackId + "  .txtCustomProductQuantity"), function (index, value) {
                $(this).val(0);
            });

            $.each($("#modalCustomize_" + rackId + "  .txtCustomRackQuantity"), function (index, value) {
                $(this).val(0);
            });

        }

        function displayCustomize(e) {

            var checkbox = e.parent().parent().parent().parent().find(".chkCustomize");
            var checked = checkbox.prop("checked");

            console.log(checkbox)
            console.log("checked")
            if (checked == false) {
                checkbox.prop("checked", true);
                checked = true;
            }

            $(".chkCustomize").prop("checked", false);
            $(".customize-btn").show();
            $(e).hide();
            if (checked) {
                checkbox.prop("checked", true);
                checkbox.next(".customizeModal").modal("show");
            }
        }

        function applyCustomization(rackId) {
            var modal = $('#modalCustomize_' + rackId);

            var isCustomized = parseInt(modal.find("#isCustomized").val());
            var custPrice = parseFloat(modal.find("#customizePrice").val());
            var custUnits = parseInt(modal.find("#customizeUnits").val());

            var custDiv = modal.parent().parent().parent().find('#priceCustom');
            var defaultDiv = modal.parent().parent().parent().find('#priceDefault');
            var custUnitsDiv = modal.parent().parent().parent().find('#unitsCustom');
            var defaultUnitsDiv = modal.parent().parent().parent().find('#unitsDefault');

            var btnReset = modal.parent().parent().find('#btnReset');

            var defaultPrice = parseFloat(defaultDiv.find(".price").html());
            var defaultUnits = parseInt(defaultUnitsDiv.find(".units").html());

            if (isCustomized === 1) {
                var modalLnk1 = $("#lnkDetailsImg_" + rackId);
                var modalLnk2 = $("#lnkDetailsDescription_" + rackId);

                btnReset.removeClass('d-none');
                modal.parent().parent().parent().find('.hiddenPrice').val(custPrice);
                custDiv.find('.price').html("$" + custPrice);
                custDiv.removeClass("d-none");
                defaultDiv.addClass("d-none");

                modal.parent().parent().parent().find('.hiddenUnits').val(custUnits);
                custUnitsDiv.find('.units').html(custUnits);
                custUnitsDiv.removeClass("d-none");
                defaultUnitsDiv.addClass("d-none");

                var requestData = [];

                //standard racks
                $.each($('#modalCustomize_' + rackId + ' .rptCard'), function (index, value) {
                    var qty = parseInt($(value).find('.input-custom-number').val());

                    if (qty > 0) {
                        var selectedProduct = {
                            id: parseInt($(value).find('.productId').val()),
                            q: qty
                        };
                        requestData.push(selectedProduct);
                    }
                });
                var test = $('#modalCustomize_' + rackId + ' .rptProductRack');

                //bulk racks
                $.each($('#modalCustomize_' + rackId + ' .rptProductRack'), function (index, value) {

                    var qty = parseInt($(value).find('.input-custom-number').val());

                    if (qty > 0) {
                        var selectedProduct = {
                            id: parseInt($(value).find('.productRackId').val()),
                            q: qty
                        };
                        requestData.push(selectedProduct);
                    }
                });

                var lnk = "/forms/customdetail.aspx?rackid=" + rackId + "&json=" + encodeURIComponent(JSON.stringify(requestData));

                modalLnk1.attr("href", lnk);
                modalLnk2.attr("href", lnk);

            } else {
                modal.parent().parent().parent().find('.hiddenPrice').val(defaultPrice);
                modal.parent().parent().parent().find('.hiddenUnits').val(defaultUnits);
                defaultDiv.removeClass("d-none");
                custDiv.addClass("d-none");
                defaultUnitsDiv.removeClass("d-none");
                custUnitsDiv.addClass("d-none");

                btnReset.addClass('d-none');

                var lnk = "/forms/rackdetail.aspx?rackid=" + rackId;
                modalLnk1.attr("href", lnk);
                modalLnk2.attr("href", lnk);

            }
            closeModal(rackId);
        }

    </script>
</asp:Content>
