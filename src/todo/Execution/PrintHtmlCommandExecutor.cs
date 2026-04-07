using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Markdig;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Html;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintHtmlCommandExecutor : CommandExecutorBase<PrintHtmlCommand>, IPrintHtmlCommandExecutor
{
    private readonly IHtmlTemplateProvider _htmlTemplateProvider;
    private readonly IMarkdownFileReader _markdownFileReader;
    private readonly IHtmlSubstitutionsMaker _htmlSubstitutionsMaker;
    private readonly IDateFormatter _dateFormatter;
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IConfigurationProvider _configurationProvider;

    public PrintHtmlCommandExecutor(IHtmlTemplateProvider htmlTemplateProvider,
        IMarkdownFileReader markdownFileReader, IHtmlSubstitutionsMaker htmlSubstitutionsMaker,
        IDateFormatter dateFormatter, IDateListPathResolver dateListPathResolver, 
        IOutputWriter outputWriter, IConfigurationProvider configurationProvider)
        : base(outputWriter)
    {
        _htmlTemplateProvider = htmlTemplateProvider;
        _markdownFileReader = markdownFileReader;
        _htmlSubstitutionsMaker = htmlSubstitutionsMaker;
        _dateFormatter = dateFormatter;
        _dateListPathResolver = dateListPathResolver;
        _configurationProvider = configurationProvider;
    }

    public override void Execute(PrintHtmlCommand command)
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseBootstrap().Build();

        var markdownSourceFile = _markdownFileReader.ReadMarkdownFile(command.Date);
        
        var htmlTitle = _dateFormatter.GetHtmlTitle(command.Date);

        var htmlBody = Markdown.ToHtml(markdownSourceFile.FileContents, pipeline);

        var htmlTheme = _configurationProvider.ConfigInfo.Configuration.HtmlTheme switch
        {
            HtmlThemeEnum.Light => "vscode-light",
            HtmlThemeEnum.Dark => "vscode-dark",
            _ => throw new Exception("Unknown html theme"),
        };
            
        var htmlSubstitutions = HtmlSubstitutions.Of(htmlTitle, htmlBody, htmlTheme);

        var htmlTemplateFile = _htmlTemplateProvider.GetTemplate();

        var outputHtml = _htmlSubstitutionsMaker.MakeSubstitutions(htmlSubstitutions,
            htmlTemplateFile.FileContents);

        var pathInfo = _dateListPathResolver.GetFilePathFor(command.Date, FileTypeEnum.Html);

        OutputWriter.WriteLine($"Writing file for {command.Date} to {pathInfo.Path}");

        File.WriteAllText(pathInfo.Path, outputHtml);
    }
}
