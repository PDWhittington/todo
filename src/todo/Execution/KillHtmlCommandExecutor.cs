﻿using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandExecutor : CommandExecutorBase<KillHtmlCommand>, IKillHtmlCommandExecutor
{
    private readonly IOutputFolderPathProvider _outputFolderPathProvider;
    private readonly IFileDeleter _fileDeleter;

    public KillHtmlCommandExecutor(IOutputFolderPathProvider pathRootingProvider,
        IFileDeleter fileDeleter, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _outputFolderPathProvider = pathRootingProvider;
        _fileDeleter = fileDeleter;
    }

    public override void Execute(KillHtmlCommand command)
    {
        OutputWriter.WriteLine("Deleting html files in the output and archive folders.");

        _fileDeleter.Delete(_outputFolderPathProvider.GetRootedOutputFolder(), "*.html");
        _fileDeleter.Delete(_outputFolderPathProvider.GetRootedArchiveFolder(), "*.html");
    }
}
