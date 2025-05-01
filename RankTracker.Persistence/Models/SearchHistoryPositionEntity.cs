namespace RankTracker.Persistence.Models;

public class SearchHistoryPositionEntity
{
    public long PositionId { get; set; }
    public long HistoryId { get; set; }
    public byte Position { get; set; }
}
