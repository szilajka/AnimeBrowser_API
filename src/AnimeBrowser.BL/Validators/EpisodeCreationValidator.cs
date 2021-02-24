using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using FluentValidation;
using System;

namespace AnimeBrowser.BL.Validators
{
    public class EpisodeCreationValidator : AbstractValidator<EpisodeCreationRequestModel>
    {
        public EpisodeCreationValidator(IDateTime dateTimeProvider, DateTime? seasonStartDate, DateTime? seasonEndDate)
        {
            var minDate = dateTimeProvider.FromYearUtc(1900);
            var maxDate = dateTimeProvider.FromDateUtc(dateTimeProvider.UtcNow.AddYears(10));
            var beforeTwoDays = dateTimeProvider.FromDateUtc(dateTimeProvider.UtcNow.AddDays(-2));
            var afterTwoDays = dateTimeProvider.FromDateUtc(dateTimeProvider.UtcNow.AddDays(2));
            var today = dateTimeProvider.FromDateUtc(dateTimeProvider.UtcNow);
            DateTime startDate = minDate;
            DateTime endDate = maxDate;
            if (seasonStartDate.HasValue)
            {
                startDate = dateTimeProvider.FromDateUtc(seasonStartDate.Value);
            }
            if (seasonEndDate.HasValue)
            {
                endDate = dateTimeProvider.FromDateUtc(seasonEndDate.Value);
            }

            RuleFor(x => x.EpisodeNumber).GreaterThan(0)
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
            RuleFor(x => x.AirStatus).IsInEnum()
                .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
            When(x => !string.IsNullOrEmpty(x.Title), () =>
            {
                Transform(x => x.Title, x => x.Trim()).MaximumLength(255)
                    .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            });
            When(x => !string.IsNullOrEmpty(x.Description), () =>
            {
                Transform(x => x.Description, x => x.Trim()).MaximumLength(30000)
                    .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());
            });
            When(x => x.AirStatus == AirStatusEnum.Airing, () =>
            {
                RuleFor(x => x.AirDate).NotEmpty()
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());

                When(x => x.AirDate.HasValue, () =>
                {
                    Transform(x => x.AirDate, x => dateTimeProvider.FromDateUtc(x!.Value)).InclusiveBetween(beforeTwoDays, afterTwoDays)
                        .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                });
            });

            When(x => x.AirStatus == AirStatusEnum.Aired, () =>
            {
                RuleFor(x => x.AirDate).NotEmpty()
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());

                When(x => x.AirDate.HasValue, () =>
                {
                    Transform(x => x.AirDate, x => dateTimeProvider.FromDateUtc(x!.Value)).InclusiveBetween(startDate, endDate)
                        .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                });
            });
            When(x => x.AirDate.HasValue && x.AirStatus != AirStatusEnum.Airing && x.AirStatus != AirStatusEnum.Aired, () =>
            {
                Transform(x => x.AirDate, x => dateTimeProvider.FromDateUtc(x!.Value)).InclusiveBetween(startDate, endDate)
                        .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
            });
            RuleFor(x => x.Cover).NotEmpty()
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
            RuleFor(x => x.SeasonId).GreaterThan(0)
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
            RuleFor(x => x.AnimeInfoId).GreaterThan(0)
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
        }
    }
}
