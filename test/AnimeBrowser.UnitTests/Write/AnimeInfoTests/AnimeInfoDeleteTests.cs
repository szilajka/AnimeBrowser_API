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

namespace AnimeBrowser.UnitTests.Write.AnimeInfoTests
{
    [TestClass]
    public class AnimeInfoDeleteTests : TestBase
    {
        private IList<User> allUsers;
        private IList<Episode> allEpisodes;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<EpisodeRating> allEpisodeRatings;
        private IList<SeasonRating> allSeasonRatings;

        [TestInitialize]
        public void InitTests()
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

            allAnimeInfos = new List<AnimeInfo>
            {
                new AnimeInfo
                {
                    Id = 1, Title = "JoJo's Bizarre Adventure", IsNsfw = false, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 2, Title = "Made in Abyss",
                    Description = "A horrorific story about a girl, who lost his mom and goes to the Abyss to search for her. For her surprise, a robot boy accompanies her and they are looking for her mom together, while they discover terrific cretures of the abyss who tries to kill them and other people from expeditions. Well, those people are still not nice guys.",
                    IsNsfw = true, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 3, Title = "A Certain Specific Railgun",
                    Description = "Misaka is not an average school girl, she is the Railgun. In this anime we know more and more about the girl and her friends while the story of the city and the image of the world they are living are building up continually.",
                    IsNsfw = false, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 7, Title = "A Certain Magical Index",
                    Description = "The main character of the story meets with Index, a magician girl who was chased by some magician. As the story goes, the MC knows the girl's story and get familiar with the magician's world and what world he lives in. The story follows the MC guy and Index's adventures and sometimes their friends are get their showtime.",
                    IsNsfw = false, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 10, Title = "KonoSuba",
                    Description = "The main character guy dies and he's teleported into a medieval village with a goddes named Aqua. Their main quest is to defeat the Demon King and sometimes they get into trouble while other folks join to their party.",
                    IsNsfw = false, IsActive = true
                }
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
                new Season{ Id = 5405, SeasonNumber = 1, Title = "Into the Abyss", Description = "MC and her robot friend goes into the Abyss in hope to find her mother.",
                    StartDate = today.AddYears(-10).AddMonths(-3), EndDate = today.AddYears(-9),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("MadeInAbyss Carousel"), Cover = Encoding.UTF8.GetBytes("MadeInAbyss Cover"),
                },
                  new Season{ Id = 6001, SeasonNumber = 1, Title = "Season 1", Description = "Main characters are not so clever...",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatuses.NotAired, NumberOfEpisodes = 10, AnimeInfoId = 10,
                    CoverCarousel = Encoding.UTF8.GetBytes("KonoSuba Carousel"), Cover = Encoding.UTF8.GetBytes("KonoSuba Cover"),
                }
            };

            allEpisodes = new List<Episode> {
                new Episode { Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Prologue", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatuses.Aired, Title = "Beginning of something new", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 3, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Family relations", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 4, EpisodeNumber = 5, AirStatus = (int)AirStatuses.NotAired, Title = "Intro to the Other world", Description = "No one knows what it's like...",
                    AirDate =  null, Cover = Encoding.UTF8.GetBytes("S1Ep5Cover"), SeasonId = 1, AnimeInfoId = 10}
            };

