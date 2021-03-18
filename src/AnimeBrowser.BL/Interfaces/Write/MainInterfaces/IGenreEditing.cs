using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IGenreEditing
    {
        Task<GenreEditingResponseModel> EditGenre(long id, GenreEditingRequestModel genreRequestModel);
    }
}
