namespace Desapt.CLI.Args;

public interface IArgExecutor
{
    ushort Priority { get; }

    bool Meet(string[] args);
    ValueTask ExecuteAsync();
}
