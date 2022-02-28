namespace Todo.Git.Commands;

public class GitAddCommand : GitCommandBase
{
    public string Path { get; }

    public GitAddCommand(string path)
    {
        Path = path;
    }

    internal override string GetCommand() => $"add {Path}";
}
