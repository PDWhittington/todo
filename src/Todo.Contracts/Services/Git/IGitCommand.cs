namespace Todo.Contracts.Services.Git;

public interface IGitCommand<T>
{
    T ExecuteCommand(IGitInterface gitInterface);
}