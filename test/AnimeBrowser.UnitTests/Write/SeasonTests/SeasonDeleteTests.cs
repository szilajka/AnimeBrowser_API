using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Services.Write.MainHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Entities.Identity;
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

namespace AnimeBrowser.UnitTests.Write.SeasonTests
{
    [TestClass]
    public class SeasonDeleteTests : TestBase
    {
        private IList<User> allUsers;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<Season> allSeasons;
        private IList<Episode> allEpisodes;
        private IList<SeasonRating> allSeasonRatings;
        private IList<EpisodeRating> allEpisodeRatings;

        [TestInitialize]
        public void InitDb()
        {
            allUsers = new List<User> {
                new User { Id = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new User { Id = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new User { Id = "5879560D-65C5-4699-9449-86CC57EF3111" },
                new User { Id = "817AB8E7-CE92-4D45-A93E-31A5D17430A9" },
                new User { Id = "F6560F7D-08B5-402D-90EC-C701952A0CF2" },
                new User { Id = "D7623518-D2C2-4E71-9A9B-C825CE9A44B9" },
                new User { Id = "60697390-85E4-451E-82F6-CB3C13B32B18" },
                new User { Id = "027AAEC6-ED12-420B-9467-1984D4396971" }
            };

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

            allSeasonRatings = new List<SeasonRating>
            {
                new SeasonRating { Id = 1, Rating = 4, Message = "", SeasonId = 1, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new SeasonRating { Id = 2, Rating = 3, Message = "Not bad.", SeasonId = 2, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new SeasonRating { Id = 3, Rating = 5, Message = "Cool anime!", SeasonId = 3, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new SeasonRating { Id = 4, Rating = 1, Message = "Very very bad....", SeasonId = 1, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 5, Rating = 2, Message = "", SeasonId = 10, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 6, Rating = 3, Message = "", SeasonId = 20, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 7, Rating = 4, Message = "", SeasonId = 22, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 8, Rating = 2, Message = "", SeasonId = 2, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
                new SeasonRating { Id = 9, Rating = 5, Message = "", SeasonId = 3, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
            };

            allEpisodes = new List<Episode> {
                new Episode { Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Prologue", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatuses.Aired, Title = "Beginning of something new", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 3, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Family relations", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 4, EpisodeNumber = 2, AirStatus = (int)AirStatuses.Aired, Title = "S2 Episode 2", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2014, 3, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 5, EpisodeNumber = 3, AirStatus = (int)AirStatuses.Aired, Title = "S2 Episode 3", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2014, 3, 15, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 6, EpisodeNumber = 4, AirStatus = (int)AirStatuses.Aired, Title = "S2 Episode 4", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 21, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 7, EpisodeNumber = 5, AirStatus = (int)AirStatuses.Aired, Title = "S2 Episode 5", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2014, 3, 28, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 8, EpisodeNumber = 6, AirStatus = (int)AirStatuses.Aired, Title = "S2 Episode 6", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 4, 5, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1}
            };

            allEpisodeRatings = new List<EpisodeRating> {
                new EpisodeRating { Id = 1, Rating = 4, Message = "", EpisodeId = 1, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new EpisodeRating { Id = 2, Rating = 3, Message = "Not bad.", EpisodeId = 2, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new EpisodeRating { Id = 3, Rating = 5, Message = "Cool anime!", EpisodeId = 3, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new EpisodeRating { Id = 4, Rating = 1, Message = "Very very bad....", EpisodeId = 4, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" }
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
                seasonWriteRepo.Setup(sw => sw.DeleteSeason(It.IsAny<Season>(), It.IsAny<IEnumerable<Episode>>(), It.IsAny<IEnumerable<SeasonRating>>(), It.IsAny<IEnumerable<EpisodeRating>>()))
                    .Callback<Season, IEnumerable<Episode>, IEnumerable<SeasonRating>, IEnumerable<EpisodeRating>>((s, e, sr, er) =>
                    {
                        if (er?.Any() == true)
                        {
                            foreach (var epRating in er)
                            {
                                allEpisodeRatings.Remove(epRating);
                            }
                        }
                        if (e?.Any() == true)
                        {
                            foreach (var ep in e)
                            {
                                allEpisodes.Remove(ep);
                            }
                        }
                        if (sr?.Any() == true)
                        {
                            foreach (var sRating in sr)
                            {
                                allSeasonRatings.Remove(sRating);
                            }
                        }
                        allSeasons.Remove(s);
                    });
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            });

            var beforeSeasonsCount = allSeasons.Count;
            var seasonDeleteHandler = sp.GetService<ISeasonDelete>();
            await seasonDeleteHandler!.DeleteSeason(seasonId);

            allSeasons.Count.Should().Be(beforeSeasonsCount - 1);
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
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            });

            var seasonDeleteHandler = sp.GetService<ISeasonDelete>();
            Func<Task> deleteSeasonFunc = async () => await seasonDeleteHandler!.DeleteSeason(seasonId);

            await deleteSeasonFunc.Should().ThrowAsync<NotFoundObjectException<Season>>();
        }

        [DataTestMethod,
        DataRow(0), DataRow(-1), DataRow(-31), DataRow(-1040)]
        public async Task DeleteSeason_InvalidId_ThrowException(long seasonId)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            });

            var seasonDeleteHandler = sp.GetService<ISeasonDelete>();
            Func<Task> deleteSeasonFunc = async () => await seasonDeleteHandler!.DeleteSeason(seasonId);

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
            Func<Task> deleteSeasonFunc = async () => await seasonDeleteHandler!.DeleteSeason(seasonId);

            await deleteSeasonFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanDb()
        {
            allAnimeInfos.Clear();
            allEpisodeRatings.Clear();
            allEpisodes.Clear();
            allSeasonRatings.Clear();
            allSeasons.Clear();
            allUsers.Clear();

            allAnimeInfos = null;
            allEpisodeRatings = null;
            allEpisodes = null;
            allSeasonRatings = null;
            allSeasons = null;
            allUsers = null;
        }
    }
}
