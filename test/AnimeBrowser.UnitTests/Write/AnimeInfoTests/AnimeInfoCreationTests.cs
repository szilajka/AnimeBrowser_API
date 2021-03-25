using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Services.Write.MainHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Data.Converters.MainConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using AnimeBrowser.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.AnimeInfoTests
{
    [TestClass]
    public class AnimeInfoCreationTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private static IList<AnimeInfoCreationRequestModel> allRequestModels;

        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<AnimeInfoCreationRequestModel>();

            var titles = new string[] {
                "JoJo's Bizarre Adventure",
                "Boku no Hero Academia",
                "One Piece",
                "T",
                new string('T', 254),
                new string('T', 255),
                $"{new string(' ', 105)}Boku no Hero Academia{new string(' ', 314)}",
                $"{new string(' ', 30000)}One Piece{new string(' ', 314)}"
            };
            var descriptions = new string[] {
                "The series tells about the Joestar family, starting with Jonathan Joestar and his adventures, involving Dio Brando, who is the first part's bad guy. Later on, the story tells about Joseph, Jotaro, Josuke and Giorno.",
                null,
                "",
                "D",
                new string('D', 29999),
                new string('D', 30000),
                $"{new string(' ', 1456)}Fighting heroes in school.{new string(' ', 134)}",
                $"{new string(' ', 30000)}MC wanan be the next Pirate King.{new string(' ', 134)}"
            };
            var isNsfws = new bool[] { false, false, false, false, true, true, true, true };
            var isActives = new bool[] { true, true, true, true, false, false, false, false };
            for (var i = 0; i < titles.Length; i++)
            {
                allRequestModels.Add(new AnimeInfoCreationRequestModel(title: titles[i], description: descriptions[i], isNsfw: isNsfws[i], isActive: isActives[i]));
            }
        }

        [TestInitialize]
        public void InitDb()
        {
            allAnimeInfos = new List<AnimeInfo> {
                new AnimeInfo { Id = 10, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false, IsActive = true },
                new AnimeInfo { Id = 20, Title = "Kuroku no Basketball", Description = string.Empty, IsNsfw = false, IsActive = true }
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { new AnimeInfoCreationRequestModel(title: airm.Title, description: airm.Description, isNsfw: airm.IsNsfw, isActive: airm.IsActive) };
            }
        }

        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            var titles = new string[] {
                null,
                "A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of existence in this spot, which was created for the bliss of souls like mine. I am so happy, my dear friend, so absorbed in the exquisit",
                new string(' ', 100),
                new string(' ', 300)
            };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty };
            string propertyName = nameof(AnimeInfoCreationRequestModel.Title);
            for (var i = 0; i < titles.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { new AnimeInfoCreationRequestModel(title: titles[i], description: airm.Description, isNsfw: airm.IsNsfw, isActive: airm.IsActive), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetInvalidDescriptionData()
        {
            var descriptions = new string[] { new string('D', 30001), new string('D', 35000), new string('D', 90000) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            string propertyName = nameof(AnimeInfoCreationRequestModel.Description);
            for (var i = 0; i < descriptions.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { new AnimeInfoCreationRequestModel(title: airm.Title, description: descriptions[i], isNsfw: airm.IsNsfw, isActive: airm.IsActive), errorCodes[i], propertyName };
            }
        }


        [DataTestMethod,
        DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_ShouldWork(AnimeInfoCreationRequestModel animeInfoRequestModel)
        {
            AnimeInfo savedAnimeInfo = null;
            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();

                animeInfoRepo.Setup(ai => ai.CreateAnimeInfo(It.IsAny<AnimeInfo>())).Callback<AnimeInfo>(ai => { savedAnimeInfo = ai; savedAnimeInfo.Id = 1; allAnimeInfos.Add(savedAnimeInfo); }).ReturnsAsync(() => savedAnimeInfo!);

                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfo = animeInfoRequestModel.ToAnimeInfo();
            animeInfo.Id = 1;
            var responseModel = animeInfo.ToCreationResponseModel();

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            var result = await animeInfoHandler!.CreateAnimeInfo(animeInfoRequestModel);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(responseModel);
            allAnimeInfos.Should().ContainEquivalentOf(animeInfo);
        }

        [DataTestMethod,
           DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_InvalidTitle_ExceptionThrown(AnimeInfoCreationRequestModel animeInfoRequestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            Func<Task> createAnimeInfoFunc = async () => await animeInfoHandler!.CreateAnimeInfo(animeInfoRequestModel);
            var valEx = await createAnimeInfoFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(o => o.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidDescriptionData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_InvalidDescription_ExceptionThrown(AnimeInfoCreationRequestModel animeInfoRequestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            Func<Task> createAnimeInfoFunc = async () => await animeInfoHandler!.CreateAnimeInfo(animeInfoRequestModel);
            var valEx = await createAnimeInfoFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(o => o.Description));
        }

        [TestMethod]
        public async Task CreateAnimeInfo_NullModel_ExceptionThrown()
        {
            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            Func<Task> createAnimeInfoFunc = async () => await animeInfoHandler!.CreateAnimeInfo(null!);
            await createAnimeInfoFunc.Should().ThrowAsync<EmptyObjectException<AnimeInfoCreationRequestModel>>();
        }

        [DataTestMethod,
             DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_RepositoryException_ExceptionThrown(AnimeInfoCreationRequestModel animeInfoRequestModel)
        {
            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                animeInfoRepo.Setup(air => air.CreateAnimeInfo(It.IsAny<AnimeInfo>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            Func<Task> createAnimeInfoFunc = async () => await animeInfoHandler!.CreateAnimeInfo(animeInfoRequestModel);
            await createAnimeInfoFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanDb()
        {
            allAnimeInfos.Clear();
            allAnimeInfos = null;
        }

        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
