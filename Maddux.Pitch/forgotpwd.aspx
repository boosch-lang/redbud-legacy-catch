<%@ Page Title="" Language="C#" MasterPageFile="~/Unauthorized.master" AutoEventWireup="true" CodeBehind="forgotpwd.aspx.cs" Inherits="Maddux.Pitch.forgotpwd" %>

<asp:Content ID="formContent" ContentPlaceHolderID="cphForm" runat="server">
    <div class="row justify-content-center p-0 p-md-5 my-0 my-md-2 my-lg-3">
        <div class="col-auto">
            <div class="row justify-content-center p-5 pt-3">
                <form id="frmLogin" runat="server">
                    <div class="row mb-4">
                        <div class="col-12 text-center">
                            <asp:Label ID="lblSignIn" runat="server" CssClass="ms-400 fs-30" Text="Forgot your password?"></asp:Label>
                        </div>
                    </div>

                    <asp:Panel runat="server" ID="pnlPwdSent" CssClass="col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="false">
                        <div class="alert alert-success" role="alert">
                            Your password has been sent.
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlPwdSentError" CssClass="col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="true">
                        <div class="alert alert-danger" role="alert">
                            An error occurred when sending your password.
                        </div>
                    </asp:Panel>

                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtEmailAddress" CssClass="ms-250 fs-20 form-control login-form-control bg-white" type="email" required pattern=".*[^ ].*" placeholder="Please enter your email address" runat="server"></asp:TextBox>
                            <asp:Label ID="lblInvalidUserName" runat="server" CssClass="ms-250 fs-20 text-danger" Visible="False" Text="We can't find that email address.  Please try again."></asp:Label>
                            <asp:Label ID="lblInactiveUser" runat="server" CssClass="ms-250 fs-20 text-danger" Visible="False" Text="This email account is not currently active.  Please try a different email address or contact the site administrator."></asp:Label>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center py-3">
                            <asp:Button CssClass="btn btn-block btn-dark w-100 ms-400 fs-20 text-wrap" ID="cmdLogin" runat="server" Text="Send Password" />
                        </div>
                    </div>
                </form>

                <div class="row justify-content-between py-5">
                    <div class="col-12 col-md-6 order-2 order-md-1 text-center py-3">
                        <a href="login.aspx" class="btn btn-block border-dark w-100 ms-400 fs-20">Back to Home</a>
                    </div>
                    <div class="col-12 col-md-6 order-2 order-md-1 text-center py-3">
                        <a href="register.aspx" class="btn btn-block border-dark w-100 ms-400 fs-20">Request Account</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
