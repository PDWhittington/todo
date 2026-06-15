using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Todo.Contracts.Data.Memory;

public readonly unsafe record struct UnmanagedByteArray
{
    public GCHandle Handle { get; }
    public IntPtr Start { get; }
    public int Length { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte GetByte(int offset) => *((byte*)Start + offset);
    
    private UnmanagedByteArray(GCHandle handle, IntPtr start, int length)
    {
        Handle = handle;
        Start = start;
        Length = length;
    }
    
    public static UnmanagedByteArray Of(GCHandle handle, IntPtr start, int length)
        => new (handle, start, length);
}