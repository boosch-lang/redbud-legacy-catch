<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="Maddux.Catch.credit.search" %>
<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvCredits" runat="server"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                    ShowFooter="true" OnRowDataBound="dgvCredits_RowDataBound"
                    EmptyDataText="No credit memos found.">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-lg-1">
                            <HeaderTemplate>Credit #</HeaderTemplate>
                            <ItemTemplate>
                                <a href="creditdetail.aspx?CreditID=<%# Eval("CreditID") %>&CustomerId=<%# Eval("CustomerId") %>"><%# Eval("CreditID") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreateDate" HeaderText="Date Created" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass= "visible-sm visible-md visible-lg col-xs-2 col-lg-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-xs-2 col-lg-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-7 col-lg-3" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-7 col-lg-3">
                            <HeaderTemplate>Ship To</HeaderTemplate>
                            <ItemTemplate>
                                <a href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                    <%# Eval("CustomerName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OfficeNotes" HeaderText="Notes" HeaderStyle-CssClass= "visible-md visible-lg col-lg-5" ItemStyle-CssClass="visible-md visible-lg col-lg-5">
                            <ItemStyle VerticalAlign="Top"/>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
