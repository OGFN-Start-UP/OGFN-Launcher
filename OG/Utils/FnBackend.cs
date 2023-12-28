using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace og.Utils
{
    public class FnBackend
    {
        internal class Endpoints
        {
            public static readonly Uri Base = new Uri("http://localhost:8080/");
            public static readonly Uri Api = new Uri("http://localhost:3001");

            public static readonly Uri Token = new Uri(Base, "account/api/oauth/token");
            public static readonly Uri Shop = new Uri(Api, "V1/shop");
            public static readonly Uri exchnageToken = new Uri(Base, "/account/api/oauth/exchange");


            public static Uri Profile(string id) => new Uri(Base, $"fortnite/api/game/v2/profile/{id}/client/QueryProfile?profileId=athena&rvn=-1");
        }

        public static async Task<AccountStorage> Login(string email, string password)
        {
            var client = new RestClient();
            var request = new RestRequest(Endpoints.Token, Method.Post)
                .AddHeader("authorization", "basic ZWM2ODRiOGM2ODdmNDc5ZmFkZWEzY2IyYWQ4M2Y1YzY6ZTFmMzFjMjExZjI4NDEzMTg2MjYyZDM3YTEzZmM4NGQ=")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("grant_type", "password")
                .AddParameter("username", email)
            .AddParameter("password", password);

            Logger.Log($"Making a Post Request to {Endpoints.Token} type of {typeof(AccountStorage).Name}");

            var response = await client.ExecuteAsync(request);
            // Logger.Log($"{response.Content}", LogLevel.Debug);

            return JsonConvert.DeserializeObject<AccountStorage>(response.Content);
        }

        public static async Task<ItemShopStorage> GetShop()
        {
            var client = new RestClient();
            var request = new RestRequest(Endpoints.Shop, Method.Get)
                .AddHeader("authorization", Constants.ApiAuth);

            Logger.Log($"Making a Get Request to {Endpoints.Token} type of {typeof(ItemShopStorage).Name}");

            var response = await client.ExecuteAsync(request);
            // Logger.Log($"{response.Content}", LogLevel.Debug);

            return JsonConvert.DeserializeObject<ItemShopStorage>(response.Content);
        }

        public static async Task<exchange> GetExchange()
        {
            string accessToken = "bearer " + Globals.AccountStorage.AccessToken;
            var client = new RestClient();
            var request = new RestRequest(Endpoints.exchnageToken, Method.Get)
                .AddHeader("authorization", accessToken);
            Logger.Log(accessToken, LogLevel.Debug);

            Logger.Log($"Making a Get Request to {Endpoints.Token} type of {typeof(exchange).Name}");

            var response = await client.ExecuteAsync(request);
            Logger.Log($"{response.Content}", LogLevel.Debug);

            return JsonConvert.DeserializeObject<exchange>(response.Content);
        }

        /*public static async Task<string> GetCharacter(string id, string bearer)
        {
            var client = new RestClient();
            var request = new RestRequest(Endpoints.Profile(id), Method.Post)
                .AddHeader("authorization", $"bearer {bearer}");

            Logger.Log($"Making a Post Request to {Endpoints.Profile(id)} type of {typeof(Athena).Name}");

            var response = await client.ExecuteAsync(request);

            var athenaData = JsonConvert.DeserializeObject<Athena>(response.Content);

            return athenaData.ProfileChanges[0].Profile.Items.LawinLoadOut.Attributes.LockerSlotsData.Slots.Character.Items[0].Replace("AthenaCharacter:", "");
        }

        public static async Task<FKaedeApi> GetIcon(string cid)
        {
            var client = new RestClient();
            var request = new RestRequest($"https://fortnite-api.com/images/cosmetics/br/{cid}/icon.png", Method.Get);

            Logger.Log($"Making a Get Request to {($"https://fortnite-api.com/images/cosmetics/br/{cid}/icon.png")}");

            var response = await client.ExecuteAsync(request);

            var characterData = JsonConvert.DeserializeObject<FKaedeApi>(response.Content);

            return characterData;
        }*/
    }
}
