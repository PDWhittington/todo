using System;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.FileSystem;

namespace Todo.Templates;

public abstract class TemplateProviderBase : FileReaderBase
{
    private readonly IPathHelper _pathHelper;
    private readonly IManifestStreamProvider _manifestStreamProvider;
    private readonly IMarkdownLineInterpreter _markdownLineInterpreter;

    protected TemplateProviderBase(IPathHelper pathHelper, IManifestStreamProvider manifestStreamProvider, 
        IMarkdownLineInterpreter markdownLineInterpreter)
    {
        _pathHelper = pathHelper;
        _manifestStreamProvider = manifestStreamProvider;
        _markdownLineInterpreter = markdownLineInterpreter;
    }

    /// <summary>
    /// Returns a string representing the Markdown template
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public TodoFile GetTemplate()
    {
        var pathToUse = GetTemplateFileName();

        var templatePathRootedToWorkingFolder = _pathHelper.GetRootedToWorkingFolder(pathToUse);

        if (File.Exists(templatePathRootedToWorkingFolder))
        {
            var filePathInfo = FilePathInfo.Of(templatePathRootedToWorkingFolder,
                FileTypeEnum.MarkdownTemplate, FolderEnum.SpecifiedInSettings);

            var lines = GetFileText(filePathInfo.Path);

            var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
                _markdownLineInterpreter.CreateMarkdownLine(filePathInfo, lines));
            
            var fileContents = new Lazy<string>(() => string.Join(Environment.NewLine, lines));
            
            return TodoFile.Of(filePathInfo, GetFileText(filePathInfo.Path),  markdownLines, fileContents);
        }

        var templatePathRootedToAssemblyFolder = _pathHelper.GetRootedToAssemblyFolder(pathToUse);

        if (File.Exists(templatePathRootedToAssemblyFolder))
        {
            var filePathInfo = FilePathInfo.Of(templatePathRootedToAssemblyFolder,
                GetFileType(), FolderEnum.AssemblyFolder);

            var lines = GetFileText(filePathInfo.Path);

            var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
                _markdownLineInterpreter.CreateMarkdownLine(filePathInfo, lines));
            
            var fileContents = new Lazy<string>(() => string.Join(Environment.NewLine, lines));
            
            return TodoFile.Of(filePathInfo, lines, markdownLines, fileContents);
        }

        {
            var manifestName = GetManifestStreamName();

            var lines = _manifestStreamProvider.GetLinesFromManifest(manifestName);

            var manifestFileInfo = FilePathInfo.Of($"/{manifestName}",
                GetFileType(), FolderEnum.Manifest);

            var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
                _markdownLineInterpreter.CreateMarkdownLine(manifestFileInfo, lines));
        
            var fileContents = new Lazy<string>(() => string.Join(Environment.NewLine, lines));
        
            return TodoFile.Of(manifestFileInfo, lines, markdownLines, fileContents);    
        }
    }

    protected abstract string GetTemplateFileName();

    protected abstract string GetManifestStreamName();

    protected abstract FileTypeEnum GetFileType();

    // ReSharper disable once UnusedMember.Global
    protected abstract string GetTemplateDescription();
}
