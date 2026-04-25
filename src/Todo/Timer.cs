using System;
using System.Diagnostics;

namespace Todo;

public static class Timer
{
    private static bool _isStarted;
    private static Stopwatch _watch = new();
    
    public static void Start()
    {
        if (_isStarted) throw new Exception("Timer is already started");
        _isStarted = true;

        _watch.Start();
    }

    public static TimeSpan Elapsed => _isStarted 
        ? _watch.Elapsed 
        : throw new Exception("Timer is not started");
}