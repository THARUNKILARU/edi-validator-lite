# EDI Validator Lite

A simple C# console app that validates basic X12 structure for healthcare EDI files.

## Why this project
This is a strong first GitHub project for a healthcare EDI professional because it shows:
- X12 file parsing
- C# console app development
- validation logic
- JSON reporting
- healthcare transaction awareness

## Supported checks
### Envelope checks
- ISA / IEA presence
- GS / GE presence
- ST / SE presence
- ST/SE count match
- GS/GE count match
- GE01 transaction count validation
- SE01 segment count validation

### Transaction detection
- 834 Benefit Enrollment
- 835 Claim Payment/Advice
- 837 Health Care Claim

### Minimal transaction-specific checks
- 834: BGN, INS, HD, REF or NM1
- 835: BPR, TRN, CLP
- 837: BHT, NM1, CLM/SV1/LX

## Project structure
```text
edi-validator-lite/
├── edi-validator-lite.csproj
├── Program.cs
├── Models/
│   └── ValidationResult.cs
├── Services/
│   ├── X12Parser.cs
│   └── X12Validator.cs
├── Samples/
│   ├── sample-834.edi
│   └── sample-bad-834.edi
└── README.md
```

## Run locally
```bash
dotnet run -- Samples/sample-834.edi
```

## Example output
```text
EDI VALIDATION SUMMARY
File              : sample-834.edi
Transaction Type  : 834 Benefit Enrollment
Segment Count     : 12
Valid             : True
Errors            : 0
Warnings          : 0
```

## Good next upgrades
- Add 270/271 support
- Add 999/TA1 validation
- Validate control numbers
- Export CSV report
- Add unit tests
- Add loop-level business rule validation
- Make rules config-driven from JSON

## Best GitHub description
C# healthcare EDI validator for X12 834/835/837 files with basic envelope checks, transaction detection, and JSON validation reports.
