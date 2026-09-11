namespace PhotoOrder.Application.Interfaces;

public interface ITrainingDatasetService
{
    Task<DatasetVersionInfo> CreateDatasetVersionAsync(
        string name,
        string description,
        CancellationToken ct = default);
    
    Task<DatasetSampleInfo> AddSampleAsync(
        Guid datasetVersionId,
        byte[] cropImage,
        string label,
        string split,
        Guid? sourceDocumentId,
        CancellationToken ct = default);
    
    Task<int> ApproveSamplesForTrainingAsync(
        Guid datasetVersionId,
        IEnumerable<Guid> sampleIds,
        bool approved,
        CancellationToken ct = default);
    
    Task<DatasetVersionInfo> FinalizeDatasetAsync(
        Guid datasetVersionId,
        CancellationToken ct = default);
    
    Task<DatasetExportResult> ExportDatasetAsync(
        Guid datasetVersionId,
        string exportPath,
        CancellationToken ct = default);
    
    Task<DatasetVersionInfo?> GetLatestDatasetVersionAsync(CancellationToken ct = default);
    Task<IEnumerable<DatasetSampleInfo>> GetSamplesAsync(Guid datasetVersionId, string? split = null, bool? approvedOnly = null, CancellationToken ct = default);
}

public record DatasetVersionInfo(
    Guid Id,
    string VersionName,
    string Description,
    int TrainCount,
    int ValidationCount,
    int TestCount,
    DateTime CreatedAt
);

public record DatasetSampleInfo(
    Guid Id,
    Guid DatasetVersionId,
    string ImagePath,
    string Label,
    string Split,
    bool ApprovedForTraining,
    Guid? SourceDocumentId
);

public record DatasetExportResult(
    string ManifestPath,
    string ImagesDirectory,
    int TotalSamples
);
