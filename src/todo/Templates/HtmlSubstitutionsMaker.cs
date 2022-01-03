using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class HtmlSubstitutionsMaker : IHtmlSubstitutionsMaker
{
    public string MakeSubstitutions(HtmlSubstitutions substitutions, string template)
        => template
            .Replace("{title}", substitutions.Title)
            .Replace("{body}", substitutions.Body);
}
