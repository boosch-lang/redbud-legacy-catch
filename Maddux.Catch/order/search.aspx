<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="Maddux.Catch.order.search" %>
<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row top-buffer">  
        <div class="col-sm-12 ">  
            <div class="table-responsive">
                <asp:GridView ID="dgvOrders" runat="server"
                    CssClass="table table-hover table-bordered table-hover dataTable" 
                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                    ShowFooter="true" OnRowDataBound="dgvOrders_RowDataBound"
                    EmptyDataText="No orders found.">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-1">
                            <HeaderTemplate>Order #</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href="orderdetail.aspx?id=<%# Eval("OrderID") %>"><%# Eval("OrderID") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>                            
                        <asp:TemplateField HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-6 col-lg-3" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-lg-3">
                            <HeaderTemplate>Customer</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                    <%# Eval("Company") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField> 
                        <asp:BoundField DataField="State" HeaderText="Province" HeaderStyle-CssClass= "visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top"/>
                        </asp:BoundField>                                                                                                           
                        <asp:BoundField DataField="CustomerPhone" HeaderText="Phone" HeaderStyle-CssClass= "visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-md visible-lg col-lg-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>  
                        <asp:TemplateField HeaderStyle-CssClass= "visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-md visible-lg col-lg-2">
                            <HeaderTemplate>Email</HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hypEmail" runat="server" />
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>                                                                                                                                                              
                        <asp:BoundField DataField="OrderTotal" HeaderText="Total" DataFormatString="{0:C}" HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-1">
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
