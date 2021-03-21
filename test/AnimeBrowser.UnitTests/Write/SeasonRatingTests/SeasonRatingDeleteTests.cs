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

namespace AnimeBrowser.UnitTests.Write.SeasonRatingTests
{
    [TestClass]
    public class SeasonRatingDeleteTests : TestBase
    {
        private IList<User> allUsers;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<SeasonRating> allSeasonRatings;

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
                // For some reason, when episode/season was deleted, it was not deleted and still here
                new SeasonRating { Id = 912, Rating = 2, Message = "", SeasonId = 912, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
                new SeasonRating { Id = 913, Rating = 5, Message = "", SeasonId = 913, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
                // For some reason, when user was deleted, it was not deleted and still here
                new SeasonRating { Id = 916, Rating = 2, Message = "", SeasonId = 2, UserId = "CF596B99-A0CC-463B-8090-4D156B40D433" },
                new SeasonRating { Id = 917, Rating = 5, Message = "", SeasonId = 5401, UserId = "CF596B99-A0CC-463B-8090-4D156B40D433" }
            };
        }


        private static IEnumerable<object[]> GetBasicData()
        {
            var ids = new long[] { 1, 2, 3, 912, 913, 916, 917 };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i] };
            }
        }

        private static IEnumerable<object[]> GetInvalidIdData()
        {
            var ids = new long[] { 0, -1, -5, -10, -99 };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i] };
            }
        }

        private static IEnumerable<object[]> GetNotExistingIdData()
        {
            var ids = new long[] { 231, 5566 };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i] };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonRating_ShouldWork(long id)
        {
            SeasonRating foundSeasonRating = null;
            var sp = SetupDI(services =>
            {
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonRatingReadRepo.Setup(srr => srr.GetSeasonRatingById(It.IsAny<long>())).Callback<long>(srId => foundSeasonRating = allSeasonRatings.SingleOrDefault(sr => sr.Id == srId)).ReturnsAsync(() => foundSeasonRating);
                seasonRatingWriteRepo.Setup(srw => srw.DeleteSeasonRating(It.IsAny<SeasonRating>())).Callback<SeasonRating>(sr => allSeasonRatings.Remove(sr));

                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingDelete, SeasonRatingDeleteHandler>();
            });

            var beforeCount = allSeasonRatings.Count;
            var seasonRatingDeleteHandler = sp.GetService<ISeasonRatingDelete>();
            await seasonRatingDeleteHandler!.DeleteSeasonRating(id);
            allSeasonRatings.Count.Should().BeLessThan(beforeCount);
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidIdData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonRating_InvalidId_ThrowException(long id)
        {
            var sp = SetupDI(services =>
            {
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingDelete, SeasonRatingDeleteHandler>();
            });

            var seasonRatingDeleteHandler = sp.GetService<ISeasonRatingDelete>();
            Func<Task> deleteSeasonRatingFunc = async () => await seasonRatingDeleteHandler!.DeleteSeasonRating(id);
            await deleteSeasonRatingFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingIdData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonRating_NotExistingId_ThrowException(long id)
        {
            SeasonRating foundSeasonRating = null;
            var sp = SetupDI(services =>
            {
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonRatingReadRepo.Setup(srr => srr.GetSeasonRatingById(It.IsAny<long>())).Callback<long>(srId => foundSeasonRating = allSeasonRatings.SingleOrDefault(sr => sr.Id == srId)).ReturnsAsync(() => foundSeasonRating);

                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingDelete, SeasonRatingDeleteHandler>();
            });

            var seasonRatingDeleteHandler = sp.GetService<ISeasonRatingDelete>();
            Func<Task> deleteSeasonRatingFunc = async () => await seasonRatingDeleteHandler!.DeleteSeasonRating(id);
            await deleteSeasonRatingFunc.Should().ThrowAsync<NotFoundObjectException<SeasonRating>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonRating_ThrowException(long id)
        {
            var sp = SetupDI(services =>
            {
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonRatingReadRepo.Setup(srr => srr.GetSeasonRatingById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingDelete, SeasonRatingDeleteHandler>();
            });

            var seasonRatingDeleteHandler = sp.GetService<ISeasonRatingDelete>();
            Func<Task> deleteSeasonRatingFunc = async () => await seasonRatingDeleteHandler!.DeleteSeasonRating(id);
            await deleteSeasonRatingFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanDb()
        {
            allSeasonRatings.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allUsers.Clear();
            allUsers = null;
            allSeasons = null;
            allAnimeInfos = null;
            allSeasonRatings = null;
        }
    }
}
