using Microsoft.CodeAnalysis;

namespace Todo.SourceGenerators;

public record ExecutorPair
{
    public INamedTypeSymbol? Interface { get; }
    public ITypeSymbol? Implementation { get; }
    public ITypeSymbol? CommandType { get; }

    public ExecutorPair(INamedTypeSymbol? interfaceType, ITypeSymbol? implementationType,
        INamedTypeSymbol? commandType)
    {
        Interface = interfaceType;
        Implementation = implementationType;
        CommandType = commandType;
    }
}