using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write;
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

namespace AnimeBrowser.UnitTests.Write.GenreTests
{
    [TestClass]
    public class GenreCreationTest : TestBase
    {

        private static IEnumerable<object[]> GetCreateGenreData()
        {
            var genreNameLengths = new int[] { 1, 35, 70, 99, 100 };
            var descriptionLengths = new int[] { 1, 35, 2535, 9999, 10000 };
            foreach (var gnLength in genreNameLengths)
            {
                foreach (var dLength in descriptionLengths)
                {
                    yield return new object[] { new string('G', gnLength), new string('D', dLength) };
                }
            }
        }

        private static IEnumerable<object[]> GetCreateGenre_InvalidGenreNameData()
        {
            var genreNames = new string[] { null, "", new string('G', 101), new string('G', 464) };
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            var description = "Japanese term, literally \"rotten girl,\" referring to female fans of BL (Boy's Love). The term fujoshi is often used to refer to female otaku in general, however strictly speaking, a female otaku who is not a fan of BL should not be called a fujoshi.";
            for (var i = 0; i < genreNames.Length; i++)
            {
                yield return new object[] { genreNames[i], description, errorCodes[i] };
            }
        }

        private static IEnumerable<object[]> GetCreateGenre_InvalidDescriptionData()
        {
            var genreName = "Shounen";
            var errorCodes = new ErrorCodes[] { ErrorCodes.EmptyProperty, ErrorCodes.EmptyProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty, ErrorCodes.TooLongProperty };
            var descriptions = new string[] { null, "", new string('D', 10001), new string('D', 12330), new string('D', 999999) };
            for (var i = 0; i < descriptions.Length; i++)
            {
                yield return new object[] { genreName, descriptions[i], errorCodes[i] };
            }
        }

        [DataTestMethod,
            DynamicData(nameof(GetCreateGenreData), DynamicDataSourceType.Method)]
        public async Task CreateGenre_ShouldWork(string genreName, string description)
        {
            Genre savedGenre = null;
            Genre actualGenre = new Genre { Id = 1, GenreName = genreName, Description = description };
            var requestModel = new GenreCreationRequestModel(genreName, description);
            var sp = SetupDI(services =>
            {
                var genreWriteRepo = new Mock<IGenreWrite>();
                genreWriteRepo.Setup(gwr => gwr.CreateGenre(It.IsAny<Genre>())).Callback<Genre>(g => { savedGenre = g; savedGenre.Id = 1; }).ReturnsAsync(() => savedGenre);

                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            var createdGenre = await genreCreationHandler.CreateGenre(requestModel);

            createdGenre.Should().NotBeNull();
            createdGenre.Should().BeEquivalentTo(savedGenre, options => options.ExcludingMissingMembers());
            createdGenre.Should().BeEquivalentTo(actualGenre, options => options.ExcludingMissingMembers());
            savedGenre.Should().BeEquivalentTo(actualGenre, options => options.ExcludingMissingMembers());
        }


        [DataTestMethod,
            DynamicData(nameof(GetCreateGenre_InvalidGenreNameData), DynamicDataSourceType.Method)]
        public async Task CreateGenre_InvalidGenreName_ThrowException(string genreName, string description, ErrorCodes errorCode)
        {
            var errors = CreateErrorList(errorCode, nameof(GenreCreationRequestModel.GenreName));
            var requestModel = new GenreCreationRequestModel(genreName, description);
            var sp = SetupDI(services =>
            {
                var genreWriteRepo = new Mock<IGenreWrite>();
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
        public async Task CreateGenre_InvalidDescription_ThrowException(string genreName, string description, ErrorCodes errorCode)
        {
            var errors = CreateErrorList(errorCode, nameof(GenreCreationRequestModel.Description));
            var requestModel = new GenreCreationRequestModel(genreName, description);
            var sp = SetupDI(services =>
            {
                var genreWriteRepo = new Mock<IGenreWrite>();
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            Func<Task> createGenreFunc = async () => await genreCreationHandler.CreateGenre(requestModel);

            var valEx = await createGenreFunc.Should().ThrowAsync<ValidationException>();
            valEx.And.Errors.Should().BeEquivalentTo(errors, options => options.Excluding(o => o.Description));
        }

        [TestMethod]
        public async Task CreateGenre_NullObject_ThrowException()
        {
            var sp = SetupDI(services =>
            {
                var genreWriteRepo = new Mock<IGenreWrite>();
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
                var genreWriteRepo = new Mock<IGenreWrite>();
                genreWriteRepo.Setup(gwr => gwr.CreateGenre(It.IsAny<Genre>())).ThrowsAsync(new InvalidOperationException());
                services.AddTransient(factory => genreWriteRepo.Object);
                services.AddTransient<IGenreCreation, GenreCreationHandler>();
            });

            var genreCreationHandler = sp.GetService<IGenreCreation>();
            Func<Task> createGenreFunc = async () => await genreCreationHandler.CreateGenre(requestModel);

            await createGenreFunc.Should().ThrowAsync<InvalidOperationException>();
        }

    }
}
