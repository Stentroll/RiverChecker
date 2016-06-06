namespace RiverChecker
{
    // Class for receiving parsed JSON data for individual station items from the station on river API call
    public class StationItem
    {
        public string riverName { get; set; }
        public string stationReference { get; set; }
        public string town { get; set; }

        //public string id { get; set; }
        //public string RLOIid { get; set; }
        //public string catchmentName { get; set; }
        //public string dateOpened { get; set; }
        //public string easting { get; set; }
        //public string label { get; set; }
        //public string lat { get; set; }
        //public string long {get; set; }
        //public string northing { get; set; }
        //public string notation { get; set; }
        //public string wiskiID { get; set; }
        //public string stageScale { get; set; }
}
}