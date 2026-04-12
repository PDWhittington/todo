using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Todo;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.Html;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;

namespace TodoTests;

public static class Constants
{
    public static DateTime CurrentTimeForTest { get; } = new(2026, 04, 07, 12,  0, 0);
}

[TestFixture]
public class CommandInterpretationTests
{
    private static ServiceProvider GetServiceProvider(ICommandLineProvider mockCommandLineProvider)
    {
        var serviceCollection = Initialise.GetServiceCollection();
        
        var mockDateAccessor = Substitute.For<IDateAccessor>();
        mockDateAccessor.GetNow().Returns(Constants.CurrentTimeForTest);
        
        var mockConfigProvider = Substitute.For<IConfigurationProvider>();
        mockConfigProvider.ConfigInfo.Returns(Config.GetMockConfigInfo());
        
        serviceCollection.AddSingleton(mockDateAccessor);
        serviceCollection.AddSingleton(mockConfigProvider);
        serviceCollection.AddSingleton(mockCommandLineProvider);
        
        return serviceCollection.BuildServiceProvider();
    }
    
    [Test]
    [TestCaseSource(nameof(GetCommandLineTests))]
    public void TestCommandLine(CommandLineTestInfo commandLineTestInfo)
    {
        var mockCommandLine = Substitute.For<ICommandLineProvider>();
        mockCommandLine.GetCommandLineMinusAssemblyLocation().Returns(commandLineTestInfo.CommandLine);

        var serviceProvider = GetServiceProvider(mockCommandLine);

        var commandProvider = serviceProvider.GetRequiredService<ICommandProvider>();

        var actualCommand = commandProvider.GetCommand();
        
        TestActualCommandAgainstExpected(commandLineTestInfo.ExpectedCommand, actualCommand);
    }

    private static void TestActualCommandAgainstExpected(CommandBase expectedCommand, CommandBase actualCommand)
    {
        if (expectedCommand.GetType() != actualCommand.GetType())
        {
            throw new Exception($"Expected {expectedCommand.GetType().Name}, but got {actualCommand.GetType().Name}");
        }
        
        // Test all of the public properties
        
        var type = expectedCommand.GetType();
        var publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var errorList = new List<string>();
        
        foreach (var property in publicProperties)
        {
            var expectedPropertyValue = property.GetValue(expectedCommand);
            var actualPropertyValue = property.GetValue(actualCommand);
            
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (expectedPropertyValue is null && actualPropertyValue is null) continue;
            
            if (expectedPropertyValue is null)
            {
                errorList.Add($"{property.Name}: Expected null but got {actualPropertyValue?.GetType().Name}");
                continue;
            }

            if (actualPropertyValue is null)
            {
                errorList.Add($"{property.Name}: Expected {expectedPropertyValue} but got <NULL>");
                continue;
            }
            
            if (!expectedPropertyValue.Equals(actualPropertyValue))
            {
                errorList.Add($"{property.Name}: Expected {expectedPropertyValue} but got {actualPropertyValue}");
            }
        }

        if (errorList.Count == 0) return;
        
        var message = "The following properties were not as expected:" + Environment.NewLine + 
                      string.Join(Environment.NewLine, errorList);
            
        throw new Exception(message);
    }
    
    public class CommandLineTestInfo
    {
        public required string CommandLine { get; init; }
        public required CommandBase ExpectedCommand { get; init; }

        public override string ToString() => $"Command line of '{CommandLine}' should create command {ExpectedCommand}";
    }

    private static IEnumerable<CommandLineTestInfo> GetCommandLineTests()
    {
        yield return new CommandLineTestInfo
        {
            CommandLine = "",
            ExpectedCommand = CreateOrShowDayListCommand.Of(DateOnly.FromDateTime(Constants.CurrentTimeForTest))
        };
    }
}