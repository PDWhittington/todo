namespace Todo.Git.Commands;

public class GitAddCommand : GitSingleCommandBase
{
    public string Path { get; }

    public GitAddCommand(string path)
    {
        Path = path;
    }

    protected override string SingleCommand()  => $"add {Path}";
}
