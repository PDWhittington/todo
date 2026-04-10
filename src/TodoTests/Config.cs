using System;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.Html;

namespace TodoTests;

public class Config
{
    public static ConfigurationInfo GetMockConfig()
    {
        var blankProcessLaunchInfo = new ProcessLaunchInfo("", "");
        
        var blankPaths = new PerOsFilePaths(blankProcessLaunchInfo, 
            blankProcessLaunchInfo, blankProcessLaunchInfo);
        
        var configuration = new Configuration(blankPaths, blankPaths, "",
            "", "", HtmlThemeEnum.Dark, "", "", "",
            true, true, new TimeSpan(4, 0, 0), 
            80, IterationMethodEnum.Parallel);

        return ConfigurationInfo.Of("", configuration);
    }
}