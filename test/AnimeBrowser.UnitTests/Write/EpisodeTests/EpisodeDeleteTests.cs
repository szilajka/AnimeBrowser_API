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

namespace AnimeBrowser.UnitTests.Write.EpisodeTests
{
    [TestClass]
    public class EpisodeDeleteTests : TestBase
    {
        private IList<User> allUsers;
        private IList<Episode> allEpisodes;
        private IList<EpisodeRating> allEpisodeRatings;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;

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

            allAnimeInfos = new List<AnimeInfo> {
                new AnimeInfo { Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false },
                new AnimeInfo { Id = 2, Title = "Kuroku no Basketball", Description = string.Empty, IsNsfw = false }
            };
            allSeasons = new List<Season>
            {
                 new Season{Id = 1, SeasonNumber = 1, Title = "Phantom Blood", Description = "In this season we know the story of Jonathan, Dio and Speedwagon, then Joseph and the Pillarmen's story",
                    StartDate = new DateTime(2012, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2012, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 2, SeasonNumber = 1, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 5401, SeasonNumber = 1, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 412,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
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
            DataRow(1), DataRow(2), DataRow(3), DataRow(4)]
        public async Task DeleteEpisode_ShouldWork(long episodeId)
        {
            Episode foundEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                episodeWriteRepo.Setup(ew => ew.DeleteEpisode(It.IsAny<Episode>(), It.IsAny<IEnumerable<EpisodeRating>>())).Callback<Episode, IEnumerable<EpisodeRating>>((e, er) =>
                {
                    if (er?.Any() == true)
                    {
                        foreach (var epRating in er)
                        {
                            allEpisodeRatings.Remove(epRating);
                        }
                    }
                    allEpisodes.Remove(e);
                });

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeDelete, EpisodeDeleteHandler>();
            });

            var beforeEpisodeCount = allEpisodes.Count;
            var episodeDeleteHandler = sp.GetService<IEpisodeDelete>();
            await episodeDeleteHandler!.DeleteEpisode(episodeId);
            foundEpisode.Should().NotBeNull();
            allEpisodes.Should().NotContain(foundEpisode);
            allEpisodes.Count.Should().Be(beforeEpisodeCount - 1);
        }


        [DataTestMethod,
            DataRow(0), DataRow(-1), DataRow(-100), DataRow(-4040)]
        public async Task DeleteEpisode_InvalidId_ThrowException(long episodeId)
        {
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeDelete, EpisodeDeleteHandler>();
            });

            var episodeDeleteHandler = sp.GetService<IEpisodeDelete>();
            Func<Task> episodeDeleteFunc = async () => await episodeDeleteHandler!.DeleteEpisode(episodeId);
            await episodeDeleteFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DataRow(123), DataRow(9), DataRow(300)]
        public async Task DeleteEpisode_NotExistingId_ThrowException(long episodeId)
        {
            Episode foundEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeDelete, EpisodeDeleteHandler>();
            });

            var episodeDeleteHandler = sp.GetService<IEpisodeDelete>();
            Func<Task> episodeDeleteFunc = async () => await episodeDeleteHandler!.DeleteEpisode(episodeId);
            await episodeDeleteFunc.Should().ThrowAsync<NotFoundObjectException<Episode>>();
        }

        [TestMethod]
        public async Task DeleteEpisode_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeWriteRepo = new Mock<IEpisodeWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => episodeWriteRepo.Object);
                services.AddTransient<IEpisodeDelete, EpisodeDeleteHandler>();
            });

            var episodeDeleteHandler = sp.GetService<IEpisodeDelete>();
            Func<Task> episodeDeleteFunc = async () => await episodeDeleteHandler!.DeleteEpisode(1);
            await episodeDeleteFunc.Should().ThrowAsync<InvalidOperationException>();
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
