using PhotoOrder.Domain.Common;
using PhotoOrder.Domain.Enums;

namespace PhotoOrder.Domain.Entities;

public class DatasetVersion : BaseEntity
{
    public string VersionName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int TrainCount { get; private set; }
    public int ValidationCount { get; private set; }
    public int TestCount { get; private set; }
    public string? ManifestPath { get; private set; }
    
    public List<DatasetSample> Samples { get; private set; } = new();
    public List<TrainingJob> TrainingJobs { get; private set; } = new();
    
    public void SetInfo(string versionName, string description)
    {
        VersionName = versionName;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetCounts(int train, int validation, int test)
    {
        TrainCount = train;
        ValidationCount = validation;
        TestCount = test;
        UpdatedAt = DateTime.UtcNow;
    }
}
