using PhotoOrder.Domain.Common;

namespace PhotoOrder.Domain.Entities;

public class FieldCorrection : BaseEntity
{
    public string OriginalValue { get; private set; } = string.Empty;
    public string CorrectedValue { get; private set; } = string.Empty;
    public string FieldName { get; private set; } = string.Empty;
    public bool ApprovedForTraining { get; private set; } = false;
    
    public Guid OrderLineId { get; private set; }
    public OrderLine OrderLine { get; private set; } = null!;
    
    public void SetValues(string originalValue, string correctedValue, string fieldName)
    {
        OriginalValue = originalValue;
        CorrectedValue = correctedValue;
        FieldName = fieldName;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetApprovedForTraining(bool approved)
    {
        ApprovedForTraining = approved;
        UpdatedAt = DateTime.UtcNow;
    }
}
