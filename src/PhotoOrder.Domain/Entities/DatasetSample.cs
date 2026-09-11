using PhotoOrder.Domain.Common;

namespace PhotoOrder.Domain.Entities;

public class DatasetSample : BaseEntity
{
    public string ImagePath { get; private set; } = string.Empty;
    public string Label { get; private set; } = string.Empty;
    public string Split { get; private set; } = string.Empty; // train, validation, test
    public bool ApprovedForTraining { get; private set; } = false;
    
    public Guid? SourceDocumentId { get; private set; }
    public SourceDocument? SourceDocument { get; private set; }
    
    public Guid? DatasetVersionId { get; private set; }
    public DatasetVersion? DatasetVersion { get; private set; }
    
    public byte[]? CropImageData { get; private set; }
    
    public void SetValues(string imagePath, string label, string split)
    {
        ImagePath = imagePath;
        Label = label;
        Split = split;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetApproved(bool approved)
    {
        ApprovedForTraining = approved;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetCropData(byte[] imageData)
    {
        CropImageData = imageData;
        UpdatedAt = DateTime.UtcNow;
    }
}
