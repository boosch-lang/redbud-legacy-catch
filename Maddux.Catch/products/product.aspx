<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="product.aspx.cs" Inherits="Maddux.Catch.products.product" %>


<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        .table-center td {
            vertical-align: middle !important;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li><a href="#details" class="active" data-toggle="tab">Details</a></li>
        <% //This tab is disabled on client's request
// <li><a href="#sub-products" data-toggle="tab">Sub Products</a></li> 
        %>
        <li><a href="#pictures" data-toggle="tab">Picture(s)</a></li>
        <li><a href="#catalogs" data-toggle="tab">Catalogs(s)</a></li>
        <li><a href="#orders" data-toggle="tab">Orders</a></li>
        <li><a href="#shipping" data-toggle="tab">Shipping</a></li>
    </ul>
    <div class="tab-content" style="padding: 15px">
        <div class="tab-pane fade in active" id="details">
            <div class="row">
                <div class="col-xs-12">
                    <p class="text-right">
                        <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                        <asp:Button Text="Delete Product" runat="server" ID="btnDelete"
                            OnClientClick="return confirm('Are you sure you want to delete this product?')"
                            OnClick="btnDelete_Click" CssClass="btn btn-danger" />
                        <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click"
                            CausesValidation="false" CssClass="btn btn-default" />
                    </p>
                </div>
            </div>
            <div class="panel-group">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" href="#productDetails">Product Details</a>
                        </h4>
                    </div>
                    <div id="productDetails" style="padding: 15px" class="panel-collapse collapse in" aria-expanded="true">
                        <div class="row">
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="required font-weight-bold">Item Number (Internal):</label>
                                        <asp:TextBox ID="txtInternalNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvInternalNumber" ControlToValidate="txtInternalNumber" ErrorMessage="Item Number (Internal) is required" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="font-weight-bold">Suggested Retail:</label>
                                        <asp:TextBox ID="txtSuggestedRetail" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="required font-weight-bold">Name:</label>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="reqName" ControlToValidate="txtName" ErrorMessage="Product name is required" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="font-weight-bold">Supplier:</label>
                                        <asp:DropDownList ID="ddSupplier" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="font-weight-bold">Description:</label>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="font-weight-bold">UPC Code:</label>
                                        <asp:TextBox ID="txtUPCCode" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="font-weight-bold">Category:</label>
                                        <asp:DropDownList ID="ddCategory" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="required font-weight-bold">Items Per Package:</label>
                                        <asp:TextBox ID="txtItemsPerPAckage" runat="server" type="number" step="any" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="reqItemsPerPAckage" ControlToValidate="txtItemsPerPAckage" ErrorMessage="Items Per Package is required." />
                                        <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer"
                                            ControlToValidate="txtItemsPerPAckage" ErrorMessage="Items per package must be a whole number." />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="font-weight-bold">Size:</label>
                                        <asp:TextBox ID="txtSize" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="String"
                                            ControlToValidate="txtSize" ErrorMessage="Size must be a number." />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="required font-weight-bold">Units/Case:</label>
                                        <asp:TextBox ID="txtPacksPerUnit" runat="server" type="number" step="any" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="reqPacksPerUnit" ControlToValidate="txtPacksPerUnit" ErrorMessage="Packs Per Unit is Required." />
                                        <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer"
                                            ControlToValidate="txtPacksPerUnit" ErrorMessage="Units/Case must be a whole number." />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <label class="font-weight-bold">Unit Weight:</label>
                                        <asp:TextBox ID="txtProductWeight" runat="server" type="number" step="any" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-xs-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:CheckBox ID="chkNewProd" runat="server"></asp:CheckBox>

                                        <label class="font-weight-bold">New Product</label>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:CheckBox ID="chkNotAvailable" runat="server"></asp:CheckBox>
                                        <label class="font-weight-bold">Product Not Available</label>
                                    </div>
                                    <div class="col-xs-12">
                                        <asp:TextBox ID="txtNotAvailableMsg" placeholder="reason if product is not available" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="phNewProduct" runat="server">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" href="#productDetails">Available Catalogs</a>
                            </h4>
                        </div>
                        <div style="padding: 15px" class="panel-collapse collapse in" aria-expanded="true">

                            <asp:GridView ID="gvNewCatalogs" runat="server"
                                AutoGenerateColumns="False"
                                AutoGenerateEditButton="false"
                                AllowSorting="true"
                                SortMode="Automatic"
                                CssClass="table table-hover table-bordered table-hover dataTable table-center"
                                EnableModelValidation="True"
                                Width="100%"
                                CellPadding="3"
                                EmptyDataText="No records found"
                                GridLines="Horizontal"
                                DataKeyNames="CatalogID">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSelected" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Catalog">
                                        <ItemTemplate>
                                            <%# Eval("CatalogName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Number (Customer)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtItemNumber" runat="server" CssClass="form-control" Text=''></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Cost">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control" Text='0'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" Text='0'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>

        </div>
        <!--End #details--->
        <% //hidden on client's request %>
        <div class="tab-pane fade" id="sub-products">
            <div class="row">
                <div class="col-xs-12">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Label ID="lblNoSubProducts" runat="server"
                                Text="This product does not have any sub-products."
                                Visible="False"></asp:Label>
                        </div>
                        <div class="col-xs-12">
                            <asp:GridView ID="uwGridSubProducts" runat="server"
                                AutoGenerateColumns="False"
                                AutoGenerateEditButton="false"
                                AllowSorting="true"
                                SortMode="Automatic"
                                CssClass="table table-hover table-bordered table-hover dataTable"
                                EnableModelValidation="True" Width="100%" CellPadding="3"
                                EmptyDataText="No records found"
                                GridLines="Horizontal">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Main Product #</HeaderTemplate>
                                        <ItemTemplate>
                                            <a href='productdetail.aspx?ProductID=<%# Eval("MainProductId")%>'>
                                                <%# Eval("MainProductId") %>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle Width="31%" HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SubCategoryDesc" HeaderText="Category">
                                        <ItemStyle Width="14%" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PageNumber" HeaderText="Page #" DataFormatString="{0:C}">
                                        <ItemStyle Width="7%" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemNumber" HeaderText="Item # (Cust.)">
                                        <ItemStyle Width="11%" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LinkedProductQuantity" HeaderText="Quantity">
                                        <ItemStyle Width="11%" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Size" HeaderText="Size">
                                        <ItemStyle Width="8%" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CatalogPageStart" HeaderText="Start Page">
                                        <ItemStyle Width="8%" VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Linked Product #</HeaderTemplate>
                                        <ItemTemplate>
                                            <a href='productdetail.aspx?ProductID=<%# Eval("LinkedProductId")%>'>
                                                <%# Eval("LinkedProductId") %>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle Width="31%" HorizontalAlign="Left" VerticalAlign="Top" />
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
        </div>
        <!---End #sub-products--->
        <!--Orders--->
        <div class="tab-pane fade" id="orders">
            <asp:Label ID="lblQtyOrdered" runat="server" CssClass="Ctrls pull-right" Text="Total Ordered:" Font-Bold="True"></asp:Label>
            <div class="">
                <div class="table-responsive">
                    <asp:Label ID="lblNoOrders" runat="server"
                        Text="This product does not appear on any orders."
                        Visible="False"></asp:Label>
                    <asp:GridView ID="gridOrders" runat="server"
                        AutoGenerateColumns="False"
                        AutoGenerateEditButton="false"
                        AllowPaging="true"
                        PageSize="25"
                        AllowSorting="true"
                        SortMode="Automatic"
                        CssClass="table table-hover table-bordered table-hover dataTable"
                        EnableModelValidation="True" Width="100%" CellPadding="3"
                        EmptyDataText="No records found"
                        OnPageIndexChanging="SubmitOrdersGrid_PageIndexChanging"
                        GridLines="Horizontal">
                        <PagerStyle CssClass="pagination-ys" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>Order #</HeaderTemplate>
                                <ItemTemplate>
                                    <a href='/order/orderdetail.aspx?id=<%# Eval("OrderID")%>'>
                                        <%# Eval("OrderID") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="5%" HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="OrderDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Order Date">
                                <ItemStyle Width="15%" VerticalAlign="Top" />
                            </asp:BoundField>

                            <asp:TemplateField>
                                <HeaderTemplate>Company</HeaderTemplate>
                                <ItemTemplate>
                                    <a href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                        <%# Eval("Company") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="20%" HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="State" HeaderText="State / Province">
                                <ItemStyle Width="15%" VerticalAlign="Top" />
                            </asp:BoundField>

                            <asp:BoundField DataField="CustomerPhone" HeaderText="Customer Phone #">
                                <ItemStyle Width="15%" VerticalAlign="Top" />
                            </asp:BoundField>

                            <asp:TemplateField>
                                <HeaderTemplate>Email</HeaderTemplate>
                                <ItemTemplate>
                                    <a href='mailto:<%# Eval("Email")%>'>
                                        <%# Eval("Email") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                <ItemStyle Width="15%" VerticalAlign="Top" />
                            </asp:BoundField>

                        </Columns>
                        <EmptyDataRowStyle CssClass="grdNoData" />
                        <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                        <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                    </asp:GridView>
                </div>
            </div>
        </div>
        <!--End orders-->
        <!--Shipping-->
        <div class="tab-pane fade" id="shipping">
            <asp:Label ID="lblQtyShipped" runat="server" CssClass="Ctrls pull-right" Text="Total Shipped:" Font-Bold="True"></asp:Label>
            <div class="">
                <div class="table-responsive">
                    <asp:Label ID="lblNoShipments" runat="server"
                        Text="This product does not appear on any shipments."
                        Visible="False"></asp:Label>
                    <asp:GridView ID="uwGridShipments" runat="server"
                        AutoGenerateColumns="False"
                        AutoGenerateEditButton="false"
                        AllowSorting="true"
                        SortMode="Automatic"
                        CssClass="table table-hover table-bordered table-hover dataTable"
                        EnableModelValidation="True" Width="100%" CellPadding="3"
                        EmptyDataText="No records found"
                        GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>Shipment #</HeaderTemplate>
                                <ItemTemplate>
                                    <a href='/shipping/shipmentdetail.aspx?id=<%# Eval("ShipmentID")%>'>
                                        <%# Eval("ShipmentID") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="31%" HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="DateShipped" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Date Shipped">
                                <ItemStyle Width="14%" VerticalAlign="Top" />
                            </asp:BoundField>

                            <asp:TemplateField>
                                <HeaderTemplate>Company</HeaderTemplate>
                                <ItemTemplate>
                                    <a href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                        <%# Eval("Company") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="31%" HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="State" HeaderText="State / Province">
                                <ItemStyle Width="14%" VerticalAlign="Top" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CustomerPhone" HeaderText="Customer Phone #">
                                <ItemStyle Width="14%" VerticalAlign="Top" />
                            </asp:BoundField>

                            <asp:TemplateField>
                                <HeaderTemplate>Email</HeaderTemplate>
                                <ItemTemplate>
                                    <a href='mailto:<%# Eval("Email")%>'>
                                        <%# Eval("Email") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="31%" HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                <ItemStyle Width="14%" VerticalAlign="Top" />
                            </asp:BoundField>

                        </Columns>
                        <EmptyDataRowStyle CssClass="grdNoData" />
                        <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                        <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                    </asp:GridView>
                </div>
            </div>
        </div>
        <!--End shipping-->
        <!--Pictures-->
        <div class="tab-pane fade" id="pictures">
            <div class="row">
                <div class="col-xs-6">
                    <div class="row" style="padding-top: 20px">
                        <asp:Repeater ID="rptPhotos" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4">
                                    <img src="<%# ResolveUrl(Eval("PhotoPath").ToString()) %>" id="<%# Eval("PhotoID") %>" class="img-thumbnail" alt="Product Image" />
                                    <br />
                                    <br />
                                    <div class="text-center">
                                        <asp:Button Text="Delete" ID="DeleteButton" CssClass="btn btn-danger" CommandArgument='<%# Eval("PhotoID") %>' CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this photo?');" OnClick="DeleteButton_Click1" runat="server" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="col-xs-6">
                    <div class="col-xs-12 mb-3">
                        <asp:FileUpload ID="fupPhoto" runat="server" Width="100%" />
                    </div>
                    <div style="margin-top: 15px" class="col-xs-12 mb-3">
                        <label class="font-weight-bold">Photo Description</label>
                        <asp:TextBox ID="txtPhotoDescription" TextMode="multiline"
                            Columns="50" Rows="4" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div style="margin-top: 15px" class="col-xs-12 mb-3">
                        <label class="font-weight-bold">Photo Notes</label>
                        <asp:TextBox ID="txtPhotoNotes" TextMode="multiline" Columns="50" Rows="6"
                            runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div style="margin-top: 15px" class="col-xs-12 mb-3">
                        <asp:Button Text="Upload" runat="server" ID="Button1" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
        <!--End pictures-->
        <div class="tab-pane fade" id="catalogs">
            <div class="input-group">
                <asp:DropDownList ID="ddlNewCatalog" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control"></asp:DropDownList>
                <span class="input-group-btn">
                    <asp:Button ID="btnAddCatalog" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAddCatalog_Click" />
                </span>
            </div>
            <br />
            <asp:GridView ID="gvCatalogs" runat="server"
                AutoGenerateColumns="False"
                AutoGenerateEditButton="false"
                AllowSorting="true"
                SortMode="Automatic"
                CssClass="table table-hover table-bordered table-hover dataTable"
                EnableModelValidation="True"
                Width="100%"
                CellPadding="3"
                EmptyDataText="No records found"
                GridLines="Horizontal"
                DataKeyNames="ProductID"
                OnRowCancelingEdit="gvCatalogs_RowCancelingEdit"
                OnRowDeleting="gvCatalogs_RowDeleting"
                OnRowEditing="gvCatalogs_RowEditing"
                OnRowUpdating="gvCatalogs_RowUpdating">
                <Columns>
                    <asp:TemplateField HeaderText="Catalog">
                        <ItemTemplate>
                            <%# Eval("ProductCatalog.CatalogName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Number (Customer)">
                        <ItemTemplate>
                            <%# Eval("ItemNumber") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtItemNumber" runat="server" CssClass="form-control" Text='<%# Eval("ItemNumber") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvItemNumber" ValidationGroup="edit" runat="server" ControlToValidate="txtItemNumber" ErrorMessage="Item Number (Customer) is required"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Cost">
                        <ItemTemplate>
                            <%# Eval("UnitCost") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control" Text='<%# Eval("UnitCost") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Price">
                        <ItemTemplate>
                            <%# Eval("UnitPrice") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" Text='<%# Eval("UnitPrice") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div style="white-space: nowrap;">
                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-primary" Text="Edit" />
                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this offering?');" Text="Delete" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="white-space: nowrap;">
                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-primary" Text="Update" />
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-default" Text="Cancel" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <!--End tab-content-->
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/extra.css" rel="stylesheet" />
    <link href="../css/pagination.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "details";
            $('.nav-tabs a[href="#' + tabName + '"]').tab('show');
            $(".nav-tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
</asp:Content>
