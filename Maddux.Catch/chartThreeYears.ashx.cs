using Newtonsoft.Json;
using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Maddux.Catch
{
    public class chartThreeYears : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (MadduxEntities madduxEntities = new MadduxEntities())
            {

                int startYear = DateTime.Now.Year;

                // In October, we want to start displaying next year instead of 3 years back
                if (DateTime.Now.Month < 10)
                {
                    startYear = startYear - 1;
                }

                DateTime firstDayLastYear = new DateTime(startYear - 1, 1, 1);
                DateTime lastDayNextYear = new DateTime(startYear + 1, 12, 31);

                var shippedOrders = madduxEntities.Orders
                                        .Include(x => x.OrderItems)
                                        .Where(order =>
                                            order.OrderStatus == 1
                                            && order.RequestedShipDate.HasValue
                                            && order.RequestedShipDate >= firstDayLastYear
                                            && order.RequestedShipDate <= lastDayNextYear
                                        );

                var lastYearsShippedOrders = shippedOrders
                    .Where(x => x.RequestedShipDate.Value.Year == startYear - 1)
                    .ToList();

                var thisYearsShippedOrders = shippedOrders
                    .Where(x => x.RequestedShipDate.Value.Year == startYear)
                    .ToList();

                var nextYearsShippedOrders = shippedOrders
                    .Where(x => x.RequestedShipDate.Value.Year == startYear + 1)
                    .ToList();

                GraphModel graphModel = new GraphModel();

                graphModel.labels = new List<string>()
                    {
                        "January",
                        "February",
                        "March",
                        "April",
                        "May",
                        "June",
                        "July",
                        "August",
                        "September",
                        "October",
                        "November",
                        "December"
                    };

                GraphDataSet lastYearChart = new GraphDataSet
                {
                    label = $"{startYear - 1}",
                    backgroundColor = "rgb(255, 128, 0)",
                    borderColor = "rgb(255, 128, 0)",
                    fill = false,
                    type = "bar"
                };

                GraphDataSet thisYearChart = new GraphDataSet
                {
                    label = $"{startYear}",
                    backgroundColor = "rgb(51, 121, 183)",
                    borderColor = "rgb(51, 121, 183)",
                    fill = true,
                    type = "bar"
                };

                GraphDataSet nextYearChart = new GraphDataSet
                {
                    backgroundColor = "rgb(204, 0, 0)",
                    borderColor = "rgb(204, 0, 0)",
                    label = $"{startYear + 1}",
                    fill = false,
                    type = "bar"
                };

                for (int month = 1; month < 13; month++)
                {

                    var lastYearOrders = lastYearsShippedOrders
                        .Where(x => x.RequestedShipDate.Value.Month == month).ToList();

                    var thisYearOrders = thisYearsShippedOrders
                        .Where(x => x.RequestedShipDate.Value.Month == month).ToList();

                    var nextYearOrders = nextYearsShippedOrders
                        .Where(x => x.RequestedShipDate.Value.Month == month).ToList();

                    lastYearChart.data
                        .Add((decimal)lastYearOrders
                        .Where(x => x.RequestedShipDate.Value.Month == month)
                        .Sum(x => x.GrandTotal));

                    thisYearChart.data
                        .Add((decimal)thisYearOrders
                        .Where(x => x.RequestedShipDate.Value.Month == month)
                        .Sum(x => x.GrandTotal));

                    nextYearChart.data
                        .Add((decimal)nextYearOrders
                        .Where(x => x.RequestedShipDate.Value.Month == month)
                        .Sum(x => x.GrandTotal));
                }

                graphModel.datasets.Add(lastYearChart);
                graphModel.datasets.Add(thisYearChart);
                graphModel.datasets.Add(nextYearChart);

                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(graphModel));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private class GraphDataElement
        {
            public DateTime? DateShipped { get; set; }
            public Order Order { get; set; }
        }
    }
}