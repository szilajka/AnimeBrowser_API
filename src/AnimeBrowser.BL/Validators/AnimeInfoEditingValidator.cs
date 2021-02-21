using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators
{
    public class AnimeInfoEditingValidator : AbstractValidator<AnimeInfoEditingRequestModel>
    {
        public AnimeInfoEditingValidator()
        {
            RuleFor(x => x).NotNull().WithErrorCode(ErrorCodes.EmptyObject.GetIntValueAsString());
            RuleFor(x => x.Id).GreaterThan(0).WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
            RuleFor(x => x.Title).NotEmpty().WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(255).WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            RuleFor(x => x.Description).MaximumLength(30000).WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
        }
    }
}
