using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Todo.Contracts.Services.StringOperations;

namespace Todo.StringOperations;

public class FastUtf8Substitutor : IFastUtf8Substitutor
{
    public void CopyToStream(byte[] template, Dictionary<string, string> substitutions, Stream outputStream)
    {
        var insideBrackets = false;
        var copyFrom = 0;
        var lastOpenBracketIndex = -1;
        
        for (var i = 0; i < template.Length; i++)
        {
            var b = template[i];
            
            if (b == (byte)'{')
            {
                insideBrackets = true;
                lastOpenBracketIndex = i;
                continue;
            }

            if (!insideBrackets) continue;

            if (IsWhitespace(b))
            {
                insideBrackets = false;
                continue;
            }

            if (b != '}') continue;

            //Create unicode string for lookup (for now)
            var key = GetString(template, lastOpenBracketIndex + 1, i - lastOpenBracketIndex - 1);
            
            if (substitutions.TryGetValue(key, out var substitution))
            {
                outputStream.Write(template, copyFrom, lastOpenBracketIndex - copyFrom);

                foreach (var c in substitution)
                {
                    outputStream.WriteByte((byte)c);
                }
                
                copyFrom = i + 1;
            }
            
            insideBrackets = false;

        }
        
        outputStream.Write(template, copyFrom, template.Length - copyFrom);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetString(byte[] arr, int index, int length)
    {
        var keyArr = arr
            .Skip(index)
            .Take(length)
            .Select(b => (char)b)
            .ToArray();

        return new string(keyArr);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsWhitespace(byte b) =>
        b switch
        {
            0x09 => true,              // CHARACTER TABULATION \t    
            0x0a => true,              // LINE FEED (LF) \n
            0x0B => true,              // LINE TABULATION (VT) \v         
            0x0C => true,              // FORM FEED (FF) \f         
            0x0D => true,              // CARRIAGE RETURN (CR) \r
            0x20 => true,              // SPACE                       
            0x85 => true,              // NEXT LINE (NEL)         
            _ => false
        };

}