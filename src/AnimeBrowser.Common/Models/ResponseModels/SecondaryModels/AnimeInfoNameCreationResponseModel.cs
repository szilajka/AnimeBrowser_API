using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class AnimeInfoNameCreationResponseModel : AnimeInfoNameResponseModel
    {
        public AnimeInfoNameCreationResponseModel(long id, long animeInfoId, string title = "")
            : base(id, animeInfoId, title)
        {
        }
    }
}
