# edi-validator-lite
# EDI Validator Lite

## Project Summary
EDI Validator Lite is a lightweight C# console application that reads X12 healthcare EDI files, validates core structural segments, detects transaction type, and generates a JSON validation report.

## Purpose
This project was built to validate the basic structure of healthcare EDI files before further processing. It focuses on practical X12 validation for common healthcare transactions such as 834, 835, and 837.

## Features
- Reads X12 EDI files from a local path
- Splits raw EDI content into segments
- Detects transaction type from the ST segment
- Validates ISA, IEA, GS, GE, ST, and SE segments
- Displays human-readable console output
- Generates a JSON validation report
- Supports basic validation for 834, 835, and 837 files

## Technologies Used
- C#
- .NET
- Console Application
- JSON Serialization
- X12 EDI

## Project Structure
- `Program.cs` - application entry point
- `Models/ValidationResult.cs` - stores validation result details
- `Services/X12Parser.cs` - reads and splits EDI content into segments
- `Services/X12Validator.cs` - performs structural validation and transaction detection
- `Samples/` - contains sample input files
- `Reports/` - contains generated JSON validation reports

## Sample Input
The application accepts a local X12 EDI file as input.

Example command:

```bash
dotnet run -- Samples/sample-834.edi
