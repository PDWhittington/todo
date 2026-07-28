using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public record GitGetConflictsCommand : IGitCommand<ConflictsResult>
{
    public static readonly GitGetConflictsCommand Instance = new();

    private GitGetConflictsCommand() { }
}