<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="shipdate.aspx.cs" Inherits="Maddux.Catch.products.shipdate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/font-awesome.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/AdminLTE.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/skins/skin-red.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/bootstrap-datetimepicker.min.css") %>" />

</head>
<body>

    <form id="frmShipdate" runat="server">
        <asp:Literal runat="server" ID="litMessage"></asp:Literal>
        <div class="modal-body">
            <div class="row">
                <div class="col-xs-12">
                    <label class="font-weight-bold">Ship Date:</label>
                    <div class='input-group datepicker'>
                        <asp:TextBox ID="txtShipDate" data-date-format="MMMM DD, YYYY" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="fa fa-calendar"></span>
                        </span>
                    </div>
                    <asp:RequiredFieldValidator CssClass="text-danger"
                        ID="rfvShipDate" runat="server"
                        ControlToValidate="txtShipDate" Display="Dynamic"
                        ErrorMessage="You must enter a ship date."></asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-12">
                    <label class="font-weight-bold">Order Deadline:</label>
                    <div class='input-group datepicker'>
                        <asp:TextBox ID="txtOrderDeadline" data-date-format="MMMM DD, YYYY" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="fa fa-calendar"></span>
                        </span>
                    </div>
                    <asp:RequiredFieldValidator CssClass="text-danger"
                        ID="rfvDeadlineDate" runat="server"
                        ControlToValidate="txtOrderDeadline" Display="Dynamic"
                        ErrorMessage="You must enter an order deadline date."></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button ID="saveAndClose" CssClass="btn btn-primary" runat="server" OnClick="saveAndClose_Click" Text="Save and Close" />
                    <asp:Button ID="delete" CssClass="btn btn-danger" runat="server" OnClick="delete_Click" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this ship date?')" />
                    <button type="button" class="btn btn-default" onclick="Close()">Close</button>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-2.2.3.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/app.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/date.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/scripts/moment-with-locales.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/scripts/bootstrap-datetimepicker.min.js") %>"></script>

    <script type="text/javascript">
        function ConfirmAction(Message) {
            if (confirm(Message) == true)
                return true;
            else
                return false;
        }

        function Close() {
            window.parent.CloseModal(window.frameElement);
        }

        $(document).ready(function () {
            $('.datepicker').datetimepicker({
                format: 'MMMM dd, yyyy'
            });
        });
    </script>
</body>
</html>
