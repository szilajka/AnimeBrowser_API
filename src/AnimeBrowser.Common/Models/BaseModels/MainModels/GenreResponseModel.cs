using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class GenreResponseModel
    {
        public GenreResponseModel(long id, string genreName = "", string description = "")
        {
            this.Id = id;
            this.GenreName = genreName;
            this.Description = description;
            this.Description = description;
        }

        public long Id { get; set; }
        public string GenreName { get; set; }
        public string Description { get; set; }
    }
}
