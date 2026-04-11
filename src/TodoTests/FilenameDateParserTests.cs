using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Todo.Contracts.Data.Config;

namespace TodoTests;

[TestFixture]
public class FilenameDateParserTests
{
    [Test]
    [TestCaseSource(nameof(GetFileNameParsingTests))]
    public void TestCommandLine(FilenameDateParsingTestInfo filenameDateParsingTestInfo)
    {
        var configuration = Substitute.For<Configuration>();
        configuration.TodoListFilenameFormat.Returns(filenameDateParsingTestInfo.TemplateString);
        
        // var configInfo = Substitute.
        //
        // var configurationProvider = Substitute.For<IConfigurationProvider>();
        //
        // configurationProvider.ConfigInfo.Returns(configuration);
        //
        // var filenameDateParser = new FilenameDateParser(configurationProvider);
        //
        // var actualMatch = filenameDateParser.TryParse(filenameDateParsingTestInfo.TestFileName, out var actualDate);
        //
        // Assert.AreEqual(filenameDateParsingTestInfo.IsMatch, actualMatch);
        //
        // if (!filenameDateParsingTestInfo.IsMatch) return;
        //
        // Assert.AreEqual(filenameDateParsingTestInfo.ExpectedDate, actualDate);
    }

    public class FilenameDateParsingTestInfo
    {
        public required string TemplateString { get; init; }

        public required string TestFileName { get; init; }

        public required DateOnly? ExpectedDate  { get; init; }

        public required bool IsMatch { get; init; }

        public override string ToString() => $"{TemplateString}, {TestFileName}";
    }

    private static IEnumerable<FilenameDateParsingTestInfo> GetFileNameParsingTests()
    {
        yield return new FilenameDateParsingTestInfo()
        {
            TemplateString = "todo-{yyyy-MM-dd}.md",
            TestFileName = "todo-2026-04-01.md",
            ExpectedDate =  new DateOnly(2026, 4, 01),
            IsMatch = true
        };
    }
}