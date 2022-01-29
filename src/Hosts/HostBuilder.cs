using Desapt.CLI.Exceptions;
using Desapt.CLI.Hosts.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Desapt.CLI.Hosts;

internal class HostBuilder : IHostBuilder
{
    private readonly IServiceCollection _services;

    public HostBuilder(
        IServiceCollection services)
    {
        _services = services;
    }

    public IHostBuilder UseEntrypoint<TEntrypoint>()
        where TEntrypoint : IEntrypoint
    {
        var entrypointType = typeof(TEntrypoint);
        var containsEntrypoint = entrypointType.IsClass
            && !entrypointType.IsAbstract;

        if (!containsEntrypoint)
            throw new EntrypointNotFoundException(typeof(TEntrypoint));

        _services.AddSingleton(typeof(IEntrypoint), entrypointType);

        return this;
    }

    public IHostBuilder ConfigureServices(Action<IServiceCollection> config)
    {
        config.Invoke(_services);
        return this;
    }

    public Task RunAsync(string[] args)
    {
        return _services
            .BuildServiceProvider()
            .GetRequiredService<IEntrypoint>()
            .RunAsync(args);
    }
}
