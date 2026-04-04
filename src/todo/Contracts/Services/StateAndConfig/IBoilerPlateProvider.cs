using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Todo.Contracts.Services.StateAndConfig;

public interface IBoilerPlateProvider
{
    void MakeBoilerPlate(StringBuilder sb);

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    string GetBoilerPlate();
}
