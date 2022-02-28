using System;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class DateListPathResolver : PathResolverBase<DateOnly>, IDateListPathResolver
{
    public DateListPathResolver(IConfigurationProvider configurationProvider,
        IOutputFolderPathProvider outputFolderPathProvider)
        : base(configurationProvider, outputFolderPathProvider)
    { }

    public override string GetRegExForThisFileType()
    {
        var fileNameFragments = GetFragments(ConfigurationProvider.Config.TodoListFilenameFormat,
            '{', '}', _ => ".*");

        return string.Join("", fileNameFragments) + $".{MarkdownExtension}";
    }

    protected override string FileNameWithoutExtension(DateOnly dateOnly)
    {
        var fileNameFragments = GetFragments(ConfigurationProvider.Config.TodoListFilenameFormat,
            '{', '}', dateOnly.ToString);

        return string.Join("", fileNameFragments);
    }
}
