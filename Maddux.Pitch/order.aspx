<%@ Page Title="Orders | Redbud" Language="C#" MasterPageFile="~/Authorized.Master" AutoEventWireup="true" CodeBehind="order.aspx.cs" Inherits="Maddux.Pitch.order" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="css/bootstrap-datetimepicker.min.css" type="text/css" />
    <style>
        .btn-primary {
            color: #fff !important;
        }

        .phOtherOpenOrders0 {
            display: none;
        }

        .phOtherShippedOrders0 {
            display: none;
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

        .accordion-item {
            background-color: #F4EBE4 !important;
            border: none;
        }

        .accordion-button {
            background-color: #F4EBE4 !important;
            font-size: 20px;
        }

            .accordion-button:focus {
                box-shadow: none;
                border: none;
                color: #000;
            }

            .accordion-button:not(.collapsed) {
                box-shadow: none;
                color: #000;
            }

        .nav-link {
            background: none;
            color: #000;
        }

        .nav-tabs {
            border-bottom: 1px solid #000;
        }

            .nav-tabs .nav-link {
                border-bottom: 1px solid #000;
                font-weight: 300;
            }

                .nav-tabs .nav-link:hover {
                    color: #000;
                    border-bottom: 1px solid #000;
                }

                .nav-tabs .nav-link.active,
                .nav-tabs .nav-item.show .nav-link {
                    color: #000;
                    font-weight: 600;
                    background-color: #F4EBE4;
                    border-left: 1px solid #000;
                    border-top: 1px solid #000;
                    border-right: 1px solid #000;
                    border-bottom: 1px solid transparent;
                    border-top-left-radius: 20px;
                    border-top-right-radius: 20px;
                }



        .card {
            border-color: #000;
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
            max-width: 95vw;
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

        .tab-padding {
            padding-left: 60px;
            margin-bottom: 25px;
        }

        .tab-content-padding {
            padding-left: 60px;
        }


        .btn-alert-close {
            background-color: transparent !important;
            border: none !important;
            color: white !important;
        }

        /* On screens that are 600px or less, set the background color to olive */
        @media screen and (max-width: 600px) {
            .tab-padding {
                padding-left: 20px;
            }

            .tab-content-padding {
                padding-left: 30px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody2" runat="server">
    <div class="container-fluid">
        <div class="row no-gutters">
            <div class="col-12 px-0 px-sm-1">
                <div style="padding-top: 50px !important;">
                    <asp:Literal runat="server" ID="litWelcomeMessage"></asp:Literal>
                </div>
                <div class="redbud-margin">
                    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
                </div>
                <asp:HiddenField ID="hfCurrentTab" runat="server" Value="tabOpenOrders" />
                <div style="margin-bottom: 15px !important"></div>

                <ul class="nav nav-tabs fs-20 tab-padding pt-3" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active ms-600 fs-20 pt-4" id="order-tab" onclick="SetCurrentTab('tabOpenOrders');" data-bs-toggle="tab" data-bs-target="#tabOpenOrders" type="button" role="tab" aria-controls="tabOpenOrders" aria-selected="true">To Be Shipped</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link pt-4 ms-600 fs-20" id="shipped-tab" onclick="SetCurrentTab('tabShippedOrders');" data-bs-toggle="tab" data-bs-target="#tabShippedOrders" type="button" role="tab" aria-controls="tabShippedOrders" aria-selected="false">Shipment History</button>
                    </li>
                </ul>
                <div class="redbud-margin">
                    <div class="tab-content">
                        <div id="tabOpenOrders" class="tab-pane fade show active" aria-labelledby="order-tab">
                            <div id="divOtherOpenOrders" runat="server">
                                <div>
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="accordion" id="accOtherOpenOrders" role="tablist" aria-multiselectable="true">
                                                <asp:Repeater ID="rptOtherOpenOrders" runat="server" OnItemDataBound="rptOtherOpenOrders_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="accordion-item">
                                                            <p class="accordion-header phOtherOpenOrders<%# Container.ItemIndex %> fs-20 ms-300" id="headingOO<%#Eval("CustomerID") %>">
                                                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
                                                                    data-bs-target="#collapseOO<%#Eval("CustomerID") %>" aria-expanded="false"
                                                                    aria-controls="collapseOO<%#Eval("CustomerID") %>">
                                                                    <%# Eval("Company") %>
                                                                </button>
                                                            </p>
                                                            <div id="collapseOO<%#Eval("CustomerID") %>" class="accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>"
                                                                aria-labelledby="headingOO<%#Eval("CustomerID") %>">
                                                                <div class="accordion-body px-0">
                                                                    <div class="row card-deck">
                                                                        <asp:Repeater ID="repOpenOrders" runat="server">
                                                                            <ItemTemplate>
                                                                                <div class="col-xl-3 col-lg-4 col-md-6 col-sm-12 col-12 mb-4 ">
                                                                                    <div class="card h-100 px-0 py-1 px-md-3">
                                                                                        <div class="card-body d-flex flex-column">
                                                                                            <div class="row">
                                                                                                <div class="col">
                                                                                                </div>
                                                                                                <p class="card-text ms-250 fs-20">
                                                                                                    <asp:Label runat="server" Text='<%#Eval("OrderID") %>'></asp:Label>
                                                                                                </p>
                                                                                                <p class="card-text ms-250 fs-20">
                                                                                                    Ship Week:
                                                                                            <asp:Label runat="server" Text='<%#Eval("RequestedShipDate", "{0:MMMM dd, yyyy}") %>'></asp:Label>
                                                                                                </p>
                                                                                                <p class="card-text ms-250 fs-20">
                                                                                                    <asp:Label runat="server" Text='<%#Eval("RackName") %>'></asp:Label>
                                                                                                </p>
                                                                                                <p class="card-text ms-250 fs-20">
                                                                                                    <asp:Label runat="server" Text='<%#Eval("Total", "{0:C2}") %>'></asp:Label>
                                                                                                </p>
                                                                                                <p class="card-text ms-250 fs-20">
                                                                                                    Placed:
                                                                                            <asp:Label runat="server" Text='<%#Eval("OrderDateDisplay", "{0:MMMM dd, yyyy}") %>'></asp:Label>
                                                                                                </p>
                                                                                            </div>
                                                                                            <div class="row justify-content-center mt-4">
                                                                                                <div class="col text-center">
                                                                                                    <a href="orderdetail.aspx?id=<%# Eval("OrderID") %>" title="Order Details" data-bs-toggle="modal" data-bs-target="#modalView" data-remote="false" title_text="<%# Eval("OrderID") %>" class="btn btn-dark ms-300 fs-20 my-2  w-100">View Details</a>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row justify-content-center">
                                                                                                <div class="col text-center">
                                                                                                    <a href='<%# String.Format("download-confirmation.aspx?id={0}", Eval("OrderID")) %>' target="_blank" runat="server" class="btn btn-dark ms-300 fs-20 my-2  w-100">Print Order Confirmation</a>
                                                                                                </div>
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
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="tabShippedOrders" class="tab-pane fade" aria-labelledby="shipped-tab">
                            <asp:Literal runat="server" ID="litNoOtherShippedOrders"></asp:Literal>
                            <div id="divOtherShippedOrders" runat="server">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="accordion" id="accOtherShippedOrders" role="tablist" aria-multiselectable="true">
                                                <asp:Repeater ID="rptOtherShippedOrders" runat="server"
                                                    OnItemDataBound="rptOtherShippedOrders_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="accordion-item">
                                                            <p class="accordion-header phOtherShippedOrders<%# Container.ItemIndex %> fs-20 ms-300" id="headingSO<%#Eval("CustomerID") %>">
                                                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
                                                                    data-bs-target="#collapseSO<%#Eval("CustomerID") %>" aria-expanded="false"
                                                                    aria-controls="collapseSO<%#Eval("CustomerID") %>">
                                                                    <%# Eval("Company") %>
                                                                </button>
                                                            </p>
                                                            <div id="collapseSO<%#Eval("CustomerID") %>" class="accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>"
                                                                aria-labelledby="headingSO<%#Eval("CustomerID") %>">
                                                                <div class="accordion-body">
                                                                    <div class="card-deck row">
                                                                        <asp:Repeater ID="repShippedOrders" runat="server">
                                                                            <ItemTemplate>
                                                                                <div class="col-xl-3 col-lg-4 col-md-6 col-sm-12 col-12 mb-4 ">
                                                                                    <div class="card h-100 px-0 py-1 px-md-3">
                                                                                        <div class="card-body d-flex flex-column">
                                                                                            <div class="row">
                                                                                                <div class="col">
                                                                                                    <p class="card-text ms-250 fs-20">
                                                                                                        <asp:Label runat="server" Text='<%#Eval("OrderID") %>'></asp:Label>
                                                                                                    </p>
                                                                                                    <p class="card-text ms-250 fs-20">
                                                                                                        Ship Date:
                                                                                                <asp:Label runat="server" Text='<%#(Eval("ShipDate") == null ? "Pending" : Eval("ShipDate", "{0:MMMM dd, yyyy}")) %>'></asp:Label>
                                                                                                    </p>
                                                                                                    <p class="card-text ms-250 fs-20">
                                                                                                        <asp:Label runat="server" Text='<%#Eval("RackName") %>'></asp:Label>
                                                                                                    </p>
                                                                                                    <p class="card-text ms-250 fs-20">
                                                                                                        <asp:Label runat="server" Text='<%#Eval("Total", "{0:C2}") %>'></asp:Label>
                                                                                                    </p>
                                                                                                    <p class="card-text ms-250 fs-20">
                                                                                                        Placed:
                                                                                                <asp:Label runat="server" Text='<%#Eval("OrderDateDisplay", "{0:MMMM dd, yyyy}") %>'></asp:Label>
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="row justify-content-center mt-4">
                                                                                            <div class="col text-center">
                                                                                                <a href="orderdetail.aspx?id=<%# Eval("OrderID") %>" title="Order Details" data-bs-toggle="modal" data-bs-target="#modalView" data-remote="false" title_text="<%# Eval("OrderID") %>" class="btn btn-dark ms-300 fs-20 my-2  w-100">View Details</a>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row justify-content-center">
                                                                                            <div class="col text-center">
                                                                                                <a href='<%# String.Format("download-confirmation.aspx?id={0}{1}", Eval("OrderID"),(Eval("ShipDate") == null) ? "" : "&sid=" + Eval("ShipmentId")) %>'
                                                                                                    target="_blank" runat="server" class="btn btn-dark ms-300 fs-20 my-2 w-100"><%# (Eval("ShipDate") == null ? "Print Order Confirmation" : "Print Invoice") %></a>
                                                                                            </div>
                                                                                        </div>
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
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                            <!-- Hidden on clients' request-->
                            <div class="row d-none">
                                <div class="col-xs-12">
                                    <div class="col-xs-12 grandtotal-bar" runat="server" id="divShippedGrandTotal">
                                        Grand Total
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header position-relative d-flex justify-content-between align-items-center">
                            <h4 class="modal-title mx-auto text-center fs-30 ms-400" id="modalViewTitle">Loading...</h4>

                            <button type="button" class="btn-close modal-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div id="divModalViewBody" class="modal-body">
                            <img src="img/loading.gif" alt="Loading..." id="imgLoadingModalContent" />
                        </div>
                        <div class="modal-footer  d-flex justify-content-center">
                            <button id="btnModalClose" type="button" class="btn  btn-dark px-4 mx-2" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">
        function SetCurrentTab(tabID) {
            document.getElementById("<%=hfCurrentTab.ClientID%>").value = tabID;
            return true;
        }

        $("#modalView").on("show.bs.modal", function (e) {
            console.log("e");
            $(this).find(".modal-title").text("Loading...");
            $(this).find(".modal-body").html("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

            var eventSource = $(e.relatedTarget);
            if (eventSource.text().length > 0) {
                $(this).find(".modal-title").text(eventSource.attr("title_text"));
                $(this).find(".modal-body").load(eventSource.attr("href"));
            }
        });

    </script>
</asp:Content>
