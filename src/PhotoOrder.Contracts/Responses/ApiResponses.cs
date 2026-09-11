using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Contracts.Responses;

public record DocumentUploadResponse(
    Guid DocumentId,
    string FileName,
    DocumentStatus Status
);

public record DocumentStatusResponse(
    Guid DocumentId,
    string FileName,
    DocumentStatus Status,
    string? ErrorMessage,
    int? ImageWidth,
    int? ImageHeight,
    Guid? OrderId
);

public record ExtractedFieldDto(
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

public record OrderLineDto(
    Guid Id,
    int RowNumber,
    string? ItemName,
    decimal? LengthMm,
    decimal? WidthMm,
    int? Quantity,
    string? Material,
    decimal? ThicknessMm,
    string? Notes,
    List<ExtractedFieldDto> ExtractedFields
);

public record OrderDto(
    Guid Id,
    string OrderNumber,
    string? Material,
    decimal? ThicknessMm,
    DocumentStatus Status,
    int Revision,
    Guid SourceDocumentId,
    List<OrderLineDto> Lines
);

public record RecognitionRunDto(
    Guid Id,
    RecognitionProvider Provider,
    string ModelVersion,
    string ConfigHash,
    TimeSpan ProcessingTimeMs,
    int LinesDetected,
    double AverageConfidence,
    DateTime CreatedAt
);

public record CsvDownloadResponse(
    string FileName,
    string ContentType,
    byte[] Content
);

public record DatasetVersionDto(
    Guid Id,
    string VersionName,
    string Description,
    int TrainCount,
    int ValidationCount,
    int TestCount,
    DateTime CreatedAt
);

public record DatasetSampleDto(
    Guid Id,
    string ImagePath,
    string Label,
    string Split,
    bool ApprovedForTraining,
    Guid? SourceDocumentId
);

public record TrainingJobDto(
    Guid Id,
    Guid DatasetVersionId,
    TrainingJobStatus Status,
    string? ErrorMessage,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    int Epochs,
    int BatchSize,
    double LearningRate,
    double? ValidationCER,
    double? TestCER,
    double? NumericTokenAccuracy,
    Guid? ResultingModelId
);

public record ModelVersionDto(
    Guid Id,
    string VersionName,
    string Description,
    ModelState State,
    string ProviderType,
    double? ValidationCER,
    double? TestCER,
    double? NumericTokenAccuracy,
    DateTime? ActivatedAt,
    DateTime CreatedAt
);

public record ModelComparisonResult(
    Guid CandidateId,
    Guid ActiveId,
    double CandidateCER,
    double ActiveCER,
    double CandidateNumericAcc,
    double ActiveNumericAcc,
    bool IsBetter,
    string Recommendation
);

public record HealthCheckResponse(
    bool IsHealthy,
    string WebStatus,
    string DatabaseStatus,
    string InferenceServiceStatus,
    string? ActiveModelVersion
);
