using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Data.Converters.SecondaryConverters;
using AnimeBrowser.Data.Entities;
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
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.AnimeInfoNameTests
{
    [TestClass]
    public class AnimeInfoNameEditingTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private IList<AnimeInfoName> allAnimeInfoNames;
        private static IList<AnimeInfoNameEditingRequestModel> allRequestModels;


        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<AnimeInfoNameEditingRequestModel>();
            var ids = new long[] { 1, 2, 3, 4, 5,
                10, 11, 100, 110, 200 };
            var titles = new string[] { "Jo", "T", "JoJo ano no Kimyō na nana Bōken", "Some writing with ジョジョの奇妙な冒険", "Las extrañas aventuras de Jojo spanish title",
                $"{new string(' ', 150)}JojoJoJoJoJoJo{new string(' ', 150)}", $"{new string(' ', 300)}ジョジョの奇妙な冒", $"JoJo no Kimyō{new string(' ', 300)}", new string('T', 254), new string('T', 255)};
            var animeInfoIds = new long[] { 1, 1, 1, 1, 1,
                2, 2, 2, 2, 2 };
            for (var i = 0; i < titles.Length; i++)
            {
                allRequestModels.Add(new AnimeInfoNameEditingRequestModel(id: ids[i], title: titles[i], animeInfoId: animeInfoIds[i]));
            }
        }

        [TestInitialize]
        public void InitDb()
        {
            allAnimeInfos = new List<AnimeInfo>
            {
                new AnimeInfo { Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false },
                new AnimeInfo { Id = 2, Title = "Kuroko no Basketball", Description = string.Empty, IsNsfw = false }
            };

            allAnimeInfoNames = new List<AnimeInfoName>
            {
                new AnimeInfoName { Id = 1, Title = "JoJo no Kimyō na Bōken", AnimeInfoId = 1 },
                new AnimeInfoName { Id = 2, Title = "ジョジョの奇妙な冒険", AnimeInfoId = 1 },
                new AnimeInfoName { Id = 3, Title = "JoJo", AnimeInfoId = 1 },
                new AnimeInfoName { Id = 4, Title = "Las extrañas aventuras de Jojo", AnimeInfoId = 1 },
                new AnimeInfoName { Id = 5, Title = "JoJos bisarre eventyr", AnimeInfoId = 1 },

                new AnimeInfoName { Id = 10, Title = "Kuroko no Basket", AnimeInfoId = 2 },
                new AnimeInfoName { Id = 11, Title = "黒子のバスケ", AnimeInfoId = 2 },
                new AnimeInfoName { Id = 100, Title = "Kuroko's Basketball", AnimeInfoId = 2 },
                new AnimeInfoName { Id = 110, Title = "El baloncesto de Kuroko", AnimeInfoId = 2 },
                new AnimeInfoName { Id = 200, Title = "Kurokos basketball", AnimeInfoId = 2 },
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { arm.Id, new AnimeInfoNameEditingRequestModel(id: arm.Id, title: arm.Title, animeInfoId: arm.AnimeInfoId) };
            }
        }

        private static IEnumerable<object[]> GetMismatchingIdData()
        {
            var ids = new long[] { 10, 5, 200, 1, 0, -10 };
            for (var i = 0; i < ids.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { ids[i], new AnimeInfoNameEditingRequestModel(id: arm.Id, title: arm.Title, animeInfoId: arm.AnimeInfoId) };
            }
        }

        private static IEnumerable<object[]> GetInvalidOrNotExistingAnimeInfoNameIdData()
        {
            var ids = new long[] { 99, 0, -10, 500 };
            for (var i = 0; i < ids.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { ids[i], new AnimeInfoNameEditingRequestModel(id: ids[i], title: arm.Title, animeInfoId: arm.AnimeInfoId) };
            }
        }

        private static IEnumerable<object[]> GetInvalidOrNotExistingAnimeInfoIdData()
        {
            var animeInfoIds = new long[] { 99, 0, -10, 500 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { arm.Id, new AnimeInfoNameEditingRequestModel(id: arm.Id, title: arm.Title, animeInfoId: animeInfoIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            var titles = new string[] { "", null, new string(' ', 300), new string('T', 256), new string('T', 300) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            var propertyName = nameof(AnimeInfoNameEditingRequestModel.Title);
            for (var i = 0; i < titles.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { arm.Id, new AnimeInfoNameEditingRequestModel(id: arm.Id, title: titles[i], animeInfoId: arm.AnimeInfoId), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingTitleData()
        {
            var titles = new string[] { "ジョジョの奇妙な冒険", "JoJo no Kimyō na Bōken", "JoJos bisarre eventyr", "ジョジョの奇妙な冒険" };
            for (var i = 0; i < titles.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { arm.Id, new AnimeInfoNameEditingRequestModel(id: arm.Id, title: titles[i], animeInfoId: arm.AnimeInfoId) };
            }
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task EditAnimeInfoName_ShouldWork(long id, AnimeInfoNameEditingRequestModel requestModel)
        {
            AnimeInfoName foundAnimeInfoName = null;
            AnimeInfo foundAnimeInfo = null;
            AnimeInfoName updatedAnimeInfoName = null;
            bool isExistsWithSameTitle = false;
            var sp = SetupDI(services =>
            {
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).Callback<long>(ainId => foundAnimeInfoName = allAnimeInfoNames.SingleOrDefault(ain => ain.Id == ainId)).ReturnsAsync(() => foundAnimeInfoName);
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                animeInfoNameReadRepo.Setup(ainr => ainr.IsExistingWithSameTitle(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long>()))
                    .Callback<long, string, long>((ainId, ainTitle, ainAnimeInfoId) => isExistsWithSameTitle = allAnimeInfoNames.Any(ain => ain.Id != ainId && ain.AnimeInfoId == ainAnimeInfoId && ain.Title.Equals(ainTitle, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => isExistsWithSameTitle);

                animeInfoNameWriteRepo.Setup(ainw => ainw.UpdateAnimeInfoName(It.IsAny<AnimeInfoName>())).Callback<AnimeInfoName>(ain => { allAnimeInfoNames.Remove(foundAnimeInfoName); allAnimeInfoNames.Add(ain); updatedAnimeInfoName = ain; }).ReturnsAsync(() => updatedAnimeInfoName!);

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            });

            var animeInfoName = requestModel.ToAnimeInfoName();
            var responseModel = animeInfoName.ToEditingResponseModel();
            var animeInfoNameEditingHandler = sp.GetService<IAnimeInfoNameEditing>();
            var animeInfoNameResponseModel = await animeInfoNameEditingHandler!.EditAnimeInfoName(id, requestModel);
            animeInfoNameResponseModel.Should().NotBeNull();
            animeInfoNameResponseModel.Should().BeEquivalentTo(responseModel);
            allAnimeInfoNames.Should().ContainEquivalentOf(animeInfoName, options => options.Excluding(x => x.AnimeInfo));
        }


        [DataTestMethod,
            DynamicData(nameof(GetMismatchingIdData), DynamicDataSourceType.Method), DataRow(1, null)]
        public async Task EditAnimeInfoName_MismatchingId_ThrowException(long id, AnimeInfoNameEditingRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            });

            var animeInfoNameEditingHandler = sp.GetService<IAnimeInfoNameEditing>();
            Func<Task> animeInfoNameEditingFunc = async () => await animeInfoNameEditingHandler!.EditAnimeInfoName(id, requestModel);
            await animeInfoNameEditingFunc.Should().ThrowAsync<MismatchingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidOrNotExistingAnimeInfoNameIdData), DynamicDataSourceType.Method)]
        public async Task EditAnimeInfoName_InvalidOrNotExistingAnimeInfoNameId_ThrowException(long id, AnimeInfoNameEditingRequestModel requestModel)
        {
            AnimeInfoName foundAnimeInfoName = null;
            var sp = SetupDI(services =>
            {
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).Callback<long>(ainId => foundAnimeInfoName = allAnimeInfoNames.SingleOrDefault(ain => ain.Id == ainId)).ReturnsAsync(() => foundAnimeInfoName);

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            });

            var animeInfoNameEditingHandler = sp.GetService<IAnimeInfoNameEditing>();
            Func<Task> animeInfoNameEditingFunc = async () => await animeInfoNameEditingHandler!.EditAnimeInfoName(id, requestModel);
            await animeInfoNameEditingFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfoName>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidOrNotExistingAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task EditAnimeInfoName_InvalidOrNotExistingAnimeInfoId_ThrowException(long id, AnimeInfoNameEditingRequestModel requestModel)
        {
            AnimeInfoName foundAnimeInfoName = null;
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).Callback<long>(ainId => foundAnimeInfoName = allAnimeInfoNames.SingleOrDefault(ain => ain.Id == ainId)).ReturnsAsync(() => foundAnimeInfoName);
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            });

            var animeInfoNameEditingHandler = sp.GetService<IAnimeInfoNameEditing>();
            Func<Task> animeInfoNameEditingFunc = async () => await animeInfoNameEditingHandler!.EditAnimeInfoName(id, requestModel);
            await animeInfoNameEditingFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task EditAnimeInfoName_InvalidTitle_ThrowException(long id, AnimeInfoNameEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfoName foundAnimeInfoName = null;
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).Callback<long>(ainId => foundAnimeInfoName = allAnimeInfoNames.SingleOrDefault(ain => ain.Id == ainId)).ReturnsAsync(() => foundAnimeInfoName);
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            });

            var animeInfoNameEditingHandler = sp.GetService<IAnimeInfoNameEditing>();
            Func<Task> animeInfoNameEditingFunc = async () => await animeInfoNameEditingHandler!.EditAnimeInfoName(id, requestModel);
            var valEx = await animeInfoNameEditingFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(x => x.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetAlreadyExistingTitleData), DynamicDataSourceType.Method)]
        public async Task EditAnimeInfoName_AlreadyExistingTitle_ThrowException(long id, AnimeInfoNameEditingRequestModel requestModel)
        {
            AnimeInfoName foundAnimeInfoName = null;
            AnimeInfo foundAnimeInfo = null;
            bool isExistsWithSameTitle = false;
            var sp = SetupDI(services =>
            {
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).Callback<long>(ainId => foundAnimeInfoName = allAnimeInfoNames.SingleOrDefault(ain => ain.Id == ainId)).ReturnsAsync(() => foundAnimeInfoName);
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                animeInfoNameReadRepo.Setup(ainr => ainr.IsExistingWithSameTitle(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long>()))
                    .Callback<long, string, long>((ainId, ainTitle, ainAnimeInfoId) => isExistsWithSameTitle = allAnimeInfoNames.Any(ain => ain.Id != ainId && ain.AnimeInfoId == ainAnimeInfoId && ain.Title.Equals(ainTitle, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => isExistsWithSameTitle);

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            });

            var animeInfoNameEditingHandler = sp.GetService<IAnimeInfoNameEditing>();
            Func<Task> animeInfoNameEditingFunc = async () => await animeInfoNameEditingHandler!.EditAnimeInfoName(id, requestModel);
            await animeInfoNameEditingFunc.Should().ThrowAsync<AlreadyExistingObjectException<AnimeInfoName>>();
        }

        [DataTestMethod,
                    DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task EditAnimeInfoName_ThrowException(long id, AnimeInfoNameEditingRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            });

            var animeInfoNameEditingHandler = sp.GetService<IAnimeInfoNameEditing>();
            Func<Task> animeInfoNameEditingFunc = async () => await animeInfoNameEditingHandler!.EditAnimeInfoName(id, requestModel);
            await animeInfoNameEditingFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanDb()
        {
            allAnimeInfoNames.Clear();
            allAnimeInfos.Clear();
            allAnimeInfos = null;
            allAnimeInfoNames = null;
        }


        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
