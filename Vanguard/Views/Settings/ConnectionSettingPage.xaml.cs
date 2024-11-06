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

        //ViewModel.AppSettings = _yamlConfigService.Settings;

        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        BreadCrumbBarItemText = e.Parameter as string;
    }
}
