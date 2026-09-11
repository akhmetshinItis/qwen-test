using PhotoOrder.Application.Interfaces;

namespace PhotoOrder.Application.Services;

public class OrderValidator : IOrderValidator
{
    public ValidationResult ValidateOrder(OrderValidationInput input)
    {
        var errors = new List<string>();
        var warnings = new List<string>();
        var fieldResults = new List<FieldValidationResult>();
        
        // Check order number
        if (string.IsNullOrEmpty(input.OrderNumber))
        {
            warnings.Add("Номер заявки не распознан");
        }
        
        // Validate each line
        foreach (var line in input.Lines)
        {
            var lineErrors = new List<string>();
            var lineWarnings = new List<string>(line.FieldWarnings);
            
            // Required fields validation
            if (string.IsNullOrEmpty(line.ItemName))
            {
                lineErrors.Add($"Строка {line.RowNumber}: не указано наименование");
            }
            
            if (!line.LengthMm.HasValue || line.LengthMm <= 0)
            {
                lineErrors.Add($"Строка {line.RowNumber}: некорректная длина");
            }
            else if (line.LengthMm > 10000)
            {
                lineWarnings.Add($"Строка {line.RowNumber}: подозрительно большая длина ({line.LengthMm}мм)");
            }
            
            if (!line.WidthMm.HasValue || line.WidthMm <= 0)
            {
                lineErrors.Add($"Строка {line.RowNumber}: некорректная ширина");
            }
            else if (line.WidthMm > 5000)
            {
                lineWarnings.Add($"Строка {line.RowNumber}: подозрительно большая ширина ({line.WidthMm}мм)");
            }
            
            if (!line.Quantity.HasValue || line.Quantity <= 0)
            {
                lineErrors.Add($"Строка {line.RowNumber}: некорректное количество");
            }
            else if (line.Quantity > 1000)
            {
                lineWarnings.Add($"Строка {line.RowNumber}: подозрительно большое количество ({line.Quantity})");
            }
            
            // Add field results
            fieldResults.Add(new FieldValidationResult(
                "ItemName", line.RowNumber, !string.IsNullOrEmpty(line.ItemName), 
                string.IsNullOrEmpty(line.ItemName), lineErrors.Where(e => e.Contains("наименование")).ToList()));
            
            fieldResults.Add(new FieldValidationResult(
                "LengthMm", line.RowNumber, line.LengthMm.HasValue && line.LengthMm > 0,
                !line.LengthMm.HasValue || line.LengthMm <= 0, lineWarnings.Where(w => w.Contains("длину")).ToList()));
            
            fieldResults.Add(new FieldValidationResult(
                "WidthMm", line.RowNumber, line.WidthMm.HasValue && line.WidthMm > 0,
                !line.WidthMm.HasValue || line.WidthMm <= 0, lineWarnings.Where(w => w.Contains("ширину")).ToList()));
            
            fieldResults.Add(new FieldValidationResult(
                "Quantity", line.RowNumber, line.Quantity.HasValue && line.Quantity > 0,
                !line.Quantity.HasValue || line.Quantity <= 0, new List<string>()));
            
            errors.AddRange(lineErrors);
            warnings.AddRange(lineWarnings);
        }
        
        // Overall confidence check
        if (input.OverallConfidence < 0.70)
        {
            warnings.Add($"Низкая общая уверенность распознавания: {input.OverallConfidence:F2}");
        }
        
        if (input.HasOcrSubstitutions)
        {
            warnings.Add("Обнаружены возможные ошибки распознавания символов");
        }
        
        bool requiresReview = errors.Count == 0 && (warnings.Count > 0 || input.HasLowConfidenceFields);
        bool isValid = errors.Count == 0;
        
        return new ValidationResult(
            isValid,
            requiresReview,
            errors,
            warnings,
            fieldResults
        );
    }
}
