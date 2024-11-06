using Vanguard.Services;

namespace Vanguard.ViewModels;
public partial class ConnectionSettingViewModel : ObservableObject
{
    [ObservableProperty]
    private AppSettings appSettings;
}
