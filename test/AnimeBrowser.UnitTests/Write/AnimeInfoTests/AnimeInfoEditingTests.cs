using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Services.Write.MainHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Data.Converters.MainConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using AnimeBrowser.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.AnimeInfoTests
{
    [TestClass]
    public class AnimeInfoEditingTests : TestBase
    {
        private IList<Episode> allEpisodes;
        private IList<Season> allSeasons;
        private IList<AnimeInfo> allAnimeInfos;
        private static IList<AnimeInfoEditingRequestModel> allRequestModels;

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

        [TestInitialize]
        public void InitDb()
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

            allAnimeInfos = new List<AnimeInfo>
            {
                new AnimeInfo
                {
                    Id = 1, Title = "JoJo's Bizarre Adventure", IsNsfw = false, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 2, Title = "Made in Abyss",
                    Description = "A horrorific story about a girl, who lost his mom and goes to the Abyss to search for her. For her surprise, a robot boy accompanies her and they are looking for her mom together, while they discover terrific cretures of the abyss who tries to kill them and other people from expeditions. Well, those people are still not nice guys.",
                    IsNsfw = true, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 3, Title = "A Certain Specific Railgun",
                    Description = "Misaka is not an average school girl, she is the Railgun. In this anime we know more and more about the girl and her friends while the story of the city and the image of the world they are living are building up continually.",
                    IsNsfw = false, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 7, Title = "A Certain Magical Index",
                    Description = "The main character of the story meets with Index, a magician girl who was chased by some magician. As the story goes, the MC knows the girl's story and get familiar with the magician's world and what world he lives in. The story follows the MC guy and Index's adventures and sometimes their friends are get their showtime.",
                    IsNsfw = false, IsActive = true
                },
                new AnimeInfo
                {
                    Id = 10, Title = "KonoSuba",
                    Description = "The main character guy dies and he's teleported into a medieval village with a goddes named Aqua. Their main quest is to defeat the Demon King and sometimes they get into trouble while other folks join to their party.",
                    IsNsfw = false, IsActive = true
                }
            };
            allSeasons = new List<Season>
            {
                 new Season{ Id = 1, SeasonNumber = 1, Title = "Phantom Blood", Description = "In this season we know the story of Jonathan, Dio and Speedwagon, then Joseph and the Pillarmen's story",
                    StartDate = new DateTime(2012, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2012, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 2, SeasonNumber = 1, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 5401, SeasonNumber = 1, Title = "The Pillarmen's revenge", Description = "In this season the pillarmen are taking revenge for their death.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{ Id = 5405, SeasonNumber = 1, Title = "Into the Abyss", Description = "MC and her robot friend goes into the Abyss in hope to find her mother.",
                    StartDate = today.AddYears(-10).AddMonths(-3), EndDate = today.AddYears(-9),
                    AirStatus = (int)AirStatuses.Aired, NumberOfEpisodes = 24, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("MadeInAbyss Carousel"), Cover = Encoding.UTF8.GetBytes("MadeInAbyss Cover"),
                },
                  new Season{ Id = 6001, SeasonNumber = 1, Title = "Season 1", Description = "Main characters are not so clever...",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatuses.NotAired, NumberOfEpisodes = 10, AnimeInfoId = 10,
                    CoverCarousel = Encoding.UTF8.GetBytes("KonoSuba Carousel"), Cover = Encoding.UTF8.GetBytes("KonoSuba Cover"),
                }
            };
            allEpisodes = new List<Episode> {
                new Episode { Id = 1, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Prologue", Description = "This episode tells the backstory of Jonathan and Dio and their fights",
                    AirDate =  new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep1Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 2, EpisodeNumber = 2, AirStatus = (int)AirStatuses.Aired, Title = "Beginning of something new", Description = "More fighting for the family.",
                    AirDate =  new DateTime(2012, 1, 8, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S1Ep2Cover"), SeasonId = 1, AnimeInfoId = 1},
                new Episode { Id = 3, EpisodeNumber = 1, AirStatus = (int)AirStatuses.Aired, Title = "Family relations", Description = "Jotaro is in prison and we will know who is Jotaro and the old man.",
                    AirDate =  new DateTime(2014, 3, 1, 0, 0, 0, DateTimeKind.Utc), Cover = Encoding.UTF8.GetBytes("S2Ep1Cover"), SeasonId = 2, AnimeInfoId = 1},
                new Episode { Id = 4, EpisodeNumber = 5, AirStatus = (int)AirStatuses.NotAired, Title = "Intro to the Other world", Description = "No one knows what it's like...",
                    AirDate =  null, Cover = Encoding.UTF8.GetBytes("S1Ep5Cover"), SeasonId = 1, AnimeInfoId = 10}
            };
        }


        private static IEnumerable<object[]> GetBasicData()
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
            string propertyName = nameof(AnimeInfoEditingRequestModel.Title);
            for (var i = 0; i < titles.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { airm.Id, new AnimeInfoEditingRequestModel(id: airm.Id, title: titles[i], description: airm.Description, isNsfw: airm.IsNsfw), errorCodes[i], propertyName };
            }
        }

        public static IEnumerable<object[]> GetInvalidDescriptionData()
        {
            var descriptions = new string[] { new string('D', 30001), new string('J', 84123), new string('J', 302414) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            string propertyName = nameof(AnimeInfoEditingRequestModel.Description);
            for (var i = 0; i < descriptions.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { airm.Id, new AnimeInfoEditingRequestModel(id: airm.Id, title: airm.Title, description: descriptions[i], isNsfw: airm.IsNsfw), errorCodes[i], propertyName };
            }
        }

        private static IEnumerable<object[]> GetInvalidIdData()
        {
            var ids = new long[] { 0, -1, -10 };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty };
            string propertyName = nameof(AnimeInfoEditingRequestModel.Id);
            for (var i = 0; i < ids.Length; i++)
            {
                var airm = allRequestModels[i];
                yield return new object[] { ids[i], new AnimeInfoEditingRequestModel(id: ids[i], title: airm.Title, description: airm.Description, isNsfw: airm.IsNsfw), errorCodes[i], propertyName };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_ShouldWork(long animeInfoId, AnimeInfoEditingRequestModel requestModel)
        {
            var animeInfo = requestModel.ToAnimeInfo();
            animeInfo.Id = animeInfoId;
            AnimeInfo currentAnimeInfo = null;
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(id => { currentAnimeInfo = allAnimeInfos.Single(ai => ai.Id == id); }).ReturnsAsync(() => currentAnimeInfo);
                animeInfoWriteRepo.Setup(aiw => aiw.UpdateAnimeInfo(It.IsAny<AnimeInfo>())).Callback<AnimeInfo>(ai =>
                {
                    allAnimeInfos.Remove(currentAnimeInfo);
                    currentAnimeInfo!.Title = ai.Title;
                    currentAnimeInfo.Description = ai.Description;
                    currentAnimeInfo.IsNsfw = ai.IsNsfw;
                    allAnimeInfos.Add(currentAnimeInfo);

                })
                .ReturnsAsync(() => currentAnimeInfo!);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            var updatedAnimeInfo = await animeInfoEditor!.EditAnimeInfo(animeInfoId, requestModel);

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
            Func<Task> editAnimeInfoFunc = async () => await animeInfoEditor!.EditAnimeInfo(animeInfoId, requestModel);

            await editAnimeInfoFunc.Should().ThrowAsync<MismatchingIdException>();
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
                animeInfoReadRepo.Setup(air => air.GetAnimeInfoById(It.IsAny<long>())).Callback<long>(id => { currentAnimeInfo = allAnimeInfos.SingleOrDefault(ai => ai.Id == id); }).ReturnsAsync(() => currentAnimeInfo);
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeInfoFunc = async () => await animeInfoEditor!.EditAnimeInfo(animeInfoId, requestModel);

            await editAnimeInfoFunc.Should().ThrowAsync<NotFoundObjectException<AnimeInfo>>();
        }

        [DataTestMethod,
         DynamicData(nameof(GetInvalidIdData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_InvalidId_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeInfoFunc = async () => await animeInfoEditor!.EditAnimeInfo(animeInfoId, requestModel);

            var valEx = await editAnimeInfoFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidTitleData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_InvalidTitle_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeInfoFunc = async () => await animeInfoEditor!.EditAnimeInfo(animeInfoId, requestModel);

            var valEx = await editAnimeInfoFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidDescriptionData), DynamicDataSourceType.Method)]
        public async Task AnimeInfoEditing_TooLongDescription_ThrowException(long animeInfoId, AnimeInfoEditingRequestModel requestModel, ErrorCodes errorCode, string propertyName)
        {
            var errors = CreateErrorList(errorCode, propertyName);
            var sp = SetupDI(services =>
            {
                var animeInfoWriteRepo = new Mock<IAnimeInfoWrite>();
                var animeInfoReadRepo = new Mock<IAnimeInfoRead>();
                services.AddTransient(factory => animeInfoReadRepo.Object);
                services.AddTransient(factory => animeInfoWriteRepo.Object);
                services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            });

            var animeInfoEditor = sp.GetService<IAnimeInfoEditing>();
            Func<Task> editAnimeInfoFunc = async () => await animeInfoEditor!.EditAnimeInfo(animeInfoId, requestModel);

            var valEx = await editAnimeInfoFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(e => e.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
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
            Func<Task> editAnimeInfoFunc = async () => await animeInfoEditor!.EditAnimeInfo(animeInfoId, requestModel);

            await editAnimeInfoFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanDb()
        {
            allEpisodes.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allEpisodes = null;
            allSeasons = null;
            allAnimeInfos = null;
        }

        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
