<%@ Page Title="" Language="C#" MasterPageFile="~/Unauthorized.master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Maddux.Pitch.login" %>



<asp:Content ID="formContent" ContentPlaceHolderID="cphForm" runat="server">
    <style>
        .image-container {
            position: relative;
            display: inline-block;
        }

            .image-container::before {
                content: "";
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                border-top-left-radius: 50px;
                border-bottom-right-radius: 50px;
                z-index: 1;
            }


        .landing-page-rack {
            max-width: 310.52px;
        }

        /* On screens that are 577px or less, set the background color to blue */
        @media screen and (max-width: 577px) {
            .landing-page-rack {
                max-width: 200px;
            }
        }

        .banner-icon {
            max-height: 100px;
        }
        @media (max-width: 575px) {
            .banner-icon {
                max-height: 63px;
            }
        }
    </style>

    <div class="row justify-content-center">
        <div class="col-auto">
            <div class="row">
                <div class="col-6 col-lg-3 text-center">
                    <img class="img-fluid mt-5 mb-5 m-lg-5 banner-icon" src="img/landing-banner-icon-1.png" />
                </div>
                <div class="col-6 col-lg-3 text-center">
                    <img class="img-fluid mt-5 mb-5 m-lg-5 banner-icon" src="img/landing-banner-icon-2.png" />
                </div>
                <div class="col-6 col-lg-3 text-center">
                    <img class="img-fluid mt-5 mb-5 m-lg-5 banner-icon" src="img/landing-banner-icon-3.png" />
                </div>
                <div class="col-6 col-lg-3 text-center">
                    <img class="img-fluid mt-5 mb-5 m-lg-5 banner-icon" src="img/landing-banner-icon-4.png" />
                </div>
            </div>
        </div>
    </div>

    <div class="row justify-content-center p-5 my-5" style="background: rgba(134, 164, 146, 1);">
        <div class="col-12 col-sm-10 col-md-8 col-lg-6 col-xl-5">
            <form id="frmMain" runat="server">
                <div class="row py-3">
                    <div class="col-12 text-center">
                        <asp:Label ID="lblSignIn" runat="server" CssClass="ms-400 fs-30 text-white" Text="Sign In to Shop"></asp:Label>
                    </div>
                </div>
                <div class="row py-3">
                    <div class="col-12 text-center">
                        <asp:TextBox ID="txtEmailAddress" CssClass="ms-250 fs-20 form-control login-form-control bt-white mb-1" required placeholder="Please enter your email address" runat="server"></asp:TextBox>
                        <asp:Label ID="lblInvalidUserName" runat="server" CssClass="ms-250 fs-20 text-danger ms-300 fs-18" Visible="False" Text="We can't find that email address.  Please try again."></asp:Label>
                        <asp:Label ID="lblInactiveUser" runat="server" CssClass="ms-250 fs-20 ms-250 fs-20text-danger" Visible="False" Text="This email account is not currently active.  Please try a different email address or contact the site administrator."></asp:Label>
                    </div>
                </div>

                <div class="row py-3">
                    <div class="col-12 text-center">
                        <asp:TextBox ID="txtPassword" CssClass="ms-250 fs-20 form-control login-form-control bg-white mb-1" required TextMode="Password" placeholder="Please enter your password" runat="server"></asp:TextBox>
                        <asp:Label ID="lblInvalidPassword" runat="server" CssClass="ms-250 fs-20 text-danger" Text="Invalid password (passwords are case-sensitive)." Visible="False"></asp:Label>
                    </div>
                </div>

                <div class="row py-1">
                    <div class="col-12">
                        <asp:CheckBox ID="chkRememberMe" runat="server" />
                        <asp:Literal ID="litRememberMeLabel" runat="server" Text="<label for='chkRememberMe' class='ps-2 ms-250 fs-20'>Remember Me</label>" />
                    </div>
                </div>

                <asp:Literal Text="" ID="litMessage" runat="server" />

                <div class="row justify-content-between">
                    <div class="col-12 col-md-6 order-2 order-md-1 text-center  py-3">
                        <a href="forgotpwd.aspx" class="btn btn-block btn-dark w-100 ms-400 fs-20">Forgot Password</a>
                    </div>
                    <div class="col-12 col-md-6 order-1 order-md-2 text-center py-3">
                        <asp:Button CssClass="btn  btn-block btn-dark w-100 loginButton ms-400 fs-20" ID="cmdLogin" runat="server" Text="Login" />
                    </div>
                </div>

                <div class="row py-5">
                    <div class="col-12 text-center">
                        <a href="register.aspx" class="btn btn-block btn-dark w-100 ms-400 fs-20">Request Account</a>
                    </div>
                </div>
            </form>

        </div>
    </div>

    <div class="pt-5 pb-3">
        <div class="row ms-400 fs-30 justify-content-center pb-1">
            <div class="col-auto text-center">
                We Have Been Serving Retailers Across Canada With 
            </div>
        </div>

        <div class="row ms-300 fs-30 justify-content-center pt-1">
            <div class="col-auto text-center">
                High-Quality Horticultural Products Since 2007.
            </div>
        </div>
    </div>

    <div class="row justify-content-center py-5 mb-5">
        <div class="col-auto">
            <div class="image-container">
                <div style="aspect-ratio: 520 / 257.94; max-width: 520px;">
                    <img class="img-fluid" src="img/login_hero.png" />
                </div>
            </div>
        </div>
    </div>

    <div class="row text-white justify-content-center py-5 px-0 px-sm-1 px-sm-3 px-lg-5" style="background: rgba(17, 49, 76, 1);">
        <div class="col-12 col-md-10 p-5">
            <div class="row pb-4">
                <div class="col ms-400 fs-30 text-center">
                    Plant a Little Passion
                </div>
            </div>
            <div class="row py-2">
                <div class="col ms-300 fs-20 text-center lh-28">
                    Redbud is a family-owned and operated business dedicated to growing people, plants, and businesses. With years of experience, we source, develop, and distribute a wide range of high-quality, innovative horticultural products for Canadian retailers.
                </div>
            </div>
            <div class="row py-2">
                <div class="col ms-300 fs-20 text-center lh-28">
                    We build strong relationships with our suppliers to consistently offer the best products to our customers. Our knowledgeable team understands the challenges in both the horticultural and retail industries.
                </div>
            </div>
            <div class="row py-2">
                <div class="col ms-300 fs-20 text-center lh-28">
                    Our goal is to help you grow your business by simplifying your buying and selling processes. We offer regular shipping cycles, and our well-presented product displays combine optimal product mixes with innovative products and marketing concepts.
                </div>
            </div>
        </div>
    </div>



    <div class="row bg-white py-5">
        <div class="col-12 py-5">
            <div class="row justify-content-around">
                <div class="col-12 col-sm-3 py-3">
                    <div class="row justify-content-center justify-content-sm-end">
                        <div class="col-auto">
                            <div class="landing-page-rack">
                                <img src="img/landing-rack-1.png" class="img-fluid" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-sm-3 py-3">
                    <div class="row justify-content-center">
                        <div class="col-auto">
                            <div class="landing-page-rack">
                                <img src="img/landing-rack-2.png" class="img-fluid" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-sm-3 py-3">
                    <div class="row justify-content-center justify-content-sm-start">
                        <div class="col-auto">
                            <div class="landing-page-rack">
                                <img src="img/landing-rack-3.png" class="img-fluid" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row justify-content-center">
        <div class="p-0">
            <div style="aspect-ratio: 1440 / 377; overflow: hidden">
                <img src="img/landing-footer.png" class="img-fluid w-100" />
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function scrollToLoginForm() {
            var loginForm = document.getElementById('<%= lblSignIn.ClientID %>');
            if (loginForm) {
                loginForm.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        }


    </script>


</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
