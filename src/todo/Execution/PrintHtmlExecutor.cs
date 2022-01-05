using System.IO;
using Markdig;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Templates;

namespace Todo.Execution;

public class PrintHtmlExecutor : IPrintHtmlExecutor
{
    private readonly IHtmlTemplateProvider _htmlTemplateProvider;
    private readonly IMarkdownFileReader _markdownFileReader;
    private readonly IHtmlSubstitutionsMaker _htmlSubstitutionsMaker;
    private readonly IDateFormatter _dateFormatter;
    private readonly IFileNamer _fileNamer;

    public PrintHtmlExecutor(IHtmlTemplateProvider htmlTemplateProvider, IMarkdownFileReader markdownFileReader,
        IHtmlSubstitutionsMaker htmlSubstitutionsMaker, IDateFormatter dateFormatter, IFileNamer fileNamer)
    {
        _htmlTemplateProvider = htmlTemplateProvider;
        _markdownFileReader = markdownFileReader;
        _htmlSubstitutionsMaker = htmlSubstitutionsMaker;
        _dateFormatter = dateFormatter;
        _fileNamer = fileNamer;
    }

    public void Execute(PrintHtmlCommand command)
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseBootstrap().Build();

        var markdownSourceFile = _markdownFileReader.ReadMarkdownFile(command.Date);

        var htmlBody = Markdown.ToHtml(markdownSourceFile.FileContents, pipeline);

        var htmlTitle = _dateFormatter.GetHtmlTitle(command.Date);

        var htmlSubstitutions = HtmlSubstitutions.Of(htmlTitle, htmlBody);

        var htmlTemplateFile = _htmlTemplateProvider.GetTemplate();

        var outputHtml = _htmlSubstitutionsMaker.MakeSubstitutions(htmlSubstitutions, htmlTemplateFile.FileContents);

        var pathInfo = _fileNamer.GetFilePath(command.Date, FileTypeEnum.Html);

        File.WriteAllText(pathInfo.Path, outputHtml);
    }
}
