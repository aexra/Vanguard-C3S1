using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vanguard.Web.Data.Models;
using Vanguard.Web.Services;

namespace Vanguard.Web.Data.Contexts;

public class IdentityContext : IdentityDbContext<User>
{
    //private readonly YamlConfigService _yamlConfigService;

    //public IdentityContext(YamlConfigService yamlConfigService)
    //{
    //    _yamlConfigService = yamlConfigService;
    //}

    public IdentityContext()
    {
        
    }

    public async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        
    }

    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //var conf = await _yamlConfigService.LoadSettingsAsync();

        //optionsBuilder.UseNpgsql($"Host={conf.DatabaseHostIP};Port={conf.DatabaseHostPort};Database={conf.DatabaseName};Username={conf.DatabaseUsername};Password={conf.DatabasePassword};Include Error Detail=True");
        optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1234;Include Error Detail=True");
    }
}
