namespace Todo.Contracts.Data.Config;

public record PackageReferenceInfo(string Name, string Identity, string Version)
{
    public override string ToString() => $"{Name} | {Identity} | {Version}";
}