using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace F_ListNameChecker
{
    public static class FlistApi
    {

        private static readonly HttpClient client = new HttpClient();
        public static Ticket GetApiTicket(string user, string pass)
        {
            string responseString = "";
            var values = new Dictionary<string, string>
            {
                { "account", user },
                { "password", pass },
                { "no_characters", "true"},
                { "no_friends", "true"},
                { "no_bookmarks", "true"}
            };
            var responseObj = SendRequestToFlist(values, @"https://www.f-list.net/json/getApiTicket.php");
            responseString = responseObj["ticket"].ToString();
            return new Ticket(user, responseString);
        }

        public static JObject GetCharacterInfo(string name, Ticket ticket)
        {
            var values = new Dictionary<string, string>
            {
                { "account", ticket.Name },
                { "ticket", ticket.Key },
                { "name", name}
            };
            return SendRequestToFlist(values, @"https://www.f-list.net/json/api/character-data.php");
        }

        public static JObject SendRequestToFlist(Dictionary<string, string> values, string url)
        {
            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync(url, content);
            var responseObj = (JObject)DeserializeObject(response.Result.Content.ReadAsStringAsync().Result.ToString());
            return responseObj;
        }
    }
}
