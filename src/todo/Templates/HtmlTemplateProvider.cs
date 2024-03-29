﻿using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class HtmlTemplateProvider : TemplateProviderBase, IHtmlTemplateProvider
{
    private readonly IConstantsProvider _constantsProvider;

    public HtmlTemplateProvider(IPathHelper pathHelper,
        IManifestStreamProvider manifestStreamProvider, IConstantsProvider constantsProvider)
        : base(pathHelper, manifestStreamProvider)
    {
        _constantsProvider = constantsProvider;
    }

    protected override string GetTemplateFileName()
        => _constantsProvider.DefaultHtmlTemplate.FileName;

    protected override string GetManifestStreamName()
        => _constantsProvider.DefaultHtmlTemplate.FullName;

    protected override FileTypeEnum GetFileType()
        => FileTypeEnum.HtmlTemplate;

    protected override string GetTemplateDescription() => "html";
}
