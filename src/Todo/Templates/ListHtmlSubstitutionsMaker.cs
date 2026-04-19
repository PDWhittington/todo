using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class ListHtmlSubstitutionsMaker(IFastUtf8Substitutor fastUtf8Substitutor) 
    : SubstitutionMakerBase(fastUtf8Substitutor), IListHtmlSubstitutionsMaker
{
    public string MakeSubstitutions(ListHtmlSubstitutions substitutions, string template)
        => template
            .Replace("{title}", substitutions.Title)
            .Replace("{body}", substitutions.Body)
            .Replace("{theme}", substitutions.Theme);

    public void WriteSubstitutionsToStream(byte[] template, ListHtmlSubstitutions substitutions, Stream stream)
    {
        var dict = new Dictionary<string, string>
        {
            { "title", substitutions.Title },
            { "body", substitutions.Body },
            { "theme", substitutions.Theme }
        };

        WriteSubstitutionsToStreamBase(template, dict, stream);
    }
}
