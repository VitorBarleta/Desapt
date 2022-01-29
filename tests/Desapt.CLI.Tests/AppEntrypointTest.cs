using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desapt.CLI.Args;
using Desapt.CLI.Tests.Helpers;
using Moq;
using Xunit;

namespace Desapt.CLI.Tests;

public class AppEntrypointTest
{
    private readonly IList<IArgExecutor> _argExecutors;

    private readonly AppEntrypoint _subject;

    public AppEntrypointTest()
    {
        _argExecutors = new List<IArgExecutor>();
        
        _subject = new AppEntrypoint(_argExecutors);
    }
    
    [Fact]
    public async Task RunAsync_Should_Check_Only_The_Executors_In_Which_The_Criteria_Is_Met()
    {
        var args = Array.Empty<string>();

        var argExecutor1 = new Mock<IArgExecutor>();
        var argExecutor2 = new Mock<IArgExecutor>();

        RegisterArgExecutors(argExecutor1, argExecutor2);

        argExecutor1
            .Setup(ae => ae.Meet(args))
            .Returns(false)
            .Verifiable();

        argExecutor2
            .Setup(ae => ae.Meet(args))
            .Returns(false)
            .Verifiable();
        
        await _subject.RunAsync(args);

        argExecutor1.Verify();
        argExecutor2.Verify();
    }

    [Fact]
    public async Task RunAsync_Should_Check_Only_The_Executors_In_Which_The_Criteria_Is_Met_In_Priority_Order()
    {
        var args = Array.Empty<string>();

        var argExecutor1 = new Mock<IArgExecutor>();
        var argExecutor2 = new Mock<IArgExecutor>();
        var argExecutor3 = new Mock<IArgExecutor>();

        RegisterArgExecutors(argExecutor1, argExecutor2, argExecutor3);

        const ushort argExecutor1ExpectedPriority = 2;
        const ushort argExecutor2ExpectedPriority = 0;
        const ushort argExecutor3ExpectedPriority = 1;

        var priorityHelper = new PriorityHelper();

        argExecutor1
            .SetupGet(ae => ae.Priority)
            .Returns(argExecutor1ExpectedPriority);

        argExecutor1
            .Setup(ae => ae.Meet(args))
            .Returns(false)
            .Callback(() => priorityHelper.Check(argExecutor1ExpectedPriority))
            .Verifiable();
            
        argExecutor2
            .SetupGet(ae => ae.Priority)
            .Returns(argExecutor2ExpectedPriority);

        argExecutor2
            .Setup(ae => ae.Meet(args))
            .Returns(false)
            .Callback(() => priorityHelper.Check(argExecutor2ExpectedPriority))
            .Verifiable();
            
        argExecutor3
            .SetupGet(ae => ae.Priority)
            .Returns(argExecutor3ExpectedPriority);

        argExecutor3
            .Setup(ae => ae.Meet(args))
            .Returns(false)
            .Callback(() => priorityHelper.Check(argExecutor3ExpectedPriority))
            .Verifiable();
        
        await _subject.RunAsync(args);

        argExecutor1.Verify();
        argExecutor2.Verify();
    }

    [Fact]
    public async Task RunAsync_Should_Execute_Only_The_Arg_Executors_That_Met_The_Criteria()
    {
        var args = Array.Empty<string>();

        var argExecutor1 = new Mock<IArgExecutor>();
        var argExecutor2 = new Mock<IArgExecutor>();

        RegisterArgExecutors(argExecutor1, argExecutor2);

        argExecutor1
            .Setup(ae => ae.Meet(args))
            .Returns(false);

        argExecutor2
            .Setup(ae => ae.Meet(args))
            .Returns(true);
        
        await _subject.RunAsync(args);
        
        argExecutor1.Verify(ae => ae.ExecuteAsync(), Times.Never);
        argExecutor2.Verify(ae => ae.ExecuteAsync(), Times.Once);
    }

    private void RegisterArgExecutors(params Mock<IArgExecutor>[] argExecutorMocks)
    {
        foreach (var argExecutorMock in argExecutorMocks)
        {
            _argExecutors.Add(argExecutorMock.Object);
        }
    }
}