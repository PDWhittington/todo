using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Todo.SourceGenerators;

[Generator]
public class CommandFactoryRegistrationGenerator : BaseRegistrationGenerator
{
 
    protected override bool NodePredicate(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode is ClassDeclarationSyntax;
    }
    
    protected override RegistrationPair[]? Transform(GeneratorSyntaxContext context, 
        CancellationToken cancellationToken)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        
        if (semanticModel.GetDeclaredSymbol(classDecl, cancellationToken)
            is not INamedTypeSymbol classSymbol)
            return null;
        
        // Must be a concrete class (not abstract)
        if (classSymbol.IsAbstract || classSymbol.IsGenericType)
            return null;
        
        foreach (var iface in classSymbol.AllInterfaces)
        {
            if (iface.OriginalDefinition.Name == "ICommandFactory"
                && iface.TypeArguments.Length == 1)
            {
                return
                [
                    new RegistrationPair(iface, classSymbol)
                ];
            }
        }
        
        return null;
    }

    protected override string OutputFileName() => "CommandFactoryRegistrations.g.cs";

    protected override string OutputClassName() => "CommandFactoryRegistrations";

    protected override string OutputMethodName() => "RegisterCommandFactories";

    protected override string[] OutputUsings()
    {
        return
        [
            "Todo.Contracts.Services.CommandFactories",
            "Todo.Contracts.Data.Commands"
        ];
    }
}
