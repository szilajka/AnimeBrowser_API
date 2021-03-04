﻿using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using FluentValidation;

namespace AnimeBrowser.BL.Validators
{
    public class SeasonEditingValidator : AbstractValidator<SeasonEditingRequestModel>
    {
        public SeasonEditingValidator(IDateTime dateTimeProvider)
        {
            var minDate = dateTimeProvider.FromYearUtc(1900);
            var maxDate = dateTimeProvider.FromDateUtc(dateTimeProvider.UtcNow.AddYears(10));
            RuleFor(x => x).NotNull().WithErrorCode(ErrorCodes.EmptyObject.GetIntValueAsString());
            When(x => x != null, () =>
            {
                RuleFor(x => x.Id).GreaterThan(0)
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
                RuleFor(x => x.SeasonNumber).NotNull()
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());

                Transform(x => x.Title, x => string.IsNullOrEmpty(x) ? x : x.Trim()).NotEmpty()
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                    .MaximumLength(255)
                    .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());

                Transform(x => x.Description, x => string.IsNullOrEmpty(x) ? x : x.Trim()).MaximumLength(30000)
                    .WithErrorCode(ErrorCodes.TooLongProperty.GetIntValueAsString());

                When(x => x.NumberOfEpisodes.HasValue, () =>
                {
                    RuleFor(x => x.NumberOfEpisodes).GreaterThan(0)
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
                });

                When(x => x.CoverCarousel != null, () =>
                {
                    RuleFor(x => x.CoverCarousel).NotEmpty().WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
                });

                When(x => x.Cover != null, () =>
                {
                    RuleFor(x => x.Cover).NotEmpty().WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
                });

                RuleFor(x => x.AnimeInfoId).NotNull()
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString())
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());


                RuleFor(x => x.AirStatus).IsInEnum()
                    .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());

                When(x => x.AirStatus == AirStatusEnum.Airing, () =>
                {
                    RuleFor(x => x.StartDate).NotNull()
                        .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());

                    When(x => x.StartDate.HasValue, () =>
                    {
                        Transform(x => x.StartDate, x => dateTimeProvider.FromDateUtc(x!.Value)).InclusiveBetween(minDate, maxDate)
                           .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                    });

                    When(x => x.EndDate.HasValue && x.StartDate.HasValue, () =>
                    {
                        Transform(x => x.EndDate, x => dateTimeProvider.FromDateUtc(x!.Value)).GreaterThanOrEqualTo(x => dateTimeProvider.FromDateUtc(x.StartDate!.Value))
                            .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString())
                            .LessThanOrEqualTo(maxDate)
                            .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                    });
                });

                When(x => x.AirStatus == AirStatusEnum.Aired, () =>
                {
                    RuleFor(x => x.StartDate).NotNull()
                        .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());

                    RuleFor(x => x.EndDate).NotNull()
                        .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());

                    When(x => x.StartDate.HasValue, () =>
                    {
                        Transform(x => x.StartDate, x => dateTimeProvider.FromDateUtc(x!.Value))
                            .InclusiveBetween(minDate, maxDate)
                            .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                    });

                    When(x => x.EndDate.HasValue && x.StartDate.HasValue, () =>
                    {
                        Transform(x => x.EndDate, x => dateTimeProvider.FromDateUtc(x!.Value))
                            .GreaterThanOrEqualTo(x => dateTimeProvider.FromDateUtc(x.StartDate!.Value))
                            .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString())
                            .LessThanOrEqualTo(maxDate)
                            .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                    });
                });

                When(x => x.StartDate.HasValue && x.AirStatus != AirStatusEnum.Airing && x.AirStatus != AirStatusEnum.Aired, () =>
                {
                    Transform(x => x.StartDate, x => dateTimeProvider.FromDateUtc(x!.Value)).InclusiveBetween(minDate, maxDate)
                        .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                });
                When(x => x.EndDate.HasValue && x.AirStatus != AirStatusEnum.Airing && x.AirStatus != AirStatusEnum.Aired, () =>
                {
                    Transform(x => x.StartDate, sd => sd.HasValue ? dateTimeProvider.FromDateUtc(sd.Value) : sd).NotNull()
                        .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
                });
                When(x => x.EndDate.HasValue && x.StartDate.HasValue && x.AirStatus != AirStatusEnum.Airing && x.AirStatus != AirStatusEnum.Aired, () =>
                {
                    Transform(x => x.EndDate, x => dateTimeProvider.FromDateUtc(x!.Value)).GreaterThanOrEqualTo(x => dateTimeProvider.FromDateUtc(x.StartDate!.Value))
                        .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString())
                        .LessThanOrEqualTo(maxDate)
                        .WithErrorCode(ErrorCodes.OutOfRangeProperty.GetIntValueAsString());
                });
            });
        }
    }
}
