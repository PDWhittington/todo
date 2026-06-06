using Microsoft.CodeAnalysis;

namespace Todo.SourceGenerators;

public record RegistrationPair
{
    public INamedTypeSymbol Interface { get; }
    public INamedTypeSymbol Implementation { get; }

    public RegistrationPair(INamedTypeSymbol interfaceType, INamedTypeSymbol implementationType)
    {
        Interface = interfaceType;
        Implementation = implementationType;
    }
}