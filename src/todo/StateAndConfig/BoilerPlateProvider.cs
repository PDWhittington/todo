using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class BoilerPlateProvider : IBoilerPlateProvider
{
    private readonly IAssemblyInformationProvider _assemblyInformationProvider;
    private readonly IConstantsProvider _constantsProvider;

    public BoilerPlateProvider(IAssemblyInformationProvider assemblyInformationProvider,
        IConstantsProvider constantsProvider)
    {
        _assemblyInformationProvider = assemblyInformationProvider;
        _constantsProvider = constantsProvider;
    }

    public string GetBoilerPlate()
    {
        var sb = new StringBuilder();
        MakeBoilerPlate(sb);
        return sb.ToString();
    }

    public void MakeBoilerPlate(StringBuilder sb)
    {
        sb
            .AppendLine($"Assembly location: {_assemblyInformationProvider.AssemblyLocation()}")
            .AppendLine($"Todo version (commit): {_assemblyInformationProvider.GetCommitHash()}")
            .AppendLine($"Build time: {_assemblyInformationProvider.GetBuildTime().ToString("yyyy-MM-dd HH:mm:ss")}")
            .AppendLine($"DEBUG flag: {_assemblyInformationProvider.DebugFlag()}")
            .AppendLine($"Process architecture: {RuntimeInformation.ProcessArchitecture}")
            .AppendLine()
            .AppendLine($"Framework version: {RuntimeInformation.FrameworkDescription}")
            .AppendLine($"OS architecture: {RuntimeInformation.OSArchitecture}")
            .AppendLine($"OS description: {RuntimeInformation.OSDescription}")
            .AppendLine()
            .AppendLine($"Project author: {_constantsProvider.ProjectAuthor} " + 
                $"({_constantsProvider.ProjectAuthorContactDetails})")
            .AppendLine($"Project website: {_constantsProvider.ProjectWebsite}")
            .AppendLine();
    }
}
