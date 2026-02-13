<%@ Page Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="categorylist.aspx.cs" Inherits="Maddux.Catch.categories.categorylist" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <div class="form-inline">
                <div class="form-group">
                    <a href="categorydetail.aspx" class="btn btn-primary">
                        <i class="fa fa-plus"></i>
                        New Category</a>
                    <asp:Button type="button" runat="server" OnClick="btnExportCategories_Click"
                        class="btn btn-success" Text="Export Categories"></asp:Button>
                </div>

                <div class="form-group pull-right">
                    <asp:Label ID="lblRecordCount" runat="server" Text="RecordCount"></asp:Label>
                </div>
            </div>
        </div>
    </div>

        <div class="row top-buffer">
        <div class="col-xs-12">
            <div class="table-responsive">
                <asp:GridView ID="gridCategories" runat="server"
                    AutoGenerateColumns="False"
                    AutoGenerateEditButton="false"
                    AllowPaging="true"
                    AllowSorting="true"
                    SortMode="Automatic"
                    PageSize="25"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    OnPageIndexChanging="SubmitCategoriesGrid_PageIndexChanging"
                    EnableModelValidation="True" Width="100%" CellPadding="3"
                    EmptyDataText="No records found"
                    GridLines="Horizontal" OnRowDataBound="gridCategories_RowDataBound">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>                   
                        <asp:BoundField DataField="SubCategoryID" HeaderText="Category #">
                            <ItemStyle Width="5%" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderTemplate>Category Description</HeaderTemplate>
                            <ItemTemplate>
                                <a href='categorydetail.aspx?CategoryID=<%#Eval("SubCategoryID")%>'>
                                    <%# Eval("SubCategoryDesc") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="30%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="grdNoData" />
                    <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                    <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                    <PagerSettings  Mode="NumericFirstLast" FirstPageText="<<" PreviousPageText="<" PageButtonCount="5" NextPageText=">" LastPageText=">>" />
                </asp:GridView>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
        <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
