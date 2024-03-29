﻿using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class DayListMarkdownTemplateProvider : TemplateProviderBase,
    IDayListMarkdownTemplateProvider
{
    private readonly IConstantsProvider _constantsProvider;

    public DayListMarkdownTemplateProvider(IPathHelper pathHelper,
        IManifestStreamProvider manifestStreamProvider, IConstantsProvider constantsProvider)
        : base(pathHelper, manifestStreamProvider)
    {
        _constantsProvider = constantsProvider;
    }

    protected override string GetTemplateFileName()
        => _constantsProvider.DayListMarkdownTemplate.FileName;

    protected override string GetManifestStreamName()
        => _constantsProvider.DayListMarkdownTemplate.FullName;

    protected override FileTypeEnum GetFileType()
        => FileTypeEnum.MarkdownTemplate;

    protected override string GetTemplateDescription() => "daylist markdown";
}
