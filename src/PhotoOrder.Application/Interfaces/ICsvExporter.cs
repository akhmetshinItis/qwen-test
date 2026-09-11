namespace PhotoOrder.Application.Interfaces;

public interface ICsvExporter
{
    CsvExportResult ExportToCsv(CsvExportInput input);
}

public record CsvExportInput(
    string OrderNumber,
    int Revision,
    List<CsvLineInput> Lines,
    char Delimiter = ';',
    char DecimalSeparator = ','
);

public record CsvLineInput(
    int RowNumber,
    string? ItemName,
    decimal? LengthMm,
    decimal? WidthMm,
    int? Quantity,
    string? Material,
    decimal? ThicknessMm,
    double Confidence,
    bool NeedsReview,
    string? Notes
);

public record CsvExportResult(
    string FileName,
    string ContentType,
    byte[] Content,
    int LineCount
);
