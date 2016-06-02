using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RiverChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://environment.data.gov.uk/flood-monitoring/id/stations?riverName=Stort";
            List<string> stationNames = new List<string>();

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(url);

                Regex rg = new Regex("wiskiID.*");

                MatchCollection mc = rg.Matches(json);

                foreach (var match in mc)
                {
                    stationNames.Add(match.ToString().Split(':')[1].Replace("\"", "").Trim());
                }

                foreach (var name in stationNames)
                {
                    string stationUrl = BuildStationUrl(name);
                    json = wc.DownloadString(stationUrl);

                }
            }


            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static string BuildStationUrl(string name)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"http://environment.data.gov.uk/flood-monitoring/id/stations/");
            sb.Append(name);
            sb.Append(@"/readings?_sorted&startdate=");
            sb.Append("2016-05-29");
            sb.Append("enddate=");
            sb.Append("2016-05-30");
            return sb.ToString();
        }
    }
}
