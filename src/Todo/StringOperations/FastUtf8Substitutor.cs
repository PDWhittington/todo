using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.StringOperations;

namespace Todo.StringOperations;

public unsafe class FastUtf8Substitutor : IFastUtf8Substitutor
{
    public void CopyToStream(UnmanagedByteArray template, Dictionary<string, string> substitutions, Stream outputStream)
    {
        var insideBrackets = false;
        var copyFrom = 0;
        var lastOpenBracketIndex = -1;
        
        for (var i = 0; i < template.Length; i++)
        {
            var b = template.GetByte(i);
            
            if (b == (byte)'{')
            {
                insideBrackets = true;
                lastOpenBracketIndex = i;
                continue;
            }

            if (!insideBrackets) continue;

            if (b.IsWhitespace())
            {
                insideBrackets = false;
                continue;
            }

            if (b != '}') continue;

            //Create unicode string for lookup (for now)
            var key = GetString(template, lastOpenBracketIndex + 1, i - lastOpenBracketIndex - 1);
            
            if (substitutions.TryGetValue(key, out var substitution))
            {
                var span = new ReadOnlySpan<byte>((void*)(template.Start + copyFrom), lastOpenBracketIndex - copyFrom);
                outputStream.Write(span);

                foreach (var c in substitution)
                {
                    outputStream.WriteByte((byte)c);
                }
                
                copyFrom = i + 1;
            }
            
            insideBrackets = false;

        }
        
        var finalSpan = new ReadOnlySpan<byte>((void*)(template.Start + copyFrom), template.Length - copyFrom);
        outputStream.Write(finalSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetString(UnmanagedByteArray arr, int index, int length)
    {
        return Encoding.UTF8.GetString((byte*)arr.Start + index, length);
    }
}