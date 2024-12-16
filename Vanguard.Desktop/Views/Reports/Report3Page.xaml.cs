using Vanguard.Desktop.ViewModels.Reports;

namespace Vanguard.Desktop.Views.Reports;
public sealed partial class Report3Page : Page
{
    public Report3PageViewModel ViewModel { get; set; }

    public Report3Page()
    {
        ViewModel = App.GetService<Report3PageViewModel>();
        this.InitializeComponent();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        DispatcherQueue.TryEnqueue(async () => await ViewModel.Load());
    }
}
