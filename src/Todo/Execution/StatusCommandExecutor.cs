using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class StatusCommandExecutor(IOutputWriter outputWriter) 
    : CommandExecutorBase<StatusCommand>(outputWriter), IStatusCommandExecutor 
{
    public override void Execute(StatusCommand command)
    {
        OutputWriter.WriteLine("There is no todo command called 'status'.");
        OutputWriter.WriteLine("Did you mean 'git status'?");
    }
}