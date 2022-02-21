using System.IO;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Git;

public class GitDependencyValidator : IGitDependencyValidator
{
    private readonly IGitExecutableResolver _gitExecutableResolver;

    private bool? _validated;

    public GitDependencyValidator(IGitExecutableResolver gitExecutableResolver)
    {
        _gitExecutableResolver = gitExecutableResolver;
    }

    public bool GitDependenciesValidated()
    {
        _validated ??= File.Exists(_gitExecutableResolver.GitPath);

        return (bool)_validated;
    }
}
