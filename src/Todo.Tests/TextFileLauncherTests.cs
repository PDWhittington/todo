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
public class TextFileLauncherTests
{
    private IConfigurationProvider _configurationProvider = null!;
    private IPathHelper _pathHelper = null!;
    private IOutputWriter _outputWriter = null!;
    private ILaunchInfoSelector _launchInfoSelector = null!;
    private IProcessLauncher _processLauncher = null!;
    private PerOsLaunchInfos _textEditorLaunchInfos = null!;
    private PerOsLaunchInfos _browserLaunchInfos = null!;

    [SetUp]
    public void SetUp()
    {
        _configurationProvider = Substitute.For<IConfigurationProvider>();
        _pathHelper = Substitute.For<IPathHelper>();
        _outputWriter = Substitute.For<IOutputWriter>();
        _launchInfoSelector = Substitute.For<ILaunchInfoSelector>();
        _processLauncher = Substitute.For<IProcessLauncher>();

        _textEditorLaunchInfos = new PerOsLaunchInfos(
            new ProcessLaunchInfo("notepad.exe", "{0}"),
            new ProcessLaunchInfo("vim", "-f {0}"),
            new ProcessLaunchInfo("open", "-t {0}"));

        _browserLaunchInfos = new PerOsLaunchInfos(
            new ProcessLaunchInfo("msedge.exe", "{0}"),
            new ProcessLaunchInfo("firefox", "{0}"),
            new ProcessLaunchInfo("safari", "{0}"));

        _configurationProvider.ConfigInfo.Returns(Config.GetMockConfigInfo(c => c with
        {
            TextEditorPath = _textEditorLaunchInfos,
            BrowserPath = _browserLaunchInfos
        }));

        _launchInfoSelector
            .SelectLaunchInfoForThisOs(_textEditorLaunchInfos)
            .Returns(new ProcessLaunchInfo("vim", "-f {0}"));

        _pathHelper.ResolveIfNotRooted("vim").Returns("/usr/bin/vim");
    }

    [Test]
    public void LaunchFiles_StartsResolvedEditorOncePerPath_WithInterpolatedArguments()
    {
        var launcher = CreateSut();

        launcher.LaunchFiles("/todos/a.md", "/todos/b.md");

        _launchInfoSelector.Received(1).SelectLaunchInfoForThisOs(_textEditorLaunchInfos);
        _launchInfoSelector.DidNotReceive().SelectLaunchInfoForThisOs(_browserLaunchInfos);
        _pathHelper.Received(1).ResolveIfNotRooted("vim");

        Received.InOrder(() =>
        {
            _outputWriter.WriteLine("Launching /usr/bin/vim -f /todos/a.md");
            _processLauncher.Start("/usr/bin/vim", "-f /todos/a.md");
            _outputWriter.WriteLine("Launching /usr/bin/vim -f /todos/b.md");
            _processLauncher.Start("/usr/bin/vim", "-f /todos/b.md");
        });
    }

    [Test]
    public void LaunchFiles_WithNoPaths_DoesNotStartAProcess()
    {
        var launcher = CreateSut();

        launcher.LaunchFiles();

        _processLauncher.DidNotReceive().Start(Arg.Any<string>(), Arg.Any<string>());
        _outputWriter.DidNotReceive().WriteLine(Arg.Any<string>());
    }

    private TextFileLauncher CreateSut() =>
        new(_configurationProvider, _pathHelper, _outputWriter, _launchInfoSelector, _processLauncher);
}
