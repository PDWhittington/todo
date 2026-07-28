using Todo.Contracts.Data.Config;

namespace Todo.Contracts.Services.AppLaunching;

public interface ILaunchInfoSelector
{
    ProcessLaunchInfo SelectLaunchInfoForThisOs(PerOsLaunchInfos perOsInfos);
}