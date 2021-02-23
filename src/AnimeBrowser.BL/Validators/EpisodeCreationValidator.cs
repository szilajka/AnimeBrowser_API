﻿using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Validators
{
    public class EpisodeCreationValidator : AbstractValidator<EpisodeCreationRequestModel>
    {
        public EpisodeCreationValidator(IDateTime dateTimeProvider)
        {
            var minDate = dateTimeProvider.FromYearUtc(1900);
            var maxDate = dateTimeProvider.UtcNow.AddYears(10);
            RuleFor(x => x.EpisodeNumber).GreaterThan(0)
                .WithErrorCode(ErrorCodes.EmptyProperty.GetIntValueAsString());
            RuleFor(x => x.AirStatus).IsInEnum();
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
            When(x => x.AirDate.HasValue, () =>
            {
                Transform(x => x.AirDate, x => dateTimeProvider.FromDateUtc(x!.Value)).InclusiveBetween(minDate, maxDate)
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
