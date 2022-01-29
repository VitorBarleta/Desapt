using Desapt.CLI.Args;
using Desapt.CLI.Hosts.Contracts;

namespace Desapt.CLI;

public class AppEntrypoint : IEntrypoint
{
    private readonly IEnumerable<IArgExecutor> _argExecutors;

    public AppEntrypoint(IEnumerable<IArgExecutor> argExecutors)
    {
        _argExecutors = argExecutors;
    }

    public async Task RunAsync(string[] args)
    {
        var executors = _argExecutors
            .OrderBy(ae => ae.Priority)
            .Where(ae => ae.Meet(args));
        
        foreach (var executor in executors)
        {
            await executor.ExecuteAsync();
        }
    }
}
