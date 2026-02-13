<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="Maddux.Catch.customer.search" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvCustomers" runat="server"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                    ShowFooter="true" OnRowDataBound="dgvCustomers_RowDataBound"
                    EmptyDataText="No customer records found.">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-md-3" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-6 col-md-3">
                            <HeaderTemplate>Company</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='<%# ResolveUrl("~/customer/customerdetail.aspx?CustomerID=" + Eval("CustomerId")) %>'>
                                    <%# Eval("Company") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-md visible-lg col-md-2" ItemStyle-CssClass="visible-md visible-lg col-md-2">
                            <HeaderTemplate>Contact</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblContact" runat="server" />
                                <asp:HyperLink ID="hypContact" runat="server"><%# Eval("Contact") %></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CityState" HeaderText="City" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2">
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-md visible-lg col-md-1" ItemStyle-CssClass="visible-md visible-lg col-md-1">
                            <HeaderTemplate>Last Journal</HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hypJournal" runat="server" title="Journal Details" data-toggle="modal" data-target="#modalView" data-remote="false" title_text='Journal #<%# Eval("JournalID") %>'><%# Eval("JournalID") %></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-md visible-lg col-md-2" ItemStyle-CssClass="visible-md visible-lg col-md-2">
                            <HeaderTemplate>Last&nbsp;Order&nbsp;Placed</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='<%# ResolveUrl("~/order/orderdetail.aspx?id=" + Eval("LastOrderId")) %>'>
                                    <%# Eval("LastOrderDate", "{0:MMMM dd, yyyy}") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
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
            //$('#modalView').modal("handleUpdate");
        });

        $('#modalView').on('hidden.bs.modal', function (e) {
            $(this).find('iframe').html("").attr("src", "");
        });

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
