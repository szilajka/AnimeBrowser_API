using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    [ToJsonString]
    public partial class AnimeInfoNameCreationResponseModel : AnimeInfoNameResponseModel
    {
        public AnimeInfoNameCreationResponseModel(long id, long animeInfoId, string title = "")
            : base(id, animeInfoId, title)
        {
        }
    }
}
