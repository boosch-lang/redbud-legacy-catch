<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="myshipments.aspx.cs" Inherits="Maddux.Catch.shipping.myshipments" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="col-xs-12" style="padding-left: 0px;">
        <div class="btn-group">
            <button type="button" class="btn btn-default btn-sm checkbox-toggle-on"><i class="fa fa-check-square-o"></i>&nbsp;Select All</button>
            <button type="button" class="btn btn-default btn-sm checkbox-toggle-off"><i class="fa fa-square-o"></i>&nbsp;Select None</button>
        </div>
        <div class="btn-group">
            <button type="button" id="InvoiceItemsBtn" class="btn btn-default btn-sm"><i class="fa fa-thumbs-o-up"></i>&nbsp;Invoice Items</button>
            <button id="btnPrintPackingSlips" type="button" class="btn btn-default btn-sm" runat="server" onserverclick="btnPrintPackingSlips_ServerClick"><i class="fa fa-truck"></i>&nbsp;Print Packing Slips</button>
        </div>
        <div class="pull-right">
            <asp:DropDownList ID="ddlFilterProvince" runat="server" AutoPostBack="True" CssClass="btn btn-default btn-sm" OnSelectedIndexChanged="ddlFilterProvince_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:DropDownList ID="ddlFilterCatalog" runat="server" AutoPostBack="True" CssClass="btn btn-default btn-sm" OnSelectedIndexChanged="ddlFilterCatalog_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <iframe id="printBatchReport" style="display: none" runat="server"></iframe> 
    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvShipments"
                    runat="server"
                    AllowPaging="false"
                    AllowSorting="true"
                    SortMode="Automatic"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal"
                    AutoGenerateColumns="False"
                    ShowHeaderWhenEmpty="true"
                    ShowFooter="true"
                    OnPageIndexChanging="dgvShipments_PageIndexChanging"
                    DataKeyNames="ShipmentID"
                    EmptyDataText="There are no shipments to display.">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkShipmentSelect" CssClass="checkbox-toggle" runat="server" />
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Ship #</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href="shipmentdetail.aspx?id=<%# Eval("ShipmentID") %>&customerId=<%# Eval("CustomerId") %>" title="Shipment Details" title_text="Shipment #<%# Eval("ShipmentID") %>">
                                    <%# Eval("ShipmentID") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Order #</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href="/order/orderdetail.aspx?id=<%# Eval("OrderID") %>"><%# Eval("OrderID") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Catalog" HeaderText="Catalog" HeaderStyle-CssClass="visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-md visible-lg col-lg-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RackName" HeaderText="Rack" HeaderStyle-CssClass="visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-md visible-lg col-lg-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CreateDate" HeaderText="Date Created" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="InvoiceSentDate" HeaderText="Inv. Sent" DataFormatString="{0:MMMM dd, yyyy}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ShippingMethodDesc" HeaderText="Ship Via" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-2">
                            <HeaderTemplate>Ship To</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                    <%# Eval("ShippingName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-2">
                            <HeaderTemplate>Star Rating</HeaderTemplate>
                            <ItemTemplate>                              
                                    <%# Eval("StarRatingGraphic") %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Province" HeaderText="Province" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" HeaderStyle-CssClass="visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ShipmentTotal" HeaderText="Total" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-sm visible-md visible-lg col-lg-1" ItemStyle-CssClass="visible-sm visible-md visible-lg col-lg-1">
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:BoundField>
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

                            <a href="#" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></a>
                        </div>
                    </div>


                </div>

                <div id="divModalViewBody" class="modal-body">
                    <img src="<%=ResolveUrl("~/img/loading.gif") %>" alt="Loading..." id="imgLoadingModalContent" />
                </div>

            </div>
        </div>
    </div>
    <div class="modal fade" id="invoiceSentDateModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <span>Select Shipped Date</span>
                        <span class="pull-right">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </span>
                    </h4>

                </div>
                <div class="modal-body">
                    <div class="">
                        <asp:CheckBox Text="" CssClass="" ID="chkUpdateShippedDate" ClientIDMode="Static" Checked="true" runat="server" />&nbsp; <span style="font-weight: 700">Update Shipped date.</span>
                    </div>
                    <div class="row" id="ShippedDateDiv">
                        <div class="col-md-12">
                            <label class="font-weight-bold">Select Shipped Date</label>
                            <div class='input-group datepicker'>
                                <asp:TextBox runat="server" ID="shippedDate" data-date-format="MMMM DD, YYYY" CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                                <span class="input-group-addon">
                                    <span class="fa fa-calendar"></span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button id="btnShipSelected" type="button" class="btn btn-primary" runat="server" onserverclick="btnShipSelected_ServerClick"><i class="fa fa-thumbs-o-up"></i>Invoice Items</button>
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
            console.log(e);
            $(this).find(".modal-title").text("Loading...");
            $(this).find(".modal-body").html("<img src=\"img/loading.gif\" alt=\"Loading...\" id=\"imgLoadingModalContent\" />");

            var eventSource = $(e.relatedTarget);
            if (eventSource.text().length > 0) {
                $(this).find(".modal-title").text(eventSource.attr("title_text"));
                $(this).find(".modal-body").load(eventSource.attr("href"));
            }
        });

        $('#modalView').on('hidden.bs.modal', function (e) {
            $(this).removeData('bs.modal');
        });

        $('.checkbox-toggle-on').on('click', function (e) {
            toggleCheck(true);
        });

        $('.checkbox-toggle-off').on('click', function (e) {
            toggleCheck(false);
        });
        $("#InvoiceItemsBtn").on("click", function (e) {
            e.preventDefault();
            $("#invoiceSentDateModal").modal("show")
        });
        $("#chkUpdateInvoiceSentDate").on("change", function (e) {
            if ($(this).is(":checked")) {
                $("#InvoiceSentDateDiv").show(200);
            }
            else {
                $("#InvoiceSentDateDiv").hide(200);
            }
        })
        function toggleCheck(checked) {
            $.each($('.checkbox-toggle').find('input'), function (i, item) {

                $(item).prop('checked', checked);
            });
        }
    </script>
</asp:Content>


