using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class TopicListMarkdownSubstitutionsMaker : ITopicListMarkdownSubstitutionsMaker
{
    public string MakeSubstitutions(TopicListMarkdownSubstitutions substitutions, string template)
        => template.Replace("{topic}", substitutions.TopicName);

}
