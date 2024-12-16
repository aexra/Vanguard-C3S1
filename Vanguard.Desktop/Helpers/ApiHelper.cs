namespace Vanguard.App.Helpers;
public class ApiHelper
{
    private static string _baseAddress = "http://127.0.0.1:5231/api";
    private static HttpClient _sharedClient = new();

    public static async Task<HttpResponseMessage?> GetReport(int report)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/report/{report}");
        request.Method = HttpMethod.Get;

        return await _sharedClient.SendAsync(request);
    }
    public static async Task<HttpResponseMessage?> GetCrewCalls(int crewId)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/crew/{crewId}/calls");
        request.Method = HttpMethod.Get;

        return await _sharedClient.SendAsync(request);
    }
}
