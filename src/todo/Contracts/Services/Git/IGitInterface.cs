namespace Todo.Contracts.Services.Git;

public interface IGitInterface
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    bool RunGitCommand(string command);
}
