<%@  Language="C#" AutoEventWireup="true" CodeBehind="orderdetail.aspx.cs" Inherits="Maddux.Pitch.orderdetail" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmRack" runat="server">
        <div id="litError" style="padding-top: 10px" runat="server"></div>
        <div class="row">
            <div class="col-12">
                <asp:ListView ID="lvRacks"
                    runat="server"
                    Visible="true"
                    DataKeyNames="RackID"
                    ClientIDMode="Predictable">
                    <ItemTemplate>
                        <div class="row">
                            <div class="row">
                                <div class="col-auto">
                                    <img src="<%# Eval("RackImage") %>" class="card-img-top rounded-25 img-fluid" style="max-height: 200px; max-width: 200px" data-toggle="modal" data-target=".modal-profile-lg" />
                                </div>
                                <div class="col">
                                    <div class="row py-3">
                                        <div class="col-12">
                                            <div class="rack-discount-container">
                                                <h3 style="margin-top: 0px;"><%# Eval("RackName") %></h3>
                                                <h4 class="font-weight-bold" style="margin-top: 0px;"><%# Eval("CatalogName") %></h4>
                                                <p class="bottom-buffer"><%# Eval("RackDescription") %></p>

                                                <asp:PlaceHolder runat="server" Visible='<%# (Double)Eval("Discount") > 0 %>'>
                                                    <div class="rack-discount-overlay ms-400"><%# ((Double)Eval("Discount")) %>% Volume Discount</div>
                                                </asp:PlaceHolder>
                                            </div>
                                        </div>
                                        <div class="col-12 col-md-4">
                                            <h4 class="font-weight-bold">Purchase Order:</h4>
                                            <p><%# Eval("PurchaseOrderNumber") %></p>
                                        </div>
                                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("IsShipped") %>'>
                                            <div class="col-12 col-md-4">
                                                <h4 class="font-weight-bold">Ship To:</h4>
                                                <p><%# Eval("ShipTo") %></p>
                                            </div>
                                        </asp:PlaceHolder>
                                        <div class="col-12 col-md-4">
                                            <h4 class="font-weight-bold">Ship Week:</h4>
                                            <p><%# Eval("ShipWeek") %></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col">
                            <div class="card-content-scroll-shadow-wrapper mt-4">
                                <div class="card-content-scroll-wrapper">
                                    <div class="card-content-scroll-section overflow-hidden">
                                        <div class="row mb-2">
                                            <div class="col-12">
                                                <div class="bg-dark text-white  py-3">
                                                    <div class="row">
                                                        <div class="col-4 text-center ms-400 fs-22">
                                                            Item Image
                                                        </div>
                                                        <div class="col-4 text-center ms-400 fs-22">
                                                            Description
                                                        </div>
                                                        <div class="col-4 text-center ms-400 fs-22">
                                                            Case Qty
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="card mt-3" style="width: 100%;">
                                                    <div class="card-body">
                                                        <asp:Repeater ID="rptProducts" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                                                            <ItemTemplate>
                                                                <div class="row align-items-center py-1">
                                                                    <div class="col-4">
                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") > 0 %>'>
                                                                            <a href="<%#Eval("ItemPhoto")%>" data-lightbox="<%# Eval("ProductID") %>-product" data-title='<%# Eval("ProductName") %>'>
                                                                                <img class="thumbnail img-fluid" style="max-height: 70px" src="<%#Eval("ItemPhoto")%>" alt="<%#Eval("ItemPhoto") %>">
                                                                            </a>
                                                                            <asp:Repeater ID="repPhotos" runat="server" DataSource='<%#Eval("Photos")%>'>
                                                                                <ItemTemplate>
                                                                                    <a href="<%#Eval("PhotoPath")%>" data-lightbox="<%# Eval("ProductID") %>-product" data-title='<%# Eval("ProductName") %>'></a>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </asp:PlaceHolder>
                                                                        <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") == 0 %>'>
                                                                            <a href="/img/program-not-available.jpg" data-lightbox="<%# Eval("ProductName") %>" data-title='<%# Eval("ProductName") %>'>
                                                                                <img class="thumbnail img-fluid" style="max-height: 70px" src="/img/program-not-available.jpg" alt="rack-not-available"></a>
                                                                        </asp:PlaceHolder>
                                                                    </div>
                                                                    <div class="col">
                                                                        <div class="row align-items-center py-1">

                                                                            <div class="col text-center ms-300 fs-20"><%# Eval("ProductName") %></div>
                                                                            <div class="col text-center ms-300 fs-20"><%# Eval("QuantityTotal") %></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row  align-items-end py-1">
                                                    <div class="col text-end ms-300 fs-20">Subtotal: <%# Eval("SubTotal") %></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </form>
</body>
</html>
