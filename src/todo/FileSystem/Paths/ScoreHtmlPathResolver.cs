using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class ScoreHtmlPathResolver(
    IConfigurationProvider configurationProvider,
    IOutputFolderPathProvider outputFolderPathProvider)
    : PathResolverBase<string>(configurationProvider, outputFolderPathProvider), 
        IScoreHtmlPathResolver
{
    public override string GetRegExForThisFileType()
    {
        return "ScoreHtml";
    }

    protected override string FileNameWithoutExtension(string parameter)
    {
        return "ScoreHtml";
    }
}