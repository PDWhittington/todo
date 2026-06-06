using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;

namespace Todo.SourceGenerators;

[Generator]
public class SpecificCommandExecutorRegistrationGenerator 
    : BaseRegistrationGenerator, IIncrementalGenerator
{
    private const string CommandExecutorInterface = "ICommandExecutor";

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
        
        if (classSymbol.IsAbstract || classSymbol.IsGenericType)
            return null;
        
        foreach (var iface in classSymbol.AllInterfaces)
        {
            if (InterfaceItselfImplementsCommandExecutor(iface))
            {
                return
                [
                    new RegistrationPair(iface, classSymbol)
                ];
            }
        }
        
        return null;
    }

    private bool InterfaceItselfImplementsCommandExecutor(INamedTypeSymbol classSymbol)
    {
        return Enumerable.Any(classSymbol.AllInterfaces, 
            iface => 
                iface.OriginalDefinition.Name == "ICommandExecutor" && 
                iface.TypeArguments.Length == 1);
    }

    protected override string OutputFileName() => "SpecificCommandExecutorRegistrations.g.cs";

    protected override string OutputClassName() => "SpecificCommandExecutorRegistrations";

    protected override string OutputMethodName()  => "RegisterSpecificCommandExecutors";

    protected override string[] OutputUsings() =>
    [
        "Todo.Execution",
        "Todo.Contracts.Services.Execution"
    ];
}