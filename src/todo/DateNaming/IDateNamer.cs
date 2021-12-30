using System;
using Cavity;

namespace Todo.DateNaming;

public interface IDateNamer
{
    bool TryGetName(DateOnly date, out string name);
}