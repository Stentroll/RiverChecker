using System;
using System.Collections.Generic;
using System.Linq;

namespace RiverChecker
{
    internal class MeasuresJsonResult
    {
        public List<MeasureItem> Items{ get; set; }

        //Returns the item with the last occurence of the maximum value
        public MeasureItem getMaxLastMeasure()
        {
            //If there are no measures, return null
            if (Items.Count < 1)
            {
                Console.WriteLine("List of items empty!");
                return null;
            }

            //Find the highest value among items
            double maxValue = Items.Max(i => i.value);
            
            //Get all items with that value
            List<MeasureItem> maxItems = Items.FindAll(i => i.value == maxValue);

            //Sort items on date and return last one as we want most recent max value
            maxItems.Sort((a, b) => a.dateTime.CompareTo(b.dateTime));
            return maxItems.Last();
        }

        //Returns the item with the first occurence of the minimum value
        public MeasureItem getMinFirstMeasure()
        {
            //If there are no measures, return null
            if (Items.Count < 1)
            {
                Console.WriteLine("List of items empty!");
                return null;
            }

            //Find the lowest value among items
            double minValue = Items.Min(i => i.value);

            //Find all items with the lowest value
            List<MeasureItem> minItems = Items.FindAll(i => i.value == minValue);

            //Sort items on date and return the first one as we want the first occurence of min value
            minItems.Sort((a, b) => a.dateTime.CompareTo(b.dateTime));
            return minItems.First();
        }

        //Returns the average of all river level measurements in the Items list
        public double getMeanRiverLevel()
        {
            if (Items.Count < 1)
            {
                Console.WriteLine("List of items empty!");
                return 0.0;
            }

            return Items.Average(i => i.value);
        }
    }


}