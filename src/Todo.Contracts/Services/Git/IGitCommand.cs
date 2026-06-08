namespace Todo.Contracts.Services.Git;

public interface IGitCommand<out T>
{
    T ExecuteCommand(IGitInterface gitInterface);
}