using System;
using System.Threading;

namespace Todo.Contracts.Data.Caching;

public class ResettableLazy<T>
{
    private Lazy<T> _lazy;

    public T Value => _lazy.Value;

    private LazyThreadSafetyMode LazyThreadSafetyMode { get; }

    private readonly Func<T> _valueFactory;

    public ResettableLazy(Func<T> valueFactory, LazyThreadSafetyMode lazyThreadSafetyMode = LazyThreadSafetyMode.ExecutionAndPublication)
    {
        _valueFactory = valueFactory;
        LazyThreadSafetyMode = lazyThreadSafetyMode;
        _lazy = new Lazy<T>(_valueFactory, LazyThreadSafetyMode);
    }

    public void Reset()
    {
        _lazy = new Lazy<T>(_valueFactory, LazyThreadSafetyMode);
    }
}
