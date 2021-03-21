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
    public class EpisodeRatingCreationTests : TestBase
    {
        private IList<User> allUsers;
        private IList<Episode> allEpisodes;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<EpisodeRating> allEpisodeRatings;
        private static IList<EpisodeRatingCreationRequestModel> allRequestModels;

        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<EpisodeRatingCreationRequestModel>();

            var ratings = new int[] {5, 3, 4, 2, 1,
                5, 5, 3, 4, 2 };
            var episodeIds = new long[] { 1, 2, 3, 4, 1,
                1, 1, 3, 2, 4 };
            var userIds = new string[] { "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "F6560F7D-08B5-402D-90EC-C701952A0CF2", "F6560F7D-08B5-402D-90EC-C701952A0CF2",
                "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "F6560F7D-08B5-402D-90EC-C701952A0CF2", "D7623518-D2C2-4E71-9A9B-C825CE9A44B9", "60697390-85E4-451E-82F6-CB3C13B32B18", "027AAEC6-ED12-420B-9467-1984D4396971" };
            var messages = new string[] { "Very cool anime, like it. I will absolutely recommend it to my friends!", "Seen better...", "Not perfect, but good enough for me", "Yeah, it was not a cool anime...", "Why on earth should anybody create this episode?!",
                "D", new string('D', 29999), new string('D', 30000), $"{new string(' ', 30000)}{new string('D', 15000)}", new string('D', 2300)};
            for (var i = 0; i < ratings.Length; i++)
            {
                allRequestModels.Add(new EpisodeRatingCreationRequestModel(rating: ratings[i], episodeId: episodeIds[i], userId: userIds[i], message: messages[i]));
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
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { new EpisodeRatingCreationRequestModel(rating: errm.Rating, episodeId: errm.EpisodeId, userId: errm.UserId, message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetInvalidOrNotExistingEpisodeIdData()
        {
            var episodeIds = new long[] { 0, -1, -10, -99, 567, 322 };
            for (var i = 0; i < episodeIds.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { new EpisodeRatingCreationRequestModel(rating: errm.Rating, episodeId: episodeIds[i], userId: errm.UserId, message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetInvalidOrNotExistingUserIdData()
        {
            var userIds = new string[] { "123123", "", null, "2F9E92EF-9B05-4DAA-BA4D-2F453A3B4E53" };
            for (var i = 0; i < userIds.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { new EpisodeRatingCreationRequestModel(rating: errm.Rating, episodeId: errm.EpisodeId, userId: userIds[i], message: errm.Message) };
            }
        }

        private static IEnumerable<object[]> GetInvalidRatingData()
        {
            var propertyName = nameof(EpisodeRatingCreationRequestModel.Rating);
            var ratings = new int[] { 0, -5, 6, 123 };
            var errorCodes = new ErrorCodes[] { ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty };
            for (var i = 0; i < ratings.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { new EpisodeRatingCreationRequestModel(rating: ratings[i], episodeId: errm.EpisodeId, userId: errm.UserId, message: errm.Message), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetInvalidMessageData()
        {
            var propertyName = nameof(EpisodeRatingCreationRequestModel.Message);
            var messages = new string[] { new string('D', 30001), $"{new string(' ', 30000)}{new string('D', 32000)}", new string('D', 32000) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            for (var i = 0; i < messages.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { new EpisodeRatingCreationRequestModel(rating: errm.Rating, episodeId: errm.EpisodeId, userId: errm.UserId, message: messages[i]), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingEpisodeRatingData()
        {
            var episodeIds = new long[] { 1, 2, 3 };
            var userIds = new string[] { "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977" };
            for (var i = 0; i < episodeIds.Length; i++)
            {
                var errm = allRequestModels[i];
                yield return new object[] { new EpisodeRatingCreationRequestModel(rating: errm.Rating, episodeId: episodeIds[i], userId: userIds[i], message: errm.Message) };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateEpisodeRating_ShouldWork(EpisodeRatingCreationRequestModel requestModel)
        {
            Episode foundEpisode = null;
            User foundUser = null;
            EpisodeRating foundEpisodeRating = null;
            EpisodeRating savedEpisodeRating = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);
                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingByEpisodeAndUserId(It.IsAny<long>(), It.IsAny<string>()))
                    .Callback<long, string>((eId, uId) => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.EpisodeId == eId && er.UserId.Equals(uId, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => foundEpisodeRating);
                episodeRatingWriteRepo.Setup(erw => erw.CreateEpisodeRating(It.IsAny<EpisodeRating>())).Callback<EpisodeRating>(er => { er.Id = 12; savedEpisodeRating = er; allEpisodeRatings.Add(savedEpisodeRating); }).ReturnsAsync(() => savedEpisodeRating!);

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRating = requestModel.ToEpisodeRating();
            episodeRating.Id = 12;
            var responseModel = episodeRating.ToCreationResponseModel();
            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            var createdEpisodeRatingResponseModel = await episodeRatingCreationHandler!.CreateEpisodeRating(requestModel);
            createdEpisodeRatingResponseModel.Should().NotBeNull();
            createdEpisodeRatingResponseModel.Should().BeEquivalentTo(responseModel);
            allEpisodeRatings.Should().ContainEquivalentOf(episodeRating, options => options.ExcludingMissingMembers());
        }

        [TestMethod]
        public async Task CreateEpisodeRating_EmptyObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();
                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            Func<Task> createdEpisodeRatingFunc = async () => await episodeRatingCreationHandler!.CreateEpisodeRating(null);
            await createdEpisodeRatingFunc.Should().ThrowAsync<EmptyObjectException<EpisodeRatingCreationRequestModel>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidOrNotExistingEpisodeIdData), DynamicDataSourceType.Method)]
        public async Task CreateEpisodeRating_InvalidOrNotExistingEpisodeId_ThrowException(EpisodeRatingCreationRequestModel requestModel)
        {
            Episode foundEpisode = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            Func<Task> createdEpisodeRatingFunc = async () => await episodeRatingCreationHandler!.CreateEpisodeRating(requestModel);
            await createdEpisodeRatingFunc.Should().ThrowAsync<NotFoundObjectException<Episode>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidOrNotExistingUserIdData), DynamicDataSourceType.Method)]
        public async Task CreateEpisodeRating_InvalidOrNotExistingUserId_ThrowException(EpisodeRatingCreationRequestModel requestModel)
        {
            Episode foundEpisode = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            Func<Task> createdEpisodeRatingFunc = async () => await episodeRatingCreationHandler!.CreateEpisodeRating(requestModel);
            await createdEpisodeRatingFunc.Should().ThrowAsync<NotFoundObjectException<User>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidRatingData), DynamicDataSourceType.Method)]
        public async Task CreateEpisodeRating_InvalidRating_ThrowException(EpisodeRatingCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            Episode foundEpisode = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            Func<Task> createdEpisodeRatingFunc = async () => await episodeRatingCreationHandler!.CreateEpisodeRating(requestModel);
            var valEx = await createdEpisodeRatingFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(x => x.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidMessageData), DynamicDataSourceType.Method)]
        public async Task CreateEpisodeRating_InvalidMessage_ThrowException(EpisodeRatingCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            Episode foundEpisode = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            Func<Task> createdEpisodeRatingFunc = async () => await episodeRatingCreationHandler!.CreateEpisodeRating(requestModel);
            var valEx = await createdEpisodeRatingFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(x => x.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetAlreadyExistingEpisodeRatingData), DynamicDataSourceType.Method)]
        public async Task CreateEpisodeRating_AlreadyExistingEpisodeRating_ThrowException(EpisodeRatingCreationRequestModel requestModel)
        {
            Episode foundEpisode = null;
            User foundUser = null;
            EpisodeRating foundEpisodeRating = null;
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).Callback<long>(eId => foundEpisode = allEpisodes.SingleOrDefault(e => e.Id == eId)).ReturnsAsync(() => foundEpisode);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);
                episodeRatingReadRepo.Setup(err => err.GetEpisodeRatingByEpisodeAndUserId(It.IsAny<long>(), It.IsAny<string>()))
                    .Callback<long, string>((eId, uId) => foundEpisodeRating = allEpisodeRatings.SingleOrDefault(er => er.EpisodeId == eId && er.UserId.Equals(uId, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => foundEpisodeRating);

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            Func<Task> createdEpisodeRatingFunc = async () => await episodeRatingCreationHandler!.CreateEpisodeRating(requestModel);
            await createdEpisodeRatingFunc.Should().ThrowAsync<AlreadyExistingObjectException<EpisodeRating>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateEpisodeRating_ThrowException(EpisodeRatingCreationRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var episodeReadRepo = new Mock<IEpisodeRead>();
                var userReadRepo = new Mock<IUserRead>();
                var episodeRatingReadRepo = new Mock<IEpisodeRatingRead>();
                var episodeRatingWriteRepo = new Mock<IEpisodeRatingWrite>();

                episodeReadRepo.Setup(er => er.GetEpisodeById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => episodeReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => episodeRatingReadRepo.Object);
                services.AddTransient(factory => episodeRatingWriteRepo.Object);
                services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            });

            var episodeRatingCreationHandler = sp.GetService<IEpisodeRatingCreation>();
            Func<Task> createdEpisodeRatingFunc = async () => await episodeRatingCreationHandler!.CreateEpisodeRating(requestModel);
            await createdEpisodeRatingFunc.Should().ThrowAsync<InvalidOperationException>();
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
