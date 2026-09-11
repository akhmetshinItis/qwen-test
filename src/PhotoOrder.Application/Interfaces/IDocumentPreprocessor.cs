namespace PhotoOrder.Application.Interfaces;

public interface IDocumentPreprocessor
{
    Task<ImageMetadata> GetImageMetadataAsync(Stream imageStream, CancellationToken ct = default);
    Task<PreprocessedImage> PreprocessAsync(Stream imageStream, PreprocessingOptions options, CancellationToken ct = default);
}

public record ImageMetadata(
    int Width,
    int Height,
    string Format,
    double? EXIFOrientation
);

public record PreprocessedImage(
    byte[] ImageData,
    int Width,
    int Height,
    bool WasRotated,
    bool WasDeskewed,
    double? QualityScore
);

public record PreprocessingOptions(
    bool CorrectOrientation = true,
    bool Deskew = true,
    bool AssessQuality = true,
    int TargetLongSide = 1920,
    double MinQualityScore = 0.3
);
