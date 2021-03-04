using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators
{
    public class AnimeInfoCreationValidator : AbstractValidator<AnimeInfoCreationRequestModel>
    {
        public AnimeInfoCreationValidator()
        {
            RuleFor(x => x).NotNull().WithErrorCode(ErrorCodes.EmptyObject.GetIntValueAsString());
            When(x => x != null, () =>
            {
                Transform(x => x.Title, x => string.IsNullOrEmpty(x) ? x : x.Trim()).NotEmpty()
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                    .MaximumLength(255)
                    .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
                Transform(x => x.Description, x => string.IsNullOrEmpty(x) ? x : x.Trim()).MaximumLength(30000)
                    .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            });
        }
    }
}
