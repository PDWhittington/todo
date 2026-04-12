using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class TopicListPathResolver : PathResolverBase<string>, ITopicListPathResolver
{
    public TopicListPathResolver(IConfigurationProvider configurationProvider,
        IOutputFolderPathProvider outputFolderPathProvider)
        : base(configurationProvider, outputFolderPathProvider)
    { }

    public override string GetRegExForThisFileType() => ".*";

    protected override string FileNameWithoutExtension(string parameter)
        => parameter.Replace(' ', '-')
            .ToLower();
}
