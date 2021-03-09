using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Data.Entities;
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
    public class AnimeInfoNameDeleteTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private IList<AnimeInfoName> allAnimeInfoNames;

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

        [DataTestMethod,
            DataRow(1), DataRow(5), DataRow(100), DataRow(200)]
        public async Task DeleteAnimeInfoName_ShouldWork(long animeInfoNameId)
        {
            AnimeInfoName foundAnimeInfoName = null;
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).Callback<long>(ainId => foundAnimeInfoName = allAnimeInfoNames.SingleOrDefault(ain => ain.Id == ainId)).ReturnsAsync(() => foundAnimeInfoName);
                animeInfoNameWriteRepo.Setup(ainw => ainw.DeleteAnimeInfoName(It.IsAny<AnimeInfoName>())).Callback<AnimeInfoName>(ain => allAnimeInfoNames.Remove(ain));

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameDelete, AnimeInfoNameDeleteHandler>();
            });

            var beforeCount = allAnimeInfoNames.Count;
            var animeInfoNameDeleteHandler = sp.GetService<IAnimeInfoNameDelete>();
            await animeInfoNameDeleteHandler!.DeleteAnimeInfoName(animeInfoNameId);
            allAnimeInfoNames.Count.Should().Be(beforeCount - 1);
            allAnimeInfoNames.Should().NotContain(foundAnimeInfoName);
        }

        [DataTestMethod,
            DataRow(0), DataRow(-5), DataRow(-100)]
        public async Task DeleteAnimeInfoName_InvalidId_ThrowException(long animeInfoNameId)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();
                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameDelete, AnimeInfoNameDeleteHandler>();
            });

            var animeInfoNameDeleteHandler = sp.GetService<IAnimeInfoNameDelete>();
            Func<Task> deleteAnimeInfoNameFunc = async () => await animeInfoNameDeleteHandler!.DeleteAnimeInfoName(animeInfoNameId);
            await deleteAnimeInfoNameFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DataRow(14), DataRow(35), DataRow(500), DataRow(341)]
        public async Task DeleteAnimeInfoName_NotExistingAnimeInfoId_ThrowException(long animeInfoNameId)
        {
            AnimeInfoName foundAnimeInfoName = null;
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).Callback<long>(ainId => foundAnimeInfoName = allAnimeInfoNames.SingleOrDefault(ain => ain.Id == ainId)).ReturnsAsync(() => foundAnimeInfoName);

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameDelete, AnimeInfoNameDeleteHandler>();
            });

            var animeInfoNameDeleteHandler = sp.GetService<IAnimeInfoNameDelete>();
            Func<Task> deleteAnimeInfoNameFunc = async () => await animeInfoNameDeleteHandler!.DeleteAnimeInfoName(animeInfoNameId);
            await deleteAnimeInfoNameFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfoName>>();
        }

        [DataTestMethod,
            DataRow(1), DataRow(5), DataRow(100), DataRow(200)]
        public async Task DeleteAnimeInfoName_ThrowException(long animeInfoNameId)
        {
            var sp = SetupDI(services =>
            {
                var animeInfoNameReadRepo = new Mock<IAnimeInfoNameRead>();
                var animeInfoNameWriteRepo = new Mock<IAnimeInfoNameWrite>();

                animeInfoNameReadRepo.Setup(ainr => ainr.GetAnimeInfoNameById(It.IsAny<long>())).ThrowsAsync(new InvalidOperationException());

                services.AddTransient(factory => animeInfoNameReadRepo.Object);
                services.AddTransient(factory => animeInfoNameWriteRepo.Object);
                services.AddTransient<IAnimeInfoNameDelete, AnimeInfoNameDeleteHandler>();
            });

            var animeInfoNameDeleteHandler = sp.GetService<IAnimeInfoNameDelete>();
            Func<Task> deleteAnimeInfoNameFunc = async () => await animeInfoNameDeleteHandler!.DeleteAnimeInfoName(animeInfoNameId);
            await deleteAnimeInfoNameFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanDb()
        {
            allAnimeInfoNames.Clear();
            allAnimeInfos.Clear();
            allAnimeInfos = null;
            allAnimeInfoNames = null;
        }
    }
}
