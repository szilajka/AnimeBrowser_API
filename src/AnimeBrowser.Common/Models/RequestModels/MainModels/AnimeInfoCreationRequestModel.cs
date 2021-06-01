using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public partial class AnimeInfoCreationRequestModel : AnimeInfoRequestModel
    {
        public AnimeInfoCreationRequestModel(string title = "", string description = "", bool isNsfw = false, bool isActive = true)
            : base(title: title, description: description, isNsfw: isNsfw)
        {
            this.IsActive = isActive;
        }

        public bool IsActive { get; set; }
    }
}
