#nullable disable

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeInfoName : Identifiable<long>
    {
        //public long Id { get; set; }
        [Attr]
        public string Title { get; set; }
        public long AnimeInfoId { get; set; }

        [HasOne]
        public virtual AnimeInfo AnimeInfo { get; set; }
    }
}
