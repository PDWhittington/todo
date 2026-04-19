using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Services.StringOperations;

namespace Todo.Templates;

public abstract class SubstitutionMakerBase(IFastUtf8Substitutor fastUtf8Substitutor)
{
    protected void WriteSubstitutionsToStreamBase(byte [] template, 
        Dictionary<string, string> substitutions, Stream stream)
    {
        fastUtf8Substitutor.CopyToStream(template, substitutions, stream);
    }
}