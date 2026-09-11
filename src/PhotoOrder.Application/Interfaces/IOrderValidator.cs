namespace PhotoOrder.Application.Interfaces;

public interface IOrderValidator
{
    ValidationResult ValidateOrder(OrderValidationInput input);
}

public record OrderValidationInput(
    string? OrderNumber,
    List<OrderLineValidationInput> Lines,
    double OverallConfidence,
    bool HasLowConfidenceFields,
    bool HasOcrSubstitutions
);

public record OrderLineValidationInput(
    int RowNumber,
    string? ItemName,
    decimal? LengthMm,
    decimal? WidthMm,
    int? Quantity,
    List<string> FieldWarnings
);

public record ValidationResult(
    bool IsValid,
    bool RequiresReview,
    List<string> Errors,
    List<string> Warnings,
    List<FieldValidationResult> FieldResults
);

public record FieldValidationResult(
    string FieldName,
    int RowNumber,
    bool IsValid,
    bool RequiresReview,
    List<string> Reasons
);
