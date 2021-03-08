using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators.SecondaryValidators
{
    public class AnimeInfoNameCreationValidator : AbstractValidator<AnimeInfoNameCreationRequestModel>
    {
        public AnimeInfoNameCreationValidator()
        {
            RuleFor(x => x.Title).NotEmpty()
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(255)
                .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
        }
    }
}
