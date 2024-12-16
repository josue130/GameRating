using FluentValidation;
using GameRatingAPI.DTOs;

namespace GameRatingAPI.Validations
{
    public class CreateCommentDTOValidator : AbstractValidator<CreateCommentDTO>
    {
        public CreateCommentDTOValidator()
        {
            RuleFor(m => m.Message).NotEmpty().WithMessage(ErrorMessages.FieldIsRequired);
        }
    }
}
