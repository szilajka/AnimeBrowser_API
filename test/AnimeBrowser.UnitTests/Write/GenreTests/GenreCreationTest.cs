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

namespace AnimeBrowser.UnitTests.Write.GenreTests
{
    [TestClass]
    public class GenreCreationTest : TestBase
    {

        private static IList<GenreCreationRequestModel> allRequestModels;
        private static IList<Genre> allGenres;

        [ClassInitialize]
        public static void InitRequests(TestContext context)
        {
            allRequestModels = new List<GenreCreationRequestModel>();

            var genreNames = new string[] {
                "Shonen",
                "Seinen",
                "Mecha",
                "Ecchi",
                "Slice of life",
                "Action",
                //----------------------------
                "G",
                new string('G', 35),
                new string('G', 70),
                new string('G', 99),
                new string('G', 100),
            };
            var descriptions = new string[] {
                "Demographic indicator for anime and manga aimed at boys. An obvious and common example of shōnen is \"fighting\" anime, where extremely powerful warriors duke it out amongst each other with various forms of martial arts and superpowers. Typical examples of this include Dragon Ball Z, Flame of Recca and Rurouni Kenshin.\r\n\r\nGiant Robots (known as \"Mecha\" among English-speaking fans) are also a very common form of shōnen. What began with Mazinger Z eventually evolved into Gundam and Macross. Even though they are huge hulking machines that should have limited mobility, giant robots often display a flexibility and range of motion comparable to humans. For this reason, it has been commented that giant robots are more an extension/extrapolation of the knight/samurai's armor than an anthropomorphization of tanks/war machines.\r\n\r\nDespite a great increase in sophistication through the years, may shōnen anime remains largely centered on the resolution of conflicts through combat. However, that is not to say this is the only thing. If nothing else, this is proven by the large quantity of romantic comedy \"harem\" anime where a large cast of attractive females are vying for the attention of the (indecisive) male protagonist, like Tenchi Muyo! and Love Hina. Other examples of non-fighting shōnen anime are the sports anime like Slam Dunk that are starting to make an impact in the West.\r\n\r\nBesides violence or combat, one characteristic of many shōnen anime is a fast-paced story where action and adrenalin are emphasized over plot. There are exceptions to this, like the romantic stories of I''s and Boys Be..." ,
                "Relatively uncommon in the west due to the emphasis on the male teen market, \"seinen\" is a demographic indicator for anime and manga aimed at a young adult male (college-aged) audience. As such, this kind of anime tends to be more sophisticated than shōnen anime. There are many of the same basic themes/subgenres as shōnen but they are more psychological, satirical, violent, sexual, etc. In other words they are intended for a more mature audience.\r\n\r\nThere is a stronger focus on plot and consequently less focus on action. Characters and their interactions are also often more developped than in shōnen anime. Because of this, seinen material is frequently misidentified as shōjo because it isn't very well known outside of Japan and because fans correctly recognize the character development, relationships and romance typical of shōjo. However, seinen anime usually deals with these subjects with a greater realism. Where shōjo will have an idealized love story, seinen will have more grays and uncertainties dealing with the practical give-and-take realities of a relationship.\r\n\r\nOverall, seinen anime tends to be more strongly rooted in reality, with many incidental details added to heighten the sense of realism and even fantasy elements being subject to a strong \"realistic\" logic. Of course, it should be noted that those stylistic guidelines are a generalization of the genre and even an anime that has none of those characteristics can be classified as seinen if that was indeed the target audience. In fact, hentai (not including yaoi) is mostly targeted at the seinen demographic.\r\n\r\nIt should be noted that many of the works of anime most acclaimed for their depth and maturity (such as Patlabor, Maison Ikkoku and Ghost in the Shell) hail from the seinen genre.",
                "Short for \"mechanical,\" mecha has two different meanings in Japanese and English.\r\n\r\nIn Japanese, the term is used to refer to anything mechanical from robots and spacecraft to bicycles and toasters. The robots in Mobile Suit Gundam are mecha in Japanese, and so are the cars in You're Under Arrest.\r\n\r\nSome English-speaking fans have repurposed the term to only mean piloted or controlled robots. The variable fighters from Macross and mobile suits from Gundams are prominent examples of mecha in this redefined version of the term.",
                "Ecchi is derived from the Japanese pronunciation of the letter \"H\", the first letter in the word hentai (as spelled in romaji). Ecchi is another way to say \"perverted\" in Japanese, but its connotation is not as strong as hentai. Anime that contain a lot of sexual humor but no outright pornography are often referred to as ecchi, examples include Love Hina and to a lesser extent Ranma 1/2.\r\n\r\nChotto Ecchi (chotto meaning \"slightly\") refers to anime that has some light sexual themes.",
                "The slice of life category of story is a story that portrays a \"cut-out\" sequence of events in a character's life. It may or may not contain any plot progress and little character development, and often has no exposition, conflict, or dénouement, with an open ending. It usually tries to depict the everyday life of ordinary people, sometimes but rarely, with fantasy or science fiction elements involved. The term slice of life is actually a dead metaphor: it often seems as if the author had taken a knife and \"cut out\" a slice of the lives of some characters, without concern for narrative form. It is sometimes called tranche de vie, from the French.\r\n\r\nSlice of life stories appear in books, TV shows, movies and video games. Lucky Star and Minami-ke are two examples of \"slice of life\" anime. Slice of life stories may either be dramatic or otherwise presented in a very serious nature, or may be used to help frame a comedic setting. Sports may be present in \"slice of life\" in a thematic form in order to advance the plot of the story.",
                "The action genre is a class of creative works characterized by more emphasis on exciting action sequences than on character development or story-telling. The genre encompasses action-adventure fiction, action films, action games and analogous media in other formats such as manga and anime. There are many sub-genres, including martial arts action, extreme sports action, car chases and vehicles, suspense action, and action comedy, with each focusing in more detail on its own type and flavour of action. It is usually possible to tell from the creative style of an action sequence, the genre of the entire creative works. For example, the style of a combat sequence will indicate whether the entire works is an action adventure, or martial arts.",
                //----------------------------
                "D",
                new string('D', 35),
                new string('D', 2535),
                new string('D', 9999),
                new string('D', 10000),
            };

            for (var i = 0; i < genreNames.Length; i++)
            {
                allRequestModels.Add(new GenreCreationRequestModel(genreName: genreNames[i], description: descriptions[i]));
            }
        }

