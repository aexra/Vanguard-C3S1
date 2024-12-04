using Vanguard.Desktop.Services;

namespace Vanguard.Desktop.Views;
public sealed partial class ConnectionSettingPage : Page
{
    public ConnectionSettingViewModel ViewModel { get; set; }
    public string BreadCrumbBarItemText { get; set; }

    private readonly YamlConfigService _yamlConfigService;

    public ConnectionSettingPage()
    {
        ViewModel = App.GetService<ConnectionSettingViewModel>();
        _yamlConfigService = App.GetService<YamlConfigService>();

        var appSettings = Task.Run(async () => await _yamlConfigService.LoadSettingsAsync()).Result;

        ViewModel.AdministratorKey = appSettings.AdministratorKey;
        ViewModel.YandexMapsApiKey = appSettings.YandexMapsApiKey;
        ViewModel.DatabaseHostIP = appSettings.DatabaseHostIP;
        ViewModel.DatabaseHostPort = appSettings.DatabaseHostPort;
        ViewModel.DatabaseName = appSettings.DatabaseName;
        ViewModel.DatabaseUsername = appSettings.DatabaseUsername;
        ViewModel.DatabasePassword = appSettings.DatabasePassword;

        this.InitializeComponent();
    }
}
