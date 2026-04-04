using Markdig.Helpers;

namespace Todo.StringOperations;

public struct CustomStringSection
{
    public string ParentString { get; }
    public int StartIndex { get; }
    public int SectionLength { get; }
    
    private CustomStringSection(string parentString, int startIndex, int sectionLength)
    {
        ParentString = parentString;
        StartIndex = startIndex;
        SectionLength = sectionLength;
    }

    public bool EndsWith(char ch) => ParentString[StartIndex + SectionLength - 1] == ch;

    public bool TryIntParseAllButLast(out int value)
    {
        var val = 0;

        for (var i = 0; i < SectionLength - 1; i++)
        {
            var currentChar = ParentString[StartIndex + i];
            
            if (!currentChar.IsDigit())
            {
                value = 0;
                return false;
            }
            
            val *= 10;
            val += currentChar - '0';
        }
        
        value = val;
        return true;
    }

    public override string ToString() => ParentString.Substring(StartIndex, SectionLength);
    
    public static CustomStringSection Of(string parentString, int startIndex, int sectionLength)
        => new (parentString, startIndex, sectionLength);
}