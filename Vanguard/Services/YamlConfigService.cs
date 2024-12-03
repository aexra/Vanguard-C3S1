namespace Vanguard.Services;

using YamlDotNet.Serialization;

public partial class AppSettings : ObservableObject
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

public class YamlConfigService
{
    private readonly string _filePath = "appsettings.yml";

    public AppSettings Settings => LoadSettingsAsync().Result;

    public async Task<AppSettings> LoadSettingsAsync()
    {
        using (var reader = new StreamReader(_filePath))
        {
            var yaml = await reader.ReadToEndAsync();
            var deserializer = new DeserializerBuilder().Build();
            System.Diagnostics.Debug.WriteLine("appsettings.yml: " + yaml);
            return deserializer.Deserialize<AppSettings>(yaml);
        }
    }

    public async Task SaveSettingsAsync(AppSettings settings)
    {
        var serializer = new SerializerBuilder().JsonCompatible().Build();
        var yaml = serializer.Serialize(settings);
        System.Diagnostics.Debug.WriteLine("appsettings.yml: " + yaml);
        await File.WriteAllTextAsync(_filePath, yaml);
    }
}

