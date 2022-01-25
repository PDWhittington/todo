using System;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class DateListPathResolver : PathResolverBase<DateOnly>,
    IDateListPathResolver
{
    public DateListPathResolver(IConfigurationProvider configurationProvider)
        : base(configurationProvider)
    { }

    public override string GetRegExForThisFileType()
    {
        var fileNameFragments = GetFragments(ConfigurationProvider.Config.TodoListFilenameFormat,
            '{', '}', _ => ".*");

        return string.Join("", fileNameFragments) + $".{MarkdownExtension}";
    }

    public override string FileNameWithoutExtension(DateOnly dateOnly)
    {
        var fileNameFragments = GetFragments(ConfigurationProvider.Config.TodoListFilenameFormat,
            '{', '}', dateOnly.ToString);

        return string.Join("", fileNameFragments);
    }
}
