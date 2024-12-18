using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.ViewModels;
public partial class MyCrewCallsPageViewModel : ObservableObject
{
    public ObservableCollection<CrewCall> Calls { get; set; } = new();

    [ObservableProperty]
    private string _id;

    public async Task LoadAsync()
    {
        Calls.Clear();

        

        if (!int.TryParse(Id, out var parsedId))
        {
            return;
        }
        else
        {
            var response = await ApiHelper.GetCrewCalls(parsedId);
            System.Diagnostics.Debug.WriteLine(response);
            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<CrewCall>>();

                if (content != null)
                {
                    foreach (var c in content)
                    {
                        Calls.Add(c);
                    }
                }
            }
        }
    }
}
