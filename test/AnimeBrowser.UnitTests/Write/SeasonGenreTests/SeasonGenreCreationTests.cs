using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
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
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.SeasonGenreTests
{
    [TestClass]
    public class SeasonGenreCreationTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private IList<Season> allSeasons;
        private IList<Genre> allGenres;
        private IList<SeasonGenre> allSeasonGenres;
        private static IList<SeasonGenreCreationRequestModel> allRequestModels;


        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<SeasonGenreCreationRequestModel>();

            var genreIds = new long[] { 1, 3, 4, 5, 5,
                3, 4, 5, 1, 2 };
            var seasonIds = new long[] { 1, 1, 1, 1, 3,
                2, 3, 10, 20, 22 };
            for (var i = 0; i < genreIds.Length; i++)
            {
                allRequestModels.Add(new SeasonGenreCreationRequestModel(seasonId: seasonIds[i], genreId: genreIds[i]));
            }
        }


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

            allSeasons = new List<Season>
            {
                new Season{Id = 1, SeasonNumber = 1, Title = "Phantom Blood", Description = "In this season we know the story of Jonathan, Dio and Speedwagon, then Joseph and the Pillarmen's story",
                    StartDate = new DateTime(2012, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2012, 3, 5, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 2, SeasonNumber = 2, Title = "Stardust Crusaders", Description = "In this season we know the story of old Joseph and young Jotaro Kujo's story while they trying to get into Egypt.",
                    StartDate = new DateTime(2014, 3, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 7, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 24, AnimeInfoId = 1,
                    CoverCarousel = Encoding.UTF8.GetBytes("JoJoCarousel"), Cover = Encoding.UTF8.GetBytes("JoJoCover"),
                },
                new Season{Id = 3, SeasonNumber = 1, Title = "Season 1", Description = "I don't know this anime. Maybe they are just fighting. Who knows? I'm sure not.",
                    StartDate = new DateTime(2013, 1, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2014, 2, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 40, AnimeInfoId = 2,
                    CoverCarousel = Encoding.UTF8.GetBytes("BnHACarousel"), Cover = Encoding.UTF8.GetBytes("BnHACover"),
                },
                new Season{Id = 10, SeasonNumber = 1, Title = "Season 1", Description = "Kayman and Nikkaido's story. Kayman is a man, but has a lizard body, well, some magician did it to him, but who?",
                    StartDate = new DateTime(2020, 9, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2020, 12, 20, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Aired, NumberOfEpisodes = 10, AnimeInfoId = 10,
                    CoverCarousel = Encoding.UTF8.GetBytes("DorohedoroCarousel"), Cover = Encoding.UTF8.GetBytes("DorohedoroCover"),
                },
                new Season{Id = 20, SeasonNumber = 1, Title = "Season 4", Description = "I don't know this anime. Maybe they are just fighting. Who knows? I'm sure not.",
                    StartDate = new DateTime(2020, 11, 1, 0 ,0 ,0, DateTimeKind.Utc), EndDate = new DateTime(2021, 4, 10, 0 ,0 ,0, DateTimeKind.Utc),
                    AirStatus = (int)AirStatusEnum.Airing, NumberOfEpisodes = 24, AnimeInfoId = 15,
                    CoverCarousel = Encoding.UTF8.GetBytes("SnKCarousel"), Cover = Encoding.UTF8.GetBytes("SnKCover"),
                },
                new Season{Id = 22, SeasonNumber = 1, Title = "Season 2", Description = "I don't know this anime. But there will be a second season. That's for sure!",
                    StartDate = null, EndDate = null,
                    AirStatus = (int)AirStatusEnum.NotAired, NumberOfEpisodes = 20, AnimeInfoId = 201,
                    CoverCarousel = Encoding.UTF8.GetBytes("YnKCarousel"), Cover = Encoding.UTF8.GetBytes("YnKCover"),
                }
            };

            allGenres = new List<Genre>
            {
                new Genre
                {
                    Id = 1,
                    GenreName = "Shoujo",
                    Description = "Sailor Moon, Cardcaptor Sakura, Fruits Basket, Nana: They might range far and wide in terms of story, but all of these series are shoujo. Like other entries on this list, shoujo (literally \"young girl\") is technically a demographic, warped by time and export into a genre term. Accordingly, shoujo anime ranges far and wide in terms of subject matter. Sailor Moon falls under this umbrella, as do other magical girl stories of young heroines in fabulous costumes fighting evil with fantastic powers. Yet so does The Rose of Versailles, a historical drama set in Marie Antoinette's Versailles, The Heart of Thomas, a love story between two boys, and Ouran High School Host Club, an often-slapstick school comedy.\r\n\r\nWhat unites shoujo? An emphasis on relationships, for one thing, most obviously evidenced by shoujo's plethora of romance series. Yet this interest in what connects people goes beyond romantic love. Series like Tokyo Mew Mew and Nana might involve love triangles, headlining couples, and fandoms chock-full of shipping, but platonic relationships are just as big a part of their narratives, especially when it comes to friendships between young girls. Moreover, shoujo's visual motifs are strikingly different: though magical girls and shonen-based giant robot pilots might triumph over evil, only the former does it in bursts of hearts, stars, and flowers. There's beating the bad guy, and then there's beating the bad guy with gem-encrusted scepters that summon attacks like \"Starlight Honeymoon Therapy Kiss.\""
                },
                new Genre
                {
                    Id = 2,
                    GenreName = "Josei",
                    Description = "There's manga for girls, full of first crushes and school-bound secrets, and then there's josei: manga for women who want to go beyond the schoolyard.  Josei is where you'll find relationships examined beyond the initial kiss, lives after graduation, and a lot more action between the sheets. The distinction might appear to be murky: Josei anime like Paradise Kiss, Princess Jellyfish, and Kids on the Slope deal with breakups, makeups, and all the attendant drama, just as anime aimed at younger girls might do. But here, the starring couple really might end up being fully, finally incompatible, and the road to a better life might require brutal years of change, heartbreak, and disappointment. \r\n\r\nBut that's not to say that josei is an entirely melancholy affair — quite the opposite, in fact. Slice-of-life josei can find transcendent joy and laugh-out-loud humor in observing the rhythms of life in Ristorante Paradiso's bustling restaurant, or the world of classical music portrayed in Nodame Cantabile. Another prominent thread in josei is that of BL stories, or \"Boys' Love\": romances between male characters that can, within josei's more mature environs, be portrayed with far more explicit sexuality. Beyond josei anime lies the creatively fertile world of josei manga, much of it wildly experimental and unlike anything even seasoned anime and manga fans might be familiar with. Erica Sakurazawa, Moyocco Anno, and Kyoko Okazaki in particular are creators whose work is available in English and worth anyone's time."
                },
                new Genre
                {
                    Id = 3,
                    GenreName = "Fantasy",
                    Description = "The fantasy genre in anime primarily deals with fantasy worlds and surreal events and locations. Most of the time, the setting is in a magical world where the characters start an adventure. Sometimes they get sent there from the real world. Magic is oftentimes a component of this genre, and various mystical elements serve as the building blocks of the story. You’ll often know it’s a fantasy anime if the environment and atmosphere seems so dazzling and dreamlike that it’ll make you feel captivated and allured.The fantasy genre in anime primarily deals with fantasy worlds and surreal events and locations. Most of the time, the setting is in a magical world where the characters start an adventure. Sometimes they get sent there from the real world. Magic is oftentimes a component of this genre, and various mystical elements serve as the building blocks of the story. You’ll often know it’s a fantasy anime if the environment and atmosphere seems so dazzling and dreamlike that it’ll make you feel captivated and allured."
                },
                new Genre
                {
                    Id = 4,
                    GenreName = "Supernatural",
                    Description = "When one says supernatural, they’re referring to stuff or events that are odd and out-of-the-blue. For this category, supernatural might refer to something mythical, mystical, bizarre, or something outside the bounds of accepted reality. There’s a shadow of mystery often found in shows involved with this genre."
                },
                new Genre
                {
                    Id = 5,
                    GenreName = "Horror",
                    Description = "It’s not difficult to spot the horror genre in anime. Usually, if there are ghosts, monsters, gore, and creeps, then you’re likely watching a horror series. Heavy gore and bloody violence is a common trait. The most important factor for a show to be considered horror is its ability to scare and creep you out."
                }
            };

            allSeasonGenres = new List<SeasonGenre> {
                new SeasonGenre {Id = 1, SeasonId = 1, GenreId = 2 },
                new SeasonGenre {Id = 2, SeasonId = 2, GenreId = 1 },
                new SeasonGenre {Id = 3, SeasonId = 2, GenreId = 5 },
                new SeasonGenre {Id = 4, SeasonId = 3, GenreId = 2 },
                new SeasonGenre {Id = 5, SeasonId = 3, GenreId = 3 },
                new SeasonGenre {Id = 10, SeasonId = 10, GenreId = 1 },
                new SeasonGenre {Id = 11, SeasonId = 20, GenreId = 4 }
            };
        }

        private static IEnumerable<object[]> GetBasicData()
        {
            var indexes = new int[][] { new int[] { 0, 1, 2, 3 }, new int[] { 4 }, new int[] { 5, 6, 7, 8, 9 }, new int[] { 7 }, new int[] { 0, 2, 9, 7 } };
            for (var i = 0; i < indexes.Length; i++)
            {
                var index = indexes[i];
                List<SeasonGenreCreationRequestModel> rms = new();
                for (var j = 0; j < index.Length; j++)
                {
                    var srm = allRequestModels[index[j]];
                    var sgcrm = new SeasonGenreCreationRequestModel(genreId: srm.GenreId, seasonId: srm.SeasonId);
                    rms.Add(sgcrm);
                }
                yield return new object[] { rms };
            }
        }

        private static IEnumerable<object[]> GetBasicData_WithDuplicates()
        {
            List<SeasonGenreCreationRequestModel> rms = new();
            var seasonIds = new long[] { 1, 1, 2, 3 };
            var genreIds = new long[] { 3, 2, 5, 1 };
            for (var i = 0; i < genreIds.Length; i++)
            {
                var sgcrm = new SeasonGenreCreationRequestModel(genreId: genreIds[i], seasonId: seasonIds[i]);
                rms.Add(sgcrm);
            }
            yield return new object[] { rms };
        }

        private static IEnumerable<object[]> GetEmptyObjectData()
        {
            yield return new object[] { new List<SeasonGenreCreationRequestModel>() };
            yield return new object[] { null };
        }

        private static IEnumerable<object[]> GetNotExistingSeasonIdData()
        {
            List<SeasonGenreCreationRequestModel> rms = new();
            var seasonIds = new long[] { 0, -10, -110 };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                var srm = allRequestModels[i];
                var sgcrm = new SeasonGenreCreationRequestModel(genreId: srm.GenreId, seasonId: seasonIds[i]);
                rms.Add(sgcrm);
            }
            yield return new object[] { rms };
        }

        private static IEnumerable<object[]> GetNotExistingGenreIdData()
        {
            List<SeasonGenreCreationRequestModel> rms = new();
            var genreIds = new long[] { 0, -10, -99 };
            for (var i = 0; i < genreIds.Length; i++)
            {
                var srm = allRequestModels[i];
                var sgcrm = new SeasonGenreCreationRequestModel(genreId: genreIds[i], seasonId: srm.SeasonId);
                rms.Add(sgcrm);
            }
            yield return new object[] { rms };
        }

        private static IEnumerable<object[]> GetNotFoundSeasonData()
        {
            var seasonIds = new long[][] { new long[] { 300, 4567, 75678 }, new long[] { 1, 2, 5, 456 } };
            for (var i = 0; i < seasonIds.Length; i++)
            {
                List<SeasonGenreCreationRequestModel> rms = new();
                var srm = allRequestModels[i];
                for (var j = 0; j < seasonIds[i].Length; j++)
                {
                    var sgcrm = new SeasonGenreCreationRequestModel(genreId: srm.GenreId, seasonId: seasonIds[i][j]);
                    rms.Add(sgcrm);
                }
                yield return new object[] { rms };
            }

        }

        private static IEnumerable<object[]> GetNotFoundGenreData()
        {
            var genreIds = new long[][] { new long[] { 35, 99, 2434 }, new long[] { 1, 2, 5, 31, 32 } };
            for (var i = 0; i < genreIds.Length; i++)
            {
                List<SeasonGenreCreationRequestModel> rms = new();
                var srm = allRequestModels[i];
                for (var j = 0; j < genreIds[i].Length; j++)
                {
                    var sgcrm = new SeasonGenreCreationRequestModel(genreId: genreIds[i][j], seasonId: srm.SeasonId);
                    rms.Add(sgcrm);
                }
                yield return new object[] { rms };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonGenre_ShouldWork(IList<SeasonGenreCreationRequestModel> requestModels)
        {
            IList<Season> foundSeasons = null;
            IList<Genre> foundGenres = null;
            List<SeasonGenre> foundSeasonGenres = null;
            IList<SeasonGenre> createdSeasonGenres = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var genreReadRepo = new Mock<IGenreRead>();
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonsByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(sIds => foundSeasons = allSeasons.Where(s => sIds.Contains(s.Id)).ToList())
                    .Returns(() => foundSeasons!);
                genreReadRepo.Setup(gr => gr.GetGenresByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(gIds => foundGenres = allGenres.Where(g => gIds.Contains(g.Id)).ToList())
                    .Returns(() => foundGenres!);
                seasonGenreReadRepo.Setup(sgr => sgr.GetSeasonGenreBySeasonAndGenreIds(It.IsAny<IEnumerable<(long SeasonId, long GenreId)>>()))
                    .Callback<IEnumerable<(long SeasonId, long GenreId)>>(sgIds =>
                    {
                        foundSeasonGenres = new List<SeasonGenre>();
                        foreach (var sgId in sgIds)
                        {
                            var sgs = allSeasonGenres.Where(sg => sg.SeasonId == sgId.SeasonId && sg.GenreId == sgId.GenreId);
                            foundSeasonGenres.AddRange(sgs);
                        }
                    })
                    .Returns(() => foundSeasonGenres!);
                seasonGenreWriteRepo.Setup(sgw => sgw.CreateSeasonGenres(It.IsAny<IEnumerable<SeasonGenre>>()))
                    .Callback<IEnumerable<SeasonGenre>>(sgs =>
                    {
                        createdSeasonGenres = new List<SeasonGenre>();
                        var counter = 100;
                        foreach (var sg in sgs) { sg.Id = counter++; allSeasonGenres.Add(sg); createdSeasonGenres.Add(sg); }
                    })
                    .ReturnsAsync(() => createdSeasonGenres!);


                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            });

            var beforeCount = allSeasonGenres.Count;
            var seasonGenres = requestModels.ToSeasonGenres();
            foreach (var sg in seasonGenres) { sg.Id = 100; }
            var responseModels = seasonGenres.ToCreationResponseModel().ToList();
            var seasonGenreCreationHandler = sp.GetService<ISeasonGenreCreation>();
            var seasonGenresResponseModel = await seasonGenreCreationHandler!.CreateSeasonGenres(requestModels);
            seasonGenresResponseModel.Should().NotBeEmpty();
            seasonGenresResponseModel.Should().BeEquivalentTo(responseModels, options => options.Excluding(x => x.Id));
            allSeasonGenres.Count.Should().BeGreaterThan(beforeCount);
        }

        [DataTestMethod,
          DynamicData(nameof(GetBasicData_WithDuplicates), DynamicDataSourceType.Method)]
        public async Task CreateSeasonGenre_ShouldWork_WithDuplicates(IList<SeasonGenreCreationRequestModel> requestModels)
        {
            IList<Season> foundSeasons = null;
            IList<Genre> foundGenres = null;
            List<SeasonGenre> foundSeasonGenres = null;
            IList<SeasonGenre> createdSeasonGenres = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var genreReadRepo = new Mock<IGenreRead>();
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonsByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(sIds => foundSeasons = allSeasons.Where(s => sIds.Contains(s.Id)).ToList())
                    .Returns(() => foundSeasons!);
                genreReadRepo.Setup(gr => gr.GetGenresByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(gIds => foundGenres = allGenres.Where(g => gIds.Contains(g.Id)).ToList())
                    .Returns(() => foundGenres!);
                seasonGenreReadRepo.Setup(sgr => sgr.GetSeasonGenreBySeasonAndGenreIds(It.IsAny<IEnumerable<(long SeasonId, long GenreId)>>()))
                    .Callback<IEnumerable<(long SeasonId, long GenreId)>>(sgIds =>
                    {
                        foundSeasonGenres = new List<SeasonGenre>();
                        foreach (var sgId in sgIds)
                        {
                            var sgs = allSeasonGenres.Where(sg => sg.SeasonId == sgId.SeasonId && sg.GenreId == sgId.GenreId);
                            foundSeasonGenres.AddRange(sgs);
                        }
                    })
                    .Returns(() => foundSeasonGenres!);
                seasonGenreWriteRepo.Setup(sgw => sgw.CreateSeasonGenres(It.IsAny<IEnumerable<SeasonGenre>>()))
                    .Callback<IEnumerable<SeasonGenre>>(sgs =>
                    {
                        createdSeasonGenres = new List<SeasonGenre>();
                        var counter = 100;
                        foreach (var sg in sgs) { sg.Id = counter++; allSeasonGenres.Add(sg); createdSeasonGenres.Add(sg); }
                    })
                    .ReturnsAsync(() => createdSeasonGenres!);


                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            });

            var beforeCount = allSeasonGenres.Count;
            var seasonGenreCreationHandler = sp.GetService<ISeasonGenreCreation>();
            var seasonGenresResponseModel = await seasonGenreCreationHandler!.CreateSeasonGenres(requestModels);
            allSeasonGenres.Count.Should().BeGreaterThan(beforeCount);
        }

        [DataTestMethod,
            DynamicData(nameof(GetEmptyObjectData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonGenre_EmptyObject_ThrowException(IList<SeasonGenreCreationRequestModel> requestModels)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var genreReadRepo = new Mock<IGenreRead>();
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            });

            var seasonGenreCreationHandler = sp.GetService<ISeasonGenreCreation>();
            Func<Task> seasonGenreCreationFunc = async () => await seasonGenreCreationHandler!.CreateSeasonGenres(requestModels);

            await seasonGenreCreationFunc.Should().ThrowAsync<EmptyObjectException<SeasonGenreCreationRequestModel>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotExistingSeasonIdData), DynamicDataSourceType.Method),
            DynamicData(nameof(GetNotExistingGenreIdData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonGenre_NotExistingIds_ThrowException(IList<SeasonGenreCreationRequestModel> requestModels)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var genreReadRepo = new Mock<IGenreRead>();
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();
                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            });

            var seasonGenreCreationHandler = sp.GetService<ISeasonGenreCreation>();
            Func<Task> seasonGenreCreationFunc = async () => await seasonGenreCreationHandler!.CreateSeasonGenres(requestModels);

            await seasonGenreCreationFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetNotFoundSeasonData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonGenre_NotFoundSeason_ThrowException(IList<SeasonGenreCreationRequestModel> requestModels)
        {
            IList<Season> foundSeasons = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var genreReadRepo = new Mock<IGenreRead>();
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonsByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(sIds => foundSeasons = allSeasons.Where(s => sIds.Contains(s.Id)).ToList())
                    .Returns(() => foundSeasons!);

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            });

            var seasonGenreCreationHandler = sp.GetService<ISeasonGenreCreation>();
            Func<Task> seasonGenreCreationFunc = async () => await seasonGenreCreationHandler!.CreateSeasonGenres(requestModels);

            await seasonGenreCreationFunc.Should().ThrowAsync<NotFoundObjectException<Season>>();
        }

        [DataTestMethod,
           DynamicData(nameof(GetNotFoundGenreData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonGenre_NotFoundGenre_ThrowException(IList<SeasonGenreCreationRequestModel> requestModels)
        {
            IList<Season> foundSeasons = null;
            IList<Genre> foundGenres = null;
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var genreReadRepo = new Mock<IGenreRead>();
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonsByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(sIds => foundSeasons = allSeasons.Where(s => sIds.Contains(s.Id)).ToList())
                    .Returns(() => foundSeasons!);
                genreReadRepo.Setup(gr => gr.GetGenresByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(gIds => foundGenres = allGenres.Where(g => gIds.Contains(g.Id)).ToList())
                    .Returns(() => foundGenres!);


                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            });

            var seasonGenreCreationHandler = sp.GetService<ISeasonGenreCreation>();
            Func<Task> seasonGenreCreationFunc = async () => await seasonGenreCreationHandler!.CreateSeasonGenres(requestModels);

            await seasonGenreCreationFunc.Should().ThrowAsync<NotFoundObjectException<Genre>>();
        }

        [DataTestMethod,
          DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task CreateSeasonGenre_ThrowException(IList<SeasonGenreCreationRequestModel> requestModels)
        {
            var sp = SetupDI(services =>
            {
                var seasonReadRepo = new Mock<ISeasonRead>();
                var genreReadRepo = new Mock<IGenreRead>();
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonReadRepo.Setup(sr => sr.GetSeasonsByIds(It.IsAny<IEnumerable<long>>())).Throws(new InvalidOperationException());

                services.AddTransient(factory => seasonReadRepo.Object);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            });

            var seasonGenreCreationHandler = sp.GetService<ISeasonGenreCreation>();
            Func<Task> seasonGenreCreationFunc = async () => await seasonGenreCreationHandler!.CreateSeasonGenres(requestModels);

            await seasonGenreCreationFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanDb()
        {
            allGenres.Clear();
            allSeasons.Clear();
            allAnimeInfos.Clear();
            allAnimeInfos = null;
            allSeasons = null;
            allGenres = null;
        }

        [ClassCleanup]
        public static void CleanRequests()
        {
            allRequestModels.Clear();
            allRequestModels = null;
        }
    }
}
