using System;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.Html;

namespace TodoTests;

public static class Config
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static Configuration GetMockConfiguration()
    {
        var blankProcessLaunchInfo = new ProcessLaunchInfo("", "");
        
        var blankPaths = new PerOsFilePaths(blankProcessLaunchInfo, 
            blankProcessLaunchInfo, blankProcessLaunchInfo);

        var todoListInfo = new TodoListInformation("Test list", true, true);
        
        var configuration = new Configuration(todoListInfo, blankPaths, blankPaths, "",
            "", "", "", HtmlThemeEnum.Dark, "", "", "",
            true, true, new TimeSpan(4, 0, 0), 
            80, IterationMethodEnum.Parallel, 21, []);
        
        return configuration;
    }
    
    public static ConfigurationInfo GetMockConfigInfo()
    {
        var configuration = GetMockConfiguration();

        return ConfigurationInfo.Of("", configuration);
    }

    public static ConfigurationInfo GetMockConfigInfo(Func<Configuration, Configuration> @override)
    {
        var baseConfiguration = GetMockConfiguration();

        var overiddenConfiguration = @override(baseConfiguration);
        return ConfigurationInfo.Of("", overiddenConfiguration);
    }
}