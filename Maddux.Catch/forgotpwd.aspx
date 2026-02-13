<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgotpwd.aspx.cs" Inherits="Maddux.Catch.forgotpwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maddux | Forgot Password</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>

    <link rel="stylesheet" href="css/bootstrap.min.css"/>
    <link rel="stylesheet" href="css/bootstrap-theme.min.css"/>
    <link rel="stylesheet" href="css/maddux.css"/>

    <script type="text/javascript" src="js/jquery-2.2.3.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
</head>
<body>
    <div class="container" >  
        <div class="col-lg-4 col-md-3 col-sm-2"></div>
        <div class="col-lg-4 col-md-6 col-sm-8">
            <div class="login-logo">
                <img src="img/LoginLogo.png"  alt="Redbud" /> 
            </div>
            <form id="frmRequestPwd" runat="server">
                <div class="row loginbox">                    
                    <div class="col-lg-12">
                        <span class="singtext" >Forgot your password?</span>   
                    </div>
                    <asp:Panel runat="server" ID="pnlPwdSent" CssClass="col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="true">
                        <div class="alert alert-success" role="alert">
                            Your password has been sent.
                        </div>
                    </asp:Panel>
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:TextBox ID="txtEmailAddress" CssClass="form-control login-form-control" required placeholder="Please enter your email address" runat="server"></asp:TextBox>
                        <asp:Label id="lblInvalidUserName" runat="server" CssClass="error_small" Visible="False" Text="We can't find that email address.  Please try again."></asp:Label>
                        <asp:Label id="lblInactiveUser" runat="server" CssClass="error_small" Visible="False" Text="This email account is not currently active.  Please try a different email address or contact the site administrator."></asp:Label>
                    </div>
       
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:Button CssClass="btn submitButton" ID="cmdLogin" runat="server" Text="Send Password" />
                    </div>                     
                </div>
            </form>
            <div class="row forGotPassword">
                <a href="login.aspx" >Back to Login </a> 
            </div>
        </div>
        <div class="col-lg-4 col-md-3 col-sm-2"></div>
    </div>
</body>
</html>
