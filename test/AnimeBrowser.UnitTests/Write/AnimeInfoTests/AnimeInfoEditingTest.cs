using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.AnimeInfoTests
{
    [TestClass]
    public class AnimeInfoEditingTest : TestBase
    {
        private IList<AnimeInfo> baseAnimeInfoData;

        private static IEnumerable<object[]> GetTitleEditing_ShouldWork_Data()
        {
            yield return new object[] { 1, new AnimeInfoEditingRequestModel(id: 1, title: "J", isNsfw: false) };
            yield return new object[] { 1, new AnimeInfoEditingRequestModel(id: 1, title: new string('j', 100), isNsfw: false) };
            yield return new object[] { 2, new AnimeInfoEditingRequestModel(id: 2, title: "Made In ABYSS", description: "A horrorific story about a girl, who lost his mom and goes to the Abyss to search for her. For her surprise, a robot boy accompanies her and they are looking for her mom together, while they discover terrific cretures of the abyss who tries to kill them and other people from expeditions. Well, those people are still not nice guys.", isNsfw: true) };
            yield return new object[] { 2, new AnimeInfoEditingRequestModel(id: 2, title: new string('j', 254), isNsfw: false) };
            yield return new object[] { 3, new AnimeInfoEditingRequestModel(id: 3, title: "A certain specific railgun", description: "Misaka is not an average school girl, she is the Railgun. In this anime we know more and more about the girl and her friends while the story of the city and the image of the world they are living are building up continually.", isNsfw: false) };
            yield return new object[] { 7, new AnimeInfoEditingRequestModel(id: 7, title: "A certain magical Index", description: "The main character of the story meets with Index, a magician girl who was chased by some magician. As the story goes, the MC knows the girl's story and get familiar with the magician's world and what world he lives in. The story follows the MC guy and Index's adventures and sometimes their friends are get their showtime.", isNsfw: false) };
            yield return new object[] { 7, new AnimeInfoEditingRequestModel(id: 7, title: new string('j', 255), isNsfw: false) };
            yield return new object[] { 10, new AnimeInfoEditingRequestModel(id: 10, title: "konoSuba", description: "The main character guy dies and he's teleported into a medieval village with a goddes named Aqua. Their main quest is to defeat the Demon King and sometimes they get into trouble while other folks join to their party.", isNsfw: false) };
        }

        private static IEnumerable<object[]> GetDescriptionEditing_ShouldWork_Data()
        {
            yield return new object[] { 1, new AnimeInfoEditingRequestModel(id: 1, title: "JoJo's Bizarre Adventures", description: "Jonathan and his family", isNsfw: false) };
            yield return new object[] { 2, new AnimeInfoEditingRequestModel(id: 2, title: "Made in Abyss", description: null, isNsfw: true) };
            yield return new object[] { 2, new AnimeInfoEditingRequestModel(id: 2, title: "Made in Abyss", description: "", isNsfw: true) };
            yield return new object[] { 2, new AnimeInfoEditingRequestModel(id: 2, title: "Made in Abyss", description: "A horrorific story about a girl, and his robot friend. Not a nice story, not recommended to watch with children.", isNsfw: true) };
            yield return new object[] { 3, new AnimeInfoEditingRequestModel(id: 3, title: "A Certain Specific Railgun", description: "M", isNsfw: false) };
            yield return new object[] { 3, new AnimeInfoEditingRequestModel(id: 3, title: "A Certain Specific Railgun", description: new string('m', 500), isNsfw: false) };
            yield return new object[] { 3, new AnimeInfoEditingRequestModel(id: 3, title: "A Certain Specific Railgun", description: "Misaka is not an average school girl, she is the Railgun. This is a spinoff series to the Certain Magical Index.", isNsfw: false) };
            yield return new object[] { 7, new AnimeInfoEditingRequestModel(id: 7, title: "A Certain Magical Index", description: "The main character and Index are teaming up and fights against enemies.", isNsfw: false) };
            yield return new object[] { 7, new AnimeInfoEditingRequestModel(id: 7, title: "A Certain Magical Index", description: new string('K', 23145), isNsfw: false) };
            yield return new object[] { 7, new AnimeInfoEditingRequestModel(id: 7, title: "A Certain Magical Index", description: new string('K', 29999), isNsfw: false) };
            yield return new object[] { 10, new AnimeInfoEditingRequestModel(id: 10, title: "KonoSuba", description: "The main character guy and goddess Aqua goes to a medieval village and go on adventures with their group.", isNsfw: false) };
            yield return new object[] { 10, new AnimeInfoEditingRequestModel(id: 10, title: "KonoSuba", description: new string('K', 30000), isNsfw: false) };
        }

        private static IEnumerable<object[]> GetIsNsfwEditing_ShouldWork_Data()
        {
            yield return new object[] { 1, new AnimeInfoEditingRequestModel(id: 1, title: "JoJo's Bizarre Adventures", isNsfw: true) };
            yield return new object[] { 3, new AnimeInfoEditingRequestModel(id: 3, title: "A Certain Specific Railgun", description: "Misaka is not an average school girl, she is the Railgun. In this anime we know more and more about the girl and her friends while the story of the city and the image of the world they are living are building up continually.", isNsfw: false) };
            yield return new object[] { 10, new AnimeInfoEditingRequestModel(id: 10, title: "KonoSuba", description: "A horrorific story about a girl, who lost his mom and goes to the Abyss to search for her. For her surprise, a robot boy accompanies her and they are looking for her mom together, while they discover terrific cretures of the abyss who tries to kill them and other people from expeditions. Well, those people are still not nice guys.", isNsfw: true) };
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
            DynamicData(nameof(GetTitleEditing_ShouldWork_Data), DynamicDataSourceType.Method),
            DynamicData(nameof(GetDescriptionEditing_ShouldWork_Data), DynamicDataSourceType.Method),
            DynamicData(nameof(GetIsNsfwEditing_ShouldWork_Data), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_TitleEditing_ShouldWork(long animeInfoId, AnimeInfoEditingRequestModel requestModel)
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
                services.AddTransient<IAnimeInfoEditor, AnimeInfoEditorHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditor>();
            var updatedAnimeInfo = await animeInfoEditor.EditAnimeInfo(animeInfoId, requestModel);

            updatedAnimeInfo.Should().NotBeNull();
            updatedAnimeInfo.Should().BeEquivalentTo(animeInfo, options => options.ExcludingMissingMembers());
        }


        [TestCleanup]
        public void CleanTests()
        {
            baseAnimeInfoData = null;
        }

    }
}
