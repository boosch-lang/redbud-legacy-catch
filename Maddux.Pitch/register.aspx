<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Unauthorized.master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="Maddux.Pitch.register" %>

<asp:Content ID="formContent" ContentPlaceHolderID="cphForm" runat="server">

    <div class="row justify-content-center p-0 p-md-5 my-3">
        <div class="col-12 col-sm-12 col-md-10 col-lg-6 col-xl-6">
            <div class="row justify-content-center p-5 pt-3">
                <form id="frmRegister" runat="server">
                    <div class="row">
                        <div class="col-12 text-center">
                            <asp:Label ID="lblSignIn" runat="server" CssClass="ms-250 fs-30" Text="Account Request"></asp:Label>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:Literal runat="server" ID="litMessage"></asp:Literal>
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="pnlSuccess" CssClass="col-xl-12 col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="false">
                        <div class="alert alert-success" role="alert">
                            Your request has been submitted.  We will contact you once we have processed your request.
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlFailure" CssClass="col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="false">
                        <div class="alert alert-danger" role="alert">
                            An error occurred sending your registration request.  Please try again.
                        </div>
                    </asp:Panel>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtCompanyName" CssClass="form-control login-form-control bg-white" required placeholder="Company Name" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtContact" CssClass="form-control login-form-control bg-white" required placeholder="Your Name" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtEmailAddress" CssClass="form-control login-form-control bg-white" required placeholder="Email Address" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtAddress" CssClass="form-control login-form-control bg-white" placeholder="Address" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtCity" CssClass="form-control login-form-control bg-white" placeholder="City" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:DropDownList ID="cboProvince" CssClass="form-control login-form-control bg-white" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtPostalCode" CssClass="form-control login-form-control bg-white" placeholder="Postal Code" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtPhone" CssClass="form-control login-form-control bg-white" placeholder="Phone #" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:TextBox ID="txtComments" CssClass="form-control login-form-control bg-white" placeholder="Comments" runat="server" TextMode="MultiLine" Width="100%" Height="200px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row py-3">
                        <div class="col-12 text-center">
                            <asp:Button CssClass="btn btn-block btn-dark w-100  ms-400 fs-20" ID="cmdSubmit" runat="server" Text="Request an Account" />
                        </div>
                    </div>
                    <div class="row py-5">
                        <div class="col-12 text-center">
                            <a href="login.aspx" class="btn btn-block border-dark w-100 ms-400 fs-20">Back to Home</a>
                        </div>
                    </div>
                    <asp:HiddenField ClientIDMode="Static" ID="hidRecaptchaToken" runat="server" />
                </form>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" src="https://www.google.com/recaptcha/api.js?render=6Lfaf2QpAAAAAMt980bBF1QzQltCfugXtVX_R-5A"></script>
    <script>
        (() => {

            const form = document.getElementById("frmRegister");
            const tokenInput = document.getElementById("hidRecaptchaToken");
            const action = 'register';
            let recaptchaToken = null;

            form.addEventListener("submit", async (evt) => {
                if (recaptchaToken == null) {
                    evt.preventDefault();
                }
                recaptchaToken = await new Promise((resolve) => {
                    grecaptcha.ready(() => grecaptcha.execute('6Lfaf2QpAAAAAMt980bBF1QzQltCfugXtVX_R-5A', { action }).then(resolve));
                });
                tokenInput.value = recaptchaToken;

                form.submit();
            });
        })();
    </script>

</asp:Content>
