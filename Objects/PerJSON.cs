using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LuisBot.Objects
{
    public class Field
    {

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }
    }

    public class Record
    {

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("Dec")]
        public string Dec { get; set; }

        [JsonProperty("Feb")]
        public string Feb { get; set; }

        [JsonProperty("Zip")]
        public string Zip { get; set; }

        [JsonProperty("Aug")]
        public string Aug { get; set; }

        [JsonProperty("Sep")]
        public string Sep { get; set; }

        [JsonProperty("Apr")]
        public string Apr { get; set; }

        [JsonProperty("Jun")]
        public string Jun { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Jul")]
        public string Jul { get; set; }

        [JsonProperty("Meals")]
        public string Meals { get; set; }

        [JsonProperty("County")]
        public string County { get; set; }

        [JsonProperty("May")]
        public string May { get; set; }

        [JsonProperty("DestinationID")]
        public string DestinationID { get; set; }

        [JsonProperty("Mar")]
        public string Mar { get; set; }

        [JsonProperty("Jan")]
        public string Jan { get; set; }

        [JsonProperty("LocationDefined")]
        public string LocationDefined { get; set; }

        [JsonProperty("Nov")]
        public string Nov { get; set; }

        [JsonProperty("_id")]
        public int _id { get; set; }

        [JsonProperty("Oct")]
        public string Oct { get; set; }

        [JsonProperty("FiscalYear")]
        public string FiscalYear { get; set; }
    }

    public class Links
    {

        [JsonProperty("start")]
        public string start { get; set; }

        [JsonProperty("next")]
        public string next { get; set; }
    }

    public class Filters
    {

        [JsonProperty("Zip")]
        public string Zip { get; set; }

        [JsonProperty("FiscalYear")]
        public string FiscalYear { get; set; }
    }

    public class Result
    {

        [JsonProperty("resource_id")]
        public string resource_id { get; set; }

        [JsonProperty("fields")]
        public IList<Field> fields { get; set; }

        [JsonProperty("records")]
        public IList<Record> records { get; set; }

        [JsonProperty("_links")]
        public Links _links { get; set; }

        [JsonProperty("filters")]
        public Filters filters { get; set; }

        [JsonProperty("limit")]
        public int limit { get; set; }

        [JsonProperty("total")]
        public int total { get; set; }
    }

    public class PerJson
    {

        [JsonProperty("help")]
        public string help { get; set; }

        [JsonProperty("success")]
        public bool success { get; set; }

        [JsonProperty("result")]
        public Result result { get; set; }
    }


}