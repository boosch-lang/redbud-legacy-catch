<%@ Page Title="My Cart | Redbud" Language="C#" MasterPageFile="~/Authorized.Master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="Maddux.Pitch.cart" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        .form-control {
            background-color: #F0F0F0 !important;
        }

        .table-borderless td {
            border: none !important;
            padding: 1rem;
        }

        .btn-delete {
            padding: 0;
            text-align: start;
            color: rgba(0, 0, 0, 0.7);
        }

        .table td, .table th {
            vertical-align: middle; /* Center-align text vertically */
        }

        .table .btn-delete {
            margin: 0; /* Remove default margin if needed */
        }

        .d-flex {
            display: flex;
        }

        .align-items-center {
            align-items: center;
        }

        .modal-body {
            background-color: #F4EBE4 !important;
        }

        .modal-footer {
            background-color: #F4EBE4 !important;
            border: none;
        }

        .modal-close {
            position: absolute;
            top: 25px;
            right: 30px;
        }


        .modal-lg {
            width: 95vw;
        }

        .card-cart a, .card-cart a:hover, .card-cart a:visited {
            color: inherit !important;
        }

        .cart-items-table {
            --row-height: 50px;
            --margin-height: 5px;
        }
        .cart-items-table tr:not(.cart-grouped-item):not(.cart-grouped-item-first) {
            height: calc(var(--row-height) + var(--margin-height));
        }
        .cart-items-table tr.cart-grouped-item .cart-delete-button,
        .cart-items-table tr.cart-grouped-item-first .cart-delete-button {
            border-left: black 1px solid;
            border-right: black 1px solid;
            height: var(--row-height);
        }
        .cart-items-table tr.cart-grouped-item-first .cart-delete-button {
            border-top: black 1px solid;
            margin-top: var(--margin-height);
        }
        .cart-items-table tr.cart-grouped-item .cart-delete-button {
            border-bottom: black 1px solid;
            margin-bottom: var(--margin-height);
            padding-bottom: 0;
        }
        .cart-items-table tr.cart-grouped-item:has(+ tr.cart-grouped-item) .cart-delete-button {
            border-bottom: none;
            margin-bottom: 0;
            height: calc(var(--row-height) + var(--margin-height));
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <%-- <div class="d-flex justify-content-center align-items-center my-lg-5 my-md-3 my-sm-1">
        <div class="bg-white border border-dark py-lg-5 py-md-3 py-sm-2">--%>

    <div class="bg-white mt-5 card-cart" style="border-radius: 10px">
        <div class="row">
            <div class="bg-white px-5 py-5  border border-dark" style="border-radius: 10px">
                <div class="row">
                    <div class="col-12">
                        <asp:Panel runat="server" ID="pnlActivitySuccess" Visible="false">
                            <div class="alert alert-success d-flex justify-content-between align-items-center" role="alert">
                                <span id="spnSuccessMessage" runat="server">Success!</span>
                                <a href="#" class="close" data-bs-dismiss="alert" aria-label="close">
                                    <i class="far fa-times-circle" aria-hidden="true"></i>
                                </a>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div class="row mb-5 align-items-center">
                    <div class="col-lg-auto fs-25 ms-400">
                        Your Shopping Cart
                    </div>
                    <div class="col">
                        <asp:Literal Text="" ID="litMessage" runat="server" />
                    </div>
                </div>
                <div class="row mb-5">
                    <div class="col-12">
                        <div class="row align-items-center">
                            <div class="col-auto">
                                <label for="PONumber" class="col-form-label fs-25 ms-300">Purchase Order #</label>
                            </div>
                            <div class="col-12 col-lg-5 col-md-6 col-sm-10">
                                <asp:TextBox runat="server" CssClass="form-control" ClientIDMode="Static" ID="PONumber" MaxLength="20" />
                            </div>
                            <div class="col-12">
                                <asp:RegularExpressionValidator ErrorMessage="Purchase Order # can't be longer than 20 characters" ValidationExpression="^[a-zA-Z0-9].{0,20}$" CssClass="text-danger fs-12" ControlToValidate="PONumber" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <div id="divOtherCartOrders" runat="server">
                            <div class="row">
                                <div class="col-12">
                                    <asp:Repeater ID="rptOtherCart" runat="server" OnItemDataBound="rptOtherCart_ItemDataBound">
                                        <ItemTemplate>
                                            <div class="row mb-4">
                                                <div class="col-12 ms-400 fs-25">
                                                    <%# Eval("Company") %>
                                                </div>
                                            </div>
                                            <div class="row mb-5">
                                                <div class="col-12">
                                                    <div class="table-responsive">
                                                        <asp:GridView
                                                            ID="dgvSubCart"
                                                            runat="server"
                                                            CssClass="table table-hover table-borderless fs-18 ms-300 cart-items-table"
                                                            OnRowDataBound="dgvSubCart_RowDataBound"
                                                            GridLines="None"
                                                            AutoGenerateColumns="False"
                                                            ShowHeader="true"
                                                            ShowFooter="false"
                                                            OnRowCommand="dgvCart_RowCommand"
                                                            HeaderStyle-CssClass="border-bottom border-dark"
                                                            EmptyDataText="You have no items in your cart.">
                                                            <Columns>
                                                                <asp:BoundField DataField="OrderID" HeaderStyle-CssClass="d-none" ItemStyle-CssClass="d-none" />
                                                                <asp:TemplateField SortExpression="OrderID" HeaderStyle-CssClass="fs-18 ms-300" ItemStyle-CssClass="fs-18 ms-300 wrap-text">
                                                                    <HeaderTemplate>Draft Order #</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <a href="orderdetail.aspx?id=<%# Eval("OrderID") %>" title="Order Details" data-bs-toggle="modal" data-bs-target="#modalView" data-remote="false" title_text="<%# Eval("OrderID") %>">
                                                                            <%# Eval("OrderID") %>
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="RequestedShipDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Requested Ship Week" HtmlEncode="false" HeaderStyle-CssClass="fs-18 ms-300" ItemStyle-CssClass="fs-18 ms-300 wrap-text" />
                                                                <asp:BoundField DataField="ProgramHTML" HeaderText="Display" HtmlEncode="false" HeaderStyle-CssClass="fs-18 ms-300" ItemStyle-CssClass="fs-18 ms-300 wrap-text" />
                                                                <asp:BoundField DataField="SubTotal" DataFormatString="{0:C2}" HeaderText="Price" HtmlEncode="false" HeaderStyle-CssClass="fs-18 ms-300" ItemStyle-CssClass="fs-18 ms-300 wrap-text" />
                                                                <asp:TemplateField ItemStyle-CssClass="p-0" ItemStyle-Width="64px">
                                                                    <ItemTemplate>
                                                                        <div class="cart-delete-button">
                                                                            <div class="d-flex align-items-center justify-content-center" style="height: inherit">
                                                                                <asp:LinkButton ID="deleteButton" CssClass="btn btn-delete fs-24 ms-300" CommandName="Delete" runat="server" Text="" CommandArgument='<%# Eval("OrderID") %>' OnClientClick="return ValidateDelete()">
                                                                                    <i class="far fa-times-circle" aria-hidden="true"></i>
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
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

                <div class="row">
                    <div class="col-xl-3 col-md-4 col-sm-12 offset-md-4 offset-xl-6 p-2 p-md-3">
                        <asp:Button ID="cmdDelete" runat="server" Text="Delete All" CssClass="btn btn-outline w-100" OnClick="cmdDelete_Click" OnClientClick="return ValidateDeleteAll();" />
                    </div>
                    <div class="col-xl-3 col-md-4 col-sm-12 p -1 p-2 p-md-3">
                        <asp:Button ID="yesButton" ClientIDMode="Static" runat="server" Text="Submit Order(s)" CssClass="btn btn-black w-100" CausesValidation="true" />
                    </div>
                </div>

                <div id="myModal" class="modal" tabindex="-1" role="dialog">
                    <div class="modal-dialog" role="document" style="display: flex; align-items: center; margin-top: 0; margin-bottom: 0; height: 100%">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 10px 15px !important;">
                                <h5 class="modal-title">
                                    <img src="img/BrandLogo_RED.png" /></h5>
                                <button type="button" class="modal-close" data-bs-dismiss="modal" aria-label="Close" style="position: absolute; top: 10px; right: 15px">
                                    <span aria-hidden="true">×</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <h4>Terms and Conditions.</h4>
                                <ol>
                                    <li>Pricing subject to change.</li>
                                    <li>A confirmation will be sent as soon as the order is processed.</li>
                                    <li>Additional freight charges may be required for remote shipping locations in Canada.</li>
                                    <li>Product availability subject to weather and crop conditions.</li>
                                    <li>Delivery date subject to weather and crop conditions.</li>
                                    <li>Quality concerns must be addressed within five business days and damages noted on bill of lading.</li>
                                </ol>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="cmdSubmit" ClientIDMode="Static" runat="server" CssClass="btn btn-black" Style="padding-left: 30px; padding-right: 30px;"
                                    OnClick="cmdSubmit_Click" OnClientClick="return ValidateCart();" />

                                <button type="button" id="no" class="btn btn-outline ms-300" data-bs-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header position-relative d-flex justify-content-between align-items-center">
                                <h4 class="modal-title" id="modalViewTitle">Loading...</h4>
                            </div>
                            <div id="divModalViewBody" class="modal-body">
                                <img src="img/loading.gif" alt="Loading..." id="imgLoadingModalContent" />
                                <button type="button" class="btn-close modal-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-footer d-flex justify-content-center">
                                <button id="btnModalClose" type="button" class="btn  btn-dark px-4 mx-2" data-bs-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
    <%--    </div>--%>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">


        $('#<%=yesButton.ClientID%>').click(function () {
            if (Page_IsValid) {
                $('#myModal').modal('show');
            }
            return false;
        });

        function ValidateCart() {
            return true;
        }

        function ValidateDeleteAll() {
            return confirm('Are you sure you wish to remove ALL orders from your cart?\n\nThis action cannot be undone!');
        }

        function ValidateDelete() {
            return confirm('Are you sure you wish to remove the selected order from your cart?\n\nThis action cannot be undone!');
        }

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
            $(this).removeData('bs.modal');
        });
        $(document).ready(function () {
            $pnl = $("#<%=pnlActivitySuccess.ClientID %>");
            $pnl.delay(10000).fadeOut(500);
        });


    </script>
</asp:Content>
