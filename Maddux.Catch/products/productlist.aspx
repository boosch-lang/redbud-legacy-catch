<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="productlist.aspx.cs" Inherits="Maddux.Catch.products.productlist" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        .modal-header {
            padding: 9px 15px;
            border-bottom: 1px solid #eee;
            background-color: #ed174f;
            color: #fff;
            -webkit-border-top-left-radius: 5px;
            -webkit-border-top-right-radius: 5px;
            -moz-border-radius-topleft: 5px;
            -moz-border-radius-topright: 5px;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
        }

        #ImportProductsModal .form-control {
            padding: 0;
        }

        #ImportProductsModal .close {
            font-size: 21px;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            opacity: 1;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <asp:Literal runat="server" ID="litWarnings"></asp:Literal>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-inline">
                <div class="form-group">
                    <asp:Button runat="server" type="button" OnClick="BtnDownloadProductsImportTemplate_Click"
                        class="btn btn-success btn-sm" Text="Download Product Import Template"></asp:Button>
                    <a href="#" class="btn btn-info btn-sm" title="Import Products" data-toggle="modal"
                        data-target="#ImportProductsModal" data-remote="false"><i class="fa fa-plus"></i>Import Products</a>
                    <a href="product.aspx" class="btn btn-primary btn-sm">
                        <i class="fa fa-plus"></i>
                        New Product</a>
                    <asp:Button type="button" runat="server" OnClick="btnExportCatalog_Click"
                        class="btn btn-success btn-sm" Text="Export Catalog"></asp:Button>
                </div>
            </div>
        </div>
        <div class="col-xs-12">
            <br />
            <div class="form-inline">
                <div class="form-group">
                    <label id="lblCatalog" runat="server">Catalog</label>
                    <asp:DropDownList ID="ddlCatalog" runat="server"
                        AutoPostBack="True" Width="350px"
                        CssClass="form-control"
                        OnSelectedIndexChanged="ddlCatalog_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblShowAll" runat="server">
                        <asp:CheckBox ID="chkShowAll" CssClass="form-control" runat="server" AutoPostBack="True"
                            OnCheckedChanged="chkShowAll_CheckedChanged" />
                        Show All Catalogs</asp:Label>
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
                <asp:GridView ID="gridProducts" runat="server"
                    AutoGenerateColumns="False"
                    AutoGenerateEditButton="false"
                    AllowPaging="true"
                    AllowSorting="true"
                    SortMode="Automatic"
                    PageSize="25"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    OnPageIndexChanging="SubmitProductsGrid_PageIndexChanging"
                    EnableModelValidation="True" Width="100%" CellPadding="3"
                    EmptyDataText="No records found"
                    GridLines="Horizontal" OnRowDataBound="gridProducts_RowDataBound">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:BoundField DataField="ItemNumber" HeaderText="Item # (Cust.)">
                            <ItemStyle Width="11%" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ItemNumberInternal" HeaderText="Item # (Int.)">
                            <ItemStyle Width="11%" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderTemplate>Name</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='product.aspx?ItemNumberInternal=<%#Eval("ItemNumberInternal")%>'>
                                    <%# Eval("ProductName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="31%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductDesc" HeaderText="Category">
                            <ItemStyle Width="14%" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Size" HeaderText="Size">
                            <ItemStyle Width="8%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SupplierName" HeaderText="Supplier">
                            <ItemStyle Width="15%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="UnitPrice" HeaderText="Case Price" DataFormatString="{0:C}">
                            <ItemStyle Width="7%" VerticalAlign="Top" HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="grdNoData" />
                    <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                    <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                </asp:GridView>
            </div>
        </div>
    </div>


    <!-- Import Products Modal -->
    <div class="modal fade" id="ImportProductsModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Import Products</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="label-control custom-file-label" for="fuProducts">Select file to import</label>
                        <asp:FileUpload runat="server" ID="fuProducts" CssClass="form-control custom-file-input" />
                        <asp:RegularExpressionValidator ID="revProducts" runat="server" CssClass="text-danger"
                            ErrorMessage="Please Upload Excel files only."
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.xls|.XLS|.xlsx|.XLSX)$"
                            ControlToValidate="fuProducts"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnImportProducts" runat="server" CssClass="btn btn-primary" Text="Import" OnClick="BtnImportProducts_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
