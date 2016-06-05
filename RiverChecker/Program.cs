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
            string riverName = "Stort";
            string dateFormat = "yyyy-MM-dd";

            List<string> stationNames = new List<string>();
            string jsonTemp;
            string stationUrl;
            MeasuresJsonResult stationMeasures;
            MeasureItem minItem;
            MeasureItem maxItem;
            double averageRiverLevel;

            //Get start and enddate for data, today and 7 days ago
            string dateToday = DateTime.Today.ToString(dateFormat);
            string dateLastWeek = DateTime.Today.AddDays(-6.0d).ToString(dateFormat);

            Console.WriteLine("Start date: " + dateLastWeek);
            Console.WriteLine("Date today: " +dateToday+"\n");

            //Use webclient to get the json response
            using (WebClient wc = new WebClient())
            {
                string riverStationsUrl = string.Format("http://environment.data.gov.uk/flood-monitoring/id/stations?riverName={0}", riverName);
                jsonTemp = wc.DownloadString(riverStationsUrl);
            }

            Regex idRegex = new Regex("wiskiID.*");
            MatchCollection regexMatches = idRegex.Matches(jsonTemp);

            string cleanID;
            foreach (var match in regexMatches)
            {
                cleanID = match.ToString().Split(':')[1].Replace("\"", "").Trim();
                stationNames.Add(cleanID);
            }

            using (WebClient wc = new WebClient())
            {
                foreach (var name in stationNames)
                {
                    Console.WriteLine("Now reading station: " + name + "...");
                    stationUrl = string.Format("http://environment.data.gov.uk/flood-monitoring/id/stations/{0}/readings?_sorted&startdate={1}&enddate={2}", name, dateLastWeek, dateToday);
                    jsonTemp = wc.DownloadString(stationUrl);

                    stationMeasures = JsonConvert.DeserializeObject<MeasuresJsonResult>(jsonTemp);
 
                    if (stationMeasures.Items.Count() == 0)
                    {
                        Console.WriteLine("No measure-items found in JSON response for this station.");
                        Console.WriteLine();
                        Console.WriteLine();
                        continue;
                    }

                    minItem = stationMeasures.getMinFirstMeasure();
                    maxItem = stationMeasures.getMaxLastMeasure();
                    averageRiverLevel = stationMeasures.getMeanRiverLevel();

                    PrintResults(minItem, maxItem, averageRiverLevel);
                }
            }

            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadKey();
        }

        //Helper function to move a lot of writelines out of the main
        public static void PrintResults(MeasureItem min, MeasureItem max, double mean)
        {
            Console.WriteLine("Minimum level:    " + min.value);
            Console.WriteLine("First occurrence: " + min.dateTime);
            Console.WriteLine("Maximum level:    " + max.value);
            Console.WriteLine("Last occurrence:  " + max.dateTime);
            Console.WriteLine("Average level:    " + mean);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
