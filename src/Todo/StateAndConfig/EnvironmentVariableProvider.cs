using System;
using System.Collections.Generic;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class EnvironmentVariableProvider : IEnvironmentVariableProvider
{
    private readonly IDictionary<string, object> _environmentVariables;

    public EnvironmentVariableProvider()
    {
        var variables = Environment.GetEnvironmentVariables();
        var dict = new Dictionary<string, object>();

        foreach (var key in variables.Keys)
        {
            if (key is null) continue;
            
            var val =  variables[key];
            
            if (val is null) continue;
            
            dict.Add(key.ToString()!, val);
        }
        
        _environmentVariables = dict;
    }
    
    public bool TryGetEnvironmentVariable(string? key, out string? value)
    {
        if (key is not null && _environmentVariables.TryGetValue(key, out var obj))
        {
            value = obj?.ToString();
            return value != null;
        }
        
        value = null;
        return false;
    }
}