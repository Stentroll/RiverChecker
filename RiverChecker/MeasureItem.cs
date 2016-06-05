using System;

namespace RiverChecker
{
    //Class for parsing JSON measurement objects
    internal class MeasureItem
    {
        public string id { get; set; }
        public DateTime dateTime { get; set; }
        public string measure { get; set; }
        public double value { get; set; }
    }
}