using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class WhichTodoCommandExecutor : CommandExecutorBase<WhichTodoCommand>, IWhichTodoCommandExecutor
{
    private readonly IAssemblyInformationProvider _assemblyInformationProvider;
    private readonly IConstantsProvider _constantsProvider;

    public WhichTodoCommandExecutor(IOutputWriter outputWriter,
        IAssemblyInformationProvider assemblyInformationProvider,
        IConstantsProvider constantsProvider)
        : base(outputWriter)
    {
        _assemblyInformationProvider = assemblyInformationProvider;
        _constantsProvider = constantsProvider;
    }

    public override void Execute(WhichTodoCommand command)
    {
        var sb = new StringBuilder()
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

        OutputWriter.WriteLine(sb);
    }
}
