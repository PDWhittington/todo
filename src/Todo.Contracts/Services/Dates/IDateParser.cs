namespace Todo.Contracts.Services.Dates;

public interface IDateParser
{
    bool TryGetDate(string? str, out DateOnly dateOnly);

}
