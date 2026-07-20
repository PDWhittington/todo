using Todo.Contracts.Data.Config;

namespace Todo.Contracts.Services.AppLaunching;

public interface ILaunchInfoSelector
{
    ProcessLaunchInfo SelectLaunchInfoForThisOS(PerOsLaunchInfos perOsInfos);
}