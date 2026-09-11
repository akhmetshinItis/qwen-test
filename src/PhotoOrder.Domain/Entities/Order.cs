using PhotoOrder.Domain.Common;
using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public string? Material { get; private set; }
    public decimal? ThicknessMm { get; private set; }
    public DocumentStatus Status { get; private set; } = DocumentStatus.NeedsReview;
    public int Revision { get; private set; } = 1;
    
    public Guid SourceDocumentId { get; private set; }
    public SourceDocument SourceDocument { get; private set; } = null!;
    
    public List<OrderLine> Lines { get; private set; } = new();
    public List<RecognitionRun> RecognitionRuns { get; private set; } = new();
    
    public void SetHeaderInfo(string orderNumber, string? material, decimal? thicknessMm)
    {
        OrderNumber = orderNumber;
        Material = material;
        ThicknessMm = thicknessMm;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetStatus(DocumentStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void IncrementRevision()
    {
        Revision++;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AddLine(OrderLine line)
    {
        Lines.Add(line);
        UpdatedAt = DateTime.UtcNow;
    }
}
