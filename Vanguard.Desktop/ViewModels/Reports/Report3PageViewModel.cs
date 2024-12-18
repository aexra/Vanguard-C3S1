using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.ViewModels.Reports;
public partial class Report3PageViewModel : ObservableObject
{
    public ObservableCollection<Report3> Reports { get; set; } = new();

    public async Task LoadAsync()
    {
        Reports.Clear();

        var response = await ApiHelper.GetReport(3);

        if (response != null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<List<Report3>>();

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
