namespace PhotoOrder.Application.Interfaces;

public interface ITrainingJobService
{
    Task<TrainingJobInfo> StartTrainingAsync(
        Guid datasetVersionId,
        TrainingConfig config,
        CancellationToken ct = default);
    
    Task<TrainingJobInfo?> GetJobAsync(Guid jobId, CancellationToken ct = default);
    Task<IEnumerable<TrainingJobInfo>> GetAllJobsAsync(CancellationToken ct = default);
    Task<bool> CancelJobAsync(Guid jobId, CancellationToken ct = default);
    
    Task<TrainingJobInfo> CompleteJobAsync(
        Guid jobId,
        string status,
        string? errorMessage,
        double? trainCER,
        double? valCER,
        double? testCER,
        double? numericAcc,
        string? checkpointPath,
        string? logPath,
        string? metricsPath,
        CancellationToken ct = default);
}

public record TrainingConfig(
    int Epochs = 20,
    int BatchSize = 16,
    double LearningRate = 0.0001,
    double WeightDecay = 0.0001,
    double WarmupRatio = 0.05,
    double GradientClip = 5.0,
    int? RandomSeed = null,
    bool UseMixedPrecision = true,
    bool UseGpu = false
);

public record TrainingJobInfo(
    Guid Id,
    Guid DatasetVersionId,
    string Status,
    string? ErrorMessage,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    int Epochs,
    int BatchSize,
    double LearningRate,
    int? RandomSeed,
    double? ValidationCER,
    double? TestCER,
    double? NumericTokenAccuracy,
    string? CheckpointPath,
    string? LogPath,
    Guid? ResultingModelId
);
