using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.Enums;
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
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.SeasonGenreTests
{
    [TestClass]
    public class SeasonGenreDeleteTests : TestBase
    {
        private IList<AnimeInfo> allAnimeInfos;
        private IList<Season> allSeasons;
        private IList<Genre> allGenres;
        private IList<SeasonGenre> allSeasonGenres;
        private static IList<IList<long>> allRequestModels;


        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<IList<long>>();

            var ids = new long[][] { new long[] { 1, 3, 4, 5, 5 }, new long[] { 3, 4, 5, 1, 2 }, new long[] { 1 }, new long[] { 3, 4, 1 }, new long[] { 3, 4, 142, 2 } };
            for (var i = 0; i < ids.Length; i++)
            {
                allRequestModels.Add(ids[i]);
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
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var sgrm = allRequestModels[i];
                yield return new object[] { sgrm.ToList() };
            }
        }

        private static IEnumerable<object[]> GetInvalidIdsData()
        {
            var ids = new long[][] { new long[] { 0, -1, -10, -99 }, new long[] { 1, -32, 5 } };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i].ToList() };
            }
        }

        private static IEnumerable<object[]> GetNotExistingSeasonGenresData()
        {
            var ids = new long[][] { new long[] { 213 }, new long[] { 15, 9, 33 } };
            for (var i = 0; i < ids.Length; i++)
            {
                yield return new object[] { ids[i].ToList() };
            }
        }


        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonGenres_ShouldWork(IList<long> seasonGenreIds)
        {
            IList<SeasonGenre> foundSeasonGenres = null;
            var sp = SetupDI(services =>
            {
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonGenreReadRepo.Setup(sgr => sgr.GetSeasonGenresByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(sgIds => foundSeasonGenres = allSeasonGenres.Where(sg => sgIds.Contains(sg.Id)).ToList())
                    .Returns(() => foundSeasonGenres!);
                seasonGenreWriteRepo.Setup(sgw => sgw.DeleteSeasonGenres(It.IsAny<IEnumerable<SeasonGenre>>()))
                    .Callback<IEnumerable<SeasonGenre>>(sgs => { foreach (var sg in sgs) { allSeasonGenres.Remove(sg); } });

                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreDelete, SeasonGenreDeleteHandler>();
            });

            var beforecount = allSeasonGenres.Count;
            var seasonGenreDeleteHandler = sp.GetService<ISeasonGenreDelete>();
            await seasonGenreDeleteHandler!.DeleteSeasonGenres(seasonGenreIds);
            allSeasonGenres.Count.Should().BeLessThan(beforecount);
            allSeasonGenres.Should().NotContain(foundSeasonGenres);
        }

        [DataTestMethod,
            DynamicData(nameof(GetInvalidIdsData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonGenres_InvalidIds_ThrowException(IList<long> seasonGenreIds)
        {
            var sp = SetupDI(services =>
            {
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreDelete, SeasonGenreDeleteHandler>();
            });

            var seasonGenreDeleteHandler = sp.GetService<ISeasonGenreDelete>();
            Func<Task> seasonGenreDeleteFunc = async () => await seasonGenreDeleteHandler!.DeleteSeasonGenres(seasonGenreIds);
            await seasonGenreDeleteFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [TestMethod]
        public async Task DeleteSeasonGenres_EmptyObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();
                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreDelete, SeasonGenreDeleteHandler>();
            });

            var seasonGenreDeleteHandler = sp.GetService<ISeasonGenreDelete>();
            Func<Task> seasonGenreDeleteFunc = async () => await seasonGenreDeleteHandler!.DeleteSeasonGenres(null);
            await seasonGenreDeleteFunc.Should().ThrowAsync<NotExistingIdException>();
        }


        [DataTestMethod,
            DynamicData(nameof(GetNotExistingSeasonGenresData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonGenres_NotExistingSeasonGenres_ThrowException(IList<long> seasonGenreIds)
        {
            IList<SeasonGenre> foundSeasonGenres = null;
            var sp = SetupDI(services =>
            {
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonGenreReadRepo.Setup(sgr => sgr.GetSeasonGenresByIds(It.IsAny<IEnumerable<long>>()))
                    .Callback<IEnumerable<long>>(sgIds => foundSeasonGenres = allSeasonGenres.Where(sg => sgIds.Contains(sg.Id)).ToList())
                    .Returns(() => foundSeasonGenres!);

                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreDelete, SeasonGenreDeleteHandler>();
            });

            var seasonGenreDeleteHandler = sp.GetService<ISeasonGenreDelete>();
            Func<Task> seasonGenreDeleteFunc = async () => await seasonGenreDeleteHandler!.DeleteSeasonGenres(seasonGenreIds);
            await seasonGenreDeleteFunc.Should().ThrowAsync<NotFoundObjectException<SeasonGenre>>();
        }

        [DataTestMethod,
            DynamicData(nameof(GetBasicData), DynamicDataSourceType.Method)]
        public async Task DeleteSeasonGenres_ThrowException(IList<long> seasonGenreIds)
        {
            var sp = SetupDI(services =>
            {
                var seasonGenreReadRepo = new Mock<ISeasonGenreRead>();
                var seasonGenreWriteRepo = new Mock<ISeasonGenreWrite>();

                seasonGenreReadRepo.Setup(sgr => sgr.GetSeasonGenresByIds(It.IsAny<IEnumerable<long>>())).Throws(new InvalidOperationException());

                services.AddTransient(factory => seasonGenreReadRepo.Object);
                services.AddTransient(factory => seasonGenreWriteRepo.Object);
                services.AddTransient<ISeasonGenreDelete, SeasonGenreDeleteHandler>();
            });

            var seasonGenreDeleteHandler = sp.GetService<ISeasonGenreDelete>();
            Func<Task> seasonGenreDeleteFunc = async () => await seasonGenreDeleteHandler!.DeleteSeasonGenres(seasonGenreIds);
            await seasonGenreDeleteFunc.Should().ThrowAsync<InvalidOperationException>();
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
