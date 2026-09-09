using NUnit.Framework;
using Todo.Contracts.Data.CommandLine;

namespace Todo.Tests;

[TestFixture]
public class CommandLineInfoTests
{
    [Test]
    public void FromArgs_NoUserArguments_HasEmptyCommandAndSwitches()
    {
        var commandLine = CommandLineInfo.FromArgs(["todo.exe"]);

        Assert.That(commandLine.Command, Is.EqualTo(string.Empty));
        Assert.That(commandLine.Switches, Is.EqualTo(string.Empty));
        Assert.That(commandLine.ToString(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void FromArgs_SkipsProcessName_AndTreatsNextTokenAsCommand()
    {
        var commandLine = CommandLineInfo.FromArgs(["C:\\portable\\todo\\todo.exe", "which"]);

        Assert.That(commandLine.Command, Is.EqualTo("which"));
        Assert.That(commandLine.Switches, Is.EqualTo(string.Empty));
        Assert.That(commandLine.ToString(), Is.EqualTo("which"));
    }

    [Test]
    public void FromArgs_JoinsRemainingTokensAsSwitches()
    {
        var commandLine = CommandLineInfo.FromArgs(["todo", "t", "my", "topic"]);

        Assert.That(commandLine.Command, Is.EqualTo("t"));
        Assert.That(commandLine.Switches, Is.EqualTo("my topic"));
        Assert.That(commandLine.ToString(), Is.EqualTo("t my topic"));
    }
}
