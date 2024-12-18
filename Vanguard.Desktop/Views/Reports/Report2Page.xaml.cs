using Vanguard.Desktop.ViewModels.Reports;

namespace Vanguard.Desktop.Views.Reports;
public sealed partial class Report2Page : Page
{
    public Report2PageViewModel ViewModel { get; set; }

    public Report2Page()
    {
        ViewModel = App.GetService<Report2PageViewModel>();
        this.InitializeComponent();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        DispatcherQueue.TryEnqueue(async () => await ViewModel.LoadAsync());
    }
}
