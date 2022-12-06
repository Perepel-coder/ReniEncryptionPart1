using System.Collections.Generic;

namespace Models
{
    public class ClassificationByNumberOfKeys
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Specificity { get; set; } = string.Empty;
        public List<TableOfCharacteristics> TableOfCharacteristics { get; set; } = new();
    }
    public class ClassificationBySpecOfInformProcess
    {
        public int Id { get; set; }
        public string Class { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Specificity { get; set; } = string.Empty;
        public List<TableOfCharacteristics> TableOfCharacteristics { get; set; } = new();
    }
    public class ReplacementMode
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string NameOfMode { get; set; } = string.Empty;
        public List<TableOfCharacteristics> TableOfCharacteristics { get; set; } = new();
    }
    public class TableOfCharacteristics
    {
        public int Id { get; set; }

        public int ClassificationByNumberOfKeysId { get; set; }
        public ClassificationByNumberOfKeys? NumberOfKeys { get; set; }

        public int ClassificationBySpecOfInformProcessId { get; set; }
        public ClassificationBySpecOfInformProcess? InformProcess { get; set; }

        public int ReplacementModeId { get; set; }
        public ReplacementMode? Mode { get; set; }

        public int? CryptoAlgorithmsTableId { get; set; }
        public CryptoAlgorithmsTable? CryptoAlgorithmsTable { get; set; }
    }
    public class CryptoAlgorithmsTable
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string NameOfAlg { get; set; } = string.Empty;
        public TableOfCharacteristics? TableOfCharacteristics { get; set; }
    }
}
