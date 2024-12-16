using System.Net.Http.Json;
using Vanguard.DataAccess.Models;
using Windows.Storage;

namespace Vanguard.App.Helpers;
public class ApiHelper
{
    private static string _baseAddress = "http://127.0.0.1:5277/api";
    private static HttpClient _sharedClient = new();
    private static string? _token => (string?)ApplicationData.Current.LocalSettings.Values["apiToken"];

    public static async Task<HttpResponseMessage?> CheckConnection()
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/user/check");
        request.Method = HttpMethod.Get;
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        return await _sharedClient.SendAsync(request);
    }
    
    public static async Task<HttpResponseMessage?> Login(string name, string password)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/user/login");
        request.Method = HttpMethod.Post;
        request.Content = JsonContent.Create(new LoginUser() { UserName=name, Password=password });

        var response = await _sharedClient.SendAsync(request);

        return response;
    }
    public static async Task<HttpResponseMessage?> Register(string name, string password)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/user/register");
        request.Method = HttpMethod.Post;
        request.Content = JsonContent.Create(new RegisterUser() { UserName = name, Password = password });

        return await _sharedClient.SendAsync(request);
    }
    
    public static async Task<HttpResponseMessage?> Me()
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/user/me");
        request.Method = HttpMethod.Get;
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        return await _sharedClient.SendAsync(request);
    }
    public static async Task<HttpResponseMessage?> GetUserProfile(string id)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/user/profile/{id}");
        request.Method = HttpMethod.Get;
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        return await _sharedClient.SendAsync(request);
    }
    public static async Task<HttpResponseMessage?> GetReport(int report)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/report/{report}");
        request.Method = HttpMethod.Get;
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        return await _sharedClient.SendAsync(request);
    }
    public static async Task<HttpResponseMessage?> GetCrewCalls(int crewId)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/crew/{crewId}/calls");
        request.Method = HttpMethod.Get;
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

        return await _sharedClient.SendAsync(request);
    }
}
