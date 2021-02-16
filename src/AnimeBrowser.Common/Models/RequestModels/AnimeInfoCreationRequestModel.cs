using AnimeBrowser.Common.Models.BaseModels;

namespace AnimeBrowser.Common.Models.RequestModels
{
    public class AnimeInfoCreationRequestModel : AnimeInfoRequestModel
    {
        public AnimeInfoCreationRequestModel(string title = "", string description = "", bool isNsfw = false)
            : base(title: title, description: description, isNsfw: isNsfw)
        {
        }
    }
}
