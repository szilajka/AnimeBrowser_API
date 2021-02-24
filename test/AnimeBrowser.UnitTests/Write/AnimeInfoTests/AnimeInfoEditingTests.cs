using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using AnimeBrowser.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.AnimeInfoTests
{
    [TestClass]
    public class AnimeInfoEditingTests : TestBase
    {
        private static IList<AnimeInfoEditingRequestModel> allRequestModels;
        private IList<AnimeInfo> baseAnimeInfoData;

        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<AnimeInfoEditingRequestModel>();
            var ids = new long[] { 1, 2, 3, 7, 10,
                1, 2, 3, 7, 10 };
            var titles = new string[] { "J", "    Made in Abyss    ", "Railgun      ", "        I n d e x         ", "Kono-Su-ba",
                new string('J', 100), $"{new string(' ', 200)}Made in Abyss{new string(' ', 100)}", $"{new string(' ', 300)}Railgun{new string(' ', 300)}", new string('R', 254), new string('I', 255) };
            var descriptions = new string[] {
                null,
                "A horrorific story about a girl, who lost his mom and goes to the Abyss to search for her. For her surprise, a robot boy accompanies her and they are looking for her mom together, while they discover terrific cretures of the abyss who tries to kill them and other people from expeditions. Well, those people are still not nice guys.",
                "Misaka is not an average school girl, she is the Railgun. In this anime we know more and more about the girl and her friends while the story of the city and the image of the world they are living are building up continually.",
                "The main character of the story meets with Index, a magician girl who was chased by some magician. As the story goes, the MC knows the girl's story and get familiar with the magician's world and what world he lives in. The story follows the MC guy and Index's adventures and sometimes their friends are get their showtime.",
                "The main character guy dies and he's teleported into a medieval village with a goddes named Aqua. Their main quest is to defeat the Demon King and sometimes they get into trouble while other folks join to their party.",
                //-----------------
                "Jonathan and his family",
                "",
                new string('R', 23454),
                new string('D', 29999),
                new string('D', 30000)
            };
            var isNsfw = new bool[] { false, false, true, false, false,
                false, false, true, true, true };

            for (var i = 0; i < ids.Length; i++)
            {
                allRequestModels.Add(new AnimeInfoEditingRequestModel(id: ids[i], title: titles[i], description: descriptions[i], isNsfw: isNsfw[i]));
            }
        }

        private static IEnumerable<object[]> GetShouldWorkData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { airm.Id, new AnimeInfoEditingRequestModel(id: airm.Id, title: airm.Title, description: airm.Description, isNsfw: airm.IsNsfw) };
            }
        }

        private static IEnumerable<object[]> GetMismatchingIdData()
        {
            var ids = new long?[] { 2, 10, 7, 1, 2, null };
            for (var i = 0; i < ids.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { ids[i], new AnimeInfoEditingRequestModel(id: airm.Id, title: airm.Title, description: airm.Description, isNsfw: airm.IsNsfw) };
            }
            yield return new object[] { null, null };
        }

        private static IEnumerable<object[]> GetNotExistingIdData()
        {
            var ids = new long[] { 6, 32, 56, 7451, 2956564 };
            for (var i = 0; i < ids.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { ids[i], new AnimeInfoEditingRequestModel(id: ids[i], title: airm.Title, description: airm.Description, isNsfw: airm.IsNsfw) };
            }
        }

        public static IEnumerable<object[]> GetInvalidTitleData()
        {
            var titles = new string[] { "", new string('J', 256), null, new string('J', 475), new string('J', 302414) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            for (var i = 0; i < titles.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { airm.Id, new AnimeInfoEditingRequestModel(id: airm.Id, title: titles[i], description: airm.Description, isNsfw: airm.IsNsfw), errorCodes[i] };
            }
        }

        public static IEnumerable<object[]> GetTooLongDescriptionData()
        {
            var descriptions = new string[] { new string('D', 30001), new string('J', 84123), new string('J', 302414) };
            for (var i = 0; i < descriptions.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { airm.Id, new AnimeInfoEditingRequestModel(id: airm.Id, title: airm.Title, description: descriptions[i], isNsfw: airm.IsNsfw), ErrorCodes.TooLongProperty };
            }
        }

        private static IEnumerable<object[]> GetInvalidIdData()
        {
            var ids = new long[] { 0, -1, -10 };
            for (var i = 0; i < ids.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { ids[i], new AnimeInfoEditingRequestModel(id: ids[i], title: airm.Title, description: airm.Description, isNsfw: airm.IsNsfw), ErrorCodes.EmptyProperty };
            }
        }

        [TestInitialize]
        public void InitTests()
        {
            baseAnimeInfoData = new List<AnimeInfo>();
            baseAnimeInfoData.Add(new AnimeInfo { Id = 1, Title = "JoJo's Bizarre Adventure", IsNsfw = false });
            baseAnimeInfoData.Add(new AnimeInfo { Id = 2, Title = "Made in Abyss", Description = "A horrorific story about a girl, who lost his mom and goes to the Abyss to search for her. For her surprise, a robot boy accompanies her and they are looking for her mom together, while they discover terrific cretures of the abyss who tries to kill them and other people from expeditions. Well, those people are still not nice guys.", IsNsfw = true });
            baseAnimeInfoData.Add(new AnimeInfo { Id = 3, Title = "A Certain Specific Railgun", Description = "Misaka is not an average school girl, she is the Railgun. In this anime we know more and more about the girl and her friends while the story of the city and the image of the world they are living are building up continually.", IsNsfw = false });
            baseAnimeInfoData.Add(new AnimeInfo { Id = 7, Title = "A Certain Magical Index", Description = "The main character of the story meets with Index, a magician girl who was chased by some magician. As the story goes, the MC knows the girl's story and get familiar with the magician's world and what world he lives in. The story follows the MC guy and Index's adventures and sometimes their friends are get their showtime.", IsNsfw = false });
            baseAnimeInfoData.Add(new AnimeInfo { Id = 10, Title = "KonoSuba", Description = "The main character guy dies and he's teleported into a medieval village with a goddes named Aqua. Their main quest is to defeat the Demon King and sometimes they get into trouble while other folks join to their party.", IsNsfw = false });
        }

        [DataTestMethod,
            DynamicData(nameof(GetShouldWorkData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_ShouldWork(long animeInfoId, AnimeInfoEditingRequestModel requestModel)
        {
            var animeInfo = requestModel.ToAnimeInfo();
            animeInfo.Id = animeInfoId;
            AnimeInfo currentAnimeInfo = new AnimeInfo();
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(id => { currentAnimeInfo = baseAnimeInfoData.Single(ai => ai.Id == id); }).ReturnsAsync(() => currentAnimeInfo);
                animeInfoWriteRepo.Setup(aiw => aiw.UpdateAnimeInfo(It.IsAny<AnimeInfo>())).Callback<AnimeInfo>(ai =>
                {
                    currentAnimeInfo.Title = ai.Title;
                    currentAnimeInfo.Description = ai.Description;
                    currentAnimeInfo.IsNsfw = ai.IsNsfw;
                })
                .ReturnsAsync(() => currentAnimeInfo);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            var updatedAnimeInfo = await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            updatedAnimeInfo.Should().NotBeNull();
            updatedAnimeInfo.Should().BeEquivalentTo(animeInfo, options => options.ExcludingMissingMembers());
        }

        [DataTestMethod,
            DynamicData(nameof(GetMismatchingIdData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_UpdateMismatchingId_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeFunc = async () => await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            await editAnimeFunc.Should().ThrowAsync<MismatchingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingIdData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_NotExistingId_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel)
        {
            AnimeInfo currentAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(id => { currentAnimeInfo = baseAnimeInfoData.SingleOrDefault(ai => ai.Id == id); }).ReturnsAsync(() => currentAnimeInfo);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeFunc = async () => await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            await editAnimeFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }

        [DataTestMethod,
         DynamicData(nameof(GetInvalidIdData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_InvalidId_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel, ErrorCodes errCode)
        {
            var errors = CreateErrorList(errCode, nameof(AnimeInfoEditingRequestModel.Id));
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeFunc = async () => await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            var valEx = await editAnimeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_InvalidTitle_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel, ErrorCodes errCode)
        {
            var errors = CreateErrorList(errCode, nameof(AnimeInfoEditingRequestModel.Title));
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeFunc = async () => await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            var valEx = await editAnimeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetTooLongDescriptionData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_TooLongDescription_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel, ErrorCodes errCode)
        {
            var errors = CreateErrorList(errCode, nameof(AnimeInfoEditingRequestModel.Description));
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeFunc = async () => await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            var valEx = await editAnimeFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetShouldWorkData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeFunc = async () => await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            await editAnimeFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanTests()
        {
            baseAnimeInfoData.Clear();
            baseAnimeInfoData = null;
        }

        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
