using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class BoilerPlateProvider(
    IAssemblyInformationProvider assemblyInformationProvider,
    IConstantsProvider constantsProvider)
    : IBoilerPlateProvider
{
    public string GetBoilerPlate()
    {
        var sb = new StringBuilder();
        MakeBoilerPlate(sb);
        return sb.ToString();
    }

    public void MakeBoilerPlate(StringBuilder sb)
    {
        sb
            .AppendLine($"Assembly location: {assemblyInformationProvider.AssemblyLocation()}")
            .AppendLine($"Todo version (commit): {assemblyInformationProvider.GetCommitHash()}")
            .AppendLine($"Build time: {assemblyInformationProvider.GetBuildTime().ToString("yyyy-MM-dd HH:mm:ss")}")
            .AppendLine($"DEBUG flag: {assemblyInformationProvider.DebugFlag()}")
            .AppendLine($"Process architecture: {RuntimeInformation.ProcessArchitecture}")
            .AppendLine()
            .AppendLine($"Framework version: {RuntimeInformation.FrameworkDescription}")
            .AppendLine($"OS description: {RuntimeInformation.OSDescription}")
            .AppendLine($"OS architecture: {RuntimeInformation.OSArchitecture}")
            .AppendLine()
            .AppendLine($"Project author: {constantsProvider.ProjectAuthor} " + 
                $"({constantsProvider.ProjectAuthorContactDetails})")
            .AppendLine($"Project website: {constantsProvider.ProjectWebsite}")
            .AppendLine();
    }
}
