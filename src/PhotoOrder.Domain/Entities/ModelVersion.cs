using PhotoOrder.Domain.Common;
using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Domain.Entities;

public class ModelVersion : BaseEntity
{
    public string VersionName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ModelState State { get; private set; } = ModelState.Candidate;
    
    public string ProviderType { get; private set; } = string.Empty; // PaddleOcr, QwenVL
    public string CheckpointPath { get; private set; } = string.Empty;
    public string ConfigPath { get; private set; } = string.Empty;
    public string? WeightsSha256 { get; private set; }
    
    public double? ValidationCER { get; private set; }
    public double? TestCER { get; private set; }
    public double? NumericTokenAccuracy { get; private set; }
    
    public DateTime? ActivatedAt { get; private set; }
    public DateTime? ArchivedAt { get; private set; }
    
    public Guid? TrainingJobId { get; private set; }
    public TrainingJob? TrainingJob { get; private set; }
    
    public void SetInfo(string versionName, string description, string providerType)
    {
        VersionName = versionName;
        Description = description;
        ProviderType = providerType;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetCheckpoint(string checkpointPath, string configPath, string? sha256)
    {
        CheckpointPath = checkpointPath;
        ConfigPath = configPath;
        WeightsSha256 = sha256;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetMetrics(double? valCER, double? testCER, double? numericAcc)
    {
        ValidationCER = valCER;
        TestCER = testCER;
        NumericTokenAccuracy = numericAcc;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetState(ModelState state)
    {
        State = state;
        if (state == ModelState.Active)
            ActivatedAt = DateTime.UtcNow;
        else if (state == ModelState.Archived)
            ArchivedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
