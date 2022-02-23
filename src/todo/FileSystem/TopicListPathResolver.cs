using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class TopicListPathResolver : PathResolverBase<string>,
    ITopicListPathResolver
{
    public TopicListPathResolver(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
        : base(configurationProvider, pathHelper)
    { }

    public override string GetRegExForThisFileType() => ".*";

    public override string FileNameWithoutExtension(string parameter)
        => parameter.Replace(' ', '-')
            .ToLower();
}
