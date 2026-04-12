using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;
using Todo.FileSystem;

namespace Todo.Dates;

public class FilenameDateParser : IFilenameDateParser
{
    private readonly Regex _regex;
    private readonly string _dateFormat;   // e.g. "yyyy-MM-dd"
    
    public FilenameDateParser(IConfigurationProvider configurationProvider)
    {
        var template = configurationProvider
            .ConfigInfo.Configuration.TodoListFilenameFormatWithoutExension + $".{FileTypes.MarkdownExtension}";
        
        if (string.IsNullOrWhiteSpace(template))
            throw new ArgumentException("Template cannot be empty.");

        // Find the single {date-format} placeholder (exactly as in your example)
        var openBrace = template.IndexOf('{');
        var closeBrace = template.IndexOf('}', openBrace + 1);

        if (openBrace == -1 || closeBrace == -1 || openBrace >= closeBrace)
            throw new ArgumentException("Template must contain exactly one {date-format} placeholder.");

        string prefix  = template.Substring(0, openBrace);
        _dateFormat    = template.Substring(openBrace + 1, closeBrace - openBrace - 1);
        string suffix  = template.Substring(closeBrace + 1);

        // Build the regex: ^literal-(.+?)\.literal$
        var pattern = new StringBuilder("^")
            .Append(Regex.Escape(prefix))
            .Append("(.+?)")                    // capture group 1 = the date string
            .Append(Regex.Escape(suffix))
            .Append('$')
            .ToString();

        _regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
    
    public bool TryParse(string fileName, out DateOnly date)
    {
        var match = _regex.Match(fileName);

        if (!match.Success)
        {
            date = default;
            return false;
        }

        var dateStr = match.Groups[1].Value;   // the part that was inside the {}
        
        var parseSuccess = DateOnly.TryParseExact(
            dateStr,
            _dateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var dateParsed);

        date = parseSuccess ? dateParsed : default;
        
        return parseSuccess;
    }
}