using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators
{
    public class GenreCreationValidator : AbstractValidator<GenreCreationRequestModel>
    {
        public GenreCreationValidator()
        {
            RuleFor(x => x.GenreName).NotEmpty().WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(100).WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            RuleFor(x => x.Description).NotEmpty().WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(10000).WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
        }
    }
}
