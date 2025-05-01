using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace TestSpot.Services
{
    public interface ISpotifyClient
    {
        Task<string> GetToken();
        Task<string> GetTopItems();
        Task<string> GetArtist(string id);
    }
    public class SpotifyClient : ISpotifyClient
    {
        private readonly HttpClient _httpClient;
        //Todo: configurationize this
        private readonly string ClientId = "a257496c12174eb4a05b28fea0459d1d";
        private readonly string ClientSecret = "d97e406811e14c909daf52714f4d3386";
        private readonly string AccessToken = "access_token";
        public SpotifyClient(HttpClient httpClient) { _httpClient = httpClient; }

        //This would be cool to store somewhere (in memory or file) and cache??
        public async Task<string> GetToken()
        {
            //You could organize this
            var dict = new Dictionary<string, string>();
            dict.Add("grant_type", "client_credentials");
            dict.Add("client_id", ClientId);
            dict.Add("client_secret", ClientSecret);

            var url = "https://accounts.spotify.com/api/token";

            var encodedContent = new FormUrlEncodedContent(dict);
            var resp = await _httpClient.PostAsync(url, encodedContent);
            var content = await resp.Content.ReadAsStringAsync();
            var jobject = JObject.Parse(content);

            var jsonToken = jobject[AccessToken];
            var accessToken = (string)jsonToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return accessToken;
        }

        public async Task<string> GetTopItems() //model
        {
            var url = "https://api.spotify.com/v1/me/top/artists?time_range=short_term&limit=10&offset=0"; //put config in object

            var resp = await _httpClient.GetAsync(url);
            
            var content = await resp.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> GetArtist(string id)
        {
            var url = "https://api.spotify.com/v1/artists/0TnOYISbd1XYRBk9myaseg";

            var resp = await _httpClient.GetAsync(url);

            var content = await resp.Content.ReadAsStringAsync();
            return content;
        }
    }
}
