using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public sealed class ScoreHtmlPathResolver(
    IConfigurationProvider configurationProvider,
    IOutputFolderPathProvider outputFolderPathProvider,
    IFileSystemFactory fileSystemFactory)
    : PathResolverBase<string>(configurationProvider, outputFolderPathProvider, fileSystemFactory), 
        IScoreHtmlPathResolver
{
    protected override string FileNameWithoutExtension(string parameter)
    {
        return "ScoreHtml";
    }
}