            allSeasonRatings = new List<SeasonRating>
            {
                new SeasonRating { Id = 1, Rating = 4, Message = "", SeasonId = 1, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new SeasonRating { Id = 2, Rating = 3, Message = "Not bad.", SeasonId = 2, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new SeasonRating { Id = 3, Rating = 5, Message = "Cool anime!", SeasonId = 5401, UserId = "15A6B54C-98D0-4396-90E7-C94761DBA977" },
                new SeasonRating { Id = 4, Rating = 1, Message = "Very very bad....", SeasonId = 5405, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 5, Rating = 2, Message = "", SeasonId = 5401, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 6, Rating = 3, Message = "", SeasonId = 2, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 7, Rating = 4, Message = "", SeasonId = 1, UserId = "65F041D2-7217-4EA6-9065-9C9AB6290B35" },
                new SeasonRating { Id = 8, Rating = 2, Message = "", SeasonId = 5401, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
                new SeasonRating { Id = 9, Rating = 5, Message = "", SeasonId = 5405, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
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
                new EpisodeRating { Id = 9, Rating = 5, Message = "", EpisodeId = 4, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" }
            };
        }


        private static IEnumerable<object[]> GetBasicData()
        {
            var ids = new long[] { 1, 2, 3, 7 };
            var seasonIds = new long[][] { new long[] { 1, 2 }, new long[] { 5401, 5405 }, Array.Empty<long>(), Array.Empty<long>() };
            var episodeIds = new long[][] { new long[] { 1, 2, 3 }, new long[] { }, Array.Empty<long>(), Array.Empty<long>() };
            var seasonRatingIds = new long[][] { new long[] { 1, 2, 6, 7 }, new long[] { 3, 4, 5, 8, 9 }, Array.Empty<long>(), Array.Empty<long>() };
            var episodeRatingIds = new long[][] { new long[] { 1, 2, 3, 5, 6, 7, 8 }, Array.Empty<long>(), Array.Empty<long>(), Array.Empty<long>() };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i], seasonIds[i], episodeIds[i], seasonRatingIds[i], episodeRatingIds[i] };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoDelete_ShouldWork(long animeInfoId, long[] seasonIds, long[] episodeIds, long[] seasonRatingIds, long[] episodeRatingIds)
        {
            AnimeInfo foundAnimeInfo = null;
            IEnumerable<Season> foundSeasons = null;
            IEnumerable<Episode> foundEpisodes = null;
            IEnumerable<EpisodeRating> foundEpisodeRatings = null;
            IEnumerable<SeasonRating> foundSeasonRatings = null;
            var beforeAnimeInfoCount = allAnimeInfos.Count;
            var sp = SetupDI(services =>
            {
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aid => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aid)).ReturnsAsync(() => foundAnimeInfo);
                seasonReadRepo.Setup(sr => sr.GetSeasonsByAnimeInfoId(It.IsAny<long>())).Callback<long>(aiId => foundSeasons = allSeasons.Where(s => s.AnimeInfoId == aiId)).Returns(() => foundSeasons);
                episodeReadRepo.Setup(er => er.GetEpisodesBySeasonIds(It.IsAny<IEnumerable<long>>())).Callback<IEnumerable<long>>(sIds => foundEpisodes = allEpisodes.Where(e => sIds.Contains(e.SeasonId))).Returns(() => foundEpisodes);
                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingsByEpisodeIds(It.IsAny<IEnumerable<long>>())).Callback<IEnumerable<long>>(eIds => foundEpisodeRatings = allEpisodeRatings.Where(er => eIds.Contains(er.EpisodeId))).Returns(() => foundEpisodeRatings);
                seasonRatingReadRepo.Setup(srr => srr.GetSeasonRatingsBySeasonIds(It.IsAny<IEnumerable<long>>())).Callback<IEnumerable<long>>(sIds => foundSeasonRatings = allSeasonRatings.Where(sr => sIds.Contains(sr.SeasonId))).Returns(() => foundSeasonRatings);
                animeInfoWriteRepo.Setup(aiw => aiw.DeleteAnimeInfo(It.IsAny<AnimeInfo>(), It.IsAny<IEnumerable<Season>>(), It.IsAny<IEnumerable<Episode>>(), It.IsAny<IEnumerable<SeasonRating>>(), It.IsAny<IEnumerable<EpisodeRating>>()))
                    .Callback<AnimeInfo, IEnumerable<Season>, IEnumerable<Episode>, IEnumerable<SeasonRating>, IEnumerable<EpisodeRating>>((ai, seasons, episodes, srs, ers) =>
                    {
                        if (ers?.Any() == true)
                        {
                            foreach (var er in ers.ToList())
                            {
                                allEpisodeRatings.Remove(er);
                            }
                        }
                        if (srs?.Any() == true)
                        {
                            foreach (var sr in srs.ToList())
                            {
                                allSeasonRatings.Remove(sr);
                            }
                        }
                        if (episodes?.Any() == true)
                        {
                            foreach (var ep in episodes.ToList())
                            {
                                allEpisodes.Remove(ep);
                            }
                        }
                        if (seasons?.Any() == true)
                        {
                            foreach (var s in seasons.ToList())
                            {
                                allSeasons.Remove(s);
                            }
                        }
                        allAnimeInfos.Remove(ai);
                    });

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            });

            var animeInfoDeleteHandler = sp.GetService<IAnimeInfoDelete>();
            await animeInfoDeleteHandler!.DeleteAnimeInfo(animeInfoId);

            allAnimeInfos.Count.Should().Be(beforeAnimeInfoCount - 1);
            foundAnimeInfo.Should().NotBeNull();
            allAnimeInfos.Should().NotContain(foundAnimeInfo);
            allSeasons.Where(s => s.AnimeInfoId == animeInfoId).ToList().Should().HaveCount(0);
            allEpisodes.Where(e => seasonIds.Contains(e.SeasonId)).ToList().Should().HaveCount(0);
            allSeasonRatings.Where(sr => seasonIds.Contains(sr.SeasonId)).ToList().Should().HaveCount(0);
            allEpisodeRatings.Where(er => episodeIds.Contains(er.EpisodeId)).ToList().Should().HaveCount(0);
        }


        [DataTestMethod,
            DataRow(-1), DataRow(-10), DataRow(0), DataRow(null)]
        public async Task AnimeInfoDelete_InvalidId_ThrowException(long animeInfoId)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            });

            var animeInfoDeleteHandler = sp.GetService<IAnimeInfoDelete>();
            Func<Task> deleteAnimeInfoFunc = async () => await animeInfoDeleteHandler!.DeleteAnimeInfo(animeInfoId);
            await deleteAnimeInfoFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DataRow(251), DataRow(1034235234), DataRow(4), DataRow(11)]
        public async Task AnimeInfoDelete_NotExistingId_ThrowException(long animeInfoId)
        {
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aid => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aid)).ReturnsAsync(() => foundAnimeInfo);

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            });

            var animeInfoDeleteHandler = sp.GetService<IAnimeInfoDelete>();
            Func<Task> deleteAnimeInfoFunc = async () => await animeInfoDeleteHandler!.DeleteAnimeInfo(animeInfoId);
            await deleteAnimeInfoFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }

        [TestMethod]
        public async Task AnimeInfoDelete_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var seasonReadRepo = new Mock<ISeasonRead>();
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            });

            var animeInfoDeleteHandler = sp.GetService<IAnimeInfoDelete>();
            Func<Task> deleteAnimeInfoFunc = async () => await animeInfoDeleteHandler!.DeleteAnimeInfo(1);
            await deleteAnimeInfoFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanDb()
        {
            allEpisodeRatings.Clear();
            allUsers.Clear();
            allEpisodes.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allEpisodeRatings = null;
            allUsers = null;
            allEpisodes = null;
            allSeasons = null;
            allAnimeInfos = null;
        }
    }
}
