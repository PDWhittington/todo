using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class ListHtmlSubstitutionsMaker : IListHtmlSubstitutionsMaker
{
    public string MakeSubstitutions(ListHtmlSubstitutions substitutions, string template)
        => template
            .Replace("{title}", substitutions.Title)
            .Replace("{body}", substitutions.Body)
            .Replace("{theme}", substitutions.Theme);
}
