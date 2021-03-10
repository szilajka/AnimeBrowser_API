using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class SeasonNameCreationResponseModel : SeasonNameResponseModel
    {
        public SeasonNameCreationResponseModel(long id, string title, long seasonId)
            : base(id: id, title: title, seasonId: seasonId)
        {
        }
    }
}
