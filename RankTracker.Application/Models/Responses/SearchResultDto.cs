namespace RankTracker.Application.Models.Responses;

public record SearchResultDto(
    IReadOnlyList<int> Positions
);
