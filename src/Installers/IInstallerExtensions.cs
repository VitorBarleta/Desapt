using Microsoft.Extensions.DependencyInjection;

namespace Desapt.CLI.Installers;

public static class InstallerExtensions
{
    public static void InstallServicesInOwnAssembly(this IServiceCollection services)
    {
        var installerImplementations = typeof(InstallerExtensions).Assembly.ExportedTypes
            .Where(et => et.IsClass && !et.IsAbstract && et.IsAssignableFrom(typeof(IInstaller)))
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>()
            .ToList();
        
        installerImplementations.ForEach(i => i.Install(services));
    }
}