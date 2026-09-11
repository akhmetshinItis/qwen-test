using PhotoOrder.Domain.Common;

namespace PhotoOrder.Domain.Entities;

public class OrderLine : BaseEntity
{
    public int RowNumber { get; private set; }
    public string? ItemName { get; private set; }
    public decimal? LengthMm { get; private set; }
    public decimal? WidthMm { get; private set; }
    public int? Quantity { get; private set; }
    public string? Material { get; private set; }
    public decimal? ThicknessMm { get; private set; }
    public string? Notes { get; private set; }
    
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    
    public List<ExtractedField> ExtractedFields { get; private set; } = new();
    public List<FieldCorrection> Corrections { get; private set; } = new();
    
    public void SetValues(int rowNumber, string? itemName, decimal? lengthMm, decimal? widthMm, 
        int? quantity, string? material, decimal? thicknessMm, string? notes)
    {
        RowNumber = rowNumber;
        ItemName = itemName;
        LengthMm = lengthMm;
        WidthMm = widthMm;
        Quantity = quantity;
        Material = material;
        ThicknessMm = thicknessMm;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AddCorrection(FieldCorrection correction)
    {
        Corrections.Add(correction);
        UpdatedAt = DateTime.UtcNow;
    }
}
