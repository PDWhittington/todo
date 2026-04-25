using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class DayListMarkdownSubstitutionsMaker : IDayListMarkdownSubstitutionsMaker
{
    public string MakeSubstitutions(DayListMarkdownSubstitutions substitutions, string template)
        => template.Replace("{date}", substitutions.DateText);
}
