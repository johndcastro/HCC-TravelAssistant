using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using LuisBot.Objects;

namespace LuisBot.Controllers
{
    public class AirportFind
    {
        public static async Task<List<XElement>> GetAirportByState(string instate)
        {
            List<XElement> retobj = new List<XElement>();
            try
            {
                XElement xelement = XElement.Parse(Properties.Resources.Airports);
                var airports = from nm in xelement.Elements("airport")
                               where (string)nm.Element("state") == instate
                               select nm;
                foreach (XElement airport in airports)
                {
                    retobj.Add(airport);
                }

            }
            catch (Exception e)
            {
                Console.Write($"Failed with exception: {e.ToString()}");
            }
            return retobj;
        }

        public static async Task<List<airport>> GetAirportsRange(Double inlat, Double inlong, Double inrange)
        {
            List<airport> retobj = new List<airport>();
            Double maxlat = inlat + inrange / 69.17;
            Double minlat = inlat - (maxlat - inlat);
            Double maxlong = inlong + inrange / (Math.Cos(minlat * Math.PI / 180) * 69.17);
            Double minlong = inlong - (maxlong - inlong);
            try
            {
                XElement xelement = XElement.Parse(Properties.Resources.Airports);
                var airports = from nm in xelement.Elements("airport")
                               where (Double)nm.Element("latitude") >= minlat &&
                               (Double)nm.Element("latitude") <= maxlat &&
                               (Double)nm.Element("longitude") <= maxlong &&
                               (Double)nm.Element("longitude") >= minlong
                               select nm;
                foreach (XElement aport in airports)
                {
                    XmlSerializer s = new XmlSerializer(typeof(airport));

                    retobj.Add((airport)s.Deserialize(aport.CreateReader()));
                }
            }
            catch
            {

            }
            return retobj;
        }

    }
}