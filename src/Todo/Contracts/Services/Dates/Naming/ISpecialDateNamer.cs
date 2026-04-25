using System;

namespace Todo.Contracts.Services.Dates.Naming;

public interface ISpecialDateNamer
{
    bool TryGetSpecialName(DateOnly date, out string? name);
}
