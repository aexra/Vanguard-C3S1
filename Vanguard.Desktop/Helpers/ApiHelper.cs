using System.Net.Http.Json;
using Vanguard.DataAccess.Models;

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

    public static async Task<HttpResponseMessage?> CreateContracts(params Contract[] contracts)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/contracts");
        request.Method = HttpMethod.Post;
        request.Content = JsonContent.Create(contracts);

        return await _sharedClient.SendAsync(request);
    }
    public static async Task<HttpResponseMessage?> GetContracts(int? id = null, bool isLegalEntity = false)
    {
        using var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;

        if (id == null)
        {
            // Возвращаем все контракты
            request.RequestUri = new Uri($"{_baseAddress}/contracts");
        }
        else
        {
            if (isLegalEntity)
            {
                // Возвращаем все контракты по идентификатору организации
                request.RequestUri = new Uri($"{_baseAddress}/contracts/organization={id}");
            }
            else
            {
                // Возвращаем все контракты по идентификатору заказчика
                request.RequestUri = new Uri($"{_baseAddress}/contracts/user={id}");
            }
        }

        return await _sharedClient.SendAsync(request);
    }
    public static async Task<HttpResponseMessage?> DeleteContracts(params int[] ids)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/contracts");
        request.Method = HttpMethod.Delete;
        request.Content = JsonContent.Create(ids);

        return await _sharedClient.SendAsync(request);
    }
    public static async Task<HttpResponseMessage?> UpdateContract(Contract contract)
    {
        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_baseAddress}/contracts");
        request.Method = HttpMethod.Put;
        request.Content = JsonContent.Create(contract);

        return await _sharedClient.SendAsync(request);
    }
}
