using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.ViewModels;
public partial class AllContractsPageViewModel : ObservableObject
{
    public ObservableCollection<Contract> Contracts { get; set; } = new();

    public async Task LoadAsync()
    {
        Contracts.Clear();

        var response = await ApiHelper.GetContracts();

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

    public async Task DeleteAsync(Contract c)
    {
        var response = await ApiHelper.DeleteContracts(c.ContractId);

        if (response != null && response.IsSuccessStatusCode)
        {
            Contracts.Remove(c);
        }
    }

    public async Task CreateAsync(Contract c)
    {
        var response = await ApiHelper.CreateContract(c);

        if (response != null && response.IsSuccessStatusCode)
        {
            Contracts.Add(c);
        }
    }

    public async Task UpdateAsync(Contract c)
    {
        var response = await ApiHelper.UpdateContract(c);

        if (response != null && response.IsSuccessStatusCode)
        {
            await LoadAsync();
        }
    }
}
