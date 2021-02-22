using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.DateTimeProviders;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using AnimeBrowser.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.SeasonTests
{
    [TestClass]
    public class SeasonCreationTests : TestBase
    {
        public IList<AnimeInfo> allAnimeInfos;

        [TestInitialize]
        public void InitDb()
        {
            allAnimeInfos = new List<AnimeInfo>
            {
                new AnimeInfo {Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false },
                new AnimeInfo {Id = 2, Title = "Boku no Hero Academia", Description = string.Empty, IsNsfw = false },
                new AnimeInfo {Id = 10, Title = "Dorohedoro", Description = string.Empty, IsNsfw = true},
                new AnimeInfo {Id = 15, Title = "Shingeki no Kyojin", Description = string.Empty, IsNsfw = true },
                new AnimeInfo {Id = 201, Title = "Yakusoku no Neverland", Description = "Neverland...", IsNsfw = false }
            };
        }

        private static IEnumerable<object[]> GetBasicDatesData()
        {
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] { new SeasonCreationRequestModel(
                    seasonNumber: seasonNumbers[i],
                    title: titles[i],
                    description: descriptions[i],
                    startDate: startDates[i],
                    endDate: endDates[i],
                    airStatus: airStatuses[i],
                    numberOfEpisodes: numOfEpisodes[i],
                    coverCarousel: coverCarousels[i],
                    cover: covers[i],
                    animeInfoId: animeInfoIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetInvalidSeasonNumbersData()
        {
            var propertyName = nameof(SeasonCreationRequestModel.SeasonNumber);
            var seasonNumbers = new int[] { 0, -1, -100, -9999999, -10213 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddDays(876)
            };
            var endDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddDays(877)
            };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    ErrorCodes.EmptyProperty,
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            var propertyName = nameof(SeasonCreationRequestModel.Title);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "", null, new string('T', 256), new string('T', 300), new string('T', 10000) };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddDays(876)
            };
            var endDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddDays(877)
            };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    errorCodes[i],
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidDescriptionData()
        {
            var propertyName = nameof(SeasonCreationRequestModel.Description);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { new string('D', 642510), new string('D', 30001), new string('D', 30002), new string('D', 987612), new string('D', 12313123) };
            var startDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddDays(876)
            };
            var endDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddDays(877)
            };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    ErrorCodes.TooLongProperty,
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidStartDateData()
        {
            var errorCodes = new ErrorCodes[] { ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.EmptyProperty };
            var propertyName = nameof(SeasonCreationRequestModel.StartDate);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] {
                new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                DateTime.UtcNow.AddYears(10).AddDays(1),
                DateTime.Now.AddYears(32),
                null
            };
            var endDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                null,
                null,
                DateTime.Now.AddDays(877)
            };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    errorCodes[i],
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidEndDateData()
        {
            var propertyName = nameof(SeasonCreationRequestModel.EndDate);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] {
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddDays(876)
            };
            var endDates = new DateTime?[] {
                new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10).AddDays(1),
                DateTime.Now.AddDays(877),
                DateTime.Now.AddYears(10).AddDays(1)
            };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]) ,
                    ErrorCodes.OutOfRangeProperty,
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidAirStatusData()
        {
            var errorCodes = new ErrorCodes[] { ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.EmptyProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty };
            var propertyNames = new string[] { nameof(SeasonCreationRequestModel.AirStatus), nameof(SeasonCreationRequestModel.AirStatus), nameof(SeasonCreationRequestModel.StartDate), nameof(SeasonCreationRequestModel.StartDate), nameof(SeasonCreationRequestModel.EndDate) };
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                null,
                new DateTime(1800, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddDays(876)
            };
            var endDates = new DateTime?[] {
                null,
                new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc),
                DateTime.Now.AddYears(10),
                DateTime.Now.AddYears(12)
            };
            var airStatuses = new AirStatusEnum[] {
                (AirStatusEnum)(-1),
                (AirStatusEnum)(-10),
                AirStatusEnum.Airing,
                AirStatusEnum.Airing,
                AirStatusEnum.Aired
            };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    errorCodes[i],
                    propertyNames[i]
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidNumOfEpisodesData()
        {
            var propertyName = nameof(SeasonCreationRequestModel.NumberOfEpisodes);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { 0, -1, -123, -400, -675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]) ,
                    ErrorCodes.EmptyProperty,
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidCoverCarouselData()
        {
            var propertyName = nameof(SeasonCreationRequestModel.CoverCarousel);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1), new DateTime(2020, 12, 22), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1), new DateTime(2020, 12, 23), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { Array.Empty<byte>() };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < coverCarousels.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    ErrorCodes.EmptyProperty,
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidCoverData()
        {
            var propertyName = nameof(SeasonCreationRequestModel.Cover);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1), new DateTime(2020, 12, 22), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1), new DateTime(2020, 12, 23), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { Encoding.UTF8.GetBytes("C") };
            var covers = new byte[]?[] { Array.Empty<byte>() };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < covers.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    ErrorCodes.EmptyProperty,
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidAnimeInfoIdData()
        {
            var propertyName = nameof(SeasonCreationRequestModel.AnimeInfoId);
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 0, -5, -123 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                yield return new object[] {
                    new SeasonCreationRequestModel(
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i]),
                    ErrorCodes.EmptyProperty,
                    propertyName
                };
            }
        }

        private static IEnumerable<object[]> GetNotExistingAnimeInfoIdData()
        {
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 3, 5, 37, 90, 122 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] { new SeasonCreationRequestModel(
                    seasonNumber: seasonNumbers[i],
                    title: titles[i],
                    description: descriptions[i],
                    startDate: startDates[i],
                    endDate: endDates[i],
                    airStatus: airStatuses[i],
                    numberOfEpisodes: numOfEpisodes[i],
                    coverCarousel: coverCarousels[i],
                    cover: covers[i],
                    animeInfoId: animeInfoIds[i]) };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicDatesData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_WithBasicDates_ShouldWork(SeasonCreationRequestModel requestModel)
        {
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            var createdSeason = await seasonCreationHandler.CreateSeason(requestModel);

            createdSeason.Should().NotBeNull();
            createdSeason.Should().BeEquivalentTo(responseModel);
        }

        #region If I don't separate these tests into multiple tests, then VS kills itself when viewing test results.....

        [DataTestMethod,
            DynamicData(nameof(GetInvalidSeasonNumbersData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidSeasonNumbers_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidTitle_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidDescriptionData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidDescription_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidStartDateData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidStartDate_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidEndDateData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidEndDate_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAirStatusData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidAirStatus_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidNumOfEpisodesData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidNumOfEpisodes_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidCoverCarouselData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidCoverCarousel_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidCoverData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidCover_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_InvalidAnimeInfoId_ThrowException(SeasonCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            var valEx = await createSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        #endregion If I don't separate these tests into multiple tests, then VS kills itself when viewing test results.....


        [DataTestMethod,
            DynamicData(nameof(GetNotExistingAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_NotExistingAnimeInfoId_ThrowException(SeasonCreationRequestModel requestModel)
        {
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            await createSeasonFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }

        [TestMethod]
        public async Task CreateSeason_NullObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(null);

            await createSeasonFunc.Should().ThrowAsync<EmptyObjectException<SeasonCreationRequestModel>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicDatesData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_ThrowException(SeasonCreationRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            Func<Task> createSeasonFunc = async () => await seasonCreationHandler.CreateSeason(requestModel);

            await createSeasonFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanDb()
        {
            allAnimeInfos.Clear();
            allAnimeInfos = null;
        }
    }
}
