using Todo.Contracts.Data.Substitutions;

namespace Todo.Contracts.Services.Templates;

public interface ISubstitutionsMaker<in T> where T : SubstitutionsBase
{
    string MakeSubstitutions(T substitutions, string template);
}
