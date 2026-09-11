namespace PhotoOrder.Domain.Enums;

public enum DocumentStatus
{
    Uploaded = 0,
    Queued = 1,
    Preprocessing = 2,
    Recognizing = 3,
    NeedsReview = 4,
    Confirmed = 5,
    Failed = 6
}

public enum TrainingJobStatus
{
    Queued = 0,
    PreparingDataset = 1,
    Training = 2,
    Evaluating = 3,
    CandidateReady = 4,
    Rejected = 5,
    Failed = 6
}

public enum ModelState
{
    Candidate = 0,
    Active = 1,
    Archived = 2
}

public enum RecognitionProvider
{
    PaddleOcr = 0,
    QwenVL = 1,
    Fake = 99
}
