using AnimeBrowser.Common.Models.BaseModels;

namespace AnimeBrowser.Common.Models.ResponseModels
{
    public class GenreCreationResponseModel : GenreResponseModel
    {
        public GenreCreationResponseModel(long id, string genreName = "", string description = "")
            : base(id: id, genreName: genreName, description: description)
        {
        }
    }
}
