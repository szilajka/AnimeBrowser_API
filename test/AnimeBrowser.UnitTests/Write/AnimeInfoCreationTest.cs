using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write;
using AnimeBrowser.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write
{
    [TestClass]
    public class AnimeInfoCreationTest
    {
        private static IEnumerable<object[]> GetShouldWorkData()
        {
            var title = "JoJo's Bizarre Adventure";
            yield return new object[] { title, null, false };
            yield return new object[] { title, "The series tells about the Joestar family, starting with Jonathan Joestar and his adventures, involving Dio Brando, who is the first part's bad guy. Later on, the story tells about Joseph, Jotaro, Josuke and Giorno.", false };
            yield return new object[] { $"  {title}  ", "   The series tells about the Joestar family, starting with Jonathan Joestar and his adventures, involving Dio Brando, who is the first part's bad guy. Later on, the story tells about Joseph, Jotaro, Josuke and Giorno.     ", false };
            yield return new object[] { $"  {title} ", "       ", false };
        }


        [DataTestMethod,
        DynamicData(nameof(GetShouldWorkData), DynamicDataSourceType.Method)]
        public async Task CreateAnimeInfo_ShouldWork(string title, string description, bool isNsfw)
        {
            var requestModel = new AnimeInfoCreationRequestModel { Title = title, Description = description, IsNsfw = isNsfw };
            var animeInfo = new AnimeInfo { Title = title?.Trim(), Description = description?.Trim(), IsNsfw = isNsfw };
            var responseModel = new AnimeInfoCreationResponseModel { Id = 1, Title = title?.Trim(), Description = description?.Trim(), IsNsfw = isNsfw };

            var sp = StartupHelper.SetupDI((services) =>
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
    }
}
