namespace RankTracker.Application.Models.Responses;

public record TrendEntryDto(
    DateTime CheckedAt,
    IReadOnlyList<int> Positions
);