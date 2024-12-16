using FluentValidation;
using GameRatingAPI.DTOs;

namespace GameRatingAPI.Validations
{
    public class RegisterRequestDTOValidator : AbstractValidator<RegisterRequestDTO>
    {
        public RegisterRequestDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(ErrorMessages.FieldIsRequired)
                .MaximumLength(256).WithMessage(ErrorMessages.MaximumLength)
                .EmailAddress().WithMessage(ErrorMessages.IncorrectEmailFormat);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ErrorMessages.FieldIsRequired)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character."); ;
        }
    }
}
