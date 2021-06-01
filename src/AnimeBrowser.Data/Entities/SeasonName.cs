using AnimeBrowser.Common.Attributes;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    [ToJsonString]
    public partial class SeasonName
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long SeasonId { get; set; }

        public virtual Season Season { get; set; }
    }
}
