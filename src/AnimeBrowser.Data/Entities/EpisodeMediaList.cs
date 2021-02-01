#nullable disable

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace AnimeBrowser.Data.Entities
{
    public partial class EpisodeMediaList : Identifiable<long>
    {
        //public long Id { get; set; }
        public long ListId { get; set; }
        public long EpisodeId { get; set; }

        [HasOne]
        public virtual Episode Episode { get; set; }
        [HasOne]
        public virtual MediaList List { get; set; }
    }
}
