namespace Todo.Contracts.Services;

public interface IGitDependencyValidator
{
    bool IsGitPresent();
}