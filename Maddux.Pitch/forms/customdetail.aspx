<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customdetail.aspx.cs" Inherits="Maddux.Pitch.forms.customdetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .text-multiline {
            white-space: pre-line;
        }

        .bulk-rack-row {
            background-color: #87a492;
            color: white;
        }
    </style>
</head>
<body>
    <form id="frmRack" runat="server">
        <div id="litError" style="padding-top: 10px" runat="server"></div>
        <div class="row bottom-buffer align-top">
            <div class="col-lg-9 col-md-9 col-sm-8 col-xs-12">
                <p id="pRackDescription" runat="server" class="bottom-buffer"></p>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:ListView ID="lvRacks"
                    OnItemDataBound="lvRacks_ItemDataBound"
                    runat="server"
                    Visible="true"
                    DataKeyNames="RackID"
                    ClientIDMode="Predictable">
                    <ItemTemplate>

                        <div class="row">
                            <div class="row">
                                <div class="col-auto">
                                    <div class="rack-discount-container">
                                        <img src="<%# Eval("RackImage") %>" class="card-img-top rounded-25 img-fluid" style="max-height: 200px; max-width: 200px" data-toggle="modal" data-target=".modal-profile-lg" />

                                        <asp:PlaceHolder runat="server" Visible='<%# (Double)Eval("Discount") > 0 %>'>
                                            <div class="rack-discount-overlay ms-400"><%# ((Double)Eval("Discount")) %>% Volume Discount</div>
                                        </asp:PlaceHolder>
                                    </div>

                                </div>
                                <div class="col">
                                    <div class="row d-none d-sm-flex py-3">
                                        <div class="col-12">
                                            <p class="text-multiline"><%# Eval("ProductCatalogDescription") %></p>
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
                                <div class="card-content-scroll-shadow-wrapper mt-4"></div>
                                <div class="card-content-scroll-wrapper">
                                    <div class="card-content-scroll-section overflow-hidden">
                                        <div class="row mb-2">
                                            <div class="col-12">
                                                <div class="bg-dark text-white py-3 rounded-25">
                                                    <div class="row">
                                                        <div class="col ms-400 fs-22">
                                                            <div class="ps-3">
                                                                <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Standard %>'>Quantity
                                                                </asp:PlaceHolder>
                                                                <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("RackType") == (int)Redbud.BL.RackType.Bulk %>'>Smart Stack
                                                                </asp:PlaceHolder>
                                                            </div>
                                                        </div>
                                                        <div class="col ms-400 fs-22">
                                                            Item Image
                                                        </div>
                                                        <div class="col ms-400 fs-22">
                                                            Description
                                                        </div>
                                                        <div class="col ms-400 fs-22">
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
                                                                    <div class="row align-items-center bulk-rack-row">
                                                                        <div class="col-2">
                                                                            <div class="row">
                                                                                <div class="col-12">
                                                                                    <div class="input-group rptProductRack">
                                                                                        <asp:TextBox ID="TextBox1"
                                                                                            runat="server"
                                                                                            type="text"
                                                                                            min="0"
                                                                                            disabled="disabled"
                                                                                            CssClass="form-control bg-input text-center"
                                                                                            data-name='<%# string.Format("{0}_{1}", Eval("ParentRackId"), Eval("RackId")) %>'
                                                                                            data-default='<%# Eval("DefaultQuantity") %>'
                                                                                            data-rack='<%# Eval("RackId") %>'
                                                                                            data-parentRack='<%# Eval("ParentRackId") %>'
                                                                                            Text='<%# Eval("DefaultQuantity") %>'
                                                                                            Style="height: 40px;" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col">
                                                                            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") > 0 %>'>
                                                                                <a href="<%#Eval("RackPhoto")%>" data-lightbox="<%# Eval("RackID") %>-rack" data-title='<%# Eval("RackName") %>'>
                                                                                    <img class="thumbnail img-fluid rounded" style="max-height: 70px" src="<%#Eval("RackPhoto")%>" alt="<%#Eval("RackPhoto") %>">
                                                                                </a>
                                                                                <asp:Repeater ID="repPhotos" runat="server" DataSource='<%#Eval("Photos")%>'>
                                                                                    <ItemTemplate>
                                                                                        <a href="<%#Eval("PhotoPath")%>" data-lightbox="<%# Eval("RackID") %>-rack" data-title='<%# Eval("RackName") %>'></a>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </asp:PlaceHolder>
                                                                            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("PhotoCount") == 0 %>'>
                                                                                <a href="/img/program-not-available.jpg" data-lightbox="<%# Eval("RackName") %>" data-title='<%# Eval("RackName") %>'>
                                                                                    <img class="thumbnail img-fluid rounded" style="max-height: 70px" src="/img/program-not-available.jpg" alt="rack-not-available">
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


                                                                                                <asp:TextBox ID="txtCustomProductQuantity"
                                                                                                    runat="server"
                                                                                                    type="text"
                                                                                                    min="0"
                                                                                                    disabled="disabled"
                                                                                                    CssClass="form-control text-center bg-input"
                                                                                                    data-name='<%# string.Format("{0}_{1}", Eval("RackID"), Eval("ProductID")) %>'
                                                                                                    data-default='<%# Eval("DefaultQuantity") %>'
                                                                                                    data-rack='<%# Eval("RackID") %>'
                                                                                                    Text='<%# Eval("DefaultQuantity") %>' />
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
                                                                                                <div class="col ms-300 fs-20"><%# Eval("PackagesPerUnit")  %></div>
                                                                                            </asp:PlaceHolder>
                                                                                            <asp:PlaceHolder runat="server" Visible='<%# (int)Eval("ParentRackType") == (int)Redbud.BL.RackType.Bulk %>'>
                                                                                                <div class="col ms-300 fs-20"><%# CalculateRackQty((int)Eval("PackagesPerUnit"), (double)Eval("DefaultQuantity"))  %></div>
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
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </form>
</body>
</html>
