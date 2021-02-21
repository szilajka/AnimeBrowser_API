using AnimeBrowser.Common.Models.BaseModels;

namespace AnimeBrowser.Common.Models.ResponseModels
{
    public class GenreEditingResponseModel : GenreResponseModel
    {
        public GenreEditingResponseModel(long id, string genreName = "", string description = "")
            : base(id: id, genreName: genreName, description: description)
        {
        }
    }
}
