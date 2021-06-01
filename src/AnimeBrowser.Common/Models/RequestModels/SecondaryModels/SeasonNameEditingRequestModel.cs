using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public partial class SeasonNameEditingRequestModel : SeasonNameRequestModel
    {
        public SeasonNameEditingRequestModel(long id, string title, long seasonId)
            : base(title: title, seasonId: seasonId)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }
}
