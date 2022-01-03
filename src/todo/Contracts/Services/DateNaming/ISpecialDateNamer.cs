using System;

namespace Todo.Contracts.Services.DateNaming;

public interface ISpecialDateNamer
{
    bool TryGetSpecialName(DateOnly date, out string? name);
}
