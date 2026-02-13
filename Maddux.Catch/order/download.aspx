<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="download.aspx.cs" Inherits="Maddux.Catch.order.download" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/jquery-2.2.3.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/moment.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/utils.js") %>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdnShipmentID" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnCustomerID" ClientIDMode="Static" />
        <iframe name="packingSlipIFrame" id="packingSlipIFrame" style="display:none" runat="server"></iframe>
        <div>
        </div>
        <script>
           <%-- $(document).ready(function () {
                var shipmentID = $("#hdnShipmentID").val();
                var customerID = $("#hdnCustomerID").val();
                location = `<%= ResolveUrl(Request.Url.Scheme+"://"+Request.Url.Authority) %>/shipping/shipmentdetail.aspx?id=${shipmentID}&CustomerId=${customerID}`;
            })--%>
        </script>
    </form>
    
</body>
</html>