        [TestInitialize]
        public void InitGenres()
        {
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
        }

        private static IEnumerable<object[]> GetCreateGenreData()
        {
            for (var i = 0; i < allRequestModels.Count; i++)
            {
                var grm = allRequestModels[i];
                yield return new object[] { new GenreCreationRequestModel(genreName: grm.GenreName, description: grm.Description) };
            }
        }

        private static IEnumerable<object[]> GetCreateGenre_InvalidGenreNameData()
        {
            var genreNames = new string[] { null, "", new string('G', 101), new string('G', 464) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            for (var i = 0; i < genreNames.Length; i++)
            {
                var grm = allRequestModels[i];
                yield return new object[] { new GenreCreationRequestModel(genreName: genreNames[i], description: grm.Description), errorCodes[i] };
            }
        }

        private static IEnumerable<object[]> GetCreateGenre_InvalidDescriptionData()
        {
            var descriptions = new string[] { null, "", new string('D', 10001), new string('D', 12330), new string('D', 999999) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            for (var i = 0; i < descriptions.Length; i++)
            {
                var grm = allRequestModels[i];
                yield return new object[] { new GenreCreationRequestModel(genreName: grm.GenreName, description: descriptions[i]), errorCodes[i] };
            }
        }

        private static IEnumerable<object[]> GetAlreadyExistingData()
        {
            var genreNames = new string[] { "Shoujo", "Horror", "Fantasy" };
            for (var i = 0; i < genreNames.Length; i++)
            {
                var grm = allRequestModels[i];
                yield return new object[] { new GenreCreationRequestModel(genreName: genreNames[i], description: grm.Description) };
            }
        }



        [DataTestMethod,
            DynamicData(nameof(GetCreateGenreData), DynamicDataSourceType.Method)]
        public async Task CreateGenre_ShouldWork(GenreCreationRequestModel requestModel)
        {
            var isAlreadyExists = false;
            Genre savedGenre = null;
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                genreWriteRepo.Setup(gwr => gwr.CreateGenre(It.IsAny<Genre>())).Callback<Genre>(g => { savedGenre = g; savedGenre.Id = 10; allGenres.Add(savedGenre); }).ReturnsAsync(() => savedGenre!);
                genreReadRepo.Setup(gr => gr.IsExistWithSameName(It.IsAny<string>()))
                    .Callback<string>(gn => isAlreadyExists = allGenres.Any(g => g.GenreName.Equals(gn, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => isAlreadyExists);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var responseModel = requestModel.ToGenre().ToCreationResponseModel();
            responseModel.Id = 10;
            var genreCreationHandler = sp.GetService<IGenreCreation>();
            var createdGenre = await genreCreationHandler.CreateGenre(requestModel);

            createdGenre.Should().NotBeNull();
            createdGenre.Should().BeEquivalentTo(responseModel);
            allGenres.Should().ContainEquivalentOf(savedGenre);
        }


        [DataTestMethod,
            DynamicData(nameof(GetCreateGenre_InvalidGenreNameData), DynamicDataSourceType.Method)]
        public async Task CreateGenre_InvalidGenreName_ThrowException(GenreCreationRequestModel requestModel, ErrorCodes errorCode)
        {
            var errors = CreateErrorList(errorCode, nameof(GenreCreationRequestModel.GenreName));
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            Func<Task> createGenreFunc = async () => await genreCreationHandler.CreateGenre(requestModel);

            var valEx = await createGenreFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(o => o.Description));
        }

        [DataTestMethod,
            DynamicData(nameof(GetCreateGenre_InvalidDescriptionData), DynamicDataSourceType.Method)]
        public async Task CreateGenre_InvalidDescription_ThrowException(GenreCreationRequestModel requestModel, ErrorCodes errorCode)
        {
            var errors = CreateErrorList(errorCode, nameof(GenreCreationRequestModel.Description));
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            Func<Task> createGenreFunc = async () => await genreCreationHandler.CreateGenre(requestModel);

            var valEx = await createGenreFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(o => o.Description));
        }


        [DataTestMethod,
           DynamicData(nameof(GetAlreadyExistingData), DynamicDataSourceType.Method)]
        public async Task CreateGenre_AlreadyExistingGenre_ThrowException(GenreCreationRequestModel requestModel)
        {
            var isAlreadyExists = false;
            Genre savedGenre = null;
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                genreWriteRepo.Setup(gwr => gwr.CreateGenre(It.IsAny<Genre>())).Callback<Genre>(g => { savedGenre = g; savedGenre.Id = 10; allGenres.Add(savedGenre); }).ReturnsAsync(() => savedGenre!);
                genreReadRepo.Setup(gr => gr.IsExistWithSameName(It.IsAny<string>()))
                    .Callback<string>(gn => isAlreadyExists = allGenres.Any(g => g.GenreName.Equals(gn, StringComparison.OrdinalIgnoreCase)))
                    .Returns(() => isAlreadyExists);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            Func<Task> createGenreFunc = async () => await genreCreationHandler.CreateGenre(requestModel);

            createGenreFunc.Should().ThrowAsync<AlreadyExistingObjectException<Genre>>();
        }

        [TestMethod]
        public async Task CreateGenre_NullObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            Func<Task> createGenreFunc = async () => await genreCreationHandler.CreateGenre(null);

            await createGenreFunc.Should().ThrowAsync<EmptyObjectException<GenreCreationRequestModel>>();
        }

        [TestMethod]
        public async Task CreateGenre_InvalidOperation_ThrowException()
        {
            var requestModel = new GenreCreationRequestModel("Shounen", "Main character usually fights.");
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                genreWriteRepo.Setup(gwr => gwr.CreateGenre(It.IsAny<Genre>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            Func<Task> createGenreFunc = async () => await genreCreationHandler.CreateGenre(requestModel);

            await createGenreFunc.Should().ThrowAsync<InvalidOperationException>();
        }


        [TestCleanup]
        public void CleanGenres()
        {
            allGenres.Clear();
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
