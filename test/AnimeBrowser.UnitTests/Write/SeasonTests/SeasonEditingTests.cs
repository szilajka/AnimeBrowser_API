using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.DateTimeProviders;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.SeasonTests
{
    [TestClass]
    public class SeasonEditingTests : TestBase
    {
        public IList<AnimeInfo> allAnimeInfos;
        public IList<Season> allSeasons;

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

            allSeasons = new List<Season>
            {
                new Season{Id = 1, SeasonNumber = 1, Title = "Phantom Blood", Description = "In this season we know the story of Jonathan, Dio and Speedwagon, then Joseph and the Pillarmen's story",
                    StartDate = new DateTime(2012, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2012, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 2, SeasonNumber = 1, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 3, SeasonNumber = 1, Title = "Season 1", Description = "I don't know this anime. Maybe they are just fighting. Who knows? I'm sure not.",
                    StartDate = new DateTime(2013, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 2, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 40, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("BnHACarousel"), Cover = Encoding.UTF8.GetBytes("BnHACover"),
                },
                new Season{Id = 10, SeasonNumber = 1, Title = "Season 1", Description = "Kayman and Nikkaido's story. Kayman is a man, but has a lizard body, well, some magician did it to him, but who?",
                    StartDate = new DateTime(2020, 9, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2020, 12, 20, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 10, AnimeInfoId = 10,
                    CoverCarousel = Encoding.UTF8.GetBytes("DorohedoroCarousel"), Cover = Encoding.UTF8.GetBytes("DorohedoroCover"),
                },
                new Season{Id = 20, SeasonNumber = 1, Title = "Season 4", Description = "I don't know this anime. Maybe they are just fighting. Who knows? I'm sure not.",
                    StartDate = new DateTime(2020, 11, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2021, 4, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Airing, NumberOfEpisodes = 24, AnimeInfoId = 15,
                    CoverCarousel = Encoding.UTF8.GetBytes("SnKCarousel"), Cover = Encoding.UTF8.GetBytes("SnKCover"),
                },
                new Season{Id = 22, SeasonNumber = 1, Title = "Season 2", Description = "I don't know this anime. But there will be a second season. That's for sure!",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatusEnum.NotAired, NumberOfEpisodes = 20, AnimeInfoId = 201,
                    CoverCarousel = Encoding.UTF8.GetBytes("YnKCarousel"), Cover = Encoding.UTF8.GetBytes("YnKCover"),
                }
            };
        }

        private static IEnumerable<object[]> GetBasicDatesData()
        {
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 22, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 12, 23, 0, 0, 0, DateTimeKind.Utc), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 1, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] {
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
                        seasonNumber: seasonNumbers[i],
                        title: titles[i],
                        description: descriptions[i],
                        startDate: startDates[i],
                        endDate: endDates[i],
                        airStatus: airStatuses[i],
                        numberOfEpisodes: numOfEpisodes[i],
                        coverCarousel: coverCarousels[i],
                        cover: covers[i],
                        animeInfoId: animeInfoIds[i])
                };
            }
        }

        private static IEnumerable<object[]> GetInvalidSeasonNumbersData()
        {
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var propertyName = nameof(SeasonEditingRequestModel.SeasonNumber);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            var propertyName = nameof(SeasonEditingRequestModel.Title);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var propertyName = nameof(SeasonEditingRequestModel.Description);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var errorCodes = new ErrorCodes[] { ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.EmptyProperty };
            var propertyName = nameof(SeasonEditingRequestModel.StartDate);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var propertyName = nameof(SeasonEditingRequestModel.EndDate);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var errorCodes = new ErrorCodes[] { ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.EmptyProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty };
            var propertyNames = new string[] { nameof(SeasonEditingRequestModel.AirStatus), nameof(SeasonEditingRequestModel.AirStatus), nameof(SeasonEditingRequestModel.StartDate), nameof(SeasonEditingRequestModel.StartDate), nameof(SeasonEditingRequestModel.EndDate) };
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var propertyName = nameof(SeasonEditingRequestModel.NumberOfEpisodes);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var propertyName = nameof(SeasonEditingRequestModel.CoverCarousel);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var propertyName = nameof(SeasonEditingRequestModel.Cover);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
            var propertyName = nameof(SeasonEditingRequestModel.AnimeInfoId);
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
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
            var ids = new long[] { 1, 2, 10, 20, 22 };
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
                yield return new object[] {
                    ids[i],
                    new SeasonEditingRequestModel(
                        id: ids[i],
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
        public async Task EditSeason_WithBasicDates_ShouldWork(long seasonId, SeasonEditingRequestModel requestModel)
        {
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            var createdSeason = await seasonEditingHandler.EditSeason(seasonId, requestModel);

            createdSeason.Should().NotBeNull();
            createdSeason.Should().BeEquivalentTo(responseModel);
        }

        #region If I don't separate these tests into multiple tests, then VS kills itself when viewing test results.....

        [DataTestMethod,
            DynamicData(nameof(GetInvalidSeasonNumbersData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidSeasonNumbers_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidTitle_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidDescriptionData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidDescription_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidStartDateData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidStartDate_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidEndDateData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidEndDate_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAirStatusData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidAirStatus_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidNumOfEpisodesData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidNumOfEpisodes_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidCoverCarouselData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidCoverCarousel_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidCoverData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidCover_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task EditSeason_InvalidAnimeInfoId_ThrowException(long seasonId, SeasonEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            Season foundSeason = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.UpdateSeason(It.IsAny<Season>())).Callback<Season>(s => callbackSeason = s).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
             
            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            var valEx = await EditSeasonFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        #endregion If I don't separate these tests into multiple tests, then VS kills itself when viewing test results.....


        [DataTestMethod,
            DynamicData(nameof(GetNotExistingAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task EditSeason_NotExistingAnimeInfoId_ThrowException(long seasonId, SeasonEditingRequestModel requestModel)
        {
            Season foundSeason = null;
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            await EditSeasonFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }

        [TestMethod]
        public async Task EditSeason_NullObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(1, null);

            await EditSeasonFunc.Should().ThrowAsync<MismatchingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicDatesData), DynamicDataSourceType.Method)]
        public async Task EditSeason_ThrowException(long seasonId, SeasonEditingRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            });

            var seasonEditingHandler = sp.GetService<ISeasonEditing>();
            Func<Task> EditSeasonFunc = async () => await seasonEditingHandler.EditSeason(seasonId, requestModel);

            await EditSeasonFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanDb()
        {
            allAnimeInfos.Clear();
            allSeasons.Clear();
            allAnimeInfos = null;
            allSeasons = null;
        }

    }
}
