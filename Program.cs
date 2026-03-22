using System.Text.Json;
using EdiValidatorLite.Services;

if (args.Length == 0)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run -- <path-to-x12-file>");
    Console.WriteLine();
    Console.WriteLine("Example:");
    Console.WriteLine("  dotnet run -- Samples/sample-834.edi");
    return;
}

var filePath = args[0];

if (!File.Exists(filePath))
{
    Console.WriteLine($"File not found: {filePath}");
    return;
}

try
{
    var rawText = await File.ReadAllTextAsync(filePath);
    var parser = new X12Parser();
    var validator = new X12Validator();

    var parseResult = parser.Parse(rawText);
    var validationResult = validator.Validate(parseResult, filePath);

    Console.WriteLine("========================================");
    Console.WriteLine("EDI VALIDATION SUMMARY");
    Console.WriteLine("========================================");
    Console.WriteLine($"File              : {validationResult.FileName}");
    Console.WriteLine($"Transaction Type  : {validationResult.TransactionType}");
    Console.WriteLine($"Segment Count     : {validationResult.TotalSegments}");
    Console.WriteLine($"Valid             : {validationResult.IsValid}");
    Console.WriteLine($"Errors            : {validationResult.Errors.Count}");
    Console.WriteLine($"Warnings          : {validationResult.Warnings.Count}");
    Console.WriteLine();

    if (validationResult.Errors.Count > 0)
    {
        Console.WriteLine("Errors:");
        foreach (var error in validationResult.Errors)
        {
            Console.WriteLine($"  - {error}");
        }
        Console.WriteLine();
    }

    if (validationResult.Warnings.Count > 0)
    {
        Console.WriteLine("Warnings:");
        foreach (var warning in validationResult.Warnings)
        {
            Console.WriteLine($"  - {warning}");
        }
        Console.WriteLine();
    }

    var reportPath = Path.Combine(
        Path.GetDirectoryName(Path.GetFullPath(filePath)) ?? Environment.CurrentDirectory,
        $"validation-report-{DateTime.Now:yyyyMMdd-HHmmss}.json");

    var json = JsonSerializer.Serialize(validationResult, new JsonSerializerOptions
    {
        WriteIndented = true
    });

    await File.WriteAllTextAsync(reportPath, json);
    Console.WriteLine($"JSON report saved: {reportPath}");
}
catch (Exception ex)
{
    Console.WriteLine("Validation failed.");
    Console.WriteLine(ex.Message);
}
