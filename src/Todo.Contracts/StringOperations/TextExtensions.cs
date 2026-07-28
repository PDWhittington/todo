using System.Runtime.CompilerServices;

namespace Todo.Contracts.StringOperations;

public static class TextExtensions
{
    extension(byte b)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsWhitespace() =>
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsLetterOrDigit() =>
            b switch
            {
                (byte)'A' => true, (byte)'B' => true, (byte)'C' => true, (byte)'D' => true,
                (byte)'E' => true, (byte)'F' => true, (byte)'G' => true, (byte)'H' => true,
                (byte)'I' => true, (byte)'J' => true, (byte)'K' => true, (byte)'L' => true,
                (byte)'M' => true, (byte)'N' => true, (byte)'O' => true, (byte)'P' => true,
                (byte)'Q' => true, (byte)'R' => true, (byte)'S' => true, (byte)'T' => true,
                (byte)'U' => true, (byte)'V' => true, (byte)'W' => true, (byte)'X' => true,
                (byte)'Y' => true, (byte)'Z' => true,
                (byte)'a' => true, (byte)'b' => true, (byte)'c' => true, (byte)'d' => true,
                (byte)'e' => true, (byte)'f' => true, (byte)'g' => true, (byte)'h' => true,
                (byte)'i' => true, (byte)'j' => true, (byte)'k' => true, (byte)'l' => true,
                (byte)'m' => true, (byte)'n' => true, (byte)'o' => true, (byte)'p' => true,
                (byte)'q' => true, (byte)'r' => true, (byte)'s' => true, (byte)'t' => true,
                (byte)'u' => true, (byte)'v' => true, (byte)'w' => true, (byte)'x' => true,
                (byte)'y' => true, (byte)'z' => true,
                (byte)'0' => true, (byte)'1' => true, (byte)'2' => true, (byte)'3' => true,
                (byte)'4' => true, (byte)'5' => true, (byte)'6' => true, (byte)'7' => true,
                (byte)'8' => true, (byte)'9' => true,
                _ => false
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDigit() =>
            b switch
            {
                (byte)'0' => true, (byte)'1' => true, (byte)'2' => true, (byte)'3' => true,
                (byte)'4' => true, (byte)'5' => true, (byte)'6' => true, (byte)'7' => true,
                (byte)'8' => true, (byte)'9' => true,
                _ => false
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualsIgnoreCase(byte other)
        {
            if (b == other) return true;

            return b switch
            {
                >= (byte)'a' and <= (byte)'z' => other == b + (byte)'A' - (byte)'a',
                >= (byte)'A' and <= (byte)'Z' => other == b + (byte)'a' - (byte)'A',
                _ => false
            };
        }
    }
}