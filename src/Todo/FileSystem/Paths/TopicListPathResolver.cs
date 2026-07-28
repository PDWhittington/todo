using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public sealed class TopicListPathResolver(
    IConfigurationProvider configurationProvider,
    IOutputFolderPathProvider outputFolderPathProvider)
    : PathResolverBase<string>(configurationProvider, outputFolderPathProvider), ITopicListPathResolver
{
    protected override string FileNameWithoutExtension(string parameter)
        => parameter.Replace(' ', '-')
            .ToLower();
}