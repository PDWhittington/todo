using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public sealed class TopicListPathResolver(
    IConfigurationProvider configurationProvider,
    IOutputFolderPathProvider outputFolderPathProvider,
    IFileSystemFactory fileSystemFactory)
    : PathResolverBase<string>(configurationProvider, outputFolderPathProvider, fileSystemFactory), ITopicListPathResolver
{
    protected override string FileNameWithoutExtension(string parameter)
        => parameter.Replace(' ', '-')
            .ToLower();
}