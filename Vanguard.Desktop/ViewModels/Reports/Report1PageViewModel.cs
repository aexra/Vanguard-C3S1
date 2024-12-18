using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.ViewModels.Reports;
public partial class Report1PageViewModel : ObservableObject
{
    public ObservableCollection<Report1> Reports { get; set; } = new();

    public async Task LoadAsync()
    {
        Reports.Clear();

        var response = await ApiHelper.GetReport(1);

        if (response != null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<List<Report1>>();

            if (content != null)
            {
                foreach (var r in content)
                {
                    Reports.Add(r);
                }
            }
        }
    }
}
