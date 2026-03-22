using System.Collections.Generic;

namespace EdiValidatorLite.Models
{
    public class ValidationResult
    {
        public string FileName { get; set; } = string.Empty;
        public string TransactionType { get; set; } = "Unknown";
        public int SegmentCount { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
