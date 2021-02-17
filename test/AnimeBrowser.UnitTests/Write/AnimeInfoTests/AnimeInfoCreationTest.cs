using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
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
    public class AnimeInfoCreationTest : TestBase
    {
        private static IEnumerable<object[]> GetShouldWorkData()
        {
            var title = "JoJo's Bizarre Adventure";
            yield return new object[] { title, null, false };
            yield return new object[] { title, "The series tells about the Joestar family, starting with Jonathan Joestar and his adventures, involving Dio Brando, who is the first part's bad guy. Later on, the story tells about Joseph, Jotaro, Josuke and Giorno.", false };
            yield return new object[] { $"  {title}  ", "   The series tells about the Joestar family, starting with Jonathan Joestar and his adventures, involving Dio Brando, who is the first part's bad guy. Later on, the story tells about Joseph, Jotaro, Josuke and Giorno.     ", false };
            yield return new object[] { $"  {title} ", "       ", false };
            yield return new object[] { $"  {title} ", new string('A', 100), false };
            yield return new object[] { $"  {title} ", new string('A', 29999), false };
            yield return new object[] { $"  {title} ", new string('A', 30000), false };
        }


        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            yield return new object[] { null, null, false, ErrorCodes.EmptyProperty };
            yield return new object[] { "A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of existence in this spot, which was created for the bliss of souls like mine. I am so happy, my dear friend, so absorbed in the exquisit", null, false, ErrorCodes.TooLongProperty };
            yield return new object[] { $"                             ", "", false, ErrorCodes.EmptyProperty };
            yield return new object[] { $"                                                                                                                                                                                                                                                                                                         ", "       ", false, ErrorCodes.EmptyProperty };
        }

        [DataTestMethod,
        DynamicData(nameof(GetShouldWorkData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_ShouldWork(string title, string description, bool isNsfw)
        {
            var requestModel = new AnimeInfoCreationRequestModel { Title = title, Description = description, IsNsfw = isNsfw };
            var animeInfo = new AnimeInfo { Title = title?.Trim(), Description = description?.Trim(), IsNsfw = isNsfw };
            var responseModel = new AnimeInfoCreationResponseModel(id: 1, title: title?.Trim(), description: description?.Trim(), isNsfw: isNsfw);

            var sp = SetupDI((services) =>
            {
                var animeInfoRepo = new Mock<IAnimeInfoWrite>();
                animeInfoRepo.Setup(ai => ai.CreateAnimeInfo(It.IsAny<AnimeInfo>())).Callback<AnimeInfo>(ai => animeInfo.Id = 1).ReturnsAsync(() => animeInfo);

                services.AddTransient(_ => animeInfoRepo.Object);
                services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            });

            var animeInfoHandler = sp.GetService<IAnimeInfoCreation>();
            var result = await animeInfoHandler.CreateAnimeInfo(requestModel);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(responseModel);
        }

        [DataTestMethod,
           DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_InvalidTitle_ExceptionThrown(string title, string description, bool isNsfw, ErrorCodes errCode)
        {
            var errors = CreateErrorList(errCode, nameof(AnimeInfoCreationRequestModel.Title));
            var requestModel = new AnimeInfoCreationRequestModel { Title = title, Description = description, IsNsfw = isNsfw };

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
    }
}
