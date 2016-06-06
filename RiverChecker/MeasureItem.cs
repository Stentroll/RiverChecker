using System;

namespace RiverChecker
{
    // Class for receiving parsed data from the individual measures items from the river levels API
    internal class MeasureItem
    {
        public string id { get; set; }
        public DateTime dateTime { get; set; }
        public string measure { get; set; }
        public double value { get; set; }
    }
}