using Vanguard.Services;

namespace Vanguard.Views;
public sealed partial class ConnectionSettingPage : Page
{
    public ConnectionSettingViewModel ViewModel { get; set; }
    public string BreadCrumbBarItemText { get; set; }

    private readonly YamlConfigService _yamlConfigService;

    public ConnectionSettingPage()
    {
        ViewModel = App.GetService<ConnectionSettingViewModel>();
        _yamlConfigService = App.GetService<YamlConfigService>();

        ViewModel.AppSettings = Task.Run(async () => await _yamlConfigService.LoadSettingsAsync()).Result;
        ViewModel.AppSettings.PropertyChanged += AppSettings_PropertyChanged;

        this.InitializeComponent();
    }

    private async void AppSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        await _yamlConfigService.SaveSettingsAsync(this.ViewModel.AppSettings);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        BreadCrumbBarItemText = e.Parameter as string;
    }
}
