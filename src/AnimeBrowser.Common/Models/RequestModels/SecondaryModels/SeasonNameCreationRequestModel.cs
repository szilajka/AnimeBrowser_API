using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public partial class SeasonNameCreationRequestModel : SeasonNameRequestModel
    {
        public SeasonNameCreationRequestModel(string title, long seasonId)
            : base(title: title, seasonId: seasonId)
        {
        }
    }
}
