namespace Todo.Contracts.Services.AppLaunching;

public interface IAppLauncher
{
    void LaunchFiles(params string [] paths);
}
