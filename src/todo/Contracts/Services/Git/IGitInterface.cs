namespace Todo.Contracts.Services.Git;

public interface IGitInterface
{
    bool RunGitCommand(string command);
}