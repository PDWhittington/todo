using System;
using System.Runtime.CompilerServices;
using System.Text;
using Todo.Contracts.StringOperations;

namespace Todo.Contracts.Data.Memory;

public readonly unsafe record struct ByteArraySpan(IntPtr Start, int Length)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte GetByte(int i) => *((byte*)Start + i);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    
    public bool EndsWith(byte value) =>
        *((byte*)Start + Length - 1) == value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    
    public bool TryIntParseAllButLast(out int value)
    {
        var val = 0;

        for (var i = 0; i < Length - 1; i++)
        {
            var b = GetByte(i);
            
            if (!b.IsDigit())
            {
                value = 0;
                return false;
            }
            
            val *= 10;
            val += b - (byte)'0';
        }
        
        value = val;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ByteArraySpan TrimStart() =>
        TrimStart(b => b.IsWhitespace());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ByteArraySpan TrimStart(Func<byte, bool> predicate)
    {
        for (var i = 0; i < Length; i++)
        {
            if (!predicate(GetByte(i)))
            {
                return new ByteArraySpan(Start + i, Length - i);
            }
        }
        
        return new ByteArraySpan(IntPtr.Zero, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ByteArraySpan TrimEnd() =>
        TrimEnd(b => b.IsWhitespace());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ByteArraySpan TrimEnd(Func<byte, bool> predicate)
    {
        for (var i = Length - 1; i >= 0; i--)
        {
            if (!predicate(GetByte(i)))
            {
                return new ByteArraySpan(Start, i + 1);
            }
        }
        
        return new ByteArraySpan(IntPtr.Zero, 0);
    }

    public bool EqualsIgnoreCase(string other)
    {
        if (other.Length != Length) return false;

        for (var i = 0; i < Length; i++)
        {   
            if (!GetByte(i).EqualsIgnoreCase((byte)other[i])) return false;
        }
        
        return true;
    }
    
    public override string ToString() =>
        Encoding.UTF8.GetString((byte*)Start, Length);
}