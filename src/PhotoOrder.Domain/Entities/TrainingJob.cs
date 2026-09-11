using PhotoOrder.Domain.Common;
using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Domain.Entities;

public class TrainingJob : BaseEntity
{
    public Guid DatasetVersionId { get; private set; }
    public DatasetVersion DatasetVersion { get; private set; } = null!;
    
    public TrainingJobStatus Status { get; private set; } = TrainingJobStatus.Queued;
    public string? ErrorMessage { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    
    public int Epochs { get; private set; }
    public int BatchSize { get; private set; }
    public double LearningRate { get; private set; }
    public int? RandomSeed { get; private set; }
    
    public double? TrainCER { get; private set; }
    public double? ValidationCER { get; private set; }
    public double? TestCER { get; private set; }
    public double? NumericTokenAccuracy { get; private set; }
    
    public string? CheckpointPath { get; private set; }
    public string? LogPath { get; private set; }
    public string? MetricsPath { get; private set; }
    
    public Guid? ResultingModelId { get; private set; }
    public ModelVersion? ResultingModel { get; private set; }
    
    public void SetConfig(int epochs, int batchSize, double learningRate, int? seed)
    {
        Epochs = epochs;
        BatchSize = batchSize;
        LearningRate = learningRate;
        RandomSeed = seed;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetStatus(TrainingJobStatus status, string? errorMessage = null)
    {
        Status = status;
        ErrorMessage = errorMessage;
        if (status == TrainingJobStatus.Training && !StartedAt.HasValue)
            StartedAt = DateTime.UtcNow;
        if (status is TrainingJobStatus.CandidateReady or TrainingJobStatus.Rejected or TrainingJobStatus.Failed)
            CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetMetrics(double? trainCER, double? valCER, double? testCER, double? numericAcc)
    {
        TrainCER = trainCER;
        ValidationCER = valCER;
        TestCER = testCER;
        NumericTokenAccuracy = numericAcc;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetResult(string? checkpointPath, string? logPath, string? metricsPath)
    {
        CheckpointPath = checkpointPath;
        LogPath = logPath;
        MetricsPath = metricsPath;
        UpdatedAt = DateTime.UtcNow;
    }
}
