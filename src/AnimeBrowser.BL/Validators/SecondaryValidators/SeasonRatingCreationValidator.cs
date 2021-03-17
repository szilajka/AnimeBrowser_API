using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators.SecondaryValidators
{
    public class SeasonRatingCreationValidator : AbstractValidator<SeasonRatingCreationRequestModel>
    {
        public SeasonRatingCreationValidator()
        {
            const int minRating = 1;
            const int maxRating = 5;

            RuleFor(x => x.Rating).InclusiveBetween(minRating, maxRating)
                .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());

            When(x => !string.IsNullOrWhiteSpace(x.Message), () =>
            {
                Transform(x => x.Message, x => x.Trim()).MaximumLength(30000)
                    .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            });
        }
    }
}
