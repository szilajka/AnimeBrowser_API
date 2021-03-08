using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators.MainValidators
{
    public class GenreCreationValidator : AbstractValidator<GenreCreationRequestModel>
    {
        public GenreCreationValidator()
        {
            Transform(x => x.GenreName, x => string.IsNullOrEmpty(x) ? x : x.Trim()).NotEmpty()
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(100)
                .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            Transform(x => x.Description, x => string.IsNullOrEmpty(x) ? x : x.Trim()).NotEmpty()
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                .MaximumLength(10000)
                .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
        }
    }
}
