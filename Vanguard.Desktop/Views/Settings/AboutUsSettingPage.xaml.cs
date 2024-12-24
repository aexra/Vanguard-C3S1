namespace Vanguard.Desktop.Views;

public sealed partial class AboutUsSettingPage : Page
{
    public AboutUsSettingViewModel ViewModel { get; }

    public AboutUsSettingPage()
    {
        ViewModel = App.GetService<AboutUsSettingViewModel>();
        this.InitializeComponent();
    }

    private void GrowlButton1_Click(object sender, RoutedEventArgs e)
    {
        Growl.Info("Успешно", $"Контракт №{1} успешно создан для организации {"Envelope"}");
    }

    private void GrowlButton2_Click(object sender, RoutedEventArgs e)
    {

    }

    private void GrowlButton3_Click(object sender, RoutedEventArgs e)
    {

    }
}


