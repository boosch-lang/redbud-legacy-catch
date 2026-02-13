<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="Maddux.Catch.shipping.search" %>
<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row top-buffer">  
        <div class="col-sm-12 ">  
            <div class="table-responsive">
                <asp:GridView ID="dgvShipments" runat="server"
                    CssClass="table table-hover table-bordered table-hover dataTable" 
                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                    ShowFooter="true" OnRowDataBound="dgvShipments_RowDataBound"
                    EmptyDataText="No shipments found.">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-3 col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-lg-1">
                            <HeaderTemplate>Shipment #</HeaderTemplate>
                            <ItemTemplate>
                                <a href="shipmentdetail.aspx?id=<%# Eval("ShipmentID") %>&customerId=<%# Eval("CustomerId") %>"><%# Eval("ShipmentID") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass= "visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Order #</HeaderTemplate>
                            <ItemTemplate>
                                <a href="/order/orderdetail.aspx?id=<%# Eval("OrderID") %>"><%# Eval("OrderID") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreateDate" HeaderText="Date Created" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass= "visible-sm visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>                            
                        <asp:BoundField DataField="ShippingMethodDesc" HeaderText="Ship Via" HeaderStyle-CssClass= "visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-md visible-lg col-lg-2">
                            <ItemStyle VerticalAlign="Top"/>
                        </asp:BoundField>                                                                                                           
                        <asp:TemplateField HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-6 col-lg-3" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-lg-3">
                            <HeaderTemplate>Ship To</HeaderTemplate>
                            <ItemTemplate>
                                <a href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                    <%# Eval("ShippingName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField> 
                        <asp:BoundField DataField="Province" HeaderText="Province" HeaderStyle-CssClass= "visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top"/>
                        </asp:BoundField>                                                                                                           
                        <asp:BoundField DataField="ShipmentTotal" HeaderText="Total" DataFormatString="{0:C}" HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-3 col-lg-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-lg-2">
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:BoundField>                            
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
