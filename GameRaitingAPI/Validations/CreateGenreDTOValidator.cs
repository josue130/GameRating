using FluentValidation;
using GameRatingAPI.DTOs;
using GameRatingAPI.Repository.IRepository;

namespace GameRatingAPI.Validations
{
    public class CreateGenreDTOValidator : AbstractValidator<CreateGenreDTO>
    {
        public CreateGenreDTOValidator(IGenreRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            var routeValue = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (routeValue is string stringValue)
            {
                int.TryParse(stringValue, out id);
            }

            RuleFor(x => x.Name).NotEmpty().
                MaximumLength(50).WithMessage(ErrorMessages.MaximumLength)
                .Must(FisrtCaptialLetter).WithMessage(ErrorMessages.FirstCapitalLetter)
                .MustAsync(async (name, _) =>
                {
                    var exist = await repository.Exist(id, name);
                    return !exist;
                }).WithMessage(ErrorMessages.GenreAlreadyExist);
        }

        private bool FisrtCaptialLetter(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            var firstLetter = value[0].ToString();
            return firstLetter == firstLetter.ToUpper();

        }
    }
}
