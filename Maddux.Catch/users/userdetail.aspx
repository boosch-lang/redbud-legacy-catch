<%@ Page Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="userdetail.aspx.cs" Inherits="Maddux.Catch.users.userdetail" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="alert alert-success alert-dismissible" id="successAlert" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <strong>Success!</strong> <span id="spSuccessMessage" runat="server"></span>
    </div>
    <div class="row row-margin">
        <div class="col-xs-12">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete User" runat="server" ID="btnDeleteUser"
                OnClientClick="return confirm('Are you sure you want to delete this user?')"
                OnClick="btnDeleteUser_Click" CssClass="btn btn-danger" />
            <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click" CausesValidation="false" CssClass="btn btn-default" />
        </div>
    </div>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li id="tab-item-details"><a href="#details" class="active" data-toggle="tab" id="tabDetails" runat="server">Details</a></li>
        <li id="tab-item-associations"><a href="#associations" data-toggle="tab" id="tabAssociations" runat="server">Associations</a></li>
        <li id="tab-item-provinces"><a href="#provinces" data-toggle="tab" id="tabProvinces" runat="server">Provinces</a></li>
        <li id="tab-item-catalogues"><a href="#catalogues" data-toggle="tab" id="tabCatalogues" runat="server">Catalogues</a></li>
    </ul>
    <div class="tab-content" style="padding: 15px">
        <!-- Detatils-->
        <div class="tab-pane fade" id="details">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#userdetails">User Details</a>
                    </h4>
                </div>
                <div id="userdetails" class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-3">
                                        <label class="required">First Name: </label>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:TextBox ID="txtFirstName" runat="server"
                                            CssClass="form-control" MaxLength="100"
                                            TabIndex="7" required="required"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvFirstName" runat="server"
                                            ControlToValidate="txtFirstName" Display="Dynamic"
                                            ErrorMessage="You must enter a first name."></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-3">
                                        <label class="required">Last Name: </label>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:TextBox ID="txtLastName" runat="server"
                                            required="required" CssClass="form-control"
                                            MaxLength="100" TabIndex="7"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                            Display="Dynamic" ErrorMessage="You must enter a last name."></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-xs-12 " runat="server" id="genderField">
                                    <div class="col-xs-3">
                                        <label class="required">Gender:</label>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:DropDownList ID="ddlGender" runat="server"
                                            required="required" Width="200px" CssClass="CtrlsLeftAlign" TabIndex="1">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-xs-12">
                                    <div class="col-xs-3">
                                        <label class="required">Email: </label>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:TextBox ID="txtEmailAddress" runat="server" Type="Email"
                                            required="required" CssClass="form-control" AutoCompleteType="Email"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmailAddress"
                                            Display="Dynamic" ErrorMessage="You must enter a valid email address."></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-xs-12" id="passwordField" runat="server">
                                    <div class="col-xs-3">
                                        <label class="input-form-label required">Password</label>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:TextBox ID="txtUserPassword" CssClass="form-control login-form-control"
                                            runat="server" required="required" MinLength="6" MaxLength="20" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUserPassword"
                                            runat="server" ControlToValidate="txtUserPassword" Display="Dynamic"
                                            ErrorMessage="You must enter a valid password."></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-3">
                                        <label>Active:</label>
                                    </div>
                                    <div class="col-xs-9">
                                        <asp:CheckBox ID="chkActive" runat="server" CssClass="Ctrls" />
                                    </div>
                                </div>

                                <div class="col-xs-12">
                                    <div class="col-xs-3">
                                        <div class="JournalDetail" style="padding-right: 10px; text-align: left; vertical-align: top;">
                                            <label>Highlight fore colour:</label>
                                        </div>
                                    </div>
                                    <div class="col-xs-9">
                                        <div class="JournalDetail" style="padding-right: 10px; text-align: left; vertical-align: top;">
                                            <asp:DropDownList ID="ddlHighlightForeColour" runat="server" CssClass="form-control" TabIndex="1">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-xs-12">
                                    <div class="col-xs-3">
                                        <div class="JournalDetail" style="padding-right: 10px; text-align: left; vertical-align: top;">
                                            <label>Highlight back colour:</label>
                                        </div>
                                    </div>
                                    <div class="col-xs-9">
                                        <div class="JournalDetail" style="padding-right: 10px; text-align: left; vertical-align: top;">
                                            <asp:DropDownList ID="ddlHighlightBackColour" runat="server"
                                                CssClass="CtrlsLeftAlign form-control" TabIndex="1">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--End row--->
                    </div>
                </div>
            </div>
            <!--End panel User Details-->
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#journalsdetails">Journals Details</a>
                    </h4>
                </div>
                <div id="journalsdetails" class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkShowOtherMyJournals" runat="server" CssClass="Ctrls" />
                                    </div>

                                    <div class="col-xs-11">
                                        <label>Can view other users 'My Journals'</label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkShowOtherMyOrders" runat="server" CssClass="Ctrls" />
                                    </div>
                                    <div class="col-xs-11">
                                        <label>Can view other users 'My Orders/Quotes'</label>
                                    </div>

                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkDefaultToPersonalJournals" runat="server" CssClass="Ctrls" />
                                    </div>

                                    <div class="col-xs-11">
                                        <label>Default to personal journals</label>
                                    </div>

                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkDefaultToPersonalOrders" runat="server" CssClass="Ctrls" />
                                    </div>

                                    <div class="col-xs-11">
                                        <label>Default to personal orders</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--End row-->
                    </div>
                </div>
            </div>
            <!--End panel--->

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#shipmentsdetails">Shipments Details</a>
                    </h4>
                </div>
                <div id="shipmentsdetails" class="panel-collapse collapse">
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkDefaultToPersonalShipments" runat="server" CssClass="Ctrls" />
                                    </div>
                                    <div class="col-xs-11">
                                        <label>Default to personal shipments</label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkShowOtherMyShipments" runat="server" CssClass="Ctrls" />
                                    </div>

                                    <div class="col-xs-11">
                                        <label>Can view other users 'My Shipments'</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-6">

                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkCanEditShipments" runat="server" CssClass="Ctrls" />
                                    </div>

                                    <div class="col-xs-11">
                                        <label>Can create/edit shipments</label>
                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <div class="col-xs-1">
                                        <asp:CheckBox ID="chkCanDeleteShipments" runat="server" CssClass="Ctrls" />
                                    </div>
                                    <div class="col-xs-11">
                                        <label>Can delete shipments</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" href="#customersdetails">Customers Details</a>
                </h4>
            </div>
            <div id="customersdetails" class="panel-collapse collapse">
            <div class="panel-body">
                <div class="row row-margin">
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanOnlyViewOwnCustomers" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can only view own customers and unassigned customers</label>
                            </div>
                        </div>

                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanOnlyViewAssignedProvinces" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can only view customers in assigned provinces</label>
                            </div>
                        </div>

                    </div>
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanOnlyViewAssignedAssociations" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can only view customers with assigned associations</label>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanOnlyViewAssignedCatalogs" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can only view/select from assigned catalogs</label>
                            </div>
                        </div>
                    </div>
                </div>
                <!--End row-->

                <div class="row row-margin">
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanDeleteCustomers" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can delete customers</label>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanViewCustomerAssociations" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can view associations on customer screen</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanMergeCustomers" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can merge customers</label>
                            </div>
                        </div>
                    </div>
                </div>
                <!--End row-->
            </div>
            </div>
        </div>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" href="#ordersdetails">Orders Details</a>
                </h4>
            </div>
            <div id="ordersdetails" class="panel-collapse collapse">
            <div class="panel-body">
                <div class="row row-margin">
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanEditOrders" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can create/edit orders and quotes</label>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanEmailPrintExportOrders" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can email/print/export orders and quotes</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanDeleteOrders" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can delete orders and quotes</label>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanApproveOrders" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can approve orders and quotes</label>
                            </div>
                        </div>
                    </div>
                </div>
                <!--End row-->
            </div>
            </div>
        </div>

        <div class="panel panel-primary">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" href="#creditmemos">Credit Memos</a>
                </h4>
            </div>
            <div id="creditmemos" class="panel-collapse collapse">
            <div class="panel-body">
                <div class="row row-margin">
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanEditCreditMemos" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can create/edit credit memos</label>
                            </div>
                        </div>

                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanEmailPrintExportShipments" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can email/print/export shipments</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanDeleteCreditMemos" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can delete credit memos</label>
                            </div>
                        </div>

                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanEmailPrintExportCreditMemos" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can email/print/export credit memos</label>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanSendNewsletters" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can send newsletters</label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </div>

        <div class="panel panel-primary">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" href="#settingsdetails">Settings Details</a>
                </h4>
            </div>
            <div id="settingsdetails" class="panel-collapse collapse">
            <div class="panel-body">
                <div class="row row-margin">
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkShowSettings" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can view settings</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanCreateLabels" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can create labels</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <div class="col-xs-1">
                                <asp:CheckBox ID="chkCanViewMyAccountActivity" runat="server" CssClass="Ctrls" />
                            </div>
                            <div class="col-xs-11">
                                <label>Can view my account activity</label>
                            </div>
                        </div>
                    </div>
                </div>
                <!--End row-->
            </div>
            </div>
        </div>
    </div>
    <!--End Details-->

    <!--Associations--->
    <div class="tab-pane fade" id="associations">
        <div class="row row-margin">
            <div class="col-xs-3">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblEmpAssc" runat="server" Font-Bold="True" Text="Assigned Associations:"></asp:Label>
                        <br />
                        <asp:ListBox ID="lbEmpAssc" runat="server" CssClass="Ctrls" Height="250px"
                            Width="225px" TabIndex="36"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-xs-2 action-margin">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Button ID="cmdAddAssc" runat="server" Text="  <<  " CausesValidation="False"
                            OnClick="cmdAddAssc_Click" Width="50px" CssClass="Ctrls btn btn-success" TabIndex="35" /><br />
                        <br />
                        <asp:Button ID="cmdRemAssc" runat="server" Text="  >>  " CausesValidation="False"
                            OnClick="cmdRemAssc_Click" Width="50px" CssClass="Ctrls  btn btn-danger" TabIndex="37" /><br />
                        <br />
                        <asp:Button ID="cmdSaveAssc" runat="server" OnClick="cmdSaveAssc_Click" Text="Save"
                            CssClass="Ctrls btn btn-primary" Width="50px" TabIndex="38" />
                    </div>
                </div>
            </div>

            <div class="col-xs-3">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblAssociations" runat="server" Font-Bold="True" Text="Available Associations:"></asp:Label><br />
                        <asp:ListBox ID="lbAssociations" runat="server" CssClass="Ctrls" Height="250px"
                            Width="225px" TabIndex="34"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--End Associations--->

    <!--Provinces-->
    <div class="tab-pane fade" id="provinces">
        <div class="row row-margin">
            <div class="col-xs-3">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblAssignedProvinces" runat="server" Font-Bold="True" Text="Assigned Provinces:"></asp:Label>
                        <br />
                        <asp:ListBox ID="lbEmpProvinces" runat="server" CssClass="Ctrls" Height="250px"
                            Width="225px" TabIndex="36"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-xs-2 action-margin">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Button ID="cmdAddProv" runat="server" Text="  <<  "
                            CausesValidation="False" OnClick="cmdAddProv_Click" Width="50px"
                            CssClass="Ctrls btn btn-success" TabIndex="35" /><br />
                        <br />
                        <asp:Button ID="cmdRemProv" runat="server" Text="  >>  "
                            CausesValidation="False" OnClick="cmdRemProv_Click" Width="50px"
                            CssClass="Ctrls btn btn-danger" TabIndex="37" /><br />
                        <br />
                        <asp:Button ID="cmdSaveProv" runat="server"
                            OnClick="cmdSaveProv_Click" Text="Save" Width="50px" CssClass="Ctrls btn btn-primary" TabIndex="38" />
                    </div>
                </div>
            </div>
            <div class="col-xs-3">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblAvailableProvinces" runat="server" Font-Bold="True" Text="Available Provinces:"></asp:Label><br />
                        <asp:ListBox ID="lbProvinces" runat="server" CssClass="Ctrls" Height="250px"
                            Width="225px" TabIndex="34"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--End Provinces-->

    <!--End catalogues--->
    <div class="tab-pane fade" id="catalogues">
        <div class="row row-margin">
            <div class="col-xs-3">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblAssignedCatalogs" runat="server" Font-Bold="True" Text="Assigned Catalogues:"></asp:Label>
                        <br />
                        <asp:ListBox ID="lbEmpCatalogs" runat="server" CssClass="Ctrls" Height="250px"
                            Width="225px" TabIndex="36"></asp:ListBox>
                    </div>
                </div>
            </div>
            <div class="col-xs-2 action-margin">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Button ID="cmdAddCat" runat="server" Text="  <<  "
                            CausesValidation="False" OnClick="cmdAddCat_Click" Width="50px"
                            CssClass="Ctrls btn btn-success" TabIndex="35" /><br />
                        <br />
                        <asp:Button ID="cmdRemCat" runat="server" Text="  >>  "
                            CausesValidation="False" OnClick="cmdRemCat_Click" Width="50px"
                            CssClass="Ctrls btn btn-danger" TabIndex="37" /><br />
                        <br />
                        <asp:Button ID="cmdSaveCat" runat="server" OnClick="cmdSaveCat_Click" Text="Save"
                            Width="50px" CssClass="Ctrls btn btn-primary" CausesValidation="false" TabIndex="38" />
                    </div>
                </div>
            </div>

            <div class="col-xs-3">
                <div class="col-xs-12">
                    <div class="PageMainText col-xs-12">
                        <asp:Label ID="lblAvailableCatalogs" runat="server" Font-Bold="True"
                            Text="Available Catalogues:"></asp:Label><br />
                        <asp:ListBox ID="lbCatalogs" runat="server" CssClass="Ctrls" Height="250px"
                            Width="225px" TabIndex="34"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--End Catalogues-->
    </div>
    <asp:TextBox ID="txtActiveTab" runat="server" CssClass="form-control" Type="hidden"></asp:TextBox>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">

    <script src="../js/extra.js"></script>
</asp:Content>

