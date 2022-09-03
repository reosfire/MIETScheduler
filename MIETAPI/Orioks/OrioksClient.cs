using System;
using System.Net.Http;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using MIETAPI.Orioks.Models;

namespace MIETAPI.Orioks
{
    public class OrioksClient
    {
        public string? ApiToken { get; set; }

        private readonly Uri _baseAddress = new Uri("https://orioks.miet.ru/");
        private readonly HttpClient _httpClient;

        public OrioksClient(string? apiToken = null)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = _baseAddress
            };
            ApiToken = apiToken;
        }

        public async Task<string?> GetToken(string login, string password)
        {
            HttpRequestMessage getTokenRequest = new HttpRequestMessage();
            getTokenRequest.Headers.Add("Accept", "application/json");
            getTokenRequest.Headers.Add("Authorization", $"Basic {GetEncodedAuth(login, password)}");
            getTokenRequest.Headers.Add("User-Agent", GetUserAgent());
            getTokenRequest.RequestUri = new Uri("api/v1/auth", UriKind.Relative);

            HttpResponseMessage response = await _httpClient.SendAsync(getTokenRequest);

            if (!response.IsSuccessStatusCode) throw new Exception($"Authentication failed with code: {response.StatusCode}");

            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);

            JsonElement responseJson = JsonDocument.Parse(responseString).RootElement;
            if (responseJson.TryGetProperty("error", out JsonElement error)) throw new Exception($"Authentication failed with error: {error.GetString()}");

            return responseJson.GetProperty("token").GetString();
        }

        private string GetEncodedAuth(string login, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
        }
        private string GetUserAgent()
        {
            string appName = Assembly.GetCallingAssembly().GetName().Name;
            string appVersion = Assembly.GetCallingAssembly().GetName().Version.ToString();
            string os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

            return $"{appName}/{appVersion} {os}";
        }
        private HttpRequestMessage CreateRequest(string uri)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {ApiToken}");
            request.Headers.Add("User-Agent", GetUserAgent());
            request.RequestUri = new Uri(uri, UriKind.Relative);
            return request;
        }
        private async Task<JsonElement> GetJson(string uri)
        {
            HttpResponseMessage response = await _httpClient.SendAsync(CreateRequest(uri));
            return JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        }

        private void CheckToken()
        {
            if (ApiToken is null) throw new Exception("Token not set, please set it");
        }

        public async Task<SemesterInfo> GetSemesterInfo()
        {
            CheckToken();
            
            return new SemesterInfo(await GetJson("api/v1/schedule"));
        }

        public async Task<Group[]> GetGroups()
        {
            CheckToken();

            JsonElement responseJson = await GetJson("api/v1/schedule/groups");

            return responseJson.EnumerateArray().Select(e => new Group(e)).ToArray();
        }

        public async Task<Pair[]> GetPairsTimings()
        {
            CheckToken();
            
            JsonElement responseJson = await GetJson("/api/v1/schedule/timetable");

            return responseJson.EnumerateObject().Select(e => new Pair(e)).ToArray();
        }

        public async Task<Schedule> GetSchedule(string groupId)
        {
            CheckToken();
            
            JsonElement responseJson = await GetJson($"/api/v1/schedule/groups/{groupId}");
            
            Console.WriteLine(responseJson);
            return null;
        }
    }
}
