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

namespace AnimeBrowser.UnitTests.Write.EpisodeTests
{
    [TestClass]
    public class EpisodeCreationTests : TestBase
    {
        private IList<Episode> allEpisodes;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;

        [TestInitialize]
        public void InitDb()
        {
            allAnimeInfos = new List<AnimeInfo> {
                new AnimeInfo { Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false },
                new AnimeInfo { Id = 2, Title = "Kuroku no Basketball", Description = string.Empty, IsNsfw = false }
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
                new Season{Id = 5401, SeasonNumber = 1, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 412,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
            };
            allEpisodes = new List<Episode> {
                new Episode { Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatusEnum.Aired, Title = "Prologue", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatusEnum.Aired, Title = "Beginning of something new", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 3, EpisodeNumber = 1, AirStatus = (int)AirStatusEnum.Aired, Title = "Family relations", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1}
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: 1, seasonId: 2) };
            }
            yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: 12, airStatus: AirStatusEnum.Airing, title: "", description: "", cover: Encoding.UTF8.GetBytes("Cover"),
                airDate: today.AddHours(-2), animeInfoId: 1, seasonId: 2) };
            yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: 15, airStatus: AirStatusEnum.Aired, title: "T", description: "D", cover: Encoding.UTF8.GetBytes("Cover"),
                airDate: today, animeInfoId: 1, seasonId: 2) };
            yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: 18, airStatus: AirStatusEnum.Aired, title: "T", description: "D", cover: Encoding.UTF8.GetBytes("Cover"),
                airDate: new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), animeInfoId: 1, seasonId: 2) };
            yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: 35, airStatus: AirStatusEnum.Aired, title: "T", description: "D", cover: Encoding.UTF8.GetBytes("Cover"),
                airDate: today.AddYears(-10), animeInfoId: 1, seasonId: 2) };
            yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: 36, airStatus: AirStatusEnum.NotAired, title: "T", description: "D", cover: Encoding.UTF8.GetBytes("Cover"),
                airDate: today.AddYears(10), animeInfoId: 1, seasonId: 2) };
        }


        private static IEnumerable<object[]> GetInvalidEpisodeNumberData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 0, -1, -3, -213 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: 1, seasonId: 2), ErrorCodes.EmptyProperty, nameof(EpisodeCreationRequestModel.EpisodeNumber) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAirStatusData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { (AirStatusEnum)(-10), (AirStatusEnum)(-30), (AirStatusEnum)3, (AirStatusEnum)4, (AirStatusEnum)(-1) };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: 1, seasonId: 2), ErrorCodes.OutOfRangeProperty, nameof(EpisodeCreationRequestModel.AirStatus) };
            }
        }

        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { new string('T', 256), new string('T', 300), new string('T', 355) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            for (var i = 0; i < titles.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: 1, seasonId: 2), ErrorCodes.TooLongProperty, nameof(EpisodeCreationRequestModel.Title)  };
            }
        }

        private static IEnumerable<object[]> GetInvalidDescriptionData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { new string('D', 30001), new string('T', 35000), new string('T', 40000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            for (var i = 0; i < description.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: 1, seasonId: 2), ErrorCodes.TooLongProperty, nameof(EpisodeCreationRequestModel.Description) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAirDateData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20, 30, 40, 50, 70, 100 };
            var titles = new string[] { "", "T", "TITLE", "TI", "TIT", "TITL", "TITLE", "TITLE1", "TITLE2", "TITLE3" };
            var description = new string[] { "", "D", "DE", "DES", "DESC", "DESCR", "DESCRI", "DESCRIP", "DESCRIPT", "DESCRIPTI" };
            var airStatuses = new AirStatusEnum[] {
                AirStatusEnum.NotAired,
                AirStatusEnum.Airing,
                AirStatusEnum.Airing,
                AirStatusEnum.Airing,
                AirStatusEnum.Aired,
                AirStatusEnum.Aired,
                AirStatusEnum.Aired,
                AirStatusEnum.Aired,
                AirStatusEnum.NotAired,
                AirStatusEnum.NotAired
            };
            var airDates = new DateTime?[] {
                today.AddYears(10).AddDays(1),
                null,
                today.AddDays(-3),
                today.AddDays(3),
                null,
                today.AddDays(1),
                new DateTime(1899,12,31, 0, 0, 0, DateTimeKind.Utc),
                today.AddYears(10).AddDays(1),
                new DateTime(1899,12,31, 0, 0, 0, DateTimeKind.Utc),
                today.AddYears(10).AddDays(1)
            };
            var errorCodes = new ErrorCodes[] {
                ErrorCodes.OutOfRangeProperty,
                ErrorCodes.EmptyProperty,
                ErrorCodes.OutOfRangeProperty,
                ErrorCodes.OutOfRangeProperty,
                ErrorCodes.EmptyProperty,
                ErrorCodes.OutOfRangeProperty,
                ErrorCodes.OutOfRangeProperty,
                ErrorCodes.OutOfRangeProperty,
                ErrorCodes.OutOfRangeProperty,
                ErrorCodes.OutOfRangeProperty
            };
            for (var i = 0; i < airDates.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: airDates[i], animeInfoId: 1, seasonId: 2), errorCodes[i], nameof(EpisodeCreationRequestModel.AirDate) };
            }
        }

        private static IEnumerable<object[]> GetInvalidCoverData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            var covers = new byte[][] { null, Encoding.UTF8.GetBytes(""), new byte[0] { } };
            for (var i = 0; i < covers.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: covers[i],
                            airDate: null, animeInfoId: 1, seasonId: 2), ErrorCodes.EmptyProperty, nameof(EpisodeCreationRequestModel.Cover) };
            }
        }

        private static IEnumerable<object[]> GetInvalidSeasonIdData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            var seasonIds = new long[] { 0, -1, -123 };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: 1, seasonId: seasonIds[i]), ErrorCodes.EmptyProperty, nameof(EpisodeCreationRequestModel.SeasonId) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAnimeInfoIdData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            var animeInfoIds = new long[] { 0, -1, -123 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: animeInfoIds[i], seasonId: 2), ErrorCodes.EmptyProperty, nameof(EpisodeCreationRequestModel.AnimeInfoId) };
            }
        }

        private static IEnumerable<object[]> GetMismatchingIdsData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            var seasonIds = new long[] { 1, 2, 5401 };
            var animeInfoIds = new long[] { 2, 3, 412 };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: animeInfoIds[i], seasonId: seasonIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingEpisodeData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 1, 2, 1 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255) };
            var description = new string[] { "", "D", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            var seasonIds = new long[] { 1, 1, 2 };
            var animeInfoIds = new long[] { 1, 1, 1 };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i], cover: Encoding.UTF8.GetBytes("Cover"),
                            airDate: null, animeInfoId: animeInfoIds[i], seasonId: seasonIds[i]) };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_ShouldWork(EpisodeCreationRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode savedEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                episodeReadRepo.Setup(er => er.IsSeasonAndAnimeInfoExistsAndReferences(It.IsAny<long>(), It.IsAny<long>()))
                    .Callback<long, long>((sId, aiId) =>
                        {
                            var season = allSeasons.SingleOrDefault(s => s.Id == sId);
                            if (season == null || season.AnimeInfoId != aiId) isExistAnimeInfoAndSeason = false;
                            else
                            {
                                var animeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId);
                                if (animeInfo == null) isExistAnimeInfoAndSeason = false;
                                else isExistAnimeInfoAndSeason = true;
                            }
                        })
                    .ReturnsAsync(() => isExistAnimeInfoAndSeason);
                episodeReadRepo.Setup(er => er.IsEpisodeWithEpisodeNumberExists(It.IsAny<long>(), It.IsAny<int>()))
                    .Callback<long, int>((sId, epNum) => isExistWithSameEpNum = allEpisodes.Any(e => e.SeasonId == sId && e.EpisodeNumber == epNum))
                    .Returns(() => isExistWithSameEpNum);
                episodeWriteRepo.Setup(ew => ew.CreateEpisode(It.IsAny<Episode>())).Callback<Episode>(e => { savedEpisode = e; savedEpisode.Id = 10; }).ReturnsAsync(() => savedEpisode!);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            var createdEpisode = await episodeCreationHandler.CreateEpisode(requestModel);
            createdEpisode.Should().BeEquivalentTo(responseModel);
        }


        [DataTestMethod,
            DynamicData(nameof(GetInvalidEpisodeNumberData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidEpisodeNumber_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
           {
               var episodeReadRepo = new Mock<IEpisodeRead>();
               var episodeWriteRepo = new Mock<IEpisodeWrite>();
               services.AddSingleton<IDateTime, DateTimeProvider>();
               services.AddTransient(factory => episodeReadRepo.Object);
               services.AddTransient(factory => episodeWriteRepo.Object);
               services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
           });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAirStatusData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidAirStatus_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidTitle_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidDescriptionData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidDescription_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAirDateData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidAirDate_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidCoverData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidCover_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidSeasonIdData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidSeasonId_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidAnimeInfoId_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToCreationResponseModel();
            responseModel.Id = 10;
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }


        [DataTestMethod,
            DynamicData(nameof(GetMismatchingIdsData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_MismatchingIds_ThrowException(EpisodeCreationRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode savedEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                episodeReadRepo.Setup(er => er.IsSeasonAndAnimeInfoExistsAndReferences(It.IsAny<long>(), It.IsAny<long>()))
                    .Callback<long, long>((sId, aiId) =>
                    {
                        var season = allSeasons.SingleOrDefault(s => s.Id == sId);
                        if (season == null || season.AnimeInfoId != aiId) isExistAnimeInfoAndSeason = false;
                        else
                        {
                            var animeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId);
                            if (animeInfo == null) isExistAnimeInfoAndSeason = false;
                            else isExistAnimeInfoAndSeason = true;
                        }
                    })
                    .ReturnsAsync(() => isExistAnimeInfoAndSeason);
                episodeReadRepo.Setup(er => er.IsEpisodeWithEpisodeNumberExists(It.IsAny<long>(), It.IsAny<int>()))
                    .Callback<long, int>((sId, epNum) => isExistWithSameEpNum = allEpisodes.Any(e => e.SeasonId == sId && e.EpisodeNumber == epNum))
                    .Returns(() => isExistWithSameEpNum);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            await createEpisodeFunc.Should().ThrowAsync<MismatchingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetAlreadyExistingEpisodeData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_AlreadyExistingEpisode_ThrowException(EpisodeCreationRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode savedEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                episodeReadRepo.Setup(er => er.IsSeasonAndAnimeInfoExistsAndReferences(It.IsAny<long>(), It.IsAny<long>()))
                    .Callback<long, long>((sId, aiId) =>
                    {
                        var season = allSeasons.SingleOrDefault(s => s.Id == sId);
                        if (season == null || season.AnimeInfoId != aiId) isExistAnimeInfoAndSeason = false;
                        else
                        {
                            var animeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId);
                            if (animeInfo == null) isExistAnimeInfoAndSeason = false;
                            else isExistAnimeInfoAndSeason = true;
                        }
                    })
                    .ReturnsAsync(() => isExistAnimeInfoAndSeason);
                episodeReadRepo.Setup(er => er.IsEpisodeWithEpisodeNumberExists(It.IsAny<long>(), It.IsAny<int>()))
                    .Callback<long, int>((sId, epNum) => isExistWithSameEpNum = allEpisodes.Any(e => e.SeasonId == sId && e.EpisodeNumber == epNum))
                    .Returns(() => isExistWithSameEpNum);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            await createEpisodeFunc.Should().ThrowAsync<AlreadyExistingObjectException<Episode>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_ThrowException(EpisodeCreationRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                episodeReadRepo.Setup(er => er.IsSeasonAndAnimeInfoExistsAndReferences(It.IsAny<long>(), It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            await createEpisodeFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public async Task CreateEpisode_NullObject_ThrowException()
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode savedEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(null);
            await createEpisodeFunc.Should().ThrowAsync<EmptyObjectException<EpisodeCreationRequestModel>>();
        }



        [TestCleanup]
        public void CleanDb()
        {
            allEpisodes.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allEpisodes = null;
            allSeasons = null;
            allAnimeInfos = null;
        }
    }
}
