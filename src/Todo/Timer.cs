using System;
using System.Diagnostics;

namespace Todo;

public static class Timer
{
    private static bool _isStarted;
    private static readonly Stopwatch Watch = new();

    public static void Start()
    {
        if (_isStarted) throw new Exception("Timer is already started");
        _isStarted = true;

        Watch.Start();
    }

    public static TimeSpan Elapsed => _isStarted 
        ? Watch.Elapsed 
        : throw new Exception("Timer is not started");
}