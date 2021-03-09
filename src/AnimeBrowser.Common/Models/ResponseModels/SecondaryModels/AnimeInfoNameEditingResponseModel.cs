using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class AnimeInfoNameEditingResponseModel : AnimeInfoNameResponseModel
    {
        public AnimeInfoNameEditingResponseModel(long id, long animeInfoId, string title = "")
            : base(id: id, animeInfoId: animeInfoId, title: title)
        {
        }
    }
}
