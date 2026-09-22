using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public class TextFileLauncher(
    IConfigurationProvider configurationProvider,
    IPathHelper pathHelper,
    IOutputWriter outputWriter,
    ILaunchInfoSelector launchInfoSelector,
    IProcessLauncher processLauncher)
    : AppLauncherBase(pathHelper, outputWriter, launchInfoSelector, processLauncher), ITextFileLauncher
{
    protected override PerOsLaunchInfos GetLaunchInfos()
        => configurationProvider.ConfigInfo.Configuration.TextEditorPath;
}
