using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using LuisBot.Objects;
using BingMapsRESTToolkit;

namespace LuisBot.Controllers
{

    class PQuery
    {

        private static async Task<Double> GetMealPost(string fisc, string postal)
        {
            Double retres = 0.00;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"https://inventory.data.gov/api/action/datastore_search?resource_id=8ea44bc4-22ba-4386-b84c-1494ab28964b&limit=1");
            string filters = string.Format("&filters={{\"FiscalYear\":\"{0}\",\"Zip\":\"{1}\"}}", fisc, postal);
            sb.Append(filters);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(sb.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var resp = await response.Content.ReadAsStringAsync();
                        PerJson jresult = JsonConvert.DeserializeObject<PerJson>(resp);
                        retres = Convert.ToDouble(jresult.result.records[0].Meals);
                    }
                    else
                    {
                        retres = 9999.99;
                    }
                }
            }
            catch
            {
                retres = 8888.88;
            }
            return retres;
        }

        private static async Task<Double> GetMealCity(string fisc, string city, string state)
        {
            Double retres = 0.00;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"https://inventory.data.gov/api/action/datastore_search?resource_id=8ea44bc4-22ba-4386-b84c-1494ab28964b&limit=1");
            string filters = string.Format("&filters={{\"FiscalYear\":\"{0}\",\"State\":\"{1}\",\"City\":\"{2}\"}}", fisc, state, city);
            sb.Append(filters);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(sb.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var resp = await response.Content.ReadAsStringAsync();
                        PerJson jresult = JsonConvert.DeserializeObject<PerJson>(resp);
                        retres = Convert.ToDouble(jresult.result.records[0].Meals);
                    }
                    else
                    {
                        retres = 9999.99;
                    }
                }
            }
            catch
            {
                retres = 8888.88;
            }
            return retres;
        }

        private static async Task<Double> GetMealState(string fisc, string state)
        {
            Double retres = 0.00;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"https://inventory.data.gov/api/action/datastore_search?resource_id=8ea44bc4-22ba-4386-b84c-1494ab28964b&limit=1");
            string filters = string.Format("&filters={{\"FiscalYear\":\"{0}\",\"State\":\"{1}\"}}", fisc, state);
            sb.Append(filters);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(sb.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        var resp = await response.Content.ReadAsStringAsync();
                        PerJson jresult = JsonConvert.DeserializeObject<PerJson>(resp);
                        retres = Convert.ToDouble(jresult.result.records[0].Meals);
                    }
                    else
                    {
                        retres = 9999.99;
                    }
                }
            }
            catch
            {
                retres = 8888.88;
            }
            return retres;
        }

        public static async Task<Double> GetMealLoc(Address inadd)
        {
            Double retres = 0.00;
            string fiscal = DateTime.Now.Year.ToString();
            if (inadd.PostalCode != null)
            {
                retres = await GetMealPost(fiscal, inadd.PostalCode);
            }
            else if (inadd.Locality != null)
            {
                Double testcity = await GetMealCity(fiscal, inadd.Locality, inadd.AdminDistrict);
                if (testcity > 555.55)
                {
                    retres = await GetMealState(fiscal, inadd.AdminDistrict);
                }
                else
                {
                    retres = testcity;
                }
            }
            else
            {
                retres = await GetMealState(fiscal, inadd.AdminDistrict);
            }
            return retres;
        }

    }

}