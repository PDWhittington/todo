using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace Todo.SourceGenerators;

public abstract class BaseRegistrationGenerator(
    string commandFactoryInterface,
    string? commandBaseTypeName,
    string outputFileName)
{
    protected string CommandFactoryInterface { get; } = commandFactoryInterface;

    protected string? CommandBaseTypeName { get; } = commandBaseTypeName;

    protected string OutputFileName { get; } = outputFileName;

    protected abstract string GenerateRegistrationCode(ImmutableArray<INamedTypeSymbol> factories);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all classes that implement ICommandFactory<CommandBase>
        var factories = context
            .SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, ct) => GetFactoryIfMatches(ctx, ct)
            )
            .Where(static m => m is not null)
            .Collect(); // Collect all results into one ImmutableArray

        // Generate one file from all factories
        context.RegisterSourceOutput(
            factories,
            (spc, factoriesArray) =>
            {
                var source = GenerateRegistrationCode(factoriesArray!);
                spc.AddSource(OutputFileName,
                    SourceText.From(source, Encoding.UTF8)
                );
            }
        );
    }
    
    protected static bool IsOrInheritsFrom(ITypeSymbol type, string baseName)
    {
        var current = type;
        while (current != null)
        {
            if (current.Name == baseName)
                return true;
            current = current.BaseType;
        }
        return false;
    }
    
    protected INamedTypeSymbol? GetFactoryIfMatches(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;

        if (
            semanticModel.GetDeclaredSymbol(classDecl, cancellationToken)
            is not INamedTypeSymbol classSymbol
        )
            return null;

        // Must be a concrete class (not abstract)
        if (classSymbol.IsAbstract || classSymbol.IsGenericType)
            return null;

        foreach (var iface in classSymbol.AllInterfaces)
        {
            if (
                iface.OriginalDefinition.Name == CommandFactoryInterface
                && iface.TypeArguments.Length == 1
            )
            {
                var typeArg = iface.TypeArguments[0];
                
                if (CommandBaseTypeName is null ||
                    IsOrInheritsFrom(typeArg, CommandBaseTypeName))
                {
                    return classSymbol;
                }
            }
        }

        return null;
    }
}