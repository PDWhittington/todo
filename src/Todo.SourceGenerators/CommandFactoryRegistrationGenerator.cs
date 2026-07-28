using Microsoft.CodeAnalysis;

namespace Todo.SourceGenerators;

[Generator]
internal class CommandFactoryRegistrationGenerator : BaseRegistrationGenerator
{
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

    protected override bool InterfaceIsPrimaryInterface(INamedTypeSymbol interfaceSymbol)
        => interfaceSymbol.OriginalDefinition.Name == "ICommandFactory"
           && interfaceSymbol.TypeArguments.Length == 1;

    protected override bool InterfaceIsRequired(INamedTypeSymbol interfaceSymbol)
    {
        return true;
    }
}