<%@ Page Title="Profile | Redbud" Language="C#" MasterPageFile="~/Authorized.Master" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="Maddux.Pitch.account" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        .form-control {
            background-color: #F0F0F0 !important;
        }

        .btn-black:hover {
            color: white;
        }

        a, a:hover, a:visited {
            color: inherit;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row pt-5 p-4 align-items-center">
        <div class="col">
            <span class="ms-400 fs-25">Profile</span>
        </div>
        <div class="col-auto">
            <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-black" Style="padding-left: 80px; padding-right: 80px;" OnClick="cmdSave_Click" />
        </div>
    </div>

    <div class="row">
        <asp:Panel runat="server" ID="pnlAccountChanged" CssClass="col-lg-12 col-md-12 col-sm-12" Visible="false">
            <div class="alert alert-success d-flex justify-content-between align-items-center" role="alert">
                Your changes have been successfully saved.
                <a class="close" data-bs-dismiss="alert" aria-label="Close">
                    <i class="far fa-times-circle" aria-hidden="true"></i>
                </a>
            </div>
        </asp:Panel>


        <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    </div>


    <div class="row p-4">
        <div class="col-12  ms-250 fs-25">
            * Required Field
        </div>
        <div class="col-12 pt-2  ms-250 fs-25">
            ** Please contact us at <a href="mailto:sales@redbud.com">sales@redbud.com</a> or call us at <a href="tel:+18887332830">1.888.733.2830</a> if you wish to modify this field
        </div>
    </div>

    <div class="row my-5 ms-250 fs-25 bg-white p-4 border border-dark">
        <div class="col-12">
            <div class="row">
                <div class="col-12">
                    <h3 class="ms-400 fs-25">Business / Shipping Address</h3>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtShippingCompany.ClientID%>" class="input-form-label ms-400 fs-20">Company Name**</label>
                    <asp:TextBox ID="txtShippingCompany" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#ddlShippingSalutation.ClientID%>" class="input-form-label ms-400 fs-20">Title</label>
                    <asp:DropDownList ID="ddlShippingSalutation" CssClass="form-control shippingTitle" runat="server"></asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtShippingFirstName.ClientID%>" class="input-form-label ms-400 fs-20">First Name</label>
                    <asp:TextBox ID="txtShippingFirstName" CssClass="form-control shippingFirst" runat="server" MaxLength="50"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtShippingLastName.ClientID%>" class="input-form-label ms-400 fs-20">Last Name</label>
                    <asp:TextBox ID="txtShippingLastName" CssClass="form-control shippingLast" runat="server" MaxLength="50"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <label for="<%#txtShippingAddress.ClientID%>" class="input-form-label ms-400 fs-20">Address</label>
                    <asp:TextBox ID="txtShippingAddress" TextMode="SingleLine" Height="37" CssClass="form-control shippingAddress" runat="server" MaxLength="75" Enabled="false"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtShippingCity.ClientID%>" class="input-form-label ms-400 fs-20">City</label>
                    <asp:TextBox ID="txtShippingCity" CssClass="form-control shippingCity" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#ddlShippingProvince.ClientID%>" class="input-form-label ms-400 fs-20">Province</label>
                    <asp:DropDownList ID="ddlShippingProvince" DataTextField="StateId" DataValueField="StateName" CssClass="form-control shippingProvince" runat="server" Enabled="false"></asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#ddlShippingCountry.ClientID%>" class="input-form-label ms-400 fs-20">Country</label>
                    <asp:DropDownList ID="ddlShippingCountry" CssClass="form-control shippingCountry" DataTextField="CountryCode" DataValueField="CountryName" runat="server" Enabled="false">
                        <asp:ListItem Text="---Select Country---" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtShippingPostalCode.ClientID%>" class="input-form-label ms-400 fs-20">Postal Code</label>
                    <asp:TextBox ID="txtShippingPostalCode" CssClass="form-control shippingPostal" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row my-5 ms-250 fs-25 bg-white p-4 border border-dark">
        <div class="col-12">
            <div class="row align-items-center">
                <div class="col-auto">
                    <h3 class="ms-400 fs-25 mb-0">Billing Address and Contact</h3>
                </div>
                <div class="col">
                    <span class="pull-right">
                        <label for="chkSame">Same as Shipping information?</label>
                        <input type="checkbox" id="chkSame" name="chkSame" disabled="disabled" />
                    </span>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtBillingCompany.ClientID%>" class="input-form-label ms-400 fs-20">Company Name**</label>
                    <asp:TextBox ID="txtBillingCompany" CssClass="form-control billingCompany" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#ddlBillingSalutation.ClientID%>" class="input-form-label ms-400 fs-20">Title</label>
                    <asp:DropDownList ID="ddlBillingSalutation" CssClass="form-control billingTitle" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtBillingFirstName.ClientID%>" class="input-form-label ms-400 fs-20">First Name</label>
                    <asp:TextBox ID="txtBillingFirstName" CssClass="form-control billingFirst" runat="server" MaxLength="50"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtBillingLastName.ClientID%>" class="input-form-label ms-400 fs-20">Last Name</label>
                    <asp:TextBox ID="txtBillingLastName" CssClass="form-control billingLast" runat="server" MaxLength="50"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtBillingPhone.ClientID%>" class="input-form-label ms-400 fs-20">Billing Phone</label>
                    <asp:TextBox CssClass="form-control billingPhone phoneValidator" ID="txtBillingPhone" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtBillingAddress.ClientID%>" class="input-form-label ms-400 fs-20">Address</label>
                    <asp:TextBox ID="txtBillingAddress" TextMode="SingleLine" Height="37" CssClass="form-control billingAddress" runat="server" MaxLength="75" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtBillingCity.ClientID%>" class="input-form-label ms-400 fs-20">City</label>
                    <asp:TextBox ID="txtBillingCity" CssClass="form-control billingCity" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#ddlBillingProvince.ClientID%>" class="input-form-label ms-400 fs-20">Province</label>
                    <asp:DropDownList ID="ddlBillingProvince" DataTextField="StateId" DataValueField="StateName" CssClass="form-control billingProvince" runat="server" Enabled="false"></asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#ddlBillingCountry.ClientID%>" class="input-form-label ms-400 fs-20">Country</label>
                    <asp:DropDownList ID="ddlBillingCountry" DataTextField="CountryName" DataValueField="CountryCode" CssClass="form-control billingCountry" runat="server" Enabled="false"></asp:DropDownList>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtBillingPostalCode.ClientID%>" class="input-form-label ms-400 fs-20">Postal Code</label>
                    <asp:TextBox ID="txtBillingPostalCode" CssClass="form-control billingPostal" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row my-5 ms-250 fs-25 bg-white p-4 border border-dark">
        <div class="col-12">
            <div class="row">
                <div class="col-12">
                    <h3 class="ms-400 fs-25">Phone & Email Contact</h3>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtPhone.ClientID%>" class="input-form-label ms-400 fs-20">Phone</label>
                    <asp:TextBox class="form-control phone phoneValidator" ID="txtPhone" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtEmail.ClientID%>" class="input-form-label ms-400 fs-20">Email*</label>
                    <asp:TextBox CssClass="form-control" type="email" ID="txtEmail" runat="server" Width="100%" required MaxLength="70"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtFax.ClientID%>" class="input-form-label ms-400 fs-20">Fax</label>
                    <asp:TextBox class="form-control phoneValidator" ID="txtFax" runat="server" Width="100%" MaxLength="75"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtAltEmail.ClientID%>" class="input-form-label ms-400 fs-20">Alternate Email</label>
                    <asp:TextBox CssClass="form-control" ID="txtAltEmail" type="email" runat="server" Width="100%" MaxLength="70"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-6">
                    <label for="<%#txtMobile.ClientID%>" class="input-form-label ms-400 fs-20">Mobile</label>
                    <asp:TextBox CssClass="form-control phoneValidator" ID="txtMobile" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="col-12 col-md-6">
                    <label for="<%#txtWebsite.ClientID%>" class="input-form-label ms-400 fs-20">Website</label>
                    <asp:TextBox CssClass="form-control" ID="txtWebsite" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">
        var flgDirty = false;
        $(document).ready(function () {
            $pnl = $("#<%#pnlAccountChanged.ClientID %>");
            $pnl.delay(10000).fadeOut(500);
        });

        window.onbeforeunload = function () {
            if (flgDirty) {
                return 'You have unsaved changes. Are you sure you want to leave this page?';
            }
        };

        window.onload = function () {
            $('input,checkbox,textarea,radio,select').bind('change', function (event) { flgDirty = true })
            $('#<%#cmdSave.ClientID%>').bind('click', function (event) { flgDirty = false })
        }
    </script>
</asp:Content>
