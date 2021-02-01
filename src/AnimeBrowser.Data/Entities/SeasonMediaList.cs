#nullable disable

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonMediaList : Identifiable<long>
    {
        //public long Id { get; set; }
        public long ListId { get; set; }
        public long SeasonId { get; set; }

        [HasOne]
        public virtual MediaList List { get; set; }
        [HasOne]
        public virtual Season Season { get; set; }
    }
}
