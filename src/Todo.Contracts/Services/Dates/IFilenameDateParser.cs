namespace Todo.Contracts.Services.Dates;

public interface IFilenameDateParser
{
    bool TryParse(string fileName, out DateOnly date);
}