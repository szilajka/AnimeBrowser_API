using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public class AnimeInfoCreationRequestModel : AnimeInfoRequestModel
    {
        public AnimeInfoCreationRequestModel(string title = "", string description = "", bool isNsfw = false)
            : base(title: title, description: description, isNsfw: isNsfw)
        {
        }
    }
}
