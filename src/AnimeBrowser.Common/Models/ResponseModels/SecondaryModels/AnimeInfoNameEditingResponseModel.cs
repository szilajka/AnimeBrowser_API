using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    [ToJsonString]
    public partial class AnimeInfoNameEditingResponseModel : AnimeInfoNameResponseModel
    {
        public AnimeInfoNameEditingResponseModel(long id, long animeInfoId, string title = "")
            : base(id: id, animeInfoId: animeInfoId, title: title)
        {
        }
    }
}
