using Desapt.CLI.Hosts.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Desapt.CLI.Hosts;

internal static class Host
{
    public static IHostBuilder CreateBuilder()
    {
        var services = CreateServices();

        return new HostBuilder(services);
    }

    private static IServiceCollection CreateServices()
    {
        return new ServiceCollection();
    }
}
