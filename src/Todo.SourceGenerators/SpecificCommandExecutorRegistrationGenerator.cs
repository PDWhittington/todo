using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using Microsoft.CodeAnalysis.Text;

namespace Todo.SourceGenerators;

[Generator]
public class SpecificCommandExecutorRegistrationGenerator : BaseRegistrationGenerator
{
    private static INamedTypeSymbol? GetCommandFactoryOfCommandBase(Compilation compilation)
    {
        var openFactory = compilation.GetTypeByMetadataName("Todo.Contracts.Services.Execution.ICommandExecutor`1");
        var commandBase = compilation.GetTypeByMetadataName("Todo.Contracts.Data.Commands.CommandBase");

        if (openFactory is null || commandBase is null)
            return null;

        // Construct ICommandFactory<CommandBase>
        return openFactory.Construct(commandBase);
    }
    
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {   
        // Get the constructed symbol (ICommandFactory<CommandBase>)
        var targetInterface = context.CompilationProvider.Select(static (compilation, _) =>
            GetCommandFactoryOfCommandBase(compilation));
        
        var iCommandExecutorType = GetType(context, 
            "Todo.Contracts.Services.Execution.ICommandExecutor`1");
        
        var commandBase = GetType(context, 
            "Todo.Contracts.Data.Commands.CommandBase");
        

        
        // Find all classes that implement ICommandFactory<CommandBase>
        var registrationPairs = context
            .SyntaxProvider.CreateSyntaxProvider(
                predicate: NodePredicate,
                transform: Transform)
            .SelectMany(static (pairs, _) => pairs ?? [])
            .Collect(); 
        
        // Generate one file from all factories
        context.RegisterSourceOutput(
            registrationPairs,
            (spc, rps) =>
            {
                var source = "//Hello world";
                spc.AddSource(OutputFileName(),
                    SourceText.From(source, Encoding.UTF8));
                
            });
    }
    
    protected override bool NodePredicate(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode is ClassDeclarationSyntax;
    }
    
    // protected override RegistrationPair[]? Transform(GeneratorSyntaxContext context, 
    //     CancellationToken cancellationToken)
    // {
    //     return [];
    // }
    
    protected override RegistrationPair[]? Transform(GeneratorSyntaxContext context, 
        CancellationToken cancellationToken)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        
        if (semanticModel.GetDeclaredSymbol(classDecl, cancellationToken)
            is not INamedTypeSymbol classSymbol)
            return null;
        
        if (classSymbol.IsAbstract || classSymbol.IsGenericType)
            return null;
        
        
        foreach (var iface in classSymbol.AllInterfaces)
        {
            if (InterfaceItselfImplementsCommandExecutor(iface, 
                    out var commandExecutorBase))
            {
                return
                [
                    new RegistrationPair(commandExecutorBase!, classSymbol)
                ];
            }
        }
        
        return null;
    }

    private IncrementalValueProvider<INamedTypeSymbol> GetType(
        IncrementalGeneratorInitializationContext context, string typeName)
    {
        return context.CompilationProvider.Select((comp, _) => comp.GetTypeByMetadataName(typeName))!;
        
        // var knownTypes = context.CompilationProvider.Select(static (comp, _) => new KnownTypes
        // {
        //     CommandFactory = comp.GetTypeByMetadataName("MyApp.ICommandFactory`1"),   // Note the `1
        //     CommandBase    = comp.GetTypeByMetadataName("MyApp.CommandBase")
        // });
    }
    
    private bool InterfaceItselfImplementsCommandExecutor(INamedTypeSymbol classSymbol, 
        out INamedTypeSymbol? commandExecutorBase)
    {
        foreach (var iface in classSymbol.AllInterfaces)
        {
            if (iface.Name == "ICommandExecutor" 
                && iface.TypeArguments.Length == 1
                && iface.TypeArguments[0].Name == "CommandBase")
                {
                    commandExecutorBase = iface;
                    return true;
                }
        }
        
        commandExecutorBase = null;
        return false;
    }

    protected override string OutputClassName() => "SpecificCommandExecutorRegistrations";

    protected override string OutputMethodName()  => "RegisterSpecificCommandExecutors";

    protected override string[] OutputUsings() =>
    [
        "Todo.Execution",
        "Todo.Contracts.Services.Execution"
    ];
}