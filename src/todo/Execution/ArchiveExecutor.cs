using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Git;

namespace Todo.Execution;

public class ArchiveExecutor : IArchiveExecutor
{
    private readonly IGitInterface _gitInterface;

    public ArchiveExecutor(IGitInterface gitInterface)
    {
        _gitInterface = gitInterface;
    }

    public void Execute(ArchiveCommand command)
    {
        
    }
}