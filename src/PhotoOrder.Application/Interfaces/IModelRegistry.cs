namespace PhotoOrder.Application.Interfaces;

public interface IModelRegistry
{
    Task<ModelVersionInfo> RegisterCandidateAsync(
        string providerType,
        string checkpointPath,
        string configPath,
        string? weightsSha256,
        Guid? trainingJobId,
        CancellationToken ct = default);
    
    Task<ModelVersionInfo?> GetActiveModelAsync(string providerType, CancellationToken ct = default);
    Task<IEnumerable<ModelVersionInfo>> GetAllModelsAsync(string? providerType = null, CancellationToken ct = default);
    
    Task<bool> PromoteToActiveAsync(Guid modelId, CancellationToken ct = default);
    Task<bool> ArchiveModelAsync(Guid modelId, CancellationToken ct = default);
    Task<bool> RollbackToPreviousAsync(string providerType, CancellationToken ct = default);
    
    Task<ModelComparisonResult?> CompareModelsAsync(Guid candidateId, Guid activeId, CancellationToken ct = default);
}

public record ModelVersionInfo(
    Guid Id,
    string VersionName,
    string Description,
    string State,
    string ProviderType,
    string CheckpointPath,
    string ConfigPath,
    string? WeightsSha256,
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
