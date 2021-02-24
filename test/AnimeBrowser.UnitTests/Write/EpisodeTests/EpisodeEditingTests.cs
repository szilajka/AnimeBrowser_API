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
    public class EpisodeEditingTests : TestBase
    {
        private static IList<EpisodeEditingRequestModel> allRequestModels;
        private IList<Episode> allEpisodes;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;

        [ClassInitialize]
        public static void InitClassDb(TestContext context)
        {
            allRequestModels = new List<EpisodeEditingRequestModel>();

            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

            var ids = new long[] { 1, 2, 3, 4, 5,
                1, 2, 3, 4, 5, 6 };
            var epNumbers = new int[] { 1, 2, 1, 2, 3,
                1, 2, 1, 2, 3, 13 };
            var titles = new string[] { "", "T", new string('T', 150), new string('T', 254), new string('T', 255),
                "T", "TI", "TIT", "TITL", "TITLE", "TITLEE" };
            var description = new string[] { "", "D", new string('D', 1500), new string('D', 29999), new string('D', 30000),
                "D", "DE", "DES", "DESC", "DESCR", "DESCRI" };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.NotAired,
                AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.Aired, AirStatusEnum.Aired, AirStatusEnum.NotAired, AirStatusEnum.NotAired };
            var airDates = new DateTime?[] { null, null, null, null, null,
                today.AddHours(-2), today, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), today.AddYears(-10), today.AddYears(10), today.AddYears(5) };
            var animeInfoIds = new long[] { 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1 };
            var seasonIds = new long[] { 1, 1, 2, 2, 2,
                2, 2, 2, 2, 2, 2 };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                var epReqMod = new EpisodeEditingRequestModel(id: ids[i], episodeNumber: epNumbers[i], airStatus: airStatuses[i], title: titles[i], description: description[i],
                   cover: Encoding.UTF8.GetBytes("Cover"), airDate: airDates[i], animeInfoId: animeInfoIds[i], seasonId: seasonIds[i]);
                allRequestModels.Add(epReqMod);
            }
        }

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
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 4, EpisodeNumber = 2, AirStatus = (int)AirStatusEnum.Aired, Title = "S2 Episode 2", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2014, 3, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 5, EpisodeNumber = 3, AirStatus = (int)AirStatusEnum.Aired, Title = "S2 Episode 3", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2014, 3, 15, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 6, EpisodeNumber = 4, AirStatus = (int)AirStatusEnum.Aired, Title = "S2 Episode 4", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 21, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 7, EpisodeNumber = 5, AirStatus = (int)AirStatusEnum.Aired, Title = "S2 Episode 5", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2014, 3, 28, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 8, EpisodeNumber = 6, AirStatus = (int)AirStatusEnum.Aired, Title = "S2 Episode 6", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 4, 5, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1}
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, erm };
            }
        }


        private static IEnumerable<object[]> GetInvalidEpisodeNumberData()
        {
            var epNumbers = new int[] { 0, -1, -3, -213 };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id,episodeNumber: epNumbers[i], airStatus: erm.AirStatus, title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.EmptyProperty, nameof(EpisodeEditingRequestModel.EpisodeNumber) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAirStatusData()
        {
            var airStatuses = new AirStatusEnum[] { (AirStatusEnum)(-10), (AirStatusEnum)(-30), (AirStatusEnum)3, (AirStatusEnum)4, (AirStatusEnum)(-1) };
            for (var i = 0; i < airStatuses.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: airStatuses[i], title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.OutOfRangeProperty, nameof(EpisodeEditingRequestModel.AirStatus) };
            }
        }

        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            var titles = new string[] { new string('T', 256), new string('T', 300), new string('T', 355) };
            for (var i = 0; i < titles.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: titles[i], description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.TooLongProperty, nameof(EpisodeEditingRequestModel.Title) };
            }
        }

        private static IEnumerable<object[]> GetInvalidDescriptionData()
        {
            var description = new string[] { new string('D', 30001), new string('T', 35000), new string('T', 40000) };
            for (var i = 0; i < description.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: description[i], cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.TooLongProperty, nameof(EpisodeEditingRequestModel.Description) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAirDateData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
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
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: airStatuses[i], title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: airDates[i], animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), errorCodes[i], nameof(EpisodeEditingRequestModel.AirDate) };
            }
        }

        private static IEnumerable<object[]> GetInvalidCoverData()
        {
            var covers = new byte[][] { null, Encoding.UTF8.GetBytes(""), new byte[0] { } };
            for (var i = 0; i < covers.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description, cover: covers[i],
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.EmptyProperty, nameof(EpisodeEditingRequestModel.Cover) };
            }
        }

        private static IEnumerable<object[]> GetInvalidSeasonIdData()
        {
            var seasonIds = new long[] { 0, -1, -123 };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: seasonIds [i]), ErrorCodes.EmptyProperty, nameof(EpisodeEditingRequestModel.SeasonId) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAnimeInfoIdData()
        {
            var animeInfoIds = new long[] { 0, -1, -123 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: animeInfoIds[i], seasonId: erm.SeasonId), ErrorCodes.EmptyProperty, nameof(EpisodeEditingRequestModel.AnimeInfoId) };
            }
        }

        private static IEnumerable<object[]> GetMismatchingIdsData()
        {
            var ids = new long[] { 1, 2, 5 };
            var seasonIds = new long[] { 1, 2, 5401 };
            var animeInfoIds = new long[] { 2, 3, 412 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { ids[i], new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: animeInfoIds[i], seasonId: seasonIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingEpisodeData()
        {
            var epNumbers = new int[] { 2, 1, 4 };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { erm.Id, new EpisodeEditingRequestModel(id: erm.Id, episodeNumber: epNumbers[i], airStatus: erm.AirStatus, title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId) };
            }
        }

        private static IEnumerable<object[]> GetNotExistingIdData()
        {
            var ids = new int[] { 123, 200 };
            for (var i = 0; i < ids.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { ids[i], new EpisodeEditingRequestModel(id: ids[i], episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description, cover: erm.Cover,
                            airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId) };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_ShouldWork(long id, EpisodeEditingRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode savedEpisode = null;
            Episode foundEpisode = null;
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
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                episodeWriteRepo.Setup(ew => ew.UpdateEpisode(It.IsAny<Episode>())).Callback<Episode>(e => savedEpisode = e).ReturnsAsync(() => savedEpisode!);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            var createdEpisode = await episodeEditingHandler.EditEpisode(id, requestModel);
            createdEpisode.Should().BeEquivalentTo(responseModel);
        }


        [DataTestMethod,
            DynamicData(nameof(GetInvalidEpisodeNumberData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidEpisodeNumber_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAirStatusData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidAirStatus_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidTitle_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidDescriptionData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidDescription_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAirDateData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidAirDate_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidCoverData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidCover_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidSeasonIdData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidSeasonId_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_InvalidAnimeInfoId_ThrowException(long id, EpisodeEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var responseModel = requestModel.ToEpisode().ToEditingResponseModel();
            responseModel.Id = 10;
            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }


        [DataTestMethod,
            DynamicData(nameof(GetMismatchingIdsData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_MismatchingIds_ThrowException(long id, EpisodeEditingRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode foundEpisode = null;
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
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            await createEpisodeFunc.Should().ThrowAsync<MismatchingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetAlreadyExistingEpisodeData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_AlreadyExistingEpisode_ThrowException(long id, EpisodeEditingRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode foundEpisode = null;
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
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            await createEpisodeFunc.Should().ThrowAsync<AlreadyExistingObjectException<Episode>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingIdData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_NotExistingId_ThrowException(long id, EpisodeEditingRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            var isExistAnimeInfoAndSeason = false;
            Episode foundEpisode = null;
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
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            await createEpisodeFunc.Should().ThrowAsync<NotFoundObjectException<EpisodeEditingRequestModel>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task EditingEpisode_ThrowException(long id, EpisodeEditingRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(id, requestModel);
            await createEpisodeFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public async Task EditingEpisode_NullObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            });

            var episodeEditingHandler = sp.GetService<IEpisodeEditing>();
            Func<Task> createEpisodeFunc = async () => await episodeEditingHandler.EditEpisode(1, null);
            await createEpisodeFunc.Should().ThrowAsync<MismatchingIdException>();
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

        [ClassCleanup]
        public static void CleanClassDb()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
