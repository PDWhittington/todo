using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NUnit.Framework;
using Todo;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Config;
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
    private IServiceCollection _serviceCollection;
    private static readonly DateTime _currentDayForTest = new(2026, 04, 07, 12,  0, 0);
    
    [SetUp]
    public void Setup()
    {
        _serviceCollection = Initialise.GetServiceCollection();
        
        // // 2. Create the mock / substitute for the command-line extractor
        // var mockCommandLine = Substitute.For<ICommandLineProvider>();
        //
        // // Configure it however you want for this test
        // mockCommandLine.GetCommandLineMinusAssemblyLocation()
        //     
        // serviceCollection.
        //     
        //     GetArguments().Returns(new[] { "--mode", "test", "--value", "42" });
        // // or whatever your actual methods/properties are
        //
        // // 3. Replace the real implementation with the mock
        // // (Re-registering the same interface after the normal setup = the mock wins)
        // services.AddSingleton<ICommandLineExtractor>(mockCommandLine);
        //
        // // 4. Build the provider
        // var serviceProvider = services.BuildServiceProvider();

    }

    private IServiceProvider GetServiceProvider(ICommandLineProvider mockCommandLineProvider)
    {
        var serviceCollection = new ServiceCollection();

        foreach (var serviceDescriptor in _serviceCollection)
        {
            serviceCollection.Add(serviceDescriptor);
        }
        
        var mockDateAccessor = Substitute.For<IDateAccessor>();
        mockDateAccessor.GetNow().Returns(_currentDayForTest);
        
        var mockConfigProvider = Substitute.For<IConfigurationProvider>();
        mockConfigProvider.ConfigInfo.Returns(GetConfiguration());
        
        serviceCollection.AddSingleton(mockDateAccessor);
        serviceCollection.AddSingleton(mockConfigProvider);
        serviceCollection.AddSingleton(mockCommandLineProvider);
        
        return serviceCollection.BuildServiceProvider();
    }

    private ConfigurationInfo GetConfiguration()
    {
        var blankProcessLaunchInfo = new ProcessLaunchInfo("", "");
        
        var blankPaths = new PerOsFilePaths(blankProcessLaunchInfo, blankProcessLaunchInfo, blankProcessLaunchInfo);
        
        var configuration = new Configuration(blankPaths, blankPaths, "",
            "", "", "", "", "",
            true, true, new TimeSpan(4, 0, 0), 
            80, IterationMethodEnum.Parallel);

        return ConfigurationInfo.Of("", configuration);
    }
    
    [Test]
    // [TestCase("", new CreateOrShowDayListCommand(Constants.CurrentDayForTest.))]
    [TestCaseSource(nameof(GetCommandLineTests))]
    public void TestCommandLine(CommandLineTestInfo commandLineTestInfo)
    {
        // Arrange - different mock per test
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

        if (errorList.Count != 0)
        {
            var message = "The following properties were not as expected:" + Environment.NewLine + 
                string.Join(Environment.NewLine, errorList);
            
            throw new Exception(message);
        }
    }
    
    public class CommandLineTestInfo
    {
        public required string CommandLine { get; init; }
        public required CommandBase ExpectedCommand { get; init; }

        public override string ToString() => $"Command line of '{CommandLine}' should create command {ExpectedCommand}";
    }

    static IEnumerable<CommandLineTestInfo> GetCommandLineTests()
    {
        yield return new CommandLineTestInfo
        {
            CommandLine = "",
            ExpectedCommand = CreateOrShowDayListCommand.Of(DateOnly.FromDateTime(Constants.CurrentTimeForTest))
        };
    }
}