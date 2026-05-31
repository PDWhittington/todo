using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Todo.SourceGenerators;

// Register MSBuild (important - do this early, before any Roslyn/MSBuild use)
MSBuildLocator.RegisterDefaults();   // or RegisterInstance() if you need specific VS/MSBuild

// Path to your .sln or .csproj
var solutionPath = @"/Users/philipwhittington/Workspace/src/todo/src/Todo.sln";

Console.WriteLine("Opening solution...");

using var workspace = MSBuildWorkspace.Create(new Dictionary<string, string>
{
    // Optional: Common properties
    { "Configuration", "Debug" },
    { "Platform", "AnyCPU" },
    { "TargetFramework", "net10.0" },
    { "DesignTimeBuild", "true" },
    { "BuildProjectReferences", "false" },  // Often helps
    { "AppendTargetFrameworkToOutputPath", "true" }  // Force it
});

workspace.WorkspaceFailed += (sender, e) =>
{
    Console.WriteLine($"Workspace warning/error: {e.Diagnostic.Message}");
};

// Load the solution
var solution = await workspace.OpenSolutionAsync(solutionPath);

// Or load a single project:
// var project = await workspace.OpenProjectAsync(projectPath);

Console.WriteLine($"Loaded solution with {solution.Projects.Count()} projects");

// === Choose which projects you want to analyze ===
var projectsToAnalyze = solution.Projects
    .Where(p => 
        !p.Name.Contains("Test", StringComparison.OrdinalIgnoreCase) &&   // skip test projects
        p.Language == LanguageNames.CSharp &&
        // Add more filters as needed, e.g. specific project names
        (p.Name.Contains("Commands") || p.Name.Contains("Domain")))
    .ToList();

foreach (var project in projectsToAnalyze)
{
    Console.WriteLine($"Processing project: {project.Name}");

    var compilation = await project.GetCompilationAsync();

    if (compilation is null)
    {
        Console.WriteLine($"  Failed to get compilation for {project.Name}");
        continue;
    }

    // 3. Instantiate your generator
    var generator = new ServiceRegistrationGenerator();

    // 4. Run the generator
    GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

    driver = driver.RunGenerators(compilation);

    // Inspect results
    var result = driver.GetRunResult();
    Console.WriteLine($"  Generated {result.GeneratedTrees.Length} files for {project.Name}");

    foreach (var tree in result.GeneratedTrees)
    {
        Console.WriteLine($"  → {tree.FilePath}");
        Console.WriteLine(tree.GetRoot().ToFullString());
    }
}





//
// var sourceCode = """
//                  using System;
//
//                  namespace Todo.Commands
//                  {
//                      public interface ICommandExecutor { }
//
//                      public class ArchiveCommandExecutor : ICommandExecutor { }
//                      public class CommitCommandExecutor : ICommandExecutor { }
//                      public class GraphCommandExecutor : ICommandExecutor { }
//
//                      // This base class should be skipped by your generator
//                      public abstract class CommandExecutorBase : ICommandExecutor { }
//                  }
//                  """;
//
//
//
// // 2. Create a real compilation (exactly what the compiler sees)
// var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
// var compilation = CSharpCompilation.Create(
//     assemblyName: "TestAssembly",
//     syntaxTrees: [syntaxTree],
//     references: [
//         MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
//         // Add any other references your real project needs (e.g. your own assemblies)
//     ],
//     options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));
//
// // 3. Instantiate your generator
// var generator = new ServiceRegistrationGenerator();   // your real class
//
// // 4. Create the driver and RUN it (this calls Initialize exactly as in a real build)
// GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
//
// Console.WriteLine("Running generator... (set breakpoints now)");
//
// driver = driver.RunGenerators(compilation);
//
// // Optional: inspect results
// var result = driver.GetRunResult();
// Console.WriteLine($"Generated {result.GeneratedTrees.Length} files");
//
// foreach (var tree in result.GeneratedTrees)
// {
//     Console.WriteLine($"→ {tree.FilePath}");
//     Console.WriteLine(tree.GetRoot().ToFullString());
// }
//
// Console.WriteLine("Done. Press any key to exit.");
// Console.ReadKey();
