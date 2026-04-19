using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class TopicListMarkdownSubstitutionsMaker(IFastUtf8Substitutor fastUtf8Substitutor) 
    : SubstitutionMakerBase(fastUtf8Substitutor), ITopicListMarkdownSubstitutionsMaker
{
    public string MakeSubstitutions(TopicListMarkdownSubstitutions substitutions, string template)
        => template.Replace("{topic}", substitutions.TopicName);

    public void WriteSubstitutionsToStream(byte[] template, TopicListMarkdownSubstitutions substitutions, Stream stream)
    {
        var dict = new Dictionary<string, string>
        {
            { "topic", substitutions.TopicName },
        };
        
        WriteSubstitutionsToStreamBase(template, dict, stream);
    }
}
