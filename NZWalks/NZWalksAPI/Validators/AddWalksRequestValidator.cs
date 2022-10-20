using FluentValidation;

namespace NZWalksAPI.Validators
{
    public class AddWalksRequestValidator:AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalksRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
