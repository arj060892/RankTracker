using FluentValidation;
using RankTracker.Application.Models.Requests;

namespace RankTracker.Api.Validators
{
    public class TrendRequestDtoValidator : AbstractValidator<SearchRequestDto>
    {
        public TrendRequestDtoValidator()
        {
            Include(new SearchRequestDtoValidator());
        }
    }
}
