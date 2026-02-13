<%@ Page Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="rackdetail.aspx.cs" Inherits="Maddux.Catch.racks.rackdetail" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        label {
            padding-left: 5px;
        }

        .red-text {
            color: #ed174f;
        }

        .img-placeholder {
            border: 3px dashed #9e9e9e; /* Darker gray border */
            background-color: #e0e0e0; /* Light gray background */
            padding: 80px; /* Increased padding */
            color: #616161; /* Medium gray text color */
            font-size: 28px; /* Larger font size */
            font-weight: bold;
            display: flex; /* Use flexbox */
            align-items: center; /* Center vertically */
            justify-content: center; /* Center horizontally */
            text-align: center; /* Center text inside the div */
            border-radius: 12px; /* Slightly larger border radius */
            margin: 20px 0; /* Increased margin */
            width: 100%; /* Maintain the width */
            height: 350px; /* Increased height for a rectangle shape */
        }
    </style>
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:HiddenField runat="server" ID="CurrentTab" ClientIDMode="Static" Value="details" />
    <div class="row row-margin">
        <div class="alert alert-success alert-dismissible" id="successAlert" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <strong>Success!</strong> <span id="spSuccessMessage" runat="server"></span>
        </div>

        <div class="alert alert-danger alert-dismissible" id="errorAlert" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <strong>Error!</strong> <span id="spErrorMessage" runat="server"></span>
        </div>
        <asp:Literal runat="server" ID="litError"></asp:Literal>
        <div class="col-xs-12">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete Rack" runat="server" ID="btnDeleteRack"
                OnClientClick="return confirm('Are you sure you want to delete this Rack?')"
                OnClick="btnDeleteRack_Click" CssClass="btn btn-danger" />
            <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click" CssClass="btn btn-default" />
        </div>
    </div>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li id="tab-item-details">
            <a href="#details" class="active" data-toggle="tab" id="tabDetails" runat="server">Details</a>
        </li>
        <li id="tab-item-pictures">
            <a href="#pictures" data-toggle="tab" id="tabPictures" runat="server">Picture(s)</a>
        </li>
        <li id="tab-item-orders"><a href="#orders" data-toggle="tab" id="tabOrders" runat="server">Orders</a></li>
        <li id="tab-item-associations">
            <a href="#associations" data-toggle="tab" id="tabAssociations" runat="server">Products</a>
        </li>
        <li id="tab-item-rack-associations">
            <a href="#rackAssociations" data-toggle="tab" id="tabRackAssociations" runat="server">Product Racks</a>
        </li>
    </ul>
    <div class="row">
        <div class="tab-content" style="padding: 15px">
            <!--details--->
            <div class="tab-pane fade" id="details">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#rackDetails">Rack Details</a>
                                    </h4>
                                </div>
                                <div id="rackDetails" style="padding: 15px" class="panel-collapse collapse in" aria-expanded="true">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <label class="font-weight-bold required">Rack Name: </label>
                                            <asp:TextBox ID="txtRackName" runat="server"
                                                CssClass="form-control" MaxLength="100"
                                                TabIndex="7" required="required"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvRackName" runat="server"
                                                ControlToValidate="txtRackName" Display="Dynamic"
                                                ErrorMessage="You must enter a rack name."></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-xs-12">

                                            <label class="font-weight-bold">Rack Type: </label>
                                            <asp:DropDownList ID="ddlRackType" runat="server" CssClass="CtrlsLeftAlign form-control">
                                            </asp:DropDownList>

                                        </div>
                                        <div class="col-xs-12">
                                            <div class="row" style="padding: 10px 0;">
                                                <div class="col-xs-12">
                                                    <label class="font-weight-bold">Is Active? </label>
                                                    &nbsp;                                               
                                                    <asp:CheckBox ID="chkBoxIsActive" runat="server" CssClass="Ctrls formatted-chk" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-xs-12">
                                            <label class="font-weight-bold">Rack Description: </label>
                                            <asp:TextBox ID="txtRackDescription" runat="server" TextMode="multiline" Columns="50" Rows="6"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-xs-12">

                                            <label class="font-weight-bold">Discount: </label>
                                            <asp:DropDownList ID="ddlDiscount" runat="server" CssClass="CtrlsLeftAlign form-control">
                                            </asp:DropDownList>

                                        </div>
                                        <div class="col-xs-12">
                                            <label class="required font-weight-bold">Rack Catalog: </label>
                                            <div class="RackCatalog">
                                                <asp:DropDownList ID="ddlRackCatalog" runat="server" AutoPostBack="true" required="required" CssClass="CtrlsLeftAlign form-control">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator
                                                    ID="rfvRackCatalog" runat="server"
                                                    ControlToValidate="ddlRackCatalog" Display="Dynamic"
                                                    ErrorMessage="You must selected a Rack Catalog"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="col-xs-12">
                                            <label class="font-weight-bold">Display Order: </label>
                                            <div class="DisplayOrder">
                                                <asp:TextBox ID="txtDisplayOrder" runat="server" type="number"
                                                    CssClass="form-control" min="0"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-xs-12">
                                            <label class="required font-weight-bold">Rack Size: </label>
                                            <div class="RackSize">
                                                <asp:DropDownList ID="ddlRackSize" runat="server" required="required" CssClass="CtrlsLeftAlign form-control">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator
                                                    ID="rfvRackSize" runat="server"
                                                    ControlToValidate="ddlRackSize" Display="Dynamic"
                                                    ErrorMessage="You must selected a Rack Size"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-xs-12">
                                            <label class="required font-weight-bold">Minimum # Items: </label>
                                            <asp:TextBox ID="txtRackMinimumItems" runat="server" type="number"
                                                CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvRackMinimumItems" runat="server"
                                                ControlToValidate="txtRackMinimumItems" Display="Dynamic"
                                                ErrorMessage="You must enter a rack minimum # of items."></asp:RequiredFieldValidator>

                                        </div>

                                        <div class="col-xs-12">
                                            <label class="required font-weight-bold">Maximum # Items: </label>
                                            <asp:TextBox ID="txtRackMaximumItems" runat="server" type="number"
                                                CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvRackMaximumItems" runat="server"
                                                ControlToValidate="txtRackMaximumItems" Display="Dynamic"
                                                ErrorMessage="You must enter a rack maximum # of items."></asp:RequiredFieldValidator>
                                            <div class="col-xs-3">
                                            </div>
                                            <div class="col-xs-9">
                                            </div>
                                        </div>
                                        <div class="col-xs-12">
                                            <div class="col-xs-12 pl-0">
                                                <label class="font-weight-bold">Rack Dimensions: </label>
                                            </div>
                                            <div class="col-xs-4 col-md-3 col-lg-2 pl-0">
                                                <label class="font-weight-bold">Length: </label>
                                                <asp:TextBox runat="server" ID="RackLength" min="0" ClientIDMode="Static" type="number" CssClass="form-control" />
                                            </div>
                                            <div class="col-xs-4 col-md-3 col-lg-2 pl-0">
                                                <label class="font-weight-bold">Width: </label>
                                                <asp:TextBox runat="server" ID="RackWidth" min="0" ClientIDMode="Static" type="number" CssClass="form-control" />
                                            </div>
                                            <div class="col-xs-4 col-md-3 col-lg-2 pl-0">
                                                <label class="font-weight-bold">Height: </label>
                                                <asp:TextBox runat="server" ID="RackHeight" min="0" ClientIDMode="Static" type="number" CssClass="form-control" />
                                            </div>
                                            <div class="col-xs-4 col-md-3 col-lg-2 pl-0">
                                                <label class="font-weight-bold">Weight: </label>
                                                <asp:TextBox runat="server" ID="RackWeight" min="0" ClientIDMode="Static" type="number" Step="0.01" CssClass="form-control" MaxLength="20" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12">
                                            <div class="row">
                                                <div class="col-xs-3">
                                                    <label class="font-weight-bold">Allow Customization: </label>
                                                </div>
                                                <div class="col-xs-9">
                                                    <asp:CheckBox ID="chkAllowCustomization" runat="server" CssClass="Ctrls formatted-chk" />
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="col-xs-12">
                                            <label class="font-weight-bold">Order Form: </label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtForm" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                                                <span class="input-group-btn">
                                                    <button onclick="moxman.browse({view: 'thumbs',  fields: 'txtForm', convert_urls : false, rootpath: '/uploads/files/', insert_filter: function(file) {file.url = file.path.replace('/content/{0}',''); file.meta.url = file.url;} });" class="btn btn-default" type="button">Select/Upload</button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <label class="h4 font-weight-bold required">Available Ship Dates:</label>
                        </div>
                        <div class="col-xs-12">
                            <asp:Repeater ID="rpAvailableShipDates" runat="server">
                                <ItemTemplate>
                                    <div class="panel-group">
                                        <div class="panel panel-primary">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" href="#<%#Eval("ProductRackShipDateID") %>"><%# Eval("ShipDate", "{0:MMMM dd, yyyy}") %></a>
                                                </h4>
                                            </div>
                                            <div id="<%#Eval("ProductRackShipDateID") %>" style="padding: 15px" class="panel-collapse collapse" aria-expanded="false">
                                                <asp:HiddenField ID="hfproductRackShipDateID" Value='<%#Eval("ProductRackShipDateID") %>' runat="server" />
                                                <asp:CheckBox runat="server" ID="cbxshipDate" CssClass="formatted-chk" Checked='<%# ((bool)Eval("Active") == true) ? true : false %>' Text='<%# Eval("ShipDate", "{0:MMMM dd, yyyy}") %>' />
                                                <br />

                                                <label class="font-weight-bold">Quantity Available:</label>
                                                <asp:TextBox runat="server" ID="txtShipDateRacksvailable" CssClass="form-control" type="number" min="0" value='<%#Eval("Available") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <span runat="server" id="spAvailableShipDates"></span>
                        </div>


                    </div>
                </div>
            </div>
            <!--End details--->
            <!-- Pictures -->
            <div class="tab-pane fade" id="pictures">
                <div class="col-xs-4">
                    <div class="col-xs-12">
                        <div class="text-center">
                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="ImageTextBox" />
                            <div id="imagePlaceholder" class="img-placeholder">Upload new photo</div>
                            <asp:Image ID="rackImage" CssClass="img-thumbnail" ClientIDMode="Static" runat="server" Style="display: none;" /><br />
                            <br />
                            <button id="btnUploadPhoto" type="button" class="btn btn-default " onclick="moxman.browse({fields: 'ImageTextBox', rootpath: '/uploads/files/racks'}); ">Upload New Image</button>
                            <asp:Button Text="Add to Rack" ClientIDMode="Static" OnClick="btnAddPhoto_Click" runat="server" ID="btnAddPhoto" CssClass="btn btn-primary" Style="display: none;" />
                        </div>
                        <br />
                    </div>
                </div>
                <div class="col-xs-8">
                    <div class="row" style="padding-top: 20px">
                        <asp:Repeater ID="rptPhotos" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4">
                                    <img src="<%# ResolveUrl(Eval("PhotoPath").ToString()) %>" id="<%# Eval("PhotoID") %>" class="img-thumbnail" alt="Rack Image" />
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

            </div>
            <!-- End of Pictures -->
            <!--Orders-->
            <div class="tab-pane fade" id="orders">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gridRackOrders" runat="server"
                            AutoGenerateColumns="False"
                            AutoGenerateEditButton="false"
                            AllowPaging="true"
                            AllowSorting="true"
                            SortMode="Automatic"
                            PageSize="25"
                            CssClass="table table-hover table-bordered table-hover dataTable"
                            EnableModelValidation="True" Width="100%" CellPadding="3"
                            OnPageIndexChanging="gridRackOrders_PageIndexChanging"
                            EmptyDataText="No records found"
                            GridLines="Horizontal">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>Order #</HeaderTemplate>
                                    <ItemTemplate>
                                        <a href='/order/orderdetail.aspx?id=<%# Eval("OrderId")%>'>
                                            <%# Eval("OrderId") %>
                                        </a>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>Customer </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href='/customer/customerdetail.aspx?CustomerID=<%# Eval("Order.Customer.CustomerID")%>'>
                                            <%# Eval("Order.Customer.Company") %>
                                        </a>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="Order.OrderDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="OrderDate">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Order.OrderNotes" HeaderText="Order Notes">
                                    <ItemStyle Width="20%" VerticalAlign="Top" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Order.ShipDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="ShipDate">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Order.RequestedShipDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Requested Ship Date">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Order.ConfirmationSentDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Confirmation Sent Date">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Order.UpdatedBy" HeaderText="Updated By">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Order.CreatedBy" HeaderText="Created By">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                    <ItemStyle Width="5%" VerticalAlign="Top" />
                                </asp:BoundField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" PreviousPageText="<" PageButtonCount="5" NextPageText=">" LastPageText=">>" />
                            <EmptyDataRowStyle CssClass="grdNoData" />
                            <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                            <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <!---End orders--->
            <!--Product Associations-->
            <div class="tab-pane fade" id="associations">
                <div class="col-xs-5">
                    <div class="col-xs-12">
                        <h3 class="text-center">Assigned Products
                            <span class="pull-right">
                                <asp:Button Text="Update Qty" CausesValidation="false" CssClass="btn btn-primary" ID="btnUpdateQty" OnClick="btnUpdateQty_Click" runat="server" /></span>
                        </h3>
                        <table class="table table-striped" style="margin-bottom: 0;">
                            <thead>
                                <tr class="row">
                                    <th class="col-xs-2">#</th>
                                    <th class="col-xs-6">Product Name</th>
                                    <th class="col-xs-4">Default Qty</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptAssigned" runat="server">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnProductID" Value='<%#Eval("ProductID") %>' runat="server" />
                                        <tr class="row">
                                            <td class="col-xs-2">
                                                <asp:CheckBox runat="server" ID="chkProductSelected" Checked="false" CssClass="chkSelected" />
                                            </td>
                                            <td class="col-xs-6"><%# Eval("ProductName") %></td>
                                            <td class="col-xs-4">
                                                <asp:HiddenField ID="hdnID" Value='<%#Eval("ProductID") %>' runat="server" />
                                                <asp:TextBox runat="server" CssClass="form-control txtProductQuantity" type="number" min="0" data-default='<%# Eval("DefaultQuantity") %>' ID="txtProductQuantity" Text='<%# Eval("DefaultQuantity") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr class="row">
                                    <td class="col-xs-2"></td>
                                    <td class="col-xs-6">
                                        <span class="pull-right" style="font-weight: 700">Default Total :
                                        </span>
                                    </td>
                                    <td class="col-xs-4">
                                        <asp:Literal runat="server" ID="totalQty"></asp:Literal>
                                        <br />

                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="col-xs-2 action-margin" id="racksControls" runat="server">
                    <div class="col-xs-12">
                        <div class="PageMainText col-xs-12">
                            <br />
                            <asp:Button runat="server" ID="AddSelected" OnClick="AddSelected_Click" Width="150" Text=" << Add Selected " CausesValidation="False" CssClass="btn btn-success" /><br />
                            <br />
                            <asp:Button runat="server" ID="RemoveSelected" OnClick="RemoveSelected_Click" Width="150" Text="Remove Selected >>" CausesValidation="False" CssClass="btn btn-danger" />
                        </div>
                    </div>
                </div>

                <div class="col-xs-5">
                    <div class="col-xs-12">
                        <h3 class="text-center">Available Products</h3>
                        <table class="table table-striped" style="margin-bottom: 0;">
                            <thead>
                                <tr class="row">
                                    <th class="col-xs-2">#</th>
                                    <th class="col-xs-6">Product Name</th>
                                    <th class="col-xs-4">Default Qty</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRackProducts" runat="server">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnProductID" Value='<%#Eval("ProductID") %>' runat="server" />
                                        <tr class="row">
                                            <td class="col-xs-2">
                                                <asp:CheckBox runat="server" ID="chkProductSelected" Checked="false" CssClass="chkSelected" />
                                            </td>
                                            <td class="col-xs-6"><%# Eval("ProductName") %></td>
                                            <td class="col-xs-4">
                                                <asp:TextBox runat="server" CssClass="form-control txtProductQuantity" type="number" min="0" data-default='0' ID="txtProductQuantity" Text='0' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <!---End product associations--->
            <!--Bulk Rack Associations-->
            <div class="tab-pane fade" id="rackAssociations">
                <div class="col-xs-5">
                    <div class="col-xs-12">
                        <h3 class="text-center">Assigned Racks
                            <span class="pull-right">
                                <asp:Button Text="Update Qty" CausesValidation="false" CssClass="btn btn-primary" ID="btnUpdateRackQty" OnClick="btnUpdateRackQty_Click" runat="server" /></span>
                        </h3>
                        <table class="table table-striped" style="margin-bottom: 0;">
                            <thead>
                                <tr class="row">
                                    <th class="col-xs-2">#</th>
                                    <th class="col-xs-6">Rack Name</th>
                                    <th class="col-xs-4">Default Qty</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptAssignedRacks" runat="server">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnRackID" Value='<%#Eval("ProductRackID") %>' runat="server" />
                                        <tr class="row">
                                            <td class="col-xs-2">
                                                <asp:CheckBox runat="server" ID="chkRackSelected" Checked="false" CssClass="chkSelected" />
                                            </td>
                                            <td class="col-xs-6"><%# Eval("RackName") %></td>
                                            <td class="col-xs-4">
                                                <asp:HiddenField ID="hdnRID" Value='<%#Eval("ProductRackID") %>' runat="server" />
                                                <asp:TextBox runat="server" CssClass="form-control txtRackQuantity" type="number" min="0" data-default='<%# Eval("DefaultQuantity") %>' ID="txtRackQuantity" Text='<%# Eval("DefaultQuantity") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr class="row">
                                    <td class="col-xs-2"></td>
                                    <td class="col-xs-6">
                                        <span class="pull-right" style="font-weight: 700">Default Total :
                                        </span>
                                    </td>
                                    <td class="col-xs-4">
                                        <asp:Literal runat="server" ID="totalRackQty"></asp:Literal>
                                        <br />

                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="col-xs-2 action-margin" id="racksControl2" runat="server">
                    <div class="col-xs-12">
                        <div class="PageMainText col-xs-12">
                            <br />
                            <asp:Button runat="server" ID="AddSelectedRack" OnClick="AddSelectedRack_Click" Width="150" Text=" << Add Selected " CausesValidation="False" CssClass="btn btn-success" /><br />
                            <br />
                            <asp:Button runat="server" ID="RemoveSelectedRack" OnClick="RemoveSelectedRack_Click" Width="150" Text="Remove Selected >>" CausesValidation="False" CssClass="btn btn-danger" />
                        </div>
                    </div>
                </div>

                <div class="col-xs-5">
                    <div class="col-xs-12">
                        <h3 class="text-center">Available Racks</h3>
                        <table class="table table-striped" style="margin-bottom: 0;">
                            <thead>
                                <tr class="row">
                                    <th class="col-xs-2">#</th>
                                    <th class="col-xs-6">Rack Name</th>
                                    <th class="col-xs-4">Default Qty</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRackRacks" runat="server">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnRackID" Value='<%#Eval("RackID") %>' runat="server" />
                                        <tr class="row">
                                            <td class="col-xs-2">
                                                <asp:CheckBox runat="server" ID="chkRackSelected" Checked="false" CssClass="chkSelected" />
                                            </td>
                                            <td class="col-xs-6"><%# Eval("RackID") %> - <%# Eval("RackName") %></td>
                                            <td class="col-xs-4">
                                                <asp:TextBox runat="server" CssClass="form-control txtRackQuantity" type="number" min="0" data-default='0' ID="txtRackQuantity" Text='0' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <!---End bulk rack associations--->
        </div>
    </div>
    <asp:TextBox ID="txtActiveTab" runat="server" CssClass="form-control" Type="hidden"></asp:TextBox>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script src="https://cloud.tinymce.com/stable/jquery.tinymce.min.js"></script>
    <script src="https://cloud.tinymce.com/stable/tinymce.min.js?apiKey=tq4yog56p3newhumnylnl4w3xhgpwknugdwnyxgsekhzaba7"></script>
    <script src="../js/moxiemanager/js/moxman.loader.min.js"></script>
    <script src="../js/tinyMCE.js"></script>
    <link href="../css/extra.css" rel="stylesheet" />
    <link href="../css/pagination.css" rel="stylesheet" />
    <script src="../js/extra.js"></script>
    <script>
        $(document).ready(function () {

            var imageTextBox = $('#<%= ImageTextBox.ClientID %>');
            var imagePlaceholder = $('#imagePlaceholder');
            var rackImage = $('#<%= rackImage.ClientID %>');
            var addButton = $("#<%= btnAddPhoto.ClientID %>");
            var uploadButton = $("#btnUploadPhoto");

            function updateImage() {
                var imageUrl = `${$("#ImageTextBox").val()}`;
                var rackImage = $("#rackImage");
                var placeholder = $("#imagePlaceHolder");

                if (imageUrl) {
                    rackImage.prop("src", `${imageUrl}`);
                    rackImage.show();
                    imagePlaceholder.hide();
                    addButton.show();
                    uploadButton.hide();
                } else {
                    rackImage.hide();
                    imagePlaceholder.show();
                    addButton.hide();
                    uploadButton.show();
                }
            }
            imageTextBox.on('change', updateImage);
            updateImage();

            var tab = $("#CurrentTab").val();
            $('.nav-tabs li a[href="#' + tab + '"]').tab('show');
        });


    </script>
</asp:Content>
