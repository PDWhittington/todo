using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Markdig;
using Microsoft.Extensions.Logging;
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
public class PrintHtmlCommandExecutor(
    IListHtmlTemplateProvider listHtmlTemplateProvider,
    IMarkdownFileReader markdownFileReader,
    IListHtmlSubstitutionsMaker listHtmlSubstitutionsMaker,
    IDateFormatter dateFormatter,
    IDateListPathResolver dateListPathResolver,
    IOutputWriter outputWriter,
    IConfigurationProvider configurationProvider,
    ILogger<PrintHtmlCommandExecutor> logger)
    : CommandExecutorBase<PrintHtmlCommand>(outputWriter, logger), IPrintHtmlCommandExecutor
{
    public override unsafe void Execute(PrintHtmlCommand command)
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseBootstrap().Build();

        var markdownSourceFile = markdownFileReader.ReadMarkdownFile(command.Date);
        
        var htmlTitle = dateFormatter.GetHtmlTitle(command.Date);

        var markdownStr = Encoding.UTF8.GetString(
            (byte*)markdownSourceFile.FileContents.Start,
            markdownSourceFile.FileContents.Length);
        
        var htmlBody = Markdown.ToHtml(markdownStr, pipeline);
        
        htmlBody = InsertRepoNameIfNecessary(htmlBody);

        var htmlTheme = configurationProvider.ConfigInfo.Configuration.HtmlTheme switch
        {
            HtmlThemeEnum.Light => "vscode-light",
            HtmlThemeEnum.Dark => "vscode-dark",
            _ => throw new Exception("Unknown html theme")
        };
            
        var htmlSubstitutions = ListHtmlSubstitutions.Of(htmlTitle, htmlBody, htmlTheme);

        var htmlTemplateFile = listHtmlTemplateProvider.GetTemplate();

        var pathInfo = dateListPathResolver.GetFilePathFor(command.Date, FileTypeEnum.Html);
        OutputWriter.WriteLine($"Writing file for {command.Date} to {pathInfo.Path}");

        using var stream = File.Create(pathInfo.Path);
        listHtmlSubstitutionsMaker.WriteSubstitutionsToStream(htmlTemplateFile.FileContents, htmlSubstitutions, stream);
    }

    private string InsertRepoNameIfNecessary(string htmlBody)
    {
        ArgumentNullException.ThrowIfNull(htmlBody);

        var configuration = configurationProvider.ConfigInfo.Configuration;
        
        if (!configuration.TodoListInfo.AppearInHtmlLists) return htmlBody;

        const string closingHeaderTag = "</h1>";

        var endOfHeaderIndex = htmlBody.IndexOf(closingHeaderTag, StringComparison.Ordinal);
        
        if (endOfHeaderIndex == -1) return htmlBody; //This should not happen, but don't throw exception.

        return
            string.Concat(htmlBody.AsSpan()[..(endOfHeaderIndex + closingHeaderTag.Length)], 
                $"<div class='todo-list-name-container'>(Todo list: {configuration.TodoListInfo.Name})</div></br>", 
                htmlBody.AsSpan(endOfHeaderIndex + closingHeaderTag.Length));
    }
}
