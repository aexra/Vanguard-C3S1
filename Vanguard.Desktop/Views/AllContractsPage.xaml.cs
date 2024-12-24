using Vanguard.DataAccess.Models;
using Vanguard.Desktop.Controls;

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

    private async void CreateContractBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog();
        var content = new ContractFormContent();

        content.ViewModel.ContentTitle = "Новый контракт";

        dialog.Content = content;
        dialog.XamlRoot = this.XamlRoot;
        dialog.PrimaryButtonText = "Создать";
        dialog.CloseButtonText = "Отмена";
        dialog.DefaultButton = ContentDialogButton.Primary;

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var c = content.ViewModel.ToContract();

            if (c == null || c.IsLegalEntity && c.OrganizationId == null || !c.IsLegalEntity && c.OwnerId == null || c.Address == null)
            {
                Growl.Error("Операция отклонена", "Введенные данные содержат ошибку");
                return;
            }

            await ViewModel.CreateAsync(c);
        }
    }

    private async void DeleteContractMFI_Click(object sender, RoutedEventArgs e)
    {
        var menuFlyoutItem = sender as MenuFlyoutItem;

        if (menuFlyoutItem?.DataContext is Contract c)
        {
            await ViewModel.DeleteAsync(c);
        }
    }

    private async void EditContractMFI_Click(object sender, RoutedEventArgs e)
    {
        var menuFlyoutItem = sender as MenuFlyoutItem;

        if (menuFlyoutItem?.DataContext is Contract c)
        {
            var dialog = new ContentDialog();
            var content = new ContractFormContent();

            content.ViewModel.ContentTitle = "Редактировать контракт";

            content.ViewModel.FromContract(c);

            dialog.Content = content;
            dialog.XamlRoot = this.XamlRoot;
            dialog.PrimaryButtonText = "Применить";
            dialog.CloseButtonText = "Отмена";
            dialog.DefaultButton = ContentDialogButton.Primary;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var updatedContract = content.ViewModel.ToContract();

                if (updatedContract != null)
                {
                    updatedContract.ContractId = c.ContractId;
                    await ViewModel.UpdateAsync(updatedContract);
                }
            }
        }
    }
}
