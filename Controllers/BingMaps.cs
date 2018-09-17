using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BingMapsRESTToolkit;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LuisBot.Objects;


namespace LuisBot.Controllers
{
    public class PostAddress
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string GeoCoords { get; set; }
        public string Success { get; set; }
        public string ErrorException { get; set; }
        public string imagedata { get; set; }
    }

    class BingQuery
    {
        private static string bmkey = ConfigurationManager.AppSettings["BingMapsKey"];

        public static async Task<Location> GetGeoLoc(string inquery)
        {
            Location retad = new Location();

            var request = new GeocodeRequest()
            {
                Query = inquery,
                IncludeIso2 = true,
                IncludeNeighborhood = false,
                MaxResults = 3,
                BingMapsKey = bmkey
            };
            try
            {
                var response = await request.Execute();
                if (response != null &&
                        response.ResourceSets != null &&
                        response.ResourceSets.Length > 0 &&
                        response.ResourceSets[0].Resources != null &&
                        response.ResourceSets[0].Resources.Length > 0)
                {
                    var qresult = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;
                    retad = qresult;
                }
                else
                {
                    retad = null;
                }
            }
            catch 
            {
                retad = null;
            }

            return retad;
        }

        public static async Task<String> GetImageURL(Location inloc)
        {
            ImageryRequest retimg = new ImageryRequest();
            var request = new ImageryRequest()
            {
                CenterPoint = inloc.Point.GetCoordinate(),
                ZoomLevel = 14,
                ImagerySet = ImageryType.CanvasLight,
                Pushpins = new List<ImageryPushpin>()
                {
                    new ImageryPushpin()
                    {
                        Location = inloc.Point.GetCoordinate()
                    }
                },
                BingMapsKey = bmkey
            };
            return request.GetRequestUrl();

        }

        public static async Task<Location> GetRevGeo(Double inlong, Double inlat)
        {
            Location retad = new Location();
            Coordinate secoord = new Coordinate();
            secoord.Latitude = inlat;
            secoord.Longitude = inlong;

            var request = new ReverseGeocodeRequest()
            {
                IncludeIso2 = true,
                IncludeNeighborhood = false,
                BingMapsKey = bmkey,
                Point = secoord
            };
            try
            {
                var response = await request.Execute();
                if (response != null &&
                        response.ResourceSets != null &&
                        response.ResourceSets.Length > 0 &&
                        response.ResourceSets[0].Resources != null &&
                        response.ResourceSets[0].Resources.Length > 0)
                {
                    var qresult = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;
                    retad = qresult;
                }
                else
                {
                    retad = null;
                }
            }
            catch
            {
                retad = null;
            }

            return retad;
        }

        public static async Task<DistanceMatrix> GetDriveDistance(Location startloc, Location endloc)
        {
            DistanceMatrix retad = new DistanceMatrix();

            SimpleWaypoint sway = new SimpleWaypoint(startloc.Point.GetCoordinate());
            List<SimpleWaypoint> sways = new List<SimpleWaypoint>();
            sways.Add(sway);
            SimpleWaypoint dway = new SimpleWaypoint(endloc.Point.GetCoordinate());
            List<SimpleWaypoint> dways = new List<SimpleWaypoint>();
            dways.Add(dway);

            var request = new DistanceMatrixRequest()
            {
                Origins = sways,
                Destinations = dways,
                TravelMode = TravelModeType.Driving,
                BingMapsKey = bmkey,
            };
            try
            {
                var response = await request.Execute();
                if (response != null &&
                        response.ResourceSets != null &&
                        response.ResourceSets.Length > 0 &&
                        response.ResourceSets[0].Resources != null &&
                        response.ResourceSets[0].Resources.Length > 0)
                {
                    var qresult = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.DistanceMatrix;
                    retad = qresult;
                }
                else
                {
                    retad = null;
                }
            }
            catch
            {
                retad = null;
            }

            return retad;
        }
    }
        
 }