using AnimeBrowser.Common.Models.BaseModels;

namespace AnimeBrowser.Common.Models.ResponseModels
{
    public class AnimeInfoEditingResponseModel : AnimeInfoResponseModel
    {
        public AnimeInfoEditingResponseModel(long id, string title = "", string description = "", bool isNsfw = false)
            : base(id: id, title: title, description: description, isNsfw: isNsfw)
        {
        }
    }
}
