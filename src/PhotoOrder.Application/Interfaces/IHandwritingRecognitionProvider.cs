using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Application.Interfaces;

public interface IHandwritingRecognitionProvider
{
    string ProviderName { get; }
    RecognitionProvider ProviderType { get; }
    
    Task<RecognitionResult> RecognizeAsync(byte[] imageData, RecognitionOptions options, CancellationToken ct = default);
    Task<ModelInfo> GetModelInfoAsync(CancellationToken ct = default);
}

public record RecognitionOptions(
    bool UseGpu = false,
    int CpuThreads = 4,
    double DetectionThreshold = 0.30,
    double BoxThreshold = 0.50,
    double UnclipRatio = 1.5,
    int RecognitionHeight = 48,
    int RecognitionMaxWidth = 640,
    int RecognitionBatchSize = 4,
    bool EnableOrientationClassifier = true,
    bool EnableTextUnwarping = true,
    double ReviewConfidenceThreshold = 0.80
);

public record RecognitionResult(
    List<TextLineResult> Lines,
    TimeSpan ProcessingTime,
    string ModelVersion,
    string ConfigHash,
    RecognitionProvider Provider
);

public record TextLineResult(
    string Text,
    double Confidence,
    int BboxX,
    int BboxY,
    int BboxWidth,
    int BboxHeight,
    byte[]? CropImage = null
);

public record ModelInfo(
    string Version,
    string CheckpointPath,
    string? WeightsSha256,
    DateTime LoadedAt,
    bool IsGpuAvailable
);
