using PhotoOrder.Application.Interfaces;

namespace PhotoOrder.Application.Services;

public class FakeHandwritingRecognitionProvider : IHandwritingRecognitionProvider
{
    public string ProviderName => "Fake OCR Provider (Mock)";
    public RecognitionProvider ProviderType => RecognitionProvider.Fake;
    
    private static readonly List<TextLineResult> SampleResults = new()
    {
        new TextLineResult("ЗК-1042", 0.95, 100, 50, 200, 40),
        new TextLineResult("Боковина 720x560 =2", 0.88, 100, 120, 350, 35),
        new TextLineResult("Полка 540x300 x4", 0.85, 100, 180, 320, 35),
        new TextLineResult("Дно 700x550 =2", 0.91, 100, 240, 300, 35),
        new TextLineResult("Задняя стенка 720x750 =1", 0.82, 100, 300, 450, 35)
    };
    
    public Task<RecognitionResult> RecognizeAsync(byte[] imageData, RecognitionOptions options, CancellationToken ct = default)
    {
        // Simulate processing time
        var delay = Random.Shared.Next(500, 1500);
        Thread.Sleep(delay);
        
        // Add some randomness to confidence
        var lines = SampleResults.Select(l => new TextLineResult(
            l.Text,
            Math.Max(0.5, Math.Min(1.0, l.Confidence + (Random.Shared.NextDouble() - 0.5) * 0.2)),
            l.BboxX + Random.Shared.Next(-5, 5),
            l.BboxY + Random.Shared.Next(-5, 5),
            l.BboxWidth,
            l.BboxHeight,
            null
        )).ToList();
        
        var result = new RecognitionResult(
            lines,
            TimeSpan.FromMilliseconds(delay),
            "fake-v1.0.0",
            "mock-config-hash",
            RecognitionProvider.Fake
        );
        
        return Task.FromResult(result);
    }
    
    public Task<ModelInfo> GetModelInfoAsync(CancellationToken ct = default)
    {
        var info = new ModelInfo(
            "fake-v1.0.0",
            "N/A - Mock Provider",
            null,
            DateTime.UtcNow,
            false
        );
        
        return Task.FromResult(info);
    }
}
