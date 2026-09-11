namespace PhotoOrder.Application.Interfaces;

public interface IOrderExtractor
{
    Task<OrderExtractionResult> ExtractFromLinesAsync(
        List<TextLineResult> lines,
        ExtractionContext context,
        CancellationToken ct = default);
}

public record ExtractionContext(
    string? KnownOrderNumber = null,
    string? KnownMaterial = null,
    decimal? KnownThicknessMm = null
);

public record OrderExtractionResult(
    string? OrderNumber,
    string? Material,
    decimal? ThicknessMm,
    List<OrderLineExtraction> Lines,
    List<string> Warnings
);

public record OrderLineExtraction(
    int RowNumber,
    string? ItemName,
    decimal? LengthMm,
    decimal? WidthMm,
    int? Quantity,
    string? Material,
    decimal? ThicknessMm,
    string? Notes,
    List<ExtractedFieldData> Fields,
    List<string> Warnings
);

public record ExtractedFieldData(
    string FieldName,
    string? RawValue,
    string? NormalizedValue,
    double Confidence,
    bool RequiresReview,
    string? ReviewReasons,
    int? BboxX,
    int? BboxY,
    int? BboxWidth,
    int? BboxHeight
);
