<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Maddux.Catch.error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maddux.Catch | Error</title>
</head>
<body>
    <form id="frmError" runat="server">
        <div>
            <h2>An Application Error Occurred</h2>
            <asp:Literal ID="litError" runat="server"></asp:Literal>
            <asp:Panel ID="pnlInnerError" runat="server" Visible="false">
                <p>
                    Inner Error Message:<br />
                    <asp:Label ID="lblInnerMessage" runat="server" Font-Bold="true" Font-Size="Large" /><br />
                </p>
                <pre>
        <asp:Label ID="lblInnerTrace" runat="server" />
      </pre>
            </asp:Panel>
            <p>
                Error Message:<br />
                <asp:Label ID="lblExceptionMessage" runat="server" Font-Bold="true"
                    Font-Size="Large" />
            </p>
            <pre>
      <asp:Label ID="lblExceptionTrace" runat="server" Visible="false" />
    </pre>
            <br />
            Return to your <a href='default.aspx'>Default Page</a>
        </div>
    </form>
</body>
</html>
