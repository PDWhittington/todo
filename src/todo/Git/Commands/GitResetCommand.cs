namespace Todo.Git.Commands;

public class GitResetCommand : GitSingleCommandBase
{
    public bool Hard { get; }

    public GitResetCommand(bool hard = false)
    {
        Hard = hard;
    }

    protected override string SingleCommand() => Hard ? "reset --hard" : "reset";
}
