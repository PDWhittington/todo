using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

public class StatusCommandExecutor(IOutputWriter outputWriter) 
    : CommandExecutorBase<StatusCommand>(outputWriter), IStatusCommandExecutor 
{
    public override void Execute(StatusCommand command)
    {
        outputWriter.WriteLine("There is no todo command called 'status'.");
        outputWriter.WriteLine("Did you mean 'git status'?");
    }
}