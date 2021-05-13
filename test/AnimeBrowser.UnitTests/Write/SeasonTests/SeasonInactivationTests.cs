using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Services.Write.MainHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Entities.Identity;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
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
    public class SeasonInactivationTests : TestBase
    {
        private IList<User> allUsers;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<Season> allSeasons;
        private IList<SeasonRating> allSeasonRatings;
        private IList<Episode> allEpisodes;
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

            allAnimeInfos = new List<AnimeInfo> {
                new AnimeInfo { Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false, IsActive = true },
                new AnimeInfo { Id = 2, Title = "Kuroku no Basketball", Description = string.Empty, IsNsfw = false, IsActive = true }
            };

            allSeasons = new List<Season> {
                new Season {
                    Id = 1, SeasonNumber = 1, Title = "Phantom Blood", Description = "...",
                    StartDate = new DateTime(2012, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2012, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                new Season {
                    Id = 2, SeasonNumber = 2, Title = "Stardust Crusaders", Description = "...",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                new Season {
                    Id = 3, SeasonNumber = 3, Title = "Diamond is Unbreakable", Description = "...",
                    StartDate = new DateTime(2016, 4, 2, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2016, 12, 24, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                new Season {
                    Id = 4, SeasonNumber = 1, Title = "Season 1", Description = "...",
                    StartDate = new DateTime(2018, 11, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2019, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("KurokunoBasketball"), Cover = Encoding.UTF8.GetBytes("BBCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
                new Season {
                    Id = 5, SeasonNumber = 2, Title = "Season 2", Description = "...",
                    StartDate = new DateTime(2019, 8, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2019, 12, 30, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("KurokunoBasketball"), Cover = Encoding.UTF8.GetBytes("BBCover"),
                    IsAnimeInfoActive = true, IsActive = true
                },
            };

            allSeasonRatings = new List<SeasonRating>
            {
                new SeasonRating { Id = 1, SeasonId = 1, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977", Rating = 5, Message = "Very good season! (Y)", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 2, SeasonId = 2, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977", Rating = 5, Message = "The first season was good, it is really better! (Y)", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 3, SeasonId = 3, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977", Rating = 4, Message = "Good season too, but not as good as the first two", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 4, SeasonId = 1, UserId = "5879560D-65C5-4699-9449-86CC57EF3111", Rating = 4, Message = "Good season!", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 5, SeasonId = 4, UserId = "5879560D-65C5-4699-9449-86CC57EF3111", Rating = 4, Message = "Not a sport anime fan, but it was really good.", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 6, SeasonId = 5, UserId = "D7623518-D2C2-4E71-9A9B-C825CE9A44B9", Rating = 4, Message = "A good sequel.", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 7, SeasonId = 1, UserId = "D7623518-D2C2-4E71-9A9B-C825CE9A44B9", Rating = 2, Message = "Good starting, but bad ending.", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 8, SeasonId = 2, UserId = "D7623518-D2C2-4E71-9A9B-C825CE9A44B9", Rating = 4, Message = "Finally, a worthy anime.", IsAnimeInfoActive = true, IsSeasonActive = true },
                new SeasonRating { Id = 9, SeasonId = 5, UserId = "027AAEC6-ED12-420B-9467-1984D4396971", Rating = 4, Message = "Not bad.", IsAnimeInfoActive = true, IsSeasonActive = true }
            };

            allEpisodes = new List<Episode> {
                new Episode {
                    Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Pilot", Description = "...",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                },
                new Episode {
                    Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatuses.Aired, Title = "A new beginning", Description = "...",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                },
                new Episode {
                    Id = 3, EpisodeNumber = 3, AirStatus = (int)AirStatuses.Aired, Title = "Secret of the mask", Description = "...",
                    AirDate =  new DateTime(2012, 1, 15, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep3Cover"), SeasonId = 1, AnimeInfoId = 1,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                },
                new Episode {
                    Id = 4, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "The beginning", Description = "...",
                    AirDate =  new DateTime(2018, 11, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 4, AnimeInfoId = 2,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                },
                new Episode {
                    Id = 5, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "A new basketball", Description = "...",
                    AirDate =  new DateTime(2019, 8, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 5, AnimeInfoId = 2,
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsActive = true
                }
            };

            allEpisodeRatings = new List<EpisodeRating> {
                new EpisodeRating {
                    Id = 1, EpisodeId = 1, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977", Rating = 4, Message = "Very good opening for a season",
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsEpisodeActive = true
                },
                new EpisodeRating {
                    Id = 2, EpisodeId = 1, UserId = "5879560D-65C5-4699-9449-86CC57EF3111", Rating = 5, Message = "A terrifying episode, how long have I waited for an episode like this <3",
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsEpisodeActive = true
                },
                new EpisodeRating {
                    Id = 3, EpisodeId = 3, UserId = "F6560F7D-08B5-402D-90EC-C701952A0CF2", Rating = 4, Message = "Seems good, hope that it keeps the fun and horror elements",
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsEpisodeActive = true
                },
                new EpisodeRating {
                    Id = 4, EpisodeId = 4, UserId = "F6560F7D-08B5-402D-90EC-C701952A0CF2", Rating = 4, Message = "Very good sport anime",
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsEpisodeActive = true
                },
                new EpisodeRating {
                    Id = 5, EpisodeId = 5, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977", Rating = 2, Message = "Not the best opening for a new season...",
                    IsAnimeInfoActive = true, IsSeasonActive = true, IsEpisodeActive = true
                },
            };
        }

        [DataTestMethod,
            DataRow(1), DataRow(2), DataRow(3), DataRow(4), DataRow(5)]
        public async Task InactivateSeason_ShouldWork(long seasonId)
        {
            Season foundSeason = null;
            IEnumerable<SeasonRating> foundSeasonRatings = null;
            IEnumerable<Episode> foundEpisodes = null;
            IEnumerable<EpisodeRating> foundEpisodeRatings = null;

            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                seasonRatingReadRepo.Setup(srr => srr.GetSeasonRatingsBySeasonId(It.IsAny<long>())).Callback<long>(sId => foundSeasonRatings = allSeasonRatings.Where(sr => sr.SeasonId == sId)).Returns(() => foundSeasonRatings);
                episodeReadRepo.Setup(er => er.GetEpisodesBySeasonId(It.IsAny<long>())).Callback<long>(sId => foundEpisodes = allEpisodes.Where(e => e.SeasonId == sId)).Returns(() => foundEpisodes);
                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingsByEpisodeIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(episodeIds =>
                    {
                        if (episodeIds?.Any() == true)
                        {
                            foundEpisodeRatings = allEpisodeRatings.Where(er => episodeIds.Contains(er.EpisodeId));
                        }
                    }).Returns(() => foundEpisodeRatings);
                seasonWriteRepo.Setup(sw => sw.UpdateSeasonActiveStatus(It.IsAny<Season>(), It.IsAny<IEnumerable<SeasonRating>>(), It.IsAny<IEnumerable<Episode>>(), It.IsAny<IEnumerable<EpisodeRating>>()))
                    .Callback<Season, IEnumerable<SeasonRating>, IEnumerable<Episode>, IEnumerable<EpisodeRating>>((s, sr, e, er) =>
                    {
                        foundSeason = s;
                        foundSeasonRatings = sr;
                        foundEpisodes = e;
                        foundEpisodeRatings = er;
                    });

                services.AddTransient(_ => seasonReadRepo.Object);
                services.AddTransient(_ => seasonRatingReadRepo.Object);
                services.AddTransient(_ => episodeReadRepo.Object);
                services.AddTransient(_ => episodeRatingReadRepo.Object);
                services.AddTransient(_ => seasonWriteRepo.Object);
                services.AddTransient<ISeasonInactivation, SeasonInactivationHandler>();
            });

            var seasonInactivationHandler = sp.GetService<ISeasonInactivation>();
            var season = await seasonInactivationHandler!.Inactivate(seasonId);
            season.Should().NotBeNull();
            season.IsActive.Should().BeFalse();
            season.Should().BeEquivalentTo(foundSeason);
            if (foundSeasonRatings?.Any() == true)
            {
                foundSeasonRatings.Should().Match(seasonRatings => seasonRatings.All(sr => sr.IsSeasonActive == false));
            }
            if (foundEpisodeRatings?.Any() == true)
            {
                foundEpisodeRatings.Should().Match(episodeRatings => episodeRatings.All(er => er.IsSeasonActive == false));
            }
            if (foundEpisodes?.Any() == true)
            {
                foundEpisodes.Should().Match(episodes => episodes.All(e => e.IsSeasonActive == false));
            }
        }

        [DataTestMethod,
            DataRow(0), DataRow(-1), DataRow(-5), DataRow(-1000)]
        public async Task InactivateSeason_NotExistingId_ThrowException(long seasonId)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();

                services.AddTransient(_ => seasonReadRepo.Object);
                services.AddTransient(_ => seasonRatingReadRepo.Object);
                services.AddTransient(_ => episodeReadRepo.Object);
                services.AddTransient(_ => episodeRatingReadRepo.Object);
                services.AddTransient(_ => seasonWriteRepo.Object);
                services.AddTransient<ISeasonInactivation, SeasonInactivationHandler>();
            });

            var seasonInactivationHandler = sp.GetService<ISeasonInactivation>();
            Func<Task> inactivateSeasonFunc = async () => await seasonInactivationHandler!.Inactivate(seasonId);
            await inactivateSeasonFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DataRow(67), DataRow(10), DataRow(213123), DataRow(111)]
        public async Task InactivateSeason_NotExistingEpisode_ThrowException(long seasonId)
        {
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);

                services.AddTransient(_ => seasonReadRepo.Object);
                services.AddTransient(_ => seasonRatingReadRepo.Object);
                services.AddTransient(_ => episodeReadRepo.Object);
                services.AddTransient(_ => episodeRatingReadRepo.Object);
                services.AddTransient(_ => seasonWriteRepo.Object);
                services.AddTransient<ISeasonInactivation, SeasonInactivationHandler>();
            });

            var seasonInactivationHandler = sp.GetService<ISeasonInactivation>();
            Func<Task> inactivateSeasonFunc = async () => await seasonInactivationHandler!.Inactivate(seasonId);
            await inactivateSeasonFunc.Should().ThrowAsync<NotFoundObjectException<Season>>();
        }

        [DataTestMethod,
            DataRow(1), DataRow(2)]
        public async Task InactivateSeason_ThrowException(long seasonId)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonWriteRepo = new Mock<ISeasonWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(_ => seasonReadRepo.Object);
                services.AddTransient(_ => seasonRatingReadRepo.Object);
                services.AddTransient(_ => episodeReadRepo.Object);
                services.AddTransient(_ => episodeRatingReadRepo.Object);
                services.AddTransient(_ => seasonWriteRepo.Object);
                services.AddTransient<ISeasonInactivation, SeasonInactivationHandler>();
            });

            var seasonInactivationHandler = sp.GetService<ISeasonInactivation>();
            Func<Task> inactivateSeasonFunc = async () => await seasonInactivationHandler!.Inactivate(seasonId);
            await inactivateSeasonFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanDb()
        {
            allEpisodeRatings.Clear();
            allEpisodes.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allUsers.Clear();
            allEpisodeRatings = null;
            allEpisodes = null;
            allSeasons = null;
            allAnimeInfos = null;
            allUsers = null;
        }
    }
}
