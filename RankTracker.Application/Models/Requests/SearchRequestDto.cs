namespace RankTracker.Application.Models.Requests;

public record SearchRequestDto(
    string Engine = "Google",
    string Query = "DefaultQuery",
    string Url = "https://default.com"
);