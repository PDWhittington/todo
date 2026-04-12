using System;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class DateListPathResolver(
    IConfigurationProvider configurationProvider,
    IOutputFolderPathProvider outputFolderPathProvider,
    IFilenameDateParser filenameDateParser)
    : PathResolverBase<DateOnly>(configurationProvider, outputFolderPathProvider, filenameDateParser), 
        IDateListPathResolver
{
    public override string GetRegExForThisFileType()
    {
        var fileNameFragments = GetFragments(
            ConfigurationProvider.ConfigInfo.Configuration.TodoListFilenameFormat,
            '{', '}', _ => ".*");

        return string.Join("", fileNameFragments);
    }

    protected override string FileNameWithoutExtension(DateOnly dateOnly)
    {
        var fileNameFragments = GetFragments(
            ConfigurationProvider.ConfigInfo.Configuration.TodoListFilenameFormat,
            '{', '}', dateOnly.ToString);

        return string.Join("", fileNameFragments);
    }
}
