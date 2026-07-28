using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

// ReSharper disable once UnusedTypeParameter
// we want to make sure that all git commands
// have a result that implements GitResultBase 
public interface IGitCommand<out T>
    where T : GitResultBase;