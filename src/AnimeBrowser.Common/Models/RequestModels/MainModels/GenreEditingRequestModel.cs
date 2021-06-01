using AnimeBrowser.Common.Models.BaseModels.MainModels;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public partial class GenreEditingRequestModel : GenreRequestModel
    {
        public GenreEditingRequestModel(long id, string genreName = "", string description = "")
            : base(genreName: genreName, description: description)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
