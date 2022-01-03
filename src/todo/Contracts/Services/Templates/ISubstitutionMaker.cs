using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Substitutions;

namespace Todo.Contracts.Services.Templates;

public interface ISubstitutionMaker<T> where T : SubstitutionsBase
{
    string MakeSubstitutions(T substitutions, string template);
}
