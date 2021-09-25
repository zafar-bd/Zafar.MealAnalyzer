using FluentValidation;
using System;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.WebApi.Validators
{
    public class DocumentConfigValidator : AbstractValidator<MealAnalyzerQueryDto>
    {
        public DocumentConfigValidator()
        {
            RuleFor(d => d.StartDate).NotEqual(DateTime.MinValue).NotEqual(DateTime.MaxValue);
            RuleFor(d => d.EndDate).NotEqual(DateTime.MinValue).NotEqual(DateTime.MaxValue);
            RuleFor(d => d.EndDate).GreaterThanOrEqualTo(d => d.StartDate)
                .WithMessage("End date must be greater than or equal start date");
            RuleFor(d => d.UserType).IsInEnum().WithMessage("Invalid User Type");
        }
    }
}
