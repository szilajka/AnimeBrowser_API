using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public partial class AnimeInfoEditingRequestModel : AnimeInfoRequestModel
    {
        public AnimeInfoEditingRequestModel(long id, string title = "", string description = "", bool isNsfw = false)
            : base(title: title, description: description, isNsfw: isNsfw)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
