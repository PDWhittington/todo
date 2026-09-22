using System.Runtime.InteropServices;
using NSubstitute;
using NUnit.Framework;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.FileSystem;

namespace Todo.Tests;

[TestFixture]
public class FileSystemTests
{
    [TestCase(@"C:\todos\a.md", true)]
    [TestCase(@"C:/todos/a.md", true)]
    [TestCase(@"\todos", true)]
    [TestCase("/todos", true)]
    [TestCase(@"C:todos", true)]
    [TestCase(@"todos\a.md", false)]
    [TestCase("todos/a.md", false)]
    public void WindowsFileSystem_IsPathRooted(string path, bool expected)
    {
        Assert.That(new WindowsFileSystem().IsPathRooted(path), Is.EqualTo(expected));
    }

    [TestCase("/todos/a.md", true)]
    [TestCase("/", true)]
    [TestCase(@"C:\todos\a.md", false)]
    [TestCase(@"C:/todos/a.md", false)]
    [TestCase("todos/a.md", false)]
    [TestCase(@"todos\a.md", false)]
    public void UnixFileSystem_IsPathRooted(string path, bool expected)
    {
        Assert.That(new LinuxFileSystem().IsPathRooted(path), Is.EqualTo(expected));
        Assert.That(new MacOSFileSystem().IsPathRooted(path), Is.EqualTo(expected));
    }

    [Test]
    public void WindowsFileSystem_Combine_UsesBackslashAndReplacesWhenSecondPathIsRooted()
    {
        var fileSystem = new WindowsFileSystem();

        Assert.That(fileSystem.Combine(@"C:\todos", "a.md"), Is.EqualTo(@"C:\todos\a.md"));
        Assert.That(fileSystem.Combine(@"C:\todos\", "a.md"), Is.EqualTo(@"C:\todos\a.md"));
        Assert.That(fileSystem.Combine(@"C:\todos", @"D:\other.md"), Is.EqualTo(@"D:\other.md"));
    }

    [Test]
    public void UnixFileSystem_Combine_UsesForwardSlashAndReplacesWhenSecondPathIsRooted()
    {
        var fileSystem = new LinuxFileSystem();

        Assert.That(fileSystem.Combine("/todos", "a.md"), Is.EqualTo("/todos/a.md"));
        Assert.That(fileSystem.Combine("/todos/", "a.md"), Is.EqualTo("/todos/a.md"));
        Assert.That(fileSystem.Combine("/todos", "/other.md"), Is.EqualTo("/other.md"));
    }

    [Test]
    public void WindowsFileSystem_GetFullPath_CollapsesRelativeSegmentsWithoutUsingTheHostOs()
    {
        var fileSystem = new WindowsFileSystem();

        Assert.That(
            fileSystem.GetFullPath(@"foo\..\bar.md", @"C:\todos"),
            Is.EqualTo(@"C:\todos\bar.md"));
        Assert.That(
            fileSystem.GetFullPath(@"C:\todos\.\a.md", @"C:\unused"),
            Is.EqualTo(@"C:\todos\a.md"));
    }

    [Test]
    public void UnixFileSystem_GetFullPath_CollapsesRelativeSegmentsWithoutUsingTheHostOs()
    {
        var fileSystem = new LinuxFileSystem();

        Assert.That(
            fileSystem.GetFullPath("foo/../bar.md", "/todos"),
            Is.EqualTo("/todos/bar.md"));
        Assert.That(
            fileSystem.GetFullPath("/todos/./a.md", "/unused"),
            Is.EqualTo("/todos/a.md"));
    }

    [Test]
    public void GetFileNameWithoutExtension_IsTheSameOnWindowsAndUnixForSimpleNames()
    {
        const string fileName = "todo-2026-04-01.md";

        Assert.That(new WindowsFileSystem().GetFileNameWithoutExtension(fileName), Is.EqualTo("todo-2026-04-01"));
        Assert.That(new LinuxFileSystem().GetFileNameWithoutExtension(fileName), Is.EqualTo("todo-2026-04-01"));
        Assert.That(new MacOSFileSystem().GetFileNameWithoutExtension(fileName), Is.EqualTo("todo-2026-04-01"));
    }

    [Test]
    public void FilePathInfo_AcceptsWindowsPaths_WhenTheFactoryReturnsWindowsFileSystem()
    {
        var factory = Substitute.For<IFileSystemFactory>();
        factory.Create().Returns(new WindowsFileSystem());

        var pathInfo = FilePathInfo.Of(
            @"C:\todos\a.md",
            FileTypeEnum.MarkdownDayList,
            FolderEnum.TodoRoot,
            factory.Create());

        Assert.That(pathInfo.Path, Is.EqualTo(@"C:\todos\a.md"));
    }

    [Test]
    public void FilePathInfo_RejectsWindowsPaths_WhenTheFactoryReturnsAUnixFileSystem()
    {
        var factory = Substitute.For<IFileSystemFactory>();
        factory.Create().Returns(new MacOSFileSystem());

        Assert.That(
            () => FilePathInfo.Of(
                @"C:\todos\a.md",
                FileTypeEnum.MarkdownDayList,
                FolderEnum.TodoRoot,
                factory.Create()),
            Throws.Exception.With.Message.EqualTo("Only rooted paths are valid"));
    }

    [Test]
    public void FileSystemFactory_Create_IsCachedAndMatchesTheHostOs()
    {
        var factory = new FileSystemFactory();
        var first = factory.Create();
        var second = factory.Create();

        Assert.That(first, Is.SameAs(second));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Assert.That(first, Is.TypeOf<WindowsFileSystem>());
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            Assert.That(first, Is.TypeOf<MacOSFileSystem>());
        else
            Assert.That(first, Is.TypeOf<LinuxFileSystem>());
    }
}
