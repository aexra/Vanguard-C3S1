using Vanguard.Services;

namespace Vanguard.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; }

    private readonly YamlConfigService _yamlConfigService;

    public SettingsPage()
    {
        _yamlConfigService = App.GetService<YamlConfigService>();
        ViewModel = App.GetService<SettingsViewModel>();

        this.InitializeComponent();
    }
}

