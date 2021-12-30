namespace Todo.Contracts.Services;

public interface IGitInterface
{
    bool RunGitCommand(string command);
}