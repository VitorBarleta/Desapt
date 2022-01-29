using Desapt.CLI.Hosts;
using Desapt.CLI.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace Desapt.CLI;

public class Program
{
    private static Task Main(string[] args)
    {
        var builder = Host.CreateBuilder()
            .ConfigureServices(ConfigureServices)
            .UseEntrypoint<AppEntrypoint>();

        return builder.RunAsync(args);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.InstallServicesInOwnAssembly();
    }
}
