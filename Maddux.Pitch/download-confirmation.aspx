<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="download-confirmation.aspx.cs" Inherits="Maddux.Pitch.download_confirmation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download invoice</title>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/jquery-1.9.1.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/moment.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/utils.js") %>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal Text="" ID="litMessage" runat="server" />
        <iframe name="confirmationIFrame" id="packingSlipIFrame" style="display:none" runat="server"></iframe>
        <div>
        </div>
        <script>
            $(document).ready(function () {
                //window.close();
            })
        </script>
    </form>
</body>
</html>
