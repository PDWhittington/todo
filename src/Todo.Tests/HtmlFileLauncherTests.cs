using System;
using System.Diagnostics;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using Todo.AppLaunching;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Tests;

[TestFixture]
public class HtmlFileLauncherTests
{
    private IConfigurationProvider _configurationProvider = null!;
    private IPathHelper _pathHelper = null!;
    private IOutputWriter _outputWriter = null!;
    private ILaunchInfoSelector _launchInfoSelector = null!;
    private IProcessLauncher _processLauncher = null!;
    private PerOsLaunchInfos _browserLaunchInfos = null!;
    private PerOsLaunchInfos _textEditorLaunchInfos = null!;

    [SetUp]
    public void SetUp()
    {
        _configurationProvider = Substitute.For<IConfigurationProvider>();
        _pathHelper = Substitute.For<IPathHelper>();
        _outputWriter = Substitute.For<IOutputWriter>();
        _launchInfoSelector = Substitute.For<ILaunchInfoSelector>();
        _processLauncher = Substitute.For<IProcessLauncher>();

        _browserLaunchInfos = new PerOsLaunchInfos(
            new ProcessLaunchInfo("msedge.exe", "{0}"),
            new ProcessLaunchInfo("firefox", "{0}"),
            new ProcessLaunchInfo("open", "{0}"));

        _textEditorLaunchInfos = new PerOsLaunchInfos(
            new ProcessLaunchInfo("notepad.exe", "{0}"),
            new ProcessLaunchInfo("vim", "{0}"),
            new ProcessLaunchInfo("open", "-t {0}"));

        _configurationProvider.ConfigInfo.Returns(Config.GetMockConfigInfo(c => c with
        {
            BrowserPath = _browserLaunchInfos,
            TextEditorPath = _textEditorLaunchInfos
        }));

        _launchInfoSelector
            .SelectLaunchInfoForThisOs(_browserLaunchInfos)
            .Returns(new ProcessLaunchInfo("firefox", "{0}"));

        _pathHelper.ResolveIfNotRooted("firefox").Returns("/usr/bin/firefox");
    }

    [Test]
    public void LaunchFiles_ConvertsEachPathToAFileUriBeforeStartingTheBrowser()
    {
        var htmlPath = Path.GetFullPath(Path.Combine("todos", "page.html"));
        var expectedUri = new Uri(htmlPath).ToString();
        var launcher = CreateSut();

        launcher.LaunchFiles(htmlPath);

        _launchInfoSelector.Received(1).SelectLaunchInfoForThisOs(_browserLaunchInfos);
        _launchInfoSelector.DidNotReceive().SelectLaunchInfoForThisOs(_textEditorLaunchInfos);
        _processLauncher.Received(1).Start("/usr/bin/firefox", expectedUri);
        _outputWriter.Received(1).WriteLine($"Launching /usr/bin/firefox {expectedUri}");
        _processLauncher.DidNotReceive().Start(Arg.Any<string>(), htmlPath);
    }

    [Test]
    public void LaunchFiles_WhenProcessStartReturnsNull_DoesNotThrow()
    {
        _processLauncher.Start(Arg.Any<string>(), Arg.Any<string>()).Returns((Process?)null);
        var htmlPath = Path.GetFullPath("page.html");
        var launcher = CreateSut();

        Assert.DoesNotThrow(() => launcher.LaunchFiles(htmlPath));
    }

    private HtmlFileLauncher CreateSut() =>
        new(_configurationProvider, _pathHelper, _outputWriter, _launchInfoSelector, _processLauncher);
}
