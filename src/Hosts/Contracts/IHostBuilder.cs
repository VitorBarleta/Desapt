using Microsoft.Extensions.DependencyInjection;

namespace Desapt.CLI.Hosts.Contracts;

internal interface IHostBuilder
{
    IHostBuilder UseEntrypoint<TEntrypoint>()
        where TEntrypoint : IEntrypoint;

    IHostBuilder ConfigureServices(Action<IServiceCollection> services);

    Task RunAsync(string[] args);
}
