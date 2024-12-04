namespace Vanguard.Desktop.ViewModels;
public partial class ConnectionSettingViewModel : ObservableObject
{
    [ObservableProperty]
    public string administratorKey;

    [ObservableProperty]
    public string yandexMapsApiKey;

    [ObservableProperty]
    public string databaseHostIP;

    [ObservableProperty]
    public string databaseHostPort;

    [ObservableProperty]
    public string databaseName;

    [ObservableProperty]
    public string databaseUsername;

    [ObservableProperty]
    public string databasePassword;
}
