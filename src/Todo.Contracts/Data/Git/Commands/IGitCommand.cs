using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public interface IGitCommand<out T>
    where T : GitResultBase;