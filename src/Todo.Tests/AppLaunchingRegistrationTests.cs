using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Todo.AppLaunching;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem;
using Todo.FileSystem;

namespace Todo.Tests;

[TestFixture]
public class AppLaunchingRegistrationTests
{
    [Test]
    public void AppLaunchingServicesCanBeResolvedFromTheContainer()
    {
        using var provider = Initialise.GetServiceCollection().BuildServiceProvider();

        Assert.That(provider.GetRequiredService<IProcessLauncher>(), Is.TypeOf<ProcessLauncher>());
        Assert.That(provider.GetRequiredService<ITextFileLauncher>(), Is.TypeOf<TextFileLauncher>());
        Assert.That(provider.GetRequiredService<IHtmlFileLauncher>(), Is.TypeOf<HtmlFileLauncher>());
        Assert.That(provider.GetRequiredService<IFileExplorerLauncher>(), Is.TypeOf<FileExplorerLauncher>());
        Assert.That(provider.GetRequiredService<ILaunchInfoSelector>(), Is.TypeOf<LaunchInfoSelector>());
    }

    [Test]
    public void FileSystemFactoryIsRegisteredAndReturnsTheSameInstance()
    {
        using var provider = Initialise.GetServiceCollection().BuildServiceProvider();

        var factory = provider.GetRequiredService<IFileSystemFactory>();
        var fileSystem = factory.Create();

        Assert.That(factory, Is.TypeOf<FileSystemFactory>());
        Assert.That(fileSystem, Is.SameAs(factory.Create()));
        Assert.That(provider.GetRequiredService<IFileSystem>(), Is.SameAs(fileSystem));
    }
}
