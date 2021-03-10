using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators.SecondaryValidators
{
    public class SeasonNameCreationValidator : AbstractValidator<SeasonNameCreationRequestModel>
    {
        public SeasonNameCreationValidator()
        {
            Transform(x => x.Title, x => string.IsNullOrWhiteSpace(x) ? x : x.Trim()).NotEmpty()
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(255)
                .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
        }
    }
}
