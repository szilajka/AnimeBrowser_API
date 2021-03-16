using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Data.Converters.SecondaryConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Entities.Identity;
using AnimeBrowser.Data.Interfaces.Read.IdentityInterfaces;
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

namespace AnimeBrowser.UnitTests.Write.EpisodeRatingTests
{
    [TestClass]
    public class EpisodeRatingEditingTests : TestBase
    {
        private IList<User> allUsers;
        private IList<Episode> allEpisodes;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<EpisodeRating> allEpisodeRatings;
        private static IList<EpisodeRatingEditingRequestModel> allRequestModels;

        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<EpisodeRatingEditingRequestModel>();

            var ids = new long[] { 1, 2, 3, 4, 5,
                1, 6, 7, 8, 9};
            var ratings = new int[] {5, 2, 4, 2, 1,
                5, 5, 3, 4, 2 };
            var episodeIds = new long[] { 1, 2, 3, 4, 3,
                1, 2, 1, 3, 4 };
            var userIds = new string[] { "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977", "65F041D2-7217-4EA6-9065-9C9AB6290B35", "65F041D2-7217-4EA6-9065-9C9AB6290B35",
                "15A6B54C-98D0-4396-90E7-C94761DBA977", "65F041D2-7217-4EA6-9065-9C9AB6290B35", "65F041D2-7217-4EA6-9065-9C9AB6290B35", "5879560D-65C5-4699-9449-86CC57EF3111", "5879560D-65C5-4699-9449-86CC57EF3111" };
            var messages = new string[] { "I really liked this episode. One of the best episodes ever!", "It was so booooooring, I almost fall asleep while watching...", "After rewatching, I've got some questions about the story, but it was good after all.", "It wasn't that bad, just simply bad.", "This was very bad!",
                "", new string('M', 29999), new string('M', 30000), $"{new string(' ', 32000)}{new string('M', 1500)}", string.Empty };
            for (var i = 0; i < ratings.Length; i++)
            {
                allRequestModels.Add(new EpisodeRatingEditingRequestModel(id: ids[i], rating: ratings[i], episodeId: episodeIds[i], userId: userIds[i], message: messages[i]));
            }
        }

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
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 2, SeasonNumber = 1, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 5401, SeasonNumber = 1, Title = "The Pillarmen's revenge", Description = "In this season the pillarmen are taking revenge for their death.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 5405, SeasonNumber = 1, Title = "Life is basketball", Description = "We know the MC, who wants to get his revenge for kicking her out of the basketball team by making a new team.",
                    StartDate = today.AddYears(-10).AddMonths(-3), EndDate = today.AddYears(-9),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("Basketball Carousel"), Cover = Encoding.UTF8.GetBytes("Basketball Cover"),
                },
                  new Season{ Id = 6001, SeasonNumber = 1, Title = "Monochrome", Description = "Mc sees everything in monochrome. Due to his illness, demons attack him.",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatusEnum.NotAired, NumberOfEpisodes = 10, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("Basketball Carousel"), Cover = Encoding.UTF8.GetBytes("Basketball Cover"),
                }
            };

            allEpisodes = new List<Episode>
            {
                new Episode { Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatusEnum.Aired, Title = "Prologue", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatusEnum.Aired, Title = "Beginning of something new", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 3, EpisodeNumber = 1, AirStatus = (int)AirStatusEnum.Aired, Title = "Family relations", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 4, EpisodeNumber = 2, AirStatus = (int)AirStatusEnum.NotAired, Title = "Parasites", Description = "No one knows what it's like...",
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
                // For some reason, when episode/season was deleted, it was not deleted and still here
                new EpisodeRating { Id = 912, Rating = 2, Message = "", EpisodeId = 912, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
                new EpisodeRating { Id = 913, Rating = 5, Message = "", EpisodeId = 913, UserId = "5879560D-65C5-4699-9449-86CC57EF3111" },
                // For some reason, when user was deleted, it was not deleted and still here
                new EpisodeRating { Id = 916, Rating = 2, Message = "", EpisodeId = 2, UserId = "CF596B99-A0CC-463B-8090-4D156B40D433" },
                new EpisodeRating { Id = 917, Rating = 5, Message = "", EpisodeId = 3, UserId = "CF596B99-A0CC-463B-8090-4D156B40D433" }
            };
        }


        private static IEnumerable<object[]> GetBasicData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { errm.Id, new EpisodeRatingEditingRequestModel(id: errm.Id, rating: errm.Rating, episodeId: errm.EpisodeId, userId: errm.UserId, message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetMismatchingIdData()
        {
            var ids = new long[] { 10, 30, -1, 4, 5, 1, 6 };
            var episodeIds = new long[] { 1, 2, 3, 1, 2, 1, 2 };
            var userIds = new string[] { "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977", "65F041D2-7217-4EA6-9065-9C9AB6290B35", "65F041D2-7217-4EA6-9065-9C9AB6290B35",
                "5879560D-65C5-4699-9449-86CC57EF3111", "5879560D-65C5-4699-9449-86CC57EF3111" };
            for (var i = 0; i < ids.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { ids[i], new EpisodeRatingEditingRequestModel(id: errm.Id, rating: errm.Rating, episodeId: episodeIds[i], userId: userIds[i], message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetNotExistingEpisodeRatingData()
        {
            var episodeRatingIds = new long[] { 0, -1, -99, 151, 200 };
            for (var i = 0; i < episodeRatingIds.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { episodeRatingIds[i], new EpisodeRatingEditingRequestModel(id: episodeRatingIds[i], rating: errm.Rating, episodeId: errm.EpisodeId, userId: errm.UserId, message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetNotExistingEpisodeData()
        {
            var ids = new long[] { 912, 913 };
            var episodeIds = new long[] { 912, 913 };
            var userIds = new string[] { "5879560D-65C5-4699-9449-86CC57EF3111", "5879560D-65C5-4699-9449-86CC57EF3111" };
            for (var i = 0; i < episodeIds.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { ids[i], new EpisodeRatingEditingRequestModel(id: ids[i], rating: errm.Rating, episodeId: episodeIds[i], userId: userIds[i], message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetNotExistingUserData()
        {
            var ids = new long[] { 916, 917 };
            var episodeIds = new long[] { 2, 3 };
            var userIds = new string[] { "CF596B99-A0CC-463B-8090-4D156B40D433", "CF596B99-A0CC-463B-8090-4D156B40D433" };
            for (var i = 0; i < ids.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { ids[i], new EpisodeRatingEditingRequestModel(id: ids[i], rating: errm.Rating, episodeId: episodeIds[i], userId: userIds[i], message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetInvalidRatingData()
        {
            var propertyName = nameof(EpisodeRatingEditingRequestModel.Rating);
            var errorCodes = new ErrorCodes[] { ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty };
            var ratings = new int[] { 0, -1, -5, 6, 7, 10 };
            for (var i = 0; i < ratings.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { errm.Id, new EpisodeRatingEditingRequestModel(id: errm.Id, rating: ratings[i], episodeId: errm.EpisodeId, userId: errm.UserId, message: errm.Message), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetInvalidMessageData()
        {
            var propertyName = nameof(EpisodeRatingEditingRequestModel.Message);
            var errorCodes = new ErrorCodes[] { ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            var messages = new string[] { new string('M', 30001), new string('M', 35000), $"{new string(' ', 35000)}{new string('M', 32000)}" };
            for (var i = 0; i < messages.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { errm.Id, new EpisodeRatingEditingRequestModel(id: errm.Id, rating: errm.Rating, episodeId: errm.EpisodeId, userId: errm.UserId, message: messages[i]), errorCodes[i], propertyName };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_ShouldWork(long id, EpisodeRatingEditingRequestModel requestModel)
        {
            EpisodeRating foundEpisodeRating = null;
            Episode foundEpisode = null;
            User foundUser = null;
            EpisodeRating savedEpisodeRating = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);
                episodeRatingWriteRepo.Setup(erw => erw.UpdateEpisodeRating(It.IsAny<EpisodeRating>())).Callback<EpisodeRating>(er => { allEpisodeRatings.Remove(foundEpisodeRating); savedEpisodeRating = er; allEpisodeRatings.Add(savedEpisodeRating); }).ReturnsAsync(() => savedEpisodeRating!);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRating = requestModel.ToEpisodeRating();
            var responseModel = episodeRating.ToEditingResponseModel();
            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            var updatedEpisodeRatingResponseModel = await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            updatedEpisodeRatingResponseModel.Should().NotBeNull();
            updatedEpisodeRatingResponseModel.Should().BeEquivalentTo(responseModel);
            allEpisodeRatings.Should().ContainEquivalentOf(savedEpisodeRating);
        }

        [DataTestMethod,
            DynamicData(nameof(GetMismatchingIdData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_MismhatchingId_ThrowException(long id, EpisodeRatingEditingRequestModel requestModel)
        {
            EpisodeRating foundEpisodeRating = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            Func<Task> episodeRatingEditingFunc = async () => await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            await episodeRatingEditingFunc.Should().ThrowAsync<MismatchingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingEpisodeRatingData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_NotExistingEpisodeRating_ThrowException(long id, EpisodeRatingEditingRequestModel requestModel)
        {
            EpisodeRating foundEpisodeRating = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            Func<Task> episodeRatingEditingFunc = async () => await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            await episodeRatingEditingFunc.Should().ThrowAsync<NotFoundObjectException<EpisodeRating>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingEpisodeData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_NotExistingEpisode_ThrowException(long id, EpisodeRatingEditingRequestModel requestModel)
        {
            EpisodeRating foundEpisodeRating = null;
            Episode foundEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            Func<Task> episodeRatingEditingFunc = async () => await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            await episodeRatingEditingFunc.Should().ThrowAsync<NotFoundObjectException<Episode>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingUserData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_NotExistingUser_ThrowException(long id, EpisodeRatingEditingRequestModel requestModel)
        {
            EpisodeRating foundEpisodeRating = null;
            Episode foundEpisode = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            Func<Task> episodeRatingEditingFunc = async () => await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            await episodeRatingEditingFunc.Should().ThrowAsync<NotFoundObjectException<User>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidRatingData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_InvalidRating_ThrowException(long id, EpisodeRatingEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            EpisodeRating foundEpisodeRating = null;
            Episode foundEpisode = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            Func<Task> episodeRatingEditingFunc = async () => await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            var valEx = await episodeRatingEditingFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(x => x.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidMessageData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_InvalidMessage_ThrowException(long id, EpisodeRatingEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            EpisodeRating foundEpisodeRating = null;
            Episode foundEpisode = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).Callback<long>(erId => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.Id == erId)).ReturnsAsync(() => foundEpisodeRating);
                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            Func<Task> episodeRatingEditingFunc = async () => await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            var valEx = await episodeRatingEditingFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(x => x.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task EditEpisodeRating_ThrowException(long id, EpisodeRatingEditingRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            });

            var episodeRatingEditingHandler = sp.GetService<IEpisodeRatingEditing>();
            Func<Task> episodeRatingEditingFunc = async () => await episodeRatingEditingHandler!.EditEpisodeRating(id, requestModel);
            await episodeRatingEditingFunc.Should().ThrowAsync<InvalidOperationException>();
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

        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
