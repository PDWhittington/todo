using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Template;

public interface ISubstitutionMaker
{
    string MakeSubstitutions(CreateOrShowCommand command, string template);
}