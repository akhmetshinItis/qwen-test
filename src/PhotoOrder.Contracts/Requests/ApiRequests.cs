namespace PhotoOrder.Contracts.Requests;

public record UploadDocumentRequest(
    string FileName,
    string ContentType,
    byte[] Content
);

public record ReprocessDocumentRequest(
    Guid DocumentId,
    bool UseGpu = false
);

public record UpdateOrderLineRequest(
    Guid OrderId,
    int RowNumber,
    string? ItemName,
    decimal? LengthMm,
    decimal? WidthMm,
    int? Quantity,
    string? Material,
    decimal? ThicknessMm,
    string? Notes
);

public record ConfirmOrderRequest(
    Guid OrderId
);

public record ApproveSampleForTrainingRequest(
    Guid SampleId,
    bool Approved
);

public record ImportDatasetRequest(
    string ZipPath,
    string ManifestPath
);

public record StartTrainingRequest(
    Guid DatasetVersionId,
    int Epochs = 20,
    int BatchSize = 16,
    double LearningRate = 0.0001,
    int? RandomSeed = null,
    bool UseGpu = false
);

public record PromoteModelRequest(
    Guid ModelId
);

public record RollbackModelRequest(
    string ProviderType
);
