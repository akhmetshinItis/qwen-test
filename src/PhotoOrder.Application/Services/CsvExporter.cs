using System.Globalization;
using System.Text;
using PhotoOrder.Application.Interfaces;

namespace PhotoOrder.Application.Services;

public class CsvExporter : ICsvExporter
{
    public CsvExportResult ExportToCsv(CsvExportInput input)
    {
        var sb = new StringBuilder();
        
        // Header with BOM
        sb.AppendLine("\uFEFFOrderNumber" + input.Delimiter + "RowNumber" + input.Delimiter + 
                      "ItemName" + input.Delimiter + "LengthMm" + input.Delimiter + "WidthMm" + input.Delimiter +
                      "Quantity" + input.Delimiter + "Material" + input.Delimiter + "ThicknessMm" + input.Delimiter +
                      "Confidence" + input.Delimiter + "NeedsReview" + input.Delimiter + "Notes");
        
        var culture = new CultureInfo("ru-RU");
        culture.NumberFormat.NumberDecimalSeparator = input.DecimalSeparator.ToString();
        
        foreach (var line in input.Lines)
        {
            var values = new List<string>
            {
                EscapeCsvField(input.OrderNumber, input.Delimiter),
                line.RowNumber.ToString(),
                EscapeCsvField(line.ItemName, input.Delimiter),
                FormatDecimal(line.LengthMm, input.DecimalSeparator),
                FormatDecimal(line.WidthMm, input.DecimalSeparator),
                line.Quantity?.ToString() ?? "",
                EscapeCsvField(line.Material, input.Delimiter),
                FormatDecimal(line.ThicknessMm, input.DecimalSeparator),
                line.Confidence.ToString("F2", culture),
                line.NeedsReview ? "true" : "false",
                EscapeCsvField(line.Notes, input.Delimiter)
            };
            
            sb.AppendLine(string.Join(input.Delimiter, values));
        }
        
        var content = Encoding.UTF8.GetBytes(sb.ToString());
        var fileName = $"order-{input.OrderNumber}-{input.Revision}.csv";
        
        return new CsvExportResult(
            fileName,
            "text/csv; charset=utf-8",
            content,
            input.Lines.Count);
    }
    
    private static string EscapeCsvField(string? value, char delimiter)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        
        // CSV injection protection
        if (value.StartsWith('=') || value.StartsWith('+') || 
            value.StartsWith('-') || value.StartsWith('@'))
        {
            value = "'" + value;
        }
        
        // RFC 4180 escaping
        if (value.Contains('"') || value.Contains(delimiter) || 
            value.Contains('\n') || value.Contains('\r'))
        {
            value = "\"" + value.Replace("\"", "\"\"") + "\"";
        }
        
        return value;
    }
    
    private static string FormatDecimal(decimal? value, char decimalSeparator)
    {
        if (!value.HasValue)
            return string.Empty;
        
        return value.Value.ToString(CultureInfo.InvariantCulture)
            .Replace('.', decimalSeparator);
    }
}
