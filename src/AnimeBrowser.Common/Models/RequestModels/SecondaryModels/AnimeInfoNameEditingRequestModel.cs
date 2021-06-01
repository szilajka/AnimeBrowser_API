using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public partial class AnimeInfoNameEditingRequestModel : AnimeInfoNameRequestModel
    {
        public AnimeInfoNameEditingRequestModel(long id, long animeInfoId, string title = "")
            : base(animeInfoId: animeInfoId, title: title)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }
}
