<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="programdetail.aspx.cs" Inherits="Maddux.Catch.products.programdetail" %>


<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row row-margin">
        <div class="alert alert-success alert-dismissible" id="successAlert" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <strong>Success!</strong> <span id="spSuccessMessage" runat="server"></span>
        </div>

        <div class="col-xs-12">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <asp:Button Text="Delete Program" runat="server" ID="btnDeleteProgram"
                OnClientClick="return confirm('Are you sure you want to delete this program?')"
                OnClick="btnDeleteProgram_Click" CssClass="btn btn-danger" />
            <asp:Button Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click" CssClass="btn btn-default" />
        </div>
    </div>

    <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
        <li id="tab-item-details"><a href="#details" class="active" data-toggle="tab" id="tabDetails" runat="server">Details</a></li>
        <li id="tab-item-catalogs"><a href="#catalogs" data-toggle="tab" id="tabProductCatalogs" runat="server">Product Catalogs</a></li>
    </ul>
    <div class="tab-content" style="padding: 15px">
        <div class="tab-pane fade" id="details">
            <div class="panel-group">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" href="#programDetails">Program Details</a>
                        </h4>
                    </div>
                    <div id="programDetails" class="panel-collapse collapse in p-1" aria-expanded="true">
                        <div class="row">
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <label class="font-weight-bold required">Program Name: </label>
                                    <asp:TextBox ID="txtProgramName" runat="server"
                                        CssClass="form-control" MaxLength="100"
                                        TabIndex="7" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvFirstName" runat="server"
                                        ControlToValidate="txtProgramName" Display="Dynamic"
                                        ErrorMessage="You must enter a program name."></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-xs-12">
                                    <label class="font-weight-bold">Program Description: </label>
                                    <asp:TextBox ID="txtProgramDescription" runat="server" TextMode="multiline" Columns="50" Rows="6"
                                        CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <div class="col-xs-12">
                                    <div class="col-xs-2">
                                        <label class="font-weight-bold">Active:</label>
                                    </div>
                                    <div class="col-xs-10">
                                        <asp:CheckBox ID="chkActive" runat="server" CssClass="Ctrls formatted-chk" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="tab-pane fade" style="display:none" id="shipdates">
            <div class="row">
                <div class="col-xs-12">
                    <a href="/products/shipdate.aspx?programID=<%= ProgramID %>" class="btn btn-primary" title_text="New Ship Date" data-toggle="modal" data-target="#modalView" data-remote="false">
                        <i class="fa fa-plus"></i>&nbsp;New Ship Date
                    </a>
                </div>
            </div>
            <asp:GridView ID="dgvShipdates" runat="server"
                CssClass="table table-hover table-bordered table-hover dataTable"
                GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                EmptyDataText="There are no ship dates to display.">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                        <HeaderTemplate>
                            Ship Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="shipdate.aspx?id=<%# Eval("ShipDateID") %>&programid=<%# Eval("ProgramID") %>" title="Ship Date" data-toggle="modal" data-target="#modalView" data-remote="false" title_text="Ship Date - <%# string.Format("{0:MMMM dd, yyyy}",Eval("ShipDate")) %>">
                                <%# string.Format("{0:MMMM dd, yyyy}",Eval("ShipDate")) %></a>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderDeadlineDate" HeaderText="Order Deadline" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="tab-pane fade" id="catalogs">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div class="col-xs-3">
                        <div class="col-xs-12">
                            <div class="PageMainText col-xs-12">
                                <asp:Label ID="lblProgramAssignedCatalogs" runat="server" Font-Bold="True" Text="Assigned Catalogues:"></asp:Label>
                                <br />
                                <asp:ListBox ID="lbProgramAssignedCatalogs" runat="server" CssClass="Ctrls" Height="250px"
                                    Width="225px" TabIndex="36"></asp:ListBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-2 action-margin" id="catalogControls" runat="server">
                        <div class="col-xs-12">
                            <div class="PageMainText col-xs-12">
                                <asp:Button ID="cmdAddCat" runat="server" Text="  <<  "
                                    CausesValidation="False" OnClick="cmdAddProCat_Click" Width="50px" CssClass="Ctrls btn btn-success" TabIndex="35" /><br />
                                <br />
                                <asp:Button ID="cmdRemCat" runat="server" Text="  >>  "
                                    CausesValidation="False" OnClick="cmdRemProCat_Click" Width="50px" CssClass="Ctrls btn btn-danger" TabIndex="37" /><br />
                                <br />
                                <asp:Button ID="cmdSaveCat" runat="server" OnClick="cmdSaveProCat_Click" Text="Save"
                                    Width="50px" CssClass="Ctrls btn btn-primary" TabIndex="38" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="col-xs-12">
                            <div class="PageMainText col-xs-12">
                                <asp:Label ID="lblAvailableCatalogs" runat="server" Font-Bold="True" Text="Available Catalogues:"></asp:Label><br />
                                <asp:ListBox ID="lbProgramAvailableCatalogs" runat="server" CssClass="Ctrls" Height="250px"
                                    Width="225px" TabIndex="34"></asp:ListBox>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--End .tab-content--->
    <asp:TextBox ID="txtActiveTab" runat="server" CssClass="form-control" Type="hidden"></asp:TextBox>
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row">
                        <h4 class="modal-title col-xs-11" id="modalViewTitle">Loading...</h4>
                        <div class="text-right col-xs-1">
                            <a href="#" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></a>
                        </div>
                    </div>
                </div>
                <div id="divModalViewBody" class="modal-body">
                    <img src="img/loading.gif" alt="Loading..." id="imgLoadingModalContent" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/extra.css" rel="stylesheet" />
    <script src="../js/extra.js"></script>

    <script type="text/javascript">
        //$("#modalView").on("show.bs.modal", function (e) {
        //    $(this).find(".modal-title").text("Loading...");
        //    $(this).find(".modal-body").html("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

        //    var eventSource = $(e.relatedTarget);
        //    if (eventSource.text().length > 0) {
        //        $(this).find(".modal-title").text(eventSource.attr("title_text"));
        //        $(this).find(".modal-body").html('<div class="embed-responsive embed-responsive-4by3" id="iframeContainer"></div>');
        //        $('<iframe id="modal-popup-content"  style="border:none;" allowfullscreen="true"  />').appendTo('#iframeContainer');
        //        $('#modal-popup-content').attr("src", eventSource.attr("href"));
        //    }
        //});

        //$('#modalView').on('hidden.bs.modal', function (e) {
        //    $(this).find('iframe').html("").attr("src", "");
        //});


        function ConfirmAction(Message) {
            if (confirm(Message) == true)
                return true;
            else
                return false;
        }

        function CloseModal(frameElement) {
            if (frameElement) {
                var dialog = $(frameElement).closest(".modal");
                if (dialog.length > 0) {
                    dialog.modal("hide");
                }
            }
        }
    </script>

</asp:Content>
