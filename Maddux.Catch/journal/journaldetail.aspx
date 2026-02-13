<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="journaldetail.aspx.cs" Inherits="Maddux.Catch.journal.journaldetail" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/font-awesome.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/AdminLTE.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/skins/skin-red.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Content/bootstrap-datetimepicker.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap-multiselect.css") %>" type="text/css" />
    <script type="text/javascript">
        function ConfirmAction(Message) {
            if (confirm(Message) == true) {
                return true;
            } else {
                return false;
            }
        }

        function Close() {
            window.parent.CloseModal(window.frameElement);
        }

        function addDateStamp() {
            var user = $("#hdnUserId").val();
            $(".textNotes").val(function (i, text) {
                return text + "\n ** " + user + " - " + formatDate(new Date(), "NNN dd, yyyy hh:mm a") + "**\n";
            });

            $(".textNotes").focus();

            return false;
        }

        function addLeftMessage() {
            var user = $("#hdnUserId").val();
            $(".textNotes").val(function (i, text) {
                return text + "\n ** " + user + " - " + formatDate(new Date(), "NNN dd, yyyy hh:mm a") + " - Called and left a message with reception.**\n"
            });

            $(".textNotes").focus();

            return false;
        }

        function addVoiceMail() {
            var user = $("#hdnUserId").val();
            $(".textNotes").val(function (i, text) {
                return text + "\n ** " + user + " - " + formatDate(new Date(), "NNN dd, yyyy hh:mm a") + " - Called and left a message on voicemail.**\n"
            });

            $(".textNotes").focus();

            return false;
        }
    </script>
    <style>
        button.multiselect.dropdown-toggle.btn.btn-default {
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 0px;
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            color: #555;
            display: block;
            font-size: 14px;
            height: 34px;
            line-height: 1.42857143;
            padding: 6px 12px;
            text-align: left;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            width: 100%;
        }

        .btn-group {
            text-align: left;
            width: 100%;
        }

        b.caret {
            float: right;
            margin-right: -5px;
            margin-top: 8px;
        }
    </style>
