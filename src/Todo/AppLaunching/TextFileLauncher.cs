using System.Diagnostics;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public class TextFileLauncher(IConfigurationProvider configurationProvider,
    IPathHelper pathHelper, IOutputWriter outputWriter, ILaunchInfoSelector launchInfoSelector)
    : ITextFileLauncher
{
    public void LaunchFiles(params string [] filePaths)
    {
        var launchInfos = configurationProvider.ConfigInfo.Configuration.TextEditorPath;
        
        var launchInfoForThisOs = launchInfoSelector.SelectLaunchInfoForThisOS(launchInfos);
        
        var textEditorPath = launchInfoForThisOs.Path;
        var textEditorPathRooted = pathHelper.ResolveIfNotRooted(textEditorPath);
        
        foreach (var path in filePaths)
        {
            var parameters = launchInfoForThisOs.InterpolateParameters(path);
            
            outputWriter.WriteLine($"Launching {textEditorPathRooted} {parameters}");
            
            Process.Start(textEditorPathRooted, parameters);
        }
    }
}