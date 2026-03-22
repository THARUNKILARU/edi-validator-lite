using EdiValidatorLite.Models;

namespace EdiValidatorLite.Services;

public sealed class X12Validator
{
    public ValidationResult Validate(X12ParseResult parseResult, string filePath)
    {
        var result = new ValidationResult
        {
            FileName = Path.GetFileName(filePath),
            TotalSegments = parseResult.Segments.Count,
            TransactionType = DetectTransactionType(parseResult.Segments)
        };

        ValidateEnvelope(parseResult.Segments, result);
        ValidateTransactionCounts(parseResult.Segments, result);
        ValidateTransactionSpecificRules(parseResult.Segments, result);

        return result;
    }

    private static string DetectTransactionType(List<string[]> segments)
    {
        var st = segments.FirstOrDefault(s => TagIs(s, "ST"));
        if (st == null || st.Length < 2)
        {
            return "Unknown";
        }

        return st[1] switch
        {
            "834" => "834 Benefit Enrollment",
            "835" => "835 Claim Payment/Advice",
            "837" => "837 Health Care Claim",
            _ => $"Unknown ({st[1]})"
        };
    }

    private static void ValidateEnvelope(List<string[]> segments, ValidationResult result)
    {
        if (!segments.Any(s => TagIs(s, "ISA")))
        {
            result.Errors.Add("Missing ISA segment.");
        }

        if (!segments.Any(s => TagIs(s, "IEA")))
        {
            result.Errors.Add("Missing IEA segment.");
        }

        if (!segments.Any(s => TagIs(s, "GS")))
        {
            result.Errors.Add("Missing GS segment.");
        }

        if (!segments.Any(s => TagIs(s, "GE")))
        {
            result.Errors.Add("Missing GE segment.");
        }

        if (!segments.Any(s => TagIs(s, "ST")))
        {
            result.Errors.Add("Missing ST segment.");
        }

        if (!segments.Any(s => TagIs(s, "SE")))
        {
            result.Errors.Add("Missing SE segment.");
        }
    }

    private static void ValidateTransactionCounts(List<string[]> segments, ValidationResult result)
    {
        var stCount = segments.Count(s => TagIs(s, "ST"));
        var seCount = segments.Count(s => TagIs(s, "SE"));
        var gsCount = segments.Count(s => TagIs(s, "GS"));
        var geCount = segments.Count(s => TagIs(s, "GE"));

        if (stCount != seCount)
        {
            result.Errors.Add($"ST/SE mismatch. ST={stCount}, SE={seCount}.");
        }

        if (gsCount != geCount)
        {
            result.Errors.Add($"GS/GE mismatch. GS={gsCount}, GE={geCount}.");
        }

        var ge = segments.FirstOrDefault(s => TagIs(s, "GE"));
        if (ge != null && ge.Length > 1 && int.TryParse(ge[1], out var geDeclared) && geDeclared != stCount)
        {
            result.Errors.Add($"GE01 count mismatch. Declared={geDeclared}, actual ST count={stCount}.");
        }

        var se = segments.FirstOrDefault(s => TagIs(s, "SE"));
        var stIndex = segments.FindIndex(s => TagIs(s, "ST"));
        var seIndex = segments.FindIndex(s => TagIs(s, "SE"));

        if (se != null && se.Length > 1 && stIndex >= 0 && seIndex >= stIndex)
        {
            var actualSegmentCount = seIndex - stIndex + 1;
            if (int.TryParse(se[1], out var declaredSegmentCount) && declaredSegmentCount != actualSegmentCount)
            {
                result.Errors.Add($"SE01 segment count mismatch. Declared={declaredSegmentCount}, actual={actualSegmentCount}.");
            }
        }
    }

    private static void ValidateTransactionSpecificRules(List<string[]> segments, ValidationResult result)
    {
        var st = segments.FirstOrDefault(s => TagIs(s, "ST"));
        var code = st != null && st.Length > 1 ? st[1] : string.Empty;

        switch (code)
        {
            case "834":
                Validate834(segments, result);
                break;
            case "835":
                Validate835(segments, result);
                break;
            case "837":
                Validate837(segments, result);
                break;
            default:
                result.Warnings.Add("Transaction type not supported for rule validation yet.");
                break;
        }
    }

    private static void Validate834(List<string[]> segments, ValidationResult result)
    {
        RequireSegment(segments, result, "BGN", "834 requires BGN.");
        RequireSegment(segments, result, "INS", "834 requires at least one INS member loop.");
        RequireAnySegment(segments, result, new[] { "REF", "NM1" }, "834 should contain REF or NM1 segments for identifying members/groups.");
        RequireAnySegment(segments, result, new[] { "HD" }, "834 should contain at least one HD coverage segment.");
    }

    private static void Validate835(List<string[]> segments, ValidationResult result)
    {
        RequireSegment(segments, result, "BPR", "835 requires BPR.");
        RequireSegment(segments, result, "TRN", "835 requires TRN.");
        RequireSegment(segments, result, "CLP", "835 requires at least one CLP.");
    }

    private static void Validate837(List<string[]> segments, ValidationResult result)
    {
        RequireSegment(segments, result, "BHT", "837 requires BHT.");
        RequireSegment(segments, result, "NM1", "837 requires at least one NM1.");
        RequireAnySegment(segments, result, new[] { "CLM", "SV1", "LX" }, "837 should contain claim/service detail segments like CLM, SV1, or LX.");
    }

    private static void RequireSegment(List<string[]> segments, ValidationResult result, string tag, string error)
    {
        if (!segments.Any(s => TagIs(s, tag)))
        {
            result.Errors.Add(error);
        }
    }

    private static void RequireAnySegment(List<string[]> segments, ValidationResult result, IEnumerable<string> tags, string error)
    {
        if (!segments.Any(s => tags.Any(tag => TagIs(s, tag))))
        {
            result.Errors.Add(error);
        }
    }

    private static bool TagIs(string[] segment, string tag)
    {
        return segment.Length > 0 && string.Equals(segment[0], tag, StringComparison.OrdinalIgnoreCase);
    }
}
