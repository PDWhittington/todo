namespace Todo.Contracts.Services;

public interface IPathHelper
{
    string GetRooted(string path);
    
    string GetAssemblyFolder();
    string GetAssemblyLocation();
}