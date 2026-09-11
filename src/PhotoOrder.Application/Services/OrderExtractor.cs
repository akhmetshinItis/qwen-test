using System.Globalization;
using System.Text.RegularExpressions;
using PhotoOrder.Application.Interfaces;

namespace PhotoOrder.Application.Services;

public partial class OrderExtractor : IOrderExtractor
{
    public Task<OrderExtractionResult> ExtractFromLinesAsync(
        List<TextLineResult> lines,
        ExtractionContext context,
        CancellationToken ct = default)
    {
        var result = new OrderExtractionResult(
            context.KnownOrderNumber,
            context.KnownMaterial,
            context.KnownThicknessMm,
            new List<OrderLineExtraction>(),
            new List<string>()
        );
        
        int rowNumber = 0;
        foreach (var line in lines)
        {
            rowNumber++;
            var lineExtraction = ParseOrderLine(line, rowNumber);
            result.Lines.Add(lineExtraction);
            
            // Try to extract order number from first few lines
            if (result.OrderNumber == null && rowNumber <= 3)
            {
                var orderNum = ExtractOrderNumber(line.Text);
                if (orderNum != null)
                    result.OrderNumber = orderNum;
            }
            
            result.Warnings.AddRange(lineExtraction.Warnings);
        }
        
        return Task.FromResult(result);
    }
    
    private OrderLineExtraction ParseOrderLine(TextLineResult line, int rowNumber)
    {
        var warnings = new List<string>();
        var fields = new List<ExtractedFieldData>();
        
        // Parse dimensions: various formats like 720x560, 720×560, 720*560, 720х560, "720 на 560"
        var dimensions = ParseDimensions(line.Text);
        
        decimal? length = dimensions.Length;
        decimal? width = dimensions.Width;
        
        if (length == null || width == null)
        {
            warnings.Add("Не удалось распознать размеры");
        }
        
        // Parse quantity
        var quantity = ParseQuantity(line.Text);
        if (quantity == null)
        {
            warnings.Add("Не удалось распознать количество");
        }
        
        // Check for OCR substitutions
        if (HasOcrSubstitution(line.Text))
        {
            warnings.Add("Возможна ошибка распознавания (O↔0, З↔3, Б↔6)");
        }
        
        // Low confidence warning
        if (line.Confidence < 0.80)
        {
            warnings.Add($"Низкая уверенность распознавания: {line.Confidence:F2}");
        }
        
        // Build fields
        fields.Add(new ExtractedFieldData(
            "ItemName",
            ExtractItemName(line.Text),
            ExtractItemName(line.Text),
            line.Confidence,
            warnings.Count > 0,
            warnings.Count > 0 ? string.Join("; ", warnings) : null,
            line.BboxX, line.BboxY, line.BboxWidth, line.BboxHeight
        ));
        
        fields.Add(new ExtractedFieldData(
            "LengthMm",
            dimensions.RawLength,
            length?.ToString(),
            line.Confidence,
            length == null,
            length == null ? "Не распознана длина" : null,
            line.BboxX, line.BboxY, line.BboxWidth, line.BboxHeight
        ));
        
        fields.Add(new ExtractedFieldData(
            "WidthMm",
            dimensions.RawWidth,
            width?.ToString(),
            line.Confidence,
            width == null,
            width == null ? "Не распознана ширина" : null,
            line.BboxX, line.BboxY, line.BboxWidth, line.BboxHeight
        ));
        
        fields.Add(new ExtractedFieldData(
            "Quantity",
            quantity?.RawValue,
            quantity?.Value?.ToString(),
            line.Confidence,
            quantity == null,
            quantity == null ? "Не распознано количество" : null,
            line.BboxX, line.BboxY, line.BboxWidth, line.BboxHeight
        ));
        
        return new OrderLineExtraction(
            rowNumber,
            ExtractItemName(line.Text),
            length,
            width,
            quantity?.Value,
            null, // Material extracted from header
            null, // Thickness from header
            null, // Notes
            fields,
            warnings
        );
    }
    
