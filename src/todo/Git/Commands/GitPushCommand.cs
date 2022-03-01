namespace Todo.Git.Commands;

public class GitPushCommand : GitSingleCommandBase
{
    protected override string SingleCommand() => "push";
}
