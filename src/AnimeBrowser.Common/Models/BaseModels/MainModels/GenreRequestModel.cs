using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class GenreRequestModel
    {
        public GenreRequestModel(string genreName = "", string description = "")
        {
            this.GenreName = genreName;
            this.Description = description;
        }

        public string GenreName { get; set; }
        public string Description { get; set; }
    }
}
