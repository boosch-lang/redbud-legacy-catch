<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="customerdetail.aspx.cs" Inherits="Maddux.Catch.customer.customerdetail" ValidateRequest="false" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.20/css/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li class="active"><a href="#details" data-toggle="tab">Details</a></li>
        <li><a href="#contacts" data-toggle="tab">Contacts</a></li>
        <li><a href="#orders" data-toggle="tab">Orders</a></li>
        <li><a href="#shipments" data-toggle="tab">Shipments</a></li>
        <li><a href="#credit" data-toggle="tab">Credit Memos</a></li>
        <li><a href="#journals" data-toggle="tab">Journals</a></li>
        <li id="tabAssociations" runat="server"><a href="#associations" data-toggle="tab">Associations</a></li>
        <li><a href="#stores" data-toggle="tab">Related Stores</a></li>
        <li style="float: right">
            <asp:Button Style="color: red; font-size: 24px; font-weight: bold" Text="Pitch" runat="server" ID="btnPitch" OnClick="btnPitch_Click" CssClass="btn btn-link" OnClientClick="target ='_blank';" />
        </li>
    </ul>
    <div class="row">
        <div class="tab-content" style="padding: 15px">
            <div class="tab-pane fade in active" id="details">
                <div class="row">
                    <div class="col-xs-6">
                        <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                        <asp:Button Text="Delete Customer" runat="server" ID="btnDelete" OnClick="btnDelete_Click"
                            OnClientClick="return ConfirmAction('Are you sure you want to delete this customer?')" CssClass="btn btn-danger" />
                        <asp:HyperLink runat="server" ID="lnkMerge" Text="Merge Customer" CssClass="btn btn-primary"
                            title="Merge Customer" data-toggle="modal" data-target="#modalView" title_text="Merge Customer" data-remote="false">
                        </asp:HyperLink>
                    </div>
                    <div class="col-xs-3">
                        <div class="col-xs-6 text-right">
                            <label class="font-weight-bold">Region:</label>
                        </div>
                        <div class="col-xs-6">
                            <asp:Literal ID="litRegion" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="col-xs-6 text-right">
                            <label class="font-weight-bold">Freight Charge:</label>
                        </div>
                        <div class="col-xs-6">
                            <asp:Literal ID="litFreightCharge" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div class="col-xs-12">
                        <br />
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#billingDetails">Shipping / Billing Details</a>
                                    </h4>
                                </div>
                                <div id="billingDetails" class="panel-collapse collapse in" aria-expanded="true">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <%-- Shipping Details --%>
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label class="h4 font-weight-bold">Shipping Details</label>
                                                    </div>
                                                    <div class="col-md-8 text-right">
                                                        <asp:Button Text="Copy Billing Details" CssClass="btn btn-success btn-sm" ID="btnCopyBilling"
                                                            CausesValidation="false" OnClick="btnCopyBilling_Click" runat="server" />
                                                        <asp:HyperLink ID="hlViewShippingAddress" runat="server" CssClass="btn btn-success btn-sm" Target="_blank">View Address On Map</asp:HyperLink>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Company *</label>
                                                        <asp:TextBox ID="txtShippingCompany" runat="server" CssClass="form-control" require="required"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Contact</label>
                                                    </div>
                                                    <div class="col-xs-2">
                                                        <asp:TextBox ID="txtShippingContactPrefix" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-xs-5">
                                                        <asp:TextBox ID="txtShippingContactFirst" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-xs-5">
                                                        <asp:TextBox ID="txtShippingContactLast" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <%-- <div class="col-xs-12">
                                                        <label class="font-weight-bold">Job Title</label>
                                                        <asp:TextBox ID="txtJobTitle" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>--%>
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Address</label>
                                                        <asp:TextBox ID="txtShippingAddress" runat="server" TextMode="MultiLine" Rows="3"
                                                            CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-lg-4">
                                                        <label class="font-weight-bold">City</label>
                                                        <asp:TextBox ID="txtShippingCity" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-lg-4">
                                                        <label class="font-weight-bold">Province</label>
                                                        <asp:DropDownList ID="ddShippingProvince" DataTextField="Text" DataValueField="Value" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-12 col-lg-4">
                                                        <label class="font-weight-bold">Country</label>
                                                        <asp:DropDownList ID="ddCountry" DataTextField="Text" DataValueField="Value" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Postal Code</label>
                                                        <asp:TextBox ID="txtShippingPostal" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <%-- Billing Details --%>
                                                <div class="row" style="margin-bottom: 5px">
                                                    <div class="col-xs-5">
                                                        <label class="h4 font-weight-bold">Billing Details</label>
                                                    </div>
                                                    <div class="col-xs-7 text-right">
                                                        <asp:Button Text="Copy Shipping Details" ID="btnCopyShipping" CausesValidation="false" CssClass="btn btn-success btn-sm" OnClick="btnCopyShipping_Click" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Company*</label>
                                                        <asp:TextBox ID="txtBillingCompany" runat="server" CssClass="form-control" require="required"></asp:TextBox>
                                                    </div>
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Contact</label>
                                                    </div>
                                                    <div class="col-xs-2">
                                                        <asp:TextBox ID="txtBillingContactPrefix" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-xs-5">
                                                        <asp:TextBox ID="txtBillingContactFirst" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-xs-5">
                                                        <asp:TextBox ID="txtBillingContactLast" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <%--<div class="col-xs-12">
                                                        <label class="font-weight-bold">Phone</label>
                                                        <asp:TextBox ID="txtBillingPhone" runat="server" class="form-control phoneValidator"></asp:TextBox>
                                                    </div>--%>
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Address</label>
                                                        <asp:TextBox ID="txtBillingAddress" runat="server" TextMode="MultiLine" Rows="3"
                                                            CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-6 col-md-4">
                                                        <label class="font-weight-bold">City</label>
                                                        <asp:TextBox ID="txtBillingCity" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-6 col-md-4">
                                                        <label class="font-weight-bold">Province</label>
                                                        <asp:DropDownList ID="ddBillingProvince" DataTextField="Text" DataValueField="Value" CssClass="form-control" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-12 col-md-4">
                                                        <label class="font-weight-bold">Country</label>
                                                        <asp:DropDownList ID="ddBillingCountry" DataTextField="Text" DataValueField="Value" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Postal Code</label>
                                                        <asp:TextBox ID="txtBillingPostal" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#contactInformation">Contact Information</a>
                                    </h4>
                                </div>
                                <div id="contactInformation" class="panel-collapse collapse in" aria-expanded="true">
                                    <div class="panel-body">
                                        <asp:PlaceHolder ID="phEmailLinks" runat="server">
                                            <div class="text-right">
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-sm btn-success dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        Send Email&nbsp;<span class="caret"></span>
                                                    </button>
                                                    <ul class="dropdown-menu dropdown-menu-right">
                                                        <asp:Repeater ID="repMailto" runat="server">
                                                            <ItemTemplate>
                                                                <li>
                                                                    <a href="mailto:<%# Container.DataItem.ToString().Trim() %>"><%# Container.DataItem.ToString().Trim() %></a>
                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-xs-8">
                                                        <label class="font-weight-bold">Phone</label>
                                                        <asp:TextBox ID="txtContactPhone" runat="server" class="form-control phoneValidator"></asp:TextBox>
                                                    </div>
                                                    <div class="col-xs-4">
                                                        <label class="font-weight-bold">Ext:</label>
                                                        <asp:TextBox ID="txtPhoneExtension" runat="server" type="number" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label for="txtFax" class="font-weight-bold">Fax</label>
                                                        <asp:TextBox ID="txtFax" ClientIDMode="Static" runat="server" class="form-control phoneValidator"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label for="txtMobile" class="font-weight-bold">Mobile</label>
                                                        <asp:TextBox ID="txtMobile" ClientIDMode="Static" runat="server" class="form-control phoneValidator"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Email</label>
                                                        <asp:TextBox runat="server" ID="txtEmail" type="email" CssClass="form-control"></asp:TextBox>
                                                        <div class="row">
                                                            <div class="col-xs-3">
                                                                <label class="font-weight-bold">Receives Newsletters</label>
                                                            </div>
                                                            <div class="col-xs-9">
                                                                <asp:CheckBox ID="chkEmailRecievesNewsletters" runat="server" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <%--<asp:RequiredFieldValidator ErrorMessage="Email is required!" CssClass="text-danger" ControlToValidate="txtEmail" runat="server" />--%>
                                                    </div>
                                                    <div class="col-xs-12">
                                                        <label for="txtAltEmail" class="font-weight-bold">Alt. Email</label>
                                                        <asp:TextBox runat="server" ID="txtAltEmail" type="email" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>

                                                        <div class="row">
                                                            <div class="col-xs-3">
                                                                <label class="font-weight-bold">Receives Newsletters</label>
                                                            </div>
                                                            <div class="col-xs-9">
                                                                <asp:CheckBox ID="chkAltEmailRecievesNewsletters" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-xs-3">
                                                                <label class="font-weight-bold">Receives Confirmations &amp; Packing Slips</label>
                                                            </div>
                                                            <div class="col-xs-9">
                                                                <asp:CheckBox ID="chkAltEmailReceivesConfirmations" runat="server" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-xs-12">
                                                        <label for="txtWebsite" class="font-weight-bold">Website</label>
                                                        <asp:TextBox runat="server" ID="txtWebsite" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#ParentStore">Related Store(s)</a>
                                    </h4>
                                </div>
                                <div id="ParentStore" class="panel-collapse collapse in" aria-expanded="true">
                                    <div class="panel-body">
                                        <h4>Parent Store(s)</h4>
                                        <asp:GridView ID="gvParent" runat="server"
                                            CssClass="table table-hover table-bordered table-hover dataTable"
                                            GridLines="Horizontal"
                                            AllowPaging="false"
                                            SortMode="Automatic"
                                            AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="true"
                                            ShowFooter="false"
                                            EmptyDataText="This store has no parent stores.">
                                            <Columns>
                                                <asp:BoundField DataField="Company" HeaderText="Company" />
                                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                                <asp:BoundField DataField="City" HeaderText="City" />
                                                <asp:BoundField DataField="State" HeaderText="Province" />
                                                <asp:BoundField DataField="Country" HeaderText="Country" />
                                                <asp:BoundField DataField="Zip" HeaderText="Postal Code" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href="/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerID") %>" target="_blank" class="btn btn-success btn-sm">View Parent</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <h4>Child Store(s)</h4>
                                        <asp:GridView ID="gvChild" runat="server"
                                            CssClass="table table-hover table-bordered table-hover dataTable"
                                            GridLines="Horizontal"
                                            AllowPaging="false"
                                            SortMode="Automatic"
                                            AutoGenerateColumns="False"
                                            ShowHeaderWhenEmpty="true"
                                            ShowFooter="false"
                                            EmptyDataText="This store has no child stores.">
                                            <Columns>
                                                <asp:BoundField DataField="Company" HeaderText="Company" />
                                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                                <asp:BoundField DataField="City" HeaderText="City" />
                                                <asp:BoundField DataField="State" HeaderText="Province" />
                                                <asp:BoundField DataField="Country" HeaderText="Country" />
                                                <asp:BoundField DataField="Zip" HeaderText="Postal Code" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href="/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerID") %>" target="_blank" class="btn btn-success btn-sm">View Child</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#customerSettings" aria-expanded="false" class="collapsed">Customer Settings (Tax and Term settings)</a>
                                    </h4>
                                </div>
                                <div id="customerSettings" class="panel-collapse collapse in" aria-expanded="false">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Default Terms</label>
                                                        <asp:DropDownList runat="server" DataTextField="Text" DataValueField="Value" ID="ddDefaultTerms"
                                                            CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-xs-12">
                                                        <label class="font-weight-bold">Sales Rep</label>
                                                        <asp:DropDownList runat="server" ID="ddSalesRep" DataTextField="Text" DataValueField="Value"
                                                            CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <label class="font-weight-bold">Vendor Number</label>
                                                            <asp:TextBox ID="txtVendorNumber" CssClass="form-control login-form-control" TextMode="SingleLine"
                                                                runat="server" MinLength="6" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="" id="passwordField" runat="server">
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <label id="lblPassword" runat="server" class="font-weight-bold"></label>
                                                            <asp:TextBox ID="txtUserPassword" CssClass="form-control login-form-control" TextMode="SingleLine"
                                                                runat="server" MinLength="6" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-xs-2">
                                                        <label class="font-weight-bold">Active</label>
                                                    </div>
                                                    <div class="col-xs-10">
                                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#invoicingSettings">Invoicing Settings</a>
                                    </h4>
                                </div>
                                <div id="invoicingSettings" class="panel-collapse collapse in" aria-expanded="true">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-xs-3">
                                                        <label for="chkEmailInvoice" class="font-weight-bold">Email Invoices</label>
                                                    </div>
                                                    <div class="col-xs-9">
                                                        <asp:CheckBox ID="chkEmailInvoice" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <label for="txtInvoiceEmail" class="font-weight-bold">Invoice Email</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtInvoiceEmail" ClientIDMode="Static" type="email" CssClass="form-control" runat="server" />
                                                            <span class="input-group-btn">
                                                                <asp:HyperLink ID="hlInvoiceEmailMailto" runat="server" CssClass="btn btn-default"><i class="fa fa-envelope" aria-hidden="true"></i></asp:HyperLink>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-xs-2">
                                                        <label class="font-weight-bold">Print Invoices</label>
                                                    </div>
                                                    <div class="col-xs-10">
                                                        <asp:CheckBox ID="chkPrintInvoices" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-2">
                                                        <label class="font-weight-bold">Invoice with Shipment</label>
                                                    </div>
                                                    <div class="col-xs-10">
                                                        <asp:CheckBox ID="chkInvoiceShipment" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            <div class="row">
                <div class="col-xs-12">
                    <label for="txtNotes" class="font-weight-bold">Notes</label>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:TextBox ID="txtNotes" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" id="contacts">
            <div class="row">
                <div class="col-xs-12">
                    <a href="/customer/contactDetail.aspx?customerID=<%= CustomerID %>" class="btn btn-primary"
                        title="New Contact" data-toggle="modal" data-target="#modalView" title_text="New Contact" data-remote="false">
                        <i class="fa fa-plus"></i>&nbsp;New Contact
                    </a>
                </div>
            </div>
            <asp:GridView ID="dgvContacts" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no contacts to display.">
                <Columns>

                    <asp:TemplateField>
                        <HeaderTemplate>Name</HeaderTemplate>
                        <ItemTemplate>
                            <a target="_blank" href="/customer/contactdetail.aspx?contactid=<%# Eval("ContactID") %>&customerId=<%= CustomerID %>"
                                title="Contact Details" data-toggle="modal" data-target="#modalView" data-remote="false"
                                title_text="<%# Eval("FullName") %>">

                                <%# Eval("FullName") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Position" HeaderText="Position/Title" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:BoundField DataField="CellPhone" HeaderText="Cell" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="tab-pane fade" id="orders">
            <div class="row">
                <div class="col-xs-12">
                    <a id="lnkNewOrder" target="_blank" runat="server" class="btn btn-primary">
                        <i class="fa fa-plus"></i>&nbsp;New Order
                    </a>
                </div>
            </div>
            <asp:GridView ID="dgvOrders" runat="server"
                AllowPaging="false"
                AllowSorting="true"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal"
                AutoGenerateColumns="False"
                ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no orders to display.">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1"
                        ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1">
                        <HeaderTemplate>Order #</HeaderTemplate>
                        <ItemTemplate>
                            <a target="_blank" href='/order/orderdetail.aspx?id=<%# Eval("OrderID") %>'><%# Eval("OrderID") %></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="OrderDate" HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Order Date" />
                    <asp:BoundField DataField="Catalogues" HeaderStyle-CssClass="col-4" ItemStyle-CssClass="col-4" HeaderText="Catalogue(s)" />
                    <asp:BoundField DataField="RackName" HeaderStyle-CssClass="col-4" ItemStyle-CssClass="col-4" HeaderText="Rack" />
                    <asp:BoundField DataField="RequestedShipDate" HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Req. Ship Date" />
                    <asp:BoundField DataField="DiscountedSubTotal" HeaderStyle-CssClass="col-1 text-right" ItemStyle-CssClass="col-1" DataFormatString="{0:C}" HeaderText="Total">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataRowStyle CssClass="grdNoData" />
                <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
            </asp:GridView>

        </div>
        <div class="tab-pane fade" id="shipments">

            <asp:GridView ID="dgvShipments" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal" AllowPaging="false" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no shipments to display.">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1"
                        ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1">
                        <HeaderTemplate>Shipment #</HeaderTemplate>
                        <ItemTemplate>
                            <a target="_blank" href="/shipping/shipmentdetail.aspx?id=<%# Eval("ShipmentID") %>&customerId=<%= CustomerID %>">

                                <%# Eval("ShipmentID") %></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1"
                        ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-1">
                        <HeaderTemplate>Order #</HeaderTemplate>
                        <ItemTemplate>
                            <a target="_blank" href='/order/orderdetail.aspx?id=<%# Eval("OrderID") %>'><%# Eval("OrderID") %></a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Catalog" HeaderStyle-CssClass="col-5" ItemStyle-CssClass="col-5" HeaderText="Catalogues" />
                    <asp:BoundField DataField="RackName" HeaderStyle-CssClass="col-4" ItemStyle-CssClass="col-4" HeaderText="Rack" />
                    <asp:BoundField DataField="DateShipped" HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Date Shipped" />
                    <asp:BoundField DataField="ShipVia" HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" HeaderText="Ship Via" />
                    <asp:BoundField DataField="TrackingNumber" HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" HeaderText="Tracking #" />
                    <asp:BoundField DataField="Total" HeaderStyle-CssClass="col-1 text-right" ItemStyle-CssClass="col-1" HeaderText="Total">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="tab-pane fade" id="credit">
            <div class="row">
                <div class="col-xs-12">
                    <asp:HyperLink ID="lnkCreditMemo" Target="_blank" runat="server" CssClass="btn btn-primary">
                            <i class="fa fa-plus"></i>&nbsp;New Credit Memo
                    </asp:HyperLink>
                </div>
            </div>
            <asp:GridView ID="dgvCreditMemos" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no credit memos to display.">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1"
                        ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1">
                        <HeaderTemplate>Credit Memo #</HeaderTemplate>
                        <ItemTemplate>
                            <a target="_blank" href="/credit/creditDetail.aspx?CreditID=<%# Eval("CreditID") %>&customerId=<%= CustomerID %>">

                                <%# Eval("CreditID") %></a>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CreateDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderText="Date" />
                    <asp:BoundField DataField="CreditNotes" HeaderText="Notes" />
                    <asp:BoundField DataField="SubTotal" HeaderText="Subtotal" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="tab-pane fade" id="journals">
            <div class="row">
                <div class="col-xs-12">
                    <a href="/journal/journaldetail.aspx?customerID=<%= CustomerID %>" class="btn btn-primary" title_text="New Journal"
                        data-toggle="modal" data-target="#modalView" data-remote="false">
                        <i class="fa fa-plus"></i>&nbsp;New Journal
                    </a>
                </div>
            </div>
            <asp:GridView ID="dgvJournals" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal"
                AllowPaging="false"
                AllowSorting="true"
                SortMode="Automatic"
                AutoGenerateColumns="False"
                ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no journals to display.">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1"
                        ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1">
                        <HeaderTemplate>Journal</HeaderTemplate>
                        <ItemTemplate>
                            <a target="_blank" href="/journal/journaldetail.aspx?id=<%# Eval("JournalID") %>" title="Journal Details"
                                data-toggle="modal" data-target="#modalView" data-remote="false" title_text="Journal #<%# Eval("JournalID") %>">

                                <%# Eval("JournalID") %></a>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="DateStamp" HeaderStyle-CssClass="col-xs-1" DataFormatString="{0:MMMM dd, yyyy}"
                        ItemStyle-CssClass="col-xs-1" HeaderText="Date Stamp" />
                    <asp:BoundField DataField="FollowUpDate" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="col-xs-1"
                        ItemStyle-CssClass="col-xs-1" HeaderText="Followup" />

                    <asp:TemplateField>
                        <HeaderTemplate>Notes</HeaderTemplate>
                        <ItemTemplate>
                            <div>
                                <%# ((string)Eval("Notes")).Replace(System.Environment.NewLine, "<br />") %>
                            </div>
                            <br />
                            <br />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1"
                        ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1">
                        <HeaderTemplate>Resolved</HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Enabled="false" Checked='<%#Eval("IsResolved") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="tab-pane fade" id="associations">
            <asp:ScriptManager runat="server" ID="scriptManager1"></asp:ScriptManager>
            <asp:UpdatePanel runat="server" ID="updatePanel1">
                <ContentTemplate>
                    <div class="row text-center">
                        <div class="col-md-5">
                            <label>Assigned Associations</label>
                            <asp:ListBox ID="lstCurrentAssociations" DataValueField="Value" DataTextField="Text" Rows="10"
                                CssClass="form-control" runat="server" />
                        </div>
                        <div class="col-md-2 action-margin">
                            <div>
                                <asp:Button Text="<<" CssClass="btn btn-success" OnClick="btnAssociate_Click" ID="btnAssociate"
                                    runat="server" />
                            </div>
                            <div>
                                <asp:Button Text=">>" CssClass="btn btn-danger" OnClick="btnUnassociate_Click" ID="btnUnassociate"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="col-md-5">
                            <label>Available Associations</label>
                            <asp:ListBox ID="lstAllAssociations" CssClass="form-control" DataValueField="Value" Rows="10"
                                DataTextField="Text" runat="server"></asp:ListBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <label>Active Catalogues</label>
                        </div>
                        <div class="col-xs-12">
                            <asp:ListBox ID="lstPrograms" DataTextField="Text" DataValueField="Value" CssClass="form-control" runat="server" Rows="25"></asp:ListBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="tab-pane fade" id="stores">
            <div class="text-right">
                <asp:Button ID="btnSaveTop" runat="server" CssClass="btn btn-sm btn-primary" Text="Save" OnClick="btnSaveCustomers_Click" />
            </div>
            <asp:HiddenField ID="hfCheckedIDs" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfUncheckedIDs" runat="server" ClientIDMode="Static" />
            <br />
            <asp:GridView ID="gvCustomers" ClientIDMode="Static" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal" AllowPaging="false" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                ShowFooter="false"
                EmptyDataText="There are no shipments to display." DataKeyNames="CustomerId" OnDataBound="gvCustomers_DataBound" OnRowDataBound="gvCustomers_RowDataBound">
                <EmptyDataTemplate>
                    <div class="alert alert-info" role="alert">
                        There are no customers to display.
                    </div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-BorderWidth="0">
                        <ItemTemplate>
                            <input type="checkbox" id="cbSelected" runat="server" class="relation-checkbox" value='<%# Eval("CustomerId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" HeaderStyle-BorderWidth="0">
                        <ItemTemplate>
                            <%# Eval("Company") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-BorderWidth="0">
                        <ItemTemplate>
                            <a href="/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId") %>" target="_blank" class="btn btn-sm btn-success">View Customer</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="text-right">
                <asp:Button ID="btnSaveBottom" runat="server" CssClass="btn btn-sm btn-primary" Text="Save" OnClick="btnSaveCustomers_Click" />
            </div>
        </div>
    </div>
    </div>
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
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
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
    <script src="//cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.20/js/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript">
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

        function ConfirmAction(Message) {
            if (confirm(Message) == true)
                return true;
            else
                return false;
        }

        function CloseModal(frameElement) {
            if (frameElement) {
                var dialog = $(frameElement).closest(".modal");
                if (dialog.length > 0) {
                    dialog.modal("hide");
                }
            }
        }

        $(document).ready(function () {
            $('#gvCustomers').DataTable();
        });

        $('.relation-checkbox').change(function () {
            var checkedIDs = $('#hfCheckedIDs').val().split(",");
            var uncheckedIDs = $('#hfUncheckedIDs').val().split(",");
            var id = $(this).val();

            if (this.checked) {
                checkedIDs.push(id);

                if ($.inArray(id, uncheckedIDs) > -1) {
                    uncheckedIDs = $.grep(uncheckedIDs, function (value) {
                        return value != id;
                    });
                }
            }
            else {
                uncheckedIDs.push(id);
                if ($.inArray(id, checkedIDs) > -1) {
                    checkedIDs = $.grep(checkedIDs, function (value) {
                        return value != id;
                    });
                }
            }
            $('#hfCheckedIDs').val((checkedIDs.filter(function (v) { return v !== '' })).join());
            $('#hfUncheckedIDs').val((uncheckedIDs.filter(function (v) { return v !== '' })).join());
            console.log(id);
            console.log(checkedIDs);
            console.log(uncheckedIDs);
        });
    </script>
</asp:Content>
