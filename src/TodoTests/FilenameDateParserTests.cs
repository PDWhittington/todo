using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel;
using NSubstitute;
using NUnit.Framework;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.StateAndConfig;
using Todo.StringOperations;

namespace TodoTests;

[TestFixture]
public class FilenameDateParserTests
{
    [Test]
    [TestCaseSource(nameof(GetFileNameParsingTests))]
    public void TestCommandLine(FilenameDateParsingTestInfo filenameDateParsingTestInfo)
    {
        var configuration = Config.GetMockConfiguration() with
        {
            TodoListFilenameFormat = filenameDateParsingTestInfo.TemplateString
        };
        
        var configInfo = ConfigurationInfo.Of("", configuration);
        var configProvider = Substitute.For<IConfigurationProvider>();
        
        configProvider.ConfigInfo.Returns(configInfo);

        var filenameDateParser = new FilenameDateParser(configProvider);

        var match = filenameDateParser.TryParse(
            filenameDateParsingTestInfo.TestFileName, out var actualDate);
        
        Assert.AreEqual(filenameDateParsingTestInfo.IsMatch, match);

        if (!filenameDateParsingTestInfo.IsMatch) return;
        
        Assert.AreEqual(filenameDateParsingTestInfo.ExpectedDate, actualDate);
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