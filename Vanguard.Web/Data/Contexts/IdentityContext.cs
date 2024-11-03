using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vanguard.Web.Data.Models;
using Vanguard.Web.Services;

namespace Vanguard.Web.Data.Contexts;

public class IdentityContext : IdentityDbContext<User>
{
    private readonly YamlConfigService _yamlConfigService;

    public IdentityContext(YamlConfigService yamlConfigService)
    {
        _yamlConfigService = yamlConfigService;
    }

    public async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        
    }

    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var conf = await _yamlConfigService.LoadSettingsAsync();
        optionsBuilder.UseNpgsql($"Host={conf.DatabaseHostIP};Port={conf.DatabaseHostPort};Database={conf.DatabaseName};Username={conf.DatabaseUsername};Password={conf.DatabasePassword};Include Error Detail=True");
        System.Diagnostics.Debug.WriteLine("IdentityContext configured");
    }
}
