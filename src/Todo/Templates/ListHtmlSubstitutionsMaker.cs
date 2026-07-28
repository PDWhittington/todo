using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class ListHtmlSubstitutionsMaker(IFastUtf8Substitutor fastUtf8Substitutor) 
    : SubstitutionMakerBase(fastUtf8Substitutor), IListHtmlSubstitutionsMaker
{
    public void WriteSubstitutionsToStream(UnmanagedByteArray template, ListHtmlSubstitutions substitutions, Stream stream)
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