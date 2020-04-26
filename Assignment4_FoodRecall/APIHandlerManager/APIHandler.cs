using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FoodRecall_Group11.Models;
using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace FoodRecall_Group11.APIHandlerManager
{
    public class APIHandler
    {
        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.
        //https://open.fda.gov/apis/food/enforcement/how-to-use-the-endpoint/

        static string BASE_URL = "https://api.fda.gov/food/enforcement.json";
        static string API_KEY = "6j8sUbS7FY6XyK5p7EZ4Bg7laJISv1PGVuQwZ8Of"; //Add your API key here inside ""

        HttpClient httpClient;

        /// <summary>
        ///  Constructor to initialize the connection to the data source
        /// </summary>
        public APIHandler()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Results> GetData()
        {
            string API_PATH = BASE_URL + "?limit=100";
            string apiData = "";

            RootObject data = null;

            httpClient.BaseAddress = new Uri(API_PATH);

            // It can take a few requests to get back a prompt response, if the API has not received
            //  calls in the recent past and the server has put the service on hibernation
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    apiData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!apiData.Equals(""))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;

                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    data = JsonConvert.DeserializeObject<RootObject>(apiData, settings);
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return data.results.ToList();
        }
    }
}
