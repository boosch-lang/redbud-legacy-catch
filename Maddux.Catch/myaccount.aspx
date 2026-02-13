<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="myaccount.aspx.cs" Inherits="Maddux.Catch.myaccount" %>
<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row">
        <asp:Panel runat="server" ID="pnlPasswordChanged" CssClass="col-lg-12 col-md-12 col-sm-12 top-buffer" Visible="false">
            <div class="alert alert-success" role="alert">
                Your password has been successfully changed.
            </div>
        </asp:Panel>
    </div>

    <div class = "panel panel-primary">
        <div class = "panel-heading">
            <h3 class = "panel-title">
                Account Information
            </h3>
        </div>
        <div class = "panel-body">
            <div class="row">
                <div class="col-sm-4" >
                    <asp:Label ID="lblName" runat="server">Name:  </asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4" >
                    <asp:Label ID="lblEmail" runat="server">Email:  </asp:Label>
                </div>
            </div>
        </div>
    </div>

    <div class = "panel panel-primary top-buffer">
        <div class = "panel-heading">
            <h3 class = "panel-title">
                Change Password
            </h3>
        </div>
        <div class = "panel-body">
            <div class="row">
                <div class="col-md-4" >
                    <label for="<%=txtOriginalPassword.ClientID%>" class="input-form-label">Original Password</label>
                    <asp:TextBox ID="txtOriginalPassword" CssClass="form-control login-form-control" runat="server" required MaxLength="20" TextMode="Password"></asp:TextBox>
                    <asp:Label ID="lblInvalidPassword" runat="server" CssClass="text-danger" Text="Invalid password (passwords are case-sensitive)." Visible="False"></asp:Label>            
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" >
                    <label for="<%=txtPassword.ClientID%>" class="input-form-label">New Password</label>
                    <asp:TextBox ID="txtPassword" CssClass="form-control login-form-control" runat="server" required MaxLength="20" TextMode="Password"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" >
                    <label for="<%=txtConfirmPassword.ClientID%>" class="input-form-label">Confirm New Password</label>
                    <asp:TextBox ID="txtConfirmPassword" CssClass="form-control login-form-control" runat="server" required MaxLength="20" TextMode="Password"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" >
                    <asp:Button ID="cmdChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary" style="padding-left:30px;padding-right:30px;" OnClick="cmdChangePassword_Click" />                
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">
        var txtPassword = document.getElementById("<%=txtPassword.ClientID%>")
          , txtConfirmPassword = document.getElementById("<%=txtConfirmPassword.ClientID%>");

        function validatePassword() {
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

        txtPassword.onchange = validatePassword;
        txtConfirmPassword.onkeyup = validatePassword;
    </script>
</asp:Content>
