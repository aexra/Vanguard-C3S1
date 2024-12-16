using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.ViewModels.Reports;
public partial class Report2PageViewModel : ObservableObject
{
    public ObservableCollection<Report2> Reports { get; set; } = new();

    public async Task Load()
    {
        Reports.Clear();

        var response = await ApiHelper.GetReport(2);

        if (response != null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<List<Report2>>();

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
