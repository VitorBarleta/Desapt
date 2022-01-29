namespace Desapt.CLI.Hosts.Contracts;

public interface IEntrypoint
{
    Task RunAsync(string[] args);
}
