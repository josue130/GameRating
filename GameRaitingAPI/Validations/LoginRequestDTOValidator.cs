﻿using FluentValidation;
using GameRaitingAPI.DTOs;

namespace GameRaitingAPI.Validations
{
    public class LoginRequestDTOValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginRequestDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(ErrorMessages.FieldIsRequired)
                .MaximumLength(256).WithMessage(ErrorMessages.MaximumLength)
                .EmailAddress().WithMessage(ErrorMessages.IncorrectEmailFormat);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ErrorMessages.FieldIsRequired);
        }
    }
}