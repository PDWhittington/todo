using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class TopicListPathResolver : PathResolverBase<string>,
    ITopicListPathResolver
{
    public TopicListPathResolver(IConfigurationProvider configurationProvider)
        : base(configurationProvider)
    { }

    public override string GetRegExForThisFileType() => ".*";

    public override string FileNameWithoutExtension(string parameter)
        => parameter.Replace(' ', '-')
            .ToLower();
}
