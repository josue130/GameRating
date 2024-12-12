using FluentValidation;
using GameRaitingAPI.DTOs;

namespace GameRaitingAPI.Validations
{
    public class CreateCommentDTOValidator : AbstractValidator<CreateCommentDTO>
    {
        public CreateCommentDTOValidator()
        {
            RuleFor(m => m.Message).NotEmpty().WithMessage(ErrorMessages.FieldIsRequired);
        }
    }
}
