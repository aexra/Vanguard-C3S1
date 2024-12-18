using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.ViewModels;
public partial class MyContractsPageViewModel : ObservableObject
{
    public ObservableCollection<Contract> Contracts { get; set; } = new();

    [ObservableProperty]
    private string _id;

    [ObservableProperty]
    private bool _isLegalEntity;

    public async Task LoadAsync()
    {
        Contracts.Clear();

        if (!int.TryParse(Id, out var parsedId))
        {
            return;
        }
        else
        {
            var response = await ApiHelper.GetContracts(parsedId, IsLegalEntity);

            if (response != null && response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<Contract>>();

                if (content != null)
                {
                    foreach (var c in content)
                    {
                        Contracts.Add(c);
                    }
                }
            }
        }
    }
}
