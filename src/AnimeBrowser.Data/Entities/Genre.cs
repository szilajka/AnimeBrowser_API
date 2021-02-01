using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Genre : Identifiable<long>
    {
        public Genre()
        {
            SeasonGenres = new HashSet<SeasonGenre>();
        }

        //public long Id { get; set; }
        [Attr]
        public string GenreName { get; set; }
        [Attr]
        public string Description { get; set; }

        [HasMany]
        public virtual ICollection<SeasonGenre> SeasonGenres { get; set; }
    }
}
