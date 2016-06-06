using System;
using System.Collections.Generic;
using System.Linq;

namespace RiverChecker
{
    // Class for receiving parsed JSON data for stations on a specified river (this is not a complete model)
    internal class StationsJsonResult
    {
        public List<StationItem> items{ get; set; }
    }
}