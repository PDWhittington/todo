using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class DayListMarkdownSubstitutionsMaker(IFastUtf8Substitutor fastUtf8Substitutor) 
    : SubstitutionMakerBase(fastUtf8Substitutor), IDayListMarkdownSubstitutionsMaker
{
    public string MakeSubstitutions(DayListMarkdownSubstitutions substitutions, string template)
        => template.Replace("{date}", substitutions.DateText);

    public void WriteSubstitutionsToStream(byte [] template, DayListMarkdownSubstitutions substitutions, Stream stream)
    {
        var dict = new Dictionary<string, string>
        {
            {"date", substitutions.DateText}
        };
        
        WriteSubstitutionsToStreamBase(template, dict, stream);
    }
}
