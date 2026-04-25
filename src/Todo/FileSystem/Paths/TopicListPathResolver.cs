using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class TopicListPathResolver(
    IConfigurationProvider configurationProvider,
    IOutputFolderPathProvider outputFolderPathProvider)
    : PathResolverBase<string>(configurationProvider, outputFolderPathProvider), ITopicListPathResolver
{
    public override string GetRegExForThisFileType() => ".*";

    protected override string FileNameWithoutExtension(string parameter)
        => parameter.Replace(' ', '-')
            .ToLower();
}
