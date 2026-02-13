<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="campaigns.aspx.cs" Inherits="Maddux.Catch.Campaign.campaigns" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="col-xs-12" style="padding-left: 0px;">
        <a id="lnkNewCampaign" href="/campaign/campaigndetail.aspx" runat="server" class="btn btn-primary">
            <i class="fa fa-plus"></i>&nbsp;New Campaign
        </a>
    </div>
    <div class="col-xs-12" style="padding-left: 0px;">
        <div class="pull-right">
            <asp:DropDownList ID="ddlFilterProgram" runat="server" AutoPostBack="True" CssClass="btn btn-default btn-sm" OnSelectedIndexChanged="ddlFilterProgram_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvCampaigns" runat="server"
                    OnRowDataBound="dgvCampaigns_RowDataBound"
                    CssClass="table table-bordered table-hover dataTable"
                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                    ShowFooter="true"
                    EmptyDataText="There are no campaigns to display.">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>
                                Campaign
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href="campaigndetail.aspx?id=<%# Eval("CampaignId") %>"><%# Eval("CampaignName") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-5" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>
                                Campaign Details
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField runat="server" id="hdnCampaignID" value='<%# Eval("CampaignId") %>' />
                                <asp:GridView ID="dgvCampaignDetails" runat="server"
                                    CssClass="table-bordered dataTable"
                                    GridLines="Horizontal" AutoGenerateColumns="False"
                                    EmptyDataText="There are no campaign details to display.">
                                    <Columns>
                                        <asp:HyperLinkField DataTextField="CatalogName" DataNavigateUrlFields="CatalogID" DataNavigateUrlFormatString="/catalogs/catalogdetail.aspx?CatalogID={0}" HeaderText="Catalog Name" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-6" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" Target="_blank">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:HyperLinkField>
                                        <asp:HyperLinkField DataTextField="RackName" DataNavigateUrlFields="RackID" DataNavigateUrlFormatString="/racks/rackdetail.aspx?RackID={0}" HeaderText="Rack Name" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-6" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" Target="_blank">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:HyperLinkField>
                                        <asp:BoundField DataField="Count" HeaderText="Sold" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <%--<button class="btn btn-link campaign-details p-0" id="<%# Eval("CampaignId") %>"><%# Eval("RacksOrdered") %></button>--%>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Goal" HeaderText="Goal" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CustomersReachedNumber" HeaderText="Customer Reached" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CustomersReachedPercent" HeaderText="Customer Reached (%)" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SalesEnd" HeaderText="Order Deadline" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Shipdate" HeaderText="Ship Date" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="campaignDetails" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Rack Ordered
              <span class="pull-right">
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
              </span>
                    </h4>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <thead>
                            <tr>
                                <td style="width: 45%">Catalog Name</td>
                                <td style="width: 45%">Rack Name</td>
                                <td style="width: 10%">Sold</td>
                            </tr>
                        </thead>
                        <tbody id="cDetails">
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script>
        $(document).on("click", ".campaign-details", function (e) {
            e.preventDefault();
            var id = e.target.id;
            var data = new FormData();
            data.append("campaignID", id);
            $.ajax({
                url: "/campaign/request/GetCampaignDetail.ashx",
                data: data,
                type: 'POST',
                processData: false,
                contentType: false,
                success: function (data) {
                    console.log(data);
                    if (data.success == true) {
                        var html = "";
                        $.each(data.details, function (i, v) {
                            html += "<tr>";
                            html += `<td><a target="_blank" href="/catalogs/catalogdetail.aspx?CatalogID=${v.CatalogID}">${v.CatalogName}</a></td>`;
                            html += `<td><a target="_blank" href="/racks/rackdetail.aspx?RackID=${v.RackID}">${v.Name}</a></td>`;
                            html += `<td>${v.Count}</td>`;
                            html += "</tr>";
                        });
                        $("#cDetails").empty();
                        $("#cDetails").append(html)
                        $("#campaignDetails").modal("show");
                    }
                    else {
                        $("#cDetails").append(`<h3 class="text-danger">${data.errors}</h3>`)
                        $("#campaignDetails").modal("show");
                    }

                }
            })
            console.log(id);
        })
    </script>
</asp:Content>
