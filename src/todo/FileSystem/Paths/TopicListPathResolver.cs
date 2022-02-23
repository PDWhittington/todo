using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class TopicListPathResolver : PathResolverBase<string>, ITopicListPathResolver
{
    public TopicListPathResolver(IConfigurationProvider configurationProvider, IPathHelper pathHelper,
        IOutputFolderPathProvider outputFolderPathProvider)
        : base(configurationProvider, pathHelper, outputFolderPathProvider)
    { }

    public override string GetRegExForThisFileType() => ".*";

    protected override string FileNameWithoutExtension(string parameter)
        => parameter.Replace(' ', '-')
            .ToLower();
}
