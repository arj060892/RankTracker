namespace RankTracker.Persistence.Models;

public class TrendEntryEntity
{
    public long HistoryId { get; set; }
    public DateTime CheckedAt { get; set; }
    public string Positions { get; set; } = null!;
}
