using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class MarkdownSubstitutionMaker : IMarkdownSubstitutionMaker
{
    public string MakeSubstitutions(MarkdownSubstitutions substitutions, string template)
    {
        var outputText = template.Replace(
            "{date}", substitutions.DateText);

        return outputText;
    }
}
