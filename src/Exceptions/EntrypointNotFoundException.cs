using System.Reflection;

namespace Desapt.CLI.Exceptions;

public class EntrypointNotFoundException : Exception
{
    private const string MessageTemplate = "A proper application entrypoint could not be found. Got \"{0}\".";

    public EntrypointNotFoundException(MemberInfo entrypointType) :
        base(string.Format(MessageTemplate, entrypointType))
    {
    }
}