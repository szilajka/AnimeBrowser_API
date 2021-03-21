using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Entities.Identity;
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

namespace AnimeBrowser.UnitTests.Write.EpisodeRatingTests
{
    [TestClass]
    public class EpisodeRatingDeleteTests : TestBase
    {
        private IList<User> allUsers;
        private IList<Episode> allEpisodes;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<EpisodeRating> allEpisodeRatings;

        [TestInitialize]
        public void InitDb()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

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
                 new Season{ Id = 1, SeasonNumber = 1, Title = "Phantom Blood", Description = "In this season we know the story of Jonathan, Dio and Speedwagon, then Joseph and the Pillarmen's story",
                    StartDate = new DateTime(2012, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2012, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 2, SeasonNumber = 1, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 5401, SeasonNumber = 1, Title = "The Pillarmen's revenge", Description = "In this season the pillarmen are taking revenge for their death.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 5405, SeasonNumber = 1, Title = "Life is basketball", Description = "We know the MC, who wants to get his revenge for kicking her out of the basketball team by making a new team.",
                    StartDate = today.AddYears(-10).AddMonths(-3), EndDate = today.AddYears(-9),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("Basketball Carousel"), Cover = Encoding.UTF8.GetBytes("Basketball Cover"),
                },
                  new Season{ Id = 6001, SeasonNumber = 1, Title = "Monochrome", Description = "Mc sees everything in monochrome. Due to his illness, demons attack him.",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatuses.NotAired, NumberOfEpisodes = 10, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("Basketball Carousel"), Cover = Encoding.UTF8.GetBytes("Basketball Cover"),
                }
            };

            allEpisodes = new List<Episode>
            {
                new Episode { Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Prologue", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatuses.Aired, Title = "Beginning of something new", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 3, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Family relations", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 4, EpisodeNumber = 2, AirStatus = (int)AirStatuses.NotAired, Title = "Parasites", Description = "No one knows what it's like...",
                    AirDate =  null, Cover = Encoding.UTF8.GetBytes("S2Ep2Cover"), SeasonId = 2, AnimeInfoId = 6001}
            };

            allEpisodeRatings = new List<EpisodeRating>
            {
                new EpisodeRating { Id = 1, Rating = 4, Message = "", EpisodeId = 1, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new EpisodeRating { Id = 2, Rating = 3, Message = "Not bad.", EpisodeId = 2, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new EpisodeRating { Id = 3, Rating = 5, Message = "Cool anime!", EpisodeId = 3, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new EpisodeRating { Id = 4, Rating = 1, Message = "Very very bad....", EpisodeId = 4, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new EpisodeRating { Id = 5, Rating = 2, Message = "", EpisodeId = 3, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new EpisodeRating { Id = 6, Rating = 3, Message = "", EpisodeId = 2, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new EpisodeRating { Id = 7, Rating = 4, Message = "", EpisodeId = 1, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new EpisodeRating { Id = 8, Rating = 2, Message = "", EpisodeId = 3, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
                new EpisodeRating { Id = 9, Rating = 5, Message = "", EpisodeId = 4, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            var ids = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i] };
            }
        }

        private static IEnumerable<object[]> GetInvalidIdData()
        {
            var ids = new long[] { 0, -1, -10, -100 };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i] };
            }
        }

        private static IEnumerable<object[]> GetNotExistingIdData()
        {
            var ids = new long[] { 15, 30, 200 };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i] };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task DeleteEpisodeRating_ShouldWork(long id)
        {
            EpisodeRating foundEpisodeRating = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);
                episodeRatingWriteRepo.Setup(erw => erw.DeleteEpisodeRating(It.IsAny<EpisodeRating>())).Callback<EpisodeRating>(er => allEpisodeRatings.Remove(er));

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingDelete, EpisodeRatingDeleteHandler>();
            });

            var beforeCount = allEpisodeRatings.Count;
            var episodeRatingDeleteHandler = sp.GetService<IEpisodeRatingDelete>();
            await episodeRatingDeleteHandler!.DeleteEpisodeRating(id);
            allEpisodeRatings.Count.Should().BeLessThan(beforeCount);
            allEpisodeRatings.Should().NotContain(foundEpisodeRating);
        }

        [DataTestMethod,
           DynamicData(nameof(GetInvalidIdData), DynamicDataSourceType.Method)]
        public async Task DeleteEpisodeRating_InvalidId_ThrowException(long id)
        {
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingDelete, EpisodeRatingDeleteHandler>();
            });

            var episodeRatingDeleteHandler = sp.GetService<IEpisodeRatingDelete>();
            Func<Task> deleteEpisodeRatingFunc = async () => await episodeRatingDeleteHandler!.DeleteEpisodeRating(id);
            await deleteEpisodeRatingFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingIdData), DynamicDataSourceType.Method)]
        public async Task DeleteEpisodeRating_NotExistingId_ThrowException(long id)
        {
            EpisodeRating foundEpisodeRating = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingDelete, EpisodeRatingDeleteHandler>();
            });

            var episodeRatingDeleteHandler = sp.GetService<IEpisodeRatingDelete>();
            Func<Task> deleteEpisodeRatingFunc = async () => await episodeRatingDeleteHandler!.DeleteEpisodeRating(id);
            await deleteEpisodeRatingFunc.Should().ThrowAsync<NotFoundObjectException<EpisodeRating>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task DeleteEpisodeRating_ThrowException(long id)
        {
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingDelete, EpisodeRatingDeleteHandler>();
            });

            var episodeRatingDeleteHandler = sp.GetService<IEpisodeRatingDelete>();
            Func<Task> deleteEpisodeRatingFunc = async () => await episodeRatingDeleteHandler!.DeleteEpisodeRating(id);
            await deleteEpisodeRatingFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanDb()
        {
            allEpisodeRatings.Clear();
            allEpisodes.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allUsers.Clear();
            allUsers = null;
            allEpisodes = null;
            allSeasons = null;
            allAnimeInfos = null;
            allEpisodeRatings = null;
        }
    }
}
