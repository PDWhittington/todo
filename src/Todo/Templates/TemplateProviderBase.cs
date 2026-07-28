using System;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.FileSystem;

namespace Todo.Templates;

public abstract class TemplateProviderBase(
    IAssemblyInformationProvider assemblyInformationProvider,
    IPathHelper pathHelper,
    IMarkdownLineInterpreter markdownLineInterpreter,
    IUnmanagedByteArrayManager unmanagedByteArrayManager)
    : FileReaderBase(unmanagedByteArrayManager)
{
    /// <summary>
    /// Returns a string representing the Markdown template
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public TodoFile GetTemplate()
    {
        var pathToUse = GetTemplateFileName();

        var templatePathRootedToWorkingFolder = pathHelper.GetRootedToWorkingFolder(pathToUse);

        if (File.Exists(templatePathRootedToWorkingFolder))
        {
            var filePathInfo = FilePathInfo.Of(templatePathRootedToWorkingFolder,
                FileTypeEnum.MarkdownTemplate, FolderEnum.SpecifiedInSettings);

            var lazyFile = new Lazy<UnmanagedByteArray>(() => LoadFile(filePathInfo.Path));

            var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
                markdownLineInterpreter.CreateMarkdownLines(lazyFile.Value));
            
            return TodoFile.Of(filePathInfo, markdownLines, lazyFile);
        }

        var templatePathRootedToAssemblyFolder = assemblyInformationProvider.GetRootedToAssemblyFolder(pathToUse);

        if (File.Exists(templatePathRootedToAssemblyFolder))
        {
            var filePathInfo = FilePathInfo.Of(templatePathRootedToAssemblyFolder,
                GetFileType(), FolderEnum.AssemblyFolder);

            var lazyFile = new Lazy<UnmanagedByteArray>(() => LoadFile(filePathInfo.Path)); 

            var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
                markdownLineInterpreter.CreateMarkdownLines(lazyFile.Value));
            
            return TodoFile.Of(filePathInfo, markdownLines, lazyFile);
        }

        {
            var manifestName = GetManifestStreamName();
            
            var manifestFileInfo = FilePathInfo.Of($"/{manifestName}",
                GetFileType(), FolderEnum.Manifest);

            var lazyFile = new Lazy<UnmanagedByteArray>(() => LoadFromManifest(manifestName));
            
            var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
                markdownLineInterpreter.CreateMarkdownLines(lazyFile.Value));
        
            return TodoFile.Of(manifestFileInfo, markdownLines, lazyFile);    
        }
    }

    protected abstract string GetTemplateFileName();

    protected abstract string GetManifestStreamName();

    protected abstract FileTypeEnum GetFileType();

    // ReSharper disable once UnusedMember.Global
    protected abstract string GetTemplateDescription();
}