    private (decimal? Length, decimal? Width, string? RawLength, string? RawWidth) ParseDimensions(string text)
    {
        // Match patterns: 720x560, 720×560, 720*560, 720х560, Д=720 Ш=560, 720 на 560
        var match = DimensionRegex().Match(text);
        if (match.Success)
        {
            var raw1 = match.Groups[1].Value;
            var raw2 = match.Groups[2].Value;
            
            var val1 = ParseNumericValue(raw1);
            var val2 = ParseNumericValue(raw2);
            
            return (val1, val2, raw1, raw2);
        }
        
        // Try Д=XXX Ш=YYY pattern
        var namedMatch = NamedDimensionRegex().Match(text);
        if (namedMatch.Success)
        {
            var lengthRaw = namedMatch.Groups["length"].Value;
            var widthRaw = namedMatch.Groups["width"].Value;
            
            return (ParseNumericValue(lengthRaw), ParseNumericValue(widthRaw), lengthRaw, widthRaw);
        }
        
        return (null, null, null, null);
    }
    
    private decimal? ParseNumericValue(string raw)
    {
        if (string.IsNullOrEmpty(raw))
            return null;
        
        // Replace common OCR errors
        raw = raw.Replace('O', '0').Replace('o', '0')
                 .Replace('З', '3').Replace('з', '3')
                 .Replace('Б', '6').Replace('б', '6')
                 .Replace(',', '.');
        
        if (decimal.TryParse(raw, NumberStyles.Number | NumberStyles.AllowDecimalPoint, 
            CultureInfo.InvariantCulture, out var result))
        {
            // Convert cm to mm if value seems too small
            if (result < 10 && result > 0)
                result *= 10;
            
            return result;
        }
        
        return null;
    }
    
    private (int Value, string RawValue)? ParseQuantity(string text)
    {
        // Look for quantity patterns: "=2", "x2", "*2", "2шт", "2 шт"
        var match = QuantityRegex().Match(text);
        if (match.Success)
        {
            var raw = match.Groups[1].Value;
            if (int.TryParse(raw, out var qty) && qty > 0)
                return (qty, raw);
        }
        
        return null;
    }
    
    private string? ExtractItemName(string text)
    {
        // Remove dimensions and quantity to get item name
        var cleaned = DimensionRegex().Replace(text, "");
        cleaned = QuantityRegex().Replace(cleaned, "");
        cleaned = NamedDimensionRegex().Replace(cleaned, "");
        
        cleaned = cleaned.Trim(' ', '-', '=', ':', ';', ',');
        
        return string.IsNullOrWhiteSpace(cleaned) ? null : cleaned;
    }
    
    private string? ExtractOrderNumber(string text)
    {
        var match = OrderNumberRegex().Match(text);
        return match.Success ? match.Value : null;
    }
    
    private bool HasOcrSubstitution(string text)
    {
        // Check for common OCR confusions in numeric contexts
        return OcrSubstitutionRegex().IsMatch(text);
    }
    
    [GeneratedRegex(@"(\d+[,.]?\d*)\s*[x×*х]\s*(\d+[,.]?\d*)", RegexOptions.IgnoreCase)]
    private static partial Regex DimensionRegex();
    
    [GeneratedRegex(@"[Дд]=?\s*(?<length>\d+[,.]?\d*).*?[Шш]=?\s*(?<width>\d+[,.]?\d*)", RegexOptions.IgnoreCase)]
    private static partial Regex NamedDimensionRegex();
    
    [GeneratedRegex(@"[=x×*]\s*(\d+)\s*(?:шт|зак)?", RegexOptions.IgnoreCase)]
    private static partial Regex QuantityRegex();
    
    [GeneratedRegex(@"[А-Яа-я]{2,}-?\d+", RegexOptions.IgnoreCase)]
    private static partial Regex OrderNumberRegex();
    
    [GeneratedRegex(@"[Оо]\d|\d[Оо]|[Зз]\d|\d[Зз]|[Бб]\d|\d[Бб]", RegexOptions.IgnoreCase)]
    private static partial Regex OcrSubstitutionRegex();
}
