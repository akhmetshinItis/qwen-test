using PhotoOrder.Domain.Common;
using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Domain.Entities;

public class RecognitionRun : BaseEntity
{
    public Guid SourceDocumentId { get; private set; }
    public SourceDocument SourceDocument { get; private set; } = null!;
    
    public Guid? OrderId { get; private set; }
    public Order? Order { get; private set; }
    
    public RecognitionProvider Provider { get; private set; }
    public string ModelVersion { get; private set; } = string.Empty;
    public string ConfigHash { get; private set; } = string.Empty;
    
    public TimeSpan ProcessingTimeMs { get; private set; }
    public int LinesDetected { get; private set; }
    public double AverageConfidence { get; private set; }
    
    public List<ExtractedField> ExtractedFields { get; private set; } = new();
    
    public void SetResults(RecognitionProvider provider, string modelVersion, string configHash,
        TimeSpan processingTime, int linesDetected, double avgConfidence)
    {
        Provider = provider;
        ModelVersion = modelVersion;
        ConfigHash = configHash;
        ProcessingTimeMs = processingTime;
        LinesDetected = linesDetected;
        AverageConfidence = avgConfidence;
        UpdatedAt = DateTime.UtcNow;
    }
}
