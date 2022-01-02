namespace Todo.Contracts.Services.Helpers;

public interface IPathHelper
{
    string GetRooted(string path);
    
    string GetAssemblyFolder();
    string GetAssemblyLocation();
}