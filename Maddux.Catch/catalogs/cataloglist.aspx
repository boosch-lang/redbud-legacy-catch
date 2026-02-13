<%@ Page Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="cataloglist.aspx.cs" Inherits="Maddux.Catch.catalogs.cataloglist" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">

    <div class="row">
        <div class="col-xs-12">
            <div class="form-inline">
                <div class="form-group">
                    <a href="catalogdetail.aspx" class="btn btn-primary">
                        <i class="fa fa-plus"></i>
                        New Catalog</a>
                </div>

           <div class="form-group">
                    <asp:Label ID="lblShowAll" runat="server">
                        <asp:CheckBox ID="chkShowAll" CssClass="form-control" runat="server" AutoPostBack="True"
                            OnCheckedChanged="chkShowAll_CheckedChanged" />
                        Show All Catalogs</asp:Label>
                </div>
            <div class="form-group pull-right">
                <asp:Label ID="lblCatalogRecordCount" Font-Bold="True" runat="server" Text="RecordCount"></asp:Label>
            </div>
            </div>
        </div>
    </div>

    <div class="row ">
        <div class="col-xs-12">
            <div class="table-responsive">
                <asp:GridView ID="gridCatalogs" runat="server"
                    AutoGenerateColumns="False"
                    AutoGenerateEditButton="false"
                    AllowPaging="true"
                    AllowSorting="true"
                    SortMode="Automatic"
                    PageSize="25"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    OnPageIndexChanging="SubmitCatalogsGrid_PageIndexChanging"
                    EnableModelValidation="True" Width="100%" CellPadding="3"
                    EmptyDataText="No records found"
                    GridLines="Horizontal" OnRowDataBound="gridCatalogs_RowDataBound">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:BoundField DataField="CatalogId" HeaderText="Catalog #">
                            <ItemStyle Width="5%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:TemplateField>
                            <HeaderTemplate>Catalog Name</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='catalogdetail.aspx?CatalogID=<%# Eval("CatalogId")%>'>
                                    <%# Eval("CatalogName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="CatalogYear" HeaderText="Catalog Year">
                            <ItemStyle Width="10%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:TemplateField>
                            <HeaderTemplate>Active</HeaderTemplate>
                            <ItemTemplate><%# (Boolean.Parse(Eval("Active").ToString())) ? "Yes" : "No" %></ItemTemplate>
                            <ItemStyle Width="10%" VerticalAlign="Top" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings  Mode="NumericFirstLast" FirstPageText="First" PreviousPageText="Previous" PageButtonCount="5" NextPageText="Next" LastPageText="Last" />
                    <EmptyDataRowStyle CssClass="grdNoData" />
                    <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                    <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <!--
                        <asp:BoundField DataField="CatalogSeason" HeaderText="Catalog Season">
                            <ItemStyle Width="10%" VerticalAlign="Top" />
                        </asp:BoundField>
                        -->
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
