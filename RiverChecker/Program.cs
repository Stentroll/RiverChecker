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

            // Create and run main program component
            RiverLevelChecker rlc = new RiverLevelChecker(riverName);
            rlc.Run();

            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
