using System;

namespace Todo.Contracts.Exceptions;

public abstract class TodoExceptionBase(string message) : Exception(message)
{
    public abstract string Advice();
}
