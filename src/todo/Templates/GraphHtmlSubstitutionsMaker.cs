using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class GraphHtmlSubstitutionsMaker : IGraphHtmlSubstitutionsMaker
{
    public string MakeSubstitutions(GraphHtmlSubstitutions substitutions, string template)
        => template
            .Replace("{title}", substitutions.Title)
            .Replace("{svg}", substitutions.Svg)
            .Replace("{timestamp}", substitutions.Timestamp);
}
