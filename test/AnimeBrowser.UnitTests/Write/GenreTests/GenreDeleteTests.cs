using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Services.Write.MainHandlers;
using AnimeBrowser.Common.Exceptions;
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
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Write.GenreTests
{
    [TestClass]
    public class GenreDeleteTests : TestBase
    {
        private IList<Genre> allGenres;

        [TestInitialize]
        public void InitDb()
        {
            allGenres = new List<Genre>
            {
                new Genre { Id = 1, GenreName = "Shonen", Description = "Demographic indicator for anime and manga aimed at boys. An obvious and common example of shōnen is \"fighting\" anime, where extremely powerful warriors duke it out amongst each other with various forms of martial arts and superpowers. Typical examples of this include Dragon Ball Z, Flame of Recca and Rurouni Kenshin.\r\n\r\nGiant Robots (known as \"Mecha\" among English-speaking fans) are also a very common form of shōnen. What began with Mazinger Z eventually evolved into Gundam and Macross. Even though they are huge hulking machines that should have limited mobility, giant robots often display a flexibility and range of motion comparable to humans. For this reason, it has been commented that giant robots are more an extension/extrapolation of the knight/samurai's armor than an anthropomorphization of tanks/war machines.\r\n\r\nDespite a great increase in sophistication through the years, may shōnen anime remains largely centered on the resolution of conflicts through combat. However, that is not to say this is the only thing. If nothing else, this is proven by the large quantity of romantic comedy \"harem\" anime where a large cast of attractive females are vying for the attention of the (indecisive) male protagonist, like Tenchi Muyo! and Love Hina. Other examples of non-fighting shōnen anime are the sports anime like Slam Dunk that are starting to make an impact in the West.\r\n\r\nBesides violence or combat, one characteristic of many shōnen anime is a fast-paced story where action and adrenalin are emphasized over plot. There are exceptions to this, like the romantic stories of I''s and Boys Be..." },
                new Genre { Id = 2, GenreName = "Seinen", Description = "Relatively uncommon in the west due to the emphasis on the male teen market, \"seinen\" is a demographic indicator for anime and manga aimed at a young adult male (college-aged) audience. As such, this kind of anime tends to be more sophisticated than shōnen anime. There are many of the same basic themes/subgenres as shōnen but they are more psychological, satirical, violent, sexual, etc. In other words they are intended for a more mature audience.\r\n\r\nThere is a stronger focus on plot and consequently less focus on action. Characters and their interactions are also often more developped than in shōnen anime. Because of this, seinen material is frequently misidentified as shōjo because it isn't very well known outside of Japan and because fans correctly recognize the character development, relationships and romance typical of shōjo. However, seinen anime usually deals with these subjects with a greater realism. Where shōjo will have an idealized love story, seinen will have more grays and uncertainties dealing with the practical give-and-take realities of a relationship.\r\n\r\nOverall, seinen anime tends to be more strongly rooted in reality, with many incidental details added to heighten the sense of realism and even fantasy elements being subject to a strong \"realistic\" logic. Of course, it should be noted that those stylistic guidelines are a generalization of the genre and even an anime that has none of those characteristics can be classified as seinen if that was indeed the target audience. In fact, hentai (not including yaoi) is mostly targeted at the seinen demographic.\r\n\r\nIt should be noted that many of the works of anime most acclaimed for their depth and maturity (such as Patlabor, Maison Ikkoku and Ghost in the Shell) hail from the seinen genre." },
                new Genre { Id = 3, GenreName = "Mecha", Description = "Short for \"mechanical,\" mecha has two different meanings in Japanese and English.\r\n\r\nIn Japanese, the term is used to refer to anything mechanical from robots and spacecraft to bicycles and toasters. The robots in Mobile Suit Gundam are mecha in Japanese, and so are the cars in You're Under Arrest.\r\n\r\nSome English-speaking fans have repurposed the term to only mean piloted or controlled robots. The variable fighters from Macross and mobile suits from Gundams are prominent examples of mecha in this redefined version of the term." },
                new Genre { Id = 8, GenreName = "Ecchi", Description = "Ecchi is derived from the Japanese pronunciation of the letter \"H\", the first letter in the word hentai (as spelled in romaji). Ecchi is another way to say \"perverted\" in Japanese, but its connotation is not as strong as hentai. Anime that contain a lot of sexual humor but no outright pornography are often referred to as ecchi, examples include Love Hina and to a lesser extent Ranma 1/2.\r\n\r\nChotto Ecchi (chotto meaning \"slightly\") refers to anime that has some light sexual themes." },
                new Genre { Id = 10, GenreName = "Slice of life", Description = "The slice of life category of story is a story that portrays a \"cut-out\" sequence of events in a character's life. It may or may not contain any plot progress and little character development, and often has no exposition, conflict, or dénouement, with an open ending. It usually tries to depict the everyday life of ordinary people, sometimes but rarely, with fantasy or science fiction elements involved. The term slice of life is actually a dead metaphor: it often seems as if the author had taken a knife and \"cut out\" a slice of the lives of some characters, without concern for narrative form. It is sometimes called tranche de vie, from the French.\r\n\r\nSlice of life stories appear in books, TV shows, movies and video games. Lucky Star and Minami-ke are two examples of \"slice of life\" anime. Slice of life stories may either be dramatic or otherwise presented in a very serious nature, or may be used to help frame a comedic setting. Sports may be present in \"slice of life\" in a thematic form in order to advance the plot of the story." },
                new Genre { Id = 31, GenreName = "Action", Description = "The action genre is a class of creative works characterized by more emphasis on exciting action sequences than on character development or story-telling. The genre encompasses action-adventure fiction, action films, action games and analogous media in other formats such as manga and anime. There are many sub-genres, including martial arts action, extreme sports action, car chases and vehicles, suspense action, and action comedy, with each focusing in more detail on its own type and flavour of action. It is usually possible to tell from the creative style of an action sequence, the genre of the entire creative works. For example, the style of a combat sequence will indicate whether the entire works is an action adventure, or martial arts." }
            };
        }

        [DataTestMethod,
            DataRow(1), DataRow(2), DataRow(31), DataRow(10)]
        public async Task DeleteGenre_ShouldWork(long genreId)
        {
            Genre foundGenre = null;
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                genreReadRepo.Setup(gr => gr.GetGenreById(It.IsAny<long>())).Callback<long>(gId => foundGenre = allGenres.SingleOrDefault(g => g.Id == gId)).ReturnsAsync(() => foundGenre);
                genreWriteRepo.Setup(gw => gw.DeleteGenre(It.IsAny<Genre>())).Callback<Genre>(g => allGenres.Remove(g));
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreDelete, GenreDeleteHandler>();
            });
            var beforeGenreCount = allGenres.Count;
            var genreDeleteHandler = sp.GetService<IGenreDelete>();
            await genreDeleteHandler.DeleteGenre(genreId);
            var afterGenreCount = allGenres.Count;

            beforeGenreCount.Should().Be(afterGenreCount + 1);
        }

        [DataTestMethod,
            DataRow(0), DataRow(-1), DataRow(-5), DataRow(-53)]
        public async Task DeleteGenre_NotExistingId_ThrowException(long genreId)
        {
            var sp = SetupDI(services =>
            {
                var genreReadRepo = new Mock<IGenreRead>();
                var genreWriteRepo = new Mock<IGenreWrite>();
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreDelete, GenreDeleteHandler>();
            });
            var genreDeleteHandler = sp.GetService<IGenreDelete>();
            Func<Task> deleteGenreFunc = async () => await genreDeleteHandler.DeleteGenre(genreId);

            await deleteGenreFunc.Should().ThrowAsync<NotExistingIdException>();
        }

        [DataTestMethod,
            DataRow(6), DataRow(13), DataRow(34), DataRow(231)]
        public async Task DeleteGenre_InvalidId_ThrowException(long genreId)
        {
            Genre foundGenre = null;
            var sp = SetupDI(services =>
            {
                var genreWriteRepo = new Mock<IGenreWrite>();
                var genreReadRepo = new Mock<IGenreRead>();
                genreReadRepo.Setup(gr => gr.GetGenreById(It.IsAny<long>())).Callback<long>(gId => foundGenre = allGenres.SingleOrDefault(g => g.Id == gId)).ReturnsAsync(() => foundGenre);
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreDelete, GenreDeleteHandler>();
            });
            var genreDeleteHandler = sp.GetService<IGenreDelete>();
            Func<Task> deleteGenreFunc = async () => await genreDeleteHandler.DeleteGenre(genreId);

            await deleteGenreFunc.Should().ThrowAsync<NotFoundObjectException<Genre>>();
        }

        [DataTestMethod,
            DataRow(1), DataRow(2), DataRow(3), DataRow(8)]
        public async Task DeleteGenre_ThrowException(long genreId)
        {
            Genre foundGenre = null;
            var sp = SetupDI(services =>
            {
                var genreWriteRepo = new Mock<IGenreWrite>();
                var genreReadRepo = new Mock<IGenreRead>();
                genreReadRepo.Setup(gr => gr.GetGenreById(It.IsAny<long>())).Callback<long>(gId => foundGenre = allGenres.SingleOrDefault(g => g.Id == gId)).ReturnsAsync(() => foundGenre);
                genreWriteRepo.Setup(gw => gw.DeleteGenre(It.IsAny<Genre>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient(factory => genreReadRepo.Object);
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreDelete, GenreDeleteHandler>();
            });
            var genreDeleteHandler = sp.GetService<IGenreDelete>();
            Func<Task> deleteGenreFunc = async () => await genreDeleteHandler.DeleteGenre(genreId);

            await deleteGenreFunc.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestCleanup]
        public void CleanDb()
        {
            allGenres.Clear();
            allGenres = null;
        }
    }
}
