using System.Net;
using System.Net.Http.Json;

using NLSL.SKS.Package.Blazor.Dtos;
using NLSL.SKS.Package.Blazor.Models;

namespace NLSL.SKS.Package.Blazor.Helper;

public class HttpHelper
{
    private readonly HttpClient _httpClient;
    public HttpHelper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TrackingInformation> GetTrackingInformation(string trackingNumber)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/parcel/{trackingNumber}");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            TrackingInformation? trackingInformation = await response.Content.ReadFromJsonAsync<TrackingInformation>();

            if (trackingInformation is not null)
                return trackingInformation;
        }

        return null;
    }

    public async Task<bool> ReportDelivery(string trackingNumber)
    {
        HttpResponseMessage response = await _httpClient.PostAsync($"/parcel/{trackingNumber}/reportDelivery", null);
        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> ReportHop(string trackingNumber, string hopCode)
    {
        HttpResponseMessage response = await _httpClient.PostAsync($"/parcel/{trackingNumber}/reportHop/{hopCode}", null);
        return response.StatusCode == HttpStatusCode.OK;
    }
    
    public async Task<(bool,string)> SubmitParcel(ParcelModel parcel)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/parcel", parcel);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return (true, await response.Content.ReadAsStringAsync());
        }
        else
        {
            return (false, "");
        }
    }
}