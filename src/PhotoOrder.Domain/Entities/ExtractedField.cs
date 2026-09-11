using PhotoOrder.Domain.Common;

namespace PhotoOrder.Domain.Entities;

public class ExtractedField : BaseEntity
{
    public string FieldName { get; private set; } = string.Empty;
    public string? RawValue { get; private set; }
    public string? NormalizedValue { get; private set; }
    public double Confidence { get; private set; }
    public bool RequiresReview { get; private set; }
    public string? ReviewReasons { get; private set; }
    
    public int? BboxX { get; private set; }
    public int? BboxY { get; private set; }
    public int? BboxWidth { get; private set; }
    public int? BboxHeight { get; private set; }
    
    public Guid OrderLineId { get; private set; }
    public OrderLine OrderLine { get; private set; } = null!;
    
    public Guid? RecognitionRunId { get; private set; }
    public RecognitionRun? RecognitionRun { get; private set; }
    
    public void SetValues(string fieldName, string? rawValue, string? normalizedValue, 
        double confidence, bool requiresReview, string? reviewReasons)
    {
        FieldName = fieldName;
        RawValue = rawValue;
        NormalizedValue = normalizedValue;
        Confidence = confidence;
        RequiresReview = requiresReview;
        ReviewReasons = reviewReasons;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetBbox(int x, int y, int width, int height)
    {
        BboxX = x;
        BboxY = y;
        BboxWidth = width;
        BboxHeight = height;
        UpdatedAt = DateTime.UtcNow;
    }
}
