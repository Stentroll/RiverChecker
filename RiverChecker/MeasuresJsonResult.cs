using System;
using System.Collections.Generic;
using System.Linq;

namespace RiverChecker
{
    internal class MeasuresJsonResult
    {
        // Class for receiving parsed JSON data for the measures for a specified station (this is not a complete model)
        public List<MeasureItem> items{ get; set; }
    }
}