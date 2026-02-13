<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="newsletters.aspx.cs" Inherits="Maddux.Catch.mailing.newsletters" ValidateRequest="false" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <script type="text/javascript">
        function openModal() {
            $('#modalView').modal('show');
        }
    </script>
    <style>
        .attachmentControl:last-of-type .btn-remove {
            display: none;
        }
    </style>
    <style type="text/css">
        @font-face {
            font-family: 'Museo-700';
            src: url('http://www.redbud.com/font/180647_0.eot');
            src: url('http://www.redbud.com/font/180647_0.eot?#iefix') format('embedded-opentype'),url('http://www.redbud.com/font/180647_0.woff') format('woff'),url('http://www.redbud.com/font/180647_0.ttf') format('truetype');
        }

        @font-face {
            font-family: 'Museo-300';
            src: url('http://www.redbud.com/font/180647_1.eot');
            src: url('http://www.redbud.com/font/180647_1.eot?#iefix') format('embedded-opentype'),url('http://www.redbud.com/font/180647_1.woff') format('woff'),url('http://www.redbud.com/font/180647_1.ttf') format('truetype');
        }

        @font-face {
            font-family: 'Museo-500';
            src: url('http://www.redbud.com/font/180647_2.eot');
            src: url('http://www.redbud.com/font/180647_2.eot?#iefix') format('embedded-opentype'),url('http://www.redbud.com/font/180647_2.woff') format('woff'),url('http://www.redbud.com/font/180647_2.ttf') format('truetype');
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row">
        <div class="col-xs-12">
            <asp:Literal ID="litError" runat="server" />
            <asp:HiddenField ID="hfGuid" runat="server" />
        </div>
        <asp:Panel ID="pnlForm" runat="server">
            <div class="col-xs-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">To</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <div class="row row-margin">
                            <div class="col-xs-6 ">
                                <label>Select associations to include</label>
                                <asp:ListBox ID="lbAssociations" ClientIDMode="static" DataTextField="Text" DataValueField="Value"
                                    runat="server" CssClass="form-control" Height="250px"
                                    TabIndex="36" SelectionMode="Multiple"></asp:ListBox>
                                <div class="text-center">
                                    <div class="btn btn-success" onclick="$('#lbAssociations option').prop('selected', true);">Select All</div>
                                    <div class="btn btn-danger" onclick="$('#lbAssociations option').prop('selected', false);">Select None</div>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <label>Select region(s) to include:</label>
                                <asp:ListBox ID="lbRegion" runat="server" ClientIDMode="Static" DataTextField="Text"
                                    DataValueField="Value" CssClass="form-control" Height="250px"
                                    SelectionMode="Multiple"></asp:ListBox>
                                <div class="text-center">
                                    <div class="btn btn-success" onclick="$('#lbRegion option').prop('selected', true);">Select All</div>
                                    <div class="btn btn-danger" onclick="$('#lbRegion option').prop('selected', false);">Select None</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">From</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:TextBox ID="txtFrom" runat="server" required="required" CssClass="form-control" AutoCompleteType="Email">sales@redbud.com</asp:TextBox>
                        <asp:RequiredFieldValidator ID="frvFrom" ControlToValidate="txtFrom" CssClass="text-danger" runat="server" Display="Dynamic" ErrorMessage="You must enter a valid email address." />
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">Subject</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSubject" ControlToValidate="txtSubject" CssClass="text-danger" runat="server" Display="Dynamic" ErrorMessage="You must enter a subject." />
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">Attachments</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:Repeater ID="repUploadedFiles" runat="server">
                            <ItemTemplate>
                                <div>
                                    <a target="_blank" href='/uploads/temp/<%# hfGuid.Value %>/<%# Eval("Name") %>' style="display: inline-block; width: 200px;"><%# Eval("Name") %></a>
                                    <asp:LinkButton ID="btnFileDelete" CommandArgument='<%# Eval("Name") %>' CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this file?');" OnClick="btnFileDelete_Click" runat="server"><i class="btn-remove fa fa-times-circle fa-lg text-danger" title="Remove file" style="cursor: pointer; padding-left: 3px;"></i></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div id="hello">
                            <div class="attachmentControl">
                                <asp:FileUpload ID="txtAttach" ClientIDMode="static" AllowMultiple="false" runat="server" CssClass="fuAttach" Style="display: inline-block; width: 200px;" /><i class="btn-remove fa fa-times-circle fa-lg text-danger" title="Remove file" style="cursor: pointer; padding-left: 5px;"></i>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">Message</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control tinyMCE" Rows="20" TabIndex="5" TextMode="MultiLine" ToolTip="This will form the body of the newsletter.  You can include HTML characters in your newsletter body."></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div style="text-align: center;">
                            <asp:Button runat="server" ID="btnPreview" CssClass="btn btn-primary" OnClick="btnPreview_Click" OnClientClick="javascript: return ValidateForm()" Text="Preview Newsletter" />
                        </div>
                    </div>
                </div>
        </asp:Panel>
        <asp:Panel ID="pnlPreview" runat="server" Visible="false">
            <div class="col-xs-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">Preview</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:Literal ID="litEmailPreview" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">Attachments</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:Repeater ID="repAttachments" runat="server">
                            <ItemTemplate>
                                <div>
                                    &bull; <a target="_blank" href='/uploads/temp/<%# hfGuid.Value %>/<%# Eval("Name") %>'><%# Eval("Name") %></a>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:PlaceHolder ID="phNoAttachments" runat="server" Visible="false">
                            <p>
                                No Attachments
                            </p>
                        </asp:PlaceHolder>
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse">Recipients</a>
                        </h4>
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gvRecipients"
                            runat="server"
                            AllowPaging="false"
                            AllowSorting="true"
                            SortMode="Automatic"
                            CssClass="table table-hover table-bordered table-hover dataTable"
                            GridLines="Horizontal"
                            AutoGenerateColumns="False"
                            ShowHeaderWhenEmpty="true"
                            ShowFooter="false"
                            EnableModelValidation="True"
                            EmptyDataText="There are no reciepients to display.">
                            <Columns>
                                <asp:BoundField DataField="Company" HeaderText="Company">
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="State" HeaderText="Province">
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Email" HeaderText="Email">
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AltEmail" HeaderText="Alternate Email">
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
                <br />
                <div style="text-align: center;">
                    <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" OnClick="btnCancel_Click" Text="Cancel" />
                    <asp:Button ID="cmdSendNewsletter" runat="server" OnClick="cmdSendNewsletter_Click" Text="Send Newsletter" OnClientClick="return confirm('Are you sure you want to send this email to all valid recipients?');" CssClass="btn btn-primary" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlResult" runat="server" Visible="false">
                <div class="col-xs-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <span data-toggle="collapse">
                                    Recipients
                                </span>
                            </h4>
                        </div>
                        <div class="panel-collapse collapse in" aria-expanded="true">
                            <div class="panel-body">
                                <div class="row">
                            <div class="col-xs-4">
                                Successfully Sent Newsletters:
                                <h3 style="margin-top: 0;">
                                    <asp:Literal runat="server" ID="litSuccessfullySent"></asp:Literal>
                                </h3>
                            </div>
                            <div class="col-xs-4">
                                Failed to Send Newsletters:
                                <h3 style="margin-top: 0;">
                                    <asp:Literal runat="server" ID="litFailedToSend"></asp:Literal>
                                </h3>
                            </div>
                        </div>


                                <asp:GridView ID="gvResults"
                                    runat="server"
                                    AllowPaging="false"
                                    AllowSorting="true"
                                    SortMode="Automatic"
                                    CssClass="table table-hover table-bordered table-hover dataTable"
                                    GridLines="Horizontal"
                                    AutoGenerateColumns="False"
                                    ShowHeaderWhenEmpty="true"
                                    ShowFooter="false"
                                    EnableModelValidation="True"
                                    EmptyDataText="There are no reciepients to display.">
                                    <Columns>
                                        <asp:BoundField DataField="Company" HeaderText="Company">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="State" HeaderText="Province">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Email" HeaderText="Email">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AltEmail" HeaderText="Alternate Email">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Result" HeaderText="Reason">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>

                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            <div class="text-center">
                <a class="btn btn-primary" href="/mailing/newsletters.aspx">Send Another Newsletter</a>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script src="https://cloud.tinymce.com/stable/jquery.tinymce.min.js"></script>
    <script src="https://cloud.tinymce.com/stable/tinymce.min.js?apiKey=tq4yog56p3newhumnylnl4w3xhgpwknugdwnyxgsekhzaba7"></script>
    <script>tinymce.init({
            branding: false,
            content_css: '/css/tinyMCE.css',
            convert_urls: false,
            height: 500,
            image_class_list: [
                { title: 'None', value: 'img-fluid' },
                { title: 'Thumbnail', value: 'img-fluid img-thumbnail' },
                { title: 'Rounded', value: 'img-fluid rounded' },
                { title: 'Circle', value: 'img-fluid rounded-circle' }
            ],
            image_dimensions: false,
            removed_menuitems: 'newdocument',
            moxiemanager_insert_filter: function (file) {
                file.url = file.path;
                file.meta.url = file.url;
            },
            moxiemanager_image_template: '<img src="{$url}" class="img-fluid" /></a>',
            moxiemanager_rootpath: '/uploads/files',
            moxiemanager_remember_last_path: true,
            moxiemanager_title: 'File Manager',
            moxiemanager_upload_auto_close: true,
            moxiemanager_view: 'thumbs',
            paste_as_text: true,
            plugins: 'searchreplace autolink directionality visualblocks visualchars fullscreen link code codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount  imagetools contextmenu colorpicker textpattern help',
            relative_urls: false,
            remove_script_host: true,
            selector: '.tinyMCE',
            statusbar: false,
            style_formats_merge: true,
            table_default_attributes: {
                class: 'table'
            },
            table_cell_class_list: [
                { title: 'None', value: '' },
                { title: 'Active', value: 'table-active' },
                { title: 'Primary', value: 'table-primary' },
                { title: 'Secondary', value: 'table-secondary' },
                { title: 'Success', value: 'table-success' },
                { title: 'Danger', value: 'table-danger' },
                { title: 'Warning', value: 'table-warning' },
                { title: 'Info', value: 'table-info' },
                { title: 'Light', value: 'table-light' },
                { title: 'Dark', value: 'table-dark' }
            ],
            table_class_list: [
                { title: 'None', value: 'table' },
                { title: 'Striped rows', value: 'table table-striped' },
                { title: 'Bordered table', value: 'table table-bordered' },
                { title: 'Hoverable rows', value: 'table table-hover' },
                { title: 'Small table', value: 'table table-sm' }
            ],
            table_row_class_list: [
                { title: 'None', value: '' },
                { title: 'Active', value: 'table-active' },
                { title: 'Primary', value: 'table-primary' },
                { title: 'Secondary', value: 'table-secondary' },
                { title: 'Success', value: 'table-success' },
                { title: 'Danger', value: 'table-danger' },
                { title: 'Warning', value: 'table-warning' },
                { title: 'Info', value: 'table-info' },
                { title: 'Light', value: 'table-light' },
                { title: 'Dark', value: 'table-dark' }
            ],
            theme: 'modern',
            toolbar: 'formatselect fontselect fontsizeselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat',
            font_formats: 'Arial=arial,helvetica,sans-serif;Times New Roman=TimesNewRoman, Times New Roman, Times, Baskerville, Georgia, serif;Courier New=courier new,courier,monospace;',
            contextmenu: "link image inserttable | cell row column deletetable",
            importcss_append: true,
            importcss_merge_classes: true,
            textcolor_map: [
                '007bff', 'Primary',
                '868e96', 'Secondary',
                '28a745', 'Success',
                'dc3545', 'Danger',
                'ffc107', 'Warning',
                '17a2b8', 'Info',
                'f8f9fa', 'Light',
                '343a40', 'Dark',
                'ffffff', 'White',
                'ce0f69', 'Pink',
                '3d263a', 'Dark Plum',
                'ff9f72', 'Peach',
                '007770', 'Green'
            ]
        });</script>
    <script type="text/javascript">
        function ValidateForm() {
            var i, iSelCount, iSelExclCount;
            var messageRadioButton;
            var fromTextBox, subjectTextBox, messageTextBox, attachmentTextBox;
            var asscListBox, asscExcludeListBox, provListBox;
            var cjCheckbox;
            var _strAsscList, _strAsscExcludeList, _strProvList;
            var _strMessage;

            iSelCount = 0;
            iSelExclCount = 0;

            messageRadioButton = $$("rbTemplateMessage");
            fromTextBox = $$("txtFrom");
            subjectTextBox = $$("txtSubject");
            messageTextBox = $$("txtMessage");

            if (fromTextBox != null) {
                if (fromTextBox.val().trim() == "") {
                    alert("You must specify a from address.");
                    return false;
                }
            }

            if (subjectTextBox != null) {
                if (subjectTextBox.val().trim() == "") {
                    alert("You must specify a subject for your newsletter email message.");
                    return false;
                }
            }


            if (messageRadioButton != null) {
                if (messageRadioButton.checked == true) {
                    if (messageTextBox != null) {
                        if (messageTextBox.val().trim() == "") {
                            alert("You must specify a message for your newsletter email message.");
                            return false;
                        }
                    }
                }
            }
            else {
                alert("A problem occurred referencing the message type option.");
                return false;
            }

            asscListBox = $$("lbAssociations");
            asscExcludeListBox = $$("lbAssociationsExclude");
            provListBox = $$("lbProvinces");

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
            }
            else {
                alert("Could not locate associations list box on the form.  Contact your System Administrator.");
                return false;
            }
        }
        $('.attachmentControl .btn-remove').on('click', function () {
            $(this).parent().remove();
        });
        $('.attachmentControl input').change(function () {
            if ($(this).val() != null && !$(this).hasClass('hasFile')) {
                $(this).parent().parent().append($(this).parent().clone(true).val(null));
                $('.attachmentControl input').last().val(null);
                $(this).addClass('hasFile');
            }
        });
        $('#previewNewsletter').click(function () {
            if (ValidateForm()) {
                $('#newsletterPreview').html(tinymce.activeEditor.getContent())
                $('#previewModal').modal('show');
            }
        });
        var filenames = $('#hfAttachments').val().split(",");
        if (filenames.length > 0) {

        }
    </script>
</asp:Content>

