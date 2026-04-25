using Microsoft.CodeAnalysis;

namespace Todo.SourceGenerators;

public record RegistrationInfo(INamedTypeSymbol[] Interfaces, INamedTypeSymbol Implementation)
{
    public INamedTypeSymbol[] Interfaces { get; } = Interfaces;
    public INamedTypeSymbol Implementation { get; } = Implementation;
}