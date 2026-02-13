<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="campaigndetail.aspx.cs" Inherits="Maddux.Catch.campaign.campaigndetail" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="../css/extra.css" rel="stylesheet" />
    <style>
        .mx-0 {
            margin-left: 0px !important;
            margin-right: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div id="Tabs" role="tabpanel">
        <ul class="nav nav-tabs" role="tablist" id="nav" runat="server">
            <li id="tabDetails" runat="server"><a href="#details" data-toggle="tab">Details</a></li>
            <li id="tabStatus" onclick="drawChart()" style="display: none" runat="server"><a href="#status" data-toggle="tab">Status</a></li>
        </ul>
        <div class="row">
            <div class="tab-content" style="padding: 15px">
                <div class="tab-pane fade" id="details">
                    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
                    </asp:ScriptManager>

                    <div class="col-xs-12">
                        <asp:Button Text="Save" OnClick="btnSave_Click" runat="server" ID="btnSave" CssClass="btn btn-primary" />
                        <asp:Button Text="Delete Campaign" OnClick="btnDeleteCampaign_Click" runat="server" ID="btnDeleteCampaign" OnClientClick="return confirm('Are you sure you want to delete this campaign?')" CssClass="btn btn-primary" />
                    </div>

                    <div style="margin-top: 10px!important" class="col-xs-12">
                        <div class="panel-group">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" href="#campaignDetails">Campaign Details</a>
                                    </h4>

                                </div>
                                <div id="campaignDetails" class="panel-collapse collapse in" aria-expanded="true">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="row mx-0">
                                                    <div class="col-12">
                                                        <label for="txtName" class="required font-weight-bold">Name: </label>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                                            ID="rfvName" runat="server"
                                                            ControlToValidate="txtName" Display="Dynamic"
                                                            ErrorMessage="You must enter a program name.">

                                                        </asp:RequiredFieldValidator>
                                                    </div>

                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row mx-0">
                                                            <div class="col-12">
                                                                <label for="ddlProgram" class="required font-weight-bold">Program: </label>
                                                                <asp:DropDownList ID="ddlProgram" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <%--<div class="col-12">
                                                                <label for="ddlShipdate" class="required font-weight-bold">Ship Date:</label>
                                                                <asp:DropDownList ID="ddlShipdate" DataTextFormatString="{0:MMMM dd, yyyy}" AutoPostBack="true" OnSelectedIndexChanged="ddlShipdate_SelectedIndexChanged" runat="server" CssClass="form-control" />
                                                                <asp:HiddenField ID="hidEndDate" runat="server"></asp:HiddenField>
                                                            </div>--%>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="row mx-0">
                                                    <div class="col-12">
                                                        <label for="txtStartDate" class="required font-weight-bold">Sales Start Date: </label>
                                                        <div class='input-group datepicker'>
                                                            <asp:TextBox ID="txtStartDate" data-date-format="MMMM DD, YYYY" runat="server" CssClass="form-control" required="required" onkeydown="return false;"></asp:TextBox>
                                                            <span class="input-group-addon">
                                                                <span class="fa fa-calendar"></span>
                                                            </span>
                                                        </div>
                                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                                            ID="rfvStartDate" runat="server"
                                                            ControlToValidate="txtStartDate" Display="Dynamic"
                                                            ErrorMessage="You must enter a sales start date."></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-12">
                                                        <label for="txtEndDate" class="required font-weight-bold">Sales End Date: </label>
                                                        <div class='input-group datepicker'>
                                                            <asp:TextBox ID="txtEndDate" data-date-format="MMMM DD, YYYY" runat="server" CssClass="form-control" required="required" onkeydown="return false;"></asp:TextBox>
                                                            <span class="input-group-addon">
                                                                <span class="fa fa-calendar"></span>
                                                            </span>
                                                        </div>
                                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                                            ID="rfvEndDate" runat="server"
                                                            ControlToValidate="txtEndDate" Display="Dynamic"
                                                            ErrorMessage="You must enter a sales end date."></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-12">
                                                        <label for="txtGoal" class="required font-weight-bold">Goal: </label>
                                                        <asp:TextBox ID="txtGoal" runat="server" type="number" min="0" CssClass="form-control" required="required"></asp:TextBox>
                                                        <asp:RequiredFieldValidator CssClass="text-danger"
                                                            ID="frvGoal" runat="server"
                                                            ControlToValidate="txtGoal" Display="Dynamic"
                                                            ErrorMessage="You must enter a sales goal."></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator CssClass="text-danger" runat="server" Display="Dynamic" ErrorMessage="Sales goal must be a positive number." ValidationExpression="^[1-9]\d*$" ControlToValidate="txtGoal" ID="revGoal"></asp:RegularExpressionValidator>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel ID="upCatalogs" runat="server">
                                                    <ContentTemplate>
                                                        <label class="h4 required font-weight-bold">Select Ship Date:</label>
                                                        <asp:Repeater ID="rptCatalogs" OnItemDataBound="rptCatalogs_ItemDataBound" runat="server">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading">
                                                                        <h4 class="panel-title"><%#Eval("CatalogName") %></h4>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <asp:HiddenField runat="server" ID="chkCatalogID" Value='<%#Eval("CatalogID") %>' />
                                                                        <asp:DropDownList ID="ddShipDates" runat="server" CssClass="form-control">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="tab-pane fade" style="display: none" id="status">
                    <div class="row">

                        <div class="col-xs-12">
                            <div id="chart_div"></div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="TabName" runat="server" />
            <asp:HiddenField ID="hidCampaignId" runat="server" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script src="../js/extra.js"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <script type="text/javascript">
        var chartDiv = $$('chart_div');
        var drawn = false;
        var campaignId = $$("hidCampaignId");
        var tab = $$("TabName");
        var campaignName = $$("txtName").val();
        var tabName = tab.val() != "" ? tab.val() : "details";

        $(document).ready(function () {
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                tab.val($(this).attr("href").replace("#", ""));
            });
            google.charts.load('current', { packages: ['corechart', 'bar'] });
            if (tabName === 'status') {
                google.charts.setOnLoadCallback(drawChart);
            }
        });

        /* function drawChart() {
             tabName = tab.val() != "" ? tab.val() : "details";

             if (!drawn) {
                 $.ajax({
                     type: "POST",
                     url: "campaigndetail.aspx/GetChartData",
                     data: "{cID: " + campaignId.val() + "}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (r) {
                         var data = new google.visualization.DataTable({
                             cols: [
                                 { label: 'Category', type: 'string' },
                                 { label: 'Percentage Reached', type: 'number' },
                                 { type: 'string', role: 'annotation' },
                                 { type: 'string', role: 'annotationText' }
                             ]
                         });
                         for (var i = 0; i < r.d.length; i++) {
                             data.addRow([r.d[i][0], r.d[i][1], r.d[i][2], r.d[i][3]]);
                         }
                         var formatPercent = new google.visualization.NumberFormat({
                             pattern: '#.#%'
                         });
                         formatPercent.format(data, 1);
                         var paddingHeight = 100;
                         var rowHeight = data.getNumberOfRows() * 45;
                         // set the total chart height
                         var chartHeight = rowHeight + paddingHeight;
                         var options = {
                             title: campaignName,
                             height: chartHeight,
                             chartArea: {
                                 height: rowHeight,
                                 width: '60%'
                             },
                             annotations: {
                                 highContrast: false,
                                 textStyle: {
                                     fontSize: 16,
                                     bold: true,
                                     // The color of the text.
                                     color: '#000',
                                     // The color of the text outline.
                                     auraColor: '#fff',
                                     // The transparency of the text.
                                     opacity: 0.8
                                 },
                                 alwaysOutside: true
                             },
                             bars: 'horizontal',
                             hAxis: {
                                 format: '#%',
                                 textStyle: {
                                     fontSize: 14 // or the number you want
                                 },
                                 viewWindow: {
                                     max: 1,
                                     min: 0
                                 },
                                 title: 'Percentage Reached'
                             },
                             vAxis: {
                                 textStyle: {
                                     fontSize: 14 // or the number you want
                                 }
                             },
                             colors: ['yellow']
                         };

                         var chart = new google.visualization.BarChart(chartDiv[0]);
                         chart.draw(data, google.charts.Bar.convertOptions(options));
                         drawn = true;
                     },
                     failure: function (r) {
                         console.log(r);
                     },
                     error: function (r, err, x) {
                         console.log(x);
                     }
                 });
             }
         }*/
    </script>
</asp:Content>
