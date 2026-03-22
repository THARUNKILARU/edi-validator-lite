namespace EdiValidatorLite.Services;

public sealed class X12ParseResult
{
    public List<string[]> Segments { get; init; } = new();
}

public sealed class X12Parser
{
    public X12ParseResult Parse(string rawText)
    {
        if (string.IsNullOrWhiteSpace(rawText))
        {
            throw new InvalidOperationException("EDI file is empty.");
        }

        var cleaned = rawText
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty)
            .Trim();

        var rawSegments = cleaned
            .Split('~', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var segments = new List<string[]>();

        foreach (var segment in rawSegments)
        {
            var elements = segment.Split('*');
            if (elements.Length > 0 && !string.IsNullOrWhiteSpace(elements[0]))
            {
                segments.Add(elements);
            }
        }

        return new X12ParseResult
        {
            Segments = segments
        };
    }
}
