using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public class AnimeInfoNameCreationRequestModel : AnimeInfoNameRequestModel
    {
        public AnimeInfoNameCreationRequestModel(long animeInfoId, string title = "")
            : base(animeInfoId, title)
        {
        }
    }
}
