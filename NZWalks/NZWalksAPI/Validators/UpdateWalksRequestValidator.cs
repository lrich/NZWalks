using FluentValidation;

namespace NZWalksAPI.Validators
{
    public class UpdateWalksRequestValidator:AbstractValidator<Models.DTO.UpdateWalkRequest>
    {
        public UpdateWalksRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
