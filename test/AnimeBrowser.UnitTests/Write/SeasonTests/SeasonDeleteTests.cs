using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
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
    public class SeasonDeleteTests : TestBase
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

        [DataTestMethod,
            DataRow(1), DataRow(2), DataRow(3), DataRow(10), DataRow(20), DataRow(22)]
        public async Task DeleteSeason_ShouldWork(long seasonId)
        {
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.DeleteSeason(It.IsAny<Season>())).Callback<Season>(s => allSeasons.Remove(s));
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            });

            var beforeSeasonsCount = allSeasons.Count;
            var seasonDeleteHandler = sp.GetService<ISeasonDelete>();
            await seasonDeleteHandler.DeleteSeason(seasonId);
            var afterSeasonsCount = allSeasons.Count;

            beforeSeasonsCount.Should().Be(afterSeasonsCount + 1);
        }


        [DataTestMethod,
            DataRow(5), DataRow(100), DataRow(231)]
        public async Task DeleteSeason_NotExistingId_ThrowException(long seasonId)
        {
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.DeleteSeason(It.IsAny<Season>())).Callback<Season>(s => allSeasons.Remove(s));
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            });

            var seasonDeleteHandler = sp.GetService<ISeasonDelete>();
            Func<Task> deleteSeasonFunc = async () => await seasonDeleteHandler.DeleteSeason(seasonId);

            await deleteSeasonFunc.Should().ThrowAsync<NotFoundObjectException<Season>>();
        }

        [DataTestMethod,
        DataRow(0), DataRow(-1), DataRow(-31), DataRow(-1040)]
        public async Task DeleteSeason_InvalidId_ThrowException(long seasonId)
        {
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonWriteRepo.Setup(sw => sw.DeleteSeason(It.IsAny<Season>())).Callback<Season>(s => allSeasons.Remove(s));
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            });

            var seasonDeleteHandler = sp.GetService<ISeasonDelete>();
            Func<Task> deleteSeasonFunc = async () => await seasonDeleteHandler.DeleteSeason(seasonId);

            await deleteSeasonFunc.Should().ThrowAsync<NotExistingIdException>();
        }


        [TestMethod]
        public async Task DeleteSeason_ThrowException()
        {
            long seasonId = 1;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            });

            var seasonDeleteHandler = sp.GetService<ISeasonDelete>();
            Func<Task> deleteSeasonFunc = async () => await seasonDeleteHandler.DeleteSeason(seasonId);

            await deleteSeasonFunc.Should().ThrowAsync<InvalidOperationException>();
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
