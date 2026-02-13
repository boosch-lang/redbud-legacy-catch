<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="myaccountactivity.aspx.cs" Inherits="Maddux.Catch.customer.myaccountactivity" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="col-xs-12" style="padding-left: 0px;">
    </div>
    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="gridActivity"
                    runat="server"
                    AllowPaging="true"
                    AllowSorting="true"
                    SortMode="Automatic"
                    PageSize="25"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal"
                    OnPageIndexChanging="gridActivity_PageIndexChanging"
                    AutoGenerateColumns="False"
                    ShowHeaderWhenEmpty="true"
                    ShowFooter="true"
                    EmptyDataText="There are no records found.">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Company</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerID")%>'>
                                    <%# Eval("Company") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Date</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Convert.ToDateTime(Eval("ActivityDate")).ToLocalTime().ToString("MMM dd, yyyy hh:mm tt") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerEmail" HeaderText="Email" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ActivityDesc" HeaderText="Activity Desc" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Address" HeaderText="Address" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="City" HeaderText="City" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" PreviousPageText="Previous" PageButtonCount="5" NextPageText="Next" LastPageText="Last" />
                    <%--<PagerSettings Mode="NextPreviousFirstLast" PageButtonCount="5" PreviousPageText="Previous" NextPageText="Next" />
                    <PagerSettings Mode="Numeric" PageButtonCount="5"  />--%>
                    <EmptyDataRowStyle CssClass="grdNoData" />
                    <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                    <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
