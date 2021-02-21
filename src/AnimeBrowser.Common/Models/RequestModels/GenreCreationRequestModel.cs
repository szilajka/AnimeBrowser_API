using AnimeBrowser.Common.Models.BaseModels;

namespace AnimeBrowser.Common.Models.RequestModels
{
    public class GenreCreationRequestModel : GenreRequestModel
    {
        public GenreCreationRequestModel(string genreName = "", string description = "")
            : base(genreName: genreName, description: description)
        {
        }
    }
}
