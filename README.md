# EDI Validator Lite

A lightweight C# console application for validating the structural integrity of X12 healthcare EDI files. The project focuses on core envelope validation, transaction detection, segment count verification, and basic transaction-specific checks for common healthcare transaction sets.

## Overview
EDI Validator Lite was built as a practical foundation for healthcare EDI validation workflows. It is designed to read raw X12 files, identify the transaction type, validate key structural segments, and produce a clear validation summary. In addition to console output, the project can be extended to support structured JSON reporting for downstream review and automation.

This project reflects real-world healthcare EDI work by focusing on the kinds of structural issues that commonly cause transaction failures, file rejections, or downstream processing problems.

## Objectives
The primary goals of this project are to:

- parse raw X12 healthcare EDI files
- validate core interchange, functional group, and transaction set structure
- identify supported healthcare transaction types
- perform essential segment count and balancing checks
- provide readable validation output for technical review
- create a reusable starting point for more advanced EDI validation tooling

## Supported Transaction Types
The validator currently supports detection of the following healthcare transaction sets:

- **834** Benefit Enrollment and Maintenance
- **835** Health Care Claim Payment / Advice
- **837** Health Care Claim

## Validation Scope

### Envelope Validation
The application performs the following structural checks:

- ISA / IEA presence validation
- GS / GE presence validation
- ST / SE presence validation
- ST / SE count matching
- GS / GE count matching
- GE01 transaction count validation
- SE01 segment count validation

### Transaction Detection
The application identifies the transaction type from the `ST` segment and maps it to a healthcare transaction description.

### Minimal Transaction-Specific Validation
Basic transaction-level checks are included to ensure key segments exist for supported transaction types.

#### 834 Benefit Enrollment
Checks for the presence of:
- `BGN`
- `INS`
- `HD`
- `REF` or `NM1`

#### 835 Claim Payment / Advice
Checks for the presence of:
- `BPR`
- `TRN`
- `CLP`

#### 837 Health Care Claim
Checks for the presence of:
- `BHT`
- `NM1`
- `CLM`, `SV1`, or `LX`

## Key Features
- Reads X12 EDI files from a local file path
- Splits raw EDI data into individual segments
- Detects healthcare transaction type automatically
- Validates core X12 structural envelopes
- Verifies segment balancing and control counts
- Performs minimal transaction-specific checks
- Produces a clear validation summary in the console
- Provides a solid base for future JSON, CSV, or database-driven reporting
