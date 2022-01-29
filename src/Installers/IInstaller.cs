using Microsoft.Extensions.DependencyInjection;

namespace Desapt.CLI.Installers;

public interface IInstaller
{
    void Install(IServiceCollection services);
}
