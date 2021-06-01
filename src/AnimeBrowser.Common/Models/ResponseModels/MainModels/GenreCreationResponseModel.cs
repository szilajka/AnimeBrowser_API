using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.ResponseModels.MainModels
{
    [ToJsonString]
    public partial class GenreCreationResponseModel : GenreResponseModel
    {
        public GenreCreationResponseModel(long id, string genreName = "", string description = "")
            : base(id: id, genreName: genreName, description: description)
        {
        }
    }
}
