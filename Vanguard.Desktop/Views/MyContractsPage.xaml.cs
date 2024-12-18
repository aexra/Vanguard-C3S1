namespace Vanguard.Desktop.Views;
public sealed partial class MyContractsPage : Page
{
    public MyContractsPageViewModel ViewModel { get; set; }

    public MyContractsPage()
    {
        ViewModel = App.GetService<MyContractsPageViewModel>();
        this.InitializeComponent();
    }
}
