using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Data.Converters.SecondaryConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
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
    public class SeasonNameEditingTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private IList<Season> allSeasons;
        private IList<SeasonName> allSeasonNames;
        private static IList<SeasonNameEditingRequestModel> allRequestModels;


        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<SeasonNameEditingRequestModel>();

            var ids = new long[] { 31, 31, 32, 32, 33,
                34, 34, 35, 35, 41 };
            var titles = new string[] { "ファントムブラッド", "Fantomu Buraddo", "スターダストクルセイダース", "Sutādasuto Kuruseidāsu", "僕のヒーローアカデミア Season 1",
                "ドロヘドロ Season 1", "ドロヘドロ Kaiman", "Shingeki no Kyojin Season 4", "Shingeki no Kyojin Finale Season", "The Promised Neverland Season 2" };
            var seasonIds = new long[] { 1, 1, 2, 2, 3,
                10, 10, 20, 20, 22 };
            for (var i = 0; i < titles.Length; i++)
            {
                allRequestModels.Add(new SeasonNameEditingRequestModel(id: ids[i], title: titles[i], seasonId: seasonIds[i]));
            }
        }


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
                new Season{Id = 2, SeasonNumber = 2, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
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

            allSeasonNames = new List<SeasonName> {
                new SeasonName { Id = 31, Title = "JoJo Part 1-2", SeasonId = 1 },
                new SeasonName { Id = 32, Title = "JoJo Part 3", SeasonId = 2 },
                new SeasonName { Id = 33, Title = "Boku no Heroes Season 1", SeasonId = 3 },
                new SeasonName { Id = 34, Title = "Dorohedoro - Kaiman's story", SeasonId = 10 },
                new SeasonName { Id = 35, Title = "Finale Season", SeasonId = 20 },
                new SeasonName { Id = 41, Title = "The Second Season", SeasonId = 22 }
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var srm = allRequestModels[i];
                yield return new object[] { srm.Id, new SeasonNameEditingRequestModel(id: srm.Id, title: srm.Title, seasonId: srm.SeasonId) };
            }
        }



        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task EditSeasonName_ShouldWork(long id, SeasonNameEditingRequestModel requestModel)
        {
            SeasonName foundSeasonName = null;
            Season foundSeason = null;
            bool isExistsWithSameTitle = false;
            SeasonName updatedSeasonName = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonNameReadRepo = new Mock<ISeasonNameRead>();
                var seasonNameWriteRepo = new Mock<ISeasonNameWrite>();

                seasonNameReadRepo.Setup(snr => snr.GetSeasonNameById(It.IsAny<long>())).Callback<long>(snId => foundSeasonName = allSeasonNames.SingleOrDefault(sn => sn.Id == snId)).ReturnsAsync(() => foundSeasonName);
                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonNameReadRepo.Setup(snr => snr.IsExistsWithSameTitle(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long>()))
                    .Callback<long, string, long>((snId, snTitle, snSeasonId) => isExistsWithSameTitle = allSeasonNames.Any(sn => sn.Id != snId && sn.SeasonId == snSeasonId && sn.Title.Equals(snTitle, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => isExistsWithSameTitle);
                seasonNameWriteRepo.Setup(snw => snw.UpdateSeasonName(It.IsAny<SeasonName>())).Callback<SeasonName>(sn => { var fsn = allSeasonNames.Single(s => s.Id == sn.Id); allSeasonNames.Remove(fsn); updatedSeasonName = sn; allSeasonNames.Add(sn); }).ReturnsAsync(() => updatedSeasonName!);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonNameReadRepo.Object);
                services.AddTransient(factory => seasonNameWriteRepo.Object);
                services.AddTransient<ISeasonNameEditing, SeasonNameEditingHandler>();
            });

            var seasonName = requestModel.ToSeasonName();
            var responseModel = seasonName.ToEditingResponseModel();
            var seasonNameEditingHandler = sp.GetService<ISeasonNameEditing>();
            var updatedResponseModel = await seasonNameEditingHandler!.EditSeasonName(id, requestModel);
            updatedResponseModel.Should().NotBeNull();
            updatedResponseModel.Should().BeEquivalentTo(responseModel);
            allSeasonNames.Should().ContainEquivalentOf(seasonName);

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

        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
