<%@ Page Title="Change Password | Redbud" Language="C#" MasterPageFile="~/Authorized.Master" AutoEventWireup="true" CodeBehind="changepassword.aspx.cs" Inherits="Maddux.Pitch.changepassword" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        .form-control {
            background-color: #F0F0F0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row mt-5 p-4 align-items-center bg-white border border-dark">
        <div class="col-12">
            <div class="row pt-5 p-4 align-items-center">
                <div class="col">
                    <span class="ms-400 fs-25">Change Password</span>
                </div>
            </div>
            <div class="row ms-300 fs-20">
                <asp:Panel runat="server" ID="pnlPasswordChanged" CssClass="col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="false">
                    <div class="alert alert-success d-flex justify-content-between align-items-center fade show" role="alert">
                        Your password has been successfully changed.
                        <a class="close" data-bs-dismiss="alert" aria-label="close">
                            <i class="far fa-times-circle" aria-hidden="true"></i>
                        </a>
                    </div>
                </asp:Panel>
            </div>
            <div class="row p-4 ms-300 fs-20">
                <div class="col-12">
                    <div class="row">
                        <div class="col-sm-4">
                            <label for="<%#txtOriginalPassword.ClientID%>" class="ms-300 fs-20">Original Password</label>
                            <asp:TextBox ID="txtOriginalPassword" CssClass="form-control login-form-control" runat="server" MaxLength="20" TextMode="Password" required></asp:TextBox>
                            <asp:Label ID="lblEnterOriginalPwd" runat="server" CssClass="text-danger fs-16 ms-200" Text="Please enter the original password." Visible="False"></asp:Label>
                            <asp:Label ID="lblInvalidPassword" runat="server" CssClass="text-danger fs-16 ms-200" Text="Invalid password (passwords are case-sensitive)." Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <label for="<%#txtPassword.ClientID%>" class="ms-300 fs-20">New Password</label>
                            <asp:TextBox ID="txtPassword" CssClass="form-control login-form-control" runat="server" MaxLength="20" TextMode="Password" required></asp:TextBox>
                            <asp:Label ID="lblEnterNewPwd" runat="server" CssClass="text-danger fs-16 ms-200" Text="Please enter a new password." Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <label for="<%#txtConfirmPassword.ClientID%>" class="ms-300 fs-20">Confirm New Password</label>
                            <asp:TextBox ID="txtConfirmPassword" CssClass="form-control login-form-control" runat="server" MaxLength="20" TextMode="Password" required></asp:TextBox>
                        </div>
                    </div>
                    <div class="row my-5">
                        <div class="col-sm-4">
                            <asp:Button ID="cmdChangePassword" runat="server" Text="Change Password" CssClass="btn btn-dark text-wrap w-100" Style="padding-left: 30px; padding-right: 30px;" OnClick="cmdChangePassword_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">
        var txtPassword = document.getElementById("<%#txtPassword.ClientID%>")
            , txtConfirmPassword = document.getElementById("<%#txtConfirmPassword.ClientID%>")
            , txtOriginalPassword = document.getElementById("<%#txtOriginalPassword.ClientID%>");

        var cmdChangePwd = document.getElementById("<%#cmdChangePassword.ClientID%>");

        function validatePassword() {
            if (txtOriginalPassword.value.length == 0) {
                txtOriginalPassword.setCustomValidity("Please enter your original password");
            }
            else {
                txtOriginalPassword.setCustomValidity('');

                if (txtPassword.value.length < 5) {
                    txtPassword.setCustomValidity("Password must be at least 5 characters.");
                }
                else {
                    txtPassword.setCustomValidity('');

                    if (txtPassword.value != txtConfirmPassword.value) {
                        txtConfirmPassword.setCustomValidity("The passwords don't match.");
                    } else {
                        txtConfirmPassword.setCustomValidity('');
                    }
                }
            }
        }

        txtOriginalPassword.onchange = validatePassword;
        txtPassword.onchange = validatePassword;
        txtConfirmPassword.onkeyup = validatePassword;
        cmdChangePwd.onclick = validatePassword;

        $(document).ready(function () {
            $pnl = $("#<%#pnlPasswordChanged.ClientID %>");
            $pnl.delay(3000).fadeOut(500);
        });
    </script>

</asp:Content>
