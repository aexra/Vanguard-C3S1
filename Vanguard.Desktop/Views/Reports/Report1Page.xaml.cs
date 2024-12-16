using Vanguard.Desktop.ViewModels.Reports;

namespace Vanguard.Desktop.Views.Reports;
public sealed partial class Report1Page : Page
{
    public Report1PageViewModel ViewModel { get; set; }

    public Report1Page()
    {
        ViewModel = App.GetService<Report1PageViewModel>();
        this.InitializeComponent();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        DispatcherQueue.TryEnqueue(async () => await ViewModel.Load());
    }
}
