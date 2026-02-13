using Newtonsoft.Json;
using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Maddux.Catch
{
    public class GraphModel
    {
        public List<string> labels { get; set; } = new List<string>();
        public List<GraphDataSet> datasets { get; set; } = new List<GraphDataSet>();
    }
    public class GraphDataSet
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public List<decimal> data { get; set; } = new List<decimal>();
        public bool fill { get; set; } = false;
        public string type { get; set; }
    }

    public class YearConfiguration
    {
        public string Color { get; set; }
        public string Type { get; set; }
    }

    public class chart : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (MadduxEntities madduxEntities = new MadduxEntities())
            {
                GraphModel graphModel = new GraphModel();
                int thisYear = DateTime.Now.Year;

                var yearConfigurations = new List<YearConfiguration>()
                {
                    new YearConfiguration() { Color = "rgb(204, 0, 0)", Type = "bar" },
                    new YearConfiguration() { Color = "rgb(51, 121, 183)", Type = "line" },
                    new YearConfiguration() { Color = "rgb(255, 128, 0)", Type = "line" }
                };

                for (int i = 0; i < yearConfigurations.Count; i++)
                {
                    var firstDay = new DateTime(thisYear - i, 1, 1);
                    var lastDay = new DateTime(thisYear - i, 12, 31);

                    var ordersForYear = madduxEntities.Orders
                        .Include(x => x.OrderItems)
                        .Where(x =>
                            x.OrderStatus == 1
                            && x.OrderDate.HasValue && x.OrderDate >= firstDay
                            && x.OrderDate <= lastDay)
                        .ToList();

                    var configuration = yearConfigurations[i];
                    GraphDataSet chart = new GraphDataSet
                    {
                        label = firstDay.Year.ToString(),
                        backgroundColor = configuration.Color,
                        borderColor = configuration.Color,
                        fill = configuration.Type == "bar",
                        type = configuration.Type
                    };

                    for (int month = 1; month < 13; month++)
                    {
                        chart.data.Add((decimal)ordersForYear.Where(x => x.OrderDate.Value.Month == month).Sum(x => x.GrandTotal));
                    }

                    graphModel.datasets.Insert(0, chart);
                }

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
    }
}