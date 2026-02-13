using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Maddux.Catch.Helpers
{
    public static class RegionHelper
    {
        private static Dictionary<string, List<string>> GetRegions()
        {
            return new Dictionary<string, List<string>>()
            {
                { "NFLD", new List<string>(){"NL"} },
                { "Maritimes" , new List<string>(){"NS","NB","PE"} },
                { "N Quebec" , new List<string>(){"QC"} },
                { "S Quebec", new List<string>(){"QC"} },
                { "Montreal", new List<string>(){"QC"} },
                { "S Ontario" , new List<string>(){"ON"} },
                { "GTHA Ontario", new List<string>(){"ON"} },
                { "N Ontario", new List<string>(){"ON"} },
                { "Prairies (Core)", new List<string>(){"MB", "SK", "AB"} },
                { "Prairies (Rural)" , new List<string>(){ "MB", "SK", "AB" } },
                { "British Columbia" , new List<string>(){"BC"} },
                { "Vancouver", new List<string>() { "BC"} },
                { "Territories", new List<string>(){"YT", "NU", "NW"} }
            };
        }

        public static List<ListItem> GetRegionsForSelectList(bool addAllOption, string text = "All Regions", string value = "All")
        {
            var regions = GetRegions();
            var regionsList = regions.Select(x => new ListItem() { Value = x.Key, Text = x.Key }).ToList();

            if (addAllOption)
            {
                regionsList.Insert(0, new ListItem() { Text = text, Value = value });
            }

            return regionsList;
        }

        public static List<string> GetRegionNames()
        {
            var regions = GetRegions();
            return regions.Select(x => x.Key).ToList();
        }

        public static List<string> GetProvincesForRegion(string region)
        {
            var regions = GetRegions();
            if (!regions.TryGetValue(region, out var provinces))
            {
                return new List<string>();
            }
            return provinces;
        }

    }
}
