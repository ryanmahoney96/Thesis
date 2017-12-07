using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace citadel_wpf
{
    class EventOrdering
    {
        //Order Key: the index string used to determine where an event lies on a timeline
        //Each event is given an index. Index = string of n char from 0-9
        //(1.5) 1.6 1.7 1.8 1.9 1.91 (2)
        //TODO order keys ordered by 100s 
        //1000 1100 1200 1250 1275 1300

        //The last order key of all events
        public static string LatestEventOrderKey = "40";

        public static void Initialize()
        {
            FindLatestOrderKey();
            
        }

        private static void FindLatestOrderKey()
        {
            //TODO combine queries
            bool eventsPresent = (from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                  select c).Count() > 0 ? true : false;

            //if there are events in the list
            if (eventsPresent)
            {
                //all order keys
                var order_keys = from c in XMLParser.EventXDocument.Handle.Root.Descendants("event")
                                 select c.Element("order_key").Value;


                foreach (string o in order_keys)
                {
                    //if this key comes after the latest, reset the latest
                    if (o.CompareTo(LatestEventOrderKey) > 0)
                    {
                        LatestEventOrderKey = o;
                    }
                }
            }
        }

        //TODO if the event's order key is equal to the latest orderkey, reset the latest
        public static void EventDeleted(string orderKey)
        {
            if (orderKey.Equals(LatestEventOrderKey))
            {
                FindLatestOrderKey();
            }
        }

        public static string GetNewestOrderKey()
        {
            LatestEventOrderKey = GetKeyAfter(LatestEventOrderKey);
            return LatestEventOrderKey;
        }

        public static string GetKeyBetween(string beforeKey, string afterKey)
        {
            //TODO find middle-ish
            return afterKey;
        }

        //Get the key that occurs after the given key
        public static string GetKeyAfter(string key)
        {
            String newKey = key;
            if (newKey[0].CompareTo('9') == 0)
            {
                //check for 9s recursively
            }
            else if (newKey[newKey.Length - 1].CompareTo('9') == 0)
            {
                newKey = string.Concat(++newKey.ToCharArray()[0], newKey.Substring(1, newKey.Length - 2), '0');
            }
            else
            {
                newKey = string.Concat(newKey.Substring(0, newKey.Length - 1), ++newKey.ToCharArray()[newKey.Length - 1]);
            }

            //TODO fine tune incrementing

            return newKey;
        }

        //Get the key that occurs after the given key
        public static string GetKeyBefore(string key)
        {
            string newKey = key;
            if (key[0].CompareTo('9') >= 0)
            {

            }

            //TODO determine decrementing


            return newKey;
        }


    }
}
