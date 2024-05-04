using Newtonsoft.Json;
using System.Net.Http;
using TourNhanh.DataAcess;

namespace TourNhanh.DataQuery
{
   /* public class TokenGoogle
    {
        //[JsonProperty(PropertyName = "error_description")]
        public string error_description { get; set; }
        //[JsonProperty(PropertyName = "name")]
        public string name { get; set; }
        //[JsonProperty(PropertyName = "email")]
        public string email { get; set; }
    }*/
    public class QueryData
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly AppDbContext? context;

        public bool loginAdmin(string email, string password)
        {
            if (email.Equals("admin@a.com") && password.Equals("tournhanh@123")) return true;
            return false;
        }

        public bool loginAdmin(string password)
        {
            if (password.Equals("tournhanh@123"))
            { return true; }
            else
                return false;
        }
        /*public static async Task<TokenGoogle> VerifyTokenGoogle(string token)
        {
            TokenGoogle json = new();
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}"))
            {
                requestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var t = JsonConvert.DeserializeObject<TokenGoogle>(await response.Content.ReadAsStringAsync());
                    if (t.error_description == null)
                    {
                        json.name = t.name;
                        json.email = t.email;
                    }
                    else json.error_description = t.error_description;
                }
            }
            return json;
        }*/
    }
}
