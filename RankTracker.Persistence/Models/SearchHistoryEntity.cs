using System;

namespace RankTracker.Persistence.Models;

public class SearchHistoryEntity
{
    public long HistoryId { get; set; }
    public int EngineId { get; set; }
    public string? QueryText { get; set; }
    public string? TargetUrl { get; set; }
    public DateTime CheckedAt { get; set; }
}
