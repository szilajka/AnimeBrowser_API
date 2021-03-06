﻿using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
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
    public class AnimeInfoNameCreationTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private IList<AnimeInfoName> allAnimeInfoNames;
        private static IList<AnimeInfoNameCreationRequestModel> allRequestModels;


        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<AnimeInfoNameCreationRequestModel>();

            var titles = new string[] { "JoJo", "T", "JoJo no Kimyō na Bōken", "ジョジョの奇妙な冒険", "JoJo ninna adventurore",
                $"{new string(' ', 150)}Jojo{new string(' ', 150)}", $"{new string(' ', 300)}ジョジョの奇妙な冒険", $"JoJo no Kimyō na Bōken{new string(' ', 300)}", new string('T', 254), new string('T', 255)};
            var animeInfoIds = new long[] { 1, 1, 1, 1, 1,
                1, 1, 1, 2, 2 };
            for (var i = 0; i < titles.Length; i++)
            {
                allRequestModels.Add(new AnimeInfoNameCreationRequestModel(title: titles[i], animeInfoId: animeInfoIds[i]));
            }
        }

        [TestInitialize]
        public void InitDb()
        {
            allAnimeInfos = new List<AnimeInfo> {
                new AnimeInfo { Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false },
                new AnimeInfo { Id = 2, Title = "Kuroku no Basketball", Description = string.Empty, IsNsfw = false }
            };

            allAnimeInfoNames = new List<AnimeInfoName>
            {
                new AnimeInfoName { Id = 1, Title = "JoJo no Kimyō na Bōken Anime", AnimeInfoId = 1 },
                new AnimeInfoName { Id = 2, Title = "ジョジョの奇妙な冒険 Anime", AnimeInfoId = 1 },
                new AnimeInfoName { Id = 3, Title = "JoJo Anime", AnimeInfoId = 1 },
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
                yield return new object[] { new AnimeInfoNameCreationRequestModel(title: arm.Title, animeInfoId: arm.AnimeInfoId) };
            }
        }

        private static IEnumerable<object[]> GetInvalidAnimeInfoIdData()
        {
            var animeInfoIds = new long[] { 0, -1, -30 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { new AnimeInfoNameCreationRequestModel(title: arm.Title, animeInfoId: animeInfoIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetNotExistingAnimeInfoIdData()
        {
            var animeInfoIds = new long[] { 10, 130, 15 };
            for (var i = 0; i < animeInfoIds.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { new AnimeInfoNameCreationRequestModel(title: arm.Title, animeInfoId: animeInfoIds[i]) };
            }
        }

        private static IEnumerable<object[]> GetInvalidTitleData()
        {
            var titles = new string[] { "", new string(' ', 300), new string('\t', 300), new string('T', 256), $"{new string(' ', 300)}{new string('T', 256)}" };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            var propertyName = nameof(AnimeInfoNameCreationRequestModel.Title);
            for (var i = 0; i < titles.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { new AnimeInfoNameCreationRequestModel(title: titles[i], animeInfoId: arm.AnimeInfoId), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingTitleData()
        {
            var titles = new string[] { "JoJo no Kimyō na Bōken Anime", "ジョジョの奇妙な冒険 Anime", "JoJo's Bizarre Adventure",
                "黒子のバスケ", "El baloncesto de Kuroko", "Kuroku no Basketball" };
            var animeInfoIds = new long[] { 1, 1, 1, 2, 2, 2 };
            for (var i = 0; i < titles.Length; i++)
            {
                var arm = allRequestModels[i];
                yield return new object[] { new AnimeInfoNameCreationRequestModel(title: titles[i], animeInfoId: animeInfoIds[i]) };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfoName_ShouldWork(AnimeInfoNameCreationRequestModel requestModel)
        {
            AnimeInfoName foundAnimeInfoName = null;
            AnimeInfo foundAnimeInfo = null;
            bool isExistsWithSameTitle = false;
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.IsExistingWithSameTitle(It.IsAny<string>(), It.IsAny<long>()))
                    .Callback<string, long>((title, aiId) => isExistsWithSameTitle = allAnimeInfoNames.Any(ain => ain.AnimeInfoId == aiId && ain.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => isExistsWithSameTitle);
                animeInfoNameWriteRepo.Setup(ainw => ainw.CreateAnimeInfoName(It.IsAny<AnimeInfoName>()))
                    .Callback<AnimeInfoName>(ain => { foundAnimeInfoName = ain; foundAnimeInfoName.Id = 600; })
                    .ReturnsAsync(() => foundAnimeInfoName!);

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            });

            var responseModel = requestModel.ToAnimeInfoName().ToCreationResponseModel();
            responseModel.Id = 600;
            var animeInfoNameCreateHandler = sp.GetService<IAnimeInfoNameCreation>();
            var animeInfoNameResponseModel = await animeInfoNameCreateHandler!.CreateAnimeInfoName(requestModel);
            animeInfoNameResponseModel.Should().BeEquivalentTo(responseModel);
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingAnimeInfoIdData), DynamicDataSourceType.Method),
            DynamicData(nameof(GetInvalidAnimeInfoIdData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfoName_NotExistingAnimeInfoId_ThrowException(AnimeInfoNameCreationRequestModel requestModel)
        {
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            });

            var animeInfoNameCreateHandler = sp.GetService<IAnimeInfoNameCreation>();
            Func<Task> createAnimeInfoNameFunc = async () => await animeInfoNameCreateHandler!.CreateAnimeInfoName(requestModel);
            await createAnimeInfoNameFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfoName_InvalidTitle_ThrowException(AnimeInfoNameCreationRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            });

            var animeInfoNameCreateHandler = sp.GetService<IAnimeInfoNameCreation>();
            Func<Task> createAnimeInfoNameFunc = async () => await animeInfoNameCreateHandler!.CreateAnimeInfoName(requestModel);
            var valEx = await createAnimeInfoNameFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetAlreadyExistingTitleData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfoName_AlreadyExistingTitle_ThrowException(AnimeInfoNameCreationRequestModel requestModel)
        {
            AnimeInfoName foundAnimeInfoName = null;
            AnimeInfo foundAnimeInfo = null;
            bool isExistsWithSameTitle = false;
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoNameReadRepo.Setup(ainr => ainr.IsExistingWithSameTitle(It.IsAny<string>(), It.IsAny<long>()))
                    .Callback<string, long>((title, aiId) => isExistsWithSameTitle = allAnimeInfoNames.Any(ain => ain.AnimeInfoId == aiId && ain.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => isExistsWithSameTitle);
                animeInfoNameWriteRepo.Setup(ainw => ainw.CreateAnimeInfoName(It.IsAny<AnimeInfoName>()))
                    .Callback<AnimeInfoName>(ain => { foundAnimeInfoName = ain; foundAnimeInfoName.Id = 600; })
                    .ReturnsAsync(() => foundAnimeInfoName!);

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            });

            var animeInfoNameCreateHandler = sp.GetService<IAnimeInfoNameCreation>();
            Func<Task> createAnimeInfoNameFunc = async () => await animeInfoNameCreateHandler!.CreateAnimeInfoName(requestModel);
            await createAnimeInfoNameFunc.Should().ThrowAsync<AlreadyExistingObjectException<AnimeInfoName>>();
        }

        [TestMethod]
        public async Task CreateAnimeInfoName_EmptyObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            });

            var animeInfoNameCreateHandler = sp.GetService<IAnimeInfoNameCreation>();
            Func<Task> createAnimeInfoNameFunc = async () => await animeInfoNameCreateHandler!.CreateAnimeInfoName(null);
            await createAnimeInfoNameFunc.Should().ThrowAsync<EmptyObjectException<AnimeInfoNameCreationRequestModel>>();
        }

        [DataTestMethod,
             DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfoName_ThrowException(AnimeInfoNameCreationRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();

                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            });

            var animeInfoNameCreateHandler = sp.GetService<IAnimeInfoNameCreation>();
            Func<Task> createAnimeInfoNameFunc = async () => await animeInfoNameCreateHandler!.CreateAnimeInfoName(requestModel);
            await createAnimeInfoNameFunc.Should().ThrowAsync<InvalidOperationException>();
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
