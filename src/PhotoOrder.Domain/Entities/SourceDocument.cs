using PhotoOrder.Domain.Common;
using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Domain.Entities;

public class SourceDocument : BaseEntity
{
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public string StoragePath { get; private set; } = string.Empty;
    public int? ImageWidth { get; private set; }
    public int? ImageHeight { get; private set; }
    public DocumentStatus Status { get; private set; } = DocumentStatus.Uploaded;
    public string? ErrorMessage { get; private set; }
    
    public Guid? OrderId { get; private set; }
    public Order? Order { get; private set; }
    
    public List<RecognitionRun> RecognitionRuns { get; private set; } = new();
    public List<DatasetSample> DatasetSamples { get; private set; } = new();
    
    public void SetImageDimensions(int width, int height)
    {
        ImageWidth = width;
        ImageHeight = height;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetStatus(DocumentStatus status, string? errorMessage = null)
    {
        Status = status;
        ErrorMessage = errorMessage;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void LinkToOrder(Guid orderId)
    {
        OrderId = orderId;
        UpdatedAt = DateTime.UtcNow;
    }
}
