using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Todo.SourceGenerators;

var sourceCode = """
                 using System;

                 namespace Todo.Commands
                 {
                     public interface ICommandExecutor { }

                     public class ArchiveCommandExecutor : ICommandExecutor { }
                     public class CommitCommandExecutor : ICommandExecutor { }
                     public class GraphCommandExecutor : ICommandExecutor { }

                     // This base class should be skipped by your generator
                     public abstract class CommandExecutorBase : ICommandExecutor { }
                 }
                 """;



// 2. Create a real compilation (exactly what the compiler sees)
var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
var compilation = CSharpCompilation.Create(
    assemblyName: "TestAssembly",
    syntaxTrees: [syntaxTree],
    references: [
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        // Add any other references your real project needs (e.g. your own assemblies)
    ],
    options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

// 3. Instantiate your generator
var generator = new ServiceRegistrationGenerator();   // your real class

// 4. Create the driver and RUN it (this calls Initialize exactly as in a real build)
GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

Console.WriteLine("Running generator... (set breakpoints now)");

driver = driver.RunGenerators(compilation);

// Optional: inspect results
var result = driver.GetRunResult();
Console.WriteLine($"Generated {result.GeneratedTrees.Length} files");

foreach (var tree in result.GeneratedTrees)
{
    Console.WriteLine($"→ {tree.FilePath}");
    Console.WriteLine(tree.GetRoot().ToFullString());
}

Console.WriteLine("Done. Press any key to exit.");
Console.ReadKey();
