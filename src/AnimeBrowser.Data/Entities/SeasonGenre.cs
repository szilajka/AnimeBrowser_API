#nullable disable

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonGenre : Identifiable<long>
    {
        //public long Id { get; set; }
        public long GenreId { get; set; }
        public long SeasonId { get; set; }

        [HasOne]
        public virtual Genre Genre { get; set; }
        [HasOne]
        public virtual Season Season { get; set; }
    }
}
