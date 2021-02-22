﻿using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators
{
    public class GenreEditingValidator : AbstractValidator<GenreEditingRequestModel>
    {
        public GenreEditingValidator()
        {
            RuleFor(x => x).NotNull()
                .WithErrorCode(ErrorCodes.EmptyObject.GetIntValueAsString());
            RuleFor(x => x.Id).GreaterThan(0)
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
            RuleFor(x => x.GenreName).NotEmpty()
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(100)
                .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            RuleFor(x => x.Description).NotEmpty()
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(10000)
                .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
        }
    }
}