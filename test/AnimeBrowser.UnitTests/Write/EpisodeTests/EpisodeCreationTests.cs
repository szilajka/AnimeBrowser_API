using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Services.DateTimeProviders;
using AnimeBrowser.BL.Services.Write.MainHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Data.Converters.MainConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
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
        private static IList<EpisodeCreationRequestModel> allRequestModels;

        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<EpisodeCreationRequestModel>();

            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var epNumbers = new int[] { 5, 6, 7, 10, 20,
                5, 6, 7, 10, 20 };
            var titles = new string[] { "", "T", "New beginning", "Afterlife", "All about she said, it was all just a dream, thinking 'bout meat <3",
                $"{new string(' ', 150)}Afterlife{new string(' ', 150)}", $"{new string(' ', 300)}All that she said", $"All the time{new string(' ', 300)}", new string('T', 254), new string('T', 255)};
            var descriptions = new string[] { "", "D", "Just a description", "Tetris was a good game, this episode sets a memory to it", "In this episode all characters dies due to some mysterious accident and they revive before it happens, again and again, and they have to suffer until they figure a way out",
                $"{new string(' ', 100)}Description{new string(' ', 30000)}", $"Description{new string(' ', 34567)}", new string('D', 1500), new string('T', 29999), new string('T', 30000) };
            var covers = new byte[][] { Encoding.UTF8.GetBytes("S1Ep5Cover"), Encoding.UTF8.GetBytes("S1Ep6Cover"), Encoding.UTF8.GetBytes("S1Ep7Cover"), Encoding.UTF8.GetBytes("S1Ep10Cover"), Encoding.UTF8.GetBytes("S1Ep20Cover"),
                Encoding.UTF8.GetBytes("S1Ep5Cover"), Encoding.UTF8.GetBytes("S1Ep6Cover"), Encoding.UTF8.GetBytes("S1Ep7Cover"), Encoding.UTF8.GetBytes("S1Ep10Cover"), Encoding.UTF8.GetBytes("S1Ep20Cover") };
            var airDates = new DateTime?[] { null, null, null, null, null,
                today.AddHours(-2), new DateTime(2014, 6, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2014, 4, 15, 0, 0, 0, DateTimeKind.Utc), today.AddYears(-10), today.AddYears(10) };
            var airStatuses = new AirStatuses[] { AirStatuses.UNKNOWN, AirStatuses.UNKNOWN, AirStatuses.UNKNOWN, AirStatuses.NotAired, AirStatuses.NotAired,
                AirStatuses.Airing, AirStatuses.Aired, AirStatuses.Aired, AirStatuses.Aired, AirStatuses.NotAired};
            var animeInfoIds = new long[] { 1, 1, 1, 1, 1,
                1, 1, 1, 2, 2 };
            var seasonIds = new long[] { 2, 2, 2, 2, 2,
                2, 2, 2, 5405, 6001 };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                allRequestModels.Add(new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: airStatuses[i], cover: covers[i], airDate: airDates[i],
                    animeInfoId: animeInfoIds[i], seasonId: seasonIds[i], title: titles[i], description: descriptions[i]));
            }
        }

        [TestInitialize]
        public void InitDb()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

            allAnimeInfos = new List<AnimeInfo> {
                new AnimeInfo { Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false, IsActive = true },
                new AnimeInfo { Id = 2, Title = "Kuroku no Basketball", Description = string.Empty, IsNsfw = false, IsActive = true }
            };
            allSeasons = new List<Season>
            {
                 new Season{ Id = 1, SeasonNumber = 1, Title = "Phantom Blood", Description = "In this season we know the story of Jonathan, Dio and Speedwagon, then Joseph and the Pillarmen's story",
                    StartDate = new DateTime(2012, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2012, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                new Season{ Id = 2, SeasonNumber = 2, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                new Season{ Id = 5401, SeasonNumber = 1, Title = "The Pillarmen's revenge", Description = "In this season the pillarmen are taking revenge for their death.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                new Season{ Id = 5405, SeasonNumber = 2, Title = "Life is basketball", Description = "We know the MC, who wants to get his revenge for kicking her out of the basketball team by making a new team.",
                    StartDate = today.AddYears(-10).AddMonths(-3), EndDate = today.AddYears(-9),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("Basketball Carousel"), Cover = Encoding.UTF8.GetBytes("Basketball Cover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                  new Season{ Id = 6001, SeasonNumber = 3, Title = "Monochrome", Description = "Mc sees everything in monochrome. Due to his illness, demons attack him.",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatuses.NotAired, NumberOfEpisodes = 10, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("Basketball Carousel"), Cover = Encoding.UTF8.GetBytes("Basketball Cover"),
                    IsAnimeInfoActive = true, IsActive = true
                }
            };
            allEpisodes = new List<Episode> {
                new Episode { Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Prologue", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                },
                new Episode { Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatuses.Aired, Title = "Beginning of something new", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                },
                new Episode { Id = 3, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Family relations", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                },
                new Episode { Id = 4, EpisodeNumber = 2, AirStatus = (int)AirStatuses.NotAired, Title = "Parasites", Description = "No one knows what it's like...",
                    AirDate =  null, Cover = Encoding.UTF8.GetBytes("S2Ep2Cover"), SeasonId = 2, AnimeInfoId = 6001,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                }
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId) };
            }
        }


        private static IEnumerable<object[]> GetInvalidEpisodeNumberData()
        {
            var epNumbers = new int[] { 0, -1, -3, -213 };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: erm.AirStatus, title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.EmptyProperty, nameof(EpisodeCreationRequestModel.EpisodeNumber) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAirStatusData()
        {
            var airStatuses = new AirStatuses[] { (AirStatuses)(-10), (AirStatuses)(-30), (AirStatuses)3, (AirStatuses)4, (AirStatuses)(-1) };
            for (var i = 0; i < airStatuses.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: airStatuses[i], title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.OutOfRangeProperty, nameof(EpisodeCreationRequestModel.AirStatus) };
            }
        }

        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            var titles = new string[] { new string('T', 256), new string('T', 300), new string('T', 355) };
            for (var i = 0; i < titles.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: titles[i], description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.TooLongProperty, nameof(EpisodeCreationRequestModel.Title) };
            }
        }

        private static IEnumerable<object[]> GetInvalidDescriptionData()
        {
            var descriptions = new string[] { new string('D', 30001), new string('T', 35000), new string('T', 40000) };
            for (var i = 0; i < descriptions.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: descriptions[i],
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.TooLongProperty, nameof(EpisodeCreationRequestModel.Description) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAirDateData()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var airStatuses = new AirStatuses[] {
                AirStatuses.NotAired,
                AirStatuses.Airing,
                AirStatuses.Airing,
                AirStatuses.Airing,
                AirStatuses.Aired,
                AirStatuses.Aired,
                AirStatuses.Aired,
                AirStatuses.Aired,
                AirStatuses.NotAired,
                AirStatuses.NotAired
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
            for (var i = 0; i < airStatuses.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: airStatuses[i], title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: airDates[i], animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), errorCodes[i], nameof(EpisodeCreationRequestModel.AirDate) };
            }
        }

        private static IEnumerable<object[]> GetInvalidCoverData()
        {
            var covers = new byte[][] { null, Encoding.UTF8.GetBytes(""), new byte[0] { } };
            for (var i = 0; i < covers.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description,
                    cover: covers[i], airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId), ErrorCodes.EmptyProperty, nameof(EpisodeCreationRequestModel.Cover) };
            }
        }

        private static IEnumerable<object[]> GetInvalidSeasonIdData()
        {
            var seasonIds = new long[] { 0, -1, -123 };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: seasonIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAnimeInfoIdData()
        {
            var animeInfoIds = new long[] { 0, -1, -123 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: animeInfoIds[i], seasonId: erm.SeasonId) };
            }
        }

        private static IEnumerable<object[]> GetMismatchingIdsData()
        {
            var seasonIds = new long[] { 1, 2, 5401 };
            var animeInfoIds = new long[] { 2, 3, 412 };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: erm.EpisodeNumber, airStatus: erm.AirStatus, title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: animeInfoIds[i], seasonId: seasonIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingEpisodeData()
        {
            var epNumbers = new int[] { 1, 2 };
            for (var i = 0; i < epNumbers.Length; i++)
            {
                var erm = allRequestModels[i];
                yield return new object[] { new EpisodeCreationRequestModel(episodeNumber: epNumbers[i], airStatus: erm.AirStatus, title: erm.Title, description: erm.Description,
                    cover: erm.Cover, airDate: erm.AirDate, animeInfoId: erm.AnimeInfoId, seasonId: erm.SeasonId) };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_ShouldWork(EpisodeCreationRequestModel requestModel)
        {
            var isExistWithSameEpNum = false;
            //var isExistAnimeInfoAndSeason = false;
            Episode savedEpisode = null;
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                //episodeReadRepo.Setup(er => er.IsSeasonAndAnimeInfoExistsAndReferences(It.IsAny<long>(), It.IsAny<long>()))
                //    .Callback<long, long>((sId, aiId) =>
                //        {
                //            var season = allSeasons.SingleOrDefault(s => s.Id == sId);
                //            if (season == null || season.AnimeInfoId != aiId) isExistAnimeInfoAndSeason = false;
                //            else
                //            {
                //                var animeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId);
                //                if (animeInfo == null) isExistAnimeInfoAndSeason = false;
                //                else isExistAnimeInfoAndSeason = true;
                //            }
                //        })
                //    .ReturnsAsync(() => isExistAnimeInfoAndSeason);
                episodeReadRepo.Setup(er => er.IsEpisodeWithEpisodeNumberExists(It.IsAny<long>(), It.IsAny<int>()))
                    .Callback<long, int>((sId, epNum) => isExistWithSameEpNum = allEpisodes.Any(e => e.SeasonId == sId && e.EpisodeNumber == epNum))
                    .Returns(() => isExistWithSameEpNum);
                episodeWriteRepo.Setup(ew => ew.CreateEpisode(It.IsAny<Episode>())).Callback<Episode>(e => { savedEpisode = e; savedEpisode.Id = 10; }).ReturnsAsync(() => savedEpisode!);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episode = requestModel.ToEpisode();
            episode.Id = 10;
            episode.IsAnimeInfoActive = true;
            episode.IsSeasonActive = true;
            episode.IsActive = true;
            var responseModel = episode.ToCreationResponseModel();
            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            var createdEpisode = await episodeCreationHandler.CreateEpisode(requestModel);
            createdEpisode.Should().BeEquivalentTo(responseModel);
        }


        [DataTestMethod,
            DynamicData(nameof(GetInvalidEpisodeNumberData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidEpisodeNumber_ThrowException(EpisodeCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            Season foundSeason = null;
            var sp = SetupDI(services =>
           {
               var episodeReadRepo = new Mock<IEpisodeRead>();
               var episodeWriteRepo = new Mock<IEpisodeWrite>();
               var seasonReadRepo = new Mock<ISeasonRead>();
               seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
               services.AddSingleton<IDateTime, DateTimeProvider>();
               services.AddTransient(factory => seasonReadRepo.Object);
               services.AddTransient(factory => episodeReadRepo.Object);
               services.AddTransient(factory => episodeWriteRepo.Object);
               services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
           });

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
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

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
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

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
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

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
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

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
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            var valEx = await createEpisodeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidSeasonIdData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidSeasonId_ThrowException(EpisodeCreationRequestModel requestModel)
        {
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            await createEpisodeFunc.Should().ThrowAsync<NotFoundObjectException<Season>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_InvalidAnimeInfoId_ThrowException(EpisodeCreationRequestModel requestModel)
        {
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            });

            var episodeCreationHandler = sp.GetService<IEpisodeCreation>();
            Func<Task> createEpisodeFunc = async () => await episodeCreationHandler.CreateEpisode(requestModel);
            await createEpisodeFunc.Should().ThrowAsync<MismatchingIdException>();
        }


        [DataTestMethod,
            DynamicData(nameof(GetMismatchingIdsData), DynamicDataSourceType.Method)]
        public async Task CreateEpisode_MismatchingIds_ThrowException(EpisodeCreationRequestModel requestModel)
        {
            //var isExistAnimeInfoAndSeason = false;
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                //episodeReadRepo.Setup(er => er.IsSeasonAndAnimeInfoExistsAndReferences(It.IsAny<long>(), It.IsAny<long>()))
                //    .Callback<long, long>((sId, aiId) =>
                //    {
                //        var season = allSeasons.SingleOrDefault(s => s.Id == sId);
                //        if (season == null || season.AnimeInfoId != aiId) isExistAnimeInfoAndSeason = false;
                //        else
                //        {
                //            var animeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId);
                //            if (animeInfo == null) isExistAnimeInfoAndSeason = false;
                //            else isExistAnimeInfoAndSeason = true;
                //        }
                //    })
                //    .ReturnsAsync(() => isExistAnimeInfoAndSeason);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
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
            //var isExistAnimeInfoAndSeason = false;
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                //episodeReadRepo.Setup(er => er.IsSeasonAndAnimeInfoExistsAndReferences(It.IsAny<long>(), It.IsAny<long>()))
                //    .Callback<long, long>((sId, aiId) =>
                //    {
                //        var season = allSeasons.SingleOrDefault(s => s.Id == sId);
                //        if (season == null || season.AnimeInfoId != aiId) isExistAnimeInfoAndSeason = false;
                //        else
                //        {
                //            var animeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId);
                //            if (animeInfo == null) isExistAnimeInfoAndSeason = false;
                //            else isExistAnimeInfoAndSeason = true;
                //        }
                //    })
                //    .ReturnsAsync(() => isExistAnimeInfoAndSeason);
                episodeReadRepo.Setup(er => er.IsEpisodeWithEpisodeNumberExists(It.IsAny<long>(), It.IsAny<int>()))
                    .Callback<long, int>((sId, epNum) => isExistWithSameEpNum = allEpisodes.Any(e => e.SeasonId == sId && e.EpisodeNumber == epNum))
                    .Returns(() => isExistWithSameEpNum);
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
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
                var seasonReadRepo = new Mock<ISeasonRead>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
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
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                services.AddSingleton<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => seasonReadRepo.Object);
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


        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
