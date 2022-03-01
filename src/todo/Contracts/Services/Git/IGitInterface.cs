using Todo.Git.Commands;

namespace Todo.Contracts.Services.Git;

public interface IGitInterface
{
    // ReSharper disable once UnusedMethodReturnValue.Global

    bool RunGitCommand<T>(T command) where T : GitCommandBase;

    bool RunSpecialGitCommand(string command);

    bool NoPager { get; set; }

}
