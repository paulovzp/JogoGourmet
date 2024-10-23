using JogoGourmet.Persistence;
using JogoGourmet.UI;
using Microsoft.Extensions.DependencyInjection;

public class Program
{

    public static void Main(string[] args)
    {
        var provider = ConfigureServices();
        var appService = provider.GetService<IJogoGourmetAppService>();
        appService.Start();
    }


    public static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddScoped<JogoGourmetContext>();
        services.AddScoped<IJogoGourmetAppService, JogoGourmetAppService>();

        return services.BuildServiceProvider();
    }
}


