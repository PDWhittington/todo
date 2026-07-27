using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public record GitGetRepoInfoCommand : IGitCommand<RepoInfoResult>;
