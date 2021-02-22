using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.DateTimeProviders;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Models.Enums;
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
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.SeasonTests
{
    [TestClass]
    public class SeasonCreationTests : TestBase
    {
        [ClassInitialize]
        public static void InitLogging(TestContext context)
        {
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .CreateLogger();
        }

        [AssemblyCleanup]
        public static void CleanLogging()
        {
            Log.Logger = null;
        }


        public IList<AnimeInfo> allAnimeInfos;

        [TestInitialize]
        public void InitDb()
        {
            allAnimeInfos = new List<AnimeInfo>
            {
                new AnimeInfo {Id = 1, Title = "JoJo's Bizarre Adventure", Description = string.Empty, IsNsfw = false },
                new AnimeInfo {Id = 2, Title = "Boku no Hero Academia", Description = string.Empty, IsNsfw = false },
                new AnimeInfo {Id = 10, Title = "Dorohedoro", Description = string.Empty, IsNsfw = true},
                new AnimeInfo {Id = 15, Title = "Shingeki no Kyojin", Description = string.Empty, IsNsfw = true },
                new AnimeInfo {Id = 201, Title = "Yakusoku no Neverland", Description = "Neverland...", IsNsfw = false }
            };
        }

        private static IEnumerable<object[]> GetBasicDatesData()
        {
            var seasonNumbers = new int[] { 1, 2, 100, 9999999, 1010 };
            var titles = new string[] { "T", new string('T', 100), new string('T', 254), new string('T', 255), "Title of something" };
            var descriptions = new string[] { null, "", "A", new string('D', 29999), new string('D', 30000) };
            var startDates = new DateTime?[] { null, new DateTime(1900, 1, 1), new DateTime(2020, 12, 22), DateTime.Now.AddYears(10), DateTime.Now.AddDays(876) };
            var endDates = new DateTime?[] { null, new DateTime(1900, 1, 1), new DateTime(2020, 12, 23), DateTime.Now.AddYears(10), DateTime.Now.AddDays(877) };
            var airStatuses = new AirStatusEnum[] { AirStatusEnum.UNKNOWN, AirStatusEnum.NotAired, AirStatusEnum.Airing, AirStatusEnum.Aired, AirStatusEnum.NotAired };
            var numOfEpisodes = new int?[] { null, 1, 123, 400, 675 };
            var coverCarousels = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var covers = new byte[]?[] { null, Encoding.UTF8.GetBytes("C"), Encoding.UTF8.GetBytes("ASD"), Encoding.UTF8.GetBytes("421ASD"), Encoding.UTF8.GetBytes("asdFDSF3412") };
            var animeInfoIds = new long[] { 1, 2, 10, 15, 201 };
            for (var i = 0; i < seasonNumbers.Length; i++)
            {
                yield return new object[] { new SeasonCreationRequestModel(
                    seasonNumber: seasonNumbers[i],
                    title: titles[i],
                    description: descriptions[i],
                    startDate: startDates[i],
                    endDate: endDates[i],
                    airStatus: airStatuses[i],
                    numberOfEpisodes: numOfEpisodes[i],
                    coverCarousel: coverCarousels[i],
                    cover: covers[i],
                    animeInfoId: animeInfoIds[i]) };
            }
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicDatesData), DynamicDataSourceType.Method)]
        public async Task CreateSeason_WithBasicDates_ShouldWork(SeasonCreationRequestModel requestModel)
        {
            AnimeInfo foundAnimeInfo = null;
            Season callbackSeason = null;
            var sp = SetupDI(services =>
            {
                var seasonWriteRepo = new Mock<ISeasonWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(aiId => foundAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == aiId)).ReturnsAsync(() => foundAnimeInfo);
                seasonWriteRepo.Setup(sw => sw.CreateSeason(It.IsAny<Season>())).Callback<Season>(s => { callbackSeason = s; callbackSeason.Id = 1; }).ReturnsAsync(() => callbackSeason);
                services.AddTransient<IDateTime, DateTimeProvider>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => seasonWriteRepo.Object);
                services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            });

            var responseModel = requestModel.ToSeason().ToCreationResponseModel();
            responseModel.Id = 1;
            var seasonCreationHandler = sp.GetService<ISeasonCreation>();
            var createdSeason = await seasonCreationHandler.CreateSeason(requestModel);

            createdSeason.Should().NotBeNull();
            createdSeason.Should().BeEquivalentTo(responseModel);
        }

        [TestCleanup]
        public void CleanDb()
        {
            allAnimeInfos.Clear();
            allAnimeInfos = null;
        }
    }
}
