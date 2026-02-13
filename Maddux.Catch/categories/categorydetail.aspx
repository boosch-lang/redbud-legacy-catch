<%@ Page Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="categorydetail.aspx.cs" Inherits="Maddux.Catch.categories.categorydetail" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="alert alert-success alert-dismissible" id="successAlert" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <strong>Success!</strong> <span id="spSuccessMessage" runat="server"></span>
    </div>

    <div class="alert alert-danger alert-dismissible" id="errorAlert" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <strong>Error!</strong> <span id="spErrorMessage" runat="server"></span>
    </div>

    <div class="row top-buffer">
        <div class="col-xs-12">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="BtnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete Category" runat="server" ID="btnDelete"
                OnClientClick="return confirm('Are you sure you want to delete this category?')"
                OnClick="BtnDelete_Click" CssClass="btn btn-danger" />
            <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="BtnCancel_Click" CssClass="btn btn-default" />
        </div>
    </div>

    <ul class="nav nav-tabs top-buffer" role="tablist" id="nav" runat="server">
        <li id="tab-item-details"><a href="#details" class="active" data-toggle="tab" id="tabDetails" runat="server">Details</a></li>
        <li id="tab-item-associations"><a href="#associations" data-toggle="tab" id="tabAssociations" runat="server">Products</a></li>
    </ul>

    <div class="tab-content" style="padding: 15px">
        <!---Details---->
        <div class="tab-pane fade" id="details">
            <div class="col-xs-6">
                <div class="col-xs-12">
                    <div class="col-xs-3">
                        <label class="required">Category Description: </label>
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtCategoryDescription" runat="server"
                            CssClass="form-control" MaxLength="100"
                            TabIndex="7" required="required"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            ID="rfvCategoryDescription" runat="server"
                            ControlToValidate="txtCategoryDescription" Display="Dynamic"
                            ErrorMessage="You must enter a category description."></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-xs-12">
                    <div class="col-xs-3">
                        <label class="required">Product Category: </label>
                    </div>
                    <div class="col-xs-9">
                        <div class="SupProductCategory">
                            <asp:DropDownList ID="ddlSupProductCategory" runat="server" required="required" CssClass="CtrlsLeftAlign form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator
                                ID="rfvSupProductCategory" runat="server"
                                ControlToValidate="ddlSupProductCategory" Display="Dynamic"
                                ErrorMessage="You must selected a Sup Product Category"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!---End details--->
        <!--Associations---->
        <div class="tab-pane fade" id="associations">
            <div class="row top-buffer">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gridProducts" runat="server"
                            AutoGenerateColumns="False"
                            AutoGenerateEditButton="false"
                            AllowSorting="true"
                            SortMode="Automatic"
                            AllowPaging="true"
                            PageSize="25"
                            OnPageIndexChanging="SubmitProductsGrid_PageIndexChanging"
                            CssClass="table table-hover table-bordered table-hover dataTable"
                            EnableModelValidation="True" Width="100%" CellPadding="3"
                            EmptyDataText="No records found"
                            GridLines="Horizontal" OnRowDataBound="gridProducts_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:BoundField DataField="ProductID" HeaderText="Product #">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CatalogId" HeaderText="Catalog #">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderTemplate>Product Name</HeaderTemplate>
                                    <ItemTemplate>
                                        <a href='/products/productdetail.aspx?ProductID=<%#Eval("ProductId")%>'>
                                            <%# Eval("ProductName") %>
                                        </a>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="grdNoData" />
                            <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                            <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <!--End associations--->
    </div>
    <asp:TextBox ID="txtActiveTab" runat="server" CssClass="form-control" Type="hidden"></asp:TextBox>        
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
    <script src="../js/extra.js"></script>
</asp:Content>
