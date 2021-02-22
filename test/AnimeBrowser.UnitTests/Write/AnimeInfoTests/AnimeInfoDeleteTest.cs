using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
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
    public class AnimeInfoDeleteTest : TestBase
    {

        private IList<AnimeInfo> baseAnimeInfoData;

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

        private static IEnumerable<object[]> GetDeleteData()
        {
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
            yield return new object[] { 7 };
        }

        private static IEnumerable<object[]> GetDelete_InvalidId_Data()
        {
            yield return new object[] { -1 };
            yield return new object[] { -10 };
            yield return new object[] { 0 };
            yield return new object[] { null };
        }

        private static IEnumerable<object[]> GetDelete_NotExistingId_Data()
        {
            yield return new object[] { 251 };
            yield return new object[] { 1034235234 };
            yield return new object[] { 4 };
            yield return new object[] { 11 };
        }

        [DataTestMethod,
            DynamicData(nameof(GetDeleteData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoDelete_ShouldWork(long animeInfoId)
        {
            AnimeInfo foundAnimeInfo = null;
            var beforeAnimeInfoCount = baseAnimeInfoData.Count;
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aid => foundAnimeInfo = baseAnimeInfoData.SingleOrDefault(ai => ai.Id == aid)).ReturnsAsync(() => foundAnimeInfo);
                animeInfoWriteRepo.Setup(aiw => aiw.DeleteAnimeInfo(It.IsAny<AnimeInfo>())).Callback<AnimeInfo>(ai => baseAnimeInfoData.Remove(ai));
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            });

            var animeInfoDeleteHandler = sp.GetService<IAnimeInfoDelete>();
            await animeInfoDeleteHandler.DeleteAnimeInfo(animeInfoId);

            var afterAnimeInfoCount = baseAnimeInfoData.Count;
            afterAnimeInfoCount.Should().Be(beforeAnimeInfoCount - 1);
            foundAnimeInfo.Should().NotBeNull();
            baseAnimeInfoData.Should().NotContain(foundAnimeInfo);
        }


        [DataTestMethod,
            DynamicData(nameof(GetDelete_InvalidId_Data), DynamicDataSourceType.Method)]
        public async Task AnimeInfoDelete_InvalidId_ThrowException(long animeInfoId)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            });

            var animeInfoDeleteHandler = sp.GetService<IAnimeInfoDelete>();
            Func<Task> deleteAnimeInfoFunc = async () => await animeInfoDeleteHandler.DeleteAnimeInfo(animeInfoId);
            await deleteAnimeInfoFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetDelete_NotExistingId_Data), DynamicDataSourceType.Method)]
        public async Task AnimeInfoDelete_NotExistingId_ThrowException(long animeInfoId)
        {
            AnimeInfo foundAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aid => foundAnimeInfo = baseAnimeInfoData.SingleOrDefault(ai => ai.Id == aid)).ReturnsAsync(() => foundAnimeInfo);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            });

            var animeInfoDeleteHandler = sp.GetService<IAnimeInfoDelete>();
            Func<Task> deleteAnimeInfoFunc = async () => await animeInfoDeleteHandler.DeleteAnimeInfo(animeInfoId);
            await deleteAnimeInfoFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }


        [TestCleanup]
        public void CleanTests()
        {
            baseAnimeInfoData = null;
        }
    }
}
