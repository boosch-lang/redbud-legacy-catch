<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Maddux.Catch._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row">
        <div class="col-sm-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <span data-toggle="collapse">Campaigns</span>
                    </h4>
                </div>
                <div class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-4">
                                Active Campaigns:
                                <h3 style="margin-top: 0;">
                                    <asp:Literal ID="litCampaigns" runat="server"></asp:Literal>
                                </h3>
                            </div>
                            <div class="col-xs-4">
                                Racks Sold:
                                <h3 style="margin-top: 0;">
                                    <asp:Literal ID="litCampaignSold" runat="server"></asp:Literal>
                                </h3>
                            </div>
                            <div class="col-xs-4">
                                Customers Reached Reached:
                                <h3 style="margin-top: 0;">
                                    <asp:Literal ID="litCampaignReached" runat="server"></asp:Literal>
                                </h3>
                            </div>
                        </div>
                        <br />
                        <br />
                        <a class="btn btn-primary" href="/campaign/campaigns.aspx">View All Campaigns</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <span data-toggle="collapse">Follow Ups</span>
                    </h4>
                </div>
                <div class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-6">
                                My Follow Ups:
                        <h3 style="margin-top: 0;">
                            <asp:Literal ID="litMyFollowUps" runat="server"></asp:Literal>
                        </h3>
                            </div>
                        <div class="col-xs-6">
                        Open Follow Ups:
                        <h3 style="margin-top: 0;">
                            <asp:Literal ID="litFollowUps" runat="server"></asp:Literal>
                        </h3>
                        </div>
                    </div>
                    <br />
                    <br />
                    <a class="btn btn-primary" href="/journal/myfollowups.aspx">View All Follow Ups</a>
                </div>
            </div>
            </div>
        </div>
        <asp:PlaceHolder ID="phShipments" runat="server">
        <div class="col-sm-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <span data-toggle="collapse">Shipments</span>
                    </h4>
                </div>
                <div class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        Open Shipments:
                        <h3 style="margin-top: 0;">
                            <asp:Literal ID="litTotalShipments" runat="server"></asp:Literal>
                        </h3>
                        <br />
                        <a class="btn btn-primary" href="/shipping/myshipments.aspx">View All Shipments</a>
                    </div>
                </div>
            </div>
        </div>
            <asp:Placeholder ID="phYearOverYear" runat="server">
        <div class="col-sm-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <span data-toggle="collapse">Year Over Year</span>
                    </h4>
                </div>
                <div class="panel-collapse collapse in" aria-expanded="true">
                    <div class="panel-body">
                        <div id="revenueChart" class="text-center">
                            <i id="myChart" class="fa fa-spinner fa-pulse fa-3x fa-fw"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div></asp:Placeholder>

        <asp:Placeholder ID="phYearOverYear3" runat="server">
            <div class="col-sm-6">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <span data-toggle="collapse">Revenue 3 Year Comparison (Req Ship Month)</span>
                        </h4>
                    </div>
                    <div class="panel-collapse collapse in" aria-expanded="true">
                        <div class="panel-body">
                            <div id="revenueChart3Years" class="text-center">
                                <i id="myChart3Years" class="fa fa-spinner fa-pulse fa-3x fa-fw"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Placeholder>
        </asp:PlaceHolder>       
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js@2.8.0"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.matchHeight/0.7.2/jquery.matchHeight-min.js" integrity="sha512-/bOVV1DV1AQXcypckRwsR9ThoCj7FqTV2/0Bm79bL3YSyLkVideFLE3MIZkq1u5t28ke1c0n31WYCOrO01dsUg==" crossorigin="anonymous"></script>
 <script>
        $(function () {
            $.ajax({
                method: "GET",
                url: 'chart.ashx',
                success: function (chartData) {
                    $('#myChart').replaceWith('<canvas id="myChart"></canvas>');
                    var ctx = document.getElementById('myChart').getContext('2d');
                    new Chart(ctx, {
                        type: 'bar',
                        data: chartData,
                        options: {
                            responsive: true,
                            title: {
                                display: false,
                            },
                            tooltips: {
                                mode: 'index',
                                intersect: false,
                                callbacks: {
                                    label: function (tooltipItem, data) {
                                        var label = data.datasets[tooltipItem.datasetIndex].label || '';

                                        if (label) {
                                            label += ': ';
                                        }
                                        label += tooltipItem.yLabel.toLocaleString("en-CA", { style: "currency", currency: "CAD" });
                                        return label;
                                    },
                                },
                            },
                            hover: {
                                mode: 'nearest',
                                intersect: true
                            },
                            scales: {
                                xAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Month'
                                    }
                                }],
                                yAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Total'
                                    },
                                    ticks: {
                                        beginAtZero: true,
                                        callback: function (value, index, values) {
                                            return value.toLocaleString("en-CA", { style: "currency", currency: "CAD" });
                                        }
                                    }
                                }]
                            }
                        }
                    });
                    $('.panel').matchHeight();
                },
                error: function (error_data) {
                    console.log("Endpoint GET request error");
                    // console.log(error_data)
                }
            });

            $.ajax({
                method: "GET",
                url: 'chartThreeYears.ashx',
                success: function (chartData) {
                    $('#myChart3Years').replaceWith('<canvas id="myChart3Years"></canvas>');
                    var ctx = document.getElementById('myChart3Years').getContext('2d');
                    new Chart(ctx, {
                        type: 'bar',
                        data: chartData,
                        options: {
                            responsive: true,
                            title: {
                                display: false,
                            },
                            tooltips: {
                                mode: 'index',
                                intersect: false,
                                callbacks: {
                                    label: function (tooltipItem, data) {
                                        var label = data.datasets[tooltipItem.datasetIndex].label || '';

                                        if (label) {
                                            label += ': ';
                                        }
                                        label += tooltipItem.yLabel.toLocaleString("en-CA", { style: "currency", currency: "CAD" });
                                        return label;
                                    },
                                },
                            },
                            hover: {
                                mode: 'nearest',
                                intersect: true
                            },
                            scales: {
                                xAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Month'
                                    }
                                }],
                                yAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Total'
                                    },
                                    ticks: {
                                        beginAtZero: true,
                                        callback: function (value, index, values) {
                                            return value.toLocaleString("en-CA", { style: "currency", currency: "CAD" });
                                        }
                                    }
                                }]
                            }
                        }
                    });
                    $('.panel').matchHeight();
                },
                error: function (error_data) {
                    console.log("Endpoint GET request error");
                    // console.log(error_data)
                }
            });
        });
 </script>
</asp:Content>
