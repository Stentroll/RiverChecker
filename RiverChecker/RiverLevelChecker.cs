using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace RiverChecker
{
    internal class RiverLevelChecker
    {
        private string riverName;

        // Constructor, takes the name of the river as input
        public RiverLevelChecker(string name)
        {
            this.riverName = name;
        }

        // Main function for program execution
        internal void Run()
        {
            List<StationItem> stationsList;
            List<MeasureItem> measuresList;
            MeasureItem minItem;
            MeasureItem maxItem;
            double averageRiverLevel;
            MeasuresProcessor processor = new MeasuresProcessor();

            // Get and parse data for stations on a specified river
            Console.WriteLine("Getting stations for river {0}", riverName);
            stationsList = GetStationsForRiver(riverName);

            Console.WriteLine("Found stations:");
            foreach (var station in stationsList)
            {
                Console.WriteLine(station.stationReference);
            }
            Console.WriteLine();

            // For each station in the result, get the measurements and process
            foreach (var station in stationsList)
            {
                measuresList = GetMeasuresForStation(station);

                // If there are measures in the parsed data then process, else continue to next station
                if (measuresList.Count > 0)
                {
                    minItem = processor.getMinFirstMeasure(measuresList);
                    maxItem = processor.getMaxLastMeasure(measuresList);
                    averageRiverLevel = processor.getMeanRiverLevel(measuresList);

                    PrintResults(minItem, maxItem, averageRiverLevel);
                }
                else
                {
                    Console.WriteLine("No measure-items found in JSON response for this station. \n\n");
                    continue;
                }
            }
        }

        // Use Webclient to get the JSON response from the API for stations on river, 
        // then use Newtonsoft.Json to parse into a StationsJsonResult object
        private static List<StationItem> GetStationsForRiver(string riverName)
        {
            StationsJsonResult stationResult;

            List<string> stationNames = new List<string>();
            string jsonTemp;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string riverStationsUrl = string.Format("http://environment.data.gov.uk/flood-monitoring/id/stations?riverName={0}", riverName);
                    jsonTemp = wc.DownloadString(riverStationsUrl);
                    stationResult = JsonConvert.DeserializeObject<StationsJsonResult>(jsonTemp);
                    return stationResult.items;
                }
                catch (WebException e)
                {
                    Console.WriteLine("Getting data from API has failed with message:");
                    Console.WriteLine(e.Message);
                    return null;
                }

            }

        }

        // Use Webclient to get the JSON response from the API for measurements for a station, 
        // then use Newtonsoft.Json to parse into a MeasuresJsonResult object
        private static List<MeasureItem> GetMeasuresForStation(StationItem station)
        {
            string dateFormat = "yyyy-MM-dd";
            // Get start and enddate for data, today and 6 days ago
            string dateToday = DateTime.Today.ToString(dateFormat);
            string dateLastWeek = DateTime.Today.AddDays(-6.0d).ToString(dateFormat);

            using (WebClient wc = new WebClient())
            {
                Console.WriteLine("Now reading station: {0} ({1}) ...", station.stationReference, station.town);
                string stationUrl = string.Format("http://environment.data.gov.uk/flood-monitoring/id/stations/{0}/readings?_sorted&startdate={1}&enddate={2}", station.stationReference, dateLastWeek, dateToday);

                try
                {
                    string jsonTemp = wc.DownloadString(stationUrl);
                    MeasuresJsonResult stationMeasures = JsonConvert.DeserializeObject<MeasuresJsonResult>(jsonTemp);
                    return stationMeasures.items;
                }
                catch (WebException e)
                {
                    Console.WriteLine("Getting data from API has failed with message:");
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        // Helper function to move a lot of writelines out of the main program
        public static void PrintResults(MeasureItem min, MeasureItem max, double mean)
        {
            Console.WriteLine("Minimum level:        " + min.value);
            Console.WriteLine("First min occurrence: " + min.dateTime);
            Console.WriteLine("Maximum level:        " + max.value);
            Console.WriteLine("Last max occurrence:  " + max.dateTime);
            Console.WriteLine("Average level:        " + mean.ToString("F3"));
            Console.WriteLine("\n\n");
        }
    }
}