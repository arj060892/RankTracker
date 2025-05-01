using System;
using FluentValidation;
using RankTracker.Application.Models.Requests;

namespace RankTracker.Api.Validators
{
    public class SearchRequestDtoValidator : AbstractValidator<SearchRequestDto>
    {
        public SearchRequestDtoValidator()
        {
            RuleFor(x => x.Engine)
                .NotEmpty().WithMessage("Engine is required.");

            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Query is required.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Url is required.")
                .Must(u => Uri.IsWellFormedUriString(u, UriKind.Absolute) || u.Contains('.'))
                .WithMessage("Url must be a valid URL or domain.");
        }
    }
}
