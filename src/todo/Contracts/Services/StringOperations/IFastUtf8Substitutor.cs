using System.Collections.Generic;
using System.IO;

namespace Todo.Contracts.Services.StringOperations;

public interface IFastUtf8Substitutor
{
    public void CopyToStream(byte[] template,
        Dictionary<string, string> substitutions, Stream outputStream);
}