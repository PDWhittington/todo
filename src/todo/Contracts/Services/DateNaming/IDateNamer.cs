using System;

namespace Todo.Contracts.Services.DateNaming;

public interface IDateNamer
{
    bool TryGetName(DateOnly date, out string name);
}