using FreshnessDeliveryBot.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<FreshnessBotDbContext>(options =>
            options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

    }
}
