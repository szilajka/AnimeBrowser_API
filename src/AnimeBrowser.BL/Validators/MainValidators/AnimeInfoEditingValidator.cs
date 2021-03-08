using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators.MainValidators
{
    public class AnimeInfoEditingValidator : AbstractValidator<AnimeInfoEditingRequestModel>
    {
        public AnimeInfoEditingValidator()
        {
            RuleFor(x => x).NotNull().WithErrorCode(ErrorCodes.EmptyObject.GetIntValueAsString());
            When(x => x != null, () =>
            {
                RuleFor(x => x.Id).GreaterThan(0).WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
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
