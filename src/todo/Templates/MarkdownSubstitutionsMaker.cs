using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class MarkdownSubstitutionsMaker : IMarkdownSubstitutionsMaker
{
    public string MakeSubstitutions(MarkdownSubstitutions substitutions, string template)
        => template
            .Replace("{date}", substitutions.DateText);
}
