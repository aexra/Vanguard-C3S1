namespace Vanguard.Desktop.Services;

using YamlDotNet.Serialization;

public partial class AppSettings
{
    public string AdministratorKey { get; set; }
    public string YandexMapsApiKey { get; set; }
    public string DatabaseHostIP { get; set; }
    public string DatabaseHostPort { get; set; }
    public string DatabaseName { get; set; }
    public string DatabaseUsername { get; set; }
    public string DatabasePassword { get; set; }
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
            //System.Diagnostics.Debug.WriteLine("appsettings.yml: " + yaml);
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