</head>
<body>
    <form id="frmJournal" runat="server">
        <asp:Literal runat="server" ID="litMessage"></asp:Literal>
        <div class="modal-body">
            <div class="row">
                <input type="hidden" runat="server" id="hdnUserId" />
                <div class="col-xs-6">
                    <div class="form-group">
                        <label class="control-label">Company:</label>&nbsp;
                    <asp:HyperLink ID="hlCompany" runat="server" />
                    </div>
                    <div class="form-group">
                        <label class="control-label">Star Rating:</label>&nbsp;
                        <asp:Literal ID="litStarRating" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="form-group">
                        <label class="control-label">Contact:</label>&nbsp;
                    <asp:HyperLink ID="hlContact" runat="server" />
                        <asp:Label ID="lblContact" runat="server" />
                    </div>
                </div>
                <div class="col-xs-3">
                    <div class="form-group">
                        <label class="control-label">Phone:</label>&nbsp;
                    <asp:Label ID="lblCustomerPhone" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <div class="form-group">
                        <label class="control-label">Date Created:</label>&nbsp;
                    <asp:Label ID="lblJournalDate" runat="server" />
                    </div>
                </div>
                <div class="col-xs-3 col-xs-offset-3">
                    <div class="form-group">
                        <label class="control-label">Resolved:</label>&nbsp;
                    <asp:CheckBox ID="chkResolved" runat="server" TabIndex="3" sty />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-4">
                    <div class="form-group">
                        <label class="control-label">Campaign:</label>&nbsp;
                    <asp:ListBox ID="ddlCampaign" SelectionMode="Multiple" runat="server" CssClass="form-control multiselectNoSearch" />
                    </div>
                </div>
                <div id="AssignedToDiv" runat="server" class="col-xs-4">
                    <div class="form-group">
                        <label class="control-label">Assigned To:</label>&nbsp;
                    <asp:DropDownList ID="ddSalesPerson" runat="server" CssClass="form-control" TabIndex="4" />
                    </div>
                </div>
                <div id="NextFollowupDiv" runat="server" class="col-xs-4">
                    <div class="form-group">
                        <label class="control-label">Next Followup:</label>&nbsp;
                    <div class="input-group datepicker">
                        <asp:TextBox ID="txtFollowup" runat="server" CssClass="form-control" data-date-format="MMMM DD, YYYY" onkeypress="return false;"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="fa fa-calendar"></span>
                        </span>
                    </div>
                    </div>
                </div>
                <div id="RepeatEveryDiv" runat="server" class="col-xs-12">
                    <div class="form-group">
                        <label class="control-label">Repeat:</label>&nbsp;
                     <asp:RadioButtonList ID="rdblstRepeat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdblstRepeat_SelectedIndexChanged" RepeatLayout="Flow" RepeatDirection="Horizontal">
                         <asp:ListItem Value="week" style="margin: 10px">Every Week</asp:ListItem>
                         <asp:ListItem Value="month" style="margin: 10px">Every Month</asp:ListItem>
                         <asp:ListItem Value="none" style="margin: 10px">Don't Repeat</asp:ListItem>
                     </asp:RadioButtonList>
                    </div>
                </div>
                <div id="RepeatUntilDiv" runat="server" class="col-xs-6">
                    <div class="form-group">
                        <label for="RepeatFromDate" class="control-label">Repeat From:</label>
                        <div class="input-group datepicker">
                            <asp:TextBox ID="txtRepeatFromDate" runat="server" CssClass="form-control" ClientIDMode="Static" data-date-format="MMMM DD, YYYY" onkeypress="return false;"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="fa fa-calendar"></span>
                            </span>
                        </div>
                        <div id="RepeatFromRequired" runat="server" style="display: none" class="text-danger">Repeat from date is required!</div>
                    </div>
                    <div class="form-group">
                        <label for="RepeatUntilDate" class="control-label">Repeat Until:</label>
                        <div class="input-group datepicker">
                            <asp:TextBox ID="txtRepeatUntilDate" runat="server" CssClass="form-control" ClientIDMode="Static" data-date-format="MMMM DD, YYYY" onkeypress="return false;"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="fa fa-calendar"></span>
                            </span>
                        </div>
                        <div id="RepeatUntilRequired" runat="server" style="display: none" class="text-danger">Repeat until date is required!</div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <label class="control-label">Notes:&nbsp;<span class="text-red">*</span></label><br />
                        <asp:TextBox ID="txtNotes" runat="server" Rows="10" TextMode="MultiLine" CssClass="form-control textNotes" TabIndex="1" />
                        <asp:RequiredFieldValidator
                            ID="rfvNotes"
                            runat="server"
                            ControlToValidate="txtNotes"
                            ErrorMessage="You must enter notes to save this journal."
                            Display="Dynamic"
                            CssClass="text-danger" />
                    </div>
                </div>
                <div class="col-xs-12 text-center">
                    <input class="btn btn-default" type="button" value="New Comment" name="btnComment" onclick="addDateStamp()" />
                    <input class="btn btn-default" type="button" value="Voice Mail" name="btnVoice" onclick="addVoiceMail()" />
                    <input class="btn btn-default" type="button" value="Left Message" name="btnMessage" onclick="addLeftMessage()" />
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="saveAndClose" CssClass="btn btn-primary" runat="server" OnClick="SaveAndClose_Click" Text="Save and Close" />
            <asp:Button ID="delete" CssClass="btn btn-danger" runat="server" OnClick="Delete_Click" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this journal?')" />
            <button type="button" class="btn btn-default" onclick="Close()">Close</button>
        </div>
    </form>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-2.2.3.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/app.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/date.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/scripts/moment-with-locales.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/scripts/bootstrap-datetimepicker.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap-multiselect.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datetimepicker({
                format: 'yyyy-mm-dd'
            });

            $('.multiselectNoSearch').multiselect({
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                includeSelectAllOption: false,
                nonSelectedText: '--Select Campaign--'
            });
        });
        function toggleRepeat() {
            //chkEveryWeek chkEveryMonth
            var weeklyCheckbox = $("#chkEveryWeek").find("input[type=checkbox]");
            var monthlyCheckbox = $("#chkEveryMonth").find("input[type=checkbox]");
            if (weeklyCheckbox.is(":checked")) {
                monthlyCheckbox.prop("checked", false);
            }
            if (monthlyCheckbox.is(":checked")) {
                weeklyCheckbox.prop("checked", false);
            }
        }
    </script>
</body>
</html>
