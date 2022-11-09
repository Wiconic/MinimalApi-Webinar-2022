using Core.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.WeightInKG).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Description).MaximumLength(50);
            RuleFor(x => x.Name).MaximumLength(25);
        }

        protected override bool PreValidate(ValidationContext<Product> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure("", "Please ensure product is not null."));
                return false;
            }
            return true;
        }
    }
}
