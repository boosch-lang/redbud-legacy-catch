<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="mailinglabels.aspx.cs" Inherits="Maddux.Catch.mailing.mailinglabels" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row">
        <div class="col-xs-12">
            <asp:Panel ID="panelCustomer" runat="server" GroupingText="" Font-Bold="True">
                <div class="row">

                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <label class="font-weight-bold">Select associations to include</label>
                        </div>
                        <div class="col-xs-12">
                            <asp:ListBox ID="lbAssociations" DataTextField="Text" DataValueField="Value"
                                runat="server" CssClass="form-control" Height="250px"
                                TabIndex="36" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                        <div style="padding-top: 10px" class="col-xs-12 text-center">
                            <asp:Button ID="btnSelectAllAsscs" runat="server" CssClass="btn btn-success" Text="Select All" OnClick="btnSelectAllAsscs_Click" />
                            <asp:Button ID="btnSelectNoAssc" runat="server" CssClass="btn btn-danger" Text="Select None" OnClick="btnSelectNoAssc_Click" />
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="col-xs-12">
                            <label class="font-weight-bold">Select province(s) to include:</label>
                        </div>
                        <div class="col-xs-12">
                            <asp:ListBox ID="lbProvinces" runat="server" DataTextField="Text"
                                DataValueField="Value" CssClass="form-control" Height="250px"
                                SelectionMode="Multiple"></asp:ListBox>
                        </div>
                        <div style="padding-top: 10px" class="col-xs-12 text-center">
                            <asp:Button ID="btnSelectAllProv" runat="server" CssClass="btn btn-success" Text="Select All" OnClick="btnSelectAllProv_Click" />
                            <asp:Button ID="btnSelectNoProv" runat="server" CssClass="btn btn-danger" Text="Select None" OnClick="btnSelectNoProv_Click" />
                        </div>

                    </div>
                </div>
                <div style="margin-top:10px!important;margin-bottom:0px!important" class="panel panel-primary">
                    <div class="panel-heading">
                        <a class="panel-title" data-toggle="collapse" href="#followupDetails">Followup Details
                        </a>
                    </div>
                    <div id="followupDetails" class="panel-collapse collapse in" aria-expanded="true">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-4">
                                    <div class="col-xs-12">
                                        <asp:CheckBox ID="chkCreateJournals" runat="server" Text="Auto-create followup journals" AutoPostBack="True" OnCheckedChanged="chkCreateJournals_CheckedChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-6">
                                    <asp:Label ID="lblFollowupDate" runat="server" Text="Followup Date" Enabled="False"></asp:Label>
                                    <asp:TextBox type="date" ID="txtFollowupDate" runat="server" CssClass="form-control" Enabled="false" />

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <asp:Label ID="lblFollowupDetails" runat="server" Text="Followup Details" Enabled="False"></asp:Label>
                                    <asp:TextBox ID="txtJournalNotes" runat="server" CssClass="form-control" Rows="15"
                                        MaxLength="4000" TabIndex="5" TextMode="MultiLine" ToolTip="Notes entered into this textbox will appear on printed material sent to customers or suppliers."
                                        Enabled="False"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div style="padding-top: 10px" class="col-xs-12">
                        <asp:Button ID="cmdCreateCSV" runat="server" Text="Create CSV File" OnClick="cmdCreateCSV_Click" OnClientClick="return ValidateForm();" CssClass="btn btn-primary" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>

</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript">

        function ValidateForm() {
            var i, iSelCount, iSelExclCount;
            var asscListBox, asscExcludeListBox, provListBox;
            var cjCheckbox;
            var _strAsscList, _strAsscExcludeList, _strProvList;
            var _strMessage;

            iSelCount = 0;
            iSelExclCount = 0;

            asscListBox = $$("lbAssociations");
            asscExcludeListBox = $$("lbAssociationsExclude");
            provListBox = $$("lbProvinces");
            cjCheckbox = $$("chkCreateJournals");

            _strAsscList = "";
            _strAsscExcludeList = "";
            _strProvList = "";

            if (asscExcludeListBox != null) {
                $.each(asscExcludeListBox.find('option:selected'), function (i, val) {
                    _strAsscExcludeList += val.innerText + ', ';
                });
            }
            else {
                alert("Could not locate the associations to exclude list box on the form.  Contact your System Administrator.");
                return false;
            }

            if (provListBox != null) {
                $.each(provListBox.find('option:selected'), function (i, val) {
                    _strProvList += val.innerText + ', ';
                });
            }
            else {
                alert("Could not locate the provinces list box on the form.  Contact your System Administrator.");
                return false;
            }

            if (asscListBox != null) {
                $.each(asscListBox.find('option:selected'), function (i, val) {
                    _strAsscList += val.innerText + ', ';
                });

                if (_strAsscList == null || _strAsscList.length == 0) {
                    alert("You must select at least one association to include in the newsletter mailing.");
                    return false;
                }
                else {
                    if (cjCheckbox != null) {
                        _strMessage = "You are about to create a mailing list with the following options:\n\n";
                        _strMessage += "Associations to include:\n" + _strAsscList.substring(0, _strAsscList.length - 2);

                        if (_strAsscExcludeList != "") {
                            _strMessage += "\n\nAssociations to exclude:\n" + _strAsscExcludeList.substring(0, _strAsscExcludeList.length - 2);
                        }
                        else {
                            _strMessage += "\n\nAssociations to exclude:\nNone.";
                        }

                        if (_strProvList != "") {
                            _strMessage += "\n\nProvinces to include:\n" + _strProvList.substring(0, _strProvList.length - 2);
                        }
                        else {
                            _strMessage += "\n\nProvinces to include:\nAll provinces.";
                        }
                        _strMessage += "\n\nAre you sure you wish to proceed?";

                        if (confirm(_strMessage)) {
                            if (cjCheckbox.checked == true) {
                                if (confirm("Are you sure you wish to create follow up journals after generating the mailing list?")) {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        alert("Could not locate the journal checkbox on the form.  Contact your System Administrator.");
                        return false;
                    }
                }
            }
            else {
                alert("Could not locate associations list box on the form.  Contact your System Administrator.");
                return false;
            }
        }
    </script>
</asp:Content>
