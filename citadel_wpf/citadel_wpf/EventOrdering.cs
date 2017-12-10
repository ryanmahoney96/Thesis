using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class EventOrdering
    {
        //Order Key: Each event is given an index index used to determine where an event lies on a timeline

        //The last order key of all events
        public static int LatestEventOrderKey = 4000;

        public static void Initialize()
        {
            FindLatestOrderKey();
            
        }

        //find out if the xml has a more "recent" order key
        private static void FindLatestOrderKey()
        {
            //all order keys
            var order_keys = from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                             select int.Parse(c.Element("order_key").Value);

            bool eventsPresent = order_keys.Count() > 0 ? true : false;

            //if there are events in the list
            if (eventsPresent)
            {
                
                foreach (int o in order_keys)
                {
                    //if this key comes after the latest, reset the latest
                    if (o > LatestEventOrderKey)
                    {
                        LatestEventOrderKey = o;
                    }
                }
            }
        }

        public static void ReorderOrderKeys()
        {
            //grab every order key, sort them by value, match each old order key to its new equivalent
            List<string> listOfOrderKeys = new List<string>();

            var events = from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                             select c;

            foreach(var e in events)
            {
                listOfOrderKeys.Add(e.Element("order_key").Value);
            }

            listOfOrderKeys.Sort();

            Dictionary<string, int> orderKeyAssigner = new Dictionary<string, int>();
            LatestEventOrderKey = 4000;

            foreach(var l in listOfOrderKeys)
            {
                if (!orderKeyAssigner.ContainsKey(l))
                {
                    LatestEventOrderKey += 200;
                    orderKeyAssigner.Add(l, LatestEventOrderKey);
                }
            }

            foreach (var e in events)
            {
                e.Element("order_key").Value = orderKeyAssigner[e.Element("order_key").Value].ToString();
            }

            XMLParser.EventXDocument.Save();
        }

        public static int GetNewOrderKey()
        {
            LatestEventOrderKey = GetKeyAfter(LatestEventOrderKey);
            return LatestEventOrderKey;
        }

        public static int GetKeyBetween(int beforeKey, int afterKey)
        {
            //find middle, if it would be a fraction --> reorder after
            int average = (beforeKey + afterKey) / 2;

            if ( (beforeKey + afterKey) % 2 > 0)
            {
                average = beforeKey + 1;
                ReorderOrderKeys();
            }

            return average;
        }

        //Get the key that occurs after the given key
        public static int GetKeyAfter(int key)
        {
            int newKey = key + 200;
            
            return newKey;
        }

        //TODO negative?
        //Get the key that occurs after the given key
        public static int GetKeyBefore(int key)
        {
            int newKey = key - 200;

            return newKey;
        }


    }
}
