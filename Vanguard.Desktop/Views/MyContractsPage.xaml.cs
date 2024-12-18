namespace Vanguard.Desktop.Views;
public sealed partial class MyContractsPage : Page
{
    public MyContractsPageViewModel ViewModel { get; set; }

    public MyContractsPage()
    {
        ViewModel = App.GetService<MyContractsPageViewModel>();
        this.InitializeComponent();
    }

    private async void UpdateContractsListBtn_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
}
