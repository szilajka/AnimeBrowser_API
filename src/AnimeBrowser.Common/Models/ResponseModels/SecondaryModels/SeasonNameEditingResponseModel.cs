using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonNameEditingResponseModel : SeasonNameResponseModel
    {
        public SeasonNameEditingResponseModel(long id, string title, long seasonId)
            : base(id: id, title: title, seasonId: seasonId)
        {
        }
    }
}
