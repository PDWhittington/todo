using System;
using System.IO;
using System.Text;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.FileSystem;

namespace Todo.Templates;

public abstract class TemplateProviderBase : FileReaderBase
{
    private readonly IPathHelper _pathHelper;
    private readonly IManifestStreamProvider _manifestStreamProvider;

    protected TemplateProviderBase(IPathHelper pathHelper, IManifestStreamProvider manifestStreamProvider)
    {
        _pathHelper = pathHelper;
        _manifestStreamProvider = manifestStreamProvider;
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

            return TodoFile.Of(filePathInfo, GetFileText(filePathInfo.Path));
        }

        var templatePathRootedToAssemblyFolder = _pathHelper.GetRootedToAssemblyFolder(pathToUse);

        if (File.Exists(templatePathRootedToAssemblyFolder))
        {
            var filePathInfo = FilePathInfo.Of(templatePathRootedToAssemblyFolder,
                GetFileType(), FolderEnum.AssemblyFolder);

            return TodoFile.Of(filePathInfo, GetFileText(filePathInfo.Path));
        }

        var manifestName = GetManifestStreamName();

        var text = _manifestStreamProvider.GetStringFromManifest(manifestName);

        var manifestFileInfo = FilePathInfo.Of($"/{manifestName}",
            GetFileType(), FolderEnum.Manifest);

        return TodoFile.Of(manifestFileInfo, text);
    }

    protected abstract string GetTemplateFileName();

    protected abstract string GetManifestStreamName();

    protected abstract FileTypeEnum GetFileType();

    protected abstract string GetTemplateDescription();
}
