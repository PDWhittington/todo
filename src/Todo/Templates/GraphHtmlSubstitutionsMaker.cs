using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class GraphHtmlSubstitutionsMaker(IFastUtf8Substitutor fastUtf8Substitutor) 
    : SubstitutionMakerBase(fastUtf8Substitutor), IGraphHtmlSubstitutionsMaker
{
    public void WriteSubstitutionsToStream(UnmanagedByteArray template, GraphHtmlSubstitutions substitutions, Stream stream)
    {
        var dict = new Dictionary<string, string>
        {
            { "title", substitutions.Title },
            { "initialtheme", substitutions.InitialTheme },
            { "svg", substitutions.Svg },
            { "timestamp", substitutions.Timestamp }
        };

        WriteSubstitutionsToStreamBase(template, dict, stream);
    }
}