<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="associations.aspx.cs" Inherits="Maddux.Catch.customer.associations" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row">
        <div class="col-xs-12 text-right">
            <a href="new-association.aspx" id="btnCreateAssociation" runat="server" target="_blank" class="btn btn-primary">Create Association</a>
        </div>
    </div>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li class="active"><a href="#associations" data-toggle="tab">Associations</a></li>
        <li><a href="#unassociatedCustomers" data-toggle="tab">Unassociated Customers</a></li>
    </ul>

    <div class="row">
        <div class="tab-content" style="padding: 15px">
            <div class="tab-pane fade in active" id="associations">
                <div class="row">
                    <div class="col-xs-12" style="padding-left: 0px;">
                        <div class="pull-right">
                            <asp:DropDownList ID="ddlFilterRegion" runat="server" AutoPostBack="True" CssClass="btn btn-default btn-sm" OnSelectedIndexChanged="ddlFilterRegion_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row top-buffer">
                        <div class="col-sm-12 ">
                            <div class="table-responsive">
                                <asp:GridView ID="dgvAssociations" runat="server"
                                    CssClass="table table-hover table-bordered table-hover dataTable"
                                    GridLines="Horizontal"
                                    AutoGenerateColumns="False"
                                    ShowHeaderWhenEmpty="true"
                                    AllowPaging="false"
                                    AllowSorting="true"
                                    ShowFooter="true"
                                    SortMode="Automatic"
                                    OnPageIndexChanging="dgvAssociations_PageIndexChanging"
                                    OnRowDataBound="dgvAssociations_RowDataBound"
                                    DataKeyNames="AssociationId"
                                    EmptyDataText="There are no associations to display.">
                                    <PagerStyle CssClass="pagination-ys" />
                                    <Columns>
                                        <asp:BoundField DataField="Class" HeaderText="Class" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4 col-md-2">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>Association</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-link" style="padding:5px" CommandArgument='<%# Eval("AssociationID")%>' Text='<%# Eval("AsscDesc") %>' ID="AssociationRedirect" OnClick="AssociationRedirect_Click" runat="server" />
                                                <%--<a target="_blank" href='associationdetail.aspx?aID=<%# Eval("AssociationID")%>'>
                                                    <%# Eval("AsscDesc") %>
                                                </a>--%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CountCustomerID" HeaderText="Count" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2">
                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BlankCol" HeaderText="" HeaderStyle-CssClass="visible-md visible-lg col-md-4" ItemStyle-CssClass="visible-md visible-lg col-md-4">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="tab-pane fade" id="unassociatedCustomers">
                <div class="row top-buffer">
                    <div class="col-sm-12 ">
                        <div class="table-responsive">
                            <asp:GridView ID="dgvCustomers" runat="server"
                                CssClass="table table-hover table-bordered table-hover dataTable"
                                GridLines="Horizontal"
                                AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="true"
                                AllowPaging="false"
                                ShowFooter="true"
                                DataKeyNames="CustomerId"
                                EmptyDataText="There are no customers without associations.">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Customer</HeaderTemplate>
                                        <ItemTemplate>
                                            <a target="_blank" href='customerdetail.aspx?CustomerID=<%# Eval("CustomerID")%>'>
                                                <%# Eval("Company") %>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Contact" HeaderText="Contact" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                                    <asp:BoundField DataField="City" HeaderText="City" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                                    <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2 col-md-2"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
