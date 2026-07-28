using Microsoft.CodeAnalysis;

namespace Todo.SourceGenerators;

[Generator]
internal class CommandExecutorRegistrationGenerator: BaseRegistrationGenerator
{

    protected override bool InterfaceIsPrimaryInterface(INamedTypeSymbol interfaceSymbol)
        => interfaceSymbol.OriginalDefinition.Name == "ICommandExecutor"
           && interfaceSymbol.TypeArguments.Length == 0;

    protected override bool InterfaceIsRequired(INamedTypeSymbol interfaceSymbol) => true;

    protected override string OutputClassName() => "CommandExecutorRegistrations";

    protected override string OutputMethodName() => "RegisterCommandExecutors";

    protected override string[] OutputUsings() =>
    [
        "Todo.Execution",
        "Todo.Contracts.Services.Execution"
    ];
}