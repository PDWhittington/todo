using System;

namespace Todo.Contracts.Exceptions;

public abstract class TodoExceptionBase : Exception
{
    protected TodoExceptionBase()
        : base()
    { }

    protected TodoExceptionBase(string message)
        : base(message)
    { }

    public abstract string Advice();
}
