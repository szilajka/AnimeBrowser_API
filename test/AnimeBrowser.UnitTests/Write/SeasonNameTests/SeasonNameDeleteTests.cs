using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
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

namespace AnimeBrowser.UnitTests.Write.SeasonNameTests
{
    [TestClass]
    public class SeasonNameDeleteTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private IList<Season> allSeasons;
        private IList<SeasonName> allSeasonNames;

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
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 2, SeasonNumber = 2, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 3, SeasonNumber = 1, Title = "Season 1", Description = "I don't know this anime. Maybe they are just fighting. Who knows? I'm sure not.",
                    StartDate = new DateTime(2013, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 2, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 40, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("BnHACarousel"), Cover = Encoding.UTF8.GetBytes("BnHACover"),
                },
                new Season{Id = 10, SeasonNumber = 1, Title = "Season 1", Description = "Kayman and Nikkaido's story. Kayman is a man, but has a lizard body, well, some magician did it to him, but who?",
                    StartDate = new DateTime(2020, 9, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2020, 12, 20, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 10, AnimeInfoId = 10,
                    CoverCarousel = Encoding.UTF8.GetBytes("DorohedoroCarousel"), Cover = Encoding.UTF8.GetBytes("DorohedoroCover"),
                },
                new Season{Id = 20, SeasonNumber = 1, Title = "Season 4", Description = "I don't know this anime. Maybe they are just fighting. Who knows? I'm sure not.",
                    StartDate = new DateTime(2020, 11, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2021, 4, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Airing, NumberOfEpisodes = 24, AnimeInfoId = 15,
                    CoverCarousel = Encoding.UTF8.GetBytes("SnKCarousel"), Cover = Encoding.UTF8.GetBytes("SnKCover"),
                },
                new Season{Id = 22, SeasonNumber = 1, Title = "Season 2", Description = "I don't know this anime. But there will be a second season. That's for sure!",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatuses.NotAired, NumberOfEpisodes = 20, AnimeInfoId = 201,
                    CoverCarousel = Encoding.UTF8.GetBytes("YnKCarousel"), Cover = Encoding.UTF8.GetBytes("YnKCover"),
                }
            };

            allSeasonNames = new List<SeasonName> {
                new SeasonName { Id = 31, Title = "JoJo Part 1-2", SeasonId = 1 },
                new SeasonName { Id = 32, Title = "JoJo Part 3", SeasonId = 2 },
                new SeasonName { Id = 33, Title = "Boku no Heroes Season 1", SeasonId = 3 },
                new SeasonName { Id = 34, Title = "Dorohedoro - Kaiman's story", SeasonId = 10 },
                new SeasonName { Id = 35, Title = "Finale Season", SeasonId = 20 },
                new SeasonName { Id = 41, Title = "The Second Season", SeasonId = 22 },
                new SeasonName { Id = 42, Title = "JoJo Part 1-2 Anime", SeasonId = 1 }
            };
        }

        [DataTestMethod,
            DataRow(31), DataRow(32), DataRow(33), DataRow(42)]
        public async Task DeleteSeasonName_ShouldWork(long id)
        {
            SeasonName foundSeasonName = null;
            var sp = SetupDI(services =>
            {
                var seasonNameReadRepo = new Mock<ISeasonNameRead>();
                var seasonNameWriteRepo = new Mock<ISeasonNameWrite>();

                seasonNameReadRepo.Setup(snr => snr.GetSeasonNameById(It.IsAny<long>())).Callback<long>(snId => foundSeasonName = allSeasonNames.SingleOrDefault(sn => sn.Id == snId)).ReturnsAsync(() => foundSeasonName);
                seasonNameWriteRepo.Setup(snw => snw.DeleteSeasonName(It.IsAny<SeasonName>())).Callback<SeasonName>(sn => allSeasonNames.Remove(sn));

                services.AddTransient(factory => seasonNameReadRepo.Object);
                services.AddTransient(factory => seasonNameWriteRepo.Object);
                services.AddTransient<ISeasonNameDelete, SeasonNameDeleteHandler>();
            });

            var beforeCount = allSeasonNames.Count;
            var seasonNameDeleteHandler = sp.GetService<ISeasonNameDelete>();
            await seasonNameDeleteHandler!.DeleteSeasonName(id);
            allSeasonNames.Count.Should().Be(beforeCount - 1);
        }

        [DataTestMethod,
            DataRow(0), DataRow(-1), DataRow(-100)]
        public async Task DeleteSeasonName_InvalidId_ThrowException(long id)
        {
            var sp = SetupDI(services =>
            {
                var seasonNameReadRepo = new Mock<ISeasonNameRead>();
                var seasonNameWriteRepo = new Mock<ISeasonNameWrite>();
                services.AddTransient(factory => seasonNameReadRepo.Object);
                services.AddTransient(factory => seasonNameWriteRepo.Object);
                services.AddTransient<ISeasonNameDelete, SeasonNameDeleteHandler>();
            });

            var seasonNameDeleteHandler = sp.GetService<ISeasonNameDelete>();
            Func<Task> seasonNameDeleteFunc = async () => await seasonNameDeleteHandler!.DeleteSeasonName(id);
            await seasonNameDeleteFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DataRow(51), DataRow(101), DataRow(1)]
        public async Task DeleteSeasonName_NotExistingSeasonNameId_ThrowException(long id)
        {
            SeasonName foundSeasonName = null;
            var sp = SetupDI(services =>
            {
                var seasonNameReadRepo = new Mock<ISeasonNameRead>();
                var seasonNameWriteRepo = new Mock<ISeasonNameWrite>();

                seasonNameReadRepo.Setup(snr => snr.GetSeasonNameById(It.IsAny<long>())).Callback<long>(snId => foundSeasonName = allSeasonNames.SingleOrDefault(sn => sn.Id == snId)).ReturnsAsync(() => foundSeasonName);

                services.AddTransient(factory => seasonNameReadRepo.Object);
                services.AddTransient(factory => seasonNameWriteRepo.Object);
                services.AddTransient<ISeasonNameDelete, SeasonNameDeleteHandler>();
            });

            var seasonNameDeleteHandler = sp.GetService<ISeasonNameDelete>();
            Func<Task> seasonNameDeleteFunc = async () => await seasonNameDeleteHandler!.DeleteSeasonName(id);
            await seasonNameDeleteFunc.Should().ThrowAsync<NotFoundObjectException<SeasonName>>();
        }

        [TestMethod]
        public async Task DeleteSeasonName_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var seasonNameReadRepo = new Mock<ISeasonNameRead>();
                var seasonNameWriteRepo = new Mock<ISeasonNameWrite>();

                seasonNameReadRepo.Setup(snr => snr.GetSeasonNameById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => seasonNameReadRepo.Object);
                services.AddTransient(factory => seasonNameWriteRepo.Object);
                services.AddTransient<ISeasonNameDelete, SeasonNameDeleteHandler>();
            });

            var seasonNameDeleteHandler = sp.GetService<ISeasonNameDelete>();
            Func<Task> seasonNameDeleteFunc = async () => await seasonNameDeleteHandler!.DeleteSeasonName(1);
            await seasonNameDeleteFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanDb()
        {
            allSeasonNames.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allAnimeInfos = null;
            allSeasons = null;
            allSeasonNames = null;
        }
    }
}
