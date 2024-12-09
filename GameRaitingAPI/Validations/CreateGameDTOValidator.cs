using FluentValidation;
using GameRaitingAPI.DTOs;

namespace GameRaitingAPI.Validations
{
    public class CreateGameDTOValidator : AbstractValidator<CreateGameDTO>
    {
        public CreateGameDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(ErrorMessages.FieldIsRequired)
                .MaximumLength(150).WithMessage(ErrorMessages.MaximumLength);

            var minimumDate = new DateTime(1950, 1, 1);

            RuleFor(x => x.ReleaseDate).GreaterThanOrEqualTo(minimumDate)
                .WithMessage(GreaterThanOrEqualToOurDate(minimumDate));


        }
        public static string GreaterThanOrEqualToOurDate(DateTime minimumDate)
        {
            return "Date must be later" + minimumDate.ToString("yyyy-MM-dd");
        }
    }
}
