using NSubstitute;
using NUnit.Framework;
using Todo.AppLaunching;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Tests;

[TestFixture]
public class FileExplorerLauncherTests
{
    private IPathHelper _pathHelper = null!;
    private IOutputWriter _outputWriter = null!;
    private ILaunchInfoSelector _launchInfoSelector = null!;
    private IProcessLauncher _processLauncher = null!;

    [SetUp]
    public void SetUp()
    {
        _pathHelper = Substitute.For<IPathHelper>();
        _outputWriter = Substitute.For<IOutputWriter>();
        _launchInfoSelector = Substitute.For<ILaunchInfoSelector>();
        _processLauncher = Substitute.For<IProcessLauncher>();

        var expectedLaunchInfos = new PerOsLaunchInfos(
            new ProcessLaunchInfo("explorer.exe", "{0}"),
            new ProcessLaunchInfo("xdg-open", "{0}"),
            new ProcessLaunchInfo("open", "{0}"));

        _launchInfoSelector
            .SelectLaunchInfoForThisOs(expectedLaunchInfos)
            .Returns(new ProcessLaunchInfo("xdg-open", "{0}"));

        _pathHelper.ResolveIfNotRooted("xdg-open").Returns("/usr/bin/xdg-open");
    }

    [Test]
    public void LaunchFiles_OpensEachFolderInTheSystemFileManager()
    {
        var launcher = CreateSut();

        launcher.LaunchFiles("/todos", "/archive");

        Received.InOrder(() =>
        {
            _outputWriter.WriteLine("Opening /todos in the system file manager.");
            _outputWriter.WriteLine("(/usr/bin/xdg-open /todos)");
            _processLauncher.Start("/usr/bin/xdg-open", "/todos");
            _outputWriter.WriteLine("Opening /archive in the system file manager.");
            _outputWriter.WriteLine("(/usr/bin/xdg-open /archive)");
            _processLauncher.Start("/usr/bin/xdg-open", "/archive");
        });
    }

    [Test]
    public void LaunchFiles_UsesHardcodedFileManagerLaunchInfos()
    {
        var launcher = CreateSut();

        launcher.LaunchFiles("/todos");

        _launchInfoSelector.Received(1).SelectLaunchInfoForThisOs(Arg.Is<PerOsLaunchInfos>(infos =>
            infos.Windows.Path == "explorer.exe" &&
            infos.Windows.Arguments == "{0}" &&
            infos.Linux.Path == "xdg-open" &&
            infos.Linux.Arguments == "{0}" &&
            infos.OSX.Path == "open" &&
            infos.OSX.Arguments == "{0}" &&
            infos.EnvironmentVariableToOverridePath == null &&
            infos.EnvironmentVariableToOverrideArguments == null));
    }

    private FileExplorerLauncher CreateSut() =>
        new(_pathHelper, _outputWriter, _launchInfoSelector, _processLauncher);
}
