using FluentAssertions;

namespace Desapt.CLI.Tests.Helpers;

public class PriorityHelper
{
    private ushort _priority;

    public PriorityHelper()
    {
        _priority = 0;
    }

    public void Check(ushort expected)
    {
        expected.Should().Be(_priority++);
    }
}
