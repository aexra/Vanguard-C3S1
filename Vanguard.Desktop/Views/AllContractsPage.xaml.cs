namespace Vanguard.Desktop.Views;
public sealed partial class AllContractsPage : Page
{
    public AllContractsPageViewModel ViewModel { get; set; }

    public AllContractsPage()
    {
        ViewModel = App.GetService<AllContractsPageViewModel>();
        this.InitializeComponent();
    }
}
