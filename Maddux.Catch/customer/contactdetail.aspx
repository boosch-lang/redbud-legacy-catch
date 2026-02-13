<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contactdetail.aspx.cs" Inherits="Maddux.Catch.customer.contactdetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/font-awesome.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/AdminLTE.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/skins/skin-red.min.css")%>" />

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

    </script>
</head>
<body>
    <form id="frmJournal" runat="server">
        <asp:Literal runat="server" ID="litMessage"></asp:Literal>
        <div class="modal-body">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Panel ID="panelContact" runat="server" GroupingText="&nbsp;Contact Information&nbsp;&nbsp;&nbsp;" Font-Bold="True">
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    First Name:
                                </div>
                                <div class="col-xs-4">
                                    <asp:TextBox ID="txtContactFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="col-xs-2">
                                    Last Name:
                                </div>
                                <div class="col-xs-4">
                                    <asp:TextBox ID="txtContactLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    Position:
                                </div>
                                <div class="col-xs-10">
                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    Phone:
                                </div>
                                <div class="col-xs-7">
                                    <asp:TextBox ID="txtPhone" runat="server" class="form-control phoneValidator"></asp:TextBox>
                                </div>
                                <div class="col-xs-1">
                                    <label>Ext:</label>
                                </div>
                                <div class="col-xs-2">
                                    <asp:TextBox ID="txtExtension" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    Mobile:
                                </div>
                                <div class="col-xs-10">
                                    <asp:TextBox ID="txtMobile" runat="server" class="form-control phoneValidator"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    Fax:
                                </div>
                                <div class="col-xs-10">
                                    <asp:TextBox ID="txtFax" runat="server" class="form-control phoneValidator"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    Pager:
                                </div>
                                <div class="col-xs-10">
                                    <asp:TextBox ID="txtPager" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    Email:
                                </div>
                                <div class="col-xs-10">
                                    <asp:TextBox ID="txtEmail" runat="server" type="email" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-xs-12">
                                <div class="col-xs-2">
                                    Notes:
                                </div>
                                <div class="col-xs-10">
                                    <asp:TextBox ID="txtNotes" runat="server" BorderStyle="None" rows="5" TextMode="MultiLine"
                                        CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="saveAndClose" CssClass="btn btn-primary" runat="server" OnClick="saveAndClose_Click" Text="Save and Close" />
            <asp:Button ID="delete" CssClass="btn btn-danger" runat="server" OnClientClick="return ConfirmAction('Are you sure you want to delete this contact?')" OnClick="delete_Click" Text="Delete" />
            <button ID="close" class="btn btn-default" onclick="Close()">Close</button>

        </div>
    </form>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-2.2.3.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/app.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/date.js") %>"></script>
</body>
</html>
