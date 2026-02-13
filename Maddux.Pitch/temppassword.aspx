<%@ Page Language="C#" Title="Set Password | Redbud" MasterPageFile="~/Login.master" AutoEventWireup="true" CodeBehind="temppassword.aspx.cs" Inherits="Maddux.Pitch.temppassword" %>

<asp:Content ID="formContent" ContentPlaceHolderID="cphForm" runat="server">
    <form id="frmResetPassword" runat="server" autocomplete="off">
        <div class="row loginbox">
            <div class="col-xl-12" style="text-align: center">
                <span class="singtext">Reset Password</span>
            </div>
            <asp:Panel runat="server" ID="pnlPasswordChanged" CssClass="col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="false">
                    <div class="alert alert-success" role="alert">
                        <asp:Literal Text="" id="litSuccessMessage" runat="server" />                        
                    </div>
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                        <div class="col-sm-4">
                            <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="btn btn-primary" Style="padding-left: 30px; padding-right: 30px;" OnClick="btnContinue_Click" />
                        </div>
                    </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlForm">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 my-3">
                    You have logged in with a temporary password.  Please set a new password. 
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                    <label for="<%=txtPassword.ClientID%>" class="input-form-label">New Password</label>
                    <asp:TextBox ID="txtPassword" CssClass="form-control login-form-control" runat="server" MaxLength="20" type="Password" autocomplete="off"></asp:TextBox>
                    <asp:Label ID="lblEnterNewPwd" runat="server" CssClass="error_small" Text="Please enter a new password." Visible="False"></asp:Label>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                    <label for="<%=txtConfirmPassword.ClientID%>" class="input-form-label">Confirm New Password</label>
                    <asp:TextBox ID="txtConfirmPassword" CssClass="form-control login-form-control" runat="server" MaxLength="20" type="password" autocomplete="newpassword"></asp:TextBox>
                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                    <asp:Label ID="lblInvalidPassword" runat="server" CssClass="error_small" Text="Passwords do not match" Visible="False"></asp:Label>

                </div>
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 mt-3" style="padding: 10px 0px;text-align: center;">
                    <asp:Button ID="btnNo" runat="server" Text="Continue with Temporary Password" CssClass="btn btn-secondary" Style="padding-left: 30px; padding-right: 30px;" OnClick="btnNo_Click"/>
                    <asp:Button ID="cmdChangePassword" runat="server" Text="Set New Password" CssClass="btn btn-primary" Style="padding-left: 30px; padding-right: 30px;" OnClick="cmdChangePassword_Click" />
                </div>
            </asp:Panel>
        </div>
        <asp:HiddenField ClientIDMode="Static" ID="hidRecaptchaToken" runat="server" />
    </form>

</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $pnl = $("#<%=pnlPasswordChanged.ClientID %>");
            $pnl.delay(3000).fadeOut(500);
        });
    </script>

</asp:Content>
