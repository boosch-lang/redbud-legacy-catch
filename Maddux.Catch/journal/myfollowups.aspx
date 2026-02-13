<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="myfollowups.aspx.cs" Inherits="Maddux.Catch.Journal.myfollowups" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row">
        <div class="col-xs-12">
            <div class="btn-group">
                <button type="button" class="btn btn-default btn-sm checkbox-toggle-on"><i class="fa fa-check-square-o"></i>Select All</button>
                <button type="button" class="btn btn-default btn-sm checkbox-toggle-off"><i class="fa fa-square-o"></i>Select None</button>
                <button id="btnResolveSelected" type="button" class="btn btn-default btn-sm" runat="server" onserverclick="btnResolveSelected_Click"><i class="fa fa-thumbs-o-up"></i>Resolve Selected</button>
            </div>

            <a href="/customer/customerdetail.aspx?customerid=0" class="btn btn-default btn-sm"><i class="fa fa-address-card"></i>New Customer</a>
            <button id="ReloadRating" type="button" class="btn btn-default btn-sm" runat="server" onserverclick="btnReloadRating_Click"><i class="fa fa-refresh"></i>Reload Rating</button>
            <div class="pull-right">
            </div>
        </div>
    </div>
    <div style="margin-top: 10px" class="row top-buffer">
        <div class="col-6 col-lg-3">
            <asp:DropDownList ID="ddlFilterUser" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlFilterUser_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="col-6 col-lg-3">
            <asp:DropDownList ID="ddlFilterProvince" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlFilterProvince_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="col-6 col-lg-3">
            <asp:DropDownList ID="ddlFilterAssociation" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlFilterAssociation_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="col-6 col-lg-3">
            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlRating" AutoPostBack="True" OnSelectedIndexChanged="ddlRating_SelectedIndexChanged">
                <asp:ListItem Text="-- Select Customer Rating --" Value="" />
                <asp:ListItem Text="5 Star" Value="5" />
                <asp:ListItem Text="4 Star" Value="4" />
                <asp:ListItem Text="3 Star" Value="3" />
                <asp:ListItem Text="2 Star" Value="2" />
                <asp:ListItem Text="1 Star" Value="1" />
                <asp:ListItem Text="0 Star" Value="0" />
                <asp:ListItem Text="No Rating" Value="999" />
            </asp:DropDownList>
        </div>
        <!--
        <div class="col-6 col-lg-3">
            <asp:DropDownList runat="server" AutoPostBack="True" CssClass="form-control" ID="ddlCampaigns" OnSelectedIndexChanged="ddlCampaigns_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        -->
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvFollowups" runat="server"
                    AllowPaging="false"
                    AllowSorting="true"
                    ShowFooter="true"
                    SortMode="Automatic"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal"
                    ShowHeaderWhenEmpty="true"
                    OnPageIndexChanging="dgvFollowups_PageIndexChanging"
                    EmptyDataText="There are no followups to display."
                    DataKeyNames="JournalID" AutoGenerateColumns="false">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:ImageField DataImageUrlField="ImgHighlight" HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" Width="20px" />
                        </asp:ImageField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkJournalSelect" CssClass="checkbox-toggle" runat="server" />
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <HeaderTemplate>#</HeaderTemplate>
                            <ItemTemplate>
                                <a href="journaldetail.aspx?id=<%# Eval("JournalID") %>" title="Journal Details" data-toggle="modal" data-target="#modalView" data-remote="false" title_text="Journal #<%# Eval("JournalID") %>">
                                    <%# Eval("JournalID") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="AssignedToName" HeaderText="Assigned To" HeaderStyle-CssClass="visible-md visible-lg" ItemStyle-CssClass="visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FollowUpDate" HeaderText="Followup" DataFormatString="{0:MMMM dd, yyyy} " HeaderStyle-CssClass="visible-sm visible-md visible-lg col-sm-2" ItemStyle-CssClass="visible-sm visible-md visible-lg col-sm-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="State" HeaderText="Province" HeaderStyle-CssClass="visible-md visible-lg" ItemStyle-CssClass="visible-md visible-lg">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <HeaderTemplate>Customer</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                    <%# Eval("Company") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-sm visible-md visible-lg">
                            <HeaderTemplate>Star Rating</HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("StarRatingGraphic") %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notes" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-md-4" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-md-4">
                            <ItemStyle VerticalAlign="Top" />
                            <ItemTemplate>
                                <div>
                                    <%# Eval("JournalNotes") %>
                                </div>
                                <br />
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" PreviousPageText="Previous" PageButtonCount="5" NextPageText="Next" LastPageText="Last" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row">
                        <h4 class="modal-title col-xs-11" id="modalViewTitle">Loading...</h4>
                        <div class="text-right col-xs-1">
                            <a href="#" style="color: #fff" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">
                                <i class="fa fa-close"></i>
                            </span></a>
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
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
    <script type="text/javascript">
        $("#modalView").on("show.bs.modal", function (e) {
            $(this).find(".modal-title").text("Loading...");
            $(this).find(".modal-body").html("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

            var eventSource = $(e.relatedTarget);
            if (eventSource.text().length > 0) {
                $(this).find(".modal-title").text(eventSource.attr("title_text"));
                $(this).find(".modal-body").html('<div class="embed-responsive embed-responsive-4by3" id="iframeContainer"></div>');
                $('<iframe id="modal-popup-content"  style="border:none;" allowfullscreen="true"  />').appendTo('#iframeContainer');
                $('#modal-popup-content').attr("src", eventSource.attr("href"));
            }
        });

        $('#modalView').on('hidden.bs.modal', function (e) {
            $(this).find('iframe').html("").attr("src", "");
        });

        $('.checkbox-toggle-on').on('click', function (e) {
            toggleCheck(true);
        });

        $('.checkbox-toggle-off').on('click', function (e) {
            toggleCheck(false);
        });

        function toggleCheck(checked) {
            $.each($('.checkbox-toggle').find('input'), function (i, item) {
                $(item).prop('checked', checked);
            });
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

