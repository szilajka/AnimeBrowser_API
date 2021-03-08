using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.ResponseModels.MainModels
{
    public class GenreCreationResponseModel : GenreResponseModel
    {
        public GenreCreationResponseModel(long id, string genreName = "", string description = "")
            : base(id: id, genreName: genreName, description: description)
        {
        }
    }
}
