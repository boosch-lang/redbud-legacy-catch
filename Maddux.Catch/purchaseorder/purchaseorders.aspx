<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="purchaseorders.aspx.cs" Inherits="Maddux.Catch.purchaseorders.purchaseorders" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="col-xs-12" style="padding-left: 0px;">
        <div class="col-xs-12" style="padding-left: 0px;">
            <a id="lnkNewPurchaseOrder" href="/purchaseorder/purchaseorderdetail.aspx" runat="server" class="btn btn-primary">
                <i class="fa fa-plus"></i>&nbsp;New Purchase Order
            </a>
        </div>
        <div class="pull-right">
            <asp:DropDownList ID="ddlFilterHub" runat="server" AutoPostBack="True" CssClass="btn btn-default btn-sm" OnSelectedIndexChanged="ddlFilterHub_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row top-buffer">
        <div class="col-sm-12">
            <div class="table-responsive">
                <asp:GridView ID="dgvPurchaseOrders"
                    runat="server"
                    AllowPaging="false"
                    AllowSorting="true"
                    ShowFooter="true"
                    SortMode="Automatic"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal"
                    ShowHeaderWhenEmpty="true"
                    EmptyDataText="There are no purchase orders to display."
                    AutoGenerateColumns="false"
                    DataKeyNames="PurchaseOrderID" OnPageIndexChanging="dgvPurchaseOrders_PageIndexChanging">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <HeaderTemplate>PO #</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href="purchaseorderdetail.aspx?id=<%# Eval("PurchaseOrderID") %>" title="Purchase Order Details" title_text="Purchase Order #<%# Eval("PurchaseOrderID") %>">
                                    <%# Eval("PurchaseOrderID") %></a>
                                <asp:HiddenField runat="server" ID="PurchaseOrderID" Value='<%# Eval("PurchaseOrderID") %>' />
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DeliveryHub.Name" HeaderText="Delivery Hub" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PickupDate" HeaderText="Pickup Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ShipDate" HeaderText="Ship Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FullRackCount" HeaderText="# Full Racks" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HalfRackCount" HeaderText="# 1/2 Racks" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QuarterRackCount" HeaderText="# 1/4 Racks" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalFeet" HeaderText="Allocated Space (feet)" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" PreviousPageText="Previous" PageButtonCount="5" NextPageText="Next" LastPageText="Last" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
