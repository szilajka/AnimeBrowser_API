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

namespace AnimeBrowser.UnitTests.Write.SeasonRatingTests
{
    [TestClass]
    public class SeasonRatingCreationTests : TestBase
    {
        private IList<User> allUsers;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;
        private IList<SeasonRating> allSeasonRatings;
        private static IList<SeasonRatingCreationRequestModel> allRequestModels;

        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<SeasonRatingCreationRequestModel>();

            var ratings = new int[] {5, 3, 4, 2, 1,
                5, 5, 3, 4, 2 };
            var seasonIds = new long[] { 1, 2, 5401, 5405, 1,
                1, 1, 5401, 2, 5405 };
            var userIds = new string[] { "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "F6560F7D-08B5-402D-90EC-C701952A0CF2", "F6560F7D-08B5-402D-90EC-C701952A0CF2",
                "817AB8E7-CE92-4D45-A93E-31A5D17430A9", "F6560F7D-08B5-402D-90EC-C701952A0CF2", "D7623518-D2C2-4E71-9A9B-C825CE9A44B9", "60697390-85E4-451E-82F6-CB3C13B32B18", "027AAEC6-ED12-420B-9467-1984D4396971" };
            var messages = new string[] { "Very cool season! Waiting for the next. :)", "Not the best season of the series.", "Not perfect, but good enough for me", "Yeah, it was not a cool season, but hope that no more fillers will come...", "Why on earth should anybody create this season?!",
                "M", new string('M', 29999), new string('M', 30000), $"{new string(' ', 30000)}{new string('M', 15000)}", new string('M', 2300)};
            for (var i = 0; i < ratings.Length; i++)
            {
                allRequestModels.Add(new SeasonRatingCreationRequestModel(rating: ratings[i], seasonId: seasonIds[i], userId: userIds[i], message: messages[i]));
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
        }


