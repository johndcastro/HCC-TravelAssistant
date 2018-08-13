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

namespace LuisBot.Controllers
{
    public class PerDiemResult
    {
        public string Postal { get; set; }
        public string Fiscal { get; set; }
        public string Meals { get; set; }
        public string Output { get; set; }
    }

    class PQuery
    {

        public static async Task<PerDiemResult> GetPPost(PerDiemResult GetPer)
        {
            PerDiemResult PResult = new PerDiemResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"https://inventory.data.gov/api/action/datastore_search?resource_id=8ea44bc4-22ba-4386-b84c-1494ab28964b&limit=1");
            string filters = string.Format("&filters={{\"FiscalYear\":\"{0}\",\"Zip\":\"{1}\"}}", GetPer.Fiscal, GetPer.Postal);
            sb.Append(filters);

            PResult.Postal = GetPer.Postal;
            PResult.Fiscal = GetPer.Fiscal;

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
                        PResult.Meals = jresult.result.records[0].Meals;
                        PResult.Output = "Successful";
                    }
                    else
                    {
                        PResult.Output = $"ErrorWebAPI with string {response.ToString()}";
                        PResult.Meals = "0.00";
                    }
                }
            }
            catch (Exception e)
            {
                PResult.Output = $"ErrorWebException with error {e.ToString()}";
                PResult.Meals = "0.00";
            }
            return PResult;
        }

    }

}