<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="catalogdetail.aspx.cs" Inherits="Maddux.Catch.catalogs.catalogdetail" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap-multiselect.css") %>" type="text/css" />
    <style>
        .btn-group.multiselect-dropdown-div {
            width: 100%;
        }

        .multiselect.dropdown-toggle.btn.btn-default {
            width: 100% !important;
            text-align: left;
        }

        .caret {
            float: right;
            margin-top: 8px;
        }

        #cphBody_ProductsControls {
            text-align: center;
        }

        .multiselect-container.dropdown-menu {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row row-margin">
        <div class="alert alert-success alert-dismissible" id="successAlert" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <strong>Success!</strong> <span id="spSuccessMessage" runat="server"></span>
        </div>
        <asp:Literal Text="" ID="litError" runat="server" />
        <div class="col-xs-12">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete" runat="server" ID="btnDeleteProgramCatalog"
                OnClientClick="return confirm('Are you sure you want to delete this catalog?')"
                OnClick="btnDeleteProgramCatalog_Click" CssClass="btn btn-danger" />
            <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click" CssClass="btn btn-default" />
        </div>
    </div>

    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li><a href="#details" class="active" data-toggle="tab">Details</a></li>
        <li><a href="#shipdate" data-toggle="tab" id="tabShipDates">Ship Dates</a></li>
        <li id="tab-item-associations"><a href="#associations" data-toggle="tab" id="tabAssociations" runat="server">Associations</a></li>
        <li><a href="#products" class="active" id="tabProducts" runat="server" data-toggle="tab">Products</a></li>
    </ul>

    <div class="tab-content" style="padding: 15px">

        <div class="tab-pane fade" id="details">
            <div class="row">
                <div class="col-xs-12" id="NewCatalog" runat="server">
                    <br />
                    <div style="float: right">
                        <small class="text-danger">Copy data from another catalog</small>
                        <div style="display: flex">
                            <asp:DropDownList CssClass="form-control" DataTextField="Text" DataValueField="Value" ID="ddlCatalogs" runat="server"></asp:DropDownList>&nbsp;&nbsp;
                            <button id="CopyCatalogBtn" data-toggle="modal" data-target="#copyCatalogModal" class="btn btn-primary">Copy</button>

                        </div>
                        <asp:Literal Text="" ID="litCatalogError" runat="server" />
                    </div>
                </div>
            </div>

            <div class="panel-group">


                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" href="#catalogDetails">Catalog Details</a>
                        </h4>
                    </div>
                    <div id="catalogDetails" class="panel-collapse collapse in" aria-expanded="true">
                        <div class="row">
                            <div class="col-md-6" style="margin-top: 20px">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Catalog Name: </label>
                                    <asp:TextBox ID="txtCatalogName" ClientIDMode="Static" runat="server"
                                        CssClass="form-control" MaxLength="100"
                                        TabIndex="7"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvCatalogName" runat="server"
                                        ControlToValidate="txtCatalogName" Display="Dynamic"
                                        ErrorMessage="You must enter a catalog name."></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Customer Catalog Name: </label>
                                    <asp:TextBox ID="txtCustomerCatalogName" runat="server"
                                        CssClass="form-control" MaxLength="100"
                                        TabIndex="7"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvCustomerCatalogName" runat="server"
                                        ControlToValidate="txtCustomerCatalogName" Display="Dynamic"
                                        ErrorMessage="You must enter a customer catalog name."></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Catalog Year: </label>
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="CtrlsLeftAlign form-control" TabIndex="9">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator
                                        ID="rfvddlYear" runat="server"
                                        ControlToValidate="ddlYear" Display="Dynamic"
                                        ErrorMessage="You must select a catalog year."></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-6" style="margin-top: 20px">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Program: </label>
                                    <div class="ProductProgram" style="padding-right: 10px; text-align: left; vertical-align: top;">
                                        <asp:DropDownList ID="ddlProductProgram" runat="server" CssClass="CtrlsLeftAlign form-control">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Display Order: </label>
                                    <div class="DisplayOrder" style="padding-right: 10px; text-align: left; vertical-align: top;">
                                        <asp:TextBox ID="txtDisplayOrder" runat="server" type="number"
                                            CssClass="form-control" min="0"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="col-xs-12">
                                    <div class="col-xs-4 pl-0">
                                        <label class="font-weight-bold">Active: </label>
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:CheckBox ID="chkActive" runat="server" CssClass="Ctrls formatted-chk" />
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-4 pl-0">
                                        <label class="font-weight-bold">Show On My Account: </label>
                                    </div>
                                    <div class="col-xs-8">
                                        <asp:CheckBox ID="chkShowOnMyAccount" runat="server" CssClass="Ctrls formatted-chk" />
                                    </div>
                                </div>
                            </div>
                            <div style="margin-bottom: 15px" class="col-xs-12">
                                <div class="col-xs-12">
                                    <label class="required font-weight-bold">Notes: </label>
                                    <asp:TextBox ID="txtNotes" runat="server"
                                        TextMode="multiline" Columns="50" Rows="6"
                                        CssClass="form-control"
                                        TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!--End details--->
        <div class="tab-pane fade" id="shipdate">
            <div class="row">
                <div class="col-xs-12">
                    <a href="/products/shipdate.aspx?CatalogId=<%= CatalogID %>" class="btn btn-primary" title_text="New Ship Date" data-toggle="modal" data-target="#modalView" data-remote="false">
                        <i class="fa fa-plus"></i>&nbsp;New Ship Date
                    </a>
                </div>
            </div>
            <asp:GridView ID="dgvShipdates" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no ship dates to display.">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                        <HeaderTemplate>
                            Ship Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="/products/shipdate.aspx?id=<%# Eval("ShipDateID") %>&catalogId=<%# Eval("CatalogID") %>" title="Ship Date" data-toggle="modal" data-target="#modalView" data-remote="false" title_text="Ship Date - <%# string.Format("{0:MMMM dd, yyyy}",Eval("ShipDate")) %>">
                                <%# string.Format("{0:MMMM dd, yyyy}",Eval("ShipDate")) %></a>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderDeadlineDate" HeaderText="Order Deadline" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="tab-pane fade" id="associations">
            <div class="col-xs-5">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblAssociations" runat="server" Font-Bold="True" Text="Assigned Associations:"></asp:Label>
                        <br />
                        <asp:ListBox ID="lbAssignedAssociations" DataTextField="Text" DataValueField="Value" runat="server" CssClass="Ctrls" Height="250px"
                            Width="100%" TabIndex="36"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-xs-2 action-margin" id="ProductsControls" runat="server">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Button ID="cmdAddPro" runat="server" Text="  <<  "
                            CausesValidation="False" data-validate-target="none" OnClick="cmdAddAssoc_Click" Width="50px" CssClass="Ctrls btn btn-success" TabIndex="35" /><br />
                        <br />
                        <asp:Button ID="cmdRemPro" runat="server" Text="  >>  "
                            CausesValidation="False" data-validate-target="none" OnClick="cmdRemAssoc_Click" Width="50px" CssClass="Ctrls btn btn-danger" TabIndex="37" /><br />
                        <br />
                        <asp:Button ID="cmdSavePro" runat="server" OnClick="cmdSaveAssoc_Click" Text="Save"
                            Width="50px" CausesValidation="False" CssClass="Ctrls btn btn-primary" TabIndex="38" />
                    </div>
                </div>
            </div>
            <div class="col-xs-5">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblAvailableAssociations" runat="server" Font-Bold="True" Text="Available Associations:"></asp:Label><br />
                        <asp:ListBox ID="lbAvailableAssociations" runat="server" CssClass="Ctrls" Height="250px"
                            Width="100%" TabIndex="34"></asp:ListBox>
                    </div>
                </div>
            </div>

            <div class="col-xs-5">
                <br />
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblCatalogStates" runat="server" Font-Bold="True">Catalog States:</asp:Label>
                        <asp:ListBox ID="ddlCatalogStates" ClientIDMode="Static" SelectionMode="Multiple" runat="server" CssClass=" form-control btn btn-default btn-sm multiselect"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="products">
            <div class="row">
                <div class="col-xs-12 text-right">
                    <a target="_blank" style="margin-bottom: 10px" href="/products/productDetail.aspx?catalogId=<%= CatalogID %>" class="btn btn-primary">
                        <i class="fa fa-plus"></i>Add New Product
                    </a>
                </div>
            </div>
            <asp:GridView runat="server"
                ID="gridCatalogProducts"
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
                OnRowDataBound="gridCatalogProducts_RowDataBound">
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
                            <a target="_blank" href='/products/productdetail.aspx?ProductID=<%#Eval("ProductId")%> <%#"&CatalogId="%> <%#Eval("CatalogId")%>'>
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
            </asp:GridView>
        </div>
    </div>

    <asp:TextBox ID="txtActiveTab" runat="server" CssClass="form-control" Type="hidden"></asp:TextBox>
    <!-- Modal -->
    <div class="modal fade" id="copyCatalogModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="exampleModalLabel">Confirm New Catalog Name
                        <span class="pull-right">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </span>
                    </h4>

                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <label class="required font-weight-bold">New Catalog Name: </label>
                            <asp:TextBox ID="txtNewCatalogName" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button Text="Copy" ID="btnCopyData" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnCopyData_Click" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row">
                        <h4 class="modal-title col-xs-11" id="modalViewTitle">Loading...</h4>
                        <div class="text-right col-xs-1">
                            <a href="#" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></a>
                        </div>
                    </div>
                </div>
                <div id="divModalViewBody" class="modal-body">
                    <img src="img/loading.gif" alt="Loading..." id="imgLoadingModalContent" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/extra.css" rel="stylesheet" />
    <script src="../js/extra.js"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap-multiselect.js") %>"></script>

    <script>
        $(document).ready(function () {
            $('.multiselect').multiselect({
                buttonContainer: '<div class="btn-group multiselect-dropdown-div" />',
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: true,
                enableFullValueFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            $("#CopyCatalogBtn").on("click", function (e) {
                e.preventDefault();
                $("#txtNewCatalogName").val(`${$("#txtCatalogName").val()}`);
            })
        });
        $("#modalView").on("show.bs.modal", function (e) {
            $(this).find(".modal-title").text("Loading...");
            $(this).find(".modal-body").html("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

            var eventSource = $(e.relatedTarget);
            if (eventSource.text().length > 0) {
                $(this).find(".modal-title").text(eventSource.attr("title_text"));
                $(this).find(".modal-body").html('<div class="embed-responsive embed-responsive-4by3" id="iframeContainer"></div>');
                $('<iframe id="modal-popup-content"  style="border:none;" allowfullscreen="true"  />').appendTo('#iframeContainer');
                $('#modal-popup-content').attr("src", eventSource.attr("href"));
            }
        });

        $('#modalView').on('hidden.bs.modal', function (e) {
            $(this).find('iframe').html("").attr("src", "");
        });

        function CloseModal(frameElement) {
            if (frameElement) {
                var dialog = $(frameElement).closest(".modal");
                if (dialog.length > 0) {
                    dialog.modal("hide");
                }
            }
        }
    </script>
</asp:Content>
