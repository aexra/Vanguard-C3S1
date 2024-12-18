using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.Views;
public sealed partial class AllContractsPage : Page
{
    public AllContractsPageViewModel ViewModel { get; set; }

    public AllContractsPage()
    {
        ViewModel = App.GetService<AllContractsPageViewModel>();
        this.InitializeComponent();

        DispatcherQueue.TryEnqueue(async () => await ViewModel.LoadAsync());
    }

    private async void UpdateContractsListBtn_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }

    private void CreateContractBtn_Click(object sender, RoutedEventArgs e)
    {

    }

    private async void DeleteContractMFI_Click(object sender, RoutedEventArgs e)
    {
        var menuFlyoutItem = sender as MenuFlyoutItem;

        if (menuFlyoutItem?.DataContext is Contract c)
        {
            await ViewModel.DeleteAsync(c);
        }
    }

    private void EditContractMFI_Click(object sender, RoutedEventArgs e)
    {
        var menuFlyoutItem = sender as MenuFlyoutItem;

        if (menuFlyoutItem?.DataContext is Contract c)
        {

        }
    }
}
