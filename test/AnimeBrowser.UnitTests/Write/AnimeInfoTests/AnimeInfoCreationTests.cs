using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write;
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
            for (var i = 0; i < titles.Length; i++)
            {
                allRequestModels.Add(new AnimeInfoCreationRequestModel(title: titles[i], description: descriptions[i], isNsfw: isNsfws[i]));
            }
        }

        private static IEnumerable<object[]> GetShouldWorkData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { new AnimeInfoCreationRequestModel(title: airm.Title, description: airm.Description, isNsfw: airm.IsNsfw) };
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
            for (var i = 0; i < titles.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { new AnimeInfoCreationRequestModel(title: titles[i], description: airm.Description, isNsfw: airm.IsNsfw), errorCodes[i] };
            }
        }

        [DataTestMethod,
        DynamicData(nameof(GetShouldWorkData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_ShouldWork(AnimeInfoCreationRequestModel animeInfoRequestModel)
        {
            AnimeInfo animeInfo = null;
            var responseModel = animeInfoRequestModel.ToAnimeInfo().ToCreationResponseModel();
            responseModel.Id = 1;

            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();

                animeInfoRepo.Setup(ai => ai.CreateAnimeInfo(It.IsAny<AnimeInfo>())).Callback<AnimeInfo>(ai => { animeInfo = ai; animeInfo.Id = 1; }).ReturnsAsync(() => animeInfo!);

                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            var result = await animeInfoHandler!.CreateAnimeInfo(animeInfoRequestModel);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(responseModel);
        }

        [DataTestMethod,
           DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_InvalidTitle_ExceptionThrown(AnimeInfoCreationRequestModel animeInfoRequestModel, ErrorCodes errCode)
        {
            var errors = CreateErrorList(errCode, nameof(AnimeInfoCreationRequestModel.Title));
            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            Func<Task> act = async () => await animeInfoHandler.CreateAnimeInfo(animeInfoRequestModel);
            var valEx = await act.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(o => o.Description));
        }

        [TestMethod]
        public async Task CreateAnimeInfo_TooLongDescription_ExceptionThrown()
        {
            var errors = CreateErrorList(ErrorCodes.TooLongProperty, nameof(AnimeInfoCreationRequestModel.Description));
            var description = new string('A', 30001);
            var requestModel = new AnimeInfoCreationRequestModel { Title = "asd", Description = description, IsNsfw = false };

            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            Func<Task> act = async () => await animeInfoHandler.CreateAnimeInfo(requestModel);
            var valEx = await act.Should().ThrowAsync<ValidationException>();
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
            Func<Task> act = async () => await animeInfoHandler.CreateAnimeInfo(null);
            await act.Should().ThrowAsync<EmptyObjectException<AnimeInfoCreationRequestModel>>();
        }

        [TestMethod]
        public async Task CreateAnimeInfo_RepositoryException_ExceptionThrown()
        {
            var requestModel = new AnimeInfoCreationRequestModel { Title = "asd", Description = "", IsNsfw = false };

            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                animeInfoRepo.Setup(air => air.CreateAnimeInfo(It.IsAny<AnimeInfo>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            Func<Task> act = async () => await animeInfoHandler.CreateAnimeInfo(requestModel);
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