        private static IEnumerable<object[]> GetBasicData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var srrm = allRequestModels[i];
                yield return new object[] { new SeasonRatingCreationRequestModel(rating: srrm.Rating, seasonId: srrm.SeasonId, userId: srrm.UserId, message: srrm.Message) };
            }
        }

        private static IEnumerable<object[]> GetNotExistingSeasonData()
        {
            var seasonIds = new long[] { 10, 20, 30, 0, -10, -100 };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                var srrm = allRequestModels[i];
                yield return new object[] { new SeasonRatingCreationRequestModel(rating: srrm.Rating, seasonId: seasonIds[i], userId: srrm.UserId, message: srrm.Message) };
            }
        }

        private static IEnumerable<object[]> GetNotExistingUserData()
        {
            var userIds = new string[] { "123123", "", null, "2F9E92EF-9B05-4DAA-BA4D-2F453A3B4E53" };
            for (var i = 0; i < userIds.Length; i++)
            {
                var srrm = allRequestModels[i];
                yield return new object[] { new SeasonRatingCreationRequestModel(rating: srrm.Rating, seasonId: srrm.SeasonId, userId: userIds[i], message: srrm.Message) };
            }
        }

        private static IEnumerable<object[]> GetInvalidRatingData()
        {
            var propertyName = nameof(SeasonRatingCreationRequestModel.Rating);
            var ratings = new int[] { 0, -5, 6, 123 };
            var errorCodes = new ErrorCodes[] { ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty, ErrorCodes.OutOfRangeProperty };
            for (var i = 0; i < ratings.Length; i++)
            {
                var srrm = allRequestModels[i];
                yield return new object[] { new SeasonRatingCreationRequestModel(rating: ratings[i], seasonId: srrm.SeasonId, userId: srrm.UserId, message: srrm.Message), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetInvalidMessageData()
        {
            var propertyName = nameof(SeasonRatingCreationRequestModel.Message);
            var messages = new string[] { new string('M', 30001), $"{new string(' ', 30000)}{new string('M', 32000)}", new string('M', 32000) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            for (var i = 0; i < messages.Length; i++)
            {
                var srrm = allRequestModels[i];
                yield return new object[] { new SeasonRatingCreationRequestModel(rating: srrm.Rating, seasonId: srrm.SeasonId, userId: srrm.UserId, message: messages[i]), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingSeasonRatingData()
        {
            var seasonIds = new long[] { 1, 2, 5401 };
            var userIds = new string[] { "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977", "15A6B54C-98D0-4396-90E7-C94761DBA977" };
            for (var i = 0; i < userIds.Length; i++)
            {
                var srrm = allRequestModels[i];
                yield return new object[] { new SeasonRatingCreationRequestModel(rating: srrm.Rating, seasonId: seasonIds[i], userId: userIds[i], message: srrm.Message) };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonRating_ShouldWork(SeasonRatingCreationRequestModel requestModel)
        {
            Season foundSeason = null;
            User foundUser = null;
            SeasonRating foundSeasonRating = null;
            SeasonRating savedSeasonRating = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);
                seasonRatingReadRepo.Setup(srr => srr.GetSeasonRatingBySeasonAndUserId(It.IsAny<long>(), It.IsAny<string>()))
                    .Callback<long, string>((sId, uId) => foundSeasonRating = allSeasonRatings.SingleOrDefault(sr => sr.SeasonId == sId && sr.UserId.Equals(uId, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => foundSeasonRating);
                seasonRatingWriteRepo.Setup(srw => srw.CreateSeasonRating(It.IsAny<SeasonRating>())).Callback<SeasonRating>(sr => { sr.Id = 12; savedSeasonRating = sr; allSeasonRatings.Add(savedSeasonRating); }).ReturnsAsync(() => savedSeasonRating!);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });

            var beforeCount = allSeasonRatings.Count;
            var seasonRating = requestModel.ToSeasonRating();
            seasonRating.Id = 12;
            var responseModel = seasonRating.ToCreationResponseModel();
            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            var createdSeasonRatingResponseModel = await seasonRatingCreationHandler!.CreateSeasonRating(requestModel);
            createdSeasonRatingResponseModel.Should().NotBeNull();
            createdSeasonRatingResponseModel.Should().BeEquivalentTo(responseModel);
            allSeasonRatings.Count.Should().BeGreaterThan(beforeCount);
        }

        [TestMethod]
        public async Task CreateSeasonRating_EmptyObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });

            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            Func<Task> createSeasonRatingFunc = async () => await seasonRatingCreationHandler!.CreateSeasonRating(null);
            await createSeasonRatingFunc.Should().ThrowAsync<EmptyObjectException<SeasonRatingCreationRequestModel>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingSeasonData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonRating_NotExistingSeason_ThrowException(SeasonRatingCreationRequestModel requestModel)
        {
            Season foundSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });


            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            Func<Task> createSeasonRatingFunc = async () => await seasonRatingCreationHandler!.CreateSeasonRating(requestModel);
            await createSeasonRatingFunc.Should().ThrowAsync<NotFoundObjectException<Season>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingUserData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonRating_NotExistingUser_ThrowException(SeasonRatingCreationRequestModel requestModel)
        {
            Season foundSeason = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });


            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            Func<Task> createSeasonRatingFunc = async () => await seasonRatingCreationHandler!.CreateSeasonRating(requestModel);
            await createSeasonRatingFunc.Should().ThrowAsync<NotFoundObjectException<User>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidRatingData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonRating_InvalidRating_ThrowException(SeasonRatingCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            Season foundSeason = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });


            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            Func<Task> createSeasonRatingFunc = async () => await seasonRatingCreationHandler!.CreateSeasonRating(requestModel);
            var valEx = await createSeasonRatingFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(x => x.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidMessageData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonRating_InvalidMessage_ThrowException(SeasonRatingCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            Season foundSeason = null;
            User foundUser = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });


            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            Func<Task> createSeasonRatingFunc = async () => await seasonRatingCreationHandler!.CreateSeasonRating(requestModel);
            var valEx = await createSeasonRatingFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(x => x.Description));
        }

        [DataTestMethod,
           DynamicData(nameof(GetAlreadyExistingSeasonRatingData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonRating_AlreadyExistingSeasonRating_ThrowException(SeasonRatingCreationRequestModel requestModel)
        {
            Season foundSeason = null;
            User foundUser = null;
            SeasonRating foundSeasonRating = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).Callback<long>(sId => foundSeason = allSeasons.SingleOrDefault(s => s.Id == sId)).ReturnsAsync(() => foundSeason);
                userReadRepo.Setup(ur => ur.GetUserById(It.IsAny<string>())).Callback<string>(uId => foundUser = allUsers.SingleOrDefault(u => u.Id.Equals(uId, StringComparison.OrdinalIgnoreCase))).ReturnsAsync(() => foundUser);
                seasonRatingReadRepo.Setup(srr => srr.GetSeasonRatingBySeasonAndUserId(It.IsAny<long>(), It.IsAny<string>()))
                    .Callback<long, string>((sId, uId) => foundSeasonRating = allSeasonRatings.SingleOrDefault(sr => sr.SeasonId == sId && sr.UserId.Equals(uId, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => foundSeasonRating);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });


            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            Func<Task> createSeasonRatingFunc = async () => await seasonRatingCreationHandler!.CreateSeasonRating(requestModel);
            await createSeasonRatingFunc.Should().ThrowAsync<AlreadyExistingObjectException<SeasonRating>>();
        }

        [DataTestMethod,
           DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonRating_ThrowException(SeasonRatingCreationRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var userReadRepo = new Mock<IUserRead>();
                var seasonRatingReadRepo = new Mock<ISeasonRatingRead>();
                var seasonRatingWriteRepo = new Mock<ISeasonRatingWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => userReadRepo.Object);
                services.AddTransient(factory => seasonRatingReadRepo.Object);
                services.AddTransient(factory => seasonRatingWriteRepo.Object);
                services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            });

            var seasonRatingCreationHandler = sp.GetService<ISeasonRatingCreation>();
            Func<Task> createSeasonRatingFunc = async () => await seasonRatingCreationHandler!.CreateSeasonRating(requestModel);
            await createSeasonRatingFunc.Should().ThrowAsync<InvalidOperationException>();
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


        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
